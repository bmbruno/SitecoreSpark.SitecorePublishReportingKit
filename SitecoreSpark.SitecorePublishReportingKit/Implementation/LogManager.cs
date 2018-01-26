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
        private string[] _logFileNames;

        /// <summary>
        /// Initalizes local variables and runs other setup tasks.
        /// </summary>
        /// <param name="logFolderPath">Path to log folder. Must be a mapped path (absolute).</param>
        /// <param name="logNamePrefix">Prefix of log file names.</param>
        public void Initialize (string logFolderPath, string logNamePrefix)
        {
            // TODO: Load all files matching pattern

            // TODO: Build list of full file paths
            
        }
    }
}