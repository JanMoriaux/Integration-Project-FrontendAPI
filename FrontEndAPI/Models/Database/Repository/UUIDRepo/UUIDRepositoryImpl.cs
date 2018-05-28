using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Models.Database.Repository.UUIDRepo
{
    public class UUIDRepositoryImpl : IUUIDRepository
    {
        private readonly S2ITSP2_2_Context _ctx;
        public UUIDRepositoryImpl(S2ITSP2_2_Context ctx)
        {
            _ctx = ctx;
        }

        public long? GetActivityIdFromUUID(string UUID)
        {
            return (long?)_ctx.Activities.FirstOrDefault(a => a.UUID == UUID)?.Id;
        }

        public string GetActivityUUIDFromId(long id)
        {
            return _ctx.Activities.FirstOrDefault(a => a.Id == id)?.UUID;
        }

        public long? GetEventIdFromUUID(string UUID)
        {
            return (long?)_ctx.Events.FirstOrDefault(e => e.UUID == UUID)?.Id;
        }

        public string GetEventUUIDFromId(long id)
        {
            return _ctx.Events.FirstOrDefault(e => e.Id == id)?.UUID;
        }

        public long? GetReservationIdFromUUID(string UUID)
        {
            return (long?)_ctx.Reservations.FirstOrDefault(r => r.UUID == UUID)?.Id;
        }

        public long? GetUserIdFromUUID(string UUID)
        {
            return (long?)_ctx.Users.FirstOrDefault(u => u.UUID == UUID)?.Id;
        }

        public string GetUserUUIDFromId(long id)
        {
            return _ctx.Users.FirstOrDefault(u => u.Id == id)?.UUID;
        }
    }
}
