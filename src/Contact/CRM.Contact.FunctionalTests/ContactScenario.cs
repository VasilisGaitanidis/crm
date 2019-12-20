using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.IO;
using Microsoft.Extensions.Configuration;
using CRM.Contact.Api;

namespace CRM.Contact.FunctionalTests
{
    public class ContactScenario : IClassFixture<WebApplicationFactory<Startup>>
    {
        protected readonly WebApplicationFactory<Startup> _factory;

        public ContactScenario(WebApplicationFactory<Startup> factory)
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.json");

            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((SourceContext, config) =>
                {
                    config.AddJsonFile(configPath);
                    config.AddEnvironmentVariables();
                });
            });
        }

        // [Fact]
        // public async Task Call_ping_and_return_pong()
        // {
        //     GrpcChannelOptions options = new GrpcChannelOptions()
        //     {
        //         HttpClient = _factory.CreateClient()
        //     };
        //     var channel = GrpcChannel.ForAddress("http://localhost", options);
        //     var client = new LeadApi.LeadApiClient(channel);

        //     var result = await client.PingAsync(new Empty());

        //     Assert.NotNull(result);
        //     Assert.Equal("Pong", result.Message);
        // }
    }
}

