using FrontEndAPI.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Models.API.RequestObjects
{
    public class EventCreateObject
    {
        [Required]
        [StringLength(500, MinimumLength = 2)]
        [Display(Name = "name")]
        public string Name { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 2)]
        [Display(Name = "description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "starttime")]
        [DateValidation]
        public string StartTime { get; set; }
        [Required]
        [Display(Name = "endtime")]
        [DateValidation]
        public string EndTime { get; set; }
        public IFormFile Image { get; set; }
    }
}
