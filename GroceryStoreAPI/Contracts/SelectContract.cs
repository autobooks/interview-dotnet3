using GroceryStoreAPI.DataAccess.Tables;
using System;
using System.Collections.Generic;

namespace GroceryStoreAPI.Contracts
{
    public class SelectContract
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}