using Dapper;
using System.Collections.Generic;
using System.Linq;
using Z.Dapper.Plus;
using BilligKwhWebApp.Services.Arduino.Domain;
using BilligKwhWebApp.Services.Electricity.Dto;

namespace BilligKwhWebApp.Services.Arduino.Repository
{
    public class ArduinoRepository : IArduinoRepository
    {

        public void Update(SmartDevice SmartDevice)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            connection.BulkUpdate(SmartDevice);
        }

        public void Insert(SmartDevice SmartDevice)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            connection.BulkInsert(SmartDevice);
        }

        public SmartDevice GetSmartDeviceById(string SmartDeviceId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.QueryFirstOrDefault<SmartDevice>(@"
            SELECT * FROM [SmartDevice]
			WHERE SmartDeviceId = @SmartDeviceId", new { SmartDeviceId = SmartDeviceId });
        }

        public IReadOnlyCollection<SmartDeviceDto> GetAllSmartDeviceDto(int kundeId)
        {

            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<SmartDeviceDto>(@"
            SELECT *
            FROM [SmartDevice] WHERE KundeID = @KundeId", new { KundeId = kundeId }).ToList();
        }

        public SmartDeviceDto GetDtoById(int id)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.QueryFirstOrDefault<SmartDeviceDto>(@"
            SELECT * FROM [SmartDevice] WHERE 
            Id = @Id", new { Id = id });
        }
    }
}
