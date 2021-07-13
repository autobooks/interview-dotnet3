using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GroceryStore.Data;
using Volo.Abp.DependencyInjection;

namespace GroceryStore.EntityFrameworkCore
{
    public class EntityFrameworkCoreGroceryStoreDbSchemaMigrator
        : IGroceryStoreDbSchemaMigrator,
          ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreGroceryStoreDbSchemaMigrator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the GroceryStoreMigrationsDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider.GetRequiredService<GroceryStoreMigrationsDbContext>()
                .Database.MigrateAsync();
        }
    }
}
