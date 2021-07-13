namespace GroceryStore.EntityFrameworkCore
{
    using System;
    using System.Threading.Tasks;
    using GroceryStore.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Volo.Abp.DependencyInjection;

    /// <summary>
	/// Defines the <see cref="EntityFrameworkCoreGroceryStoreDbSchemaMigrator" />.
	/// </summary>
    public class EntityFrameworkCoreGroceryStoreDbSchemaMigrator
        : IGroceryStoreDbSchemaMigrator,
          ITransientDependency
    {
        /// <summary>
		/// Defines the _serviceProvider.
		/// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
		/// Initializes a new instance of the <see cref="EntityFrameworkCoreGroceryStoreDbSchemaMigrator"/> class.
		/// </summary>
		/// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        public EntityFrameworkCoreGroceryStoreDbSchemaMigrator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
		/// The MigrateAsync.
		/// </summary>
		/// <returns>The <see cref="Task"/>.</returns>
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
