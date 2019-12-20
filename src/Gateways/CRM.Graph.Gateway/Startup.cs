using System;
using System.Threading.Tasks;
using CRM.Graph.Gateway.Options;
using CRM.Metrics;
using CRM.Shared;
using CRM.Shared.CorrelationId;
using CRM.Shared.Interceptors;
using CRM.Shared.Services;
using CRM.Tracing.Jaeger;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using HotChocolate.AspNetCore.Subscriptions;
using HotChocolate.Execution;
using HotChocolate.Stitching;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using OpenTracing.Contrib.Grpc.Interceptors;
using static CRM.Protobuf.Contacts.V1.ContactApi;
using static CRM.Protobuf.Contacts.V1.LeadApi;

namespace CRM.Graph.Gateway
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCorrelationId()
                .AddJaeger()
                .AddAppMetrics()
                .AddGraphQL(Configuration)
                .AddCors(Configuration)
                .AddHealthChecks(Configuration)
                .AddGrpc(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UsePathBase(Configuration["PathBase"])
                .UseCorrelationId()
                .UseAppMetrics()
                .UseCors("CorsPolicy")
                .UseGraphQL("/graphql")
                .UsePlayground(new PlaygroundOptions()
                {
                    QueryPath = "/graphql",
                    Path = "/ui/playground"
                })
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapHealthChecks("/health");

                    if (Environment.IsDevelopment())
                    {
                        endpoints.MapGet(Configuration["PathBase"], context =>
                        {
                            context.Response.Redirect($"{Configuration["PathBase"]}ui/playground");
                            return Task.CompletedTask;
                        });
                    }
                });
        }
    }

    static class CustomExtensionsMethods
    {
        public static IServiceCollection AddGrpc(this IServiceCollection services, IConfiguration configuration)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            services.Scan(scan => scan
                            .FromCallingAssembly()
                            .AddClasses(x => x.AssignableTo(typeof(ServiceBase)))
                            .AsImplementedInterfaces()
                            .WithScopedLifetime());

            services.AddTransient<ClientTracingInterceptor>();
            services.AddTransient<ClientLoggerInterceptor>();
            var serviceOptions = configuration.GetOptions<ServiceOptions>("Services");

            services.AddGrpcClient<ContactApiClient>(o =>
            {
                o.Address = new Uri(serviceOptions.ContactService.Url);
            })
            .AddInterceptor<ClientLoggerInterceptor>()
            .AddInterceptor<ClientTracingInterceptor>();

            services.AddGrpcClient<LeadApiClient>(o =>
            {
                o.Address = new Uri(serviceOptions.ContactService.Url);
            });

            return services;
        }

        public static IServiceCollection AddGraphQL(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceOptions = configuration.GetOptions<ServiceOptions>("Services");

            services.AddHttpClient("contact", (sp, client) =>
            {
                // in order to pass on the token or any other headers to the backend schema use the IHttpContextAccessor
                var context = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
                client.BaseAddress = new Uri($"{serviceOptions.ContactService.Url}/graphql");
            });
            services.AddHttpContextAccessor();
            services.AddSingleton<IQueryResultSerializer, JsonQueryResultSerializer>();

            services
                .AddGraphQLSubscriptions()
                .AddStitchedSchema(builder => builder
                    .AddSchemaFromHttp("contact"));

            return services;
        }

        public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                var cors = configuration.GetValue<string>("Cors:Origins").Split(',');
                options.AddPolicy("CorsPolicy",
                          policy =>
                          {
                              var builder = policy
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials()
                                .WithOrigins(cors)
                                /* https://github.com/aspnet/AspNetCore/issues/4457 */
                                .SetIsOriginAllowed(host => true);
                          });
            });

            return services;
        }

        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceOptions = configuration.GetOptions<ServiceOptions>("Services");

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddUrlGroup(new Uri(System.IO.Path.Combine(serviceOptions.CommunicationService.Url, "health")), name: "communicationapi-check", tags: new string[] { "communicationapi" })
                .AddUrlGroup(new Uri(System.IO.Path.Combine(serviceOptions.ContactService.Url, "health")), name: "contactapi-check", tags: new string[] { "contactapi" })
                .AddUrlGroup(new Uri(System.IO.Path.Combine(serviceOptions.IdentityService.Url, "health")), name: "identityapi-check", tags: new string[] { "idsvrapi" });

            return services;
        }
    }
}
