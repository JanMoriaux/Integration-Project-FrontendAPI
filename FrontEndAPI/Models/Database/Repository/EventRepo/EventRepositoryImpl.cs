using FrontEndAPI.Models.Database.Repository.ActivityRepo;
using FrontEndAPI.Models.Entities;
using FrontEndAPI.XML;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Models.Database.Repository.EventRepo
{
    public class EventRepositoryImpl : IEventRepository
    {
        private readonly S2ITSP2_2_Context _ctx;
        private readonly IXMLService _xmlService;

        public EventRepositoryImpl(S2ITSP2_2_Context ctx, IXMLService xmlService)
        {
            _ctx = ctx;
            _xmlService = xmlService;
        }

        public Event Create(Event e, bool fromMessage = false)
        {
            _ctx.Events.Add(e);
            _ctx.SaveChanges();

            //send xml messages
            if (!fromMessage) _xmlService.SendMessageAsync(e);

            return e;
        }

        //public void Delete(Event e, bool fromMessage = false)
        //{
        //    //deactivate event 
        //    e.Version++;
        //    e.IsActive = false;
        //    _ctx.Events.Update(e);

        //    //deactivate all dependent activities
        //    var acts = _ctx.Activities.Where(a => a.EventId == e.Id && a.IsActive).ToList<Activity>();
        //    var activityReservations = new List<Reservation>();
        //    foreach (var a in acts)
        //    {
        //        a.Version++;
        //        a.IsActive = false;
        //        _ctx.Activities.Update(a);

        //        //deactivate the activity's dependent reservations
        //        activityReservations = _ctx.Reservations.Where(r => r.ActivityId == a.Id && r.IsActive).ToList<Reservation>();
        //        foreach (var r in activityReservations)
        //        {
        //            r.Version++;
        //            r.IsActive = false;
        //            _ctx.Reservations.Update(r);
        //        }
        //    }
        //    _ctx.SaveChanges();

        //    //send xml messages
        //    if (!fromMessage)
        //    {
        //        _xmlService.SendMessageAsync(e);
        //        foreach (var a in acts)
        //        {
        //            _xmlService.SendMessageAsync(a);
        //        }
        //        foreach (var r in activityReservations)
        //        {
        //            _xmlService.SendMessageAsync(r);
        //        }
        //    }
        //}

        public Event Get(long id)
        {
            var evt = _ctx.Events.FirstOrDefault(e => e.Id == id && e.IsActive);
            return evt;
        }

        public HashSet<Event> GetAll()
        {
            return _ctx.Events.Where(e => e.IsActive).ToHashSet<Event>();
        }

        public Event GetByName(string name)
        {
            var evt = _ctx.Events.FirstOrDefault(e => e.Name == name && e.IsActive);
            return evt;
        }

        //public Event Update(Event e, bool fromMessage = false)
        //{
        //    e.Version++;
        //    _ctx.Events.Update(e);
        //    _ctx.SaveChanges();

        //    //send xml messages
        //    if(!fromMessage) _xmlService.SendMessageAsync(e);

        //    return e;
        //}
    }
}
