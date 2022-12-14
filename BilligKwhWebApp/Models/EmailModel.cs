using System;

namespace BilligKwhWebApp.Models
{
    public class EmailModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string CustomerName { get; set; }
        public string Body { get; set; }
        public string FromEmail { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public DateTime? DateSentUtc { get; set; }
        public string BccEmails { get; set; }
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string FromName { get; set; }
        public int CateGoryEnum { get; set; }
    }
}
