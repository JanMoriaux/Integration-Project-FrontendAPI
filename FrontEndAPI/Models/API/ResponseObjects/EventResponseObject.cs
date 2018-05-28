using FrontEndAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Models.API.ResponseObjects
{
    public class EventResponseObject
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
        //[Display(Name = "activities")]
        //public HashSet<Activity> Activities { get; set; } = new HashSet<Activity>();
        [Display(Name = "imageurl")]
        public string ImageURL { get; set; }
        [Display(Name = "file")]
        public byte[] Image { get; set; }
    }
}
