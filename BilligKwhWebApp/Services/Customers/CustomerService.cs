using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Events;
using BilligKwhWebApp.Services.Customers.Repository;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Services.Invoicing.Economic.Customers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BilligKwhWebApp.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        // Props
        private readonly ISystemLogger _logger;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBaseRepository _baseRepository;
        private readonly IMediator _mediator;
        private readonly IEconomicHttpClient _economicHttpClient;

        // Ctor
        public CustomerService(
            ISystemLogger logger,
            ICustomerRepository customerRepository,
            IBaseRepository baseRepository,
            IMediator mediator, IEconomicHttpClient economicHttpClient)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            _baseRepository = baseRepository;
            _mediator = mediator;
            _economicHttpClient = economicHttpClient;
        }

        public void Create(Kunde customer)
        {
            if (customer != null)
            {
                customer.SidstRettet = DateTime.UtcNow;
                _customerRepository.Insert(customer);
                _mediator.Publish(new CustomerCreatedEvent(customer));
            }
            else
            {
                _logger.Warning("Customer NULL in InsertCustomer", null, "CustomerService");
            }
        }
        public void Update(Kunde customer)
        {
            if (customer != null)
            {
                customer.SidstRettet = DateTime.UtcNow;
                _customerRepository.Update(customer);
                _mediator.Publish(new CustomerUpdatedEvent(customer));
            }
            else
            {
                _logger.Warning("Customer is NULL in UpdateCustomer!", null, "CustomerService");
            }
        }

        // CustomerUserRoles
        public void InsertCustomerUserRoleMapping(IEnumerable<CustomerUserRoleMapping> customerMappings)
        {
            _baseRepository.BulkInsert(customerMappings);
        }

        // Queries
        public Kunde Get(int customerId)
        {
            return _customerRepository.GetById(customerId);
        }

        public Result<Kunde> GetByName(string name)
        {
            var customer = _customerRepository.GetByName(name);

            if (customer == null)
                return Result.Fail<Kunde>("Customer not found.");
            else
                return Result.Ok(customer);
        }
        public Result<Kunde> GetByEconomicId(int economicId)
        {
            var customer = _customerRepository.GetByEconomicId(economicId);

            if (customer == null)
                return Result.Fail<Kunde>("Customer not found.");
            else
                return Result.Ok(customer);
        }
        public Kunde GetByGuid(Guid publicId)
        {
            return _customerRepository.GetByGuid(publicId);
        }
        public IReadOnlyCollection<Kunde> GetAll(bool inclDeleted = false)
        {
            return _customerRepository.GetAll(inclDeleted);
        }

        public IReadOnlyCollection<Kunde> GetAllByUser(int userId)
        {
            return _customerRepository.GetAllByUser(userId);
        }



        //public bool AddUser(Customer customer, User user)
        //{
        //    if (customer != null && user != null)
        //    {
        //        var mapping = new CustomerUserMapping
        //        {
        //            CustomerId = customer.Id,
        //            Customer = customer,
        //            User = user,
        //            UserId = user.Id,
        //            DateCreatedUtc = DateTime.UtcNow,
        //        };
        //        _baseRepository.Insert(mapping);
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public void RemoveUser(int customerId, int userId)
        //{
        //    _baseRepository.Execute(@"DELETE FROM dbo.CustomerUserMappings WHERE CustomerId = @CustomerId AND UserId = @UserId", new { CustomerId = customerId, UserId = userId });
        //}

        //public void AddUserToCustomer(int customerId, int userId)
        //{
        //    _baseRepository.Insert<CustomerUserMapping>(new CustomerUserMapping { CustomerId = customerId, UserId = userId, DateCreatedUtc = DateTime.UtcNow });
        //}

        public async Task<int> CreateOrUpdateEconomicCustomer(int kundeId)
        {
            var kunde = Get(kundeId);

            var exsistingCustomer = await _economicHttpClient.GetEconomicCustomer(kundeId).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(kunde.Adresse)) throw new EconomicException("Can't create an e-Conomic customer without address");

            if (kunde.BrancheTypeID < 1) throw new EconomicException("Can't create an e-Conomic customer without a BrancheTypeID");

            //if (string.IsNullOrEmpty(kunde.FakturaKontaktPerson)) throw new EconomicException("Can't create an e-Conomic customer without a FakturaKontaktPerson");

            var groupName = MapBrancheTypeToCustomerGroupName(kunde.BrancheTypeID);
            var customerGroup = (await _economicHttpClient.GetCustomerGroups().ConfigureAwait(false)).FirstOrDefault(cg => cg.Name.StartsWith(groupName, StringComparison.OrdinalIgnoreCase));

            if (customerGroup is null) throw new EconomicException("Can't create an e-Conomic customer without a customer group");

            var economicCustomer = new EconomicCustomer()
            {
                Name = kunde.Kundenavn,
                CustomerNumber = kunde.Id,
                Currency = GetCurrency(kunde.LandID),
                PaymentTerms = new EconomicPaymentTerms(2),
                CustomerGroup = customerGroup,
                Address = kunde.Adresse,
                City = kunde.By,
                Zip = kunde.PostNr.ToString(),
                CorporateIdentificationNumber = kunde.Cvr == null ? "" : kunde.Cvr.ToString(),
                TelephoneAndFaxNumber = kunde.FakturaTelefonFax,
                Country = CountryConstants.GetCountryName(kunde.LandID),
                Email = kunde.FakturaMail,
                MobilePhone = kunde.FakturaMobil,
            };

            var customerResult = await _economicHttpClient.CreateOrUpdateCustomer(economicCustomer, exsistingCustomer != null ? exsistingCustomer.CustomerNumber : kunde.EconomicId).ConfigureAwait(false);

            if (customerResult.IsFailure)
            {
                throw new EconomicException(customerResult.Message);
            }

            var economicCustomerContacts = await _economicHttpClient.GetEconomicCustomerContacts(kunde.Id);

            EconomicCustomerContact contact;

            if (!economicCustomerContacts.Any())
            {
                contact = new EconomicCustomerContact()
                {
                };
            }
            else
            {
                contact = economicCustomerContacts.First();
            }

            if (string.IsNullOrWhiteSpace(kunde.FakturaKontaktPerson)) kunde.FakturaKontaktPerson = "Faktura";

            contact.Name = kunde.FakturaKontaktPerson ;
            contact.Email = kunde.FakturaMail;
            contact.Phone = kunde.FakturaTelefonFax + "";
            contact.MobilePhone = kunde.FakturaMobil + "";
            contact.Customer = new EconomicCustomerReference(customerResult.Value.CustomerNumber.Value);

            var contactResult = await _economicHttpClient.CreateOrUpdateCustomerContact(contact).ConfigureAwait(false);

            if (contactResult.IsFailure)
            {
                throw new EconomicException(contactResult.Message);
            }

            var refContact = contactResult.Value;

            var customer = customerResult.Value;

            customer.Attention = new EconomicCustomerContactReference(customer.CustomerNumber.Value, contactResult.Value.CustomerContactNumber.Value);

            var updateResult = await _economicHttpClient.CreateOrUpdateCustomer(customer, customer.CustomerNumber).ConfigureAwait(false);

            if (updateResult.IsFailure)
            {
                throw new EconomicException(contactResult.Message);
            }

            kunde.EconomicId = customerResult.Value.CustomerNumber;

            Update(kunde);

            return kunde.EconomicId.Value;
        }

        private static string GetCurrency(int countryId)
        {
            return countryId switch
            {
                1 => "DKK",
                2 => "SEK",
                4 => "EUR",
                _ => "DKK"
            };
        }

        private static string MapBrancheTypeToCustomerGroupName(int brancheTypeId)
        {
            return brancheTypeId switch
            {
                11 => "Diverse",
                _ => "Diverse",
            };
        }

    }
}
