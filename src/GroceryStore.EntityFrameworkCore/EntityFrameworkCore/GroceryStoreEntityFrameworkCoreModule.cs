namespace GroceryStore.EntityFrameworkCore
{
    using Microsoft.Extensions.DependencyInjection;
    using Volo.Abp.AuditLogging.EntityFrameworkCore;
    using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
    using Volo.Abp.EntityFrameworkCore;
    using Volo.Abp.EntityFrameworkCore.SqlServer;
    using Volo.Abp.FeatureManagement.EntityFrameworkCore;
    using Volo.Abp.Identity.EntityFrameworkCore;
    using Volo.Abp.IdentityServer.EntityFrameworkCore;
    using Volo.Abp.Modularity;
    using Volo.Abp.PermissionManagement.EntityFrameworkCore;
    using Volo.Abp.SettingManagement.EntityFrameworkCore;
    using Volo.Abp.TenantManagement.EntityFrameworkCore;

    /// <summary>
	/// Defines the <see cref="GroceryStoreEntityFrameworkCoreModule" />.
	/// </summary>
    [DependsOn(
        typeof(GroceryStoreDomainModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpIdentityServerEntityFrameworkCoreModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
        typeof(AbpAuditLoggingEntityFrameworkCoreModule),
        typeof(AbpTenantManagementEntityFrameworkCoreModule),
        typeof(AbpFeatureManagementEntityFrameworkCoreModule)
    )]
    public class GroceryStoreEntityFrameworkCoreModule : AbpModule
    {
        /// <summary>
		/// The PreConfigureServices.
		/// </summary>
		/// <param name="context">The context<see cref="ServiceConfigurationContext"/>.</param>
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            GroceryStoreEfCoreEntityExtensionMappings.Configure();
        }

        /// <summary>
		/// The ConfigureServices.
		/// </summary>
		/// <param name="context">The context<see cref="ServiceConfigurationContext"/>.</param>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<GroceryStoreDbContext>(
                options =>
                {
                    /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
                    options.AddDefaultRepositories(includeAllEntities: true);
                }
            );

            Configure<AbpDbContextOptions>(
                options =>
                {
                    /* The main point to change your DBMS.
                 * See also GroceryStoreMigrationsDbContextFactory for EF Core tooling. */
                    options.UseSqlServer();
                }
            );
        }
    }
}
