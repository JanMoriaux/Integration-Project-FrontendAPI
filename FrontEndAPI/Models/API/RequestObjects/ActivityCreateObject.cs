using FrontEndAPI.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Models.API.RequestObjects
{
    public class ActivityCreateObject
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
        [Required]
        [Display(Name = "eventId")]
        public long EventId { get; set; }
        [Display(Name = "speakerId")]
        public long? SpeakerId { get; set; }
        [Required]
        [Display(Name = "price")]
        public decimal Price { get; set; }
        [Required]
        [Display(Name = "remainingcapacity")]
        public int RemainingCapacity { get; set; }
    }
}
