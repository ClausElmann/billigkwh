using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Text.Json;
using BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts;
using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Services.Invoicing.Economic.Customers;
using BilligKwhWebApp.Core.Toolbox;
using BilligKwhWebApp.Services.Invoicing.EconomicDTO.Infrastructure;

namespace BilligKwhWebApp.Services
{
    public interface IEconomicHttpClient
    {
        // Economic Customer management        
        Task<Result<EconomicCustomer>> CreateOrUpdateCustomer(EconomicCustomer customer, int? customerNumber);
        Task<EconomicCustomer> GetEconomicCustomer(int customerNumber);
        Task<IReadOnlyCollection<EconomicCustomerGroup>> GetCustomerGroups();
        Task<Result<EconomicCustomerContact>> CreateOrUpdateCustomerContact(EconomicCustomerContact customerContact);
        Task<IEnumerable<EconomicCustomerContact>> GetEconomicCustomerContacts(int customerNumber);

        //Følgeseddel
        Task<Result<EconomicOrder>> CreateOrUpdateOrder(EconomicOrder order, int? economicOrderNumber);
        Task<byte[]> GetOrderPdf(int economicOrderNumber);

        //Invoice Draft
        Task<Result<EconomicOrder>> CreateOrUpdateInvoiceDraft(EconomicOrder order, int? economicDraftInvoiceNumber);
        Task<InvoiceDraft> GetInvoiceDraft(int draftInvoiceNumber);
        Task<byte[]> GetDraftInvoicePdf(int draftInvoiceNumber);

        //Invoice Booked
        Task<Result<EconomicOrder>> BookInvoice(int draftInvoiceNumber);
        Task<EconomicOrder> GetBookedInvoice(int bookedInvoiceNumber);
        Task<byte[]> GetBookedInvoicePdf(int bookedInvoiceNumber);
    }

    public class EconomicHttpClient : IEconomicHttpClient, IDisposable
    {
        private readonly CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
        private readonly HttpClient _client;
        private bool disposedValue;

        // Ctor
        public EconomicHttpClient(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _client.DefaultRequestHeaders.Clear();
            _client.BaseAddress = new Uri("https://restapi.e-conomic.com");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Add("X-AppSecretToken", "XKxjdj6dPFY5SlFES0dFLkJQx6IwPmpsBScgqDYTzPg1");
            _client.DefaultRequestHeaders.Add("X-AgreementGrantToken", "elbRbDGYUtXJzfauQ7Q2zJtyUMuZTLo2LFHFUJeNEbw1");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        }

        /// Generic [GET] & [PUT] Request
        public async Task<InvoiceDraft> GetInvoiceDraft(int invoiceDraftId)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "/invoices/drafts/" + invoiceDraftId);
            using var response = await _client.SendAsync(request).ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<InvoiceDraft>(json, JsonHelper.JsonSerializerOptions);
        }

        public async Task<EconomicCustomer> GetEconomicCustomer(int customerNumber)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "/customers/" + customerNumber);
            using var response = await _client.SendAsync(request).ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonSerializer.Deserialize<EconomicCustomer>(json, JsonHelper.JsonSerializerOptions);
        }

        //public async Task<EconomicCustomerContact> GetEconomicCustomerContact(int customerNumber)
        //{
        //    using var request = new HttpRequestMessage(HttpMethod.Get, $"/customers/{customerNumber}/contacts/1");
        //    using var response = await _client.SendAsync(request).ConfigureAwait(false);
        //    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        //    return JsonSerializer.Deserialize<EconomicCustomerContact>(json, JsonHelper.JsonSerializerOptions);
        //}

        //public async Task<EconomicCustomerContact> GetEconomicCustomerContact(int customerNumber)
        //{
        //    using var request = new HttpRequestMessage(HttpMethod.Get, $"/customers/{customerNumber}/contacts");
        //    using var response = await _client.SendAsync(request).ConfigureAwait(false);
        //    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        //    return JsonSerializer.Deserialize<EconomicCustomerContact>(json, JsonHelper.JsonSerializerOptions);
        //}
        public async Task<IEnumerable<EconomicCustomerContact>> GetEconomicCustomerContacts(int customerNumber)
        {
            var pagination = new Pagination();
            var collection = new List<EconomicCustomerContact>();

            do
            {
                // Send Request and create a stream
                using var request = new HttpRequestMessage(HttpMethod.Get, pagination.NextPage ?? $"/customers/{customerNumber}/contacts?pagesize=100");
                using var response = await _client.SendAsync(request).ConfigureAwait(false);
                var stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                // Update the Api Details and Results
                var jsonData = JsonDocument.Parse(stringData);
                pagination = JsonSerializer.Deserialize<Pagination>(jsonData.RootElement.GetProperty("pagination").ToString(), JsonHelper.JsonSerializerOptions);

                foreach (JsonElement el in jsonData.RootElement.GetProperty("collection").EnumerateArray())
                {
                    collection.Add(JsonSerializer.Deserialize<EconomicCustomerContact>(el.ToString(), JsonHelper.JsonSerializerOptions));
                }
            } while (!string.IsNullOrEmpty(pagination.NextPage));

            return collection;
        }

        public async Task<IReadOnlyCollection<EconomicCustomerGroup>> GetCustomerGroups()
        {
            // Send Request and await the Response
            var stringData = await _client.GetStringAsync("/customer-groups").ConfigureAwait(false);

            // Update the Api Details and Results
            var jsonData = JsonDocument.Parse(stringData);

            var result = new List<EconomicCustomerGroup>();

            foreach (JsonElement el in jsonData.RootElement.GetProperty("collection").EnumerateArray())
            {
                result.Add(JsonSerializer.Deserialize<EconomicCustomerGroup>(el.ToString(), JsonHelper.JsonSerializerOptions));
            }

            return result;
        }

        public async Task<Result<EconomicCustomerContact>> CreateOrUpdateCustomerContact(EconomicCustomerContact customerContact)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"/customers/{customerContact.Customer.CustomerNumber}/contacts/");

            if (customerContact.CustomerContactNumber!=null)
                httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, $"/customers/{customerContact.Customer.CustomerNumber}/contacts/{customerContact.CustomerContactNumber}");

            // Create http Message
            using var request = httpRequestMessage;

            // Configure Request Message
            request.Content = new StringContent(JsonSerializer.Serialize(customerContact, JsonHelper.JsonSerializerOptions));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Send Request and await the Response
            using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created)
            {
                return Result.Fail<EconomicCustomerContact>(stringResponse);
            }

            var updatedCustomerContact = JsonSerializer.Deserialize<EconomicCustomerContact>(stringResponse, JsonHelper.JsonSerializerOptions);

            response.EnsureSuccessStatusCode();
            return Result.Ok(updatedCustomerContact);
        }

        public async Task<Result<EconomicCustomer>> CreateOrUpdateCustomer(EconomicCustomer customer, int? economicId)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "/customers");

            if (economicId != null)
                httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, $"/customers/{customer.CustomerNumber}");

            // Create http Message
            using var request = httpRequestMessage;

            // Configure Request Message
            request.Content = new StringContent(JsonSerializer.Serialize(customer, JsonHelper.JsonSerializerOptions));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Send Request and await the Response
            using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created)
            {
                return Result.Fail<EconomicCustomer>(stringResponse);
            }

            var updatedCustomer = JsonSerializer.Deserialize<EconomicCustomer>(stringResponse, JsonHelper.JsonSerializerOptions);

            response.EnsureSuccessStatusCode();
            return Result.Ok(updatedCustomer);
        }
        public async Task<Result<EconomicOrder>> CreateOrUpdateOrder(EconomicOrder order, int? orderNumber)
        {
            if (orderNumber != null)
            {
                // hent sent og flyt til draft
                var sentOrder = await GetSentOrder(orderNumber);
                await ToggleOrderSentOrDraft(sentOrder, orderNumber);
                await DeleteOrderDraft((int)orderNumber);
            }

            // opret / overskriv klade
            var klade = await CreateOrderDraft(order).ConfigureAwait(false);
            return await ToggleOrderSentOrDraft(klade, null).ConfigureAwait(false);
        }

        public async Task<string> GetSentOrder(int? orderNumber)
        {
            // Create http Message
            using var request = new HttpRequestMessage(HttpMethod.Get, $"orders/sent/{orderNumber}");
            using var response = await _client.SendAsync(request).ConfigureAwait(false);

            var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return string.Empty;
            }
            return stringResponse;
        }


        public async Task<EconomicOrder> GetBookedInvoice(int invoiceNumber)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"/invoices/booked/{invoiceNumber}");

            using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return JsonSerializer.Deserialize<EconomicOrder>(stringResponse, JsonHelper.JsonSerializerOptions);
        }


        public async Task<string> DeleteOrderDraft(int economicId)
        {
            // Create http Message
            using var request = new HttpRequestMessage(HttpMethod.Delete, $"/orders/drafts/{economicId}");
            using var response = await _client.SendAsync(request).ConfigureAwait(false);

            var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return string.Empty;
            }
            return stringResponse;
        }

        public async Task<string> CreateOrderDraft(EconomicOrder order)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "/orders/drafts");

            // Configure Request Message
            request.Content = new StringContent(JsonSerializer.Serialize(order, JsonHelper.JsonSerializerOptions));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Send Request and await the Response
            using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.Created)
            {
                return string.Empty;
            }
            return stringResponse;
        }

        public async Task<Result<EconomicOrder>> CreateOrUpdateInvoiceDraft(EconomicOrder order, int? economicDraftInvoiceNumber)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"/invoices/drafts");

            if (economicDraftInvoiceNumber != null)
            {

                httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, $"/invoices/drafts/" + economicDraftInvoiceNumber);
                order.DraftInvoiceNumber = economicDraftInvoiceNumber;
            }

            // Create http Message
            using var request = httpRequestMessage;

            // Configure Request Message
            request.Content = new StringContent(JsonSerializer.Serialize(order, JsonHelper.JsonSerializerOptions));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Send Request and await the Response
            using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.OK)
            {
                return Result.Fail<EconomicOrder>(stringResponse);
            }

            EconomicOrder fakturaDraft = JsonSerializer.Deserialize<EconomicOrder>(stringResponse, JsonHelper.JsonSerializerOptions);

            return Result.Ok(fakturaDraft);
        }

        //public async Task<Result<HttpResponseMessage>> UpdateInvoiceDraft(EconomicOrder invoiceDraft, CancellationToken cancellationToken)
        //{
        //    // Create http Message
        //    using var request = new HttpRequestMessage(HttpMethod.Put, "/invoices/drafts/" + invoiceDraft.DraftInvoiceNumber);

        //    // Configure Request Message
        //    request.Content = new StringContent(JsonSerializer.Serialize(invoiceDraft, JsonHelper.JsonSerializerOptions));
        //    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        //    // Send Request and await the Response
        //    using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

        //    if (response.StatusCode != HttpStatusCode.OK)
        //    {
        //        var errorResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        //        return Result.Fail<HttpResponseMessage>(errorResponse);
        //    }
        //    // Ensure we are good..
        //    response.EnsureSuccessStatusCode();

        //    return Result.Ok(response);
        //}

        public async Task<Result<EconomicOrder>> BookInvoice(int draftInvoiceNumber)
        {
            BookInvoice bookInvoice = new() { DraftInvoice = new DraftInvoice() { DraftInvoiceNumber = draftInvoiceNumber } };

            using var request = new HttpRequestMessage(HttpMethod.Post, "/invoices/booked");

            // Configure Request Message
            request.Content = new StringContent(JsonSerializer.Serialize(bookInvoice, JsonHelper.JsonSerializerOptions));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Send Request and await the Response
            using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.Created)
            {
                return Result.Fail<EconomicOrder>(stringResponse);
            }

            EconomicOrder fakturaDraft = JsonSerializer.Deserialize<EconomicOrder>(stringResponse, JsonHelper.JsonSerializerOptions);

            return Result.Ok(fakturaDraft);
        }

        public async Task<Result<EconomicOrder>> ToggleOrderSentOrDraft(string orderJson, int? economicId)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"/orders/sent");

            if (economicId != null)
                httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"/orders/drafts");

            // Create http Message
            using var request = httpRequestMessage;

            // Configure Request Message
            request.Content = new StringContent(orderJson);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Send Request and await the Response
            using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created)
            {
                return Result.Fail<EconomicOrder>(stringResponse);
            }

            EconomicOrder createdOrder = JsonSerializer.Deserialize<EconomicOrder>(stringResponse, JsonHelper.JsonSerializerOptions);

            return Result.Ok(createdOrder);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _cancelTokenSource.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async Task<byte[]> GetOrderPdf(int economicOrderNumber)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"https://restapi.e-conomic.com/orders/sent/{economicOrderNumber}/pdf");
            using var response = await _client.SendAsync(request).ConfigureAwait(false);
            return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }

        public async Task<byte[]> GetBookedInvoicePdf(int bookedInvoiceNumber)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"https://restapi.e-conomic.com/invoices/booked/{bookedInvoiceNumber}/pdf");
            using var response = await _client.SendAsync(request).ConfigureAwait(false);
            return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }

        public async Task<byte[]> GetDraftInvoicePdf(int draftInvoiceNumber)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"https://restapi.e-conomic.com/invoices/drafts/{draftInvoiceNumber}/pdf");
            using var response = await _client.SendAsync(request).ConfigureAwait(false);
            return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }

    }
}
