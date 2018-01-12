using Sitecore.Data.Items;
using Sitecore.Publishing.Pipelines.PublishItem;
using SitecoreSpark.SPRK.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace SitecoreSpark.SPRK.Publishing.Pipelines.PublishItem
{
    public class PublishLoggerProcessor : PublishItemProcessor
    {
        private readonly ISparkLogger _logger;

        public PublishLoggerProcessor(ISparkLogger logger)
        {
            _logger = logger;
        }

        public override void Process(PublishItemContext context)
        {
            if (context == null || context.Aborted)
                return;

            try
            {
                if (context.PublishContext.CustomData.ContainsKey("SPRKEnabled"))
                {

                    int? logKey = (int?)context.PublishContext.CustomData["SPRKKey"];
                    if (!logKey.HasValue)
                    {
                        Sitecore.Diagnostics.Log.Error($"[SPRK] 'SPRKKey' key not found in PublishContext.CustomData. Cannot log publishing for item ID {context.ItemId.ToString()}.", this);
                        return;
                    }

                    try
                    {
                        _logger.AddToLog(
                            logKey.Value,
                            context.ItemId.ToString(),
                            context.PublishOptions.Mode.ToString(),
                            context.Result.Operation.ToString(),
                            context.User.Name,
                            context.PublishOptions.SourceDatabase.Name,
                            context.PublishOptions.TargetDatabase.Name,
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        );
                    }
                    catch (Exception exc)
                    {
                        Sitecore.Diagnostics.Log.Error($"[SPRK] Exception logging publish for item ID {context.ItemId.ToString()}. Exception: {exc.ToString()}", this);
                    }
                }
            }
            catch (Exception exc)
            {
                Sitecore.Diagnostics.Log.Error($"[SPRK] Exception during PublishLoggerProcessor.Process() method. Exception: {exc.ToString()}", this);
            }
        }
    }
}