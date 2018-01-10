namespace SitecoreSpark.SPRK.Interfaces
{
    public interface ISparkLogger
    {
        /// <summary>
        /// Should start a new in-memory log, if one isn't already started.
        /// </summary>
        /// <param name="logFolderPath">Path to folder where log file will be written.</param>
        /// <param name="logFilePrefix">Log file prefix.</param>
        /// <returns>Key of the new log entry.</returns>
        int StartLog(string logFolderPath, string logFilePrefix);

        /// <summary>
        /// Should write the provided data to the log buffer for the given log key.
        /// </summary>
        /// <param name="logKey">Key of the current log.</param>
        /// <param name="mode">Publish mode description.</param>
        /// <param name="itemID">ID of the published item.</param>
        /// <param name="result">Publish result decription.</param>
        /// <param name="username">Username of who performed the publish.</param>
        /// <param name="sourceDB">Source content database.</param>
        /// <param name="targetDB">Target content database.</param>
        /// <param name="datetime">Formatted date/time of publish.</param>
        void AddToLog(int logKey, string mode, string itemID, string result, string username, string sourceDB, string targetDB, string datetime);

        /// <summary>
        /// Should remove the current in-memory log buffer for the given log key.
        /// </summary>
        /// <param name="logKey">Key of the log to close</param>
        void RemoveLog(int logKey);

        /// <summary>
        /// Should write in-memory log file to disk for the given log key.
        /// </summary>
        /// <param name="logKey">Key of the log to close</param>
        void CloseLog(int logKey);
    }
}
