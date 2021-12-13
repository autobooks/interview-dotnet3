using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using GroceryStoreAPI.Domain;
using GroceryStoreAPI.DTO;
using GroceryStoreAPI.Infrastructure;

namespace GroceryStoreAPI
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly GroceryStoreDbContext _context;
        private readonly ILogger<CustomerRepository> _logger;
        public CustomerRepository(GroceryStoreDbContext context, ILogger<CustomerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IEnumerable<ICustomer>> GetList()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<ICustomer> GetById(int id)
        {
            // For demo simplicity not returning error if not found
            return await _context.Customers.FindAsync(id);
        }

        public async Task<ICustomer> Add(CustomerDTO customer)
        {            
            try
            {
                // Validations, creation of new object(s) and saving to context normally should be in separate methods 
                // but putting them all here for demo simplicity
                var newCustomer = Customer.Create(customer.Name);     
                _context.Customers.Add(newCustomer);               
                await _context.SaveChangesAsync();                
                return newCustomer;
            }
            catch(Exception ex)            
            {
                // Just display in debug output for demo purposes
                _logger.LogDebug(ex.Message);
                throw;               
            }                        
        }

        public async Task<ICustomer> Update(int id, CustomerDTO customer)
        {
            var entity = (Customer) await GetById(id);
            if (entity is null)
            {
                return null;
            }
            try
            {
                entity.SetData(customer.Name);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                 // Just display in debug output for demo purposes
                _logger.LogDebug(ex.Message);
                throw; 
            }            
        }

        public async Task<bool> Delete(int id)
        {
            var entity = (Customer) await GetById(id);
            if (entity == null)
            {
                return false;
            }
            _context.Customers.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }       
    }
}
