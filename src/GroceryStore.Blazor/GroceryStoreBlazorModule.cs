namespace GroceryStore.Blazor
{
    using System;
    using System.IO;
    using System.Net.Http;
    using Blazorise.Bootstrap;
    using Blazorise.Icons.FontAwesome;
    using GroceryStore.Blazor.Menus;
    using GroceryStore.EntityFrameworkCore;
    using GroceryStore.Localization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Volo.Abp;
    using Volo.Abp.Account.Web;
    using Volo.Abp.AspNetCore.Authentication.JwtBearer;
    using Volo.Abp.AspNetCore.Components.Server.BasicTheme;
    using Volo.Abp.AspNetCore.Components.Server.BasicTheme.Bundling;
    using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
    using Volo.Abp.AspNetCore.Mvc;
    using Volo.Abp.AspNetCore.Mvc.Localization;
    using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
    using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
    using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
    using Volo.Abp.AspNetCore.Serilog;
    using Volo.Abp.Autofac;
    using Volo.Abp.AutoMapper;
    using Volo.Abp.Identity.Blazor.Server;
    using Volo.Abp.Localization;
    using Volo.Abp.Modularity;
    using Volo.Abp.SettingManagement.Blazor.Server;
    using Volo.Abp.Swashbuckle;
    using Volo.Abp.TenantManagement.Blazor.Server;
    using Volo.Abp.UI.Navigation;
    using Volo.Abp.UI.Navigation.Urls;
    using Volo.Abp.VirtualFileSystem;

    /// <summary>
	/// Defines the <see cref="GroceryStoreBlazorModule" />.
	/// </summary>
    [DependsOn(
        typeof(GroceryStoreApplicationModule),
        typeof(GroceryStoreEntityFrameworkCoreDbMigrationsModule),
        typeof(GroceryStoreHttpApiModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(AbpAutofacModule),
        typeof(AbpSwashbuckleModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpAccountWebIdentityServerModule),
        typeof(AbpAspNetCoreComponentsServerBasicThemeModule),
        typeof(AbpIdentityBlazorServerModule),
        typeof(AbpTenantManagementBlazorServerModule),
        typeof(AbpSettingManagementBlazorServerModule)
    )]
    public class GroceryStoreBlazorModule : AbpModule
    {
        /// <summary>
		/// The PreConfigureServices.
		/// </summary>
		/// <param name="context">The context<see cref="ServiceConfigurationContext"/>.</param>
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(
                options =>
                {
                    options.AddAssemblyResource(
                        typeof(GroceryStoreResource),
                        typeof(GroceryStoreDomainModule).Assembly,
                        typeof(GroceryStoreDomainSharedModule).Assembly,
                        typeof(GroceryStoreApplicationModule).Assembly,
                        typeof(GroceryStoreApplicationContractsModule).Assembly,
                        typeof(GroceryStoreBlazorModule).Assembly
                    );
                }
            );
        }

        /// <summary>
		/// The ConfigureServices.
		/// </summary>
		/// <param name="context">The context<see cref="ServiceConfigurationContext"/>.</param>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            ConfigureUrls(configuration);
            ConfigureBundles();
            ConfigureAuthentication(context, configuration);
            ConfigureAutoMapper();
            ConfigureVirtualFileSystem(hostingEnvironment);
            ConfigureLocalizationServices();
            ConfigureSwaggerServices(context.Services);
            ConfigureAutoApiControllers();
            ConfigureHttpClient(context);
            ConfigureBlazorise(context);
            ConfigureRouter(context);
            ConfigureMenu(context);
        }

        /// <summary>
		/// The ConfigureUrls.
		/// </summary>
		/// <param name="configuration">The configuration<see cref="IConfiguration"/>.</param>
        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(
                options =>
                {
                    options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
                }
            );
        }

        /// <summary>
		/// The ConfigureBundles.
		/// </summary>
        private void ConfigureBundles()
        {
            Configure<AbpBundlingOptions>(
                options =>
                {
                    // MVC UI
                    options.StyleBundles.Configure(
                        BasicThemeBundles.Styles.Global,
                        bundle =>
                        {
                            bundle.AddFiles("/global-styles.css");
                        }
                    );

                    //BLAZOR UI
                    options.StyleBundles.Configure(
                        BlazorBasicThemeBundles.Styles.Global,
                        bundle =>
                        {
                            bundle.AddFiles("/blazor-global-styles.css");
                            //You can remove the following line if you don't use Blazor CSS isolation for components
                            bundle.AddFiles("/GroceryStore.Blazor.styles.css");
                        }
                    );
                }
            );
        }

        /// <summary>
		/// The ConfigureAuthentication.
		/// </summary>
		/// <param name="context">The context<see cref="ServiceConfigurationContext"/>.</param>
		/// <param name="configuration">The configuration<see cref="IConfiguration"/>.</param>
        private void ConfigureAuthentication(
            ServiceConfigurationContext context,
            IConfiguration configuration
        ) {
            context.Services.AddAuthentication()
                .AddJwtBearer(
                    options =>
                    {
                        options.Authority = configuration["AuthServer:Authority"];
                        options.RequireHttpsMetadata = Convert.ToBoolean(
                            configuration["AuthServer:RequireHttpsMetadata"]
                        );
                        options.Audience = "GroceryStore";
                    }
                );
        }

        /// <summary>
		/// The ConfigureVirtualFileSystem.
		/// </summary>
		/// <param name="hostingEnvironment">The hostingEnvironment<see cref="IWebHostEnvironment"/>.</param>
        private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(
                    options =>
                    {
                        options.FileSets.ReplaceEmbeddedByPhysical<GroceryStoreDomainSharedModule>(
                            Path.Combine(
                                hostingEnvironment.ContentRootPath,
                                $"..{Path.DirectorySeparatorChar}GroceryStore.Domain.Shared"
                            )
                        );
                        options.FileSets.ReplaceEmbeddedByPhysical<GroceryStoreDomainModule>(
                            Path.Combine(
                                hostingEnvironment.ContentRootPath,
                                $"..{Path.DirectorySeparatorChar}GroceryStore.Domain"
                            )
                        );
                        options.FileSets.ReplaceEmbeddedByPhysical<GroceryStoreApplicationContractsModule>(
                            Path.Combine(
                                hostingEnvironment.ContentRootPath,
                                $"..{Path.DirectorySeparatorChar}GroceryStore.Application.Contracts"
                            )
                        );
                        options.FileSets.ReplaceEmbeddedByPhysical<GroceryStoreApplicationModule>(
                            Path.Combine(
                                hostingEnvironment.ContentRootPath,
                                $"..{Path.DirectorySeparatorChar}GroceryStore.Application"
                            )
                        );
                        options.FileSets.ReplaceEmbeddedByPhysical<GroceryStoreBlazorModule>(
                            hostingEnvironment.ContentRootPath
                        );
                    }
                );
            }
        }

        /// <summary>
		/// The ConfigureLocalizationServices.
		/// </summary>
        private void ConfigureLocalizationServices()
        {
            Configure<AbpLocalizationOptions>(
                options =>
                {
                    options.Languages.Add(new LanguageInfo("ar", "ar", "العربية"));
                    options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
                    options.Languages.Add(new LanguageInfo("en", "en", "English"));
                    options.Languages.Add(new LanguageInfo("en-GB", "en-GB", "English (UK)"));
                    options.Languages.Add(new LanguageInfo("hu", "hu", "Magyar"));
                    options.Languages.Add(new LanguageInfo("fr", "fr", "Français"));
                    options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
                    options.Languages.Add(new LanguageInfo("ru", "ru", "Русский"));
                    options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
                    options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
                    options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
                    options.Languages.Add(new LanguageInfo("de-DE", "de-DE", "Deutsch", "de"));
                    options.Languages.Add(new LanguageInfo("es", "es", "Español"));
                }
            );
        }

        /// <summary>
		/// The ConfigureSwaggerServices.
		/// </summary>
		/// <param name="services">The services<see cref="IServiceCollection"/>.</param>
        private void ConfigureSwaggerServices(IServiceCollection services)
        {
            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc(
                        "v1",
                        new OpenApiInfo { Title = "GroceryStore API", Version = "v1" }
                    );
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                }
            );
        }

        /// <summary>
		/// The ConfigureHttpClient.
		/// </summary>
		/// <param name="context">The context<see cref="ServiceConfigurationContext"/>.</param>
        private static void ConfigureHttpClient(ServiceConfigurationContext context)
        {
            context.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri("/") });
        }

        /// <summary>
		/// The ConfigureBlazorise.
		/// </summary>
		/// <param name="context">The context<see cref="ServiceConfigurationContext"/>.</param>
        private void ConfigureBlazorise(ServiceConfigurationContext context)
        {
            context.Services.AddBootstrapProviders().AddFontAwesomeIcons();
        }

        /// <summary>
		/// The ConfigureMenu.
		/// </summary>
		/// <param name="context">The context<see cref="ServiceConfigurationContext"/>.</param>
        private void ConfigureMenu(ServiceConfigurationContext context)
        {
            Configure<AbpNavigationOptions>(
                options =>
                {
                    options.MenuContributors.Add(new GroceryStoreMenuContributor());
                }
            );
        }

        /// <summary>
		/// The ConfigureRouter.
		/// </summary>
		/// <param name="context">The context<see cref="ServiceConfigurationContext"/>.</param>
        private void ConfigureRouter(ServiceConfigurationContext context)
        {
            Configure<AbpRouterOptions>(
                options =>
                {
                    options.AppAssembly = typeof(GroceryStoreBlazorModule).Assembly;
                }
            );
        }

        /// <summary>
		/// The ConfigureAutoApiControllers.
		/// </summary>
        private void ConfigureAutoApiControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(
                options =>
                {
                    options.ConventionalControllers.Create(
                        typeof(GroceryStoreApplicationModule).Assembly
                    );
                }
            );
        }

        /// <summary>
		/// The ConfigureAutoMapper.
		/// </summary>
        private void ConfigureAutoMapper()
        {
            Configure<AbpAutoMapperOptions>(
                options =>
                {
                    options.AddMaps<GroceryStoreBlazorModule>();
                }
            );
        }

        /// <summary>
		/// The OnApplicationInitialization.
		/// </summary>
		/// <param name="context">The context<see cref="ApplicationInitializationContext"/>.</param>
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var env = context.GetEnvironment();
            var app = context.GetApplicationBuilder();

            app.UseAbpRequestLocalization();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            app.UseMultiTenancy();

            app.UseUnitOfWork();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseAbpSwaggerUI(
                options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "GroceryStore API");
                }
            );
            app.UseConfiguredEndpoints();
        }
    }
}
