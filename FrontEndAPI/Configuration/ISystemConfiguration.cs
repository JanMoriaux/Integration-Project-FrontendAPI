using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Configuration
{
    public interface ISystemConfiguration
    {
        string Frontend { get; set; }
        string CashRegister { get; set; }
        string Facturation { get; set; }
        string CRM { get; set; }
    }
}
