using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Dto;
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
