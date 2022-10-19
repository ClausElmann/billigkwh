using BilligKwhWebApp.Core.Domain;

namespace BilligKwhWebApp.Services.Arduino.Repository
{
    public interface IArduinoRepository
    {
        Print GetPrintById(string printId);
        void Update(Print print);
        void Insert(Print print);
    }
}
