using System;
using System.Collections.Generic;
using System.Linq;
using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Services.Interfaces;

namespace BilligKwhWebApp.Services
{
    public class LogService : ILogService
    {
        // Props
        private readonly IBaseRepository _baseRepository;

        // Ctor
        public LogService(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public LogEntry Get(int id)
        {
            #region
            var param = new { Id = id };
            var sql = @"SELECT
                          [Id],
                          [LogLevelId] AS LogType,
                          [ShortMessage],
                          [FullMessage],
                          [IpAddress],
                          [UserId],
                          [PageUrl],
                          [ReferrerUrl],
                          [DateCreatedUtc],
                          [Module],
                          [DataObject],
                          [IsHandled]
                      FROM [dbo].[Logs]
                      WHERE Id = @Id";
            #endregion

            return _baseRepository.Query<LogEntry>(sql, param).FirstOrDefault();
        }
        public IEnumerable<LogEntry> Get(DateTime fromDate, DateTime toDate)
        {
            #region
            var param = new { FromDateUtc = fromDate, ToDateUtc = toDate };
            var sql = @"SELECT
                          [Id],
                          [LogLevelId] AS LogType,
                          [ShortMessage],
                          [FullMessage],
                          [IpAddress],
                          [UserId],
                          [PageUrl],
                          [ReferrerUrl],
                          [DateCreatedUtc],
                          [Module],
                          [DataObject],
                          [IsHandled]
                      FROM [dbo].[Logs]
                      WHERE DateCreatedUtc between @FromDateUtc and dateadd(dy,1, @ToDateUtc)";
            #endregion

            return _baseRepository.Query<LogEntry>(sql, param);
        }
        public IEnumerable<LogEntry> Get(LogLevel logType)
        {
            #region
            var param = new { LogLevel = logType };
            var sql = @"SELECT
                          [Id],
                          [LogLevelId] AS LogType,
                          [ShortMessage],
                          [FullMessage],
                          [IpAddress],
                          [UserId],
                          [PageUrl],
                          [ReferrerUrl],
                          [DateCreatedUtc],
                          [Module],
                          [DataObject],
                          [IsHandled]
                      FROM[dbo].[Logs]
                      WHERE LogLevelId = @LogLevel";
            #endregion

            return _baseRepository.Query<LogEntry>(sql, param);
        }

        public Result<LogEntry> GetLastFatal()
        {
            var sql = @"SELECT TOP (1)
                            log.Id,
                            log.LogLevelId AS LogType,
                            log.ShortMessage,
                            log.FullMessage,
                            log.IpAddress,
                            log.UserId,
                            log.PageUrl,
                            log.ReferrerUrl,
                            log.DateCreatedUtc,
                            log.Module,
                            log.DataObject,
                            log.IsHandled
                        FROM dbo.Logs log
                        WHERE log.LogLevelId = 50
                        ORDER BY Id DESC";

            var result = _baseRepository.Query<LogEntry>(sql).FirstOrDefault();
            if (result == null)
            {
                return Result.Fail<LogEntry>("There are currently no 'Fatal Entries' in the log.");
            }
            return Result.Ok(result);
        }
    }
}
