using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Services.Customers;
using BilligKwhWebApp.Services.Arduino;
using BilligKwhWebApp.Services.Arduino.Repository;

namespace BilligKwhWebApp.Services
{
    public class ArduinoService : IArduinoService
    {
        private readonly ISystemLogger _logger;
        private readonly IArduinoRepository _arduinoRepository;
        private readonly ICustomerService _customerService;

        public ArduinoService(ISystemLogger logger, IArduinoRepository arduinoRepository, ICustomerService customerService)
        {
            _logger = logger;
            _arduinoRepository = arduinoRepository;
            _customerService = customerService;
        }
        public Print GetPrintById(string printId)
        {
            return _arduinoRepository.GetPrintById(printId);
        }

        public void Update(Print print)
        {
            if (print != null)
            {
                _arduinoRepository.Update(print);
            }
            else
            {
                _logger.Warning("print is NULL in Update!", null, "ArduinoService");
            }
        }

        public void Insert(Print print)
        {
            _arduinoRepository.Insert(print);
        }

    }
}
