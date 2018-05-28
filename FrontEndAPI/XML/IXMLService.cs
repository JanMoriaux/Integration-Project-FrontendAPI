using FrontEndAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FrontEndAPI.XML.XMLService;

namespace FrontEndAPI.XML
{
    public interface IXMLService
    {
        Task SendMessageAsync(MQEntity entity);
        bool ValidateXML(string xml);
        MessageType? GetMessageTypeFromXML(string xml);
        string GetSenderUUIDFromXML(string xml);
        MQEntity GenerateEntityFromMessage(string xml);
    }
}
