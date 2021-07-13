namespace GroceryStore.IdentityServer
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using IdentityServer4.Models;
    using Microsoft.Extensions.Configuration;
    using Volo.Abp.Authorization.Permissions;
    using Volo.Abp.Data;
    using Volo.Abp.DependencyInjection;
    using Volo.Abp.Guids;
    using Volo.Abp.IdentityServer.ApiResources;
    using Volo.Abp.IdentityServer.ApiScopes;
    using Volo.Abp.IdentityServer.Clients;
    using Volo.Abp.IdentityServer.IdentityResources;
    using Volo.Abp.MultiTenancy;
    using Volo.Abp.PermissionManagement;
    using Volo.Abp.Uow;
    using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;
    using ApiScope = Volo.Abp.IdentityServer.ApiScopes.ApiScope;
    using Client = Volo.Abp.IdentityServer.Clients.Client;

    /// <summary>
	/// Defines the <see cref="IdentityServerDataSeedContributor" />.
	/// </summary>
    public class IdentityServerDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        /// <summary>
		/// Defines the _apiResourceRepository.
		/// </summary>
        private readonly IApiResourceRepository _apiResourceRepository;

        /// <summary>
		/// Defines the _apiScopeRepository.
		/// </summary>
        private readonly IApiScopeRepository _apiScopeRepository;

        /// <summary>
		/// Defines the _clientRepository.
		/// </summary>
        private readonly IClientRepository _clientRepository;

        /// <summary>
		/// Defines the _identityResourceDataSeeder.
		/// </summary>
        private readonly IIdentityResourceDataSeeder _identityResourceDataSeeder;

        /// <summary>
		/// Defines the _guidGenerator.
		/// </summary>
        private readonly IGuidGenerator _guidGenerator;

        /// <summary>
		/// Defines the _permissionDataSeeder.
		/// </summary>
        private readonly IPermissionDataSeeder _permissionDataSeeder;

        /// <summary>
		/// Defines the _configuration.
		/// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
		/// Defines the _currentTenant.
		/// </summary>
        private readonly ICurrentTenant _currentTenant;

        /// <summary>
		/// Initializes a new instance of the <see cref="IdentityServerDataSeedContributor"/> class.
		/// </summary>
		/// <param name="clientRepository">The clientRepository<see cref="IClientRepository"/>.</param>
		/// <param name="apiResourceRepository">The apiResourceRepository<see cref="IApiResourceRepository"/>.</param>
		/// <param name="apiScopeRepository">The apiScopeRepository<see cref="IApiScopeRepository"/>.</param>
		/// <param name="identityResourceDataSeeder">The identityResourceDataSeeder<see cref="IIdentityResourceDataSeeder"/>.</param>
		/// <param name="guidGenerator">The guidGenerator<see cref="IGuidGenerator"/>.</param>
		/// <param name="permissionDataSeeder">The permissionDataSeeder<see cref="IPermissionDataSeeder"/>.</param>
		/// <param name="configuration">The configuration<see cref="IConfiguration"/>.</param>
		/// <param name="currentTenant">The currentTenant<see cref="ICurrentTenant"/>.</param>
        public IdentityServerDataSeedContributor(
            IClientRepository clientRepository,
            IApiResourceRepository apiResourceRepository,
            IApiScopeRepository apiScopeRepository,
            IIdentityResourceDataSeeder identityResourceDataSeeder,
            IGuidGenerator guidGenerator,
            IPermissionDataSeeder permissionDataSeeder,
            IConfiguration configuration,
            ICurrentTenant currentTenant
        ) {
            _clientRepository = clientRepository;
            _apiResourceRepository = apiResourceRepository;
            _apiScopeRepository = apiScopeRepository;
            _identityResourceDataSeeder = identityResourceDataSeeder;
            _guidGenerator = guidGenerator;
            _permissionDataSeeder = permissionDataSeeder;
            _configuration = configuration;
            _currentTenant = currentTenant;
        }

        /// <summary>
		/// The SeedAsync.
		/// </summary>
		/// <param name="context">The context<see cref="DataSeedContext"/>.</param>
		/// <returns>The <see cref="Task"/>.</returns>
        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            using (_currentTenant.Change(context?.TenantId))
            {
                await _identityResourceDataSeeder.CreateStandardResourcesAsync();
                await CreateApiResourcesAsync();
                await CreateApiScopesAsync();
                await CreateClientsAsync();
            }
        }

        /// <summary>
		/// The CreateApiScopesAsync.
		/// </summary>
		/// <returns>The <see cref="Task"/>.</returns>
        private async Task CreateApiScopesAsync()
        {
            await CreateApiScopeAsync("GroceryStore");
        }

        /// <summary>
		/// The CreateApiResourcesAsync.
		/// </summary>
		/// <returns>The <see cref="Task"/>.</returns>
        private async Task CreateApiResourcesAsync()
        {
            var commonApiUserClaims = new[]
            {
                "email",
                "email_verified",
                "name",
                "phone_number",
                "phone_number_verified",
                "role"
            };

            await CreateApiResourceAsync("GroceryStore", commonApiUserClaims);
        }

        /// <summary>
		/// The CreateApiResourceAsync.
		/// </summary>
		/// <param name="name">The name<see cref="string"/>.</param>
		/// <param name="claims">The claims<see cref="IEnumerable{string}"/>.</param>
		/// <returns>The <see cref="Task{ApiResource}"/>.</returns>
        private async Task<ApiResource> CreateApiResourceAsync(
            string name,
            IEnumerable<string> claims
        ) {
            var apiResource = await _apiResourceRepository.FindByNameAsync(name);
            if (apiResource == null)
            {
                apiResource = await _apiResourceRepository.InsertAsync(
                    new ApiResource(_guidGenerator.Create(), name, name + " API"),
                    autoSave: true
                );
            }

            foreach (var claim in claims)
            {
                if (apiResource.FindClaim(claim) == null)
                {
                    apiResource.AddUserClaim(claim);
                }
            }

            return await _apiResourceRepository.UpdateAsync(apiResource);
        }

        /// <summary>
		/// The CreateApiScopeAsync.
		/// </summary>
		/// <param name="name">The name<see cref="string"/>.</param>
		/// <returns>The <see cref="Task{ApiScope}"/>.</returns>
        private async Task<ApiScope> CreateApiScopeAsync(string name)
        {
            var apiScope = await _apiScopeRepository.GetByNameAsync(name);
            if (apiScope == null)
            {
                apiScope = await _apiScopeRepository.InsertAsync(
                    new ApiScope(_guidGenerator.Create(), name, name + " API"),
                    autoSave: true
                );
            }

            return apiScope;
        }

        /// <summary>
		/// The CreateClientsAsync.
		/// </summary>
		/// <returns>The <see cref="Task"/>.</returns>
        private async Task CreateClientsAsync()
        {
            var commonScopes = new[]
            {
                "email",
                "openid",
                "profile",
                "role",
                "phone",
                "address",
                "GroceryStore"
            };

            var configurationSection = _configuration.GetSection("IdentityServer:Clients");

            //Console Test / Angular Client
            var consoleAndAngularClientId = configurationSection["GroceryStore_App:ClientId"];
            if (!consoleAndAngularClientId.IsNullOrWhiteSpace())
            {
                var webClientRootUrl =
                    configurationSection["GroceryStore_App:RootUrl"]?.TrimEnd('/');

                await CreateClientAsync(
                    name: consoleAndAngularClientId,
                    scopes: commonScopes,
                    grantTypes: new[] { "password", "client_credentials", "authorization_code" },
                    secret: (
                        configurationSection["GroceryStore_App:ClientSecret"] ?? "1q2w3e*"
                    ).Sha256(),
                    requireClientSecret: false,
                    redirectUri: webClientRootUrl,
                    postLogoutRedirectUri: webClientRootUrl,
                    corsOrigins: new[] { webClientRootUrl.RemovePostFix("/") }
                );
            }

            // Swagger Client
            var swaggerClientId = configurationSection["GroceryStore_Swagger:ClientId"];
            if (!swaggerClientId.IsNullOrWhiteSpace())
            {
                var swaggerRootUrl = configurationSection["GroceryStore_Swagger:RootUrl"].TrimEnd(
                    '/'
                );

                await CreateClientAsync(
                    name: swaggerClientId,
                    scopes: commonScopes,
                    grantTypes: new[] { "authorization_code" },
                    secret: configurationSection["GroceryStore_Swagger:ClientSecret"]?.Sha256(),
                    requireClientSecret: false,
                    redirectUri: $"{swaggerRootUrl}/swagger/oauth2-redirect.html",
                    corsOrigins: new[] { swaggerRootUrl.RemovePostFix("/") }
                );
            }
        }

        /// <summary>
		/// The CreateClientAsync.
		/// </summary>
		/// <param name="name">The name<see cref="string"/>.</param>
		/// <param name="scopes">The scopes<see cref="IEnumerable{string}"/>.</param>
		/// <param name="grantTypes">The grantTypes<see cref="IEnumerable{string}"/>.</param>
		/// <param name="secret">The secret<see cref="string"/>.</param>
		/// <param name="redirectUri">The redirectUri<see cref="string"/>.</param>
		/// <param name="postLogoutRedirectUri">The postLogoutRedirectUri<see cref="string"/>.</param>
		/// <param name="frontChannelLogoutUri">The frontChannelLogoutUri<see cref="string"/>.</param>
		/// <param name="requireClientSecret">The requireClientSecret<see cref="bool"/>.</param>
		/// <param name="requirePkce">The requirePkce<see cref="bool"/>.</param>
		/// <param name="permissions">The permissions<see cref="IEnumerable{string}"/>.</param>
		/// <param name="corsOrigins">The corsOrigins<see cref="IEnumerable{string}"/>.</param>
		/// <returns>The <see cref="Task{Client}"/>.</returns>
        private async Task<Client> CreateClientAsync(
            string name,
            IEnumerable<string> scopes,
            IEnumerable<string> grantTypes,
            string secret = null,
            string redirectUri = null,
            string postLogoutRedirectUri = null,
            string frontChannelLogoutUri = null,
            bool requireClientSecret = true,
            bool requirePkce = false,
            IEnumerable<string> permissions = null,
            IEnumerable<string> corsOrigins = null
        ) {
            var client = await _clientRepository.FindByClientIdAsync(name);
            if (client == null)
            {
                client = await _clientRepository.InsertAsync(
                    new Client(_guidGenerator.Create(), name)
                    {
                        ClientName = name,
                        ProtocolType = "oidc",
                        Description = name,
                        AlwaysIncludeUserClaimsInIdToken = true,
                        AllowOfflineAccess = true,
                        AbsoluteRefreshTokenLifetime = 31536000, //365 days
                        AccessTokenLifetime = 31536000, //365 days
                        AuthorizationCodeLifetime = 300,
                        IdentityTokenLifetime = 300,
                        RequireConsent = false,
                        FrontChannelLogoutUri = frontChannelLogoutUri,
                        RequireClientSecret = requireClientSecret,
                        RequirePkce = requirePkce
                    },
                    autoSave: true
                );
            }

            foreach (var scope in scopes)
            {
                if (client.FindScope(scope) == null)
                {
                    client.AddScope(scope);
                }
            }

            foreach (var grantType in grantTypes)
            {
                if (client.FindGrantType(grantType) == null)
                {
                    client.AddGrantType(grantType);
                }
            }

            if (!secret.IsNullOrEmpty())
            {
                if (client.FindSecret(secret) == null)
                {
                    client.AddSecret(secret);
                }
            }

            if (redirectUri != null)
            {
                if (client.FindRedirectUri(redirectUri) == null)
                {
                    client.AddRedirectUri(redirectUri);
                }
            }

            if (postLogoutRedirectUri != null)
            {
                if (client.FindPostLogoutRedirectUri(postLogoutRedirectUri) == null)
                {
                    client.AddPostLogoutRedirectUri(postLogoutRedirectUri);
                }
            }

            if (permissions != null)
            {
                await _permissionDataSeeder.SeedAsync(
                    ClientPermissionValueProvider.ProviderName,
                    name,
                    permissions,
                    null
                );
            }

            if (corsOrigins != null)
            {
                foreach (var origin in corsOrigins)
                {
                    if (!origin.IsNullOrWhiteSpace() && client.FindCorsOrigin(origin) == null)
                    {
                        client.AddCorsOrigin(origin);
                    }
                }
            }

            return await _clientRepository.UpdateAsync(client);
        }
    }
}
