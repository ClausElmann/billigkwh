using BilligKwhWebApp.Core.Domain;
using System;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.Customers.Repository
{
    public interface ICustomerRepository
    {
        void Insert(Customer customer);
        void Update(Customer customer);
        Customer GetByName(string name);
        Customer GetByEconomicId(int economicId);
        Customer GetByGuid(Guid publicId);
        IReadOnlyCollection<Customer> GetAll(bool onlyDeleted);
        IReadOnlyCollection<Customer> GetAllByUser(int userId);
        Customer GetById(int customerId);
    }
}
