using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Models.API.RequestObjects
{
    public class ReservationCreateObject
    {
        [Required]
        [Display(Name = "activityid")]
        public long ActivityId { get; set; }
        [Required]
        [Display(Name = "visitorid")]
        public long VisitorId { get; set; }
        [Required]
        [Display(Name = "withinvoice")]
        public bool WithInvoice{ get; set; }
    }
}
