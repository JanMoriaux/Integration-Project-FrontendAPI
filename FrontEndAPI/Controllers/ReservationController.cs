using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontEndAPI.Mail;
using FrontEndAPI.Models.API.RequestObjects;
using FrontEndAPI.Models.API.ResponseObjects;
using FrontEndAPI.Models.Database.Repository.ActivityRepo;
using FrontEndAPI.Models.Database.Repository.EventRepo;
using FrontEndAPI.Models.Database.Repository.ReservationRepo;
using FrontEndAPI.Models.Database.Repository.UserRepo;
using FrontEndAPI.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrontEndAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/reservation")]
    public class ReservationController : Controller
    {
        private readonly IReservationRepository _resRepo;
        private readonly IActivityRepository _actRepo;
        private readonly IUserRepository _usrRepo;
        private readonly IEventRepository _evtRepo;
        private readonly IEmailService _emailService;

        public ReservationController(IReservationRepository resRepo,IActivityRepository actRepo, IUserRepository usrRepo,IEventRepository evtRepo, IEmailService emailService)
        {
            _resRepo = resRepo;
            _actRepo = actRepo;
            _usrRepo = usrRepo;
            _evtRepo = evtRepo;
            _emailService = emailService;
        }

        //GET api/activity
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReservationResponseObject>))]
        public IActionResult Get()
        {
            return Ok(_resRepo.GetAll().Select(r => r.ToResponseObject()));
        }

        //GET api/reservation/activity/5
        [HttpGet]
        [Route("~/api/reservation/activity/{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReservationResponseObject>))]
        public IActionResult GetByEventId(long id)
        {
            var reservs = _resRepo.GetByActivityId(id);
            return Ok(reservs.Select(r => r.ToResponseObject()));
        }

        //GET api/reservation/visitor/5
        [HttpGet]
        [Route("~/api/reservation/visitor/{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReservationResponseObject>))]
        public IActionResult GetByVisitorId(long id)
        {
            var reservs = _resRepo.GetByVisitorId(id);
            return Ok(reservs.Select(r => r.ToResponseObject()));
        }

        // GET: api/reservation/5
        [HttpGet("{id}", Name = "GetReservation")]
        [ProducesResponseType(200, Type = typeof(ReservationResponseObject))]
        [ProducesResponseType(404)]
        public IActionResult GetById(long id)
        {
            Reservation res= _resRepo.Get(id);

            if (res == null)
            {
                return NotFound("No activity with the given id found");
            }
            return Ok(res.ToResponseObject());
        }

        //// POST: api/reservation
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ReservationResponseObject))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Post([FromBody] ReservationCreateObject createObj,bool fromMessage = false)
        {
            if (createObj == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //check foreign key constraints
            var allFound = true;
            var a = _actRepo.Get(createObj.ActivityId);
            if (a == null) allFound = false;
            var u = _usrRepo.Get(createObj.VisitorId);
            if (a == null) allFound = false;
            if (!allFound)
            {
                return NotFound("Could not find the referenced Activity or Visitor for this Reservation");
            }

            var r = _resRepo.GetByActivityIdAndVisitorId(createObj.ActivityId, createObj.VisitorId);
            if(r != null)
            {
                return BadRequest("This user already made a reservation for this activity");
            }

            //INSERT the Reservation 
            var createReservation = Reservation.FromCreateObjectTuple(new Tuple<ReservationCreateObject, string, string>(createObj, a.UUID, u.UUID));
            createReservation = _resRepo.Create(createReservation,fromMessage);

            //TODO send reservation confirmation email
            var e = _evtRepo.Get(a.EventId);
            _emailService.SendReservationConfirmation(new Tuple<User, Activity, Event, bool>(u, a, e, createReservation.WithInvoice)); 
           
            //TODO send XML Reservation and Activity messages (with updated remaining seats)

            return CreatedAtRoute("GetReservation", new { id = createReservation.Id }, createReservation.ToResponseObject());
        }

        //PUT: api/reservation/5
        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(ReservationResponseObject))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Put(long id, [FromBody]ReservationUpdateObject updateObj, bool fromMessage = false)
        {
            if (updateObj == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Reservation r = _resRepo.Get(id);

            if (r == null)
            {
                return NotFound();
            }

            r.PayedFee = updateObj.PayedFee;
            r.HasAttended = updateObj.HasAttended;
            r.WithInvoice = updateObj.WithInvoice;

            _resRepo.Update(r, fromMessage);

            return Ok(r.ToResponseObject());
        }

        // DELETE: api/reservation/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public IActionResult Delete(long id,bool fromMessage = false)
        {
            //check ID
            var res = this._resRepo.Get(id);
            if (res == null)
            {
                return NotFound("No reservation with the given id found");
            }
            //DELETE (soft) the reservation
            this._resRepo.Delete(res,fromMessage);

            //TODO send XML Reservation message

            return new NoContentResult();
        }
    }
}