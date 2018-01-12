using Sitecore.Diagnostics;
using Sitecore.IO;
using SitecoreSpark.SPRK.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace SitecoreSpark.SPRK
{
    public class SparkLogger : ISparkLogger
    {
        /// <summary>
        /// Master list of all in-memory logs. Each log is idenified by the Dictionary's key.
        /// </summary>
        private Dictionary<int, List<string>> _logListBuffer;

        /// <summary>
        /// Log output folder on disk. Should (ideally) be read from configuration.
        /// </summary>
        private string _logFolderPath;

        /// <summary>
        /// Filename prefix to use for logs. Date format 'yyyyMMdd' will be appended to this. Should (ideally) be read from configuration.
        /// </summary>
        private string _logFilePrefix;

        /// <summary>
        /// Class constructor. Should be called during Sitecore / resolved via DI.
        /// </summary>
        public SparkLogger()
        {
            this.InitializeLogBuffer();
        }

        /// <summary>
        /// Initalizes a new publishing log. Assigns a new LogID and returns it.
        /// </summary>
        /// <param name="logFolderPath">Path to folder where log file will be written.</param>
        /// <param name="logFilePrefix">Log file prefix.</param>
        /// <returns>New LogID.</returns>
        public int StartLog(string logFolderPath, string logFilePrefix)
        {
            Assert.IsNotNullOrEmpty(logFolderPath, "'logFolderPath' cannot be null or empty.");
            Assert.IsNotNullOrEmpty(logFilePrefix, "'logFilePrefix' cannot be null or empty.");

            // Ensure log buffer is ready
            this.InitializeLogBuffer();
            
            // Assign path settings to private variables
            _logFolderPath = logFolderPath;
            _logFilePrefix = logFilePrefix;

            // The key for this new log is based on current buffer size; just a simple increment
            int newLogKey = 0;
            newLogKey = _logListBuffer.Count + 1;
            _logListBuffer.Add(newLogKey, new List<string>());

            return newLogKey;
        }

        /// <summary>
        /// Creates a log entry for the given item and related information and stores it in the log buffer for the given logKey.
        /// </summary>
        /// <param name="logKey">Key of the current log.</param>
        /// <param name="itemID">ID of the published item.</param>
        /// <param name="mode">Publish mode description.</param>
        /// <param name="result">Publish result decription.</param>
        /// <param name="username">Username of who performed the publish.</param>
        /// <param name="sourceDB">Source content database.</param>
        /// <param name="targetDB">Target content database.</param>
        /// <param name="datetime">Formatted date/time of publish.</param>
        public void AddToLog(int logKey, string itemID, string mode, string result, string username, string sourceDB, string targetDB, string datetime)
        {
            Assert.ArgumentCondition((logKey > 0), "logKey", "LogKey must be a positive integer; if it is 0, there may be a problem with the log or the publishing context.");

            if (!_logListBuffer.ContainsKey(logKey))
            {
                throw new KeyNotFoundException("'{logKey}' key not found in SparkLogger.AddToLog method.");
            }

            username = SanitizeForPipe(username);

            var log = _logListBuffer[logKey];
            string formattedLine = $"{itemID}|{mode}|{result}|{username}|{sourceDB}|{targetDB}|{datetime}";
            log.Add(formattedLine);
            _logListBuffer[logKey] = log;
        }

        /// <summary>
        /// Removed a log from the in-memory list.
        /// </summary>
        /// <param name="logKey">Key of the current log.</param>
        public void RemoveLog(int logKey)
        {
            Assert.ArgumentCondition((logKey > 0), "logKey", "LogKey must be a positive integer; if it is 0, there may be a problem with the log or the publishing context.");

            if (!_logListBuffer.ContainsKey(logKey))
            {
                throw new KeyNotFoundException("'{logKey}' key not found in SparkLogger.ClearLog method.");
            }

            if (_logListBuffer.ContainsKey(logKey))
                _logListBuffer.Remove(logKey);
        }

        /// <summary>
        /// Appends a log to disk (the curren day's file) and removes it from the in-memory buffer.
        /// </summary>
        /// <param name="logKey">Key of the current log.</param>
        public void CloseLog(int logKey)
        {
            Assert.ArgumentCondition((logKey > 0), "logKey", "LogKey must be a positive integer; if it is 0, there may be a problem with the log or the publishing context.");

            if (!_logListBuffer.ContainsKey(logKey))
            {
                throw new KeyNotFoundException("'{logKey}' key not found in SparkLogger.CloseLog method.");
            }

            var log = _logListBuffer[logKey];

            if (log != null && log.Count > 0)
            {
                string filePath = Path.Combine(_logFolderPath, String.Format("{0}{1}.txt", _logFilePrefix, DateTime.Today.ToString("yyyyMMdd")));
                filePath = FileUtil.MapPath(filePath);

                if (!Directory.Exists(_logFolderPath))
                    Directory.CreateDirectory(_logFolderPath);

                File.AppendAllLines(filePath, log);

                this.RemoveLog(logKey);
            }
        }
        
        /// <summary>
        /// Ensures that the local log buffer is initialized.
        /// </summary>
        private void InitializeLogBuffer()
        {
            if (_logListBuffer == null)
                this._logListBuffer = new Dictionary<int, List<string>>();
        }

        private string SanitizeForPipe(string input)
        {
            if (input.IndexOf("|") > 0)
                return input.Replace("|", "\\|");

            return input;
        }
    }
}