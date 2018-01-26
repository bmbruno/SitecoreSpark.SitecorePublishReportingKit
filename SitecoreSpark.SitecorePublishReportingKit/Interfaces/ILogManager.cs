namespace SitecoreSpark.SPRK.Interfaces
{
    public interface ILogManager
    {
        /// <summary>
        /// Should init any internal variables for the implementation and populate them.
        /// </summary>
        /// <param name="logFolderPath">Path to folder where log file will be written.</param>
        /// <param name="logFilePrefix">Log file prefix.</param>
        /// <returns></returns>
        void Initialize(string logFolderPath, string logNamePrefix);

    }
}
