using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using BilligKwhWebApp.Services.Localization;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Core;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Services;
using BilligKwhWebApp.Services.Enums;
using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Infrastructure.DataTransferObjects.Common;
using BilligKwhWebApp.Models;
using BilligKwhWebApp.Extensions;
using System.Net.Mail;
using System.IO;

namespace BilligKwhWebApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(UserRolePermissionProvider.Bearer)]
    public class CommonController : BaseController
    {
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IAuthenticationService _authenticationService;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IWebHostEnvironment _environment;
        private readonly IIconService _iconService;
        private readonly ISystemLogger _logger;
        private readonly IBaseRepository _baseRepository;
        private readonly IEmailService _emailService;

        public CommonController(ISystemLogger logger,
                                ILocalizationService localizationService,
                                IHttpContextAccessor httpAccessor,
                                IWebHostEnvironment environment,
                                IWorkContext workContext,
                                IPermissionService permissionService,
                                IAuthenticationService authenticationService,
                                IIconService iconService, IBaseRepository baseRepository, IEmailService emailService) : base(logger, workContext, permissionService)
        {
            _localizationService = localizationService;
            _workContext = workContext;
            _authenticationService = authenticationService;
            _httpAccessor = httpAccessor;
            _environment = environment;
            _iconService = iconService;
            _logger = logger;
            _baseRepository = baseRepository;
            _emailService = emailService;
        }


        /// <summary>
        /// Verifies a Google Captcha user-response-token.
        /// Must be anonymous as it's being used for subscription modules
        /// </summary>
        /// <param name="token">Google Captcha user-response-token as returned by the frontend captcha component</param>
        /// <returns>boolean - true for success and false for... you guessed right</returns>
        [HttpGet, AllowAnonymous]
        public IActionResult ValidateGoogleCaptchaToken(string token, string pageNameId)
        {
            string ip;
            if (_environment.IsDevelopment())
            {
                ip = HttpContext.Connection.LocalIpAddress.ToString();
            }
            else
            {
                ip = HttpContext.Connection.RemoteIpAddress.ToString();
            }
            return Ok(_authenticationService.ValidateGoogleCaptchaToken(token, ip, pageNameId));
        }

        /// <summary>
        /// Increments the load count of a specific page
        /// </summary>
        /// <param name="PageNameId">Id of the page that is loaded</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult LoadSensitivePage(string PageNameId)
        {
            string IP = _httpAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            _authenticationService.LoadSensitivePage(IP, PageNameId);
            return Ok();
        }

        #region Localized string resources
        /// <summary>
        /// Returns all the localized resources as a correctly formated json object according to the translation tool used in frontend.
        /// Must be anonymous as it's being used for the whole and hereby also in iFrame modules
        /// </summary>
        /// <param name="languageId"></param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public IActionResult GetResourcesJson(int languageId)
        {
            var resources = _localizationService.GetAll(languageId);
            var resourcesObject = new Dictionary<string, string>();
            foreach (var item in resources)
            {
                if (!resourcesObject.ContainsKey(item.ResourceName))
                {
                    resourcesObject.Add(item.ResourceName, item.ResourceValue);
                }
            }
            return Ok(resourcesObject);
        }

        ///// <summary>
        ///// Returns a downloadable file as a "FileContentResult" object
        ///// </summary>
        ///// <param name="languageId">Optional language id if only resources for a specific language should be returned. Default null, meaning resources for all laguages are returned</param>
        ///// <returns>FileContentResult</returns>
        //[HttpGet, Authorize(UserRolePermissionProvider.SuperAdmin)]
        //public IActionResult DownloadResourcesExcel(int? languageId)
        //{
        //    var resources = localizationService.GetAll(languageId).Select(r => new TranslationRecord
        //    {
        //        ResourceName = r.ResourceName,
        //        ResourceValue = r.ResourceValue,
        //        Language = CountryConstants.GetCountryCode(r.LanguageId)
        //    });

        //    string excelFileName = localizationService.GetLocalizedResource("superAdministration.Translations.Translations", workContext.CurrentUser.LanguageId);
        //    return new ExcelReportDownloader<TranslationRecord>(this, localizationService).WriteToExcelSheet(excelFileName, resources, workContext.CurrentUser.LanguageId);

        //}

        [HttpPost, Authorize(UserRolePermissionProvider.SuperAdmin)]
        public IActionResult Resources(int languageId, [FromBody] LanguageResourcesDto model)
        {
            var query = _localizationService.GetAll(languageId).AsQueryable();

            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.SearchResourceName))
                {
                    query = query.Where(l => l.ResourceName.ToUpperInvariant().Contains(model.SearchResourceName.ToUpperInvariant(), StringComparison.Ordinal));
                }

                if (!string.IsNullOrEmpty(model.SearchResourceValue))
                {
                    query = query.Where(l => l.ResourceValue.ToUpperInvariant().Contains(model.SearchResourceValue.ToUpperInvariant(), StringComparison.Ordinal));
                }
            }

            var resources = query
               .Select(x => new LanguageResourceModel
               {
                   LanguageId = x.LanguageId,
                   Id = x.Id,
                   Name = x.ResourceName,
                   Value = x.ResourceValue.Replace("\\\\", "\\", StringComparison.OrdinalIgnoreCase),
               });

            var result = new DataTableResultModel
            {
                Total = resources.Count()
            };

            // If pagination should be used. Page size -1 means all resources should be reurned
            if (model != null && model.PageSize != -1) result.Data = resources.Paginate(model);
            else result.Data = resources; // return all

            return Ok(result);
        }

        [HttpPost, Authorize(UserRolePermissionProvider.SuperAdmin)]
        public IActionResult InsertLocalizedResource(string resourceName, string resourceValue, int languageId = 1)
        {
            if (string.IsNullOrEmpty(resourceName) || string.IsNullOrEmpty(resourceValue))
            {
                return BadRequest(new { ErrorMessage = "Missing parameter values", ResourceName = resourceName, ResourceValue = resourceValue });
            }

            var locale = new LocaleStringResource
            {
                LanguageId = languageId,
                ResourceName = resourceName.Trim(),
                ResourceValue = resourceValue
            };

            string existingResource = _localizationService.GetLocalizedResource(locale.ResourceName, locale.LanguageId);
            if (!string.IsNullOrEmpty(existingResource) && !string.Equals(existingResource, resourceName, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { ErrorMessage = "Resource already exists", Locale = locale });
            }

            _localizationService.InsertLocaleStringResource(locale);

            return Ok();
        }

        [HttpPost, Authorize(UserRolePermissionProvider.SuperAdmin)]
        public IActionResult DeleteLocalizedResource(int id)
        {
            if (id == 0)
            {
                return BadRequest("Id missing");
            }

            var locale = _localizationService.GetLocaleStringResourceById(id);
            if (locale == null)
            {
                return BadRequest($"Resource not found. {id}");
            }

            _localizationService.DeleteLocaleStringResource(locale);
            return Ok();
        }

        [HttpPost, Authorize(UserRolePermissionProvider.SuperAdmin)]
        public IActionResult UpdateLocalizedResource(int id, string resourceName, string resourceValue, int languageId)
        {
            if (id == 0)
            {
                return BadRequest("Id missing");
            }

            if (string.IsNullOrEmpty(resourceName) || string.IsNullOrEmpty(resourceValue))
            {
                return BadRequest("Missing parameter values");
            }

            var locale = _localizationService.GetLocaleStringResourceById(id);
            if (locale == null)
            {
                return BadRequest("Resource not found");
            }

            locale.ResourceName = resourceName.Trim();
            locale.ResourceValue = resourceValue;
            locale.LanguageId = languageId;
            _localizationService.UpdateLocaleStringResource(locale);
            return Ok();
        }


        #endregion

        [HttpGet]
        public IActionResult GetWebMessageMapModuleIcons(int? customerId)
        {
            //Getting icons cached by icon-type
            var icons = _iconService.GetIconsByType(IconTypeEnum.MapModule, customerId).Select(s => new Infrastructure.DataTransferObjects.Common.IconModel { Id = s.Id, Name = s.Name, Source = s.Url });
            return Ok(icons.OrderBy(i => i.Name));
        }

        /// <summary>
        /// Creates a Fatal Log - Used for testing Fatal log mail flow.
        /// Must be anonymous
        /// </summary>
        [HttpGet, AllowAnonymous]
        public IActionResult CreateFatalLog()
        {
            _logger.InsertLogAsync(LogLevel.Fatal, $"Dette er blot en test af en FATAL error - så alt er godt :-)");
            return Ok();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDto))]
        [ProducesDefaultResponseType]
        public ActionResult SendMail(int messageId)
        {
            SendTestMailFromBatchAppInDev(messageId);
            return Ok();
        }

        [HttpGet, Authorize(UserRolePermissionProvider.SuperAdmin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmailModel>))]
        public IActionResult GetEmails(int? customerId, DateTime fromDateUtc, DateTime toDateUtc)
        {
            var emails = _emailService.GetAll(customerId, fromDateUtc, toDateUtc);
            if (emails != null)
            {
                return Ok(emails.OrderByDescending(c => c.Id));
            }
            else
            {
                return Ok(new List<EmailModel>());
            }
        }

        [HttpGet, Authorize(UserRolePermissionProvider.SuperAdmin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ElprisModel>))]
        public IActionResult GetElpriser(DateTime fromDateUtc, DateTime toDateUtc)
        {
            var emails = _emailService.GetAllElpriser(fromDateUtc, toDateUtc);
            if (emails != null)
            {
                return Ok(emails.OrderByDescending(c => c.DatoUtc));
            }
            else
            {
                return Ok(new List<ElprisModel>());
            }
        }

        [HttpGet, Authorize(UserRolePermissionProvider.SuperAdmin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmailModel>))]
        public IActionResult GetTavleEmails(int tavleId)
        {
            var emails = _emailService.GetTavleEmails(tavleId);
            if (emails != null)
            {
                return Ok(emails.OrderByDescending(c => c.Id));
            }
            else
            {
                return Ok(new List<EmailModel>());
            }
        }


        /// <summary>
        /// To be used for testing purposes inn DEV, from batchapp
        /// </summary>
        /// <param name="messageId">Id of EmailMessage</param>
        /// <param name="mailAccount">Office 365 mail acount username</param>
        /// <param name="password">Office 365 mail acount password</param>
        private void SendTestMailFromBatchAppInDev(int messageId)
        {
            string mailAccount = "claus@blueIdea.dk";
            string password = "Flipper12#";

            var getMessagesSql = @"
                    SELECT *
					FROM dbo.EmailMessages
					WHERE Id = @MessageId";
            var getParam = new { MessageId = messageId };

            var emailsToSend = _baseRepository.Query<EmailMessage>(getMessagesSql, getParam);

            if (emailsToSend.Count() != 1) return;
            var mail = emailsToSend.Single();

            MailMessage msg = new(new MailAddress(mailAccount, "The sender"), new MailAddress(mailAccount, "The Recipient"))
            {
                Subject = mail.Subject,
                Body = mail.Body,
                IsBodyHtml = true
            };

            var getAttachmentSql = "SELECT * FROM dbo.EmailAttachments WHERE MessageId = @MessageId";
            var attachments = _baseRepository.Query<EmailAttachment>(getAttachmentSql, new { MessageId = mail.Id }).ToList();
            if (attachments != null && attachments.Any())
            {
                foreach (var attachment in attachments)
                {
                    msg.Attachments.Add(new Attachment(new MemoryStream(attachment.FileContent), attachment.FileName));
                }
            }

            SmtpClient client = new()
            {
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(mailAccount, password),
                Port = 587,
                Host = "smtp.office365.com",
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };
            client.Send(msg);
        }

    }
}