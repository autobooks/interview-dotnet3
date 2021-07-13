namespace GroceryStore.DbMigrator
{
    using System.Threading;
    using System.Threading.Tasks;
    using GroceryStore.Data;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Volo.Abp;

    /// <summary>
	/// Defines the <see cref="DbMigratorHostedService" />.
	/// </summary>
    public class DbMigratorHostedService : IHostedService
    {
        /// <summary>
		/// Defines the _hostApplicationLifetime.
		/// </summary>
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        /// <summary>
		/// Initializes a new instance of the <see cref="DbMigratorHostedService"/> class.
		/// </summary>
		/// <param name="hostApplicationLifetime">The hostApplicationLifetime<see cref="IHostApplicationLifetime"/>.</param>
        public DbMigratorHostedService(IHostApplicationLifetime hostApplicationLifetime)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        /// <summary>
		/// The StartAsync.
		/// </summary>
		/// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
		/// <returns>The <see cref="Task"/>.</returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (
                var application = AbpApplicationFactory.Create<GroceryStoreDbMigratorModule>(
                    options =>
                    {
                        options.UseAutofac();
                        options.Services.AddLogging(c => c.AddSerilog());
                    }
                )
            ) {
                application.Initialize();

                await application.ServiceProvider.GetRequiredService<GroceryStoreDbMigrationService>()
                    .MigrateAsync();

                application.Shutdown();

                _hostApplicationLifetime.StopApplication();
            }
        }

        /// <summary>
		/// The StopAsync.
		/// </summary>
		/// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
		/// <returns>The <see cref="Task"/>.</returns>
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
