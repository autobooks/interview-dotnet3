namespace GroceryStore
{
    using Microsoft.Extensions.DependencyInjection;
    using Volo.Abp.Account;
    using Volo.Abp.FeatureManagement;
    using Volo.Abp.Identity;
    using Volo.Abp.Modularity;
    using Volo.Abp.PermissionManagement;
    using Volo.Abp.SettingManagement;
    using Volo.Abp.TenantManagement;

    /// <summary>
    /// Defines the <see cref="GroceryStoreHttpApiClientModule" />.
    /// </summary>
    [DependsOn(
        typeof(GroceryStoreApplicationContractsModule),
        typeof(AbpAccountHttpApiClientModule),
        typeof(AbpIdentityHttpApiClientModule),
        typeof(AbpPermissionManagementHttpApiClientModule),
        typeof(AbpTenantManagementHttpApiClientModule),
        typeof(AbpFeatureManagementHttpApiClientModule),
        typeof(AbpSettingManagementHttpApiClientModule)
    )]
    public class GroceryStoreHttpApiClientModule : AbpModule
    {
        /// <summary>
        /// Defines the RemoteServiceName.
        /// </summary>
        public const string RemoteServiceName = "Default";

        /// <summary>
        /// The ConfigureServices.
        /// </summary>
        /// <param name="context">The context<see cref="ServiceConfigurationContext"/>.</param>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(GroceryStoreApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
