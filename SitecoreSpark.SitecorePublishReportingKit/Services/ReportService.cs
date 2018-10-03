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
        /// Loads data for the Incremental Publish Queue report (includes all item languages). Utilizes Sitecore.Publishing.Pipelines.Publish.GetPublishQueue(PublishOptions) method as a source of data, then augments it based on item workflow state: only returns items in a final workflow state.
        /// </summary>
        /// <param name="languageCode">Language code to load. Default is "en". Must otherwise use fully-qualified language code (example: "ja-JP").</param>
        /// <returns>Enumerable of publish candidate items.</returns>
        public IEnumerable<PublishQueueItem> IncrementalPublishQueue_GetData()
        {
            Database masterDB = Database.GetDatabase(Sitecore.Configuration.Settings.GetSetting("SitecoreSpark.SPRK.SourceDatabase"));
            Database webDB = Database.GetDatabase(Sitecore.Configuration.Settings.GetSetting("SitecoreSpark.SPRK.TargetDatabase"));
            List<PublishQueueItem> model = new List<PublishQueueItem>();

            // Note on language: the actual publish pipeline uses the language parameter; it doesn't matter what we pass in for this usage
            PublishOptions options = new PublishOptions(masterDB, webDB, PublishMode.Incremental, Language.Parse("en"), DateTime.Now.AddDays(1));
            IEnumerable<PublishingCandidate> candidateList = PublishQueue.GetPublishQueue(options);

            if (candidateList != null && candidateList.Count() > 0)
            {
                foreach (PublishingCandidate candidate in candidateList)
                {
                    // Get detailed item information (including workflow info for inclusion/exclusion on report)
                    Item scItem = masterDB.GetItem(itemId: candidate.ItemId);

                    // If scItem is null, it likely means an item was deleted from 'master' before a publish (and still exists in the PublishQueue table); safe to ignore
                    if (scItem == null)
                    {
                        Sitecore.Diagnostics.Log.Warn($"[SPRK] PublishQueue item not found in '{masterDB.Name}' database: {candidate.ItemId}.", this);
                        continue;
                    }

                    IWorkflow itemWorkflow = masterDB.WorkflowProvider.GetWorkflow(scItem);

                    if (itemWorkflow == null)
                        continue;

                    // Check for all language versions (Workflow is shared, but Workflow State may be unique across languages)
                    foreach (Language language in scItem.Languages)
                    {
                        Item scLanguageItem = masterDB.GetItem(itemId: scItem.ID, language: language);

                        if (scLanguageItem.Versions.Count > 0)
                        {
                            WorkflowState state = itemWorkflow.GetState(scLanguageItem);

                            if (state != null && state.FinalState)
                            {
                                // Map to domain model
                                model.Add(new PublishQueueItem()
                                {
                                    ItemID = candidate.ItemId.Guid,
                                    ItemName = scItem.Name,
                                    Language = language.Name,
                                    Action = candidate.PublishAction.ToString(),
                                    SourceDatabase = candidate.PublishOptions.SourceDatabase.Name,
                                    TargetDatabase = candidate.PublishOptions.TargetDatabase.Name
                                });
                            }
                        }
                    }
                }
            }
            
            return model;
        }
    }
}