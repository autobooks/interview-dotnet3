namespace GroceryStore
{
    using GroceryStore.MultiTenancy;
    using Volo.Abp.AuditLogging;
    using Volo.Abp.BackgroundJobs;
    using Volo.Abp.Emailing;
    using Volo.Abp.FeatureManagement;
    using Volo.Abp.Identity;
    using Volo.Abp.IdentityServer;
    using Volo.Abp.Modularity;
    using Volo.Abp.MultiTenancy;
    using Volo.Abp.PermissionManagement.Identity;
    using Volo.Abp.PermissionManagement.IdentityServer;
    using Volo.Abp.SettingManagement;
    using Volo.Abp.TenantManagement;

    /// <summary>
	/// Defines the <see cref="GroceryStoreDomainModule" />.
	/// </summary>
    [DependsOn(
        typeof(GroceryStoreDomainSharedModule),
        typeof(AbpAuditLoggingDomainModule),
        typeof(AbpBackgroundJobsDomainModule),
        typeof(AbpFeatureManagementDomainModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpPermissionManagementDomainIdentityModule),
        typeof(AbpIdentityServerDomainModule),
        typeof(AbpPermissionManagementDomainIdentityServerModule),
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpTenantManagementDomainModule),
        typeof(AbpEmailingModule)
    )]
    public class GroceryStoreDomainModule : AbpModule
    {
        /// <summary>
		/// The ConfigureServices.
		/// </summary>
		/// <param name="context">The context<see cref="ServiceConfigurationContext"/>.</param>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpMultiTenancyOptions>(
                options =>
                {
                    options.IsEnabled = MultiTenancyConsts.IsEnabled;
                }
            );
        }
    }
}
