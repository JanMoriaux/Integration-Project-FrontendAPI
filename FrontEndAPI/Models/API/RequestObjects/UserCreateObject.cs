using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Models.API.RequestObjects
{
    public class UserCreateObject{    
        [Required]
        [StringLength(500,MinimumLength = 2)]
        [Display(Name = "firstname")]
        public string Firstname { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 2)]
        [Display(Name = "lastname")]
        public string Lastname { get; set; }
        [Required]
        [StringLength(500)]
        [EmailAddress]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",
         ErrorMessage = "No valid email address provided")]
        [Display(Name = "email")]
        public string Email { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 2)]
        [Display(Name = "street")]
        public string Street { get; set; }
        [Required]
        [Range(1,10000)]
        [Display(Name = "number")]
        public int Number { get; set; }
        [StringLength(24, MinimumLength = 1)]
        [Display(Name = "bus")]
        public string Bus { get; set; }
        [Required]
        [Range(1000,9999)]
        [Display(Name = "zipcode")]
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
