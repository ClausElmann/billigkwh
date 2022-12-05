using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.ServiceModel.Channels;

namespace BilligKwhWebApp.Services
{
    public class ErrorEmailSender : IErrorEmailSender
    {
        public bool SendErrorMail(string subject, string body)
        {

            string mailAccount = "noreply@billigkwh.dk";
            string password = "k3dJKUl6Ej$7";

            MailMessage msg = new(new MailAddress("noreply@billigkwh.dk", "Fatal ERROR BilligKwh"), new MailAddress("claus.elmann@gmail.com", "Claus Elmann"))
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

#if DEBUG
            msg = new(new MailAddress("noreply@billigkwh.dk", "Fatal ERROR BilligKwh"), new MailAddress("claus.elmann@gmail.com", "Claus Elmann"))
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
#endif

            SmtpClient client = new()
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mailAccount, password),
                Port = 587,
                Host = "websmtp.simply.com",
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };

#if DEBUG
            client.Host = "smtp.simply.com";
#endif

            try
            {
                client.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
