namespace SitecoreSpark.SPRK.Interfaces
{
    public interface ILogManager<T>
    {
        /// <summary>
        /// Should init any internal variables for the implementation and populate them.
        /// </summary>
        /// <param name="logFolderPath">Path to folder where log file will be written.</param>
        /// <param name="logFilePrefix">Log file prefix.</param>
        void Initialize(string logFolderPath, string logNamePrefix);

        /// <summary>
        /// Should get a list of log files names.
        /// </summary>
        /// <returns>Array of file names.</returns>
        string[] GetLogFilenames();

        /// <summary>
        /// Should get a list of log item objects.
        /// </summary>
        /// <returns>Array of log item objects.</returns>
        T[] GetLogItems();

        /// <summary>
        /// Should return an array of the contents of a log file.
        /// </summary>
        /// <returns>Array of file contents by line.</returns>
        string[] GetLogContents(string logFileName);

        /// <summary>
        /// Should return a string of the contents of a log file.
        /// </summary>
        /// <param name="logFileName">Filename of the log.</param>
        /// <returns>String of file contents.</returns>
        string GetLogContentsRaw(string logFileName);

        /// <summary>
        /// Should return a log file formatted for CSV output.
        /// </summary>
        /// <param name="logFileName">Filename of the log.</param>
        /// <returns>CSV-formatted string of log contents.</returns>
        string GetFileForCSV(string logFileName);
    }
}
