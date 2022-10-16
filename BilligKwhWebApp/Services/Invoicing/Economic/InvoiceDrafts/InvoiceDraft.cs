using System;
using System.Collections.Generic;
using BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts.Lines;
using System.Linq;
using BilligKwhWebApp.Services.Invoicing.Dto;
using System.Text.Json.Serialization;
using BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts.Customers;
using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Toolbox;
using BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts.Recipients;
using BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts.References;
using BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts.Notes;
using BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts.PaymentTerms;

namespace BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts
{
    /*
     * Check Documentation for easy Access to Child-Objects (self) references 
     */

    public class InvoiceDraft
    {
        // Identification
        [JsonPropertyName("draftInvoiceNumber")]
        public int TemporaryDraftId { get; set; }

        // Monero's
        #region Monero's
        public decimal GrossAmount { get; set; }

        public decimal NetAmount { get; set; }

        public decimal VatAmount { get; set; }

        public string Currency { get; set; }

        public decimal ExchangeRate { get; set; }

        public decimal RoundingAmount { get; set; }

        public decimal MarginPercentage { get; set; }

        public decimal CostPriceInBaseCurrency { get; set; }

        public decimal GrossAmountInBaseCurrency { get; set; }

        public decimal NetAmountInBaseCurrency { get; set; }

        public decimal MarginInBaseCurrency { get; set; }
        #endregion

        // Date
        #region Dates
        public string Date { get; set; }

        public string DueDate { get; set; }
        #endregion

        // Child Objects
        #region Relations
        public InvoiceDraftCustomer Customer { get; set; }

        public InvoiceDraftRecipient Recipient { get; set; }

        [JsonPropertyName("references")]
        public InvoiceDraftReference Reference { get; set; }

        public InvoiceDraftPaymentTerms PaymentTerms { get; set; }

        public InvoiceDraftNote Notes { get; set; }

        public ICollection<InvoiceDraftLine> Lines { get; set; }

        public string ErrorCause { get; set; } = "";
        #endregion

        // Excluded Properties
        #region Properties not currently in Use
        //[JsonProperty("project")]
        //public InvoiceDraftProject Project { get; set; }

        //[JsonProperty("delivery")]
        //public InvoiceDraftDelivery Delivery { get; set; }

        //[JsonProperty("deliveryLocation")]
        //public InvoiceDraftDeliveryLocation DeliveryLocation { get; set; }

        //[JsonProperty("layout")]
        //public InvoiceDraftLayout Layout { get; set; }

        //[JsonProperty("pdf")]
        //public InvoiceDraftPdf Pdf { get; set; }
        #endregion

        // Ctor
        public InvoiceDraft()
        {
            Customer = new InvoiceDraftCustomer();
            Recipient = new InvoiceDraftRecipient();
            Reference = new InvoiceDraftReference();
            PaymentTerms = new InvoiceDraftPaymentTerms();
            Notes = new InvoiceDraftNote();
            Lines = new List<InvoiceDraftLine>();
        }
        public InvoiceDraft(int draftId)
        {
            TemporaryDraftId = draftId;

            Customer = new InvoiceDraftCustomer();
            Recipient = new InvoiceDraftRecipient();
            Reference = new InvoiceDraftReference();
            PaymentTerms = new InvoiceDraftPaymentTerms();
            Notes = new InvoiceDraftNote();
            Lines = new List<InvoiceDraftLine>();
        }

        public string ToJsonString()
        {
            CleanNotes();
            CleanRecipient();
            CleanReferences();
            CleanLines();

            return System.Text.Json.JsonSerializer.Serialize(this, JsonHelper.JsonSerializerOptions);
        }

        private void CleanLines()
        {
            Lines.ForEach(line =>
            {
                // Products
                if (string.IsNullOrEmpty(line.Product.ProductNumber))
                {
                    line.Product = null;
                    line.Quantity = null;
                }

                // Accrual
                if (string.IsNullOrEmpty(line.Accrual.StartDate) || string.IsNullOrEmpty(line.Accrual.EndDate))
                {
                    line.Accrual = null;
                }

                // Unit
                if (line.Unit.UnitId == 0)
                {
                    line.Unit = null;
                    line.UnitCostPrice = null;
                    line.UnitNetPrice = null;
                }
                else if (string.IsNullOrEmpty(line.Unit.Name))
                {
                    line.Unit.Name = null;
                }
            });
        }

        private void CleanReferences()
        {
            if (Reference.CustomerContact.CustomerContactId == 0) Reference.CustomerContact = null;
        }

        private void CleanRecipient()
        {
            // Recipient Data
            if (string.IsNullOrEmpty(Recipient.Name)) Recipient.Name = null;
            if (string.IsNullOrEmpty(Recipient.Address)) Recipient.Address = null;
            if (string.IsNullOrEmpty(Recipient.Zip)) Recipient.Zip = null;
            if (string.IsNullOrEmpty(Recipient.City)) Recipient.City = null;
            if (string.IsNullOrEmpty(Recipient.Country)) Recipient.Country = null;
            // Invoice Delivery Settings
            if (string.IsNullOrEmpty(Recipient.Ean)) Recipient.Ean = null;
            if (string.IsNullOrEmpty(Recipient.PublicEntryNumber)) Recipient.PublicEntryNumber = null;
            if (string.IsNullOrEmpty(Recipient.NemHandleType)) Recipient.NemHandleType = null;

            if (Recipient.Attention.CustomerContactId == 0) Recipient.Attention = null;
        }

        private void CleanNotes()
        {
            if (string.IsNullOrEmpty(Notes.Heading)) Notes.Heading = null;
            if (string.IsNullOrEmpty(Notes.TextLine1)) Notes.TextLine1 = null;
            if (string.IsNullOrEmpty(Notes.TextLine2)) Notes.TextLine2 = null;
        }

        public void UpdateNotes(EconomicReportDTO economicReport)
        {
            if (economicReport is null)
                throw new ArgumentNullException(nameof(economicReport));

            string heading = economicReport.InvoiceDrafts.FirstOrDefault()?.Notes?.Heading;
            if (!string.IsNullOrEmpty(heading))
            {
                Notes.Heading = heading;
            }

            string textLine1 = economicReport.InvoiceDrafts.FirstOrDefault()?.Notes?.TextLine1;
            if (!string.IsNullOrEmpty(textLine1))
            {
                Notes.TextLine1 = textLine1;
            }

            string textLine2 = economicReport.InvoiceDrafts.FirstOrDefault()?.Notes?.TextLine2;
            if (!string.IsNullOrEmpty(textLine2))
            {
                Notes.TextLine2 = textLine2;
            }
        }

        /*
        * ToDo: Abstract Country away from Implementation. 
        */
        // Add/Update Danish Lines
        public void AddDanishSmsLine(EconomicReportDTO economicReport, CustomerAccount customerAccount, int sortKey, int lineId)
        {
            if (economicReport is null)
                throw new ArgumentNullException(nameof(economicReport));
            if (customerAccount is null)
                throw new ArgumentNullException(nameof(customerAccount));

            // Filter 'Sms-Lines' for matching Accrual
            var existingLine = FindLine("32", economicReport.Accrual);
            if (existingLine != null)
            {
                existingLine.Quantity = economicReport.SmsCount;
            }
            else
            {
                AddLine(
                    lineId: lineId,
                    sortKey: sortKey,
                    lineDescriptionName: "SMS udsendelse",
                    productNumber: "32",
                    quantity: economicReport.SmsCount,
                    unitPrice: customerAccount.PricePerSms,
                    economicReport: economicReport);
            }
        }
        public void AddDanishEmailLine(EconomicReportDTO economicReport, CustomerAccount customerAccount, int sortKey, int lineId)
        {
            if (economicReport is null)
                throw new ArgumentNullException(nameof(economicReport));
            if (customerAccount is null)
                throw new ArgumentNullException(nameof(customerAccount));

            // Get 'Email-Lines' And Update Existing line if Accrual is the same
            var existingLine = FindLine("325", economicReport.Accrual);
            if (existingLine != null)
            {
                existingLine.Quantity = economicReport.EmailCount;
            }
            else
            {
                AddLine(
                    lineId: lineId,
                    sortKey: sortKey,
                    lineDescriptionName: "E-mail udsendelse",
                    productNumber: "325",
                    quantity: economicReport.EmailCount,
                    unitPrice: customerAccount.PricePerEmail,
                    economicReport: economicReport);
            }
        }
        public void AddDanishVoiceLine(EconomicReportDTO economicReport, CustomerAccount customerAccount, int sortKey, int lineId)
        {
            if (economicReport is null)
                throw new ArgumentNullException(nameof(economicReport));
            if (customerAccount is null)
                throw new ArgumentNullException(nameof(customerAccount));

            // Get 'Email-Lines' And Update Existing line if Accrual is the same
            var existingLine = FindLine("326", economicReport.Accrual);
            if (existingLine != null)
            {
                existingLine.Quantity = economicReport.SmsVoiceCount;
            }
            else
            {
                AddLine(
                    lineId: lineId,
                    sortKey: sortKey,
                    lineDescriptionName: "Voice udsendelse",
                    productNumber: "326",
                    quantity: economicReport.SmsVoiceCount,
                    unitPrice: customerAccount.PricePerVoice,
                    economicReport: economicReport);
            }
        }

        public InvoiceDraft UpdateDanishInvoiceLines(EconomicReportDTO economicReport, CustomerAccount customerAccount)
        {
            if (economicReport is null)
                throw new ArgumentNullException(nameof(economicReport));
            var sortKey = GetHighestSortKey();
            var lineId = GetHighestLineId();

            UpdateGeneralInvoiceLines(economicReport);

            if (economicReport.SmsCount > 0)
            {
                lineId++;
                sortKey++;
                AddDanishSmsLine(economicReport, customerAccount, sortKey, lineId);
            }
            if (economicReport.EmailCount > 0)
            {
                lineId++;
                sortKey++;
                AddDanishEmailLine(economicReport, customerAccount, sortKey, lineId);
            }
            if (economicReport.SmsVoiceCount > 0)
            {
                lineId++;
                sortKey++;
                AddDanishVoiceLine(economicReport, customerAccount, sortKey, lineId);
            }

            return this;
        }
        public Result<InvoiceDraft> CompareDanishLines(EconomicReportDTO economicReport)
        {
            var invoiceDraftSmsCount = Convert.ToInt32(FindLine("32", economicReport.Accrual)?.Quantity);
            var invoiceDraftEmailCount = Convert.ToInt32(FindLine("325", economicReport.Accrual)?.Quantity);
            var invoiceDraftSmsVoiceCount = Convert.ToInt32(FindLine("326", economicReport.Accrual)?.Quantity);


            if (invoiceDraftSmsCount == economicReport.SmsCount
             && invoiceDraftSmsVoiceCount == economicReport.SmsVoiceCount
             && invoiceDraftEmailCount == economicReport.EmailCount)
            {
                // Same
                return Result.Ok(this);
            }
            else
            {
                // Changed
                return Result.Fail<InvoiceDraft>("EconomicReport has changed Since the last upload to Economic.");
            }
        }

        // Add/Update Swedish Lines
        public void AddSwedishSmsLine(EconomicReportDTO economicReport, CustomerAccount customerAccount, int sortKey, int lineId)
        {
            if (economicReport is null)
                throw new ArgumentNullException(nameof(economicReport));
            if (customerAccount is null)
                throw new ArgumentNullException(nameof(customerAccount));

            var existingLine = FindLine("32", economicReport.Accrual);
            if (existingLine != null)
            {
                existingLine.Quantity = economicReport.SmsCount;
            }
            else
            {
                AddLine(
                    lineId: lineId,
                    sortKey: sortKey,
                    lineDescriptionName: "SMS-meddelande",
                    productNumber: "32",
                    quantity: economicReport.SmsCount,
                    unitPrice: customerAccount.PricePerSms,
                    economicReport: economicReport);
            }
        }
        public void AddSwedishEmailLine(EconomicReportDTO economicReport, CustomerAccount customerAccount, int sortKey, int lineId)
        {
            if (economicReport is null)
                throw new ArgumentNullException(nameof(economicReport));
            if (customerAccount is null)
                throw new ArgumentNullException(nameof(customerAccount));

            var existingLine = FindLine("325", economicReport.Accrual);
            if (existingLine != null)
            {
                existingLine.Quantity = economicReport.EmailCount;
            }
            else
            {
                AddLine(
                    lineId: lineId,
                    sortKey: sortKey,
                    lineDescriptionName: "E-postmeddelande",
                    productNumber: "325",
                    quantity: economicReport.EmailCount,
                    unitPrice: customerAccount.PricePerEmail,
                    economicReport: economicReport);
            }
        }
        public void AddSwedishVoiceLine(EconomicReportDTO economicReport, CustomerAccount customerAccount, int sortKey, int lineId)
        {
            if (economicReport is null)
                throw new ArgumentNullException(nameof(economicReport));
            if (customerAccount is null)
                throw new ArgumentNullException(nameof(customerAccount));

            var existingLine = FindLine("326", economicReport.Accrual);
            if (existingLine != null)
            {
                existingLine.Quantity = economicReport.SmsVoiceCount;
            }
            else
            {
                AddLine(
                    lineId: lineId,
                    sortKey: sortKey,
                    lineDescriptionName: "Voice-meddelande",
                    productNumber: "326",
                    quantity: economicReport.SmsVoiceCount,
                    unitPrice: customerAccount.PricePerVoice,
                    economicReport: economicReport);
            }
        }
        public InvoiceDraft UpdateSwedishInvoiceLines(EconomicReportDTO economicReport, CustomerAccount customerAccount)
        {
            if (economicReport is null)
                throw new ArgumentNullException(nameof(economicReport));
            if (customerAccount is null)
                throw new ArgumentNullException(nameof(customerAccount));

            var sortKey = GetHighestSortKey();
            var lineId = GetHighestLineId();

            UpdateGeneralInvoiceLines(economicReport);

            if (economicReport.SmsCount > 0)
            {
                lineId++;
                sortKey++;
                AddSwedishSmsLine(economicReport, customerAccount, sortKey, lineId);
            }
            if (economicReport.EmailCount > 0)
            {
                lineId++;
                sortKey++;
                AddSwedishEmailLine(economicReport, customerAccount, sortKey, lineId);
            }
            if (economicReport.SmsVoiceCount > 0)
            {
                lineId++;
                sortKey++;
                AddSwedishVoiceLine(economicReport, customerAccount, sortKey, lineId);
            }

            return this;
        }

        private void UpdateGeneralInvoiceLines(EconomicReportDTO economicReport)
        {
            if (economicReport is null)
                throw new ArgumentNullException(nameof(economicReport));

            foreach (var line in economicReport.InvoiceDrafts.FirstOrDefault()?.Lines ?? Array.Empty<InvoiceDraftLine>())
            {
                var foundLine = this.Lines.Where(l => l.LineId == line.LineId).FirstOrDefault();

                if (foundLine != null)
                {
                    foundLine.Description = line.Description;
                    foundLine.Quantity = line.Quantity;
                }
            }
        }

        public Result<InvoiceDraft> CompareSwedishLines(EconomicReportDTO economicReport)
        {
            var invoiceDraftSmsCount = Convert.ToInt32(FindLine("32", economicReport.Accrual)?.Quantity);
            var invoiceDraftEmailCount = Convert.ToInt32(FindLine("325", economicReport.Accrual)?.Quantity);
            var invoiceDraftSmsVoiceCount = Convert.ToInt32(FindLine("326", economicReport.Accrual)?.Quantity);

            if (invoiceDraftSmsCount == economicReport.SmsCount
             && invoiceDraftSmsVoiceCount == economicReport.SmsVoiceCount
             && invoiceDraftEmailCount == economicReport.EmailCount)
            {
                // Same
                return Result.Ok(this);
            }
            else
            {
                // Changed
                return Result.Fail<InvoiceDraft>("EconomicReport has changed Since the last upload to Economic.");
            }
        }

        // Add/Update Finnish Lines
        public void AddFinnishSmsLine(EconomicReportDTO economicReport, CustomerAccount customerAccount, int sortKey, int lineId)
        {
            if (economicReport is null)
                throw new ArgumentNullException(nameof(economicReport));
            if (customerAccount is null)
                throw new ArgumentNullException(nameof(customerAccount));

            var existingLine = FindLine("400", economicReport.Accrual);
            if (existingLine != null)
            {
                existingLine.Quantity = economicReport.SmsCount;
            }
            else
            {
                AddLine(
                    lineId: lineId,
                    sortKey: sortKey,
                    lineDescriptionName: "Tekstiviesti",
                    productNumber: "400",
                    quantity: economicReport.SmsCount,
                    unitPrice: customerAccount.PricePerSms,
                    economicReport: economicReport);
            }
        }
        public void AddFinnishEmailLine(EconomicReportDTO economicReport, CustomerAccount customerAccount, int sortKey, int lineId)
        {
            if (economicReport is null)
                throw new ArgumentNullException(nameof(economicReport));
            if (customerAccount is null)
                throw new ArgumentNullException(nameof(customerAccount));

            var existingLine = FindLine("401", economicReport.Accrual);
            if (existingLine != null)
            {
                existingLine.Quantity = economicReport.EmailCount;
            }
            else
            {
                AddLine(
                    lineId: lineId,
                    sortKey: sortKey,
                    lineDescriptionName: "Sähköposti",
                    productNumber: "401",
                    quantity: economicReport.EmailCount,
                    unitPrice: customerAccount.PricePerEmail,
                    economicReport: economicReport);
            }
        }
        public void AddFinnishVoiceLine(EconomicReportDTO economicReport, CustomerAccount customerAccount, int sortKey, int lineId)
        {
            if (economicReport is null)
                throw new ArgumentNullException(nameof(economicReport));
            if (customerAccount is null)
                throw new ArgumentNullException(nameof(customerAccount));

            var existingLine = FindLine("402", economicReport.Accrual);
            if (existingLine != null)
            {
                existingLine.Quantity = economicReport.SmsVoiceCount;
            }
            else
            {
                AddLine(
                    lineId: lineId,
                    sortKey: sortKey,
                    lineDescriptionName: "Ääniviesti",
                    productNumber: "402",
                    quantity: economicReport.SmsVoiceCount,
                    unitPrice: customerAccount.PricePerVoice,
                    economicReport: economicReport);
            }
        }
        public InvoiceDraft UpdateFinnishInvoiceLines(EconomicReportDTO economicReport, CustomerAccount customerAccount)
        {
            if (economicReport is null)
                throw new ArgumentNullException(nameof(economicReport));

            var sortKey = GetHighestSortKey();
            var lineId = GetHighestLineId();

            UpdateGeneralInvoiceLines(economicReport);

            if (economicReport.SmsCount > 0)
            {
                lineId++;
                sortKey++;
                AddFinnishSmsLine(economicReport, customerAccount, sortKey, lineId);
            }
            if (economicReport.EmailCount > 0)
            {
                lineId++;
                sortKey++;
                AddFinnishEmailLine(economicReport, customerAccount, sortKey, lineId);
            }
            if (economicReport.SmsVoiceCount > 0)
            {
                lineId++;
                sortKey++;
                AddFinnishVoiceLine(economicReport, customerAccount, sortKey, lineId);
            }

            return this;
        }
        public Result<InvoiceDraft> CompareFinnishLines(EconomicReportDTO economicReport)
        {
            var invoiceDraftSmsCount = Convert.ToInt32(FindLine("400", economicReport.Accrual)?.Quantity);
            var invoiceDraftEmailCount = Convert.ToInt32(FindLine("401", economicReport.Accrual)?.Quantity);
            var invoiceDraftSmsVoiceCount = Convert.ToInt32(FindLine("402", economicReport.Accrual)?.Quantity);

            if (invoiceDraftSmsCount == economicReport.SmsCount
             && invoiceDraftSmsVoiceCount == economicReport.SmsVoiceCount
             && invoiceDraftEmailCount == economicReport.EmailCount)
            {
                // Same
                return Result.Ok(this);
            }
            else
            {
                // Changed
                return Result.Fail<InvoiceDraft>("EconomicReport has changed Since the last upload to Economic.");
            }
        }

        private void AddLine(int lineId, int sortKey, string lineDescriptionName, string productNumber, decimal quantity, decimal unitPrice, EconomicReportDTO economicReport)
        {
            Lines.Add(new InvoiceDraftLine(
                lineId: lineId,
                sortKey: sortKey,
                description: lineDescriptionName + "\n(" +
                                DateTime
                                    .SpecifyKind(DateTime.Parse(economicReport.Accrual.StartDate), DateTimeKind.Utc)
                                    .FromUtcToLocalDate(TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time"), economicReport.CountryId)
                                + " - " +
                                DateTime
                                    .SpecifyKind(DateTime.Parse(economicReport.Accrual.EndDate), DateTimeKind.Utc)
                                    .FromUtcToLocalDate(TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time"), economicReport.CountryId) + ")",
                quantity: quantity,
                unitNetPrice: unitPrice,
                unit: new InvoiceDraftLineUnit(1),
                product: new InvoiceDraftLineProduct(productNumber),
                accrual: new InvoiceDraftLineAccrual(
                    DateTime.SpecifyKind(DateTime.Parse(economicReport.Accrual.StartDate), DateTimeKind.Utc).FromUtcToLocalDate(TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time")),
                    DateTime.SpecifyKind(DateTime.Parse(economicReport.Accrual.EndDate), DateTimeKind.Utc).FromUtcToLocalDate(TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time"))
                )));
        }

        private InvoiceDraftLine FindLine(string productNumber, InvoiceDraftLineAccrual accrual)
        {
            return Lines.Where(line =>
                               line.Product.ProductNumber == productNumber &&
                               line.Accrual.Equals(accrual.ConvertFromUTC(CountryConstants.DanishCountryId))).FirstOrDefault();
        }

        private int GetHighestSortKey()
        {
            if (Lines.Any())
            {
                var max = Lines.Max(line => line.SortKey);
                return Lines.First(line => line.SortKey == max).SortKey;
            }
            return 1;
        }
        private int GetHighestLineId()
        {
            if (Lines.Any())
            {
                var max = Lines?.Max(line => line.LineId);
                return Lines.First(line => line.LineId == max).LineId;
            }
            return 1;
        }
    }
}
