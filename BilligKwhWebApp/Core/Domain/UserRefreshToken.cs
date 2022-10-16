using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class UserRefreshToken : BaseEntity
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public DateTime DateExpiresUtc { get; set; }
    }
}
