using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using GroceryStoreAPI.Infrastructure;

namespace GroceryStoreAPI
{
    public class Startup
    {        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppSettings = Configuration.GetSection("DemoSettings").Get<AppSettings>();
        }

        public IConfiguration Configuration { get; }
        private AppSettings AppSettings { get;}

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GroceryStoreAPI", Version = "v1" });
            });
        
            services.AddDbContext<GroceryStoreDbContext>();                

            // Dependency injection for the repository
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            // Use a singleton for settings for better performance
            if (AppSettings != null)
            {
                services.AddSingleton(AppSettings);
            }            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GroceryStoreAPI v1"));    
                  
                using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var context = serviceScope.ServiceProvider.GetRequiredService<GroceryStoreDbContext>();            
                DataLoader.Load(context, AppSettings).Wait();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });                                  
        }
    }
}
