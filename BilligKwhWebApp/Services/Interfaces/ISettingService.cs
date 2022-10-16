using BilligKwhWebApp.Core.Domain;

namespace BilligKwhWebApp.Services.Interfaces
{
    public partial interface ISettingsService
    {
        void Create(Indstilling indstilling);
        Indstilling Get(int indstillingId, int kundeId);
        void Update(Indstilling indstilling);
    }
}
