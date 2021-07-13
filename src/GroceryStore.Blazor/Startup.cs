namespace GroceryStore.Blazor
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
	/// Defines the <see cref="Startup" />.
	/// </summary>
    public class Startup
    {
        /// <summary>
		/// The ConfigureServices.
		/// </summary>
		/// <param name="services">The services<see cref="IServiceCollection"/>.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<GroceryStoreBlazorModule>();
        }

        /// <summary>
		/// The Configure.
		/// </summary>
		/// <param name="app">The app<see cref="IApplicationBuilder"/>.</param>
		/// <param name="env">The env<see cref="IWebHostEnvironment"/>.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.InitializeApplication();
        }
    }
}
