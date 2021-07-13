using Volo.Abp.Modularity;

namespace GroceryStore
{
    [DependsOn(typeof(GroceryStoreApplicationModule), typeof(GroceryStoreDomainTestModule))]
    public class GroceryStoreApplicationTestModule : AbpModule
    {
    }
}
