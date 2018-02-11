using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using SitecoreSpark.SPRK.Interfaces;
using SitecoreSpark.SPRK.Models;

namespace SitecoreSpark.SPRK.Implementation
{
    public class LogManager : ILogManager<LogItem>
    {
        private List<LogItem> _logList;
        private string _logDateFormat = "yyyyMMdd";
        
        /// <summary>
        /// Initalizes local variables and runs other setup tasks.
        /// </summary>
        /// <param name="logFolderPath">Path to log folder. Must be a mapped path (absolute).</param>
        /// <param name="logNamePrefix">Prefix of log file names.</param>
        public void Initialize (string logFolderPath, string logNamePrefix)
        {
            // Variable init
            _logList = new List<LogItem>();

            // Load all files matching filename prefix pattern
            var allFiles = Directory.GetFiles(logFolderPath, $"{logNamePrefix}*");

            for (int i = 0; i < allFiles.Length; i++)
            {
                FileInfo info = new FileInfo(allFiles[i]);

                _logList.Add(new LogItem()
                {
                    Date = GetDateFromFileName(Path.GetFileNameWithoutExtension(allFiles[i])),
                    FileName = Path.GetFileNameWithoutExtension(allFiles[i]),
                    FilePath = allFiles[i],
                    FileSizeKB = Convert.ToInt32(info.Length / 1000)
                });
            }

            _logList = _logList.OrderByDescending(u => u.Date).ToList();
        }

        /// <summary>
        /// Returns the list of log file names.
        /// </summary>
        /// <returns>Array of log file names.</returns>
        public string[] GetLogFilenames()
        {
            return _logList.Select(u => u.FileName).ToArray();
        }

        /// <summary>
        /// Returns the list of strongly-typed LogItem objects.
        /// </summary>
        /// <returns>Array of LogItem objects.</returns>
        public LogItem[] GetLogItems()
        {
            return this._logList.ToArray();
        }

        /// <summary>
        /// Gets the date value from the log filename.
        /// </summary>
        /// <param name="fileName">Filename of the log file containing a date to parse.</param>
        /// <returns>DateTime object of the parsed date.</returns>
        private DateTime GetDateFromFileName(string fileName)
        {
            int startIndex = fileName.LastIndexOf('.') + 1;

            DateTime date = DateTime.ParseExact(fileName.Substring(startIndex, 8), this._logDateFormat, System.Globalization.CultureInfo.InvariantCulture);

            return date;
        }
    }
}