using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RegisterSQLServers.SSMS.IntegrationTests.Drivers;
using RegisterSQLServers.SSMS.Interfaces.Services;
using RegisterSQLServers.SSMS.Services;
using TechTalk.SpecFlow;

namespace RegisterSQLServers.SSMS.IntegrationTests.Hooks
{
    [Binding]
    public class SSMSHooks
    {
        private static IHost _host;

        private ScenarioContext _scenarioContext;

        private FeatureContext _featureContext;

        public SSMSHooks(ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
        }

        [BeforeTestRun]
        public static void SetupHost(ConfigurationDriver configurationDriver)
        {
            IHostBuilder hostBuilder = CreateHostBuilder(configurationDriver);

            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ISSMSService, SSMSService>();
            });

            _host = hostBuilder.Build();
            _host.StartAsync();
        }

        [AfterTestRun]
        public static void TearDownHost()
        {
            _host.Dispose();
        }

        /// <summary>
        /// Creates the host builder.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(ConfigurationDriver configurationDriver) =>
            Host.CreateDefaultBuilder();
    }
}