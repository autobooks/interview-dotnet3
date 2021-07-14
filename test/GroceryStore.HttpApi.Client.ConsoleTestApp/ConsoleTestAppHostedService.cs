namespace GroceryStore.HttpApi.Client.ConsoleTestApp
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Volo.Abp;

    /// <summary>
    /// Defines the <see cref="ConsoleTestAppHostedService" />.
    /// </summary>
    public class ConsoleTestAppHostedService : IHostedService
    {
        /// <summary>
        /// The StartAsync.
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var application =
                AbpApplicationFactory.Create<GroceryStoreConsoleApiClientModule>();
            application.Initialize();

            var demo = application.ServiceProvider.GetRequiredService<ClientDemoService>();
            await demo.RunAsync();

            application.Shutdown();
        }

        /// <summary>
        /// The StopAsync.
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
