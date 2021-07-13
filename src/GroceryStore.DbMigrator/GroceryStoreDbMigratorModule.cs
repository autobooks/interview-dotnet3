namespace GroceryStore.DbMigrator
{
    using GroceryStore.EntityFrameworkCore;
    using Volo.Abp.Autofac;
    using Volo.Abp.BackgroundJobs;
    using Volo.Abp.Modularity;

    /// <summary>
	/// Defines the <see cref="GroceryStoreDbMigratorModule" />.
	/// </summary>
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(GroceryStoreEntityFrameworkCoreDbMigrationsModule),
        typeof(GroceryStoreApplicationContractsModule)
    )]
    public class GroceryStoreDbMigratorModule : AbpModule
    {
        /// <summary>
		/// The ConfigureServices.
		/// </summary>
		/// <param name="context">The context<see cref="ServiceConfigurationContext"/>.</param>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
