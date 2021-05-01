using GroceryStoreAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Infrastructure.Repositories
{
    public class CustomerRepository
        : ICustomerRepository
    {
        private readonly GroceryStoreContext _context;

        public CustomerRepository(GroceryStoreContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Customer Add(Customer customer)
        {
            return _context.Customers.Add(customer).Entity;
        }

        public void Delete(Customer customer)
        {
            _ =_context.Customers.Remove(customer).Entity;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            var customers = await _context
                                .Customers.ToListAsync();
            return customers;
        }
        public async Task<Customer> GetAsync(Guid customerId)
        {
            var customer = await _context
                                .Customers
                                .FirstOrDefaultAsync(c => c.Id == customerId);
            if (customer == null)
            {
                customer = _context
                            .Customers
                            .FirstOrDefault(c => c.Id == customerId);
            }

            return customer;
        }

        public void Update(Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
        }

        public async Task<bool> Save(CancellationToken cancellationToken = default)
        {
            _ = await _context.SaveChangesAsync();

            return true;
        }
    }
}
