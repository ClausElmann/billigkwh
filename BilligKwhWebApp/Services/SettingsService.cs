using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Services.Interfaces;

namespace BilligKwhWebApp.Services
{
    public class SettingsService : ISettingsService
    {
        // Props
        private readonly ISystemLogger _logger;
        private readonly IBaseRepository _baseRepository;

        // Ctor
        public SettingsService(
            ISystemLogger logger,
            IBaseRepository baseRepository)
        {
            _logger = logger;
            _baseRepository = baseRepository;

        }

        public void Create(Indstilling customer)
        {
            if (customer != null)
            {
                _baseRepository.Insert(customer);
            }
            else
            {
                _logger.Warning("Customer NULL in InsertCustomer", null, "CustomerService");
            }
        }
        public void Update(Indstilling customer)
        {
            if (customer != null)
            {
                _baseRepository.Update(customer);
            }
            else
            {
                _logger.Warning("Customer is NULL in UpdateCustomer!", null, "CustomerService");
            }
        }

        public Indstilling Get(int instillingEnumID, int kundeId)
        {
            return _baseRepository.QueryFirstOrDefault<Indstilling>("SELECT * FROM Indstilling where InstillingEnumID = @InstillingEnumID AND KundeId = @KundeId", new { instillingEnumID, kundeId });
        }
    }
}
