namespace GroceryStore.EntityFrameworkCore
{
    using Microsoft.EntityFrameworkCore;
    using Volo.Abp.AuditLogging.EntityFrameworkCore;
    using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
    using Volo.Abp.EntityFrameworkCore;
    using Volo.Abp.FeatureManagement.EntityFrameworkCore;
    using Volo.Abp.Identity.EntityFrameworkCore;
    using Volo.Abp.IdentityServer.EntityFrameworkCore;
    using Volo.Abp.PermissionManagement.EntityFrameworkCore;
    using Volo.Abp.SettingManagement.EntityFrameworkCore;
    using Volo.Abp.TenantManagement.EntityFrameworkCore;

    /* This DbContext is only used for database migrations.
     * It is not used on runtime. See GroceryStoreDbContext for the runtime DbContext.
     * It is a unified model that includes configuration for
     * all used modules and your application.
     */

    /// <summary>
	/// Defines the <see cref="GroceryStoreMigrationsDbContext" />.
	/// </summary>
    public class GroceryStoreMigrationsDbContext : AbpDbContext<GroceryStoreMigrationsDbContext>
    {
        /// <summary>
		/// Initializes a new instance of the <see cref="GroceryStoreMigrationsDbContext"/> class.
		/// </summary>
		/// <param name="options">The options<see cref="DbContextOptions{GroceryStoreMigrationsDbContext}"/>.</param>
        public GroceryStoreMigrationsDbContext(
            DbContextOptions<GroceryStoreMigrationsDbContext> options
        ) : base(options) { }

        /// <summary>
		/// The OnModelCreating.
		/// </summary>
		/// <param name="builder">The builder<see cref="ModelBuilder"/>.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Include modules to your migration db context */

            builder.ConfigurePermissionManagement();
            builder.ConfigureSettingManagement();
            builder.ConfigureBackgroundJobs();
            builder.ConfigureAuditLogging();
            builder.ConfigureIdentity();
            builder.ConfigureIdentityServer();
            builder.ConfigureFeatureManagement();
            builder.ConfigureTenantManagement();

            /* Configure your own tables/entities inside the ConfigureGroceryStore method */

            builder.ConfigureGroceryStore();
        }
    }
}
