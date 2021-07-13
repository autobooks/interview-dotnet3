namespace GroceryStore
{
    using GroceryStore.Localization;
    using Volo.Abp.Application.Services;

    /* Inherit your application services from this class.
     */

    /// <summary>
	/// Defines the <see cref="GroceryStoreAppService" />.
	/// </summary>
    public abstract class GroceryStoreAppService : ApplicationService
    {
        /// <summary>
		/// Initializes a new instance of the <see cref="GroceryStoreAppService"/> class.
		/// </summary>
        protected GroceryStoreAppService()
        {
            LocalizationResource = typeof(GroceryStoreResource);
        }
    }
}
