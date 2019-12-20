using System.Net;
using CRM.Configuration.Vault;
using CRM.Shared.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;

namespace CRM.Contact.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddEnvironmentVariables();
                    config.AddCommandLine(args);
                })
                .UseLogging()
                .UseVault()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel((ctx, options) =>
                    {
                        if (ctx.HostingEnvironment.IsDevelopment())
                        {
                            IdentityModelEventSource.ShowPII = true;
                        }
                        options.Limits.MinRequestBodyDataRate = null;
                        options.Listen(IPAddress.Any, 5001, ListenOptions => {
                            ListenOptions.Protocols = HttpProtocols.Http1;
                        });
                        options.Listen(IPAddress.Any, 15001, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http2;
                        });
                    });
                    webBuilder.UseStartup<Startup>()
                        .UseKestrel(o =>
                        {
                            o.AllowSynchronousIO = true;
                        });
                });
    }
}
