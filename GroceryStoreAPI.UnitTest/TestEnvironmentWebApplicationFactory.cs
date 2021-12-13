using System;
using GroceryStoreAPI.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GroceryStoreAPI.UnitTest
{
    public class TestEnvironmentWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        ILogger<CustomerRepository> _customerRepositoryLogger;           

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var serviceProvider = services.BuildServiceProvider();
                _customerRepositoryLogger = serviceProvider.GetRequiredService<ILogger<CustomerRepository>>();   
                var appSettings = serviceProvider.GetRequiredService<AppSettings>();  
                appSettings.JsonSampleDataFile = "";
                services.UseInMemoryDatabase();                
            });
            
            base.ConfigureWebHost(builder);
        }

          // Put test data in the database
        public void SeedDbContext<DbContext>(Action<DbContext> action) where DbContext : GroceryStoreDbContext 
        {
            Server.Services.SeedDbContext(action);
        }

        public void WithDbContext<DbContext>(Action<DbContext> action) where DbContext : GroceryStoreDbContext
        => Server.Services.WithDbContext(action);

        public void ClearInMemoryDatabase() => Server.Services.ClearInMemoryDatabase();

        public ILogger<CustomerRepository> GetCustomerRepositoryLogger() => _customerRepositoryLogger;
    }
}
