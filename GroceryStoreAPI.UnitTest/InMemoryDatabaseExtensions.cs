using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;

namespace GroceryStoreAPI.UnitTest
{
    public static class InMemoryDatabaseExtensions
    {
        public static void UseInMemoryDatabase(this IServiceCollection services)
        {
            // Create a random name for the database
            string databaseName = "GroceryStoreDBTest" + Guid.NewGuid();                
            
            // Just in case, remove it if it exists
            var optionsDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<GroceryStoreDbContext>));                       
            if (optionsDescriptor != null) services.Remove(optionsDescriptor);

            services.AddDbContext<GroceryStoreDbContext>(options =>
            {   
                options.UseInMemoryDatabase(databaseName);               
            });
        }
      
        public static void SeedDbContext<DbContext>(this IServiceProvider serviceProvider, Action<DbContext> action) where DbContext : GroceryStoreDbContext
        {
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DbContext>();
            action(db);
            db.SaveChanges();
        }

        public static void WithDbContext<DbContext>(this IServiceProvider serviceProvider, Action<DbContext> action) where DbContext : GroceryStoreDbContext
        {
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DbContext>();
            action(db);
        }

        public static void ClearInMemoryDatabase(this IServiceProvider serviceProvider)
        {
            serviceProvider.ClearDbContext<GroceryStoreDbContext>();            
        }

        public static void ClearDbContext<DbContext>(this IServiceProvider serviceProvider) where DbContext : GroceryStoreDbContext
        {
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DbContext>();
            db.Database.EnsureDeleted();
        }
    }
}
