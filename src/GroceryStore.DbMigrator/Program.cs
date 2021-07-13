namespace GroceryStore.DbMigrator
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using Serilog.Events;

    /// <summary>
	/// Defines the <see cref="Program" />.
	/// </summary>
    internal class Program
    {
        /// <summary>
		/// The Main.
		/// </summary>
		/// <param name="args">The args<see cref="string[]"/>.</param>
		/// <returns>The <see cref="Task"/>.</returns>
        internal static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Volo.Abp", LogEventLevel.Warning)
#if DEBUG
				.MinimumLevel.Override("GroceryStore", LogEventLevel.Debug)
#else
                .MinimumLevel.Override("GroceryStore", LogEventLevel.Information)
#endif
                .Enrich.FromLogContext()
                .WriteTo.Async(c => c.File("Logs/logs.txt"))
                .WriteTo.Async(c => c.Console())
                .CreateLogger();

            await CreateHostBuilder(args).RunConsoleAsync();
        }

        /// <summary>
		/// The CreateHostBuilder.
		/// </summary>
		/// <param name="args">The args<see cref="string[]"/>.</param>
		/// <returns>The <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, logging) => logging.ClearProviders())
                .ConfigureServices(
                    (hostContext, services) =>
                    {
                        services.AddHostedService<DbMigratorHostedService>();
                    }
                );
    }
}
