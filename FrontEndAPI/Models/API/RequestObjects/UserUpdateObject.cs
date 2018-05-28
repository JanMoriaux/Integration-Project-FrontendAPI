using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Models.API.RequestObjects
{
    public class UserUpdateObject
    {
        //[Display(Name = "isActive")]
        //public bool? IsActive { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 2)]
        [Display(Name = "firstname")]
        public string Firstname { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 2)]
        [Display(Name = "lastname")]
        public string Lastname { get; set; }
        [StringLength(20,MinimumLength = 8)]
        [Display(Name = "password")]
        public string Password { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 2)]
        [Display(Name = "street")]
        public string Street { get; set; }
        [Required]
        [Range(1, 10000)]
        [Display(Name = "number")]
        public int Number { get; set; }
        [StringLength(24, MinimumLength = 1)]
        [Display(Name = "bus")]
        public string Bus { get; set; }
        [Required]
        [Range(1000, 9999)]
        [Display(Name = "zipCode")]
        public int ZipCode { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 2)]
        [Display(Name = "city")]
        public string City { get; set; }
        [Required]
        [Display(Name = "roles")]
        public string[] Roles { get; set; }
    }
}
