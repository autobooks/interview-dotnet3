using GroceryStoreAPI.Contracts;
using GroceryStoreAPI.DataAccess;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.BusinessLogic
{
    public class BusinessLogic : IBusinessLogic
    {
        private readonly ILogger<BusinessLogic> _logger;
        private readonly IDataAccess _dataAccess;

        public BusinessLogic(ILogger<BusinessLogic> logger, IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _logger = logger;
        }

        public async Task<IEnumerable<SelectContract>> GetAllAsync()
        {
            var customers =
                await _dataAccess.SelectAllAsync();
            return (IEnumerable<SelectContract>)customers;
        }

        public async Task<SelectContract> GetCustomersByIdAsync(Guid customerId)
        {
            var customer =
                await _dataAccess.SelectByIdAsync(customerId);
            if (customer == null)
            {
                throw new ArgumentException($"Could not find a customers by this id: {customerId}.");
            }
            return customer;
        }

        public async Task<bool> DeleteAsync(Guid customerId)
        {
            var customer =
                await _dataAccess.DeleteAsync(customerId);
            if (!customer)
            {
                throw new ArgumentException($"Could not find a customer by this id: {customerId}.");
            }
            return customer;
        }

        public async Task<bool> UpdateAsync(UpdateContract updateContract)
        {
            var author =
                await _dataAccess.UpdateAsync(updateContract);
            if (!author)
            {
                throw new ArgumentException($"Could not find a customer by this id: {updateContract}.");
            }
            return true;
        }

        public async Task<bool> InsertAsync(string name)
        {
            return await _dataAccess.AddAsync(name);
        }
    }
}