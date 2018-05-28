using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Models.Database.Repository.UUIDRepo
{
    public interface IUUIDRepository
    {
        string GetActivityUUIDFromId(long id);
        string GetEventUUIDFromId(long id);
        string GetUserUUIDFromId(long id);
        long? GetEventIdFromUUID(string UUID);
        long? GetActivityIdFromUUID(string UUID);
        long? GetUserIdFromUUID(string UUID);
        long? GetReservationIdFromUUID(string UUID);
    }
}
