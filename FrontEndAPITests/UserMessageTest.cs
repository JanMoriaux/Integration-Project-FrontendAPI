using FrontEndAPI.Models.Entities;
using FrontEndAPI.XML.Wrappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace FrontEndAPITests
{
    public class UserMessageTest
    {
        private readonly UserMessage _message;
        private readonly ITestOutputHelper _output;

        public UserMessageTest(ITestOutputHelper output)
        {
            _output = output;

            var u = new User()
            {
                Id = 1,
                UUID = "263585bf-17af-4cb3-b280-51a1865a36be",
                Version = 1,
                IsActive = true,
                LastUpdated = DateTime.Now.ToLocalTime(),
                Firstname = "firstname",
                Lastname = "lastname",
                Email = "test@test.com",
                Street = "street",
                Number = 123,
                Bus = null,
                ZipCode = 1000,
                City = "city",
                EmailVerified = false,
                RolesString = "Visitor,Admin"
            };
            
            _message = UserMessage.FromUser(u);
        }

        [Fact]
        public void TestFromUser()
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(UserMessage));

            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, _message);
                    xml = sww.ToString(); // Your XML
                }
            }
            _output.WriteLine(xml);

            XmlReaderSettings settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema
            };
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

            // Create the XmlReader object.
            XmlReader reader = XmlReader.Create(new StringReader(xml), settings);

            // Parse the file. 
            while (reader.Read()) ;
        }

        // Display any warnings or errors.
        private void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                _output.WriteLine("\tWarning: Matching schema not found.  No validation occurred." + args.Message);
            else
                _output.WriteLine("\tValidation error: " + args.Message);

        }
    }
}
