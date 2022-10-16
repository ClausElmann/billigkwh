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
            MailMessage msg = new(new MailAddress("billigkwh@farmgain.com", "Fatal ERROR BilligKwh"), new MailAddress("claus.elmann@gmail.com", "Claus Elmann"))
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

#if DEBUG
            msg = new(new MailAddress("claus@blueIdea.dk", "Fatal ERROR BilligKwh"), new MailAddress("claus.elmann@gmail.com", "Claus Elmann"))
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
#endif

            SmtpClient client = new()
            {
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("billigkwh@farmgain.com", "Kenzo1234"),
                Port = 587,
                Host = "websmtp.simply.com",
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

#if DEBUG
            client.Credentials = new System.Net.NetworkCredential("claus@blueIdea.dk", "Flipper12#");
            client.Port = 587;
            client.Host = "smtp.office365.com";
            client.EnableSsl = true;
#endif


            try
            {
                client.Send(msg);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
