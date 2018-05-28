using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontEndAPI.Models.API.RequestObjects;
using FrontEndAPI.Models.API.ResponseObjects;
using FrontEndAPI.Models.Database.Repository.ActivityRepo;
using FrontEndAPI.Models.Database.Repository.EventRepo;
using FrontEndAPI.Models.Database.Repository.UserRepo;
using FrontEndAPI.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrontEndAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/activity")]
    public class ActivityController : Controller
    {
        private readonly IActivityRepository _actRepo;
        private readonly IEventRepository _evtRepo;
        private readonly IUserRepository _usrRepo;

        public ActivityController(IActivityRepository actRepo, IEventRepository evtRepo,IUserRepository usrRepo)
        {
            _actRepo = actRepo;
            _evtRepo = evtRepo;
            _usrRepo = usrRepo;
        }

        //GET api/activity
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ActivityResponseObject>))]
        public IActionResult Get()
        {
            return Ok(_actRepo.GetAll().Select(a => a.ToResponseObject()));
        }

        //GET api/activity/event/5
        [HttpGet]
        [Route("~/api/activity/event/{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ActivityResponseObject>))]
        public IActionResult GetByEventId(long id)
        {
            var acts = _actRepo.GetByEventId(id);
            return Ok(acts.Select(a => a.ToResponseObject() ));
        }

        //GET api/activity/user/5
        [HttpGet]
        [Route("~/api/activity/user/{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ActivityResponseObject>))]
        public IActionResult GetBySpeakerId(long id)
        {
            var acts = _actRepo.GetBySpeakerId(id);
            return Ok(acts.Select(a => a.ToResponseObject()));
        }

        // GET: api/activity/5
        [HttpGet("{id}", Name = "GetActivity")]
        [ProducesResponseType(200, Type = typeof(EventResponseObject))]
        [ProducesResponseType(404)]
        public IActionResult GetById(long id)
        {
            Activity act = _actRepo.Get(id);

            if (act == null)
            {
                return NotFound("No activity with the given id found");
            }
            return Ok(act.ToResponseObject());
        }

        //// POST: api/activity
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ActivityResponseObject))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Post([FromBody] ActivityCreateObject createObj)
        {
            if (createObj == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //check for unique combination of name and EventUUID
            Event e = _evtRepo.Get(createObj.EventId);
            if (e == null)
            {
                return NotFound("No event found for the given event id");
            }
            Activity a = _actRepo.GetByNameAndEventUUID(createObj.Name, e.UUID);
            if (a != null)
            {
                return BadRequest("There already exists an activity for the event with that name");
            }
            //check if speaker Id is valid when given
            if (createObj.SpeakerId != null)
            {
                var spkr = _usrRepo.Get((long)createObj.SpeakerId);
                if (spkr == null || !spkr.Roles.Contains(Models.Entities.User.UserRole.Speaker))
                {
                    return NotFound("No speaker found with the given ID");
                }
            }
            //INSERT the Activity
            Activity createActivity = Activity.FromCreateObjectTuple(new Tuple<ActivityCreateObject, string>(createObj, e.UUID));
            createActivity = _actRepo.Create(createActivity);

            return CreatedAtRoute("GetActivity", new { id = createActivity.Id }, createActivity.ToResponseObject());
        }

        // PUT: api/activity/5
        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(ActivityResponseObject))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Put(int id, [FromBody]ActivityUpdateObject updateObj)
        {
            if (updateObj == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //check if object to update exists and FK constraints are respected
            var allFound = false;
            Activity act = _actRepo.Get(id);
            allFound = act != null;
            allFound = allFound && updateObj.SpeakerId != null ? _usrRepo.Get((long)updateObj.SpeakerId) != null : allFound;
            if (!allFound)
            {
                return NotFound("The object to update or one of it's references could not be retrieved");
            }
            
            act.Description = updateObj.Description;
            act.StartTime = DateTime.ParseExact(updateObj.StartTime, "dd/MM/yyyy HH:mm", null);
            act.EndTime = DateTime.ParseExact(updateObj.EndTime, "dd/MM/yyyy HH:mm", null);
            act.SpeakerId = updateObj.SpeakerId ?? act.SpeakerId;
            act.Price = updateObj.Price;
            act.RemainingCapacity = updateObj.RemainingCapacity;

            _actRepo.Update(act);
            return Ok(act.ToResponseObject());           
        }

        // DELETE: api/activity/5
        //[HttpDelete("{id}")]
        //[ProducesResponseType(204)]
        //public IActionResult Delete(int id)
        //{
        //    var act = this._actRepo.Get(id);
        //    if (act == null)
        //    {
        //        return NotFound("No activity with the given id found");
        //    }
        //    this._actRepo.Delete(act);

        //    return new NoContentResult();
        //}
    }
}