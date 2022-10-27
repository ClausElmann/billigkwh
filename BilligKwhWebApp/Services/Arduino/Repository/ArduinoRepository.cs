using Dapper;
using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Dto;
using System.Collections.Generic;
using System.Linq;
using Z.Dapper.Plus;

namespace BilligKwhWebApp.Services.Arduino.Repository
{
    public class ArduinoRepository : IArduinoRepository
    {

        public void Update(Print print)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            connection.BulkUpdate(print);
        }

        public void Insert(Print print)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            connection.BulkInsert(print);
        }

        public Print GetPrintById(string printId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.QueryFirstOrDefault<Print>(@"
            SELECT * FROM [Print]
			WHERE PrintId = @PrintId", new { PrintId = printId });
        }

        public IReadOnlyCollection<PrintDto> GetAllPrintDto(int kundeId)
        {

            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<PrintDto>(@"
            SELECT *
            FROM [Print] WHERE KundeID = @KundeId", new { KundeId = kundeId }).ToList();
        }

        public PrintDto GetDtoById(int id)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.QueryFirstOrDefault<PrintDto>(@"
            SELECT * FROM [Print] WHERE 
            Id = @Id", new { Id = id });
        }
    }
}
