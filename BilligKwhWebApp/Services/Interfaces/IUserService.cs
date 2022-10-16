using BilligKwhWebApp.Core.Domain;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.Interfaces
{
    public interface IUserService
    {
        //CRUD
        void Create(Bruger customer);
        void Update(Bruger customer);
        Bruger Get(int userId, bool inclDeleted = false);
        bool CanUserAccessUser(int userId, int idOfUserToAccess);
        Bruger GetUserByEmail(string email);

        IEnumerable<Bruger> GetUsers();
        IEnumerable<Bruger> GetUsers(int customerId);
        IEnumerable<Bruger> GetUsers(IEnumerable<int> userIds);
        IEnumerable<UserRoleMapping> GetUserRoles(int userId);
        IEnumerable<Bruger> GetUsersWithRoles(int customerId);
        bool UserExists(string email);
        void CountFailedLoginTry(Bruger user);
        bool IsUserLocked(Bruger user);
        void UnlockLogIn(Bruger user);

        void SendTwoFactorPinCodeByEmail(Bruger user, int pinCode);
        IList<Bruger> GetUsersByCustomer(int customerId, bool onlyDeleted = false, int? userId = null);
    }
}
