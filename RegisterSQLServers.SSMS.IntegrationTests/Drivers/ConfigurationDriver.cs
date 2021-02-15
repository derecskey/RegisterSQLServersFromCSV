using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace RegisterSQLServers.SSMS.IntegrationTests.Drivers
{
    public class ConfigurationDriver
    {
        private readonly Lazy<IConfiguration> _configurationLazy;

        public ConfigurationDriver()
        {
            _configurationLazy = new Lazy<IConfiguration>(GetConfiguration);
        }

        public IConfiguration Configuration => _configurationLazy.Value;

        public string LocalServerGroupRoot => Configuration["SSMS:LocalServerGroupRoot"];

        private IConfiguration GetConfiguration()
        {
            string directoryName = Path.GetDirectoryName(typeof(ConfigurationDriver).Assembly.Location);

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile(Path.Combine(directoryName, @"appsettings.json"));

            return configurationBuilder.Build();
        }
    }
}
