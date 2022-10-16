using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Models;
using System.Collections.Generic;

namespace BilligKwhWebApp.Core.Factories
{
    public interface IUserFactory
    {
        UserEditModel PrepareUserModel(Bruger user);

        UserRoleModel PrepareUserRoleModel(UserRole role, int languageId);

        //Bruger CreateUserEntity(UserModel dto);

        Bruger CreateUserEntity(UserEditModel model, Kunde customer);

        /// <param name="languageId">Used for correct translation</param>
        IList<UserRoleAccessModel> PrepareUserRoleAccessModels(int userId, IList<UserRole> roles, IList<UserRoleMapping> mappings, int languageId);
    }
}
