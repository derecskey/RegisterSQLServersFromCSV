using System.Collections.Generic;
using System.Threading.Tasks;
using RegisterSQLServers.CsvFileImport.Models;

namespace RegisterSQLServers.CsvFileImport.Interfaces.Services
{
    public interface ICsvFileImportService
    {
        Task<List<DatabaseInstanceImportRow>> ReadCsvFile();
    }
}
