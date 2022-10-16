//using System;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using System.Linq;
//using BilligKwhWebApp.Services.Invoicing.EconomicDTO.Infrastructure;
//using BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts;
//using System.Threading;
//using System.Text.Json;
//using BilligKwhWebApp.Services.Invoicing.Economic.Customers;
//using BilligKwhWebApp.Core.Domain;
//using BilligKwhWebApp.Core.Toolbox;

//namespace BilligKwhWebApp.Services.Invoicing
//{
//    public interface IEconomicHttpClient
//    {
//        // Danish CRUD
//        Task<InvoiceDraft> GetDanishInvoiceDraft(int invoiceDraftId);
//        Task<Result<InvoiceDraft>> GetDanishInvoiceDraft(IEnumerable<InvoiceDraft> invoiceDrafts);
//        Task<IEnumerable<InvoiceDraft>> GetAllDanishInvoiceDraft();
//        Task<Result<HttpResponseMessage>> UpdateDanishInvoiceDraft(InvoiceDraft invoiceDraft);
//        Task<Result<HttpResponseMessage>> UpdateDanishInvoiceDraft(InvoiceDraft invoiceDraft, CancellationToken cancellationToken);

//        // Swedish CRUD
//        Task<InvoiceDraft> GetSwedishInvoiceDraft(int invoiceDraftId);
//        Task<Result<InvoiceDraft>> GetSwedishInvoiceDraft(IEnumerable<InvoiceDraft> invoiceDrafts);
//        Task<IEnumerable<InvoiceDraft>> GetAllSwedishInvoiceDrafts();
//        Task<Result<HttpResponseMessage>> UpdateSwedishInvoiceDraft(InvoiceDraft invoiceDraft);
//        Task<Result<HttpResponseMessage>> UpdateSwedishInvoiceDraft(InvoiceDraft invoiceDraft, CancellationToken cancellationToken);

//        // Finnish CRUD
//        Task<InvoiceDraft> GetFinnishInvoiceDraft(int invoiceDraftId);
//        Task<Result<InvoiceDraft>> GetFinnishInvoiceDraft(IEnumerable<InvoiceDraft> invoiceDrafts);
//        Task<IEnumerable<InvoiceDraft>> GetAllFinnishInvoiceDrafts();
//        Task<Result<HttpResponseMessage>> UpdateFinnishInvoiceDraft(InvoiceDraft invoiceDraft);
//        Task<Result<HttpResponseMessage>> UpdateFinnishInvoiceDraft(InvoiceDraft invoiceDraft, CancellationToken cancellationToken);

//        // Economic Customer management        
//        Task<IReadOnlyCollection<EconomicCustomerGroup>> GetCustomerGroupsByCountry(int countryId);
//        Task<Result<EconomicCustomer>> CreateCustomer(int countryId, EconomicCustomer customer);
//        Task<Result<EconomicCustomerContact>> CreateCustomerContact(int countryId, EconomicCustomerContact newContact);
//        Task<Result<EconomicCustomer>> UpdateCustomer(int countryId, EconomicCustomer customer);
//        Task<Result<EconomicOrder>> CreateOrder(int countryId, EconomicOrder newOrder);
//        Task<EconomicProduct> GetSetupProductByCountry(int countryId);
//    }

//    // Client - Split into Country Specific?? - 
//    public class EconomicHttpClient : IEconomicHttpClient, IDisposable
//    {
//        private readonly CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
//        private readonly HttpClient client;
//        private bool disposedValue;

//        // Ctor
//        public EconomicHttpClient(HttpClient client)
//        {
//            this.client = client ?? throw new ArgumentNullException(nameof(client));
//            client.DefaultRequestHeaders.Clear();
//            client.BaseAddress = new Uri("https://restapi.e-conomic.com");
//            client.Timeout = new TimeSpan(0, 0, 30);
//        }


//        // Danish Economic.com API 
//        public async Task<InvoiceDraft> GetDanishInvoiceDraft(int invoiceDraftId)
//        {
//            SetDanishHeaders();

//            return await GetInvoiceDraft(invoiceDraftId).ConfigureAwait(false);
//        }
//        public async Task<Result<InvoiceDraft>> GetDanishInvoiceDraft(IEnumerable<InvoiceDraft> invoiceDrafts)
//        {
//            var invoiceDraft = invoiceDrafts.Count() == 1
//                               ? await GetDanishInvoiceDraft(invoiceDrafts.FirstOrDefault().TemporaryDraftId).ConfigureAwait(false)
//                               : null;

//            if (invoiceDraft == null)
//            {
//                return Result.Fail<InvoiceDraft>("Could not Retrieve any InvoiceDraft");
//            }
//            return Result.Ok(invoiceDraft);
//        }
//        public async Task<IEnumerable<InvoiceDraft>> GetAllDanishInvoiceDraft()
//        {
//            SetDanishHeaders();
//            return await GetAllInvoiceDrafts().ConfigureAwait(false);
//        }
//        public async Task<Result<HttpResponseMessage>> UpdateDanishInvoiceDraft(InvoiceDraft invoiceDraft)
//        {
//            if (invoiceDraft is null)
//                throw new ArgumentNullException(nameof(invoiceDraft));
//            SetDanishHeaders();
//            return await UpdateInvoiceDraft(invoiceDraft, _cancelTokenSource.Token).ConfigureAwait(false);
//        }
//        public async Task<Result<HttpResponseMessage>> UpdateDanishInvoiceDraft(InvoiceDraft invoiceDraft, CancellationToken cancellationToken)
//        {
//            if (invoiceDraft is null)
//                throw new ArgumentNullException(nameof(invoiceDraft));
//            SetDanishHeaders();
//            return await UpdateInvoiceDraft(invoiceDraft, cancellationToken).ConfigureAwait(false);
//        }

//        // Swedish Economic.com API
//        public async Task<InvoiceDraft> GetSwedishInvoiceDraft(int invoiceDraftId)
//        {
//            SetSwedishHeaders();

//            return await GetInvoiceDraft(invoiceDraftId).ConfigureAwait(false);
//        }
//        public async Task<Result<InvoiceDraft>> GetSwedishInvoiceDraft(IEnumerable<InvoiceDraft> invoiceDrafts)
//        {
//            if (invoiceDrafts is null)
//                throw new ArgumentNullException(nameof(invoiceDrafts));

//            var invoiceDraft = invoiceDrafts.Count() == 1
//                               ? await GetSwedishInvoiceDraft(invoiceDrafts.FirstOrDefault().TemporaryDraftId).ConfigureAwait(false)
//                               : null;

//            if (invoiceDraft == null)
//            {
//                return Result.Fail<InvoiceDraft>("Could not Retrieve any InvoiceDraft");
//            }
//            return Result.Ok(invoiceDraft);
//        }
//        public async Task<IEnumerable<InvoiceDraft>> GetAllSwedishInvoiceDrafts()
//        {
//            SetSwedishHeaders();
//            return await GetAllInvoiceDrafts().ConfigureAwait(false);
//        }
//        public async Task<Result<HttpResponseMessage>> UpdateSwedishInvoiceDraft(InvoiceDraft invoiceDraft)
//        {
//            if (invoiceDraft is null)
//                throw new ArgumentNullException(nameof(invoiceDraft));
//            SetSwedishHeaders();
//            return await UpdateInvoiceDraft(invoiceDraft, _cancelTokenSource.Token).ConfigureAwait(false);
//        }
//        public async Task<Result<HttpResponseMessage>> UpdateSwedishInvoiceDraft(InvoiceDraft invoiceDraft, CancellationToken cancellationToken)
//        {
//            if (invoiceDraft is null)
//                throw new ArgumentNullException(nameof(invoiceDraft));
//            SetSwedishHeaders();
//            return await UpdateInvoiceDraft(invoiceDraft, cancellationToken).ConfigureAwait(false);
//        }

//        // Finnish Economic.com API
//        public async Task<InvoiceDraft> GetFinnishInvoiceDraft(int invoiceDraftId)
//        {
//            SetFinnishHeaders();

//            return await GetInvoiceDraft(invoiceDraftId).ConfigureAwait(false);
//        }
//        public async Task<Result<InvoiceDraft>> GetFinnishInvoiceDraft(IEnumerable<InvoiceDraft> invoiceDrafts)
//        {
//            var invoiceDraft = invoiceDrafts.Count() == 1
//                               ? await GetFinnishInvoiceDraft(invoiceDrafts.FirstOrDefault().TemporaryDraftId).ConfigureAwait(false)
//                               : null;

//            if (invoiceDraft == null)
//            {
//                return Result.Fail<InvoiceDraft>("Could not Retrieve any InvoiceDraft");
//            }
//            return Result.Ok(invoiceDraft);
//        }
//        public async Task<IEnumerable<InvoiceDraft>> GetAllFinnishInvoiceDrafts()
//        {
//            SetFinnishHeaders();

//            return await GetAllInvoiceDrafts().ConfigureAwait(false);
//        }
//        public async Task<Result<HttpResponseMessage>> UpdateFinnishInvoiceDraft(InvoiceDraft invoiceDraft)
//        {
//            if (invoiceDraft is null)
//                throw new ArgumentNullException(nameof(invoiceDraft));
//            SetFinnishHeaders();
//            return await UpdateInvoiceDraft(invoiceDraft, _cancelTokenSource.Token).ConfigureAwait(false);
//        }
//        public async Task<Result<HttpResponseMessage>> UpdateFinnishInvoiceDraft(InvoiceDraft invoiceDraft, CancellationToken cancellationToken)
//        {
//            if (invoiceDraft is null)
//                throw new ArgumentNullException(nameof(invoiceDraft));
//            SetFinnishHeaders();
//            return await UpdateInvoiceDraft(invoiceDraft, cancellationToken).ConfigureAwait(false);
//        }

//        /// Generic [GET] & [PUT] Request
//        private async Task<InvoiceDraft> GetInvoiceDraft(int invoiceDraftId)
//        {
//            using var request = new HttpRequestMessage(HttpMethod.Get, "/invoices/drafts/" + invoiceDraftId);
//            using var response = await client.SendAsync(request).ConfigureAwait(false);
//            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

//            return JsonSerializer.Deserialize<InvoiceDraft>(json, JsonHelper.JsonSerializerOptions);
//        }

//        private async Task<IEnumerable<InvoiceDraft>> GetAllInvoiceDrafts()
//        {
//            var pagination = new Pagination();
//            var collection = new List<InvoiceDraft>();

//            do
//            {
//                // Send Request and create a stream
//                using var request = new HttpRequestMessage(HttpMethod.Get, pagination.NextPage ?? "/invoices/drafts?pagesize=100");
//                using var response = await client.SendAsync(request).ConfigureAwait(false);
//                var stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

//                // Update the Api Details and Results
//                var jsonData = JsonDocument.Parse(stringData);
//                pagination = JsonSerializer.Deserialize<Pagination>(jsonData.RootElement.GetProperty("pagination").ToString(), JsonHelper.JsonSerializerOptions);

//                foreach (JsonElement el in jsonData.RootElement.GetProperty("collection").EnumerateArray())
//                {
//                    collection.Add(JsonSerializer.Deserialize<InvoiceDraft>(el.ToString(), JsonHelper.JsonSerializerOptions));
//                }
//            } while (!string.IsNullOrEmpty(pagination.NextPage));

//            return collection;
//        }
//        private async Task<Result<HttpResponseMessage>> UpdateInvoiceDraft(InvoiceDraft invoiceDraft, CancellationToken cancellationToken)
//        {
//            // Create http Message
//            using var request = new HttpRequestMessage(HttpMethod.Put, "/invoices/drafts/" + invoiceDraft.TemporaryDraftId);

//            // Configure Request Message
//            request.Content = new StringContent(invoiceDraft.ToJsonString());
//            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

//            // Send Request and await the Response
//            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

//            if (response.StatusCode != HttpStatusCode.OK)
//            {
//                var errorResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
//                return Result.Fail<HttpResponseMessage>(errorResponse);
//            }
//            // Ensure we are good..
//            response.EnsureSuccessStatusCode();

//            return Result.Ok(response);
//        }

//        // Customer management
//        public async Task<IReadOnlyCollection<EconomicCustomerGroup>> GetCustomerGroupsByCountry(int countryId)
//        {
//            SetHeadersByCountryId(countryId);
//            return await GetCustomerGroups().ConfigureAwait(false);
//        }

//        private async Task<IReadOnlyCollection<EconomicCustomerGroup>> GetCustomerGroups()
//        {
//            // Send Request and await the Response
//            var stringData = await client.GetStringAsync("/customer-groups").ConfigureAwait(false);

//            // Update the Api Details and Results
//            var jsonData = JsonDocument.Parse(stringData);

//            var result = new List<EconomicCustomerGroup>();

//            foreach (JsonElement el in jsonData.RootElement.GetProperty("collection").EnumerateArray())
//            {
//                result.Add(JsonSerializer.Deserialize<EconomicCustomerGroup>(el.ToString(), JsonHelper.JsonSerializerOptions));
//            }

//            return result;
//        }

//        public async Task<EconomicProduct> GetSetupProductByCountry(int countryId)
//        {
//            SetHeadersByCountryId(countryId);
//            return await GetProduct(EconomicProduct.GetSetupProductNumber(countryId)).ConfigureAwait(false);
//        }

//        private async Task<EconomicProduct> GetProduct(string productNumber)
//        {
//            var stringData = await client.GetStringAsync($"/products/{productNumber}").ConfigureAwait(false);
//            return JsonSerializer.Deserialize<EconomicProduct>(stringData, JsonHelper.JsonSerializerOptions);
//        }

//        public async Task<Result<EconomicCustomer>> CreateCustomer(int countryId, EconomicCustomer customer)
//        {
//            SetHeadersByCountryId(countryId);
//            return await CreateCustomer(customer).ConfigureAwait(false);
//        }

//        private async Task<Result<EconomicCustomer>> CreateCustomer(EconomicCustomer customer)
//        {
//            // Create http Message
//            using var request = new HttpRequestMessage(HttpMethod.Post, "/customers");

//            // Configure Request Message
//            request.Content = new StringContent(JsonSerializer.Serialize(customer, JsonHelper.JsonSerializerOptions));
//            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

//            // Send Request and await the Response
//            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

//            var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

//            if (response.StatusCode != HttpStatusCode.Created)
//            {
//                return Result.Fail<EconomicCustomer>(stringResponse);
//            }

//            var createdCustomer = JsonSerializer.Deserialize<EconomicCustomer>(stringResponse, JsonHelper.JsonSerializerOptions);

//            response.EnsureSuccessStatusCode();
//            return Result.Ok(createdCustomer);
//        }

//        public async Task<Result<EconomicCustomerContact>> CreateCustomerContact(int countryId, EconomicCustomerContact customerContact)
//        {
//            SetHeadersByCountryId(countryId);
//            return await CreateCustomerContact(customerContact).ConfigureAwait(false);
//        }

//        private async Task<Result<EconomicCustomerContact>> CreateCustomerContact(EconomicCustomerContact customerContact)
//        {
//            // Create http Message
//            using var request = new HttpRequestMessage(HttpMethod.Post, $"/customers/{customerContact.Customer.CustomerNumber}/Contacts");

//            // Configure Request Message
//            request.Content = new StringContent(JsonSerializer.Serialize(customerContact, JsonHelper.JsonSerializerOptions));
//            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

//            // Send Request and await the Response
//            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

//            var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

//            if (response.StatusCode != HttpStatusCode.Created)
//            {
//                return Result.Fail<EconomicCustomerContact>(stringResponse);
//            }

//            var createdCustomerContact = JsonSerializer.Deserialize<EconomicCustomerContact>(stringResponse, JsonHelper.JsonSerializerOptions);

//            response.EnsureSuccessStatusCode();
//            return Result.Ok(createdCustomerContact);
//        }

//        public async Task<Result<EconomicCustomer>> UpdateCustomer(int countryId, EconomicCustomer customer)
//        {
//            SetHeadersByCountryId(countryId);
//            return await UpdateCustomer(customer).ConfigureAwait(false);
//        }

//        private async Task<Result<EconomicCustomer>> UpdateCustomer(EconomicCustomer customer)
//        {
//            // Create http Message
//            using var request = new HttpRequestMessage(HttpMethod.Put, $"/customers/{customer.CustomerNumber}");

//            // Configure Request Message
//            request.Content = new StringContent(JsonSerializer.Serialize(customer, JsonHelper.JsonSerializerOptions));
//            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

//            // Send Request and await the Response
//            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

//            var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

//            if (response.StatusCode != HttpStatusCode.OK)
//            {
//                return Result.Fail<EconomicCustomer>(stringResponse);
//            }

//            var updatedCustomer = JsonSerializer.Deserialize<EconomicCustomer>(stringResponse, JsonHelper.JsonSerializerOptions);

//            response.EnsureSuccessStatusCode();
//            return Result.Ok(updatedCustomer);
//        }

//        public async Task<Result<EconomicOrder>> CreateOrder(int countryId, EconomicOrder order)
//        {
//            SetHeadersByCountryId(countryId);
//            return await CreateOrder(order).ConfigureAwait(false);
//        }

//        public async Task<Result<EconomicOrder>> CreateOrder(EconomicOrder order)
//        {
//            // Create http Message
//            using var request = new HttpRequestMessage(HttpMethod.Post, $"/orders/drafts");

//            // Configure Request Message
//            request.Content = new StringContent(JsonSerializer.Serialize(order, JsonHelper.JsonSerializerOptions));
//            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

//            // Send Request and await the Response
//            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

//            var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

//            if (response.StatusCode != HttpStatusCode.Created)
//            {
//                return Result.Fail<EconomicOrder>(stringResponse);
//            }

//            var createdOrder = JsonSerializer.Deserialize<EconomicOrder>(stringResponse, JsonHelper.JsonSerializerOptions);

//            response.EnsureSuccessStatusCode();
//            return Result.Ok(createdOrder);
//        }


//        /// Set HTTP Headers (Credentials)
//        private void SetDanishHeaders()
//        {
//            // Clear to Re-use Client
//            client.DefaultRequestHeaders.Clear();

//            // Set Danish Credentials
//            client.DefaultRequestHeaders.Add("X-AppSecretToken", "YnPIXc5OjHIFoFRoalXq12oYnBrhCI8EtsfgDWKsut81");
//            client.DefaultRequestHeaders.Add("X-AgreementGrantToken", "fw-Yyx7DWXKqtoywmsbkYfNM9MkRFGxikW2TrV-uAbU1");
//            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
//        }
//        private void SetSwedishHeaders()
//        {
//            // Clear to Re-use Client
//            client.DefaultRequestHeaders.Clear();

//            // Set Swedish Credentials 
//            client.DefaultRequestHeaders.Add("X-AppSecretToken", "F0y5mfGEqfsVPPl1q3JPi8rHcTe8aR8EBXPWG3R7kI81");
//            client.DefaultRequestHeaders.Add("X-AgreementGrantToken", "vtM4CD4TIgvb3kNtiEYZ0CYzqHVrFJQaNhMp0t50KUU1");
//            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
//        }
//        private void SetFinnishHeaders()
//        {
//            client.DefaultRequestHeaders.Clear();

//            // Set Finish Credentials
//            client.DefaultRequestHeaders.Add("X-AppSecretToken", "kj8GmG1Cxsnn0Sll1hPZi9tfoUgB731VryTUDQDGmME1");
//            client.DefaultRequestHeaders.Add("X-AgreementGrantToken", "D8qj5VruFDzxdiZUTYs9rptkZWUEN0nlxjmyDT2RVJA1");
//            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
//        }

//        private void SetHeadersByCountryId(int countryId)
//        {
//            switch (countryId)
//            {
//                case 1:
//                    SetDanishHeaders();
//                    break;
//                case 2:
//                    SetSwedishHeaders();
//                    break;
//                case 4:
//                    SetFinnishHeaders();
//                    break;
//                default:
//                    SetDanishHeaders();
//                    break;
//            }
//        }

//        protected virtual void Dispose(bool disposing)
//        {
//            if (!disposedValue)
//            {
//                if (disposing)
//                {
//                    _cancelTokenSource.Dispose();
//                }
//                disposedValue = true;
//            }
//        }

//        public void Dispose()
//        {
//            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
//            Dispose(disposing: true);
//            GC.SuppressFinalize(this);
//        }

//    }
//}
