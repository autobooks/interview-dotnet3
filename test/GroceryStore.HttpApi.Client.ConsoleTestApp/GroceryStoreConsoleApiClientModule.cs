namespace GroceryStore.HttpApi.Client.ConsoleTestApp
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Polly;
    using Volo.Abp.Http.Client;
    using Volo.Abp.Http.Client.IdentityModel;
    using Volo.Abp.Modularity;

    /// <summary>
    /// Defines the <see cref="GroceryStoreConsoleApiClientModule" />.
    /// </summary>
    [DependsOn(typeof(GroceryStoreHttpApiClientModule), typeof(AbpHttpClientIdentityModelModule))]
    public class GroceryStoreConsoleApiClientModule : AbpModule
    {
        /// <summary>
        /// The PreConfigureServices.
        /// </summary>
        /// <param name="context">The context<see cref="ServiceConfigurationContext"/>.</param>
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AbpHttpClientBuilderOptions>(
                options =>
                {
                    options.ProxyClientBuildActions.Add(
                        (remoteServiceName, clientBuilder) =>
                        {
                            clientBuilder.AddTransientHttpErrorPolicy(
                                policyBuilder =>
                                    policyBuilder.WaitAndRetryAsync(
                                        3,
                                        i => TimeSpan.FromSeconds(Math.Pow(2, i))
                                    )
                            );
                        }
                    );
                }
            );
        }
    }
}
