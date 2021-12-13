using Microsoft.EntityFrameworkCore;
using GroceryStoreAPI.Domain;
using GroceryStoreAPI.Infrastructure;
using System.Threading.Tasks;

namespace GroceryStoreAPI
{
    public partial class GroceryStoreDbContext:DbContext
    {
        protected readonly AppSettings _appSettings;        
        public GroceryStoreDbContext(DbContextOptions dbContextOptions, AppSettings appSettings):base(dbContextOptions)
        {
            _appSettings = appSettings;            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("GroceryStoreDB");
            }
        }

        public DbSet<Customer> Customers { get; set;}
    }
}
