using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using SitecoreSpark.SPRK.Interfaces;
using SitecoreSpark.SPRK.Models;
using System.Text;

namespace SitecoreSpark.SPRK.Implementation
{
    public class LogManager : ILogManager<LogItem>
    {
        private List<LogItem> _logList;
        private string _logDateFormat = "yyyyMMdd";
        private string _logPrefix;
        private string _csvHeader = "ItemID,Mode,Result,User,SourceDB,TargetDB,DateTime";
        
        /// <summary>
        /// Initalizes local variables and runs other setup tasks.
        /// </summary>
        /// <param name="logFolderPath">Path to log folder. Must be a mapped path (absolute).</param>
        /// <param name="logNamePrefix">Prefix of log file names.</param>
        public void Initialize (string logFolderPath, string logNamePrefix)
        {
            // Variable init
            _logList = new List<LogItem>();
            _logPrefix = logNamePrefix;

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
        /// Returns the contents of a log file by line.
        /// </summary>
        /// <param name="logFileName">Filename of the log.</param>
        /// <returns>File contents in an array of strings.</returns>
        public string[] GetLogContents(string logFileName)
        {
            // Validate
            if (!this.IsValidLogFile(logFileName))
                return null;
            
            // Process
            LogItem currentLogItem = this._logList.FirstOrDefault(u => u.FileName == logFileName);

            if (currentLogItem == null)
                throw new Exception($"No log item found for filename: {logFileName}");

            return File.ReadAllLines(currentLogItem.FilePath);
            
        }

        /// <summary>
        /// Returns the contents of a log file in whole.
        /// </summary>
        /// <param name="logFileName">Filename of the log.</param>
        /// <returns>File contents of a log in a string.</returns>
        public string GetLogContentsRaw(string logFileName)
        {
            // Validate
            if (!this.IsValidLogFile(logFileName))
                return null;

            // Process
            LogItem currentLogItem = this._logList.FirstOrDefault(u => u.FileName == logFileName);

            if (currentLogItem == null)
                throw new Exception($"No log item found for filename: {logFileName}");

            return File.ReadAllText(currentLogItem.FilePath);
        }

        /// <summary>
        /// Gets a log file for the given filename and formats it as a CSV string.
        /// </summary>
        /// <param name="logFileName">Filename of the log.</param>
        /// <returns>CSV-formatted string.</returns>
        public string GetFileForCSV(string logFileName)
        {
            string[] contents = this.GetLogContents(logFileName);
            StringBuilder sb = new StringBuilder();

            // File header
            sb.Append($"{this._csvHeader}\n");

            // TODO: check if pipe replacement is necessary on each line before calling replace()
            foreach (string row in contents)
            {
                string line = row;
                line = line.Replace(@"\|", "##PIPE##");
                line = line.Replace("\"", string.Empty);
                line = line.Replace('|', ',');
                line = line.Replace("##PIPE##", "|");

                sb.Append($"{line}\n");
            }

            return sb.ToString();
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

        /// <summary>
        /// Verifies that the provided log filename is a valid SPRK log file name, based on the log prefix value assigned during class instantiation.
        /// </summary>
        /// <param name="logFileName">Filename of the log.</param>
        /// <returns>True/false based on filename validity.</returns>
        private bool IsValidLogFile(string logFileName)
        {
            if (String.IsNullOrEmpty(logFileName))
                return false;

            return (logFileName.StartsWith(this._logPrefix));
        }
    }
}