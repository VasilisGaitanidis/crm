using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MassTransit.AspNetCoreIntegration;
using MassTransit;
using CRM.Communication.IntegrationHandlers;
using System;
using CRM.Shared.Types;
using CRM.Shared;
using CRM.MassTransit.Tracing;
using CRM.Metrics;
using Microsoft.AspNetCore.Http;
using MassTransit.Context;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using CRM.OpenTelemetry;

namespace CRM.Communication.Api
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
                .AddCustomOpenTelemetry()
                .AddAppMetrics()
                .AddHealthChecks(_configuration)
                .AddMassTransit(_configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAppMetrics();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
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
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            var rabbitMqUrl = configuration.GetOptions<RabbitMqOptions>("rabbitMQ").Url;
            hcBuilder
                .AddRabbitMQ(rabbitMqUrl, name: "comnunication-rabbitmqbus-check", tags: new string[] { "rabbitmqbus" });

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
                    cfg.UseSerilog();
                    var host = cfg.Host(new Uri(rabbitMqOption.Url), "/", hc =>
                    {
                        hc.Username(rabbitMqOption.UserName);
                        hc.Password(rabbitMqOption.Password);
                    });

                    cfg.ReceiveEndpoint("communication", x =>
                    {
                        x.Consumer<ContactCreatedConsumer>(provider);
                        x.Consumer<ContactUpdatedConsumer>(provider);
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
    }
}
