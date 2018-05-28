using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Configuration
{
    public class MessageTypeConfiguration:IMessageTypeConfiguration
    {
        public int User { get; set; }
        public int Event { get; set; }
        public int Activity { get; set; }
        public int Reservation { get; set; }
    }
}
