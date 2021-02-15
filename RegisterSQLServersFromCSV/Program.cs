using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RegisterSQLServers.CsvFileImport.Interfaces.Services;
using RegisterSQLServers.CsvFileImport.Models;
using RegisterSQLServers.CsvFileImport.Services;
using RegisterSQLServers.Interfaces.Settings;
using RegisterSQLServers.Settings;
using RegisterSQLServers.SSMS.Interfaces.Services;
using RegisterSQLServers.SSMS.Services;

namespace RegisterSQLServers
{
    public class Program
    {
        private static IApplicationSettings _applicationSettings;

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static async Task Main(string[] args)
        {
            IHostBuilder hostBuilder = CreateHostBuilder(args);

            var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
            environmentName = string.IsNullOrEmpty(environmentName) ? "Development" : environmentName;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables();

            _applicationSettings = builder.Build().Get<ApplicationSettings>();

            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton(instance => _applicationSettings);
                services.AddSingleton<ISSMSService, SSMSService>();
                services.AddSingleton<ICsvFileImportService, CsvFileImportService>();
                services.AddHostedService<Worker>(); //akin to .UseStartup<Startup>();

                services.Configure<CsvFileImportServiceOptions>(Constants.CsvFileImportServiceOptionsName, options =>
                {
                    options.CsvInputPath = _applicationSettings.CsvFileImport.CsvInputPath;
                });
            });
            
            using IHost host = hostBuilder.Build();
            await host.RunAsync();
        }

        /// <summary>
        /// Creates the host builder.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args);
    }
}
