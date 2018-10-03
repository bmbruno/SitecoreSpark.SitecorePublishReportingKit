using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SitecoreSpark.SPRK.Models;
using Sitecore.Publishing;
using Sitecore.Publishing.Pipelines.Publish;
using Sitecore.Globalization;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Workflows;

namespace SitecoreSpark.SPRK.Services
{
    public class ReportService
    {
        public ReportService() { }

        /// <summary>
        /// Loads data for the Incremental Publish Queue report. Utilizes Sitecore.Publishing.Pipelines.Publish.GetPublishQueue(PublishOptions) method as a source of data, then augments it based on item workflow state: only returns items in a final workflow state.
        /// </summary>
        /// <param name="languageCode">Language code to load. Default is "en". Must otherwise use fully-qualified language code (example: "ja-JP").</param>
        /// <returns>Enumerable of publish candidate items.</returns>
        public IEnumerable<PublishQueueItem> IncrementalPublishQueue_GetData(string languageCode)
        {
            Database masterDB = Database.GetDatabase(Sitecore.Configuration.Settings.GetSetting("SitecoreSpark.SPRK.SourceDatabase"));
            Database webDB = Database.GetDatabase(Sitecore.Configuration.Settings.GetSetting("SitecoreSpark.SPRK.TargetDatabase"));
            List<PublishQueueItem> model = new List<PublishQueueItem>();

            PublishOptions options = new PublishOptions(masterDB, webDB, PublishMode.Incremental, Language.Parse(languageCode), DateTime.Now.AddDays(1));
            IEnumerable<PublishingCandidate> candidateList = PublishQueue.GetPublishQueue(options);

            if (candidateList != null && candidateList.Count() > 0)
            {
                foreach (PublishingCandidate candidate in candidateList)
                {
                    // Get detailed item information (including workflow info for inclusion/exclusion on report)
                    Item scItem = masterDB.GetItem(itemId: candidate.ItemId);
                    IWorkflow itemWorkflow = masterDB.WorkflowProvider.GetWorkflow(scItem);

                    if (itemWorkflow == null)
                        continue;

                    WorkflowState state = itemWorkflow.GetState(scItem);

                    if (state.FinalState)
                    {
                        // Map to domain model
                        model.Add(new PublishQueueItem()
                        {
                            ItemID = candidate.ItemId.Guid,
                            ItemName = scItem.Name,
                            FullPath = scItem.Paths.FullPath,
                            Action = candidate.PublishAction.ToString(),
                            SourceDatabase = candidate.PublishOptions.SourceDatabase.Name,
                            TargetDatabase = candidate.PublishOptions.TargetDatabase.Name
                        });
                    }
                }
            }
            
            return model;
        }
    }
}