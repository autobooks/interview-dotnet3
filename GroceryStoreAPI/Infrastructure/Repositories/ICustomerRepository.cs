using GroceryStoreAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Infrastructure.Repositories
{
    public interface ICustomerRepository
    {
        Customer Add(Customer customer);

        void Update(Customer customer);

        void Delete(Customer customer);

        Task<Customer> GetAsync(Guid customerId);

        Task<List<Customer>> GetAllAsync();

        Task<bool> Save(CancellationToken cancellationToken = default(CancellationToken));
    }
}