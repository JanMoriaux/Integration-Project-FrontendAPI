using FrontEndAPI.Models.Database.Repository.UserRepo;
using FrontEndAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static FrontEndAPI.Models.Entities.User;

namespace FrontEndAPITests
{
    public class UserTest
    {
        private readonly User _user;
        private readonly ITestOutputHelper _output;

        public UserTest(ITestOutputHelper output)
        {
            _user = new User()
            {
                Firstname = "Jan",
                Lastname = "Moriaux",
                Email = "Janmoriaux@hotmail.com",
                Password = "Letmein123",
                Street = "Melkweide",
                Number = 37,
                Bus = null,
                ZipCode = 9030,
                City = "Mariakerke"
            };


            _output = output;
        }

        [Fact]
        public void TestUserInitialize()
        {
            Assert.Null(_user.Id);
            Assert.Null(_user.UUID);
            Assert.Equal(1, _user.Version);
            Assert.True(_user.IsActive);
            //Assert.Null(_user.LastUpdated);
            Assert.Equal("Jan", _user.Firstname);
            Assert.Equal("Moriaux", _user.Lastname);
            Assert.Equal("Janmoriaux@hotmail.com", _user.Email);
            Assert.Equal("Letmein123", _user.Password);
            Assert.Equal("Melkweide", _user.Street);
            Assert.Equal(37, _user.Number);
            Assert.Null(_user.Bus);
            Assert.Equal(9030, _user.ZipCode);
            Assert.Equal("Mariakerke", _user.City);
            Assert.Contains(UserRole.Visitor, _user.Roles);
            Assert.Equal("Visitor", _user.RolesString);
        }

        [Fact]
        public void TestUserAddRole()
        {
            _user.Roles.Add(UserRole.Admin);
            Assert.Contains(UserRole.Visitor, _user.Roles);
            Assert.Contains(UserRole.Admin, _user.Roles);
        }

        [Fact]
        public void TestUserSetRolesString()
        {
            _user.RolesString = "Admin,Visitor,Staff,Speaker,Stakeholder";
            Assert.Contains(UserRole.Visitor, _user.Roles);
            Assert.Contains(UserRole.Admin, _user.Roles);
            Assert.Contains(UserRole.Staff, _user.Roles);
            Assert.Contains(UserRole.Speaker, _user.Roles);
            Assert.Contains(UserRole.Stakeholder, _user.Roles);
        }

    }
}
