﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Models.API.RequestObjects
{
    public class MessageRequestObject
    {
        [Required]
        public string Message { get; set; }
    }
}
