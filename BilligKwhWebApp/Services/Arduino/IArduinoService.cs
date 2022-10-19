using BilligKwhWebApp.Core.Domain;

namespace BilligKwhWebApp.Services.Arduino
{
    public partial interface IArduinoService
    {
        Print GetPrintById(string id);
        void Update(Print print);
        void Insert(Print print);
    }
}
