using BilligKwhWebApp.Services.Arduino.Domain;
using BilligKwhWebApp.Services.Arduino.Dto;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.Arduino.Repository
{
    public interface IArduinoRepository
    {
        PrintDto GetDtoById(int id);
        Print GetPrintById(string printId);
        void Update(Print print);
        void Insert(Print print);
        IReadOnlyCollection<PrintDto> GetAllPrintDto(int kundeId);
    }
}
