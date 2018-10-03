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

        public IEnumerable<PublishingCandidateItem> IncrementalPublishQueue_GetData(string languageCode)
        {
            Database masterDB = Database.GetDatabase("master");
            Database webDB = Database.GetDatabase(Sitecore.Configuration.Settings.GetSetting("SitecoreSpark.SPRK.TargetDatabase"));
            List<PublishingCandidateItem> model = new List<PublishingCandidateItem>();

            PublishOptions options = new PublishOptions(masterDB, webDB, PublishMode.Incremental, Language.Parse(languageCode), DateTime.Now.AddDays(1));

            IEnumerable<PublishingCandidate> candidateList = PublishQueue.GetPublishQueue(options);

            if (candidateList.Count() > 0)
            {
                // Map to domain model
                foreach (PublishingCandidate candidate in candidateList)
                {
                    Item scItem = masterDB.GetItem(itemId: candidate.ItemId);
                    IWorkflow itemWorkflow = masterDB.WorkflowProvider.GetWorkflow(scItem);
                    WorkflowState state = itemWorkflow.GetState(scItem);

                    if (state.FinalState)
                    {
                        model.Add(new PublishingCandidateItem()
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