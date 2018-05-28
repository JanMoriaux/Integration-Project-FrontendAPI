using FrontEndAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Models.API.ResponseObjects
{
    public class UserResponseObject
    {
        //[Display(Name = "isactive")]
        //public bool IsActive { get; set; }
        [Display(Name="id")]
        public long Id { get; set; }
        [Display(Name = "firstname")]
        public string Firstname { get; set; }
        [Display(Name = "lastname")]
        public string Lastname { get; set; }
        [Display(Name = "email")]
        public string Email { get; set; }
        [Display(Name = "street")]
        public string Street { get; set; }
        [Display(Name = "number")]
        public int Number { get; set; }
        [Display(Name = "bus")]
        public string Bus { get; set; }
        [Display(Name = "zipcode")]
        public int ZipCode { get; set; }
        [Display(Name = "city")]
        public string City { get; set; }
        [Display(Name = "roles")]
        public string[] Roles { get; set; }
        //[Display(Name = "activities")]
        //public HashSet<Activity> Activities { get; set; }

    }
}
