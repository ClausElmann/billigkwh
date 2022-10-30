using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Services.Interfaces;
using System;

namespace BilligKwhWebApp.Services
{
    public static class LoggingExtensions
    {
        public static void Debug(this ISystemLogger logger, string message, string fullMessage = "", User user = null, string module = null, object dataObject = null)
        {
            FilteredLog(logger, LogLevel.Debug, message, fullMessage, null, user, module, dataObject);
        }

        public static void Information(this ISystemLogger logger, string message, string fullMessage = "", User user = null, string module = null, object dataObject = null)
        {
            FilteredLog(logger, LogLevel.Information, message, fullMessage, null, user, module, dataObject);
        }

        public static int Warning(this ISystemLogger logger, string message, Exception exception = null, string fullMessage = "", User user = null, string module = null, object dataObject = null)
        {
            return FilteredLog(logger, LogLevel.Warning, message, fullMessage, exception, user, module, dataObject);
        }

        public static void Error(this ISystemLogger logger, string message, Exception exception = null, string fullMessage = "", User user = null, string module = null, object dataObject = null)
        {
            FilteredLog(logger, LogLevel.Error, message, fullMessage, exception, user, module, dataObject);
        }

        public static void Fatal(this ISystemLogger logger, string message, Exception exception = null, string fullMessage = "", string module = null, object dataObject = null)
        {
            FilteredLog(logger, LogLevel.Fatal, message, fullMessage, exception, null, module, dataObject);
        }

        private static int FilteredLog(ISystemLogger logger, LogLevel level, string message, string fullMessage = "", Exception exception = null, User user = null, string module = null, object dataObject = null)
        {
            //don't log thread abort exception
            if (exception is System.Threading.ThreadAbortException)
            {
                return -1;
            }

            if (exception != null)
            {
                fullMessage = exception.ToString();
            }

            return logger.InsertLogAsync(level, message, fullMessage, user, module, dataObject);

        }

        private static void FilteredLog(ISystemLogger logger, LogLevel level, string shortMessage, string fullMessage, User user = null, string module = null, object dataObject = null)
        {

            logger.InsertLogAsync(level, shortMessage, fullMessage, user, module, dataObject);

        }
    }

}
