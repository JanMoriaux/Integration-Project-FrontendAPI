using FrontEndAPI.Models.API.RequestObjects;
using FrontEndAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FrontEndAPI.Models.Entities.User;

namespace FrontEndAPI.Models.Database.Repository.UserRepo
{
    public interface IUserRepository
    {
        HashSet<User> GetAll();
        User Get(long id);
        User GetByEmail(string email);
        User Create(User u,bool fromMessage = false);
        User Update(User u,bool fromMessage = false);
        void Delete(User u,bool fromMessage =false);
    }
}
