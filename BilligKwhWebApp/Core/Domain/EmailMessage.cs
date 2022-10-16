using System;
using System.Collections.Generic;

namespace BilligKwhWebApp.Core.Domain
{
    public class EmailMessage : BaseEntity
    {
        public int CustomerId { get; set; }
        public int? RefTypeID { get; set; }
        public int? RefID { get; set; }
        public int CategoryId { get; set; }
        public string Subject { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public DateTime? DateSentUtc { get; set; }
        public bool HasAttachments { get; set; }
        public string BccEmails { get; set; }
        public bool UseBcc { get; set; }
        public string ReplyTo { get; set; }
        public string Body { get; set; }

        public ICollection<EmailAttachment> Attachments { get; set; }

        public EmailMessage()
        {
            Attachments = new List<EmailAttachment>();
        }
        public EmailMessage(int customerId, string fromName, string fromEmail, string replyTo, string toName, string toEmail,
            string subject, string body, bool useBcc, string bccEmails, short categoryId, int? refTypeID = null, int? refID = null)
        {
            CustomerId = customerId;
            FromName = fromName;
            FromEmail = fromEmail;
            ReplyTo = replyTo;
            ToName = toName;
            ToEmail = toEmail;
            Subject = subject;
            Body = body;
            UseBcc = useBcc;
            BccEmails = bccEmails;
            CategoryId = categoryId;
            RefID = refID;
            RefTypeID = refTypeID;
            Attachments = new List<EmailAttachment>();
        }
    }
}
