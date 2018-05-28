using FrontEndAPI.Models.Entities;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndAPI.Mail
{
    //https://dotnetcoretutorials.com/2017/11/02/using-mailkit-send-receive-email-asp-net-core/
    public class EmailService : IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailService(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }


        public void Send(EmailMessage emailMessage)
        {
            var message = new MimeMessage();
            message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

            message.Subject = emailMessage.Subject;
            //We will say we are sending HTML. But there are options for plaintext etc. 
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = emailMessage.Content
            };

            //Be careful that the SmtpClient class is the one from Mailkit not the framework!
            using (var emailClient = new SmtpClient())
            {
                try
                {
                    emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, false);
                    //Remove any OAuth functionality as we won't be using it. 
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                    emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                    emailClient.Send(message);
                    emailClient.Disconnect(true);
                } catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
        }

        //TODO add genuine account activation link
        public void SendRegistrationConfirmation(Tuple<string, User> userAndOriginalPassword)
        {
            var user = userAndOriginalPassword.Item2;

            var toAddress = GenerateToAddress(user);
            var fromAddress = GenerateFromAddress();
            var subject = "SuccessFully Registered For Integration Project";
            var body = GetRegistrationConfirmationContent(userAndOriginalPassword);
            var message = new EmailMessage()
            {
                Subject = subject,
                Content = body
            };
            message.ToAddresses.Add(toAddress);
            message.FromAddresses.Add(fromAddress);

            var sendTask = Task.Run(() => Send(message));
        }
        public void SendPasswordResetMail(Tuple<string, User> userAndOriginalPassword)
        {
            var user = userAndOriginalPassword.Item2;

            var toAddress = GenerateToAddress(user);
            var fromAddress = GenerateFromAddress();
            var subject = "Your account password has been reset";
            var body = GetPasswordResetContent(userAndOriginalPassword);
            var message = new EmailMessage()
            {
                Subject = subject,
                Content = body
            };
            message.ToAddresses.Add(toAddress);
            message.FromAddresses.Add(fromAddress);

            var sendTask = Task.Run(() => Send(message));
        }
        public void SendReservationConfirmation(Tuple<User, Activity, Event, bool> userAndActivityAndEventAndWithInvoice)
        {
            var user = userAndActivityAndEventAndWithInvoice.Item1;
            var activity = userAndActivityAndEventAndWithInvoice.Item2;
            var evt = userAndActivityAndEventAndWithInvoice.Item3;
            var withInvoice = userAndActivityAndEventAndWithInvoice.Item4;

            var toAddress = GenerateToAddress(user);
            var fromAddress = GenerateFromAddress();
            var subject = String.Format("Your reservation for {0}", activity.Name);
            var body = GetReservationConfirmationContent(userAndActivityAndEventAndWithInvoice);

            var message = new EmailMessage()
            {
                Subject = subject,
                Content = body
            };
            message.ToAddresses.Add(toAddress);
            message.FromAddresses.Add(fromAddress);

            var sendTask = Task.Run(() => Send(message));

        }

        private string GetRegistrationConfirmationContent(Tuple<string, User> userAndOriginalPassword)
        {
            var password = userAndOriginalPassword.Item1;
            var user = userAndOriginalPassword.Item2;
            var hashedPassForUrl = Uri.EscapeDataString(user.Password);

            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("<h2>Welcome to our event site, {0} {1}</h2>", user.Firstname, user.Lastname));
            builder.Append(String.Format("<p>Your password is: <b>{0}</b></p>", password));
            builder.Append("<p>Before logging in, please click the link below to activate your account</p>");
            //builder.Append(String.Format("<p><a href=\"http://localhost:59116/api/login/activate?Pass={0}&Email={1}\">Activate your account</a></p>", hashedPassForUrl, user.Email));
            builder.Append(String.Format("<p><a href=\"http://ec2-52-29-5-250.eu-central-1.compute.amazonaws.com:8080/api/login/activate?Pass={0}&Email={1}\">Activate your account</a></p>", hashedPassForUrl, user.Email));
            return builder.ToString();
        }



        private string GetPasswordResetContent(Tuple<string, User> userAndOriginalPassword)
        {
            var password = userAndOriginalPassword.Item1;
            var user = userAndOriginalPassword.Item2;

            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("<h2>Hi, {0} {1}</h2>", user.Firstname, user.Lastname));
            builder.Append("<p>Your account password has been reset.</p>");
            builder.Append(String.Format("<p>Your new password is: <b>{0}</b></p>", password));
            builder.Append("<p>You can change this auto-generated password by logging in to our website and updating your account.</p>");
            builder.Append(String.Format("<p>If you didn't request this password reset, please contact us at <a href=\"mailto:{0}\">{1}</a></p>", _emailConfiguration.SmtpUsername, _emailConfiguration.SmtpUsername));
            return builder.ToString();
        }

        private string GetReservationConfirmationContent(Tuple<User, Activity, Event, bool> userAndActivityAndEventAndWithInvoice)
        {
            var user = userAndActivityAndEventAndWithInvoice.Item1;
            var activityName = userAndActivityAndEventAndWithInvoice.Item2.Name;
            var activityPrice = userAndActivityAndEventAndWithInvoice.Item2.Price;
            var eventName = userAndActivityAndEventAndWithInvoice.Item3.Name;
            var withInvoice = userAndActivityAndEventAndWithInvoice.Item4;

            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("<h2>Hi, {0} {1}</h2>", user.Firstname, user.Lastname));
            builder.Append(String.Format("<p>We have successfully received your reservation for {0} during {1}.</p>", activityName, eventName));
            if (withInvoice)
            {
                builder.Append(String.Format("<p>You will reveive an invoice for € {0:F2} from our facturation department.</p>", activityPrice));
            }
            else
            {
                builder.Append(String.Format("<p>You can pay the entrance fee of € {0:F2} at the cash register of the event</p>", activityPrice));
            }
            builder.Append(String.Format("<p>You will be granted access to the activity upon reception of your payment.</p>", activityName, eventName));
            builder.Append(String.Format("<p>If you didn't make this reservation, please contact us at <a href=\"mailto:{0}\">{1}</a></p>", _emailConfiguration.SmtpUsername, _emailConfiguration.SmtpUsername));
            return builder.ToString();
        }

        private EmailAddress GenerateToAddress(User user)
        {
            return new EmailAddress()
            {
                Name = String.Format("{0} {1}", user.Firstname, user.Lastname),
                Address = user.Email

            };
        }

        private EmailAddress GenerateFromAddress()
        {
            return new EmailAddress()
            {
                Name = "Integration Project",
                Address = _emailConfiguration.SmtpUsername

            };
        }

    }

}
