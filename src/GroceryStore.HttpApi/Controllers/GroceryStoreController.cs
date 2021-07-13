namespace GroceryStore.Controllers
{
    using GroceryStore.Localization;
    using Volo.Abp.AspNetCore.Mvc;

    /* Inherit your controllers from this class.
     */

    /// <summary>
	/// Defines the <see cref="GroceryStoreController" />.
	/// </summary>
    public abstract class GroceryStoreController : AbpController
    {
        /// <summary>
		/// Initializes a new instance of the <see cref="GroceryStoreController"/> class.
		/// </summary>
        protected GroceryStoreController()
        {
            LocalizationResource = typeof(GroceryStoreResource);
        }
    }
}
