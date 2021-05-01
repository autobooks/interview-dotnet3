using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Domain.Models
{
    public class BaseModel
    {
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public void Created()
        {
            CreatedDate = DateTime.Now;

            ModifiedDate = DateTime.Now;
        }
        public void Modified()
        {
            ModifiedDate = DateTime.Now;
        }
    }
}
