using BilligKwhWebApp.Core.Domain;
using System;

namespace BilligKwhWebApp.Models
{
    public class TokenModel
    {
        public int StateCode { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string AccessToken { get; set; }
        public int UserId { get; set; }
        public int CustomerId { get; set; }

        //The UserId of the user doing the impersonation
        public int ImpersonateFromUserId { get; set; }
        public UserRefreshToken RefreshTokenModel { get; set; }
    }
}
