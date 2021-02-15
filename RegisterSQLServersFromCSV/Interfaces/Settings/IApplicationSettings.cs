using RegisterSQLServers.CsvFileImport.Settings;
using RegisterSQLServers.SSMS.Settings;

namespace RegisterSQLServers.Interfaces.Settings
{
    public interface IApplicationSettings
    {
        public SSMSSettings SSMS { get; set; }

        public CsvFileImportSettings CsvFileImport { get; set; }
    }
}
