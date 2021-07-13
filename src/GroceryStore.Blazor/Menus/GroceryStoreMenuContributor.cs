namespace GroceryStore.Blazor.Menus
{
    using System.Threading.Tasks;
    using GroceryStore.Localization;
    using Volo.Abp.Identity.Blazor;
    using Volo.Abp.SettingManagement.Blazor.Menus;
    using Volo.Abp.TenantManagement.Blazor.Navigation;
    using Volo.Abp.UI.Navigation;

    /// <summary>
	/// Defines the <see cref="GroceryStoreMenuContributor" />.
	/// </summary>
    public class GroceryStoreMenuContributor : IMenuContributor
    {
        /// <summary>
		/// The ConfigureMenuAsync.
		/// </summary>
		/// <param name="context">The context<see cref="MenuConfigurationContext"/>.</param>
		/// <returns>The <see cref="Task"/>.</returns>
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        /// <summary>
		/// The ConfigureMainMenuAsync.
		/// </summary>
		/// <param name="context">The context<see cref="MenuConfigurationContext"/>.</param>
		/// <returns>The <see cref="Task"/>.</returns>
        private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var administration = context.Menu.GetAdministration();
            var l = context.GetLocalizer<GroceryStoreResource>();

            context.Menu.Items.Insert(
                0,
                new ApplicationMenuItem(
                    GroceryStoreMenus.Home,
                    l["Menu:Home"],
                    "/",
                    icon: "fas fa-home",
                    order: 0
                )
            );

            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);

            administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
            administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);

            return Task.CompletedTask;
        }
    }
}
