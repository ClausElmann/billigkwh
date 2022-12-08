using Dapper;
using BilligKwhWebApp.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System;
using Z.Dapper.Plus;
using BilligKwhWebApp.Services.Electricity.Dto;

namespace BilligKwhWebApp.Services.Electricity.Repository
{
    public class ElectricityRepository : IElectricityRepository
    {
        public IReadOnlyCollection<ElectricityPrice> GetElectricityPricesForDate(DateTime date)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<ElectricityPrice>(@"
                     SELECT * FROM [ElectricityPrices]
            WHERE HourDK >= @Date", new { Date = date.Date }).ToList();
        }

        public ElectricityPrice GetLatestElectricityPrice()
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.QueryFirstOrDefault<ElectricityPrice>(@"
                     SELECT top 1 * FROM [ElectricityPrices] order by HourDK DESC");
        }

        public IReadOnlyCollection<SmartDevice> GetSmartDeviceForRecipes()
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<SmartDevice>(@"
                     SELECT * FROM [SmartDevices] WHERE [Deleted] IS NULL AND [CustomerId] IS NOT NULL").ToList();
        }

        public IReadOnlyCollection<Schedule> GetSchedulesForDate(DateTime date, int deviceId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<Schedule>(@"
                     SELECT * FROM [Schedules]
            WHERE [Date] >= @Date AND DeviceId = @DeviceId order by [Date]", new { date.Date, DeviceId = deviceId }).ToList();
        }

        public IReadOnlyCollection<Schedule> Calculate(DateTime danish, IReadOnlyCollection<ElectricityPrice> elpriser, IReadOnlyCollection<SmartDevice> devices)
        {
            DateTime timeUtc = DateTime.UtcNow;

            var today = danish.Date;
            var tomorrow = danish.AddDays(1).Date;

            var dk1SchedulesToday = from f in devices.Where(w => w.ZoneId == 1)
                                    select new Schedule()
                                    {
                                        Date = today,
                                        DeviceId = f.Id,
                                        LastUpdatedUtc = timeUtc,
                                        H00 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 0 && v.Dk1 <= f.MaxRate).Any(),
                                        H01 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 1 && v.Dk1 <= f.MaxRate).Any(),
                                        H02 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 2 && v.Dk1 <= f.MaxRate).Any(),
                                        H03 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 3 && v.Dk1 <= f.MaxRate).Any(),
                                        H04 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 4 && v.Dk1 <= f.MaxRate).Any(),
                                        H05 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 5 && v.Dk1 <= f.MaxRate).Any(),
                                        H06 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 6 && v.Dk1 <= f.MaxRate).Any(),
                                        H07 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 7 && v.Dk1 <= f.MaxRate).Any(),
                                        H08 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 8 && v.Dk1 <= f.MaxRate).Any(),
                                        H09 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 9 && v.Dk1 <= f.MaxRate).Any(),
                                        H10 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 10 && v.Dk1 <= f.MaxRate).Any(),
                                        H11 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 11 && v.Dk1 <= f.MaxRate).Any(),
                                        H12 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 12 && v.Dk1 <= f.MaxRate).Any(),
                                        H13 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 13 && v.Dk1 <= f.MaxRate).Any(),
                                        H14 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 14 && v.Dk1 <= f.MaxRate).Any(),
                                        H15 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 15 && v.Dk1 <= f.MaxRate).Any(),
                                        H16 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 16 && v.Dk1 <= f.MaxRate).Any(),
                                        H17 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 17 && v.Dk1 <= f.MaxRate).Any(),
                                        H18 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 18 && v.Dk1 <= f.MaxRate).Any(),
                                        H19 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 19 && v.Dk1 <= f.MaxRate).Any(),
                                        H20 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 20 && v.Dk1 <= f.MaxRate).Any(),
                                        H21 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 21 && v.Dk1 <= f.MaxRate).Any(),
                                        H22 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 22 && v.Dk1 <= f.MaxRate).Any(),
                                        H23 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 23 && v.Dk1 <= f.MaxRate).Any(),
                                    };

            var dk1SchedulesTomorrow = from f in devices.Where(w => w.ZoneId == 1)
                                       select new Schedule()
                                       {
                                           Date = tomorrow,
                                           DeviceId = f.Id,
                                           LastUpdatedUtc = timeUtc,
                                           H00 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 0 && v.Dk1 <= f.MaxRate).Any(),
                                           H01 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 1 && v.Dk1 <= f.MaxRate).Any(),
                                           H02 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 2 && v.Dk1 <= f.MaxRate).Any(),
                                           H03 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 3 && v.Dk1 <= f.MaxRate).Any(),
                                           H04 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 4 && v.Dk1 <= f.MaxRate).Any(),
                                           H05 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 5 && v.Dk1 <= f.MaxRate).Any(),
                                           H06 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 6 && v.Dk1 <= f.MaxRate).Any(),
                                           H07 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 7 && v.Dk1 <= f.MaxRate).Any(),
                                           H08 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 8 && v.Dk1 <= f.MaxRate).Any(),
                                           H09 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 9 && v.Dk1 <= f.MaxRate).Any(),
                                           H10 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 10 && v.Dk1 <= f.MaxRate).Any(),
                                           H11 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 11 && v.Dk1 <= f.MaxRate).Any(),
                                           H12 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 12 && v.Dk1 <= f.MaxRate).Any(),
                                           H13 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 13 && v.Dk1 <= f.MaxRate).Any(),
                                           H14 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 14 && v.Dk1 <= f.MaxRate).Any(),
                                           H15 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 15 && v.Dk1 <= f.MaxRate).Any(),
                                           H16 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 16 && v.Dk1 <= f.MaxRate).Any(),
                                           H17 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 17 && v.Dk1 <= f.MaxRate).Any(),
                                           H18 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 18 && v.Dk1 <= f.MaxRate).Any(),
                                           H19 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 19 && v.Dk1 <= f.MaxRate).Any(),
                                           H20 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 20 && v.Dk1 <= f.MaxRate).Any(),
                                           H21 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 21 && v.Dk1 <= f.MaxRate).Any(),
                                           H22 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 22 && v.Dk1 <= f.MaxRate).Any(),
                                           H23 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 23 && v.Dk1 <= f.MaxRate).Any(),
                                       };

            var dk2SchedulesToday = from f in devices.Where(w => w.ZoneId == 2)
                                    select new Schedule()
                                    {
                                        Date = today,
                                        DeviceId = f.Id,
                                        LastUpdatedUtc = timeUtc,
                                        H00 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 0 && v.Dk2 <= f.MaxRate).Any(),
                                        H01 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 1 && v.Dk2 <= f.MaxRate).Any(),
                                        H02 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 2 && v.Dk2 <= f.MaxRate).Any(),
                                        H03 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 3 && v.Dk2 <= f.MaxRate).Any(),
                                        H04 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 4 && v.Dk2 <= f.MaxRate).Any(),
                                        H05 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 5 && v.Dk2 <= f.MaxRate).Any(),
                                        H06 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 6 && v.Dk2 <= f.MaxRate).Any(),
                                        H07 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 7 && v.Dk2 <= f.MaxRate).Any(),
                                        H08 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 8 && v.Dk2 <= f.MaxRate).Any(),
                                        H09 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 9 && v.Dk2 <= f.MaxRate).Any(),
                                        H10 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 10 && v.Dk2 <= f.MaxRate).Any(),
                                        H11 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 11 && v.Dk2 <= f.MaxRate).Any(),
                                        H12 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 12 && v.Dk2 <= f.MaxRate).Any(),
                                        H13 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 13 && v.Dk2 <= f.MaxRate).Any(),
                                        H14 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 14 && v.Dk2 <= f.MaxRate).Any(),
                                        H15 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 15 && v.Dk2 <= f.MaxRate).Any(),
                                        H16 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 16 && v.Dk2 <= f.MaxRate).Any(),
                                        H17 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 17 && v.Dk2 <= f.MaxRate).Any(),
                                        H18 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 18 && v.Dk2 <= f.MaxRate).Any(),
                                        H19 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 19 && v.Dk2 <= f.MaxRate).Any(),
                                        H20 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 20 && v.Dk2 <= f.MaxRate).Any(),
                                        H21 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 21 && v.Dk2 <= f.MaxRate).Any(),
                                        H22 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 22 && v.Dk2 <= f.MaxRate).Any(),
                                        H23 = elpriser.Where(v => v.HourDK.Date == today && v.HourDKNo == 23 && v.Dk2 <= f.MaxRate).Any(),
                                    };

            var dk2SchedulesTomorrow = from f in devices.Where(w => w.ZoneId == 2)
                                       select new Schedule()
                                       {
                                           Date = tomorrow,
                                           DeviceId = f.Id,
                                           LastUpdatedUtc = timeUtc,
                                           H00 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 0 && v.Dk2 <= f.MaxRate).Any(),
                                           H01 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 1 && v.Dk2 <= f.MaxRate).Any(),
                                           H02 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 2 && v.Dk2 <= f.MaxRate).Any(),
                                           H03 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 3 && v.Dk2 <= f.MaxRate).Any(),
                                           H04 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 4 && v.Dk2 <= f.MaxRate).Any(),
                                           H05 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 5 && v.Dk2 <= f.MaxRate).Any(),
                                           H06 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 6 && v.Dk2 <= f.MaxRate).Any(),
                                           H07 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 7 && v.Dk2 <= f.MaxRate).Any(),
                                           H08 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 8 && v.Dk2 <= f.MaxRate).Any(),
                                           H09 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 9 && v.Dk2 <= f.MaxRate).Any(),
                                           H10 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 10 && v.Dk2 <= f.MaxRate).Any(),
                                           H11 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 11 && v.Dk2 <= f.MaxRate).Any(),
                                           H12 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 12 && v.Dk2 <= f.MaxRate).Any(),
                                           H13 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 13 && v.Dk2 <= f.MaxRate).Any(),
                                           H14 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 14 && v.Dk2 <= f.MaxRate).Any(),
                                           H15 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 15 && v.Dk2 <= f.MaxRate).Any(),
                                           H16 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 16 && v.Dk2 <= f.MaxRate).Any(),
                                           H17 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 17 && v.Dk2 <= f.MaxRate).Any(),
                                           H18 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 18 && v.Dk2 <= f.MaxRate).Any(),
                                           H19 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 19 && v.Dk2 <= f.MaxRate).Any(),
                                           H20 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 20 && v.Dk2 <= f.MaxRate).Any(),
                                           H21 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 21 && v.Dk2 <= f.MaxRate).Any(),
                                           H22 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 22 && v.Dk2 <= f.MaxRate).Any(),
                                           H23 = elpriser.Where(v => v.HourDK.Date == tomorrow && v.HourDKNo == 23 && v.Dk2 <= f.MaxRate).Any(),
                                       };

            var result = dk1SchedulesToday.Concat(dk1SchedulesTomorrow).Concat(dk2SchedulesToday).Concat(dk2SchedulesTomorrow).ToList();

            return result;
        }

        public Consumption GetConsumptionByIdAndDate(DateTime date, int deviceId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.QueryFirstOrDefault<Consumption>(@"
            SELECT top 1 * FROM Consumptions WHERE 
            DeviceId = @DeviceId AND Date = @Date", new { Date = date, DeviceId = deviceId });
        }

        public void UpdateConsumption(Consumption consumption)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            connection.BulkUpdate(consumption);
        }

        public void InsertConsumption(Consumption consumption)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            connection.BulkInsert(consumption);
        }

        public IReadOnlyCollection<ScheduleDto> GetSchedulesForPeriod(int deviceId, DateTime fromDateUtc, DateTime toDateUtc)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<ScheduleDto>(@"SELECT * FROM [Schedules] WHERE DeviceId = @DeviceId AND [Date] >= @FromDateUtc AND [Date] <= @ToDateUtc",
                new { DeviceId = deviceId, FromDateUtc = fromDateUtc, ToDateUtc = toDateUtc }).ToList();
        }

        public IReadOnlyCollection<ElectricityPrice> GetElectricityPricesForPeriod(DateTime fromDateUtc, DateTime toDateUtc)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<ElectricityPrice>(@"
                     SELECT * FROM [ElectricityPrices]
            WHERE HourDK >= @FromDateUtc AND HourDK <= @ToDateUtc", new { FromDateUtc = fromDateUtc, ToDateUtc = toDateUtc }).ToList();
        }

        public IReadOnlyCollection<ConsumptionDto> GetConsumptionsPeriod(int deviceId, DateTime fromDateUtc, DateTime toDateUtc)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<ConsumptionDto>(@"SELECT * FROM [Consumptions] WHERE DeviceId = @DeviceId AND [Date] >= @FromDateUtc AND [Date] <= @ToDateUtc ORDER BY [Date] DESC ",
                new { DeviceId = deviceId, FromDateUtc = fromDateUtc, ToDateUtc = toDateUtc }).ToList();
        }
    }
}
