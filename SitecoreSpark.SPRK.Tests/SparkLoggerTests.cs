using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SitecoreSpark.SPRK.Interfaces;
using System.IO;
using System.Collections.Generic;

namespace SitecoreSpark.SPRK.Tests
{
    [TestClass]
    public class SparkLoggerTests
    {
        internal string _logFolderPath = "./TESTLOGS";
        internal string _logFilePrefix = "SPRK.TESTLOG";
        internal int _logBufferSize = 1000;

        private SparkLogger SetupLogger()
        {
            SparkLogger logger = new SparkLogger();
            return logger;
        }

        private string GetTestFilePath()
        {
            return Path.Combine(_logFolderPath, ($"{this._logFilePrefix}.{DateTime.Now.ToString("yyyyMMdd")}.txt"));
        }

        private void CleanUpTestLogFiles()
        {
            if (!Directory.Exists(this._logFolderPath))
                return;

            foreach (var filePath in Directory.GetFiles(this._logFolderPath))
            {
                File.Delete(filePath);
            }

            Directory.Delete(this._logFolderPath);
        }

        [TestMethod]
        public void Logger_Instantiation_IsNotNull()
        {
            var logger = this.SetupLogger();

            Assert.IsNotNull(logger);
        }

        [TestMethod]
        public void Logger_StartLog_IsPositiveInteger()
        {
            var logger = this.SetupLogger();

            int? testKey = logger.StartLog(this._logFolderPath, this._logFilePrefix, this._logBufferSize);

            Assert.IsNotNull(testKey.Value);
            Assert.IsTrue(testKey > 0);
        }

        [TestMethod]
        public void Logger_OutputsFile()
        {
            var logger = this.SetupLogger();

            int testKey = logger.StartLog(this._logFolderPath, this._logFilePrefix, this._logBufferSize);
            logger.AddToLog(testKey, "mode", "itemID", "result", "username", "sourceDB", "targetDB", "2018-01-01 12:00:00");
            logger.CloseLog(testKey);

            Assert.IsTrue(File.Exists(GetTestFilePath()));

            this.CleanUpTestLogFiles();
        }

        [TestMethod]
        public void Logger_DoesNotOutputFile()
        {
            var logger = this.SetupLogger();

            int testKey = logger.StartLog(this._logFolderPath, this._logFilePrefix, this._logBufferSize);
            logger.CloseLog(testKey);

            Assert.IsFalse(Directory.Exists(this._logFolderPath));

            this.CleanUpTestLogFiles();
        }

        [TestMethod]
        public void Logger_LogsExpectedValue()
        {
            var logger = this.SetupLogger();

            int testKey = logger.StartLog(this._logFolderPath, this._logFilePrefix, this._logBufferSize);
            logger.AddToLog(testKey, "itemID", "mode", "result", "username", "sourceDB", "targetDB", "2018-01-01 12:00:00");
            logger.CloseLog(testKey);

            string[] fileContents = File.ReadAllLines(this.GetTestFilePath());

            Assert.IsTrue(fileContents.Length > 0, "Test log file is empty.");
            Assert.IsTrue(fileContents[0].Equals(@"itemID|mode|result|username|sourceDB|targetDB|2018-01-01 12:00:00"));

            this.CleanUpTestLogFiles();
        }

        [TestMethod]
        public void Logger_EscapesPipeCharacterCorrectly()
        {
            var logger = this.SetupLogger();

            int testKey = logger.StartLog(this._logFolderPath, this._logFilePrefix, this._logBufferSize);
            logger.AddToLog(testKey, "itemID", "mode", "result", "user|name", "sourceDB", "targetDB", "2018-01-01 12:00:00");
            logger.CloseLog(testKey);

            string[] fileContents = File.ReadAllLines(this.GetTestFilePath());

            Assert.IsTrue(fileContents.Length > 0, "Test log file is empty.");
            Assert.IsTrue(fileContents[0].Equals(@"itemID|mode|result|user\|name|sourceDB|targetDB|2018-01-01 12:00:00"));

            this.CleanUpTestLogFiles();
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException), "AddToLog method was incorrectly given a valid key. This test should always throw an exception.")]
        public void Logger_KeyNotFoundException_AddToLog()
        {
            var logger = this.SetupLogger();

            int testKey = logger.StartLog(this._logFolderPath, this._logFilePrefix, this._logBufferSize);

            logger.AddToLog((testKey + 1), "itemID", "mode", "result", "username", "sourceDB", "targetDB", "2018-01-01 12:00:00");
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException), "Close method was incorrectly given a valid key. This test should always throw an exception.")]
        public void Logger_KeyNotFoundException_CloseLog()
        {
            var logger = this.SetupLogger();

            int testKey = logger.StartLog(this._logFolderPath, this._logFilePrefix, this._logBufferSize);

            logger.CloseLog((testKey + 1));
        }
    }
}
