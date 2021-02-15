using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Options;
using RegisterSQLServers.CsvFileImport.Interfaces.Services;
using RegisterSQLServers.CsvFileImport.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RegisterSQLServers.CsvFileImport.Services
{
    public class CsvFileImportService : ICsvFileImportService
    {
        private readonly IOptionsMonitor<CsvFileImportServiceOptions> _options;

        /// <summary>
        /// Gets the CSV input path.
        /// </summary>
        /// <value>
        /// The CSV input path.
        /// </value>
        public string CsvInputPath
        {
            get
            {
                var options = _options.Get(Constants.CsvFileImportServiceOptionsName);
                return options.CsvInputPath;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvFileImportService" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public CsvFileImportService(IOptionsMonitor<CsvFileImportServiceOptions> options)
        {
            _options = options;
        }

        /// <summary>
        /// Reads the CSV file.
        /// </summary>
        public async Task<List<DatabaseInstanceImportRow>> ReadCsvFile()
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    TrimOptions = TrimOptions.Trim,
                    HasHeaderRecord = true
                };

                List<DatabaseInstanceImportRow> rows;

                //read the file
                using (var reader = new StreamReader(CsvInputPath))
                {
                    using (var csv = new CsvReader(reader, config))
                    {
                        rows = csv.GetRecords<DatabaseInstanceImportRow>().OrderBy(x => x.LogicalFolderName)
                            .ThenBy(x => x.EnvironmentTier).ThenBy(x => x.DatabaseName).ToList();
                    }
                }

                return rows;
            }
            catch (Exception ex)
            {
                //TODO Log the exception
                throw ex;
            }

            return new List<DatabaseInstanceImportRow>();
        }
    }

    public class CsvFileImportServiceOptions
    {
        public string CsvInputPath { get; set; }
    }
}