using FrontEndAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Mail
{
    public interface IEmailService
    {
        void Send(EmailMessage emailMessage);
        void SendRegistrationConfirmation(Tuple<string,User> userAndOriginalPassword);
        void SendPasswordResetMail(Tuple<string, User> userAndOriginalPassword);
        void SendReservationConfirmation(Tuple<User, Activity, Event,bool> userAndActivityAndEventAndWithInvoice);
    }
}
