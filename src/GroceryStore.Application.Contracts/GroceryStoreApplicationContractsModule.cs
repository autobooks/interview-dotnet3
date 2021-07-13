namespace GroceryStore
{
    using Volo.Abp.Account;
    using Volo.Abp.FeatureManagement;
    using Volo.Abp.Identity;
    using Volo.Abp.Modularity;
    using Volo.Abp.ObjectExtending;
    using Volo.Abp.PermissionManagement;
    using Volo.Abp.SettingManagement;
    using Volo.Abp.TenantManagement;

    /// <summary>
	/// Defines the <see cref="GroceryStoreApplicationContractsModule" />.
	/// </summary>
    [DependsOn(
        typeof(GroceryStoreDomainSharedModule),
        typeof(AbpAccountApplicationContractsModule),
        typeof(AbpFeatureManagementApplicationContractsModule),
        typeof(AbpIdentityApplicationContractsModule),
        typeof(AbpPermissionManagementApplicationContractsModule),
        typeof(AbpSettingManagementApplicationContractsModule),
        typeof(AbpTenantManagementApplicationContractsModule),
        typeof(AbpObjectExtendingModule)
    )]
    public class GroceryStoreApplicationContractsModule : AbpModule
    {
        /// <summary>
		/// The PreConfigureServices.
		/// </summary>
		/// <param name="context">The context<see cref="ServiceConfigurationContext"/>.</param>
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            GroceryStoreDtoExtensions.Configure();
        }
    }
}
