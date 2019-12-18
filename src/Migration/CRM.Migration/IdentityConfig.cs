using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CRM.Migration
{
    public static class IdentityConfig
    {
        public static async Task SeedData(string connString)
        {
            var services = new ServiceCollection();
            services.AddOperationalDbContext(options =>
            {
                options.ConfigureDbContext = db => db.UseNpgsql(connString);
            });
            services.AddConfigurationDbContext(options =>
            {
                options.ConfigureDbContext = db => db.UseNpgsql(connString);
            });

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();

                Log.Information("Seed identity resource");
                foreach (var resource in IdentityConfig.GetIdentityResources())
                {
                    if (!await context.IdentityResources.AnyAsync(x => x.Name == resource.Name))
                    {
                        await context.IdentityResources.AddAsync(resource.ToEntity());
                    }
                }

                Log.Information("Seed API resource");
                foreach (var api in IdentityConfig.GetApis())
                {
                    if (!await context.ApiResources.AnyAsync(x => x.Name == api.Name))
                    {
                        await context.ApiResources.AddAsync(api.ToEntity());
                    }
                }

                Log.Information("Seed Client resource");
                foreach (var client in GetClients())
                {
                    if (!await context.Clients.AnyAsync(x => x.ClientId == client.ClientId))
                    {
                        await context.Clients.AddAsync(client.ToEntity());
                    }
                }

                context.SaveChanges();
            }
        }

        static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource("contact", "Contact API"),
                new ApiResource("graphQL-gateway", "GraphQL gateway"),
                new ApiResource("communication", "Communication API")
            };
        }

        static IEnumerable<Client> GetClients()
        {
            return new[] {
                new Client() {
                    ClientId = "CRM-SPA",
                    ClientName = "CRM SPA",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = true,
                    PostLogoutRedirectUris = {
                        "http://localhost:5102",
                        "http://crmnow.tk"
                    },
                    RedirectUris = {
                        "http://localhost:5102/authentication/callback",
                        "http://crmnow.tk/authentication/callback"
                    },
                    AllowedCorsOrigins = {
                        "http://localhost:5102",
                        "http://crmnow.tk"
                    },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "graphQL-gateway",
                        "contact",
                        "communication"
                    }
                }
            };
        }
    }
}
