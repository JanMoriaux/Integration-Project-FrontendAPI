using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontEndAPI.Models.API.RequestObjects;
using FrontEndAPI.Models.API.ResponseObjects;
using FrontEndAPI.Models.Database.Repository.EventRepo;
using FrontEndAPI.Models.Entities;
using FrontEndAPI.Utility;
using FrontEndAPI.Validation;
using FrontEndAPI.XML;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrontEndAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/event")]
    public class EventController : Controller
    {
        private readonly IEventRepository _repo;

        public EventController(IEventRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<EventResponseObject>))]
        public IActionResult Get()
        {
            return Ok(_repo.GetAll().Select(e => e.ToResponseObject()));
        }

        // GET: api/event/5
        [HttpGet("{id}", Name = "GetEvent")]
        [ProducesResponseType(200, Type = typeof(EventResponseObject))]
        [ProducesResponseType(404)]
        public IActionResult GetById(long id)
        {
            Event evt = _repo.Get(id);

            if (evt == null)
            {
                return NotFound("No event with the given id found");
            }
            return Ok(evt.ToResponseObject());
        }

        //// POST: api/event
        [HttpPost]
        [RequestFormSizeLimit(valueCountLimit: 12000, Order = 1)]
        [ProducesResponseType(201, Type = typeof(EventResponseObject))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostAsync([FromForm] EventCreateObject createObj)
        {
            if (createObj == null)
            {
                return BadRequest();
            }
            //check for unique name
            if (createObj.Name != null)
            {
                Event e = _repo.GetByName(createObj.Name);
                if (e != null)
                {
                    ModelState.AddModelError("name", "Name already in use");
                }
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Event createEvent = Event.FromCreateObject(createObj);

            //upload the image and retrieve url
            if (createObj.Image != null)
            {
                await FileUtility.ProcessFile(new List<IFormFile>() { createObj.Image}, createEvent);
            }

            //INSERT new event
            _repo.Create(createEvent);

            return CreatedAtRoute("GetEvent", new { id = createEvent.Id }, createEvent.ToResponseObject());
        }

        //[HttpPut("{id}")]
        //[RequestFormSizeLimit(valueCountLimit: 12000, Order = 1)]
        //[ProducesResponseType(200, Type = typeof(EventResponseObject))]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        //public async Task<IActionResult> UpdateAsync(long id,[FromForm] EventUpdateObject updateObj)
        //{
        //    if (updateObj == null)
        //    {
        //        return BadRequest();
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    Event evt = _repo.Get(id);
        //    if (evt == null)
        //    {
        //        return NotFound();
        //    }
        //    evt.Description = updateObj.Description;
        //    evt.StartTime = DateTime.ParseExact(updateObj.StartTime,"dd/MM/yyyy HH:mm",null);
        //    evt.EndTime = DateTime.ParseExact(updateObj.EndTime,"dd/MM/yyyy HH:mm",null);

        //    //upload the image and retrieve url
        //    if (updateObj.Image != null)
        //    {
        //        await FileUtility.ProcessFile(new List<IFormFile>() { updateObj.Image }, evt);
        //    }
        //    //INSERT new event
        //    _repo.Update(evt);

        //    return Ok(evt.ToResponseObject());
        //}

        //// DELETE: api/event/5
        //[HttpDelete("{id}")]
        //[ProducesResponseType(204)]
        //public IActionResult Delete(int id)
        //{
        //    var evt = this._repo.Get(id);
        //    if (evt == null)
        //    {
        //        return NotFound("No event with the given id found");
        //    }

        //    this._repo.Delete(evt);

        //    return new NoContentResult();
        //}
    }
}