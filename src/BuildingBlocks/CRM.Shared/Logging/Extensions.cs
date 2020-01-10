using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Loki;

namespace CRM.Shared.Logging
{
    public static class Extensions
    {
        public static IHostBuilder UseLogging(this IHostBuilder hostBuilder, string applicationName = "")
        {
            hostBuilder.UseSerilog((context, loggerConfiguration) =>
            {
                var appOptions = context.Configuration.GetOptions<AppOptions>("App");
                var loggingOptions = context.Configuration.GetOptions<LoggingOptions>("Logging");

                applicationName = string.IsNullOrWhiteSpace(applicationName) ? appOptions.Name : applicationName;

                loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration, "Logging")
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                    .Enrich.WithProperty("ApplicationName", applicationName);

                if (loggingOptions.ConsoleEnabled)
                {
                    loggerConfiguration.WriteTo
                        .Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3} {Properties:j}] {Message:lj}{NewLine}{Exception}");
                }
                if (loggingOptions.Seq.Enabled)
                {
                    loggerConfiguration.WriteTo.Seq(loggingOptions.Seq.Url, apiKey: loggingOptions.Seq.ApiKey);
                }
                if (loggingOptions.Loki != null && loggingOptions.Loki.Enabled)
                {
                    var credentials = new NoAuthCredentials(loggingOptions.Loki.Url);
                    loggerConfiguration.WriteTo.LokiHttp(credentials, new LokiLogLabelProvider(context.HostingEnvironment.EnvironmentName, applicationName));
                }
            });

            return hostBuilder;
        }
    }
}
