namespace GroceryStore.Permissions
{
    using GroceryStore.Localization;
    using Volo.Abp.Authorization.Permissions;
    using Volo.Abp.Localization;

    /// <summary>
	/// Defines the <see cref="GroceryStorePermissionDefinitionProvider" />.
	/// </summary>
    public class GroceryStorePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        /// <summary>
		/// The Define.
		/// </summary>
		/// <param name="context">The context<see cref="IPermissionDefinitionContext"/>.</param>
        public override void Define(IPermissionDefinitionContext context)
        {
            context.AddGroup(GroceryStorePermissions.GroupName);
        }

        /// <summary>
		/// The L.
		/// </summary>
		/// <param name="name">The name<see cref="string"/>.</param>
		/// <returns>The <see cref="LocalizableString"/>.</returns>
        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<GroceryStoreResource>(name);
        }
    }
}
