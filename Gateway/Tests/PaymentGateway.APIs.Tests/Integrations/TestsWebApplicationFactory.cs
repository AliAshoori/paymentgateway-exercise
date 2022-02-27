using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Infrastructure.Context;

namespace PaymentGateway.APIs.Tests.Integrations
{
    [ExcludeFromCodeCoverage]
    public class TestsWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        internal readonly AppDbContext InMemoryAppDbContext;

        public TestsWebApplicationFactory()
        {
            InMemoryAppDbContext =
                new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "payment_api").Options);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.json");

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile(configPath);
                context.HostingEnvironment.EnvironmentName = "testing";
            });

            builder.ConfigureServices(services =>
            {
                var appDbContextServiceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(AppDbContext));
                services.Remove(appDbContextServiceDescriptor);
                services.AddSingleton(x => InMemoryAppDbContext);
            });
        }
    }
}