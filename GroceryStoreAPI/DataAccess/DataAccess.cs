using Dapper;
using GroceryStoreAPI.Configuration;
using GroceryStoreAPI.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.DataAccess
{
    public class DataAccess : IDataAccess
    {
        private readonly ILogger<DataAccess> _logger;
        private readonly string _dbConnectionString;

        public DataAccess(ILogger<DataAccess> logger,
            DataBaseConnectionStringConfiguration configuration)
        {
            _logger = logger;
            _dbConnectionString = configuration.Db;
        }

        public async Task<IEnumerable<SelectContract>> SelectAllAsync()
        {
            using var connection = new SqlConnection(_dbConnectionString);
            var results = await connection.QueryAsync<SelectContract>(
                Queries.SelectAllCustomers, new { });            

            return results;
        }

        public async Task<SelectContract> SelectByIdAsync(Guid customerId)
        {
            using var connection = new SqlConnection(_dbConnectionString);

            //Get customer
            var userInfo = await connection.QueryFirstOrDefaultAsync<SelectContract>(
                Queries.SelectCustomerById,
                new
                {
                    Id = customerId
                });

            if (userInfo == null)
            {
                throw new ArgumentException($"Customer {userInfo.Name} doesn't exists.");
            }
            else
            {   
                return userInfo;
            }            
        }

        public async Task<bool> DeleteAsync(Guid customerId)
        {
            using var connection = new SqlConnection(_dbConnectionString);

            //Get customer
            var userInfo = await connection.QueryFirstOrDefaultAsync<SelectContract>(
                Queries.SelectCustomerById,
                new
                {
                    Id = customerId
                });

            if (userInfo == null)
            {
                throw new ArgumentException($"Customer {userInfo.Name} doesn't exists.");
            }
            else
            {    
                //Delete customer
                await connection.ExecuteAsync(
                    Queries.DeleteCustomer,
                    new
                    {
                        Id = customerId
                    });

                return true;
            }
        }

        public async Task<bool> UpdateAsync(UpdateContract updateContract)
        {
            using var connection = new SqlConnection(_dbConnectionString);

            //Get customer
            var userInfo = await connection.QueryFirstOrDefaultAsync<SelectContract>(
                Queries.SelectCustomerById,
                new
                {
                    Id = updateContract.Id
                });

            if (userInfo == null)
            {
                throw new ArgumentException($"Customer {updateContract.Name} doesn't exists.");
            }
            else
            {
                //Update
                var update = await connection.ExecuteAsync(
                    Queries.UpdateCustomerById,
                    new
                    {
                        Id = updateContract.Id,
                        Name = updateContract.Name
                    });

                _logger.LogTrace($"Updated customer {updateContract.Name}.");
                if (update > 0)
                {
                    return true;
                }
            } 
            return false;
        }

        public async Task<bool> AddAsync(string name)
        {
            var customerId = Guid.NewGuid();

            using var connection = new SqlConnection(_dbConnectionString);

            var newCustomer = await connection.ExecuteAsync(
                    Queries.InsertCustomer,
                    new
                    {
                        Id = customerId,
                        Name = name
                    });

            //Select customer
            var results = await connection.QueryFirstOrDefaultAsync<SelectContract>(
                Queries.SelectCustomerById,
                new
                {
                    Id = customerId
                });
            if (results == null)
            {
                throw new ArgumentException($" Something went wrong while attempting to add the new customer {name}. Please try again");
            }
            else
            {
                return true;
            }
        }
    }
}