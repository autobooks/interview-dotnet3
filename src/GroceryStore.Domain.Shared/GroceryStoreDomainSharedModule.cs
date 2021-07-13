﻿using GroceryStore.Localization;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace GroceryStore
{
    [DependsOn(
        typeof(AbpAuditLoggingDomainSharedModule),
        typeof(AbpBackgroundJobsDomainSharedModule),
        typeof(AbpFeatureManagementDomainSharedModule),
        typeof(AbpIdentityDomainSharedModule),
        typeof(AbpIdentityServerDomainSharedModule),
        typeof(AbpPermissionManagementDomainSharedModule),
        typeof(AbpSettingManagementDomainSharedModule),
        typeof(AbpTenantManagementDomainSharedModule)
    )]
    public class GroceryStoreDomainSharedModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            GroceryStoreGlobalFeatureConfigurator.Configure();
            GroceryStoreModuleExtensionConfigurator.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(
                options =>
                {
                    options.FileSets.AddEmbedded<GroceryStoreDomainSharedModule>();
                }
            );

            Configure<AbpLocalizationOptions>(
                options =>
                {
                    options.Resources.Add<GroceryStoreResource>("en")
                        .AddBaseTypes(typeof(AbpValidationResource))
                        .AddVirtualJson("/Localization/GroceryStore");

                    options.DefaultResourceType = typeof(GroceryStoreResource);
                }
            );

            Configure<AbpExceptionLocalizationOptions>(
                options =>
                {
                    options.MapCodeNamespace("GroceryStore", typeof(GroceryStoreResource));
                }
            );
        }
    }
}
