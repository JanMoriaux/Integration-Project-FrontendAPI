using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontEndAPI.Models.Database.Repository.ActivityRepo;
using FrontEndAPI.Models.Entities;
using FrontEndAPI.XML;

namespace FrontEndAPI.Models.Database.Repository.ReservationRepo
{
    public class ReservationRepositoryImpl : IReservationRepository
    {
        private readonly S2ITSP2_2_Context _ctx;
        private readonly IXMLService _xmlService;

        public ReservationRepositoryImpl(S2ITSP2_2_Context ctx, IXMLService xmlService)
        {
            _ctx = ctx;
            _xmlService = xmlService;
        }


        public Reservation Create(Reservation r, bool fromMessage = false)
        {
            //add the reservation
            _ctx.Reservations.Add(r);
            //update the related activity's capacity
            var act = _ctx.Activities.FirstOrDefault(a => a.Id == r.ActivityId);
            act.Version++;
            act.RemainingCapacity--;
            _ctx.Activities.Update(act);

            _ctx.SaveChanges();

            //send xml messages
            //if reservation created from message, don't repost the reservation to queue
            if (!fromMessage) _xmlService.SendMessageAsync(r);
            //post the activity with updated capacity to queue
            _xmlService.SendMessageAsync(act);

            return r;
        }

        public void Delete(Reservation r, bool fromMessage = false)
        {
            //deactivate the reservation
            r.IsActive = false;
            r.Version++;
            _ctx.Reservations.Update(r);
            //update the related activity's capacity
            var act = _ctx.Activities.FirstOrDefault(a => a.Id == r.ActivityId);
            act.Version++;
            act.RemainingCapacity++;
            _ctx.Activities.Update(act);

            _ctx.SaveChanges();

            //send xml messages
            //if reservation deleted from message, don't repost the reservation to queue
            if (!fromMessage) _xmlService.SendMessageAsync(r);
            //post the activity with updated capacity to queue
            _xmlService.SendMessageAsync(act);
        }

        public Reservation Update(Reservation r, bool fromMessage = false)
        {
            r.Version++;
            _ctx.Reservations.Update(r);
            _ctx.SaveChanges();
            if (!fromMessage) _xmlService.SendMessageAsync(r);

            return r;
        }

        public Reservation Get(long id)
        {
            return _ctx.Reservations.FirstOrDefault(r => r.Id == id && r.IsActive);

        }

        public HashSet<Reservation> GetAll()
        {
            return _ctx.Reservations.Where(r => r.IsActive).ToHashSet<Reservation>();
        }

        public HashSet<Reservation> GetByActivityId(long activityId)
        {
            return _ctx.Reservations.Where(r => r.ActivityId == activityId && r.IsActive).ToHashSet<Reservation>();
        }

        public Reservation GetByActivityIdAndVisitorId(long activityId, long visitorId)
        {
            return _ctx.Reservations.FirstOrDefault(r => r.ActivityId == activityId && r.VisitorId == visitorId && r.IsActive);
        }

        public HashSet<Reservation> GetByVisitorId(long visitorId)
        {
            return _ctx.Reservations.Where(r => r.VisitorId == visitorId && r.IsActive).ToHashSet<Reservation>();
        }

    }
}
