using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Configuration
{
    public interface IMessageTypeConfiguration
    {
        int User { get; set; }
        int Event { get; set; }
        int Activity { get; set; }
        int Reservation { get; set; }
    }
}
