using FrontEndAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Models.Database.Repository.EventRepo
{
    public interface IEventRepository
    {
        HashSet<Event> GetAll();
        Event Get(long id);
        Event GetByName(string name);
        Event Create(Event e, bool fromMessage = false);
        //Event Update(Event e, bool fromMessage = false);
        //void Delete(Event e,bool fromMessage = false);
    }
}
