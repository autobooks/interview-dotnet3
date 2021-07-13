namespace GroceryStore
{
    using global::Localization.Resources.AbpUi;
    using GroceryStore.Localization;
    using Volo.Abp.Account;
    using Volo.Abp.FeatureManagement;
    using Volo.Abp.Identity;
    using Volo.Abp.Localization;
    using Volo.Abp.Modularity;
    using Volo.Abp.PermissionManagement.HttpApi;
    using Volo.Abp.SettingManagement;
    using Volo.Abp.TenantManagement;

    /// <summary>
    /// Defines the <see cref="GroceryStoreHttpApiModule" />.
    /// </summary>
    [DependsOn(
        typeof(GroceryStoreApplicationContractsModule),
        typeof(AbpAccountHttpApiModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpTenantManagementHttpApiModule),
        typeof(AbpFeatureManagementHttpApiModule),
        typeof(AbpSettingManagementHttpApiModule)
    )]
    public class GroceryStoreHttpApiModule : AbpModule
    {
        /// <summary>
        /// The ConfigureServices.
        /// </summary>
        /// <param name="context">The context<see cref="ServiceConfigurationContext"/>.</param>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureLocalization();
        }

        /// <summary>
        /// The ConfigureLocalization.
        /// </summary>
        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(
                options =>
                {
                    options.Resources.Get<GroceryStoreResource>()
                        .AddBaseTypes(typeof(AbpUiResource));
                }
            );
        }
    }
}
