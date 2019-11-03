using Sitecore.Data.Items;
using Sitecore.Publishing.Pipelines.Publish;
using SitecoreSpark.SPRK.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace SitecoreSpark.SPRK.Publishing.Pipelines.Publish
{
    public class PublishLoggerStartProcessor : PublishProcessor
    {
        private readonly ISparkLogger _logger;

        public PublishLoggerStartProcessor(ISparkLogger logger)
        {
            _logger = logger;
        }

        public override void Process(PublishContext context)
        {
            if (context == null || context.Aborted)
                return;

            string folderPath = Sitecore.Configuration.Settings.GetSetting("SitecoreSpark.SPRK.LogFolder");
            string filePrefix = Sitecore.Configuration.Settings.GetSetting("SitecoreSpark.SPRK.LogPrefix");
            int maxBufferSize = Sitecore.Configuration.Settings.GetIntSetting("SitecoreSpark.SPRK.MaxLogBufferSize", 1000);
            bool enabled = Sitecore.Configuration.Settings.GetBoolSetting("SitecoreSpark.SPRK.Enabled", false);

            // Assume thet SPRK is enabled if the "SPRKEnabled" key exists in the PublishContext.CustomData dictionary
            if (enabled)
                context.CustomData.Add("SPRKEnabled", enabled);

            // FolderPath should be mapped on the server
            folderPath = Sitecore.IO.FileUtil.MapPath(folderPath);

            try
            {
                if (context.CustomData.ContainsKey("SPRKEnabled"))
                {
                    try
                    {
                        int logKey = _logger.StartLog(logFolderPath: folderPath, logFilePrefix: filePrefix, maxBufferSize: maxBufferSize);
                        context.CustomData.Add("SPRKKey", logKey);
                    }
                    catch (Exception exc)
                    {
                        Sitecore.Diagnostics.Log.Error($"[SPRK] Exception starting log. Exception: {exc.ToString()}", this);
                    }
                }
            }
            catch (Exception exc)
            {
                Sitecore.Diagnostics.Log.Error($"[SPRK] Exception during PublishLoggerStartProcessor.Process() method. Exception: {exc.ToString()}", this);
            }
        }

    }
}