using System.ComponentModel.DataAnnotations;

namespace GroceryStoreAPI.DTO
{
    public class CustomerDTO : ICustomerDTO
    {
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
