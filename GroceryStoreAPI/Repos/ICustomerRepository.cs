using System.Collections.Generic;
using System.Threading.Tasks;
using GroceryStoreAPI.Domain;
using GroceryStoreAPI.DTO;

namespace GroceryStoreAPI
{
    public interface ICustomerRepository
    {
        Task<ICustomer> GetById(int id);
        Task<IEnumerable<ICustomer>> GetList();
        Task<ICustomer> Add(CustomerDTO customer);
        Task<ICustomer> Update(int id, CustomerDTO customer);
        Task<bool> Delete(int id);
    }
}