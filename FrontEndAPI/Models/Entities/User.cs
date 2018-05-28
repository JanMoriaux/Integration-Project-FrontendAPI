using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FrontEndAPI.Models.API.RequestObjects;
using FrontEndAPI.Models.API.ResponseObjects;
using FrontEndAPI.Utility;
using Newtonsoft.Json;

namespace FrontEndAPI.Models.Entities
{
    public class User : MQEntity
    {
        public enum UserRole { Admin, Visitor, Staff, Stakeholder, Speaker }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public string Salt { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string Bus { get; set; }
        public int ZipCode { get; set; }
        public string City { get; set; }
        public ISet<UserRole> Roles { get; set; } = new HashSet<UserRole>() { UserRole.Visitor };
        public bool EmailVerified { get; set; } = false;
        //this property is used for storing the Roles enum set to db
        //as a comma separated value
        public string RolesString
        {
            get
            {
                string RolesString = string.Join(",", Roles);
                return RolesString;
            }

            set
            {
                string[] rolesStrings = value.Split(',');
                AddRoles(rolesStrings, Roles);
            }
        }

        public override string ToXMLMessage()
        {
            throw new NotImplementedException();
        }

        public static Tuple<string,User> FromCreateObject(UserCreateObject createObj)
        {
            var password = PasswordGenerator.CreateRandomPassword(8);
            var hashedPassword = PasswordGenerator.EncryptPassword(password);
          
            User u = new User()
            {
                UUID = UUIDGenerator.GenerateUserUUID(createObj.Email),
                Firstname = createObj.Firstname,
                Lastname = createObj.Lastname,
                Email = createObj.Email,
                Password = hashedPassword.HashedPwd,
                Salt = hashedPassword.Salt,
                Street = createObj.Street,
                Number = createObj.Number,
                Bus = createObj.Bus,
                ZipCode = createObj.ZipCode,
                City = createObj.City
            };
            AddRoles(createObj.Roles, u.Roles);

            return new Tuple<string,User>(password,u);
        }

        public UserResponseObject ToResponseObject()
        {
            return new UserResponseObject()
            {
                //IsActive = IsActive,
                Id = (long)Id,
                Firstname = Firstname,
                Lastname = Lastname,
                Email = Email,
                Street = Street,
                Number = Number,
                Bus = Bus,
                ZipCode = ZipCode,
                City = City,
                Roles = RolesString.Split(','),
                //Activities = Activities
            };
        }

        public static void AddRoles(string[] rolesStrings, ISet<UserRole> roles)
        {
            foreach (var roleString in rolesStrings)
            {
                if (Enum.TryParse(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(roleString.ToLower()), out UserRole role))
                {
                    roles.Add(role);
                }
            }
        }
    }
}
