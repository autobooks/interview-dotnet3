using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Domain.Models
{
    public class Customer: BaseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Customer(Guid id, string name) {
            Name = name;
            Id = id;
        }
    }

    
}
