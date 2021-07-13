using GroceryStore.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace GroceryStore
{
    [DependsOn(typeof(GroceryStoreEntityFrameworkCoreTestModule))]
    public class GroceryStoreDomainTestModule : AbpModule
    {
    }
}
