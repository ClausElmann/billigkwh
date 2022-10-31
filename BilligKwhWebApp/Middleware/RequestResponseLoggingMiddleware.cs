using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Services;
using BilligKwhWebApp.Services.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BilligKwhWebApp.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IBaseRepository baseRepository, IApplicationSettingService applicationSettingService)
        {
            if (context == null)
                return;

            var tickCount = Environment.TickCount;
            Stream originalRequest = context.Response.Body;

            try
            {
                var logSetting = applicationSettingService?.Get(AppSettingEnum.RequestLogLevel)?.Setting;

                using (var memStream = new MemoryStream())
                {
                    context.Response.Body = memStream;

                    var log = new RequestLog
                    {
                        Path = context.Request.Path,
                        Method = context.Request.Method,
                        QueryString = context.Request.QueryString.ToString(),
                        Response = "",
                        Payload = "",
                        RequestedOnUtc = DateTime.UtcNow,
                        Ticks = 0,
                    };

                    ProcessUserClaim(context, log);

                    await ProcessRequest(context, log);

                    try
                    {
                        var remoteIpAddress = context.Connection?.RemoteIpAddress;
                        if (remoteIpAddress != null) log.IpAddress = remoteIpAddress.ToString();
                    }
                    catch (Exception)
                    {
                        // do nothing
                    }

                    try
                    {
                        await _next.Invoke(context);
                    }
                    catch (Exception ex)
                    {
                        var errorSB = new StringBuilder();
                        errorSB.AppendLine(ex.Message);
                        errorSB.AppendLine(ex.StackTrace);
                        while (ex.InnerException != null)
                        {
                            ex = ex.InnerException;
                            errorSB.AppendLine(ex.Message);
                            errorSB.AppendLine(ex.StackTrace);
                        }

                        log.ResponseCode = "500";
                        log.Response = errorSB.ToString();
                        log.IsSuccessStatusCode = false;
                        log.Ticks = Environment.TickCount - tickCount;

                        if (logSetting is not null && (logSetting == "All" || logSetting == "Light" || (logSetting == "Error" && log.IsSuccessStatusCode == false)))
                        {
                            if (logSetting == "Light") { log.Payload = ""; log.Response = ""; }
                            baseRepository.Insert(log);
                        }

                        await memStream.CopyToAsync(originalRequest);

                        throw;
                    }

                    await ProcessResponse(context, memStream, log);

                    log.Ticks = Environment.TickCount - tickCount;

                    if (logSetting is not null && (logSetting == "All" || logSetting == "Light" || (logSetting == "Error" && log.IsSuccessStatusCode == false)))
                    {
                        if (logSetting == "Light" && log.ResponseCode != "500") { log.Payload = ""; log.Response = ""; }
                        baseRepository.Insert(log);
                    }

                    memStream.Position = 0;

                    await memStream.CopyToAsync(originalRequest);
                }
            }
            finally
            {
                context.Response.Body = originalRequest;
            }
        }

        private static void ProcessUserClaim(HttpContext context, RequestLog log)
        {
            Claim claim = context.User?.Claims?.FirstOrDefault(t => t.Type == "UserId");
            if (claim != null && int.TryParse(claim.Value, out int value))
            {
                log.UserId = value;
            }
        }

        private static async Task ProcessResponse(HttpContext context, MemoryStream memStream, RequestLog log)
        {
            var responseContentType = context.Response.ContentType;

            memStream.Position = 0;

            if (ResponseIsRelevant(responseContentType))
            {
                var response = await new StreamReader(memStream).ReadToEndAsync();
                log.Response = response;
            }

            log.ResponseCode = context.Response.StatusCode.ToString();
            log.IsSuccessStatusCode = (
                  context.Response.StatusCode == 200 ||
                  context.Response.StatusCode == 201);
        }

        private static bool ResponseIsRelevant(string responseContentType)
        {
            return responseContentType != null && (responseContentType.StartsWith("application/json") || !responseContentType.StartsWith("text/plain"));
        }

        private static async Task ProcessRequest(HttpContext context, RequestLog log)
        {
            var requestContentType = context.Request.ContentType;

            if (RequestIsRelevant(context, requestContentType))
            {
                context.Request.EnableBuffering();
                var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0;
                log.Payload = body;

                // remove sensitive credentials
                if (log.Payload.Contains("password", StringComparison.InvariantCultureIgnoreCase))
                {
                    log.Payload = "Removed - GDPR";
                }
            }
        }

        private static bool RequestIsRelevant(HttpContext context, string requestContentType)
        {
            return context.Request.Method == "POST" && (requestContentType == null || requestContentType.StartsWith("application/json") || requestContentType.StartsWith("text/plain"));
        }
    }
}
