using FrontEndAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Models.API.ResponseObjects
{
    public class ActivityResponseObject
    {
        //[Display(Name = "isactive")]
        //public bool IsActive { get; set; }
        [Display(Name = "id")]
        public long Id { get; set; }
        [Display(Name = "name")]
        public string Name { get; set; }
        [Display(Name = "description")]
        public string Description { get; set; }
        [Display(Name = "starttime")]
        public DateTime StartTime { get; set; }
        [Display(Name = "endtime")]
        public DateTime EndTime { get; set; }
        //[Display(Name = "event")]
        //public  EventResponseObject Event { get; set; }
        //[Display(Name = "speaker")]
        //public UserResponseObject Speaker { get; set; }
        [Display(Name = "eventid")]
        public long EventId { get; set; }
        [Display(Name = "speakerid")]
        public long? SpeakerId { get; set; }
        [Display(Name = "price")]
        public decimal Price{ get; set; }
        [Display(Name = "remainingcapacity")]
        public int RemainingCapacity { get; set; }
    }
}
