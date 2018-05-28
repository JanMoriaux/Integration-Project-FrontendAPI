using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Models.API.RequestObjects
{
    public class ResetPasswordRequestObject
    {
        [Required]
        [StringLength(500)]
        [EmailAddress]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",
        ErrorMessage = "No valid email address provided")]
        public string Email { get; set; }
    }
}
