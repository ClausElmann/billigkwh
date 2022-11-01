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
        public IReadOnlyCollection<Customer> GetAll(bool onlyDeleted)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<Customer>(@"		
					SELECT * 
					FROM dbo.Customers WHERE ID > 3 AND
					 ((@OnlyDeleted = 1 AND Deleted=1) OR (@OnlyDeleted=0 AND Deleted <> 1))",
                    new { OnlyDeleted = onlyDeleted }).ToList();
        }

        public IReadOnlyCollection<Customer> GetAllByUser(int userId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.Query<Customer>(@"		
					SELECT c.* 
					FROM dbo.Customers c
                        INNER JOIN dbo.CustomerUserMappings cum ON cum.UserId = @UserId AND c.Id = cum.CustomerId
					WHERE Deleted <> 1",
                    new { UserId = userId }).ToList();
        }

        public Customer GetByEconomicId(int economicId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.QueryFirstOrDefault<Customer>(
                                @"SELECT TOP (1) * 
					              FROM dbo.Customers
					              WHERE EconomicId = @EconomicId", new { EconomicId = economicId });
        }

        public Customer GetByGuid(Guid publicId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.QueryFirstOrDefault<Customer>(@"		
					SELECT TOP (1) * 
					FROM dbo.Customers
					WHERE PublicId = @publicId", new { publicId });
        }

        public Customer GetById(int customerId)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.QueryFirstOrDefault<Customer>(@"		
					SELECT TOP (1) * 
					FROM dbo.Customers
					WHERE Id = @Id", new { Id = customerId });
        }

        public Customer GetByName(string name)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            return connection.QueryFirstOrDefault<Customer>(
                                @"SELECT TOP (1) * 
					              FROM dbo.Customers
					              WHERE Name = @Name", new { Name = name });
        }

        public void Insert(Customer customer)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            connection.BulkInsert(customer);
        }

        public void Update(Customer customer)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            connection.BulkUpdate(customer);
        }
    }
}
