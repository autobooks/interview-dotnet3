using GroceryStore.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace GroceryStore.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(GroceryStoreEntityFrameworkCoreDbMigrationsModule),
        typeof(GroceryStoreApplicationContractsModule)
    )]
    public class GroceryStoreDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
