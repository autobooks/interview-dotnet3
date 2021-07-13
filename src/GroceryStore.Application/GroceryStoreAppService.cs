using System;
using System.Collections.Generic;
using System.Text;
using GroceryStore.Localization;
using Volo.Abp.Application.Services;

namespace GroceryStore
{
    /* Inherit your application services from this class.
     */
    public abstract class GroceryStoreAppService : ApplicationService
    {
        protected GroceryStoreAppService()
        {
            LocalizationResource = typeof(GroceryStoreResource);
        }
    }
}
