namespace GroceryStore
{
    using Volo.Abp.Account;
    using Volo.Abp.AutoMapper;
    using Volo.Abp.FeatureManagement;
    using Volo.Abp.Identity;
    using Volo.Abp.Modularity;
    using Volo.Abp.PermissionManagement;
    using Volo.Abp.SettingManagement;
    using Volo.Abp.TenantManagement;

    /// <summary>
	/// Defines the <see cref="GroceryStoreApplicationModule" />.
	/// </summary>
    [DependsOn(
        typeof(GroceryStoreDomainModule),
        typeof(AbpAccountApplicationModule),
        typeof(GroceryStoreApplicationContractsModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpTenantManagementApplicationModule),
        typeof(AbpFeatureManagementApplicationModule),
        typeof(AbpSettingManagementApplicationModule)
    )]
    public class GroceryStoreApplicationModule : AbpModule
    {
        /// <summary>
		/// The ConfigureServices.
		/// </summary>
		/// <param name="context">The context<see cref="ServiceConfigurationContext"/>.</param>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(
                options =>
                {
                    options.AddMaps<GroceryStoreApplicationModule>();
                }
            );
        }
    }
}
