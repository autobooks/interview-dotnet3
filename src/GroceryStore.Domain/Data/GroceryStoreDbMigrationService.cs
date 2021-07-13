namespace GroceryStore.Data
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Volo.Abp.Data;
    using Volo.Abp.DependencyInjection;
    using Volo.Abp.Identity;
    using Volo.Abp.MultiTenancy;
    using Volo.Abp.TenantManagement;

    /// <summary>
	/// Defines the <see cref="GroceryStoreDbMigrationService" />.
	/// </summary>
    public class GroceryStoreDbMigrationService : ITransientDependency
    {
        /// <summary>
		/// Gets or sets the Logger.
		/// </summary>
        public ILogger<GroceryStoreDbMigrationService> Logger { get; set; }

        /// <summary>
		/// Defines the _dataSeeder.
		/// </summary>
        private readonly IDataSeeder _dataSeeder;

        /// <summary>
		/// Defines the _dbSchemaMigrators.
		/// </summary>
        private readonly IEnumerable<IGroceryStoreDbSchemaMigrator> _dbSchemaMigrators;

        /// <summary>
		/// Defines the _tenantRepository.
		/// </summary>
        private readonly ITenantRepository _tenantRepository;

        /// <summary>
		/// Defines the _currentTenant.
		/// </summary>
        private readonly ICurrentTenant _currentTenant;

        /// <summary>
		/// Initializes a new instance of the <see cref="GroceryStoreDbMigrationService"/> class.
		/// </summary>
		/// <param name="dataSeeder">The dataSeeder<see cref="IDataSeeder"/>.</param>
		/// <param name="dbSchemaMigrators">The dbSchemaMigrators<see cref="IEnumerable{IGroceryStoreDbSchemaMigrator}"/>.</param>
		/// <param name="tenantRepository">The tenantRepository<see cref="ITenantRepository"/>.</param>
		/// <param name="currentTenant">The currentTenant<see cref="ICurrentTenant"/>.</param>
        public GroceryStoreDbMigrationService(
            IDataSeeder dataSeeder,
            IEnumerable<IGroceryStoreDbSchemaMigrator> dbSchemaMigrators,
            ITenantRepository tenantRepository,
            ICurrentTenant currentTenant
        ) {
            _dataSeeder = dataSeeder;
            _dbSchemaMigrators = dbSchemaMigrators;
            _tenantRepository = tenantRepository;
            _currentTenant = currentTenant;

            Logger = NullLogger<GroceryStoreDbMigrationService>.Instance;
        }

        /// <summary>
		/// The MigrateAsync.
		/// </summary>
		/// <returns>The <see cref="Task"/>.</returns>
        public async Task MigrateAsync()
        {
            var initialMigrationAdded = AddInitialMigrationIfNotExist();

            if (initialMigrationAdded)
            {
                return;
            }

            Logger.LogInformation("Started database migrations...");

            await MigrateDatabaseSchemaAsync();
            await SeedDataAsync();

            Logger.LogInformation($"Successfully completed host database migrations.");

            var tenants = await _tenantRepository.GetListAsync(includeDetails: true);

            var migratedDatabaseSchemas = new HashSet<string>();
            foreach (var tenant in tenants)
            {
                using (_currentTenant.Change(tenant.Id))
                {
                    if (tenant.ConnectionStrings.Any())
                    {
                        var tenantConnectionStrings = tenant.ConnectionStrings.Select(x => x.Value)
                            .ToList();

                        if (!migratedDatabaseSchemas.IsSupersetOf(tenantConnectionStrings))
                        {
                            await MigrateDatabaseSchemaAsync(tenant);

                            migratedDatabaseSchemas.AddIfNotContains(tenantConnectionStrings);
                        }
                    }

                    await SeedDataAsync(tenant);
                }

                Logger.LogInformation(
                    $"Successfully completed {tenant.Name} tenant database migrations."
                );
            }

            Logger.LogInformation("Successfully completed all database migrations.");
            Logger.LogInformation("You can safely end this process...");
        }

        /// <summary>
		/// The MigrateDatabaseSchemaAsync.
		/// </summary>
		/// <param name="tenant">The tenant<see cref="Tenant"/>.</param>
		/// <returns>The <see cref="Task"/>.</returns>
        private async Task MigrateDatabaseSchemaAsync(Tenant tenant = null)
        {
            Logger.LogInformation(
                $"Migrating schema for {(tenant == null ? "host" : tenant.Name + " tenant")} database..."
            );

            foreach (var migrator in _dbSchemaMigrators)
            {
                await migrator.MigrateAsync();
            }
        }

        /// <summary>
		/// The SeedDataAsync.
		/// </summary>
		/// <param name="tenant">The tenant<see cref="Tenant"/>.</param>
		/// <returns>The <see cref="Task"/>.</returns>
        private async Task SeedDataAsync(Tenant tenant = null)
        {
            Logger.LogInformation(
                $"Executing {(tenant == null ? "host" : tenant.Name + " tenant")} database seed..."
            );

            await _dataSeeder.SeedAsync(
                new DataSeedContext(tenant?.Id).WithProperty(
                        IdentityDataSeedContributor.AdminEmailPropertyName,
                        IdentityDataSeedContributor.AdminEmailDefaultValue
                    )
                    .WithProperty(
                        IdentityDataSeedContributor.AdminPasswordPropertyName,
                        IdentityDataSeedContributor.AdminPasswordDefaultValue
                    )
            );
        }

        /// <summary>
		/// The AddInitialMigrationIfNotExist.
		/// </summary>
		/// <returns>The <see cref="bool"/>.</returns>
        private bool AddInitialMigrationIfNotExist()
        {
            try
            {
                if (!DbMigrationsProjectExists())
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            try
            {
                if (!MigrationsFolderExists())
                {
                    AddInitialMigration();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Logger.LogWarning("Couldn't determinate if any migrations exist : " + e.Message);
                return false;
            }
        }

        /// <summary>
		/// The DbMigrationsProjectExists.
		/// </summary>
		/// <returns>The <see cref="bool"/>.</returns>
        private bool DbMigrationsProjectExists()
        {
            var dbMigrationsProjectFolder = GetDbMigrationsProjectFolderPath();

            return dbMigrationsProjectFolder != null;
        }

        /// <summary>
		/// The MigrationsFolderExists.
		/// </summary>
		/// <returns>The <see cref="bool"/>.</returns>
        private bool MigrationsFolderExists()
        {
            var dbMigrationsProjectFolder = GetDbMigrationsProjectFolderPath();

            return Directory.Exists(Path.Combine(dbMigrationsProjectFolder, "Migrations"));
        }

        /// <summary>
		/// The AddInitialMigration.
		/// </summary>
        private void AddInitialMigration()
        {
            Logger.LogInformation("Creating initial migration...");

            string argumentPrefix;
            string fileName;

            if (
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                || RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
            ) {
                argumentPrefix = "-c";
                fileName = "/bin/bash";
            }
            else
            {
                argumentPrefix = "/C";
                fileName = "cmd.exe";
            }

            var procStartInfo = new ProcessStartInfo(
                fileName,
                $"{argumentPrefix} \"abp create-migration-and-run-migrator \"{GetDbMigrationsProjectFolderPath()}\"\""
            );

            try
            {
                Process.Start(procStartInfo);
            }
            catch (Exception)
            {
                throw new Exception("Couldn't run ABP CLI...");
            }
        }

        /// <summary>
		/// The GetDbMigrationsProjectFolderPath.
		/// </summary>
		/// <returns>The <see cref="string"/>.</returns>
        private string GetDbMigrationsProjectFolderPath()
        {
            var slnDirectoryPath = GetSolutionDirectoryPath();

            if (slnDirectoryPath == null)
            {
                throw new Exception("Solution folder not found!");
            }

            var srcDirectoryPath = Path.Combine(slnDirectoryPath, "src");

            return Directory.GetDirectories(srcDirectoryPath)
                .FirstOrDefault(d => d.EndsWith(".DbMigrations"));
        }

        /// <summary>
		/// The GetSolutionDirectoryPath.
		/// </summary>
		/// <returns>The <see cref="string"/>.</returns>
        private string GetSolutionDirectoryPath()
        {
            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            while (Directory.GetParent(currentDirectory.FullName) != null)
            {
                currentDirectory = Directory.GetParent(currentDirectory.FullName);

                if (
                    Directory.GetFiles(currentDirectory.FullName)
                        .FirstOrDefault(f => f.EndsWith(".sln"))
                    != null
                ) {
                    return currentDirectory.FullName;
                }
            }

            return null;
        }
    }
}
