﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Models.API.RequestObjects
{
    public class LoginRequestObject
    {
        [Required]
        [Display(Name = "email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "password")]
        public string Password { get; set; }
    }
}
