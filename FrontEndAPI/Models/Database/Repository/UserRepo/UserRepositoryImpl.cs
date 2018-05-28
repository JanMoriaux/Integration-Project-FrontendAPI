using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using FrontEndAPI.Models.Database.Repository.ActivityRepo;
using FrontEndAPI.Models.Database.Repository.ReservationRepo;
using FrontEndAPI.Models.Entities;
using FrontEndAPI.XML;
using Microsoft.EntityFrameworkCore;

namespace FrontEndAPI.Models.Database.Repository.UserRepo
{
    public class UserRepositoryImpl : IUserRepository
    {
        private readonly S2ITSP2_2_Context _ctx;
        private readonly IXMLService _xmlService;

        public UserRepositoryImpl(S2ITSP2_2_Context ctx, IXMLService xmlService)
        {
            _ctx = ctx;
            _xmlService = xmlService;
        }

        public User Create(User u, bool fromMessage = false)
        {
            _ctx.Users.Add(u);
            _ctx.SaveChanges();

            //send xml messages
            if (!fromMessage) _xmlService.SendMessageAsync(u);

            return u;
        }

        public HashSet<User> GetAll()
        {
            return _ctx.Users.Where(u => u.IsActive).ToHashSet<User>();
        }

        public User Get(long id)
        {
            User u = _ctx.Users.FirstOrDefault(user => user.Id == id && user.IsActive);
            return u;
        }
        public User GetByEmail(string email)
        {
            User u = _ctx.Users.FirstOrDefault(user => user.Email.ToLower() == email.ToLower() && user.IsActive);
            return u;
        }

        public User Update(User u, bool fromMessage = false)
        {
            u.Version++;
            _ctx.Users.Update(u);
            _ctx.SaveChanges();

            //send xml messages
            if (!fromMessage) _xmlService.SendMessageAsync(u);

            return u;
        }

        public void Delete(User u, bool fromMessage = false)
        {
            //deactivate user
            u.Version++;
            u.IsActive = false;
            _ctx.Users.Update(u);

            //deactivate related activities for speakers
            var acts = _ctx.Activities.Where(a => a.SpeakerId == u.Id).ToList<Activity>();
            var activityReservations = new List<Reservation>();
            foreach (var a in acts)
            {
                a.Version++;
                a.IsActive = false;
                _ctx.Activities.Update(a);
                //deactivate the activity's reservations
                activityReservations = _ctx.Reservations.Where(r => r.ActivityId == a.Id && r.IsActive).ToList<Reservation>();
                foreach (var r in activityReservations)
                {
                    r.Version++;
                    r.IsActive = false;
                    _ctx.Reservations.Update(r);
                }
            }

            //deactivate the user's reservations
            var ress = _ctx.Reservations.Where(r => r.VisitorId == u.Id && !activityReservations.Contains(r)).ToList<Reservation>();
            foreach (var r in ress)
            {
                r.Version++;
                r.IsActive = false;
                _ctx.Reservations.Update(r);
            }

            _ctx.SaveChanges();

            //send xml messages: 
            //don't send user message if user has been deleted by message
            if (!fromMessage)
            {
                _xmlService.SendMessageAsync(u);
             
            }
            //send updated dependent activites and reservations
            foreach (var a in acts)
            {
                _xmlService.SendMessageAsync(a);
            }
            foreach (var r in activityReservations)
            {
                _xmlService.SendMessageAsync(r);
            }
            foreach (var r in ress)
            {
                _xmlService.SendMessageAsync(r);
            }
        }
    }
}

