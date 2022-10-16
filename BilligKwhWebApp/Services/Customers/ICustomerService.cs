using BilligKwhWebApp.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BilligKwhWebApp.Services.Customers
{
    public partial interface ICustomerService
    {
        //CRUD
        void Create(Kunde customer);
        Kunde Get(int customerId);
        void Update(Kunde customer);
        void InsertCustomerUserRoleMapping(IEnumerable<CustomerUserRoleMapping> customerMappings);

        IReadOnlyCollection<Kunde> GetAll(bool inclDeleted = false);

        Task<int> CreateOrUpdateEconomicCustomer(int kundeId);
    }
}
