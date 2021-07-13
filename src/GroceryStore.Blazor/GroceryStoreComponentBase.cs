using GroceryStore.Localization;
using Volo.Abp.AspNetCore.Components;

namespace GroceryStore.Blazor
{
    public abstract class GroceryStoreComponentBase : AbpComponentBase
    {
        protected GroceryStoreComponentBase()
        {
            LocalizationResource = typeof(GroceryStoreResource);
        }
    }
}
