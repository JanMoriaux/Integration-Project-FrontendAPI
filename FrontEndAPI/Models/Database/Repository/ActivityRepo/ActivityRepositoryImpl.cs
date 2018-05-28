using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontEndAPI.Models.Database.Repository.ReservationRepo;
using FrontEndAPI.Models.Entities;
using FrontEndAPI.XML;
using Microsoft.EntityFrameworkCore;

namespace FrontEndAPI.Models.Database.Repository.ActivityRepo
{
    public class ActivityRepositoryImpl : IActivityRepository
    {
        private readonly S2ITSP2_2_Context _ctx;
        private readonly IXMLService _xmlService;

        public ActivityRepositoryImpl(S2ITSP2_2_Context ctx, IXMLService xmlService)
        {
            _ctx = ctx;
            _xmlService = xmlService;
        }

        public Activity Create(Activity a, bool fromMessage = false)
        {
            _ctx.Activities.Add(a);
            _ctx.SaveChanges();

            //send xml messages
            if (!fromMessage) _xmlService.SendMessageAsync(a);

            return a;
        }

        //public void Delete(Activity a, bool fromMessage = false)
        //{
        //    //deactivate activity
        //    a.Version++;
        //    a.IsActive = false;
        //    _ctx.Activities.Update(a);

        //    //deactivate related reservations
        //    var ress = _ctx.Reservations.Where(r => r.ActivityId == a.Id && r.IsActive).ToList<Reservation>();
        //    foreach (var r in ress)
        //    {
        //        r.Version++;
        //        r.IsActive = false;
        //        _ctx.Reservations.Update(r);
        //    }
        //    _ctx.SaveChanges();

        //    //send xml messages
        //    if (!fromMessage)
        //    {
        //        _xmlService.SendMessageAsync(a);
        //        foreach (var r in ress)
        //        {
        //            _xmlService.SendMessageAsync(r);
        //        }
        //    }
        //}

        public Activity Get(long id)
        {
            Activity act = _ctx.Activities.FirstOrDefault(a => a.Id == id && a.IsActive);
            return act;
        }

        public HashSet<Activity> GetAll()
        {
            return _ctx.Activities.Where(a => a.IsActive).ToHashSet<Activity>();
        }

        public HashSet<Activity> GetByEventId(long eventId)
        {
            return _ctx.Activities.Where(a => a.EventId == eventId && a.IsActive).ToHashSet<Activity>();
        }

        public Activity GetByNameAndEventUUID(string name, string eventUUID)
        {
            Activity act = _ctx.Activities.FirstOrDefault(a => a.Event.UUID == eventUUID && a.Name == name && a.IsActive);
            return act;
        }

        public HashSet<Activity> GetBySpeakerId(long speakerId)
        {
            return _ctx.Activities.Where(a => a.SpeakerId == speakerId && a.IsActive).ToHashSet<Activity>();
        }

        public Activity Update(Activity a, bool fromMessage = false)
        {
            a.Version++;
            _ctx.Activities.Update(a);
            _ctx.SaveChanges();

            //send Activity message
            if (!fromMessage) _xmlService.SendMessageAsync(a);

            return a;
        }
    }
}
