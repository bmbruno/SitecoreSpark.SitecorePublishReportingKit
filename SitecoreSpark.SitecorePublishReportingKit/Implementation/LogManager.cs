using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using SitecoreSpark.SPRK.Interfaces;

namespace SitecoreSpark.SPRK.Implementation
{
    public class LogManager : ILogManager
    {
        private List<string> _logFileNames;
        private string logDateFormat = "yyyyMMdd";

        /// <summary>
        /// Initalizes local variables and runs other setup tasks.
        /// </summary>
        /// <param name="logFolderPath">Path to log folder. Must be a mapped path (absolute).</param>
        /// <param name="logNamePrefix">Prefix of log file names.</param>
        public void Initialize (string logFolderPath, string logNamePrefix)
        {
            // Variable init
            _logFileNames = new List<string>();

            // Load all files matching filename prefix pattern
            var allFiles = Directory.GetFiles(logFolderPath, $"{logNamePrefix}*");

            for (int i = 0; i < allFiles.Length; i++)
            {
                _logFileNames.Add(Path.GetFileName(allFiles[i]));
            }
        }

        /// <summary>
        /// Returns the list of log file names.
        /// </summary>
        /// <returns>Array of log file names.</returns>
        public string[] GetLogFilenames()
        {
            return _logFileNames.ToArray<string>();
        }
    }
}