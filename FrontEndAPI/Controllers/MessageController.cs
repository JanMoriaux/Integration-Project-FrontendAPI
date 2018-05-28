using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontEndAPI.Configuration;
using FrontEndAPI.Models.API.RequestObjects;
using FrontEndAPI.Models.Database.Repository.ReservationRepo;
using FrontEndAPI.Models.Database.Repository.UserRepo;
using FrontEndAPI.Models.Entities;
using FrontEndAPI.RabbitMQ.Consumer;
using FrontEndAPI.Utility;
using FrontEndAPI.XML;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using static FrontEndAPI.Models.Entities.User;

namespace FrontEndAPI.Controllers
{
    //this controller exposes no endpoints,
    //but listens to messages entering the system
    //throught the ConsumerService (IHostedService implementation)
    [Route("api/message")]
    public class MessageController : Controller
    {
        private readonly IXMLService _xmlService;
        private readonly ISystemConfiguration _systemConfig;
        private readonly UserController _userController;
        private readonly ReservationController _reservationController;
        private readonly IUserRepository _usrRepo;
        private readonly IReservationRepository _resRepo;

        public MessageController(IXMLService xmlService, ISystemConfiguration systemConfig, UserController userController, ReservationController reservationController, IUserRepository usrRepo, IReservationRepository resRepo)
        {
            _xmlService = xmlService;
            _systemConfig = systemConfig;
            _userController = userController;
            _reservationController = reservationController;
            _usrRepo = usrRepo;
            _resRepo = resRepo;
        }

        [HttpPost]
        public IActionResult PostXMLMessage([FromBody]MessageRequestObject requestObject)
        {
            if (requestObject == null || !ModelState.IsValid)
                return Ok();

            string message = requestObject.Message;

            //log the message as an error when it fails xsd validation
            if (!_xmlService.ValidateXML(message))
            {
                MessageLogger.LogErroneousMessage(message, "Failed XSD validation");
                return Ok();
            }
            //log the messages as rejected when the senderUUID is not supported by the system
            //frontend only accepts messages from the CashRegister system
            if (_xmlService.GetSenderUUIDFromXML(message) != _systemConfig.CashRegister)
            {
                MessageLogger.LogRejectedMessage(message, "Sender UUID not supported");
                return Ok();
            }

            //log the messsage as rejected when message type is not supported by this system
            //frontend only accepts Reservation and User messagetypes
            var messageType = _xmlService.GetMessageTypeFromXML(message);
            if (messageType == null ||
                (messageType != XMLService.MessageType.Reservation && messageType != XMLService.MessageType.User))
            {
                MessageLogger.LogRejectedMessage(message, "Message type not supported");
                return Ok();
            }

            //Get an MQEntity from the message for further processing
            //Log as error when this fails
            var entity = _xmlService.GenerateEntityFromMessage(message);
            if (entity == null)
            {
                MessageLogger.LogErroneousMessage(message, "Could not construct an entity from message");
                return Ok();
            }

            ProcessUser(entity as User);
            ProcessReservation(entity as Reservation);
            return Ok();
        }
        private void ProcessUser(User u)
        {
            if (u != null)
            {
                //perform INSERT if no local id present
                if (u.Id == null)
                {
                    CreateUser(u);
                }
                else
                {
                    //check the version with known version and proceed if newer
                    var known = _usrRepo.Get((long)u.Id);
                    if (known.Version < u.Version)
                    {
                        //delete if active flag changed
                        if (known.IsActive && !u.IsActive) DeleteUser(u);
                        //else update
                        else UpdateUser(u);
                    }
                }
            }
        }

        private void ProcessReservation(Reservation r)
        {
            if (r != null)
            {
                //perform INSERT if no local id present
                if (r.Id == null)
                {
                    CreateReservation(r);
                }
                else
                {
                    //check the version with known version and proceed if newer
                    var known = _resRepo.Get((long)r.Id);
                    if (known.Version < r.Version)
                    {
                        //delete if active flag changed
                        if (known.IsActive && !r.IsActive) DeleteReservation(r);
                        //else update
                        else UpdateReservation(r);
                    }
                }
            }
        }

        private void CreateUser(User u)
        {
            var roles = u.Roles.Select(r => ((UserRole)r).ToString()).ToArray<string>();

            var createObj = new UserCreateObject()
            {
                Firstname = u.Firstname,
                Lastname = u.Lastname,
                Email = u.Email,
                Street = u.Street,
                Number = u.Number,
                Bus = u.Bus,
                ZipCode = u.ZipCode,
                City = u.City,
                Roles = roles
            };
            _userController.Post(createObj, true);
        }

        private void DeleteUser(User u)
        {
            _userController.Delete((long)u.Id, true);
        }

        private void UpdateUser(User u)
        {
            var roles = u.Roles.Select(r => ((UserRole)r).ToString()).ToArray<string>();

            var updateObj = new UserUpdateObject()
            {
                Firstname = u.Firstname,
                Lastname = u.Lastname,
                Street = u.Street,
                Number = u.Number,
                Bus = u.Bus,
                ZipCode = u.ZipCode,
                City = u.City,
                Roles = roles
            };

            _userController.Put((long)u.Id, updateObj, true);
        }

        private void CreateReservation(Reservation r)
        {

            var createObj = new ReservationCreateObject()
            {
                ActivityId = r.ActivityId,
                VisitorId = r.VisitorId,
                WithInvoice = r.WithInvoice
            };
            _reservationController.Post(createObj, true);
        }

        private void DeleteReservation(Reservation r)
        {
            _reservationController.Delete((long)r.Id, true);
        }

        private void UpdateReservation(Reservation r)
        {
            var updateObj = new ReservationUpdateObject()
            {
                WithInvoice = r.WithInvoice,
                PayedFee = r.PayedFee,
                HasAttended = r.HasAttended
            };
            _reservationController.Put((long)r.Id, updateObj, true);
        }
    }
}