using BilligKwhWebApp.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BilligKwhWebApp.Services.Customers
{
    public partial interface ICustomerService
    {
        //CRUD
        void Create(Customer customer);
        Customer Get(int customerId);
        void Update(Customer customer);
        void InsertCustomerUserRoleMapping(IEnumerable<CustomerUserRoleMapping> customerMappings);

        IReadOnlyCollection<Customer> GetAll(bool inclDeleted = false);
    }
}
