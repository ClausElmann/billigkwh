using BilligKwhWebApp.Core.Domain;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.Interfaces
{
    public interface IUserService
    {
        //CRUD
        void Create(User customer);
        void Update(User customer);
        User Get(int userId, bool inclDeleted = false);
        bool CanUserAccessUser(int userId, int idOfUserToAccess);
        User GetUserByEmail(string email);

        IEnumerable<User> GetUsers();
        IEnumerable<User> GetUsers(int customerId);
        IEnumerable<User> GetUsers(IEnumerable<int> userIds);
        IEnumerable<UserRoleMapping> GetUserRoles(int userId);
        IEnumerable<User> GetUsersWithRoles(int customerId);
        bool UserExists(string email);
        void CountFailedLoginTry(User user);
        bool IsUserLocked(User user);
        void UnlockLogIn(User user);

        void SendTwoFactorPinCodeByEmail(User user, int pinCode);
        IList<User> GetUsersByCustomer(int customerId, bool onlyDeleted = false, int? userId = null);
    }
}
