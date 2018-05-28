using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Configuration
{
    public class SystemConfiguration : ISystemConfiguration
    {
        public string Frontend { get; set; }
        public string CashRegister { get; set; }
        public string Facturation { get; set; }
        public string CRM { get; set; }
    }
}
