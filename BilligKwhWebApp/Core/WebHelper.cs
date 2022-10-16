using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using BilligKwhWebApp.Core.Interfaces;

namespace BilligKwhWebApp.Core
{
    public partial class WebHelper : IWebHelper
    {
        // Props
        private const string nullIpAddress = "::1";
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Ctor
        public WebHelper(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }


        // Public Api
        public virtual string GetUrlReferrer()
        {
            if (!IsRequestAvailable())
            {
                return string.Empty;
            }

            return _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Referer];
        }

        public virtual string GetCurrentIpAddress()
        {
            if (!IsRequestAvailable())
            {
                return string.Empty;
            }

            var result = string.Empty;
            try
            {
                //first try to get IP address from the forwarded header
                if (_httpContextAccessor.HttpContext.Request.Headers != null)
                {
                    var forwardedHttpHeaderKey = "X-FORWARDED-FOR";
                    var forwardedHeader = _httpContextAccessor.HttpContext.Request.Headers[forwardedHttpHeaderKey];
                    if (!StringValues.IsNullOrEmpty(forwardedHeader))
                    {
                        result = forwardedHeader.FirstOrDefault();
                    }
                }

                //if this header not exists try get connection remote IP address
                if (string.IsNullOrEmpty(result) && _httpContextAccessor.HttpContext.Connection.RemoteIpAddress != null)
                {
                    result = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                }
            }
            catch
            {
                return string.Empty;
            }

            //some of the validation
            if (result != null && result.Equals("::1", StringComparison.InvariantCultureIgnoreCase))
            {
                result = "127.0.0.1";
            }

            //"TryParse" doesn't support IPv4 with port number
            if (IPAddress.TryParse(result ?? string.Empty, out IPAddress ip))
            {
                //IP address is valid 
                result = ip.ToString();
            }
            else if (!string.IsNullOrEmpty(result))
            {
                //remove port
                result = result.Split(':').FirstOrDefault();
            }

            return result;
        }

        public virtual string GetThisPageUrl(bool includeQueryString, bool? useSsl = null, bool lowercaseUrl = false)
        {
            if (!IsRequestAvailable())
            {
                return string.Empty;
            }

            //get site location
            var siteLocation = GetSiteLocation(useSsl ?? IsCurrentConnectionSecured());

            //add local path to the URL
            var pageUrl = $"{siteLocation.TrimEnd('/')}{_httpContextAccessor.HttpContext.Request.Path}";

            //add query string to the URL
            if (includeQueryString)
            {
                pageUrl = $"{pageUrl}{_httpContextAccessor.HttpContext.Request.QueryString}";
            }

            //whether to convert the URL to lower case
            if (lowercaseUrl)
            {
                pageUrl = pageUrl.ToLowerInvariant();
            }

            return pageUrl;
        }

        public virtual string GetSiteLocation(bool? useSsl = null)
        {
            var siteLocation = string.Empty;

            //get site host
            var siteHost = GetSiteHost(useSsl ?? IsCurrentConnectionSecured());
            if (!string.IsNullOrEmpty(siteHost))
            {
                //add application path base if exists
                siteLocation = IsRequestAvailable() ? $"{siteHost.TrimEnd('/')}{_httpContextAccessor.HttpContext.Request.PathBase}" : siteHost;
            }

            //ensure that URL is ended with slash
            siteLocation = $"{siteLocation.TrimEnd('/')}/";

            return siteLocation;
        }

        public virtual bool IsCurrentConnectionSecured()
        {
            if (!IsRequestAvailable())
            {
                return false;
            }

            return _httpContextAccessor.HttpContext.Request.IsHttps;
        }

        public virtual string GetSiteHost(bool useSsl)
        {
            if (!IsRequestAvailable())
            {
                return string.Empty;
            }

            //try to get host from the request HOST header
            var hostHeader = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Host];
            if (StringValues.IsNullOrEmpty(hostHeader))
            {
                return string.Empty;
            }

            //add scheme to the URL
            var siteHost = $"{(useSsl ? Uri.UriSchemeHttps : Uri.UriSchemeHttp)}://{hostHeader.FirstOrDefault()}";

            //ensure that host is ended with slash
            siteHost = $"{siteHost.TrimEnd('/')}/";

            return siteHost;
        }

        // Internals
        protected virtual bool IsRequestAvailable()
        {
            if (_httpContextAccessor == null || _httpContextAccessor.HttpContext == null)
            {
                return false;
            }

            try
            {
                if (_httpContextAccessor.HttpContext.Request == null)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        protected virtual bool IsIpAddressSet(IPAddress address)
        {
            return address != null && address.ToString() != nullIpAddress;
        }
    }
}
