using GroceryStoreAPI.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroceryStoreAPI.DataAccess
{
    public interface IDataAccess
    {
        /// <summary>
        /// Retrieves a list of all customers.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SelectContract>> SelectAllAsync();

        /// <summary>
        /// Retrieves the of customers associated by a particular id.
        /// </summary>
        /// <returns></returns>
        Task<SelectContract> SelectByIdAsync(Guid customerId);

        /// <summary>
        /// Deletes customers associated by a particular id.
        /// </summary>
        /// <returns></returns>
        Task<bool> DeleteAsync(Guid customerId);

        /// <summary>
        /// Update customer associated by a particular id.
        /// </summary>
        /// <returns></returns>
        Task<bool> UpdateAsync(UpdateContract updateContract);

        /// <summary>
        /// Insert customer associated by a particular id.
        /// </summary>
        /// <returns></returns>
        Task<bool> AddAsync(string name);
    }
}