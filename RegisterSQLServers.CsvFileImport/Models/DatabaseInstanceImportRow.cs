namespace RegisterSQLServers.CsvFileImport.Models
{
    public class DatabaseInstanceImportRow
    {
        public string ServerName { get; set; }
        
        public string InstanceName { get; set; }

        public string DatabaseName { get; set; }

        public string VanityName { get; set; }

        public string EnvironmentTier { get; set; }

        public string LogicalFolderName { get; set; }
    }
}
