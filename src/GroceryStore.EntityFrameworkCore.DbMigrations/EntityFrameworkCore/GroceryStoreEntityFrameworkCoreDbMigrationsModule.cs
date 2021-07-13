using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace GroceryStore.EntityFrameworkCore
{
    [DependsOn(typeof(GroceryStoreEntityFrameworkCoreModule))]
    public class GroceryStoreEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<GroceryStoreMigrationsDbContext>();
        }
    }
}
