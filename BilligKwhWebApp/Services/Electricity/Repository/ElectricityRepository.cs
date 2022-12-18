using Dapper;
using BilligKwhWebApp.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System;
using Z.Dapper.Plus;
using BilligKwhWebApp.Services.Electricity.Dto;
using System.Runtime.CompilerServices;
using System.Data;

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

        public IReadOnlyCollection<SmartDevice> GetNoContactToDevices(DateTime datetimeUtc)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<SmartDevice>(@"
                     SELECT * FROM [SmartDevices] WHERE [Deleted] IS NULL AND [LatestContactUtc] < @Datetime AND ErrorMail IS NOT NULL", new { Datetime = datetimeUtc }).ToList();
        }


        public IReadOnlyCollection<Schedule> GetSchedulesForDate(DateTime date, int deviceId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<Schedule>(@"
                     SELECT * FROM [Schedules]
            WHERE [Date] >= @Date AND DeviceId = @DeviceId order by [Date]", new { date.Date, DeviceId = deviceId }).ToList();
        }

        static int RunValue(IReadOnlyCollection<ElectricityPrice> elpriser, DateTime date, int hour, SmartDevice device)
        {
            int runValue = 0;

            // Fast tændt eller slukket
            if (device.StatusId != -1) return device.StatusId;

            if (device.DisableWeekends && (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday))
            {
                // dont run in weekends
            }
            else
            {
                if (device.ZoneId == 1)
                {
                    var first = elpriser.Where(v => v.HourDK.Date == date && v.HourDKNo == hour).ToList();

                    if (elpriser.Where(v => v.HourDK.Date == date && v.HourDKNo == hour && v.Dk1 <= device.MaxRate).Any()) runValue = 1;
                }
                else if (device.ZoneId == 2)
                {
                    if (elpriser.Where(v => v.HourDK.Date == date && v.HourDKNo == hour && v.Dk2 <= device.MaxRate).Any()) runValue = 1;
                }

            }

            // hvis kør ikke men temperatur er for lav
            if (runValue == 0 && device.MinTemp != null && device.MaxRateAtMinTemp != null)
            {
                if (device.ZoneId == 1)
                {
                    if (elpriser.Where(v => v.HourDK.Date == date && v.HourDKNo == hour && v.Dk1 <= device.MaxRateAtMinTemp).Any()) runValue = (int)device.MinTemp;
                }
                else if (device.ZoneId == 2)
                {
                    if (elpriser.Where(v => v.HourDK.Date == date && v.HourDKNo == hour && v.Dk2 <= device.MaxRateAtMinTemp).Any()) runValue = (int)device.MinTemp;
                }
            }

            return runValue;
        }

        public IReadOnlyCollection<Schedule> Calculate(DateTime danish, IReadOnlyCollection<ElectricityPrice> elpriser, IReadOnlyCollection<SmartDevice> devices)
        {
            DateTime timeUtc = DateTime.UtcNow;

            var result = from f in devices
                                    select new Schedule()
                                    {
                                        Date = danish,
                                        DeviceId = f.Id,
                                        LastUpdatedUtc = timeUtc,
                                        H00 = RunValue(elpriser, danish, 0, f),
                                        H01 = RunValue(elpriser, danish, 1, f),
                                        H02 = RunValue(elpriser, danish, 2, f),
                                        H03 = RunValue(elpriser, danish, 3, f),
                                        H04 = RunValue(elpriser, danish, 4, f),
                                        H05 = RunValue(elpriser, danish, 5, f),
                                        H06 = RunValue(elpriser, danish, 6, f),
                                        H07 = RunValue(elpriser, danish, 7, f),
                                        H08 = RunValue(elpriser, danish, 8, f),
                                        H09 = RunValue(elpriser, danish, 9, f),
                                        H10 = RunValue(elpriser, danish, 10, f),
                                        H11 = RunValue(elpriser, danish, 11, f),
                                        H12 = RunValue(elpriser, danish, 12, f),
                                        H13 = RunValue(elpriser, danish, 13, f),
                                        H14 = RunValue(elpriser, danish, 14, f),
                                        H15 = RunValue(elpriser, danish, 15, f),
                                        H16 = RunValue(elpriser, danish, 16, f),
                                        H17 = RunValue(elpriser, danish, 17, f),
                                        H18 = RunValue(elpriser, danish, 18, f),
                                        H19 = RunValue(elpriser, danish, 19, f),
                                        H20 = RunValue(elpriser, danish, 20, f),
                                        H21 = RunValue(elpriser, danish, 21, f),
                                        H22 = RunValue(elpriser, danish, 22, f),
                                        H23 = RunValue(elpriser, danish, 23, f),
                                    };
            return result.ToList();
        }

        public Consumption GetConsumptionByIdAndDate(DateTime date, int deviceId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.QueryFirstOrDefault<Consumption>(@"
            SELECT top 1 * FROM Consumptions WHERE 
            DeviceId = @DeviceId AND Date = @Date", new { Date = date, DeviceId = deviceId });
        }

        public void UpdateConsumption(Consumption entity)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            connection.BulkUpdate(entity);
        }

        public void InsertConsumption(Consumption entity)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            connection.BulkInsert(entity);
        }

        public IReadOnlyCollection<ScheduleDto> GetSchedulesForPeriod(int deviceId, DateTime fromDateUtc, DateTime toDateUtc)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<ScheduleDto>(@"SELECT * FROM [Schedules] WHERE DeviceId = @DeviceId AND [Date] >= @FromDateUtc AND [Date] <= @ToDateUtc order by [Date]",
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

        public void InsertTemperatureReading(TemperatureReading temperatureReading)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            connection.BulkInsert(temperatureReading);
        }

        public IReadOnlyCollection<TemperatureReadingDto> GetTemperatureReadingsPeriod(int deviceId, DateTime fromDateTimeUtc, DateTime toDateTimeUtc)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<TemperatureReadingDto>(@"SELECT * FROM [TemperatureReadings] WHERE DeviceId = @DeviceId AND [DatetimeUtc] >= @FromDateTimeUtc AND [DatetimeUtc] <= @ToDateTimeUtc ORDER BY [DatetimeUtc] DESC ",
                new { DeviceId = deviceId, FromDateTimeUtc = fromDateTimeUtc, ToDateTimeUtc = toDateTimeUtc }).ToList();
        }

        //     public IReadOnlyCollection<RecipeDto> GetRecipes(int deviceId)
        //     {
        //         using var connection = ConnectionFactory.GetOpenConnection();
        //         return connection.Query<RecipeDto>(@"
        //             SELECT Recipes.*, DayTypes.Navn as DayTypeName FROM Recipes INNER JOIN
        //             DayTypes ON Recipes.DayTypeId = DayTypes.Id
        //             WHERE DeviceId = @DeviceId order by Recipes.[Priority]", new { DeviceId = deviceId }).ToList();
        //     }

        //     public void InsertRecipe(Recipe entity)
        //     {
        //         using var connection = ConnectionFactory.GetOpenConnection();
        //         connection.BulkInsert(entity);
        //     }

        //     public void UpdateRecipe(Recipe entity)
        //     {
        //         using var connection = ConnectionFactory.GetOpenConnection();
        //         connection.BulkUpdate(entity);
        //     }

        //     public Recipe RecipeById(int id)
        //     {
        //         using var connection = ConnectionFactory.GetOpenConnection();
        //         return connection.QueryFirstOrDefault<Recipe>(@"
        //         SELECT * FROM [Recipes]
        //WHERE Id = @Id", new { id });
        //     }
    }
}
