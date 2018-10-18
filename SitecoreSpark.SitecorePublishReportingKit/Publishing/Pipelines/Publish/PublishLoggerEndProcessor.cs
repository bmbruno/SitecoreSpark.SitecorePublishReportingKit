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
    public class PublishLoggerEndProcessor : PublishProcessor
    {
        private readonly ISparkLogger _logger;

        public PublishLoggerEndProcessor(ISparkLogger logger)
        {
            _logger = logger;
        }

        public override void Process(PublishContext context)
        {
            if (context == null || context.Aborted)
                return;

            try
            {
                if (context.CustomData.ContainsKey("SPRKEnabled"))
                {

                    int? logKey = (int?)context.CustomData["SPRKKey"];
                    if (!logKey.HasValue)
                    {
                        Sitecore.Diagnostics.Log.Error($"[SPRK] 'SPRKKey' key not found in PublishContext.CustomData. Cannot close publishing log.", this);
                        return;
                    }

                    try
                    {
                        _logger.CloseLog(logKey.Value);
                    }
                    catch (Exception exc)
                    {
                        Sitecore.Diagnostics.Log.Error($"[SPRK] Exception closing log. LogKey: {logKey}. Exception: {exc.ToString()}", this);
                    }
                }
            }
            catch (Exception exc)
            {
                Sitecore.Diagnostics.Log.Error($"[SPRK] Exception during PublishLoggerEndProcessor.Process() method. Exception: {exc.ToString()}", this);
            }
        }
    }
}