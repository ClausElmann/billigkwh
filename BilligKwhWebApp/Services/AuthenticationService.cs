using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Core.Caching.Interfaces;
using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Services.Enums;
using BilligKwhWebApp.Core.Toolbox;
using BilligKwhWebApp.Core.Interfaces;
using System.Globalization;

namespace BilligKwhWebApp.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        // Authentication Constants
        private const int _PASSWORD_LENGTH = 4;
        private const string _PASSWORDCHARSLCASE = "abcdefghijklmnopqrstuvwxyz";
        private const string _PASSWORDCHARSUCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string _PASSWORDCHARSNUMERIC = "0123456789";
        private const string _PASSWORDCHARSSPECIAL = "*$-+?_&=!%{}/()#";

        //REMEMBER TO ROLL back before PROD!
        private readonly TimeSpan accesstokenExpiresSpan = TimeSpan.FromMinutes(15);
        private readonly TimeSpan refreshTokenExpiresSpan = TimeSpan.FromMinutes(480);

        // Token Authority
        private readonly string _audience = "billigkwh.dk";
        private readonly string _issuer = "billigkwh.dk";
        private RsaSecurityKey _key;
        private SigningCredentials _signingCredentials;

        // Props
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBaseRepository _baseRepository;
        private readonly IStaticCacheManager _cacheManager;
        private readonly ISystemLogger _logger;

        private User _cachedUser;

        // Ctor
        public AuthenticationService(IUserService userService,
            IHttpContextAccessor httpContextAccessor,
            IBaseRepository baseRepository,
            IStaticCacheManager cacheManager,
            ISystemLogger logger)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _baseRepository = baseRepository;
            _cacheManager = cacheManager;
            _logger = logger;
        }

        #region Private

        /// <summary>
        /// Returns newest, non-expired refresh token.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private UserRefreshToken GetRefreshTokenByUserId(int userId)
        {
            return _baseRepository.QueryFirstOrDefault<UserRefreshToken>(@"
					SELECT TOP (1) * FROM dbo.UserRefreshTokens 
					WHERE UserId = @UserId AND DateExpiresUtc > GETUTCDATE()",
                new { UserId = userId });
        }


        public Guid BeregnBrugernavnHashUnicode(string username)
        {
            Guid g = Guid.Empty;
            byte[] source;
            int offset = 0;

            var sha = new System.Security.Cryptography.SHA1Managed();
            sha.Initialize();

            source = sha.ComputeHash(Encoding.Unicode.GetBytes(username.ToLower(CultureInfo.InvariantCulture)));

            g = new Guid(
                (source[offset + 3] << 0x18) | (source[offset + 2] << 0x10) | (source[offset + 1] << 8) | source[offset + 0],
                (short)((source[offset + 5] << 8) | source[offset + 4]),
                (short)((source[offset + 7] << 8) | source[offset + 6]),
                source[offset + 8],
                source[offset + 9],
                source[offset + 10],
                source[offset + 11],
                source[offset + 12],
                source[offset + 13],
                source[offset + 14],
                source[offset + 15]);

            return g;
        }

        public Guid BeregnBrugernavnHashUtfCode(string username)
        {
            Guid g = Guid.Empty;
            byte[] source;
            int offset = 0;

            var sha = new System.Security.Cryptography.SHA1Managed();
            sha.Initialize();

            source = sha.ComputeHash(Encoding.UTF8.GetBytes(username.ToLower(CultureInfo.InvariantCulture)));

            g = new Guid(
                (source[offset + 3] << 0x18) | (source[offset + 2] << 0x10) | (source[offset + 1] << 8) | source[offset + 0],
                (short)((source[offset + 5] << 8) | source[offset + 4]),
                (short)((source[offset + 7] << 8) | source[offset + 6]),
                source[offset + 8],
                source[offset + 9],
                source[offset + 10],
                source[offset + 11],
                source[offset + 12],
                source[offset + 13],
                source[offset + 14],
                source[offset + 15]);

            return g;
        }




        public string CreateSalt()
        {
            var randomNumber = RandomNumberGenerator.Create();
            var buffer = new byte[16];
            randomNumber.GetBytes(buffer);

            return Convert.ToBase64String(buffer);
        }

        public string GenerateSHA256Hash(string password, string salt)
        {
            var bytes = Encoding.UTF8.GetBytes(password + salt);

            var sha256HashString = SHA256.Create();

            var hash = sha256HashString.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// generate a random key for Authorization
        /// </summary>
        /// <returns></returns>
        public static RSAParameters GenerateKey()
        {
            using (var key = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    return key.ExportParameters(true);
                }
                finally
                {
                    key.PersistKeyInCsp = false;
                }
            }
        }

        /// <summary>
        /// Save and generate a random key for Authorization
        /// </summary>
        /// <param name="file">File name of the key</param>
        private static void GenerateKeyAndSave(string file)
        {
            var key = GenerateKey();
            RSAParametersWithPrivate parametersWithPrivate = new();
            parametersWithPrivate.SetParameters(key);
            File.WriteAllText(file, JsonSerializer.Serialize(parametersWithPrivate, JsonHelper.JsonSerializerOptions));
        }

        /// <summary>
        /// Read authorization key
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static RSAParameters GetKeyParameters(string file)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException("Check configuration - cannot find auth key file: " + file);
            var keyParams = JsonSerializer.Deserialize<RSAParametersWithPrivate>(File.ReadAllText(file), JsonHelper.JsonSerializerOptions);
            return keyParams.ToRSAParameters();
        }

        private class RSAParametersWithPrivate
        {
            public byte[] D { get; set; }
            public byte[] DP { get; set; }
            public byte[] DQ { get; set; }
            public byte[] Exponent { get; set; }
            public byte[] InverseQ { get; set; }
            public byte[] Modulus { get; set; }
            public byte[] P { get; set; }
            public byte[] Q { get; set; }

            public void SetParameters(RSAParameters p)
            {
                D = p.D;
                DP = p.DP;
                DQ = p.DQ;
                Exponent = p.Exponent;
                InverseQ = p.InverseQ;
                Modulus = p.Modulus;
                P = p.P;
                Q = p.Q;
            }

            public RSAParameters ToRSAParameters()
            {
                return new RSAParameters()
                {
                    D = D,
                    DP = DP,
                    DQ = DQ,
                    Exponent = Exponent,
                    InverseQ = InverseQ,
                    Modulus = Modulus,
                    P = P,
                    Q = Q
                };
            }
        }

        public bool IsPasswordValid(string password)
        {
            if (password == null)
                return false;

            if (password.Length < _PASSWORD_LENGTH)
                return false;

            bool matchlowerLetter = password.IndexOfAny(_PASSWORDCHARSLCASE.ToCharArray()) > -1;
            bool matchUpperLetter = password.IndexOfAny(_PASSWORDCHARSUCASE.ToCharArray()) > -1;
            bool matchNumeric = password.IndexOfAny(_PASSWORDCHARSNUMERIC.ToCharArray()) > -1;
            bool matchSpecial = password.IndexOfAny(_PASSWORDCHARSSPECIAL.ToCharArray()) > -1;
            // Compare a string against the regular expression
            return true;//matchlowerLetter && matchUpperLetter && matchNumeric && matchSpecial;
        }
        #endregion

        #region Public

        public virtual User GetAuthenticatedUser()
        {
            if (_cachedUser != null)
                return _cachedUser;

            int userID = GetClaimValue(AccessTokenClaims.UserId);

            if (userID > 0)
            {
                var user = _userService.Get(userID);

                if (user != null)
                {
                    _cachedUser = user;
                }
            }
            return _cachedUser;
        }

        public int GetClaimValue(AccessTokenClaims claimType)
        {
            Claim claim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(t => t.Type == claimType.ToString());

            if (claim != null && int.TryParse(claim.Value, out int value))
            {
                return value;
            }
            return -1;
        }

        public void Logout(int userId)
        {
            _baseRepository.Execute("DELETE FROM dbo.UserRefreshTokens WHERE UserId = @UserId", new { UserId = userId });
        }

        public bool SetNewPassword(string newPassword, User user)
        {
            if (IsPasswordValid(newPassword))
            {
                var salt = CreateSalt();

                user.Password = GenerateSHA256Hash(newPassword, salt);
                user.PasswordSalt = salt;
                try
                {
                    if (user.Id > 0)
                        _userService.Update(user);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.Fatal("[SetNewPassword]", ex);
                    throw;
                }
            }
            else
            {
                return false;
            }
        }

        public UserRefreshToken GetUserRefreshToken(string token, bool onlyNonExpired = true)
        {
            return _baseRepository.QueryFirstOrDefault<UserRefreshToken>(@"
					SELECT TOP (1) * FROM dbo.UserRefreshTokens 
					WHERE (Token = @Token) AND 
					      (@OnlyNonExpired = 0 OR DateExpiresUtc > GETUTCDATE()) ",
                new { Token = token, OnlyNonExpired = onlyNonExpired });
        }

        public void ExtendRefreshToken(UserRefreshToken token, DateTime refreshRequestDateTimeUtc, TimeSpan? duration = null)
        {
            token.DateExpiresUtc = refreshRequestDateTimeUtc + (duration ?? refreshTokenExpiresSpan);
            _baseRepository.Update(token);
        }
        public UserRefreshToken CreateRefreshtoken(int userId, TimeSpan? duration = null)
        {
            var existingToken = GetRefreshTokenByUserId(userId);
            if (existingToken != null)
            {
                return existingToken;
            }
            else
            {
                string refreshtoken = string.Format("{0}{1}{2}", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Replace("-", "");

                var token = new UserRefreshToken
                {
                    Token = refreshtoken,
                    DateCreatedUtc = DateTime.UtcNow,
                    DateExpiresUtc = DateTime.UtcNow.Add(duration ?? refreshTokenExpiresSpan),
                    UserId = userId
                };

                // removes all refresh tokens from the user, and insert the new one
                using (var connection = ConnectionFactory.GetOpenConnection())
                {
                    connection.Execute(@"DELETE FROM dbo.UserRefreshTokens WHERE UserId = @UserId;
									INSERT INTO dbo.UserRefreshTokens (UserId, Token, DateCreatedUtc, DateExpiresUtc) VALUES (@UserId, @Token, @DateCreatedUtc, @DateExpiresUtc)",
                                        token);
                }

                return token;
            }
        }

        public string GenerateAccessToken(User user, int customerId, DateTime? expires = null, int? impersonateFromUserId = null)
        {
            if (user == null) return string.Empty;

            if (!expires.HasValue)
            {
                expires = GetAccessTokenExpirationTime();
            }

            var handler = new JwtSecurityTokenHandler();

            // Add claims to the token payload. These can always be accessed through HttpContext.User.Claims in Controllers
            ClaimsIdentity identity = new(
                new GenericIdentity(user.Email, "TokenAuth"),
                new[] {
                    new Claim(AccessTokenClaims.UserId.ToString(), user.Id.ToString()),
                    new Claim(AccessTokenClaims.CustomerId.ToString(), customerId.ToString()),
                    new Claim(AccessTokenClaims.ImpersonateFromUserId.ToString(),impersonateFromUserId==null? user.Id.ToString():impersonateFromUserId.ToString())
                }
            );

            _key = new RsaSecurityKey(GetKeyParameters("key"));
            _signingCredentials = new SigningCredentials(_key, SecurityAlgorithms.RsaSha256Signature);

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = _signingCredentials,
                Subject = identity,
                Expires = expires,
                IssuedAt = DateTime.UtcNow
            });
            return handler.WriteToken(securityToken);
        }

        public int ValidateAccessToken(string accessToken)
        {
            try
            {
                _key = new RsaSecurityKey(GetKeyParameters("key"));

                TokenValidationParameters validationParameters = new()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = _key,
                    ValidateLifetime = false
                };

                JwtSecurityTokenHandler tokenHandler = new();

                var result = tokenHandler.ValidateToken(accessToken, validationParameters, out SecurityToken jwt);

                if (result != null && result.HasClaim(c => c.Type == AccessTokenClaims.UserId.ToString()))
                {
                    return int.Parse(result.Claims.First(c => c.Type == AccessTokenClaims.UserId.ToString()).Value);
                }

                return 0;
            }
            catch
            {
                return -1;
            }
        }

        public int GeneratePinCode()
        {
            int seed = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new(seed);
            return _rdm.Next(_min, _max);
        }

        public void SavePinCode(int pinCode, long? phoneNumber, string email)
        {
            string salt = CreateSalt();
            string SHA = GenerateSHA256Hash(pinCode.ToString(), salt);
            PinCode PC = new()
            {
                PhoneNumber = phoneNumber,
                Email = email,
                SaltedPinSHA256 = SHA,
                Salt = salt,
                DateCreatedUtc = DateTime.UtcNow
            };
            _baseRepository.Insert(PC);
        }

        public bool ValidatePinCode(int pinCode, long? phoneNumber, string email, DateTime codeNewerThanDateTime)
        {
            string sqlParam = "";
            if (phoneNumber != null)
            {
                sqlParam += " PhoneNumber = @Phone ";
                if (!string.IsNullOrEmpty(email) && email.Length > 1)
                {
                    sqlParam += " and Email = @Email";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(email) && email.Length > 1)
                {
                    sqlParam += "Email = @Email";
                }
                else
                {
                    sqlParam = "Email = 'DONOTRETURNANYTHING'";
                }
            }

            List<PinCode> pinCodes = _baseRepository.Query<PinCode>(@"SELECT PC.* FROM dbo.PinCodes PC WHERE PC.DateCreatedUtc >= @OldDate and (" + sqlParam + ")",
                    new { Phone = phoneNumber, Email = email, OldDate = codeNewerThanDateTime }).ToList();

            foreach (var item in pinCodes)
            {
                string computedCode = GenerateSHA256Hash(pinCode.ToString(), item.Salt).Trim();
                if (computedCode.Equals(item.SaltedPinSHA256.Trim()))
                {
                    _baseRepository.Delete(item);
                    return true;
                }
            }
            return false;
        }

        public bool CanLoadSensitivePage(string ip, string pageNameId, DateTime newerThanUtc, int maxLoads)
        {
            List<SensitivePageLoad> sensitivePageLoads = _baseRepository.Query<SensitivePageLoad>(@"SELECT SPL.* FROM dbo.SensitivePageLoads SPL WHERE IP = @IP and PageNameId = @PageNameId and LoadDateTimeUtc >= @DateCheck",
                    new { IP = ip, PageNameId = pageNameId, DateCheck = newerThanUtc }).ToList();

            if (sensitivePageLoads.Count > maxLoads)
            {
                return false;
            }

            return true;
        }

        public void LoadSensitivePage(string ip, string pageNameId)
        {
            SensitivePageLoad sensitivePageLoad = new()
            {
                PageNameId = pageNameId,
                IP = ip,
                LoadDateTimeUtc = DateTime.UtcNow
            };
            _baseRepository.Insert(sensitivePageLoad);
        }

        public bool ValidateGoogleCaptchaToken(string token, string ip, string pageNameId)
        {
            using (var httpClient = new HttpClient())
            {
                var httpResponse = httpClient.GetAsync(new Uri($"https://www.google.com/recaptcha/api/siteverify?secret=6LcJFLYUAAAAAMr5TExd1FZCeWbVER3YyYxNk1KS&response=" + token)).Result;

                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    return false;
                }
                string jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;

                JsonDocument jsonData = JsonDocument.Parse(jsonResponse);

                if (Boolean.Parse(jsonData.RootElement.GetProperty("success").ToString()) != true)
                {
                    return false;
                }
            }

            foreach (SensitivePageLoad sensitivePageLoad in _baseRepository.Query<SensitivePageLoad>(@"SELECT SPL.* FROM dbo.SensitivePageLoads SPL WHERE IP = @IP and PageNameId = @PageNameId", new { IP = ip, PageNameId = pageNameId }).ToList())
            {
                _baseRepository.Delete(sensitivePageLoad);
            }
            return true;
        }

        public DateTime GetAccessTokenExpirationTime()
        {
            return DateTime.UtcNow + accesstokenExpiresSpan;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="senderId"></param>0 if unknown/unspecfied
        /// <param name="expires"></param>
        /// <returns></returns>

        #endregion
    }
}
