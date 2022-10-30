using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Events;
using BilligKwhWebApp.Services.Customers.Repository;
using BilligKwhWebApp.Services.Interfaces;
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

        // Ctor
        public CustomerService(
            ISystemLogger logger,
            ICustomerRepository customerRepository,
            IBaseRepository baseRepository,
            IMediator mediator)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            _baseRepository = baseRepository;
            _mediator = mediator;
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

    }
}
