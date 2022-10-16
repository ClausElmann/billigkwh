using System;
using BilligKwhWebApp.Core.Toolbox;
using Microsoft.IdentityModel.Tokens;


namespace BilligKwhWebApp.Tools.Auth
{
	public static class TokenAuthOption
    {
        public static string Audience { get; } = "billigkwh.dk";
        public static string Issuer { get; } = "billigkwh.dk";
        public static RsaSecurityKey Key { get; } = new RsaSecurityKey(RSAKeyHelper.GetKeyParameters("key"));
        public static SigningCredentials SigningCredentials { get; } = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);

        public static TimeSpan ExpiresSpan { get; } = TimeSpan.FromMinutes(20);
    }
}
