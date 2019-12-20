using CRM.Shared.Interceptors;
using CRM.Shared.Repository;
using CRM.Tracing.Jaeger;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using OpenTracing.Contrib.Grpc.Interceptors;
using MassTransit;
using System;
using MassTransit.AspNetCoreIntegration;
using CRM.Shared.Types;
using CRM.Shared;
using CRM.Contact.IntegrationHandlers;
using CRM.Shared.CorrelationId;
using CRM.MassTransit.Tracing;
using MassTransit.Context;
using CRM.Metrics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using CRM.Contact.Services;
using CRM.Contact.Validators;
using HotChocolate;
using HotChocolate.Execution.Configuration;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using CRM.Contact.GraphType;

namespace CRM.Contact.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddGraphQL()
                .AddGrpc()
                .AddCorrelationId()
                .AddJaeger()
                .AddMediatR(typeof(ContactService))
                .AddAppMetrics()
                .AddHealthChecks(_configuration)
                .AddMassTransit(_configuration)
                .AddCustomDbContext(_configuration);

            services.Scan(scan => scan
               .FromAssemblyOf<CreateContactRequestValidator>()
               .AddClasses(c => c.AssignableTo(typeof(IValidator<>)))
               .AsImplementedInterfaces()
               .WithTransientLifetime());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCorrelationId()
                .UseAppMetrics()
                .UseGraphQL("/graphql")
                .UsePlayground(new PlaygroundOptions()
                {
                    QueryPath = "/graphql",
                    Path = "/ui/playground",
                })
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    // Communication with gRPC endpoints must be made through a gRPC client.
                    // To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909
                    endpoints.MapGrpcService<LeadService>();
                    endpoints.MapGrpcService<ContactService>();

                    endpoints.MapHealthChecks("/health");
                    endpoints.MapGet("/", async context =>
                    {
                        await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client");
                    });
                });
        }
    }

    static class CustomExtensionsMethods
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services)
        {
            services.AddAuthorization();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer((options) =>
                {
                    options.Authority = "http://localhost:8080/auth/realms/master";
                    options.RequireHttpsMetadata = false;
                    options.Audience = "account";
                });
            return services;
        }

        public static IServiceCollection AddGrpc(this IServiceCollection services)
        {
            services.AddGrpc(options =>
            {
                options.Interceptors.Add<ExceptionInterceptor>();
                options.Interceptors.Add<ServerTracingInterceptor>();
                options.EnableDetailedErrors = true;
            });

            return services;
        }

        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            var connString = configuration.GetConnectionString("contact");
            var rabbitMqUrl = configuration.GetOptions<RabbitMqOptions>("rabbitMQ").Url;
            hcBuilder
                .AddNpgSql(connString, name: "contactdb-check", tags: new string[] { "contactdb" })
                .AddRabbitMQ(rabbitMqUrl, name: "contact-rabbitmqbus-check", tags: new string[] { "rabbitmqbus" });

            return services;
        }

        public static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit((provider) =>
            {
                MessageCorrelation.UseCorrelationId<IMessage>(x => x.CorrelationId);
                var rabbitMqOption = configuration.GetOptions<RabbitMqOptions>("rabbitMQ");

                return Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri(rabbitMqOption.Url), "/", hc =>
                    {
                        hc.Username(rabbitMqOption.UserName);
                        hc.Password(rabbitMqOption.Password);
                    });

                    cfg.ReceiveEndpoint("contact", x =>
                    {
                        x.ConfigureConsumer<ContactCreatedConsumer>(provider);
                    });

                    cfg.PropagateOpenTracingContext();
                    cfg.PropagateCorrelationIdContext();
                });
            }, (cfg) =>
            {
                cfg.AddConsumersFromNamespaceContaining<ConsumerAnchor>();
            });

            return services;
        }

        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("contact");
            services.AddScoped<IUnitOfWork>(sp =>
            {
                return new UnitOfWork(() => new NpgsqlConnection(connString));
            });

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<ContactContext>(options =>
                {
                    options.UseNpgsql(connString, npgsqlOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 3);
                    });
                });

            return services;
        }

        public static IServiceCollection AddGraphQL(this IServiceCollection services)
        {
            services.AddGraphQL(sp => Schema.Create(c =>
            {
                c.RegisterServiceProvider(sp);
                c.RegisterQueryType<QueryType>();
                // c.RegisterMutationType<MutationType>();
            }), new QueryExecutionOptions
            {
                IncludeExceptionDetails = true,
                TracingPreference = TracingPreference.Never
            });
            return services;
        }
    }
}
