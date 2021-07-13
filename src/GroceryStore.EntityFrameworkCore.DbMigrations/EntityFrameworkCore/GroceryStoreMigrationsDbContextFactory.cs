namespace GroceryStore.EntityFrameworkCore
{
    using System.IO;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;

    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */

    /// <summary>
    /// Defines the <see cref="GroceryStoreMigrationsDbContextFactory" />.
    /// </summary>
    public class GroceryStoreMigrationsDbContextFactory
        : IDesignTimeDbContextFactory<GroceryStoreMigrationsDbContext>
    {
        /// <summary>
        /// The CreateDbContext.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        /// <returns>The <see cref="GroceryStoreMigrationsDbContext"/>.</returns>
        public GroceryStoreMigrationsDbContext CreateDbContext(string[] args)
        {
            GroceryStoreEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            var builder =
                new DbContextOptionsBuilder<GroceryStoreMigrationsDbContext>().UseSqlServer(
                    configuration.GetConnectionString("Default")
                );

            return new GroceryStoreMigrationsDbContext(builder.Options);
        }

        /// <summary>
        /// The BuildConfiguration.
        /// </summary>
        /// <returns>The <see cref="IConfigurationRoot"/>.</returns>
        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(
                    Path.Combine(Directory.GetCurrentDirectory(), "../GroceryStore.DbMigrator/")
                )
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
