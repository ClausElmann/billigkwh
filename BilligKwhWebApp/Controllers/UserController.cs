using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BilligKwhWebApp.Tools.Url;
using BilligKwhWebApp.Core;
using BilligKwhWebApp.Services.Interfaces;
using IAuthenticationService = BilligKwhWebApp.Services.Interfaces.IAuthenticationService;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Services.Localization;
using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Models;
using BilligKwhWebApp.Core.Dto;
using BilligKwhWebApp.Core.Domain.ValueObjects;
using BilligKwhWebApp.Services.Enums;
using BilligKwhWebApp.Core.Factories;
using BilligKwhWebApp.Infrastructure.DataTransferObjects.Common;
using BilligKwhWebApp.Services.Customers;

namespace BilligKwhWebApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(UserRolePermissionProvider.Bearer)]
    public class UserController : BaseController
    {
        private readonly ISystemLogger _logger;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICustomerService _customerService;
        private readonly IWorkContext _workContext;
        private readonly IEmailService _emailService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IUserFactory _userfactory;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILanguageService _languageService;
        private readonly ICustomerFactory _customerFactory;
        private readonly IUserUrlGenerator _userUrlGenerator;
        private readonly IUtcClock _utcClock;
        private readonly IGloballyUniqueIdentifier _globallyUniqueIdentifier;

        public IConfiguration Configuration { get; }

        // Ctor
        public UserController(ISystemLogger logger,
            IUserService userService,
            IAuthenticationService authenticationService,
            ICustomerService customerService,
            IWorkContext workContext,
            IEmailService emailService,
            IEmailTemplateService emailTemplateService,
            IPermissionService permissionService,
            IUserFactory userfactory,
            ILocalizationService localizationService,
            IHttpContextAccessor httpContextAccessor,
            ILanguageService languageService,
            IConfiguration configuration,
            ICustomerFactory customerFactory,
            IUserUrlGenerator userUrlGenerator,
            IUtcClock utcClock,
            IGloballyUniqueIdentifier globallyUniqueIdentifier) : base(logger, workContext, permissionService)
        {
            _logger = logger;
            _userService = userService;
            _authenticationService = authenticationService;
            _customerService = customerService;
            _workContext = workContext;
            _emailService = emailService;
            _emailTemplateService = emailTemplateService;
            _userfactory = userfactory;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
            _languageService = languageService;
            Configuration = configuration;
            _customerFactory = customerFactory;
            _userUrlGenerator = userUrlGenerator;
            _utcClock = utcClock;
            _globallyUniqueIdentifier = globallyUniqueIdentifier;
        }


        #region Internals
        private Language GetUserLanguage(User user)
        {
            var languages = _languageService.GetAllLanguages();
            if (user != null && user.LanguageId > 0)
            {
                return languages.FirstOrDefault(x => x.Id == user.LanguageId);
            }

            var currentLang = GetLanguageFromRequest(languages);
            if (currentLang != null)
            {
                return currentLang;
            }

            return languages.FirstOrDefault(x => x.Id == 1); //Default - Danish
        }

        private Language GetLanguageFromRequest(IList<Language> languages)
        {

            if (_httpContextAccessor.HttpContext?.Request == null)
            {
                return null;
            }

            //get request culture
            var requestCulture = _httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture;
            if (requestCulture == null)
            {
                return null;
            }

            //try to get language by culture name
            var requestLanguage = languages.FirstOrDefault(language =>
                language.LanguageCulture.Equals(requestCulture.Culture.Name, StringComparison.OrdinalIgnoreCase));

            //check language availability
            if (requestLanguage == null || !requestLanguage.Published)
            {
                return null;
            }

            return requestLanguage;
        }

        private static LanguageModel PrepareLanguageModel(Language language)
        {
            return new LanguageModel
            {
                Id = language.Id,
                LanguageCulture = language.LanguageCulture,
                Name = language.Name,
                UniqueSeoCode = language.UniqueSeoCode
            };
        }

        private string GenerateAccessToken(User user, int? impersonateFromUserId = null, DateTime? expires = null)
        {
            return _authenticationService.GenerateAccessToken(user, user.CustomerId, expires, impersonateFromUserId);
        }


        #endregion

        #region Public
        /// <summary>
        /// Gets either current user or a specific user.
        /// GET api/User/id?
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCurentUser()
        {
            var currentUserModel = new UserModel(_workContext.CurrentUser);
            if (currentUserModel != null)
                return Ok(currentUserModel);

            // Ending here means bad things happened
            return BadRequest(_workContext);
        }

        /// <summary>
        /// Gets either current user or a specific user.
        /// GET api/User/id?
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUserById(int id, bool inclDeleted = false)
        {
            //Specific user is requested - this requires access validation!
            if (_workContext.IsUserSuperAdmin() ||
            (_permissionService.DoesUserHaveRole(_workContext.CurrentUser.Id, UserRolesEnum.CustomerSetup) && _userService.CanUserAccessUser(_workContext.CurrentUser.Id, id)))
            {
                // find the user with the id
                var foundUser = _userService.Get(id, inclDeleted);
                if (foundUser != null)
                {
                    return Ok(_userfactory.PrepareUserModel(foundUser));
                }
            }

            // Ending here means bad things happened
            return BadRequest(_workContext);
        }


        /// <summary>
        /// Returns all roles that a user has.
        /// </summary>
        /// <param name="userId">Optional user ID. If not passed, current user's id will be used</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUserRoles(int? userId)
        {
            var idOfUser = userId ?? _workContext.CurrentUser.Id;
            var roles = _permissionService.GetUserRoles(idOfUser);

            if (roles == null || !roles.Any())
            {
                //User does not have any roles
                return Ok(new List<UserRoleModel>());
            }

            //var roleModels = roles
            //                    .Select(x => _userfactory.PrepareUserRoleModel(x, _workContext.CurrentUser.LanguageId))
            //                    .OrderBy(u => u.NameLocalized)
            //                    .ToList();

            return Ok(new List<UserRoleModel>());
        }


        // -------------------------------------------------------------------------------
        /// <summary>
        /// Returns the language to be used for the current user.
        /// Must be anonymous as it's being used for the whole app and hereby also the iFrame modules
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public IActionResult GetWorkingLanguage()
        {
            var language = GetUserLanguage(_workContext.CurrentUser);
            return Ok(PrepareLanguageModel(language));
        }

        /// <summary>
        /// Get RefreshToken from a email and password
        /// </summary>
        /// <param name="passwordEmail"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginEmailPasswordModel passwordEmail)
        {

            if (passwordEmail is null)
                throw new ArgumentNullException(nameof(passwordEmail));

            var user = _userService.GetUserByEmail(passwordEmail.Email);

            var language = GetUserLanguage(user).Id;

            if (user != null)
            {
                // Check if User is locked out.
                if (_userService.IsUserLocked(user))
                {
                    return ForbidWithMessage($"User with e-mail {passwordEmail.Email} is locked because of too many login attempts");
                }

                // Check the User Credentials
                if (_authenticationService.GenerateSHA256Hash(passwordEmail.Password, user.PasswordSalt) == user.Password)
                {
                    var userRoleMappings = _userService.GetUserRoles(user.Id);

                    user.DateLastFailedLoginUtc = null;
                    user.FailedLoginCount = 0;
                    user.IsLockedOut = false;
                    user.DateLastLoginUtc = _utcClock.Now();
                    user.LanguageId = language;
                    _userService.Update(user);

                    var requestedAt = DateTime.UtcNow;
                    var refreshToken = _authenticationService.CreateRefreshtoken(user.Id);
                    var expiresIn = _authenticationService.GetAccessTokenExpirationTime();

                    int? impersonateUserID = null;

                    return Ok(new TokenModel
                    {
                        StateCode = 1,
                        RequestedAt = requestedAt,
                        ExpiresAt = expiresIn,
                        AccessToken = GenerateAccessToken(user, impersonateUserID, expires: expiresIn),
                        RefreshTokenModel = refreshToken,
                        UserId = user.Id,
                        CustomerId = user.CustomerId,
                        ImpersonateFromUserId = user.Id,
                    });

                }
                else
                {
                    _userService.CountFailedLoginTry(user);

                    return UnauthorizedWithMessage(_localizationService.GetLocalizedResource("errorMessages.LoginPassword", language));
                }
            }
            else
            {
                return UnauthorizedWithMessage(_localizationService.GetLocalizedResource("errorMessages.LoginEmail", language));
            }
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> LoginCookie()
        {
            if (Request.Cookies.TryGetValue("access_token", out string accessToken))
            {
                Response.Cookies.Delete("access_token");

                var userId = _authenticationService.ValidateAccessToken(accessToken);
                if (userId > 0)
                {
                    var user = _userService.Get(userId);

                    var language = GetUserLanguage(user).Id;
                    if (user != null)
                    {
                        // Check if User is locked out.
                        if (_userService.IsUserLocked(user))
                        {
                            return ForbidWithMessage($"User is locked because of too many login attempts");
                        }

                        var userRoleMappings = _userService.GetUserRoles(user.Id);

                        user.DateLastFailedLoginUtc = null;
                        user.FailedLoginCount = 0;
                        user.IsLockedOut = false;
                        user.DateLastLoginUtc = DateTime.UtcNow;
                        user.LanguageId = language;
                        _userService.Update(user);

                        var requestedAt = DateTime.UtcNow;
                        var refreshToken = _authenticationService.CreateRefreshtoken(user.Id);
                        var expiresIn = _authenticationService.GetAccessTokenExpirationTime();

                        int? impersonateUserID = null;

                        return Ok(new TokenModel
                        {
                            StateCode = 1,
                            RequestedAt = requestedAt,
                            ExpiresAt = expiresIn,
                            AccessToken = GenerateAccessToken(user, impersonateUserID, expires: expiresIn),
                            RefreshTokenModel = refreshToken,
                            UserId = user.Id,
                            CustomerId = user.CustomerId,
                            ImpersonateFromUserId = user.Id,
                        });
                    }
                    return UnauthorizedWithMessage(_localizationService.GetLocalizedResource("errorMessages.LoginUnknown", language));
                }
            }
            return BadRequest();
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> AuthenticateTwoFactor([FromBody] TwoFactorModel twoFactorModel)
        {
            if (twoFactorModel is null)
                throw new ArgumentNullException(nameof(twoFactorModel));

            var user = _userService.GetUserByEmail(twoFactorModel.Email);
            if (user != null)
            {
                var pinCodeIsValid = _authenticationService.ValidatePinCode(
                            twoFactorModel.PinCode,
                            user.ResetPhone,
                            twoFactorModel.Email,
                            DateTime.UtcNow.AddMinutes(-5));

                if (_userService.IsUserLocked(user))
                {
                    return ForbidWithMessage($"User with e-mail {user.Email} is locked because of too many login attempts");
                }

                if (pinCodeIsValid)
                {
                    var requestedAt = DateTime.UtcNow;
                    var refreshToken = _authenticationService.CreateRefreshtoken(user.Id);
                    var expiresIn = _authenticationService.GetAccessTokenExpirationTime();

                    int? impersonateUserID = null;

                    return Ok(new TokenModel
                    {
                        StateCode = 1,
                        RequestedAt = requestedAt,
                        ExpiresAt = expiresIn,
                        AccessToken = GenerateAccessToken(user, impersonateUserID, expires: expiresIn),
                        RefreshTokenModel = refreshToken,
                        UserId = user.Id,
                        CustomerId = user.CustomerId,
                        ImpersonateFromUserId = user.Id,
                    });
                }
                else
                {
                    _userService.CountFailedLoginTry(user);
                    return UnauthorizedWithMessage(_localizationService.GetLocalizedResource("errorMessages.LoginPassword", GetUserLanguage(user).Id));
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// Refresh access token and refresh token
        /// </summary>
        /// <param name="model">Refresh token issued at user login and profileId</param>
        /// <returns>TokenModel</returns>
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto model)
        {
            if (model == null)
            {
                return UnauthorizedWithMessage($"RefreshTokenDto is missing");
            }

            if (string.IsNullOrWhiteSpace(model.RefreshToken))
            {
                return UnauthorizedWithMessage($"RefreshToken is missing from dto");
            }

            // Compare token with the one in db
            var refreshToken = _authenticationService.GetUserRefreshToken(model.RefreshToken, false);
            if (refreshToken == null)
            {
                return UnauthorizedWithMessage($"RefreshToken '{model.RefreshToken}' is invalid!");
            }
            else if (refreshToken.DateExpiresUtc <= DateTime.UtcNow)
            {
                return UnauthorizedWithMessage($"RefreshToken '{model.RefreshToken}' has expired {Math.Floor((DateTime.UtcNow - refreshToken.DateExpiresUtc).TotalSeconds)} seconds ago!");
            }
            else
            {
                var specificProfile = false;
                var tokenUser = _userService.Get(refreshToken.UserId);
                if (tokenUser != null)
                {
                    var accesTokenUser = tokenUser;

                    var profileUserID = refreshToken.UserId;

                    //ONLY BilligKwh customers can impersonate!(Id 1 and 9 are the db id's of existing Blue Idea customers)
                    if (tokenUser.CustomerId != 1)
                    {
                        //we are not BilligKwh - can't impersonate so stil : profileUserID = token.UserId;
                    }
                    // we are BilligKwh, and perhaps impersonated, and accesstoken is valid
                    else if (_workContext.CurrentUserId > -1)
                    {
                        profileUserID = _workContext.CurrentUserId;
                    }
                    // we are BilligKwh, and was perhaps impersonated but accesstoken is no longer valid so we issue er new accesstoken
                    else if (model.UserId > 0)
                    {
                        profileUserID = model.UserId;
                    }

                    if (profileUserID > 0 && profileUserID != refreshToken.UserId)
                    {
                        accesTokenUser = _userService.Get(profileUserID);
                    }

                    var requestedAt = DateTime.UtcNow;
                    var expiresIn = _authenticationService.GetAccessTokenExpirationTime();
                    var accessToken = _authenticationService.GenerateAccessToken(accesTokenUser, accesTokenUser.CustomerId, expiresIn, refreshToken.UserId);

                    _authenticationService.ExtendRefreshToken(refreshToken, requestedAt);

                    return Ok(new TokenModel
                    {
                        StateCode = 1,
                        RequestedAt = requestedAt,
                        ExpiresAt = expiresIn,
                        AccessToken = accessToken,
                        RefreshTokenModel = refreshToken,
                        UserId = accesTokenUser.Id,
                        CustomerId = accesTokenUser.CustomerId,
                        ImpersonateFromUserId = refreshToken.UserId,
                    });
                }
                else
                {
                    return UnauthorizedWithMessage("User not valid");
                }
            }
        }


        /// <summary>
        /// Generates a unique "reset password token" and sends this to the email. Returns true if success, false if not.
        /// </summary>
        /// <param name="email">Email of the user</param>
        [HttpPost, AllowAnonymous]
        public IActionResult RequestResetPasswordToken(string email)
        {

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("No email specified");
            }

            var user = _userService.GetUserByEmail(email.Trim());
            if (user != null)
            {
                var mailTemplate = _emailTemplateService.GetTemplateByNameEnum(user.LanguageId, EmailTemplateName.ResetPassword);

                user.PasswordResetToken = Guid.NewGuid();
                user.DatePasswordResetTokenExpiresUtc = DateTime.UtcNow.AddHours(1);
                _userService.Update(user);

                var url = $"{Url.ActionContext.HttpContext.Request.Scheme}://{Url.ActionContext.HttpContext.Request.Host}/reset-password?token={user.PasswordResetToken.ToString()}&email={user.Email}";

                var fields = new List<KeyValuePair<string, object>>()
                            {
                                new KeyValuePair<string, object>("RESET_URL", url),
                                new KeyValuePair<string, object>("USERNAME", user.Name),
                            };

                _emailTemplateService.MergeEmailFields(mailTemplate, fields);

                _emailService.Save(
                    customerId: user.CustomerId,
                    fromEmail: mailTemplate.FromEmail,
                    fromName: mailTemplate.FromName,
                    sendTo: user.Email,
                    sendToName: user.Name,
                    replyTo: mailTemplate.ReplyTo,
                    subject: mailTemplate.Subject,
                    body: mailTemplate.Html,
                    categoryEnum: EmailCategoryEnum.PasswordMails,
                    refTypeID: null,
                    refID: null,
                    mailTemplate.BccEmails);

                return Ok(true);
            }

            return BadRequest("User not found with email: " + email);
        }

        /// <summary>
        /// Verifies a unique "reset password token". Email must be the one that the token was sent to. Returns true if validation succeeds and false otherwise.!--
        /// </summary>
        /// <param name="token">The unique reset password token</param>
        /// <param name="email">Email of the user</param>
        [HttpPost, AllowAnonymous]
        public IActionResult VerifyResetPasswordToken(string token, string email)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Missing token parameter");
            }

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Missing email parameter");
            }

            var user = _userService.GetUserByEmail(email.Trim());
            if (user != null)
            {
                if (!user.PasswordResetToken.HasValue || user.PasswordResetToken.Value.ToString() != token.Trim())
                {
                    return BadRequest("Token invalid");
                }

                var expireDate = user.DatePasswordResetTokenExpiresUtc.GetValueOrDefault();
                if (expireDate < DateTime.UtcNow)
                {
                    return BadRequest("Token expired");
                }

                //If we got this far, then the Token is VALID!
                return Ok(true);
            }
            return BadRequest("User not found with email " + email);
        }

        /// <summary>
        /// User logout
        /// </summary>
        [HttpPost]
        public IActionResult Logout()
        {
            if (_workContext.IsLoggedIn)
            {
                _authenticationService.Logout(_workContext.CurrentUser.Id);
            }
            else
            {
                return BadRequest("User not found");
            }

            return Ok();
        }

        /// <summary>
        /// Change the password of the user (If IsImpersonating, then we are SuperAdmin, and can change password without knowing the old password)
        /// </summary>
        /// <param name="newPassword"></param>
        [HttpPost]
        public IActionResult ChangePassword(string newPassword, string currentPassword)
        {
            var user = _workContext.CurrentUser;
            if (_workContext.IsImpersonating || _authenticationService.GenerateSHA256Hash(currentPassword, user.PasswordSalt) == user.Password || _workContext.IsUserSuperAdmin())
            {
                if (_authenticationService.IsPasswordValid(newPassword))
                {
                    var result = _authenticationService.SetNewPassword(newPassword, user);
                    if (result)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest("Could not change password " + newPassword);
                    }
                }
                else
                {
                    return BadRequest("New Password invalid " + newPassword);
                }
            }
            else
            {
                return BadRequest(new { ErrorMessage = "Current Password invalid.", CurrentUser = user });
            }
        }

        /// <summary>
        /// Used for requesting a new email with link for cresting password for the user with given email. If everything goes well,
        /// it will send out an email.
        /// </summary>
        [HttpGet, AllowAnonymous]
        public IActionResult ResendPasswordEmail(string userEmail)
        {
            if (userEmail is null)
                throw new ArgumentNullException(nameof(userEmail));

            if (!Core.Toolbox.Tools.ValidateEmail(userEmail.Trim()))
                return BadRequest("Email not valid");

            var user = _userService.GetUserByEmail(userEmail);

            if (user == null)
                return BadRequest("User doesn't exist");

            // Send mail to new user with link to activate login
            SendUserNewPasswordEmail(user);

            return Ok();
        }

        /// <summary>
        /// Resets user's password if token and email are both valid AND the new specified password is valid and equal to the repeated new password.
        ///These values are found in the passed model object. Returns a new refresh token on success.
        /// </summary>
        /// <param name="resetPassModel"></param>
        [HttpPost, AllowAnonymous]
        public IActionResult ResetPassword([FromBody] ResetPasswordModel resetPassModel)
        {
            if (resetPassModel == null)
            {
                return BadRequest("No reset password data object");
            }

            if (!string.Equals(resetPassModel.NewPassword, resetPassModel.NewPasswordRepeat, StringComparison.Ordinal))
            {
                return BadRequest(new { ErrorMessage = "New password not matching repeated password", Model = resetPassModel });
            }

            if (!_authenticationService.IsPasswordValid(resetPassModel.NewPassword))
            {
                return BadRequest(new { ErrorMessage = "New password not strong enough", Model = resetPassModel });
            }

            if (string.IsNullOrEmpty(resetPassModel.Token))
            {
                return BadRequest(new { ErrorMessage = "Missing token parameter", Model = resetPassModel });
            }
            if (string.IsNullOrEmpty(resetPassModel.Email))
            {
                return BadRequest(new { ErrorMessage = "Missing email parameter", Model = resetPassModel });
            }

            var user = _userService.GetUserByEmail(resetPassModel.Email.Trim());
            if (user != null)
            {
                if (!user.PasswordResetToken.HasValue || user.PasswordResetToken.Value.ToString() != resetPassModel.Token.Trim())
                {
                    return BadRequest(new { ErrorMessage = "Token invalid", User = user, Model = resetPassModel });
                }

                var expireDate = user.DatePasswordResetTokenExpiresUtc.GetValueOrDefault();

                if (expireDate < DateTime.UtcNow)
                {
                    return BadRequest(new { ErrorMessage = "Token expired", User = user, Model = resetPassModel });
                }

                //If we got this far, then the Token is VALID!
                user.PasswordResetToken = null;
                user.DatePasswordResetTokenExpiresUtc = null;

                //Generate new Password
                string salt = _authenticationService.CreateSalt();
                user.Password = _authenticationService.GenerateSHA256Hash(resetPassModel.NewPassword, salt);
                user.PasswordSalt = salt;
                user.IsLockedOut = false;
                user.FailedLoginCount = 0;

                //Save it all
                _userService.Update(user);
                var refreshToken = _authenticationService.CreateRefreshtoken(user.Id);
                return Ok(new
                {
                    refreshToken = refreshToken.Token
                });
            }

            return BadRequest(new { ErrorMessage = "User not found", Model = resetPassModel });
        }

        ///// <summary>
        ///// Set basic user information from Users Own Page.
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public IActionResult SetUserInformation([FromBody] UserInfoModel model)
        //{
        //    if (model is null)
        //        throw new ArgumentNullException(nameof(model));

        //    if (string.IsNullOrEmpty(model.Email))
        //        return BadRequest("The user must have an e-mail address");

        //    // Validate Email
        //    var emailResult = EmailAddress.Create(model.Email);
        //    if (emailResult.IsSuccess)
        //    {
        //        // Validate PhoneNumber
        //        var phoneResult = PhoneNumber.Create(model.MobileNumber);

        //        if (!string.IsNullOrEmpty(model.MobileNumber) && phoneResult.IsFailure)
        //            return BadRequest("Mobile number is not valid");

        //        var user = _userService.Get(_workContext.CurrentUser.Id);
        //        if (user != null)
        //        {
        //            var updatedUser = user.UpdateFromForm(
        //                    name: model.Name,
        //                    email: emailResult.Value.Email,
        //                    mobileNumber: string.IsNullOrEmpty(model.MobileNumber) ? 0 : phoneResult.Value.Number,
        //                    languageId: model.LanguageId
        //                );

        //            if (updatedUser.IsSuccess)
        //            {
        //                _userService.Update(updatedUser.Value);
        //                return Ok();
        //            }
        //        }

        //    }
        //    return BadRequest();
        //}

        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDto))]
        //[ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDto))]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public IActionResult Update([FromBody] UserModel model)
        //{
        //    if (model == null) return BadRequest("Model is null");
        //    if (!_workContext.IsUserSuperAdmin())
        //    {
        //        return ForbidWithMessage("User must have role ManageUser or SubscriptionModule");
        //    }

        //    if (model.Id > 0)
        //    {
        //        var user = _userService.Get(model.Id);

        //        bool isSuperAdmin = PermissionService.DoesUserHaveRole(_workContext.CurrentUser.Id, UserRolesEnum.SuperAdmin);

        //        // Only allow this if current user is the one to update. If not, user must be super admin
        //        if (user != null)
        //        {
        //            if (_workContext.CurrentUser.Id != user.Id && !isSuperAdmin)
        //                return ForbidWithMessage("User must be super admin to update a different user than current!");

        //            user.Name = model.Name;
        //            user.CountryId = model.CountryId;
        //            user.LanguageId = model.LanguageId;
        //            user.DateLastUpdatedUtc = DateTime.UtcNow;

        //            if (model.Deleted && !user.Deleted)
        //            {
        //                user.DateDeletedUtc = DateTime.UtcNow;
        //                user.Deleted = true;
        //            }
        //            else if (user.Deleted && !model.Deleted)
        //            {
        //                user.DateDeletedUtc = null;
        //                user.Deleted = false;
        //            }

        //            _userService.Update(user);

        //            return Ok(user.Id);
        //        }
        //        return BadRequest(new { ErrorMessage = "User not found", Model = user });
        //    }
        //    else
        //    {
        //        var user = _userfactory.CreateUserEntity(model);
        //        _userService.InsertUser(user);
        //        return Ok(user.Id);
        //    }
        //}





        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDto))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult UpdateUser([FromBody] UserEditModel model)
        {
            if (model == null) return BadRequest("Model is null");
            //if (!_workContext.IsUserSuperAdmin())
            //{
            //    return ForbidWithMessage("User must have role ManageUser or SubscriptionModule");
            //}

            if (model.Id > 0)
            {
                var bruger = _userService.Get(model.Id);


                bool isSuperAdmin = _permissionService.DoesUserHaveRole(_workContext.CurrentUser.Id, UserRolesEnum.SuperAdmin);

                // Only allow this if current user is the one to update. If not, user must be super admin
                if (bruger != null)
                {
                    if (_workContext.CurrentUser.Id != bruger.Id && !isSuperAdmin)
                        return ForbidWithMessage("User must be super admin to update a different user than current!");

                    if (bruger.Email != model.Email)
                    {
                        if (_userService.UserExists(model.Email))
                            return BadRequest("Email already exists");

                        bruger.Email = model.Email;
                    }

                    if (bruger.Adgangskode != model.NewPassword)
                    {
                        bruger.Adgangskode = model.NewPassword;
                        _authenticationService.SetNewPassword(model.NewPassword, bruger);
                    }

                    bruger.Name = model.Name;
                    bruger.Phone = model.Phone;
                    bruger.LanguageId = model.LanguageId;
                    bruger.CountryId = model.CountryId;
                    bruger.Deleted = model.Deleted;
                    bruger.Administrator = model.Administrator;
                    bruger.SetTidzoneId(bruger.CountryId);

                    _userService.Update(bruger);
                    return Ok(bruger.Id);
                }
                return BadRequest(new { ErrorMessage = "User not found", Model = bruger });
            }
            else
            {
                if (_userService.UserExists(model.Email))
                {
                    return BadRequest("Email already exists");
                }

                var customer = _customerService.Get(_workContext.CustomerId);

                if (_workContext.IsUserSuperAdmin())
                {
                    customer = _customerService.Get(model.CustomerId);
                }

                var user = _userfactory.CreateUserEntity(model, customer);
                _userService.Create(user);
                _authenticationService.SetNewPassword(model.NewPassword, user);
                return Ok(user.Id);
            }
        }

        ///// <summary>
        ///// Update a Users Basic Info from Customer or SuperAdmin Pages
        ///// </summary>
        ///// <returns></returns>
        //[HttpPatch]
        //public IActionResult UpdateUserInformation([FromBody] BasicUserInfoModel model)
        //{
        //    if (model is null) throw new ArgumentNullException(nameof(model));

        //    if (_workContext.IsUserSuperAdmin() ||
        //        (PermissionService.DoesUserHaveRole(_workContext.CurrentUser.Id, UserRolesEnum.CustomerSetup) && _userService.CanUserAccessUser(_workContext.CurrentUser.Id, model.Id)))
        //    {
        //        var emailResult = EmailAddress.Create(model.Email);
        //        if (emailResult.IsSuccess)
        //        {
        //            var phoneGood = model.MobileNumber.HasValue ? PhoneNumber.Create(model.MobileNumber.Value).IsSuccess : false;
        //            var user = _userService.Get(model.Id);
        //            if (user != null)
        //            {
        //                if (CheckForUserRole(UserRolesEnum.Protected, user.Id) && !_workContext.IsUserSuperAdmin())
        //                {
        //                    return ForbidWithMessage($"User is protected");
        //                }

        //                if (!string.IsNullOrEmpty(model.NewPassword))
        //                {
        //                    if (!_workContext.IsUserSuperAdmin())
        //                    {
        //                        return ForbidWithMessage("Only super admin can change password");
        //                    }

        //                    if (_authenticationService.IsPasswordValid(model.NewPassword))
        //                    {
        //                        if (!_authenticationService.SetNewPassword(model.NewPassword, user))
        //                        {
        //                            return BadRequest("Password could not be changed");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        return BadRequest("Invalid new password");
        //                    }
        //                }

        //                var updatedUser = user.UpdateFromForm(
        //                name: model.Name,
        //                email: emailResult.Value.Email,
        //                phoneNumber: model.MobileNumber
        //                );
        //                if (updatedUser.IsSuccess)
        //                {
        //                    _userService.Update(updatedUser.Value);
        //                    return Ok();
        //                }
        //            }
        //        }

        //    }
        //    return Forbid($"Current user cannot access user with Id {model.Id}");
        //}


        /// <summary>
        /// et roles for a user.
        /// </summary>
        /// <param name="hasAccess">Wether user should have this role or not </param>
        /// <returns></returns>
        [HttpPut, Authorize(UserRolePermissionProvider.Bearer)]
        public IActionResult SetUserRoleAccess(int userId, [FromBody] List<short> roleIds, bool hasAccess)
        {
            // kun kunderolle må
            if (!_workContext.IsUserSuperAdmin() && !CheckForUserRole(UserRolesEnum.ManageUsers, _workContext.CurrentUser.Id))
            {
                return ForbidWithMessage("Current user mus have role ManageUsers in order to do this");
            }
            var user = _userService.Get(userId);
            if (user != null)
            {
                if (CheckForUserRole(UserRolesEnum.Protected, user.Id) && !_workContext.IsUserSuperAdmin())
                {
                    return ForbidWithMessage("User is protected");
                }

                _permissionService.SetUserRoleAccess(_workContext.CurrentUser.Id, _workContext.CustomerId, userId, roleIds, hasAccess);
                return Ok();
            }
            else
            {
                return BadRequest("User not found");
            }
        }

        /// <summary>
        /// Common method for generating and sending an email containing link for creating a new password for a user.
        /// </summary>
        /// <param name="user">The user for which a new password is requested</param>
        /// <param name="sendToEmails">String being a semicolon-separated list of emails (or just 1 email)</param>
        [ApiExplorerSettings(IgnoreApi = true)]
        public void SendUserNewPasswordEmail(User user)
        {
            // Send mail to new user with link to activate login
            var mailTemplate = _emailTemplateService.GetTemplateByNameEnum(user.LanguageId, EmailTemplateName.NewUser);

            user.PasswordResetToken = _globallyUniqueIdentifier.NewGuid();
            user.DatePasswordResetTokenExpiresUtc = _utcClock.Now().AddHours(1);
            _userService.Update(user);

            var url = _userUrlGenerator.GetNewPasswordUrl(user, Url);
            var fields = new List<KeyValuePair<string, object>>()
                        {
                            new KeyValuePair<string, object>("NEWUSER_URL", url.ToString()),
                            new KeyValuePair<string, object>("USERNAME", user.Name),
                        };

            _emailTemplateService.MergeEmailFields(mailTemplate, fields);

            _emailService.Save(
                    customerId: user.CustomerId,
                    fromEmail: mailTemplate.FromEmail,
                    fromName: mailTemplate.FromName,
                    sendTo: user.Email,
                    sendToName: user.Name,
                    replyTo: mailTemplate.ReplyTo,
                    subject: mailTemplate.Subject,
                    body: mailTemplate.Html,
                    refTypeID: null,
                    refID: null,
                    categoryEnum: EmailCategoryEnum.SupportMails);
        }

        #endregion


        #region SuperAdmin

        /// <summary>
        /// Reset failed login and Unlock user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ResetFailedLoginAndUnlock(int userId)
        {
            if (!_workContext.IsUserSuperAdmin() && !CheckForUserRole(UserRolesEnum.ManageUsers, _workContext.CurrentUser.Id))
            {
                return ForbidWithMessage("User is missing role ManageUsers");
            }
            var user = _userService.Get(userId);
            if (user == null)
            {
                return BadRequest(new { ErrorMessage = "User not found", UserId = userId });
            }
            _userService.UnlockLogIn(user);
            return Ok();
        }

        /// <summary>
        /// Update a Users Basic Info
        /// </summary>
        /// <returns></returns>
        [HttpPatch]
        public IActionResult UpdateUserFromEditModel([FromBody] UserEditModel model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            if (_workContext.IsUserSuperAdmin() ||
                (_permissionService.DoesUserHaveRole(_workContext.CurrentUser.Id, UserRolesEnum.CustomerSetup) && _userService.CanUserAccessUser(_workContext.CurrentUser.Id, model.Id)))
            {
                var emailResult = EmailAddress.Create(model.Email);
                if (emailResult.IsSuccess)
                {
                    var user = _userService.Get(model.Id);
                    if (user != null)
                    {
                        if (CheckForUserRole(UserRolesEnum.Protected, user.Id) && !_workContext.IsUserSuperAdmin())
                        {
                            return ForbidWithMessage($"User is protected");
                        }

                        if (!string.IsNullOrEmpty(model.NewPassword))
                        {
                            //if (!_workContext.IsUserSuperAdmin())
                            //{
                            //    return ForbidWithMessage("Only super admin can change password");
                            //}

                            if (_authenticationService.IsPasswordValid(model.NewPassword))
                            {
                                if (!_authenticationService.SetNewPassword(model.NewPassword, user))
                                {
                                    return BadRequest("Password could not be changed");
                                }
                            }
                            else
                            {
                                return BadRequest("Invalid new password");
                            }
                        }

                        if (model.Email != user.Email)
                        {
                            if (_userService.UserExists(model.Email))
                            {
                                return BadRequest("Email already exists");
                            }
                            user.Email = model.Email;
                        }

                        user.Name = model.Name;
                        user.Phone = model.Phone;
                        user.LanguageId = model.LanguageId;
                        user.Administrator = model.Administrator;
                        user.CountryId = model.CountryId;
                        user.Deleted = model.Deleted;

                        _userService.Update(user);
                        return Ok();
                    }
                }
            }
            return Forbid($"Current user cannot access user with Id {model.Id}");
        }

        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDto))]
        //[ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDto))]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public IActionResult UpdateUser([FromBody] UserModel model, bool sendEmail = true)
        //{
        //    if (model == null) return BadRequest("Model is null");
        //    if (!_workContext.IsUserSuperAdmin())
        //    {
        //        return ForbidWithMessage("User must have role ManageUser or SubscriptionModule");
        //    }

        //    if (model.Id > 0)
        //    {
        //        var bruger = _userService.Get(model.Id);

        //        bool isSuperAdmin = PermissionService.DoesUserHaveRole(_workContext.CurrentUser.Id, UserRolesEnum.SuperAdmin);

        //        // Only allow this if current user is the one to update. If not, user must be super admin
        //        if (bruger != null)
        //        {
        //            if (_workContext.CurrentUser.Id != bruger.Id && !isSuperAdmin)
        //                return ForbidWithMessage("User must be super admin to update a different user than current!");



        //            //             public string Email { get; set; }
        //            //public string Firstname { get; set; }
        //            //public string Lastname { get; set; }
        //            //public string Department { get; set; }
        //            //public string Phone { get; set; }
        //            //public string Mobile { get; set; }
        //            //public int LanguageId { get; set; }
        //            //public bool Slettet { get; set; }
        //            //public int CountryId { get; set; }
        //            //public string TimezoneId { get; set; }
        //            //public long? ResetPhone { get; set; }










        //            //bruger.Email = model.Email;
        //            //bruger.Adgangskode = model.Adgangskode;
        //            //bruger.EmailUtfCode = model.EmailUtfCode;
        //            //bruger.EmailUnicode = model.EmailUnicode;
        //            //bruger.CustomerId = model.CustomerId;
        //            //bruger.CustomerId = model.CustomerId;
        //            //bruger.ErAdministrator = model.ErAdministrator;
        //            //bruger.SystemAdministrator = model.SystemAdministrator;
        //            //bruger.KundeAdministrator = model.KundeAdministrator;
        //            bruger.Fornavn = model.Firstname;
        //            bruger.Efternavn = model.Lastname;
        //            //bruger.Afdeling = model.Afdeling;
        //            //bruger.Telefon = model.Telefon;
        //            //bruger.Mobil = model.Mobil;
        //            //bruger.NoLogin = model.NoLogin;
        //            //bruger.RemoteIp = model.RemoteIp;
        //            //bruger.LanguageId = model.LanguageId;
        //            //bruger.OploesningVandret = model.OploesningVandret;
        //            //bruger.OploesningLodret = model.OploesningLodret;
        //            //bruger.UserAgent = model.UserAgent;
        //            //bruger.Browser = model.Browser;
        //            //bruger.Version = model.Version;
        //            //bruger.VarslingSMS = model.VarslingSMS;
        //            //bruger.SidstRettet = model.SidstRettet;
        //            //bruger.SidstRettetAfBrugerID = model.SidstRettetAfBrugerID;
        //            //bruger.Slettet = model.Slettet;
        //            //bruger.ErHelpdeskBCC = model.ErHelpdeskBCC;
        //            //bruger.StandardBedriftID = model.StandardBedriftID;
        //            //bruger.Name = model.Name;
        //            //bruger.SecurityStamp = model.SecurityStamp;
        //            //bruger.PasswordHash = model.PasswordHash;
        //            //bruger.PortalAdministrator = model.PortalAdministrator;
        //            //bruger.PersonDataAcceptDato = model.PersonDataAcceptDato;
        //            bruger.LandID = model.CountryId;
        //            //bruger.Password = model.Password;
        //            //bruger.PasswordSalt = model.PasswordSalt;
        //            //bruger.FailedLoginCount = model.FailedLoginCount;
        //            //bruger.IsLockedOut = model.IsLockedOut;
        //            //bruger.DateLastFailedLoginUtc = model.DateLastFailedLoginUtc;
        //            //bruger.DateLastLoginUtc = model.DateLastLoginUtc;
        //            //bruger.PasswordResetToken = model.PasswordResetToken;
        //            //bruger.DatePasswordResetTokenExpiresUtc = model.DatePasswordResetTokenExpiresUtc;
        //            //bruger.PersonatingUserId = model.PersonatingUserId;
        //            //bruger.TidzoneId = model.TidzoneId;
        //            //bruger.ResetPhone = model.ResetPhone;

        //            //bruger.Name = model.Name;
        //            //bruger.Email = model.Email;
        //            //bruger.CountryId = model.CountryId;
        //            //bruger.LanguageId = model.LanguageId;
        //            //bruger.DateLastUpdatedUtc = DateTime.UtcNow;

        //            //if (model.Deleted && !bruger.Deleted)
        //            //{
        //            //    bruger.DateDeletedUtc = DateTime.UtcNow;
        //            //    bruger.Deleted = true;
        //            //}
        //            //else if (bruger.Deleted && !model.Deleted)
        //            //{
        //            //    bruger.DateDeletedUtc = null;
        //            //    bruger.Deleted = false;
        //            //}

        //            bruger.SetTidzoneId(bruger.LandID);

        //            _userService.Update(bruger);
        //            return Ok(bruger.Id);
        //        }
        //        return BadRequest(new { ErrorMessage = "User not found", Model = bruger });
        //    }
        //    else
        //    {
        //        int customerId = _workContext.CustomerId;

        //        bool isSuperAdmin = PermissionService.DoesUserHaveRole(_workContext.CurrentUser.Id, UserRolesEnum.SuperAdmin);

        //        if (isSuperAdmin && model.VaertKundeID > 0)
        //            customerId = model.VaertKundeID;

        //        var customer = _customerService.Get(customerId);

        //        if (customer == null)
        //        {
        //            return BadRequest("Customer does not exist");
        //        }

        //        var user = new Bruger().InitForCreate(
        //       model.Firstname,
        //       model.Lastname,
        //       customer,
        //       model.Email
        //       //model.ResetPhone.HasValue && model.ResetPhone != 0 ? model.ResetPhone : null
        //       );

        //        _userService.Create(user);

        //        //_customerService.AddUserToCustomer(customer.Id, user.Id);

        //        if (sendEmail) SendUserCreationEmail(user);

        //        return Ok(user.Id);
        //    }
        //}


        /// <summary>
        /// Used for requesting a new email with link for cresting password for the user with given email. If everything goes well,
        /// it will send out an email.
        /// </summary>
        [HttpGet, Authorize(UserRolePermissionProvider.SuperAdmin)]
        public IActionResult SendNewUserEmail(int userId)
        {
            var user = _userService.Get(userId);

            if (user == null)
                return BadRequest("User doesn't exist");

            // Send mail to new user with link to activate login
            SendNewUserEmail(user);

            return Ok();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void SendNewUserEmail(User user)
        {
            EmailTemplate mailTemplate = null;

            if (user.Administrator)
                mailTemplate = _emailTemplateService.GetTemplateByNameEnum(user.LanguageId, EmailTemplateName.AdminOprettelse);
            else
                mailTemplate = _emailTemplateService.GetTemplateByNameEnum(user.LanguageId, EmailTemplateName.BrugerOprettelse);

            var fields = new List<KeyValuePair<string, object>>()
                        {
                            new KeyValuePair<string, object>("NAME", user.Name),
                            new KeyValuePair<string, object>("USERNAME", user.Email),
                            new KeyValuePair<string, object>("PASSWORD", user.Password),
                        };

            _emailTemplateService.MergeEmailFields(mailTemplate, fields);

            _emailService.Save(
                    customerId: user.CustomerId,
                    fromEmail: mailTemplate.FromEmail,
                    fromName: mailTemplate.FromName,
                    sendTo: user.Email,
                    sendToName: user.Name,
                    replyTo: mailTemplate.ReplyTo,
                    subject: mailTemplate.Subject,
                    body: mailTemplate.Html,
                    refTypeID: null,
                    refID: null,
                    categoryEnum: EmailCategoryEnum.SupportMails);
        }

        /// <summary>
        /// Returns all users belonging to a specific customer
        /// </summary>
        [HttpGet, Authorize(UserRolePermissionProvider.SuperAdmin)]
        public IActionResult GetUsersByCustomer(int customerId, bool onlyDeleted = false, int? userId = null)
        {
            var users = _userService.GetUsersByCustomer(customerId, onlyDeleted, userId);
            var result = users.Select(u => _userfactory.PrepareUserModel(u));
            return Ok(result.OrderBy(u => u.Name));
        }

        /// <summary>
        /// For "masking" current user behind a different user and hereby make working context "think" that a different user is logged in.
        /// If user is already impersonating a different user, then we just switch to the other user (e.i. you don't have to unimpersonate a user first).
        /// </summary>
        [HttpGet, Authorize(UserRolePermissionProvider.SuperAdmin)]
        public async Task<IActionResult> ImpersonateUser(ImpersonationDto model)
        {
            if (_workContext.IsImpersonating)
            {
                return BadRequest("You are already impersonated");
            }

            if (_workContext.CurrentUser.Id == model.UserId)
            {
                return BadRequest("You cannot impersonate yourself");
            }

            var userToImpersonate = _userService.Get(model.UserId);

            if (userToImpersonate == null || userToImpersonate.Deleted)
            {
                return BadRequest(userToImpersonate == null ? "User to impersonate was not found" : "User to impersonated is deleted");
            }

            // Make sure that the current profile is updated. Get the profiles and set current as the first one that user has access to


            var requestedAt = DateTime.UtcNow;
            var expiresIn = _authenticationService.GetAccessTokenExpirationTime();

            var refreshToken = _authenticationService.GetUserRefreshToken(model.RefreshToken, false);

            _authenticationService.ExtendRefreshToken(refreshToken, requestedAt);


            return Ok(new TokenModel
            {
                StateCode = 1,
                RequestedAt = requestedAt,
                ExpiresAt = expiresIn,
                AccessToken = GenerateAccessToken(userToImpersonate, _workContext.CurrentUser.Id, expiresIn),
                RefreshTokenModel = refreshToken,
                UserId = userToImpersonate.Id,
                CustomerId = userToImpersonate.CustomerId,
                ImpersonateFromUserId = _workContext.CurrentUser.Id,
            });

        }

        /// <summary>
        /// Returns a list of user role access models telling what roles the user has access to or not
        /// </summary>
        /// <param name="userId">If set, ALL the roles that a user can have are returned along with flag telling if this user has the role or not. If not provided, current user's id is used.</param>

        [HttpGet]
        public IActionResult GetUserRoleAccess(int? userId)
        {
            var user = userId.HasValue ? _userService.Get(userId.Value) : _workContext.CurrentUser;

            if (user != null)
            {
                var roles = _permissionService.GetUserRolesByUser(user.Id, _workContext.IsUserSuperAdmin());
                var mappings = _permissionService.GetUserRoleMappingsbyUser(user.Id);
                var access = _userfactory.PrepareUserRoleAccessModels(user.Id, roles.ToList(), mappings.ToList(), _workContext.CurrentUser.LanguageId);

                return Ok(access.OrderBy(ua => ua.UserRole.NameLocalized));
            }
            return userId.HasValue ? BadRequest("User not found. UserId " + userId) : BadRequest(_workContext);
        }

        ///<summary>
        /// Cancels impersonation mode and returns back to org. user
        ///</summary>
        [HttpGet]
        public async Task<IActionResult> CancelImpersonation()
        {
            if (_workContext.IsImpersonating)
            {
                var user = _userService.Get(_workContext.ImpersonateFromUserId);

                if (user != null)
                {
                    _workContext.CurrentUser = user;

                    var requestedAt = DateTime.UtcNow;
                    var refreshToken = _authenticationService.CreateRefreshtoken(user.Id);
                    var expiresIn = _authenticationService.GetAccessTokenExpirationTime();

                    return Ok(new TokenModel
                    {
                        StateCode = 1,
                        RequestedAt = requestedAt,
                        ExpiresAt = expiresIn,
                        AccessToken = GenerateAccessToken(user, user.ImpersonatingUserId, expires: expiresIn),
                        RefreshTokenModel = refreshToken,
                        UserId = user.Id,
                        CustomerId = user.CustomerId,
                        ImpersonateFromUserId = user.Id,
                    });
                }
            }
            return BadRequest("You are not impersonating any user");
        }
        #endregion

        protected bool CheckForUserRole(UserRolesEnum role, int userId)
        {
            return _permissionService.DoesUserHaveRole(userId, role);
        }

        protected bool CheckForProfileRole(ProfileRoleName role, int profileId)
        {
            return _permissionService.DoesProfileHaveRole(profileId, role);
        }
    }
}

