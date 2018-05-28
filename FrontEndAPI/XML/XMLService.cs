using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using FrontEndAPI.Configuration;
using FrontEndAPI.Models.Database.Repository.ActivityRepo;
using FrontEndAPI.Models.Database.Repository.EventRepo;
using FrontEndAPI.Models.Database.Repository.UUIDRepo;
using FrontEndAPI.Models.Entities;
using FrontEndAPI.RabbitMQ.Producer;
using FrontEndAPI.XML.Wrappers;

namespace FrontEndAPI.XML
{
    public class XMLService : IXMLService
    {
        //enum with TypeId for messages that are accepted by the system
        public enum MessageType { User = 1,  Reservation = 4 }

        private readonly IMQProducer _producer;
        private readonly IUUIDRepository _uuidRepo;
        
        private bool _valid = true;


        public XMLService(IMQProducer producer, IUUIDRepository uuidRepo)
        {
            _producer = producer;
            _uuidRepo = uuidRepo;
        }

        public async Task SendMessageAsync(MQEntity entity)
        {
            try
            {
                var xml = GenerateXML(entity);
                if (!ValidateXML(xml))
                {
                    throw new Exception("Unable to construct valid xml from entity");
                }
                await _producer.SendMessageAsync(GenerateXML(entity));
            }
            catch (InvalidCastException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            catch (InvalidOperationException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public bool ValidateXML(string xml)
        {
            XmlReaderSettings settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema
            };
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

            using (XmlReader reader = XmlReader.Create(new StringReader(xml), settings))
            {
                try
                {
                    while (reader.Read()) ;
                }
                catch (XmlException)
                {
                    reader.Dispose();
                    _valid = false;
                }
            }
            if (_valid)
            {
                return true;
            }
            _valid = true;
            return false;
        }
        public MessageType? GetMessageTypeFromXML(string xml)
        {
            var doc = new XmlDocument();
            try
            {
                doc.LoadXml(xml);
                var root = doc.DocumentElement;
                // Check to see if the element has a typeId attribute and see if it corresponds to 
                // an accepted MessageType
                if (root.HasAttribute("typeId"))
                {
                    var typeId = Convert.ToInt32(root.GetAttribute("typeId"));

                    if (Enum.IsDefined(typeof(MessageType), typeId))
                    {
                        return (MessageType)typeId;
                    }
                }
            }
            catch (XmlException e) { System.Diagnostics.Debug.WriteLine(e.Message); }
            catch (FormatException e) { System.Diagnostics.Debug.WriteLine(e.Message); }

            return null;
        }

        public string GetSenderUUIDFromXML(string xml)
        {
            var doc = new XmlDocument();
            try
            {
                doc.LoadXml(xml);
                var root = doc.DocumentElement;
                // Check to see if the element has a typeId attribute and see if it corresponds to 
                // an accepted MessageType
                if (root.HasAttribute("senderUUID"))
                {
                    var senderUUID = root.GetAttribute("senderUUID");
                    return senderUUID;
                }
            }
            catch (XmlException e) { System.Diagnostics.Debug.WriteLine(e.Message); }
            return null;
        }

        public MQEntity GenerateEntityFromMessage(string xml)
        {
            MessageType? messageType = GetMessageTypeFromXML(xml);
            return messageType == null ? 
                null :
                GetEntityFromMessageAndMessageType(xml, (MessageType)messageType);
        }

        private string GenerateXML(MQEntity entity)
        {

            if (entity.GetType() == typeof(Event))
            {
                return GenerateEventXML((Event)entity);
            }
            else if (entity.GetType() == typeof(User))
            {
                return GenerateUserXML((User)entity);
            }
            else if (entity.GetType() == typeof(Activity))
            {
                return GenerateActivityXML((Activity)entity);
            }
            else if (entity.GetType() == typeof(Reservation))
            {
                return GenerateReservationXML((Reservation)entity);
            }
            else
            {
                throw new InvalidCastException("Could not cast MQEntity to one of it's subtypes!");
            }
        }
    
        private string GenerateEventXML(Event e)
        {
            return MessageToXML(EventMessage.FromEvent(e));
        }

        private string GenerateUserXML(User u)
        {
            return MessageToXML(UserMessage.FromUser(u));
        }
        private string GenerateActivityXML(Activity a)
        {
            var eventUUID = _uuidRepo.GetEventUUIDFromId(a.EventId);
            var speakerID = a.SpeakerId != null ? _uuidRepo.GetUserUUIDFromId((long)a.SpeakerId) : null;
            var activityWithEventUUIDAndSpeakerUUID = new Tuple<Activity, string, string>(a, eventUUID, speakerID);
            return MessageToXML(ActivityMessage.FromActivityTuple(activityWithEventUUIDAndSpeakerUUID));
        }

        private string GenerateReservationXML(Reservation r)
        {
            var activityUUID = _uuidRepo.GetActivityUUIDFromId(r.ActivityId);
            var vistorUUID = _uuidRepo.GetUserUUIDFromId(r.VisitorId);
            var reservationWithActivityUUIDAndVisitorUUID = new Tuple<Reservation, string, string>(r, activityUUID, vistorUUID);
            return MessageToXML(ReservationMessage.FromReservationTuple(reservationWithActivityUUIDAndVisitorUUID));
        }

        private string MessageToXML(Object obj)
        {
            var xs = new XmlSerializer(obj.GetType());
            var xml = "";

            using (var sw = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sw))
                {
                    try
                    {
                        xs.Serialize(writer, obj);
                        xml = sw.ToString();
                    }
                    catch (InvalidOperationException e)
                    {
                        writer.Dispose();
                        sw.Dispose();
                        throw e.InnerException;
                    }
                }
            }
            return xml;
        }

        private void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                System.Diagnostics.Debug.WriteLine("\tWarning: Matching schema not found.  No validation occurred." + args.Message);
            else
                System.Diagnostics.Debug.WriteLine("\tValidation error: " + args.Message);
            _valid = false;
        }

        private MQEntity GetEntityFromMessageAndMessageType(string xml, MessageType messageType)
        {
            MQEntity entity = null;

            switch (messageType)
            {
                case MessageType.User:
                    entity = GetUserFromMessage(xml);
                    break;
                //case MessageType.Event:
                //    entity = GetEventFromMessage(xml);
                //    break;
                //case MessageType.Activity:
                //    entity = GetActivityFromMessage(xml);
                //    break;
                case MessageType.Reservation:
                    entity = GetReservationFromMessage(xml);
                    break;
                default: break;
            }

            return entity;
        }

        private User GetUserFromMessage(string xml)
        {
            var u = UserMessage.FromMessage(xml)?.ToUser();
            if(u != null)
            {
                u.Id = _uuidRepo.GetUserIdFromUUID(u.UUID);
            }
            return u;
        }
        private Reservation GetReservationFromMessage(string xml)
        {
            Reservation r = null;
            var tuple = ReservationMessage.FromMessage(xml)?.ToReservationTuple();
            if (tuple != null)
            {
                r = tuple.Item1;
                var actUUID = tuple.Item2;
                var visUUID = tuple.Item3;

                r.Id = _uuidRepo.GetReservationIdFromUUID(r.UUID);
                r.ActivityId = (long)_uuidRepo.GetActivityIdFromUUID(actUUID);
                r.VisitorId =  (long)_uuidRepo.GetUserIdFromUUID(visUUID);
            }
            return r;
        }

        //private Event GetEventFromMessage(string xml)
        //{
        //    var e =  EventMessage.FromMessage(xml)?.ToEvent();
        //    if(e != null)
        //    {
        //        e.Id = _uuidRepo.GetEventIdFromUUID(e.UUID);
        //    }
        //    return e;
        //}

        //private Activity GetActivityFromMessage(string xml)
        //{
        //    Activity a = null;
        //    var tuple = ActivityMessage.FromMessage(xml)?.ToActivityTuple();
        //    if(tuple != null)
        //    {
        //        a = tuple.Item1;
        //        var evtUUID = tuple.Item2;
        //        var spkrUUID = tuple.Item3;

        //        a.Id = _uuidRepo.GetActivityIdFromUUID(a.UUID);
        //        a.EventId = (long)_uuidRepo.GetEventIdFromUUID(evtUUID);
        //        a.SpeakerId = spkrUUID != null ? _uuidRepo.GetUserIdFromUUID(spkrUUID) : null;
        //    }
        //    return a;
        //}
    }
}

