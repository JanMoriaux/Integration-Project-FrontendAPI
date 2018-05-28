using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Models.API.RequestObjects
{
    public class ActivationRequestObject
    {
        [Required]
        [StringLength(500)]
        [Display(Name = "pass")]
        public string Pass { get; set; }
        [Required]
        [StringLength(500)]
        [EmailAddress]
        [Display(Name = "email")]
        public string Email { get; set; }
    }
}
