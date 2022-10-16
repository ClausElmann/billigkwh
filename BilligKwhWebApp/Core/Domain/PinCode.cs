using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class PinCode : BaseEntity
    {
        public DateTime DateCreatedUtc { get; set; }
        public long? PhoneNumber { get; set; }
        public string Email { get; set; }
        public string SaltedPinSHA256 { get; set; }
        public string Salt { get; set; }
    }
}
