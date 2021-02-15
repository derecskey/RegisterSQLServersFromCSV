using RegisterSQLServers.CsvFileImport.Settings;
using RegisterSQLServers.Interfaces.Settings;
using RegisterSQLServers.SSMS.Settings;

namespace RegisterSQLServers.Settings
{
    public class ApplicationSettings : IApplicationSettings
    {
        public SSMSSettings SSMS { get; set; }

        public CsvFileImportSettings CsvFileImport { get; set; }
    }
}
