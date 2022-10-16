using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace BilligKwhWebApp.Services.Interfaces
{
    public interface ISystemLogger
    {
        Task<IPagedList<Log>> GetAllLogs(DateTime? fromUtc = null, DateTime? toUtc = null,
            string message = "", LogLevel? logLevel = null,
            int pageIndex = 0, int pageSize = int.MaxValue);


        int InsertLogAsync(LogLevel logLevel, string shortMessage, string fullMessage = "", Bruger user = null, string module = null, object dataObject = null);
    }
}
