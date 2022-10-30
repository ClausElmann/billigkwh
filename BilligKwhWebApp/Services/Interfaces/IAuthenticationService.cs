using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Services.Enums;
using System;

namespace BilligKwhWebApp.Services.Interfaces
{
    public partial interface IAuthenticationService
    {
        string CreateSalt();
        Guid BeregnBrugernavnHashUnicode(string username);
        Guid BeregnBrugernavnHashUtfCode(string username);

        string GenerateSHA256Hash(string password, string salt);
        bool IsPasswordValid(string password);
        User GetAuthenticatedUser();

        int GetClaimValue(AccessTokenClaims claimType);

        void Logout(int userId);

        bool SetNewPassword(string newPassword, User user);

        int GeneratePinCode();

        void SavePinCode(int pinCode, long? phoneNumber, string email);
        bool ValidatePinCode(int pinCode, long? phoneNumber, string email, DateTime codeNewerThanDateTime);

        void LoadSensitivePage(string ip, string pageNameId);

        bool CanLoadSensitivePage(string ip, string pageNameId, DateTime newerThanUtc, int maxLoads);
        bool ValidateGoogleCaptchaToken(string token, string ip, string pageNameId);

        #region RefreshTokens
        UserRefreshToken CreateRefreshtoken(int userId, TimeSpan? duration = null);
        string GenerateAccessToken(User user, int customerId, DateTime? expires = null, int? impersonateFromUserId = null);

        /// <summary>
        /// Extends a refresh token
        /// </summary>
        /// <param name="token">The user refresh token that should be extended</param>
        /// <param name="refreshRequestDateTimeUtc">The datetime of which the refresh was requested</param>
        /// <param name="howLong">The time span to add to the refreshRequestDateTimeUtc in order to dtermine new expiration date time</param>
		void ExtendRefreshToken(UserRefreshToken token, DateTime refreshRequestDateTimeUtc, TimeSpan? duration = null);

        UserRefreshToken GetUserRefreshToken(string token, bool onlyNonExpired = true);
        int ValidateAccessToken(string accessToken);
        DateTime GetAccessTokenExpirationTime();

        #endregion
    }
}
