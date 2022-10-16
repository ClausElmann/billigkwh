using BilligKwhWebApp.Core.Domain;
using System;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.Customers.Repository
{
    public interface ICustomerRepository
    {
        void Insert(Kunde customer);
        void Update(Kunde customer);
        Kunde GetByName(string name);
        Kunde GetByEconomicId(int economicId);
        Kunde GetByGuid(Guid publicId);
        IReadOnlyCollection<Kunde> GetAll(bool onlyDeleted);
        IReadOnlyCollection<Kunde> GetAllByUser(int userId);
        Kunde GetById(int customerId);
    }
}
