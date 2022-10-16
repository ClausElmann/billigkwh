using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Models;
using BilligKwhWebApp.Services.Enums;
using System;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email. 
        /// Notice: "sendTo" and "bccEmails" can be a semicolon-separated list of emails.
        /// </summary>
		bool Save(
            int customerId,
            string fromEmail,
            string fromName,
            string sendTo,
            string sendToName,
            string replyTo,
            string subject,
            string body,
            EmailCategoryEnum categoryEnum,
            int? refTypeID = null,
            int? refID = null,
            string bccEmails = null,
            IEnumerable<Tuple<byte[], string>> attachments = null,
            Cc_Bcc isCc_or_Bcc = Cc_Bcc.BCC);

        Result Save(EmailMessage emailMessage);
        void Save(IEnumerable<EmailMessage> emailMessages);

        Result<EmailMessage> Get(int id);
        Result Update(EmailMessage emailMessage);

        void SendTestMailFromBatchAppInDev(int messageId);

        IReadOnlyCollection<EmailModel> GetAll(int? customerId, DateTime fromDate, DateTime toDate);
        IReadOnlyCollection<EmailModel> GetTavleEmails(int tavleId);

        IReadOnlyCollection<int> GetAllUnsent(DateTime fromDate);
        void SendMail(int messageId);

        IReadOnlyCollection<ElprisModel> GetAllElpriser(DateTime fromDate, DateTime toDate);
    }
}
