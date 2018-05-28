using System;
using System.Collections.Generic;
using System.Linq;
using FrontEndAPI.Mail;
using FrontEndAPI.Models.API.RequestObjects;
using FrontEndAPI.Models.API.ResponseObjects;
using FrontEndAPI.Models.Database.Repository.ActivityRepo;
using FrontEndAPI.Models.Database.Repository.ReservationRepo;
using FrontEndAPI.Models.Database.Repository.UserRepo;
using FrontEndAPI.Models.Entities;
using FrontEndAPI.Utility;
using FrontEndAPI.XML;
using Microsoft.AspNetCore.Mvc;
using static FrontEndAPI.Models.Entities.User;

namespace FrontEndAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IUserRepository _usrRepo;
        private readonly IActivityRepository _actRepo;
        private readonly IReservationRepository _resRepo;
        private readonly IEmailService _emailService;

        public UserController(IUserRepository usrRepo,IActivityRepository actRepo,IReservationRepository resRepo,IEmailService emailService)
        {
            _usrRepo = usrRepo;
            _actRepo = actRepo;
            _resRepo = resRepo;
            _emailService = emailService;
        }

        // GET: api/user
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserResponseObject>))]
        public IActionResult Get()
        {
            ISet<User> users = _usrRepo.GetAll();
            return Ok(_usrRepo.GetAll().Select(u => u.ToResponseObject()));
        }

        // GET: api/user/5
        [HttpGet("{id}", Name = "GetUser")]
        [ProducesResponseType(200, Type = typeof(UserResponseObject))]
        [ProducesResponseType(404)]
        public IActionResult Get(long id)
        {
            User u = _usrRepo.Get(id);
            if (u == null)
            {
                return NotFound("No user with the given id found");
            }
            UserResponseObject responseObj = u.ToResponseObject();
            return Ok(responseObj);
        }

        // POST: api/user
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(UserResponseObject))]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody]UserCreateObject createObj,bool fromMessage = false)
        {
            if (createObj == null)
            {
                return BadRequest();
            }
            //check for unique email
            if (createObj.Email != null)
            {
                User u = _usrRepo.GetByEmail(createObj.Email);
                if (u != null)
                {
                    ModelState.AddModelError("email", "Email already in use");
                }
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //INSERT new user
            var userAndOriginalPassword = Models.Entities.User.FromCreateObject(createObj);
            var user = userAndOriginalPassword.Item2;
            user = _usrRepo.Create(user,fromMessage);

            //send registration confirmation email
            _emailService.SendRegistrationConfirmation(userAndOriginalPassword);
            
            return CreatedAtRoute("GetUser", new { id = user.Id }, user.ToResponseObject());
        }

        // PUT: api/user/5
        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(UserResponseObject))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Put(long id, [FromBody]UserUpdateObject updateObj,bool fromMessage = false)
        {
            if(updateObj == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = _usrRepo.Get(id);

            if(user == null)
            {
                return NotFound();
            }

            user.Firstname = updateObj.Firstname;
            user.Lastname = updateObj.Lastname;
            if(updateObj.Password != null)
            {
                HashedPassword hashedPass = PasswordGenerator.EncryptPassword(updateObj.Password);
                user.Password = hashedPass.HashedPwd;
                user.Salt = hashedPass.Salt;
            }
            user.Street = updateObj.Street;
            user.Number = updateObj.Number;
            user.Bus = updateObj.Bus ?? user.Bus;
            user.ZipCode = updateObj.ZipCode;
            user.City = updateObj.City;
            user.Roles = new HashSet<UserRole>() { UserRole.Visitor}; 
            Models.Entities.User.AddRoles(updateObj.Roles,user.Roles);

            _usrRepo.Update(user,fromMessage);

            return Ok(user.ToResponseObject());
        }

        // DELETE: api/user/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public IActionResult Delete(long id,bool fromMessage = false)
        {
            var usr = _usrRepo.Get(id);
            if (usr == null)
            {
                return NotFound("No user with the given id found");
            }
            //var acts = _actRepo.GetBySpeakerId((long)usr.Id);
            //var ress = _resRepo.GetByVisitorId((long)usr.Id);

            this._usrRepo.Delete(usr,fromMessage);

            return new NoContentResult();
        }
    }
}
