using BilligKwhWebApp.Core.Domain;

namespace BilligKwhWebApp.Core.Interfaces
{
    public interface IWorkContext
    {
        User CurrentUser { get; set; }

        Customer CurrentCustomer { get; }

        int CustomerId { get; }

        int CurrentUserId { get; }

        int ImpersonateFromUserId { get; }


        bool IsLoggedIn { get; }

        bool IsImpersonating { get; }

        bool IsUserSuperAdmin();
    }
}
