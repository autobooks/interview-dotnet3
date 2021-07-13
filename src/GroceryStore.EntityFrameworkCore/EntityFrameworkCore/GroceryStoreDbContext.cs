namespace GroceryStore.EntityFrameworkCore
{
    using GroceryStore.Customers;
    using GroceryStore.Users;
    using Microsoft.EntityFrameworkCore;
    using Volo.Abp.Data;
    using Volo.Abp.EntityFrameworkCore;
    using Volo.Abp.EntityFrameworkCore.Modeling;
    using Volo.Abp.Identity;
    using Volo.Abp.Users.EntityFrameworkCore;

    /* This is your actual DbContext used on runtime.
     * It includes only your entities.
     * It does not include entities of the used modules, because each module has already
     * its own DbContext class. If you want to share some database tables with the used modules,
     * just create a structure like done for AppUser.
     *
     * Don't use this DbContext for database migrations since it does not contain tables of the
     * used modules (as explained above). See GroceryStoreMigrationsDbContext for migrations.
     */

    /// <summary>
	/// Defines the <see cref="GroceryStoreDbContext" />.
	/// </summary>
    [ConnectionStringName("Default")]
    public class GroceryStoreDbContext : AbpDbContext<GroceryStoreDbContext>
    {
        /// <summary>
		/// Gets or sets the Users.
		/// </summary>
        public DbSet<AppUser> Users { get; set; }

        /* Add DbSet properties for your Aggregate Roots / Entities here.
         * Also map them inside GroceryStoreDbContextModelCreatingExtensions.ConfigureGroceryStore
         */

        /// <summary>
		/// Gets or sets the Customers.
		/// </summary>
        public DbSet<Customer> Customers { get; set; }

        /// <summary>
		/// Initializes a new instance of the <see cref="GroceryStoreDbContext"/> class.
		/// </summary>
		/// <param name="options">The options<see cref="DbContextOptions{GroceryStoreDbContext}"/>.</param>
        public GroceryStoreDbContext(DbContextOptions<GroceryStoreDbContext> options)
            : base(options) { }

        /// <summary>
		/// The OnModelCreating.
		/// </summary>
		/// <param name="builder">The builder<see cref="ModelBuilder"/>.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Configure the shared tables (with included modules) here */

            builder.Entity<AppUser>(
                b =>
                {
                    b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "Users"); //Sharing the same table "AbpUsers" with the IdentityUser

                    b.ConfigureByConvention();
                    b.ConfigureAbpUser();
                    /* Configure mappings for your additional properties
                 * Also see the GroceryStoreEfCoreEntityExtensionMappings class
                 */
                }
            );

            /* Configure your own tables/entities inside the ConfigureGroceryStore method */

            builder.ConfigureGroceryStore();
        }
    }
}
