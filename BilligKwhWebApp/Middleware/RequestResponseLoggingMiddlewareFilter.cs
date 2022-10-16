using System;

namespace BilligKwhWebApp.Middleware
{
    public static class RequestResponseLoggingMiddlewareFilter
    {
        public static bool RequestResponseLoggingMiddlewareRules(Microsoft.AspNetCore.Http.HttpContext ctx)
        {
            bool returnValue = false;

            if (null != ctx && ctx.Request.Path.StartsWithSegments("/api", StringComparison.InvariantCultureIgnoreCase))
            {
                returnValue = true;
            }

            return returnValue;
        }
    }
}
