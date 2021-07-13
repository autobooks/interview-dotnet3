using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Volo.Abp.Domain.Entities;

namespace GroceryStore.Customers
{
    public class Customer : Entity<Guid>
    {
        public int LegacyId { get; set; }
        public string Name { get; set; }
    }
}
