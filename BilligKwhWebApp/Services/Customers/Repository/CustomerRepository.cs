using Dapper;
using BilligKwhWebApp.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using Z.Dapper.Plus;

namespace BilligKwhWebApp.Services.Customers.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        public IReadOnlyCollection<Kunde> GetAll(bool onlyDeleted)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<Kunde>(@"		
					SELECT * 
					FROM dbo.Kunde
					WHERE (@OnlyDeleted = 1 AND Slettet=1) OR (@OnlyDeleted=0 AND Slettet <> 1)",
                    new { OnlyDeleted = onlyDeleted }).ToList();
        }

        public IReadOnlyCollection<Kunde> GetAllByUser(int userId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<Kunde>(@"		
					SELECT c.* 
					FROM dbo.Kunde c
                        INNER JOIN dbo.CustomerUserMappings cum ON cum.UserId = @UserId AND c.Id = cum.CustomerId
					WHERE Slettet <> 1",
                    new { UserId = userId }).ToList();
        }

        public Kunde GetByEconomicId(int economicId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.QueryFirstOrDefault<Kunde>(
                                @"SELECT TOP (1) * 
					              FROM dbo.Kunde
					              WHERE EconomicId = @EconomicId", new { EconomicId = economicId });
        }

        public Kunde GetByGuid(Guid publicId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.QueryFirstOrDefault<Kunde>(@"		
					SELECT TOP (1) * 
					FROM dbo.Kunde
					WHERE PublicId = @publicId", new { publicId });
        }

        public Kunde GetById(int customerId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.QueryFirstOrDefault<Kunde>(@"		
					SELECT TOP (1) * 
					FROM dbo.Kunde
					WHERE Id = @Id", new { Id = customerId });
        }

        public Kunde GetByName(string name)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.QueryFirstOrDefault<Kunde>(
                                @"SELECT TOP (1) * 
					              FROM dbo.Kunde
					              WHERE Name = @Name", new { Name = name });
        }

        public void Insert(Kunde customer)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            connection.BulkInsert(customer);
            connection.ExecuteScalar($"INSERT INTO KundeModul ([KundeID],[ModulID],[Antal],[Pris],[PrisService],[PrisHosting])VALUES ({customer.Id},1,1,0,0,0);");
            connection.ExecuteScalar($"INSERT INTO KundeModul ([KundeID],[ModulID],[Antal],[Pris],[PrisService],[PrisHosting])VALUES ({customer.Id},24,1,0,0,0);");
        }

        public void Update(Kunde customer)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            connection.BulkUpdate(customer);
        }
    }
}
