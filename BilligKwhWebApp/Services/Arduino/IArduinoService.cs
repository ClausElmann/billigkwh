using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Dto;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.Arduino
{
    public partial interface IArduinoService
    {
        Print GetPrintById(string id);
        void Update(Print print);
        void Insert(Print print);
        IReadOnlyCollection<PrintDto> GetAllPrintDto(int kundeId);
        PrintDto GetDtoById(int id);
    }
}
