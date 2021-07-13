using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GroceryStore.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class GroceryStoreMigrationsDbContextFactory
        : IDesignTimeDbContextFactory<GroceryStoreMigrationsDbContext>
    {
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
