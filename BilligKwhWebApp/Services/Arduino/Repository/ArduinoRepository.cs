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
    }
}
