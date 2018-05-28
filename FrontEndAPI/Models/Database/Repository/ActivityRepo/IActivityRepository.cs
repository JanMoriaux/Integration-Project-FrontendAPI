using FrontEndAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Models.Database.Repository.ActivityRepo
{
    public interface IActivityRepository
    {
        HashSet<Activity> GetAll();
        Activity Get(long id);
        Activity GetByNameAndEventUUID(string name, string eventUUID);
        HashSet<Activity> GetByEventId(long eventId);
        HashSet<Activity> GetBySpeakerId(long speakerId);
        Activity Create(Activity a,bool fromMessage=false);
        Activity Update(Activity a,bool fromMessage=false);
        //void Delete(Activity a,bool fromMessage = false);
    }
}
