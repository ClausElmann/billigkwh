using BilligKwhWebApp.Core.Domain;
using System;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.Interfaces
{
    public interface ILogService
    {
        LogEntry Get(int id);
        Result<LogEntry> GetLastFatal();

        IEnumerable<LogEntry> Get(DateTime from, DateTime to);
        IEnumerable<LogEntry> Get(LogLevel logType);
    }
}
