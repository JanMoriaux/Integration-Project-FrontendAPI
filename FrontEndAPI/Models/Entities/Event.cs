using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FrontEndAPI.Models.API.RequestObjects;
using FrontEndAPI.Models.API.ResponseObjects;
using FrontEndAPI.Utility;

namespace FrontEndAPI.Models.Entities
{
    public class Event : MQHappening
    {
        public string ImageURL { get; set; }
        //public HashSet<Activity> Activities { get; set; } = new HashSet<Activity>();

        public override string ToXMLMessage()
        {
            throw new NotImplementedException();
        }

        public EventResponseObject ToResponseObject()
        {
            byte[] image = null;

            if (!String.IsNullOrEmpty(ImageURL))
            {
                using (FileStream stream = File.Open(ImageURL, FileMode.Open))
                {
                    image = FileUtility.ReadFully(stream);

                }
            }
            return new EventResponseObject()
            {
                //IsActive = IsActive,
                Id = (long)Id,
                Name = Name,
                Description = Description,
                StartTime = StartTime,
                EndTime = EndTime,
                //Activities = Activities,
                ImageURL = ImageURL,
                Image = image
            };

        }

        public static Event FromCreateObject(EventCreateObject createObj)
        {
            Event e = new Event()
            {
                UUID = UUIDGenerator.GenerateEventUUID(createObj.Name),
                Name = createObj.Name,
                Description = createObj.Description,
                StartTime = DateTime.ParseExact(createObj.StartTime,"dd/MM/yyyy HH:mm",null),
                EndTime = DateTime.ParseExact(createObj.EndTime, "dd/MM/yyyy HH:mm", null)
            };

            return e;
        }
    }
}
