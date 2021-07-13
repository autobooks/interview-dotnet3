namespace GroceryStore.Blazor
{
    using GroceryStore.Localization;
    using Volo.Abp.AspNetCore.Components;

    /// <summary>
	/// Defines the <see cref="GroceryStoreComponentBase" />.
	/// </summary>
    public abstract class GroceryStoreComponentBase : AbpComponentBase
    {
        /// <summary>
		/// Initializes a new instance of the <see cref="GroceryStoreComponentBase"/> class.
		/// </summary>
        protected GroceryStoreComponentBase()
        {
            LocalizationResource = typeof(GroceryStoreResource);
        }
    }
}
