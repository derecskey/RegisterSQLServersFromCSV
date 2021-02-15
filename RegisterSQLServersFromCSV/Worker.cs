using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RegisterSQLServers.CsvFileImport.Interfaces.Services;
using RegisterSQLServers.Interfaces.Settings;
using RegisterSQLServers.SSMS.Interfaces.Services;
using RegisterSQLServers.SSMS.Model;

namespace RegisterSQLServers
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IApplicationSettings _applicationSettings;
        private readonly ISSMSService _ssms;
        private readonly ICsvFileImportService _iCsvFileImportService;

        /// <summary>
        /// Initializes a new instance of the <see cref="Worker"/> class.
        /// </summary>
        /// <param name="appLifeTime">The application life time.</param>
        /// <param name="applicationSettings">The application settings.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="ssms">The SSMS.</param>
        public Worker(IHostApplicationLifetime appLifeTime, IApplicationSettings applicationSettings, ILogger<Worker> logger, ISSMSService ssms, ICsvFileImportService iCsvFileImportService)
        {
            _appLifetime = appLifeTime;
            _applicationSettings = applicationSettings;
            _logger = logger;
            _ssms = ssms;
            _iCsvFileImportService = iCsvFileImportService;
        }

        /// <summary>
        /// This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. The implementation should return a task that represents
        /// the lifetime of the long running operation(s) being performed.
        /// </summary>
        /// <param name="stoppingToken">Triggered when <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is called.</param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            //create or get the top level root server group
            var localServerGroupRoot = await _ssms.CreateOrGetRootServerGroup(_applicationSettings.SSMS.LocalServerGroupRoot);
            
            //ingest the import file
            var databaseInstanceRows = await _iCsvFileImportService.ReadCsvFile();

            //get the distinct list of logical folder names
            var logicalFolderNames = databaseInstanceRows.Select(x => x.LogicalFolderName).Distinct();
            foreach (var logicalFolderName in logicalFolderNames)
            {
                //create the logical server group
                var logicalServerGroup = await _ssms.CreateOrGetServerGroup(localServerGroupRoot, logicalFolderName);
                
                //get the list of this logical group's environment tiers
                var scopedEnvironmentTiers = databaseInstanceRows.Where(y => y.LogicalFolderName == logicalFolderName)
                    .Select(x => x.EnvironmentTier).Distinct();
                foreach (var scopedEnvironmentTier in scopedEnvironmentTiers)
                {
                    //create the environment tier server group for this logical group of instances
                    var environmentTierServerGroup = await _ssms.CreateOrGetServerGroup(logicalServerGroup, scopedEnvironmentTier);

                    //get the list of distinct instances within this logical and environment tier
                    var databaseInstancesForThisLogicalAndEnvironmentTier = databaseInstanceRows.Where(x =>
                            x.LogicalFolderName == logicalFolderName && x.EnvironmentTier == scopedEnvironmentTier)
                        .ToList();
                    foreach (var database in databaseInstancesForThisLogicalAndEnvironmentTier)
                    {
                        var colorMapping = _applicationSettings.SSMS.CustomColorMappings.FirstOrDefault(x => x.EnvironmentTier == database.EnvironmentTier);
                        var colorInArgb = Color.White.ToArgb();
                        
                        if (colorMapping != null)
                            colorInArgb = colorMapping.ColorInArgb;

                        var databaseInstance = new DatabaseInstance()
                        {
                            ServerName = database.ServerName,
                            CustomConnectionColorARGB = colorInArgb,
                            Name = database.DatabaseName,
                            InstanceName = database.InstanceName,
                            Description = string.Empty,
                            UseCustomConnectionColor = true
                        };

                        var databaseServerRegistration = await _ssms.RegisterDatabaseInstance(environmentTierServerGroup, databaseInstance);
                    }
                }
            }

            _appLifetime.StopApplication();
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns></returns>
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnApplicationStarted);
            _appLifetime.ApplicationStopping.Register(OnApplicationStopping);
            _appLifetime.ApplicationStopped.Register(OnApplicationStopped);

            return base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        /// <returns></returns>
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        /// <summary>
        /// Delegate that is called when [application stopped] event fires.
        /// </summary>
        private void OnApplicationStopped()
        {
            _logger.LogInformation("OnApplicationStopped has been called.");

            // Perform post-stopped activities here
        }

        /// <summary>
        /// Delegate that is called when [application stopping] event fires.
        /// </summary>
        private void OnApplicationStopping()
        {
            _logger.LogInformation("OnApplicationStopping has been called.");

            // Perform on-stopping activities here
        }

        /// <summary>
        /// Delegate that is called when [application started] event fires.
        /// </summary>
        private void OnApplicationStarted()
        {
            _logger.LogInformation("OnApplicationStarted has been called.");

            // Perform post-startup activities here
        }
    }
}
