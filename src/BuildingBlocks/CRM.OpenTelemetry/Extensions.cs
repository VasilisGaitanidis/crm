using CRM.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace;
using OpenTelemetry.Trace.Samplers;
using OpenTelemetry.Trace.Configuration;
using CRM.OpenTelemetry.Collector;

namespace CRM.OpenTelemetry
{
    public static class Extensions
    {
        private static bool _initialized;

        public static IServiceCollection AddCustomOpenTelemetry(this IServiceCollection services)
        {
            if (_initialized)
            {
                return services;
            }

            _initialized = true;

            var options = GetOpenTelemetryOptions(services);

            services.AddOpenTelemetry((resolver, builder) =>
            {
                builder.SetSampler(GetSampler(options));

                builder.UseZipkin(o =>
                {
                    o.ServiceName = options.ServiceName;
                    o.Endpoint = new System.Uri(options.ZipkinEndpoint);
                })
                .AddRequestCollector()
                .AddDependencyCollector()
                .AddCollector(t => new HotChocolateCollector(t));
            });

            return services;
        }

        private static Sampler GetSampler(OpenTelemetryOptions options)
        {
            return options.Sampler switch
            {
                "const" => new AlwaysSampleSampler(),
                _ => new AlwaysSampleSampler(),
            };
        }

        private static OpenTelemetryOptions GetOpenTelemetryOptions(IServiceCollection services)
        {
            using (var seriveProvider = services.BuildServiceProvider())
            {
                var configuration = seriveProvider.GetService<IConfiguration>();
                return configuration.GetOptions<OpenTelemetryOptions>("OpenTelemetry");
            }
        }
    }
}