namespace GroceryStore.EntityFrameworkCore
{
    using Microsoft.Extensions.DependencyInjection;
    using Volo.Abp.Modularity;

    /// <summary>
	/// Defines the <see cref="GroceryStoreEntityFrameworkCoreDbMigrationsModule" />.
	/// </summary>
    [DependsOn(typeof(GroceryStoreEntityFrameworkCoreModule))]
    public class GroceryStoreEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        /// <summary>
		/// The ConfigureServices.
		/// </summary>
		/// <param name="context">The context<see cref="ServiceConfigurationContext"/>.</param>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<GroceryStoreMigrationsDbContext>();
        }
    }
}
