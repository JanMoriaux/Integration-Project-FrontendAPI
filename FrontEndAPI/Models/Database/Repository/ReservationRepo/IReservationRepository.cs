using FrontEndAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Models.Database.Repository.ReservationRepo
{
    public interface IReservationRepository
    {
        HashSet<Reservation> GetAll();
        Reservation Get(long id);
        Reservation GetByActivityIdAndVisitorId(long activityId, long visitorId);
        HashSet<Reservation> GetByActivityId(long activityId);
        HashSet<Reservation> GetByVisitorId(long visitorId);
        Reservation Create(Reservation r,bool fromMessage = false);
        void Delete(Reservation r,bool fromMessage = false);
        Reservation Update(Reservation r, bool fromMessage = false);
    }
}
