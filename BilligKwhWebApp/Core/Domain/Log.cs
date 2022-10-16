using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class Log : BaseEntity
    {
        public int LogLevelId { get; set; }
        public string ShortMessage { get; set; }
        public string FullMessage { get; set; }
        public string IpAddress { get; set; }
        public string PageUrl { get; set; }
        public string ReferrerUrl { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public string Module { get; set; }
        public string DataObject { get; set; }
        public bool? IsHandled { get; set; } // Remove??
        public int? UserId { get; set; }

        // Ctor
        public Log()
        {

        }
    }

    // Dublicate of Above.. Use Enums (logLevel) Directly..
    public class LogEntry : BaseEntity
    {
        public string ShortMessage { get; set; }
        public string FullMessage { get; set; }
        public string IpAddress { get; set; }
        public string PageUrl { get; set; }
        public string ReferrerUrl { get; set; }
        public LogLevel LogType { get; set; }
        public int? UserId { get; set; }
        public string Module { get; set; }
        public string DataObject { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public bool IsHandled { get; set; }

        // Ctor
        public LogEntry()
        {

        }
        public LogEntry(string shortMessage, string fullMessage, string ipAddress, string pageUrl, string referrerUrl, LogLevel logType,
            int userId, string module, string dataObject, bool isHandled)
        {
            ShortMessage = shortMessage;
            FullMessage = fullMessage;
            IpAddress = ipAddress;
            PageUrl = pageUrl;
            ReferrerUrl = referrerUrl;
            Module = module;
            DataObject = dataObject;
            DateCreatedUtc = DateTime.UtcNow;
            IsHandled = false;
        }

        // Set
        public LogEntry Override_CreateDate(DateTime newDateTime)
        {
            DateCreatedUtc = newDateTime;
            return this;
        }

        // Log Creation Intervals
        public double GetCreatedTimeDifference_Seconds(DateTime dateTime)
        {
            return dateTime.Subtract(DateCreatedUtc).TotalSeconds;
        }
        public double GetCreatedTimeDifference_Minutes(DateTime dateTime)
        {
            return dateTime.Subtract(DateCreatedUtc).TotalMinutes;
        }
    }
}
