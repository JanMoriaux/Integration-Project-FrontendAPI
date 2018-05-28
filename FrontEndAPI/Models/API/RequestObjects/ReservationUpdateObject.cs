using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Models.API.RequestObjects
{
    public class ReservationUpdateObject
    {
        [Required]
        [Display(Name = "payedfee")]
        public bool PayedFee { get; set; }
        [Required]
        [Display(Name = "hasattended")]
        public bool HasAttended { get; set; }
        [Required]
        [Display(Name = "withinvoice")]
        public bool WithInvoice { get; set; }
    }
}
