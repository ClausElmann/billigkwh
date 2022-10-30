using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Models;
using System.Collections.Generic;

namespace BilligKwhWebApp.Core.Factories
{
    public interface ICustomerFactory
    {
        CustomerModel CreateCustomerModel(Customer customerEntity);
        Customer CreateCustomerEntity(CustomerModel dto);

        IList<UserRoleAccessModel> PrepareCustomerUserRoleAccessModels(int customerId, IList<UserRole> roles, IList<CustomerUserRoleMapping> mappings, int languageId);
        IList<UserRoleAccessModel> PrepareCustomerUserRoleAccessModelsForSuperAdmin(int customerId, IList<UserRole> roles, IList<CustomerUserRoleMapping> mappings, int languageId);

        IEnumerable<CustomerUserRoleMapping> PrepareCustomerUserRoleMappings(int customerId, IEnumerable<UserRoleAccessModel> accessModels);
    }
}
