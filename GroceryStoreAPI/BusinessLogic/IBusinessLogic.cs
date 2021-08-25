using GroceryStoreAPI.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroceryStoreAPI.BusinessLogic
{
    /// <summary>
    /// Business logic.
    /// </summary>
    public interface IBusinessLogic
    {
        /// <summary>
        /// Retrieves a list of all customers.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SelectContract>> GetAllAsync();

        /// <summary>
        /// Retrieves a customers associated by a particular id.
        /// </summary>
        /// <returns></returns>
        Task<SelectContract> GetCustomersByIdAsync(Guid customerId);

        /// <summary>
        /// Deletes a customer associated by a particular id.
        /// </summary>
        /// <returns></returns>
        Task<bool> DeleteAsync(Guid customerId);

        /// <summary>
        /// Updates customer associated by a particular id.
        /// </summary>
        /// <returns></returns>
        Task<bool> UpdateAsync(UpdateContract updateContract);

        /// <summary>
        /// Insert customer.
        /// </summary>
        /// <returns></returns>
        Task<bool> InsertAsync(string name);
    }
}