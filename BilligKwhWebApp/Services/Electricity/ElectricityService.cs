using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Services.Customers;
using BilligKwhWebApp.Services.Electricity.Repository;
using System.Collections.Generic;
using System;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Jobs.ElectricityPrices;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;
using System.Globalization;
using BilligKwhWebApp.Services.Electricity.Dto;
using BilligKwhWebApp.Services.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using BilligKwhWebApp.Core;
using System.Text;
using BilligKwhWebApp.Services.SmartDevices;

namespace BilligKwhWebApp.Services.Electricity
{
    public class ElectricityService : IElectricityService
    {
        private readonly ISystemLogger _logger;
        private readonly IElectricityRepository _electricityRepository;
        private readonly ICustomerService _customerService;
        private readonly IBaseRepository _baseRepository;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailService _emailService;
        private readonly ISmartDeviceService _smartDeviceService;

        public ElectricityService(ISystemLogger logger, IElectricityRepository electricityRepository, ICustomerService customerService, IBaseRepository baseRepository, IEmailTemplateService emailTemplateService, IEmailService emailService, ISmartDeviceService smartDeviceService)
        {
            _logger = logger;
            _electricityRepository = electricityRepository;
            _customerService = customerService;
            _baseRepository = baseRepository;
            _emailTemplateService = emailTemplateService;
            _emailService = emailService;
            _smartDeviceService = smartDeviceService;
        }

        public IReadOnlyCollection<Schedule> GetSchedulesForDate(DateTime date, int deviceId)
        {
            return _electricityRepository.GetSchedulesForDate(date, deviceId);
        }

        private static DateTime Round(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0, dateTime.Kind);
        }

        public async Task UpdateElectricityPrices()
        {
            _logger.Debug("UpdateElectricityPrices starts.");

            DateTime danish = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Romance Standard Time");

            var latest = _electricityRepository.GetLatestElectricityPrice();

            DateTime shouldBeLatest = Round(danish.AddHours(-danish.Hour).AddHours(23));

            if (danish.Hour >= 13)
            {
                shouldBeLatest = Round(danish.AddHours(-danish.Hour).AddHours(23 + 24));
            }

            if (latest.HourDK < shouldBeLatest)
            {
                _logger.Debug("Henter elpriser...");

                HttpClient client = new();
                //HttpResponseMessage response = await client.GetAsync($"https://api.energidataservice.dk/dataset/Elspotprices?offset=0&start=2022-01-01T00:00&filter=%7B%22PriceArea%22:%22dk1,dk2%22%7D&sort=HourUTC%20ASC&timezone=dk");
                HttpResponseMessage response = await client.GetAsync($"https://api.energidataservice.dk/dataset/Elspotprices?offset=0&start={DateTime.UtcNow.AddDays(-2):yyyy-MM-dd}T00:00&filter=%7B%22PriceArea%22:%22dk1,dk2%22%7D&sort=HourUTC%20ASC&timezone=dk");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var welcome = JsonSerializer.Deserialize<Root>(responseBody);

                //var hasTodayPrices = welcome.records.Where(w => w.HourDK > DateTime.UtcNow.Date).Any();

                //if (!hasTodayPrices)
                //    SendElectricityPricesMissingEmail();

                List<ElectricityPrice> ElectricityPrices = new();

                DateTime updatedUtc = DateTime.UtcNow;

                foreach (var record in welcome.records.Where(w => w.PriceArea == "DK1"))
                {
                    var Dk2 = welcome.records.Where(w => w.HourDK == record.HourDK && w.PriceArea == "DK2").FirstOrDefault();
                    ElectricityPrices.Add(new ElectricityPrice()
                    {
                        HourUTC = record.HourUTC,
                        HourDK = record.HourDK,
                        HourDKNo = record.HourDK.Hour,
                        HourUTCNo = record.HourUTC.Hour,
                        Dk1 = (decimal)record.SpotPriceDKK / 1000,
                        Dk2 = (decimal)Dk2.SpotPriceDKK / 1000,
                        UpdatedUtc = updatedUtc,
                    });
                }
                _baseRepository.BulkMerge(ElectricityPrices.DistinctBy(d => d.HourDK));

                _logger.Debug("Henter elpriser slut - Starting to calculate schedules.");

                var elpriser = _electricityRepository.GetElectricityPricesForDate(danish.Date);

                var last = elpriser.OrderByDescending(o => o.HourDK).FirstOrDefault();

                if (last == null || last.HourDK < shouldBeLatest)
                {
                    // Send error mail
                    SendElectricityPricesMissingEmail();
                    return;
                }

                var devicesForRecipes = _electricityRepository.GetSmartDeviceForRecipes();

                var result = _electricityRepository.Calculate(danish, elpriser, devicesForRecipes);

                _baseRepository.BulkMerge(result);

                _logger.Debug("Calculate schedules ends");
            }
            else
            {
                _logger.Debug("UpdateElectricityPrices ends. Alredy up to date!");
            }
        }

        public void SendElectricityPricesMissingEmail()
        {
            var mailTemplate = _emailTemplateService.GetTemplateByNameEnum(CountryConstants.DanishCountryId, EmailTemplateName.ElectricityPricesMissing);

            var fields = new List<KeyValuePair<string, object>>()
            {
                //    new KeyValuePair<string, object>("NEWUSER_URL", url.ToString()),
                //    new KeyValuePair<string, object>("USERNAME", user.Name),
            };

            _emailTemplateService.MergeEmailFields(mailTemplate, fields);

            _emailService.Save(
               customerId: 1,
               fromEmail: mailTemplate.FromEmail,
               fromName: mailTemplate.FromName,
               sendTo: "claus.elmann@gmail.com",
               sendToName: "Claus Elmann",
               replyTo: mailTemplate.ReplyTo,
               subject: mailTemplate.Subject,
               body: mailTemplate.Html,
               categoryEnum: EmailCategoryEnum.SupportMails,
               refTypeID: (int)RefType.Overvaagning,
               refID: 0,
               mailTemplate.BccEmails);


            //    _emailService.Save(
            //        customerId: 1,
            //        fromEmail: mailTemplate.FromEmail,
            //            fromName: mailTemplate.FromName,
            //            sendTo: sendTo,
            //            sendToName: user.Name,
            //            replyTo: mailTemplate.ReplyTo,
            //            subject: mailTemplate.Subject,
            //            body: mailTemplate.Html,
            //            testMode: false,
            //            categoryEnum: EmailCategoryEnum.PasswordMails,
            //            !string.IsNullOrEmpty(sendToEmails) ? mailTemplate.BccEmails : null); // if sendToEmails is empty, the BCCs are the sendTo and then they shouldn't be set as BCC (would cause duplicate emails)
        }

        public void SendNoContactToDeviceEmail(SmartDevice device)
        {
            var mailTemplate = _emailTemplateService.GetTemplateByNameEnum(CountryConstants.DanishCountryId, EmailTemplateName.NoContactToDevice);

            var fields = new List<KeyValuePair<string, object>>()
            {
                    new KeyValuePair<string, object>("ERRORMAIL", device.ErrorMail),
                    new KeyValuePair<string, object>("LOCATION", device.Location),
                    new KeyValuePair<string, object>("LATESTCONTACT", device.LatestContactUtc.ToLocalTime()),
            };

            _emailTemplateService.MergeEmailFields(mailTemplate, fields);

            _emailService.Save(
               customerId: 1,
               fromEmail: mailTemplate.FromEmail,
               fromName: mailTemplate.FromName,
               sendTo: device.ErrorMail,
               sendToName: device.ErrorMail,
               replyTo: mailTemplate.ReplyTo,
               subject: mailTemplate.Subject,
               body: mailTemplate.Html,
               categoryEnum: EmailCategoryEnum.SupportMails,
               refTypeID: (int)RefType.Overvaagning,
               refID: 0,
               mailTemplate.BccEmails);
        }

        public void UpdateConsumption(int deviceId, IReadOnlyCollection<long> list)
        {
            long[] array = list.ToArray();

            var cultureInfo = new CultureInfo("da-DK");

            var firstDate = DateTime.ParseExact(array[0].ToString(), "yyMMdd", cultureInfo);

            var yesterday = firstDate.AddDays(-1);

            var c = _electricityRepository.GetConsumptionByIdAndDate(firstDate, deviceId);

            var cy = _electricityRepository.GetConsumptionByIdAndDate(yesterday, deviceId);

            long lastValueYesterday = 0;
            if (cy != null)
            {
                lastValueYesterday = cy.H23;
            }

            if (c != null)
            {
                c.H00 = Math.Max(c.H00, array[1]);
                c.H01 = Math.Max(c.H01, array[2]);
                c.H02 = Math.Max(c.H02, array[3]);
                c.H03 = Math.Max(c.H03, array[4]);
                c.H04 = Math.Max(c.H04, array[5]);
                c.H05 = Math.Max(c.H05, array[6]);
                c.H06 = Math.Max(c.H06, array[7]);
                c.H07 = Math.Max(c.H07, array[8]);
                c.H08 = Math.Max(c.H08, array[9]);
                c.H09 = Math.Max(c.H09, array[10]);
                c.H10 = Math.Max(c.H10, array[11]);
                c.H11 = Math.Max(c.H11, array[12]);
                c.H12 = Math.Max(c.H12, array[13]);
                c.H13 = Math.Max(c.H13, array[14]);
                c.H14 = Math.Max(c.H14, array[15]);
                c.H15 = Math.Max(c.H15, array[16]);
                c.H16 = Math.Max(c.H16, array[17]);
                c.H17 = Math.Max(c.H17, array[18]);
                c.H18 = Math.Max(c.H18, array[19]);
                c.H19 = Math.Max(c.H19, array[20]);
                c.H20 = Math.Max(c.H20, array[21]);
                c.H21 = Math.Max(c.H21, array[22]);
                c.H22 = Math.Max(c.H22, array[23]);
                c.H23 = Math.Max(c.H23, array[24]);
                c.LastUpdatedUtc = DateTime.UtcNow;
                UpdateConsumption(c, lastValueYesterday);
                _electricityRepository.UpdateConsumption(c);
            }
            else
            {
                c = new Consumption()
                {
                    Date = firstDate,
                    DeviceId = deviceId,
                    LastUpdatedUtc = DateTime.UtcNow,
                    H00 = array[1],
                    H01 = array[2],
                    H02 = array[3],
                    H03 = array[4],
                    H04 = array[5],
                    H05 = array[6],
                    H06 = array[7],
                    H07 = array[8],
                    H08 = array[9],
                    H09 = array[10],
                    H10 = array[11],
                    H11 = array[12],
                    H12 = array[13],
                    H13 = array[14],
                    H14 = array[15],
                    H15 = array[16],
                    H16 = array[17],
                    H17 = array[18],
                    H18 = array[19],
                    H19 = array[20],
                    H20 = array[21],
                    H21 = array[22],
                    H22 = array[23],
                    H23 = array[24],
                };
                UpdateConsumption(c, lastValueYesterday);
                _electricityRepository.InsertConsumption(c);
            }

            if (cy != null && array.Length > 24)
            {
                cy.LastUpdatedUtc = DateTime.UtcNow;
                cy.H23 = Math.Max(cy.H23, array[25]);
                _electricityRepository.UpdateConsumption(cy);
            }
        }

        private void UpdateConsumption(Consumption c, long? lastValueYesterday = null)
        {
            c.C00 = c.H00 != 0 ? (decimal)((c.H00 - lastValueYesterday) / 10.0) : 0;
            c.C01 = c.H01 != 0 ? (decimal)((c.H01 - c.H00) / 10.0) : 0;
            c.C02 = c.H02 != 0 ? (decimal)((c.H02 - c.H01) / 10.0) : 0;
            c.C02 = c.H03 != 0 ? (decimal)((c.H03 - c.H02) / 10.0) : 0;
            c.C04 = c.H04 != 0 ? (decimal)((c.H04 - c.H03) / 10.0) : 0;
            c.C05 = c.H05 != 0 ? (decimal)((c.H05 - c.H04) / 10.0) : 0;
            c.C06 = c.H06 != 0 ? (decimal)((c.H06 - c.H05) / 10.0) : 0;
            c.C07 = c.H07 != 0 ? (decimal)((c.H07 - c.H06) / 10.0) : 0;
            c.C08 = c.H08 != 0 ? (decimal)((c.H08 - c.H07) / 10.0) : 0;
            c.C09 = c.H09 != 0 ? (decimal)((c.H09 - c.H08) / 10.0) : 0;
            c.C10 = c.H10 != 0 ? (decimal)((c.H10 - c.H09) / 10.0) : 0;
            c.C11 = c.H11 != 0 ? (decimal)((c.H11 - c.H10) / 10.0) : 0;
            c.C12 = c.H12 != 0 ? (decimal)((c.H12 - c.H11) / 10.0) : 0;
            c.C13 = c.H13 != 0 ? (decimal)((c.H13 - c.H12) / 10.0) : 0;
            c.C14 = c.H14 != 0 ? (decimal)((c.H14 - c.H13) / 10.0) : 0;
            c.C15 = c.H15 != 0 ? (decimal)((c.H15 - c.H14) / 10.0) : 0;
            c.C16 = c.H16 != 0 ? (decimal)((c.H16 - c.H15) / 10.0) : 0;
            c.C17 = c.H17 != 0 ? (decimal)((c.H17 - c.H16) / 10.0) : 0;
            c.C18 = c.H18 != 0 ? (decimal)((c.H18 - c.H17) / 10.0) : 0;
            c.C19 = c.H19 != 0 ? (decimal)((c.H19 - c.H18) / 10.0) : 0;
            c.C20 = c.H20 != 0 ? (decimal)((c.H20 - c.H19) / 10.0) : 0;
            c.C21 = c.H21 != 0 ? (decimal)((c.H21 - c.H20) / 10.0) : 0;
            c.C22 = c.H22 != 0 ? (decimal)((c.H22 - c.H21) / 10.0) : 0;
            c.C23 = c.H23 != 0 ? (decimal)((c.H23 - c.H22) / 10.0) : 0;
        }

        public IReadOnlyCollection<ScheduleDto> GetSchedulesForPeriod(int deviceId, DateTime fromDateUtc, DateTime toDateUtc)
        {
            return _electricityRepository.GetSchedulesForPeriod(deviceId, fromDateUtc, toDateUtc);
        }

        public void Calculate(DateTime danish, IReadOnlyCollection<ElectricityPrice> elpriser, IReadOnlyCollection<SmartDevice> devices)
        {
            var result = _electricityRepository.Calculate(danish, elpriser, devices);
            _baseRepository.BulkMerge(result);
        }

        public IReadOnlyCollection<SmartDevice> GetSmartDeviceForRecipes()
        {
            return _electricityRepository.GetSmartDeviceForRecipes();
        }

        public IReadOnlyCollection<ElectricityPrice> GetElectricityPriceForDate(DateTime date)
        {
            return _electricityRepository.GetElectricityPricesForDate(date);
        }

        public IReadOnlyCollection<ElectricityPrice> GetElectricityPricesForPeriod(DateTime fromDateUtc, DateTime toDateUtc)
        {
            return _electricityRepository.GetElectricityPricesForPeriod(fromDateUtc, toDateUtc);
        }

        public IReadOnlyCollection<ConsumptionDto> GetConsumptionsPeriod(int deviceId, DateTime fromDateUtc, DateTime toDateUtc)
        {
            return _electricityRepository.GetConsumptionsPeriod(deviceId, fromDateUtc, toDateUtc);
        }

        public void SendNoContactToDeviceAdvices()
        {
            var list = _electricityRepository.GetNoContactToDevices(DateTime.UtcNow.AddHours(-1));

            foreach (var device in list)
            {
                SendNoContactToDeviceEmail(device);
            }
        }
    }
}
