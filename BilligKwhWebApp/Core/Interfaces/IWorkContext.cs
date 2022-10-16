using BilligKwhWebApp.Core.Domain;

namespace BilligKwhWebApp.Core.Interfaces
{
    public interface IWorkContext
    {
        Bruger CurrentUser { get; set; }

        Kunde CurrentCustomer { get; }

        int CurrentCustomerId { get; }

        int CurrentUserId { get; }

        int ImpersonateFromUserId { get; }


        bool IsLoggedIn { get; }

        bool IsImpersonating { get; }

        bool IsUserSuperAdmin();
    }
}
