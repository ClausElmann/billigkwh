using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class RequestLog
    {
        public long Id { get; set; }
        public int? UserId { get; set; }
        public string Path { get; set; }
        public string QueryString { get; set; }
        public string Method { get; set; }
        public string Payload { get; set; }
        public string Response { get; set; }
        public string ResponseCode { get; set; }
        public DateTime RequestedOnUtc { get; set; }
        public int Ticks { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public string IpAddress { get; set; }
    }
}
