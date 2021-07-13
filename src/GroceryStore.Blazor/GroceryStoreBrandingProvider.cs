namespace GroceryStore.Blazor
{
    using Volo.Abp.DependencyInjection;
    using Volo.Abp.Ui.Branding;

    /// <summary>
	/// Defines the <see cref="GroceryStoreBrandingProvider" />.
	/// </summary>
    [Dependency(ReplaceServices = true)]
    public class GroceryStoreBrandingProvider : DefaultBrandingProvider
    {
        /// <summary>
        /// Gets the AppName.
        /// </summary>
        public override string AppName => "GroceryStore";
    }
}
