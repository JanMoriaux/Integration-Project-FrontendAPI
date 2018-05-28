using FrontEndAPI.Models.API.RequestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Models.Entities
{
    public abstract class MQEntity
    {
        public long? Id { get; set; } 
        public string UUID { get; set; } 
        public long Version { get; set; } = 1;
        public bool IsActive { get; set; } = true;
        public DateTime LastUpdated { get; set; }

        public abstract string ToXMLMessage();
    }
}
