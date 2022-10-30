
using RestSharp;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core;
using BilligKwhWebApp.Core.Toolbox;
using BilligKwhWebApp.Services.Enums;

namespace BilligKwhWebApp.Services
{
    public class DefaultLogger : ISystemLogger
    {
        // Props
        private readonly IBaseRepository _baseRepository;
        private readonly ILogService _logService;
        private readonly IErrorEmailSender _errorEmailSender;
        private readonly IWebHelper _webHelper;
        private readonly IApplicationSettingService _applicationSettingService;

        // Ctor
        public DefaultLogger(IBaseRepository baseRepository, IWebHelper webHelper, ILogService logService, IApplicationSettingService applicationSettingService, IErrorEmailSender errorEmailSender)
        {
            _logService = logService;
            _applicationSettingService = applicationSettingService;
            _baseRepository = baseRepository;
            _webHelper = webHelper;
            _errorEmailSender = errorEmailSender;
        }

        // Public Api
        public virtual async Task<IPagedList<Log>> GetAllLogs(DateTime? fromUtc = null, DateTime? toUtc = null, string message = "", LogLevel? logLevel = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var parameters = new
            {
                DateFrom = fromUtc,
                DateTo = toUtc,
                SearchString = message,
                Loglevel = logLevel
            };

            var query = await _baseRepository.QueryStoredProdureAsync<Log>("dbo.GetLogs", parameters).ConfigureAwait(false);
            var logs = new PagedList<Log>(query.ToList(), pageIndex, pageSize);
            return logs;
        }

        public virtual int InsertLogAsync(LogLevel logLevel, string shortMessage, string fullMessage = "", User user = null, string module = null, object dataObject = null)
        {
            bool doLogReferrer = (logLevel > LogLevel.Information && _webHelper != null);

            string sDataObj;
            try
            {
                sDataObj = dataObject == null ? null : SimpleJson.SerializeObject(dataObject);
            }
            catch
            {
                sDataObj = dataObject == null ? null : $"{dataObject}";
            }

            try
            {
                // Insert Log
                var log = new Log
                {
                    LogLevelId = (int)logLevel,
                    ShortMessage = shortMessage.Truncate(1500, true),
                    FullMessage = fullMessage,
                    IpAddress = doLogReferrer ? _webHelper.GetCurrentIpAddress() : null,
                    UserId = user?.Id,
                    PageUrl = doLogReferrer ? _webHelper.GetThisPageUrl(true) : null,
                    ReferrerUrl = doLogReferrer ? _webHelper.GetUrlReferrer() : null,
                    DateCreatedUtc = DateTime.UtcNow,
                    Module = module,
                    DataObject = sDataObj,
                };

                _baseRepository.Insert(log);

                if (logLevel == LogLevel.Fatal)
                {
                    var liveCheck = _applicationSettingService.Get(AppSettingEnum.BatchAppCommandLine, "").Setting;

                    if (liveCheck.Contains("-live"))
                    {
                        try
                        {
                            var thread = new Thread(() =>
                            {
                                string body = $"Module:\n{log.Module ?? ""}\n\nShortMessage:\n{log.ShortMessage ?? ""}\n\nFullMessage:\n{(log.FullMessage ?? "")}\n\n<a href='https://billigkwh.dk/admin/super/log'>Åbn logs siden</a>".Replace("\n", "<br/>", StringComparison.InvariantCulture);
                                string subject = "A fatal error occurred";
                                if (log.Module != null)
                                    subject = subject + $" in {log.Module}";

                                var response = _errorEmailSender.SendErrorMail(subject, body);

                                if (response)
                                {
                                    // Success
                                }
                                // Error Handling
                                else
                                {
                                    //var responseBody = response.Body.ReadAsStringAsync().GetAwaiter().GetResult();

                                    var log = new Log
                                    {
                                        LogLevelId = (int)logLevel,
                                        ShortMessage = shortMessage.Truncate(1500, true),
                                        FullMessage = fullMessage,
                                        IpAddress = doLogReferrer ? _webHelper.GetCurrentIpAddress() : null,
                                        UserId = user?.Id,
                                        PageUrl = doLogReferrer ? _webHelper.GetThisPageUrl(true) : null,
                                        ReferrerUrl = doLogReferrer ? _webHelper.GetUrlReferrer() : null,
                                        DateCreatedUtc = DateTime.UtcNow,
                                        Module = module,
                                        DataObject = body,
                                    };
                                    _baseRepository.Insert(log);
                                }
                            });
                            thread.Start();
                            thread.Join();
                        }
                        catch
                        {
                            // do nothing
                        }
                    }
                }
                return log.Id;
            }
            catch
            {
                return -1;
            }
        }
    }

}
