namespace SitecoreSpark.SPRK.Interfaces
{
    public interface ISparkLogger
    {
        /// <summary>
        /// Should start a new in-memory log, if one isn't already started.
        /// </summary>
        /// <param name="logFolderPath">Path to folder where log file will be written.</param>
        /// <param name="logFilePrefix">Log file prefix.</param>
        /// <param name="logBufferSize">Max number of lines allowed in the log buffer.</param>
        /// <returns>Key of the new log entry.</returns>
        int StartLog(string logFolderPath, string logFilePrefix, int maxBufferSize);

        /// <summary>
        /// Should write the provided data to the log buffer for the given log key.
        /// </summary>
        /// <param name="logKey">Key of the current log.</param>
        /// <param name="itemID">ID of the published item.</param>
        /// <param name="mode">Publish mode description.</param>
        /// <param name="result">Publish result decription.</param>
        /// <param name="username">Username of who performed the publish.</param>
        /// <param name="sourceDB">Source content database.</param>
        /// <param name="targetDB">Target content database.</param>
        /// <param name="datetime">Formatted date/time of publish.</param>
        void AddToLog(int logKey, string itemID, string mode, string result, string username, string sourceDB, string targetDB, string datetime);

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

        /// <summary>
        /// Determines if the given log needs to flush based on maximum buffer size.
        /// </summary>
        /// <returns>True: log was flushed; false: log was not flushed.</returns>
        bool HandleLogFlush(int logKey);
    }
}
