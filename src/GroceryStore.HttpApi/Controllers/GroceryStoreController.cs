using GroceryStore.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace GroceryStore.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class GroceryStoreController : AbpController
    {
        protected GroceryStoreController()
        {
            LocalizationResource = typeof(GroceryStoreResource);
        }
    }
}
