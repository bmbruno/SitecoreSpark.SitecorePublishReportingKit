using Sitecore.Publishing.Pipelines.Publish;
using SitecoreSpark.SPRK.Models;
using SitecoreSpark.SPRK.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitecoreSpark.SPRK.Mapping
{
    public static class ViewModelMappingExtensions
    {
        /// <summary>
        /// Populates a LogIndexViewModel by mapping the related objects to it.
        /// </summary>
        /// <param name="vm">View model to be mapped.</param>
        /// <param name="logItems">Array of LogItem objects to be mapped.</param>
        public static void MapToViewModel(this LogIndexViewModel vm, LogItem[] logItems)
        {
            for (var i = 0; i < logItems.Length; i++)
            {
                vm.LogItems.Add(new LogItemViewModel()
                {
                    Date = logItems[i].Date.ToString("yyyy-MM-dd"),
                    FileSizeKB = (logItems[i].FileSizeKB.HasValue) ? logItems[i].FileSizeKB.Value.ToString() : "0",
                    FileName = logItems[i].FileName
                });
            }
        }

        /// <summary>
        /// Populates a LogDetailViewModel by mapping the related objects to it.
        /// </summary>
        /// <param name="vm">View model to be mapped.</param>
        /// <param name="fileContentByLine">Array of file contents (line by line) that will be mapped.</param>
        public static void MapToViewModel(this LogDetailViewModel vm, string[] fileContentByLine)
        {
            foreach (string line in fileContentByLine)
            {
                string localLine = line;

                // Handle escaped pipe characters
                if (localLine.IndexOf(@"\|") > 0)
                    localLine = localLine.Replace(@"\|", "##PIPE##");
                   
                string[] data = localLine.Split('|');

                vm.Rows.Add(new LogRowViewModel()
                {
                    ItemID = SanitizeForModel(data[0]),
                    Mode = SanitizeForModel(data[1]),
                    Result = SanitizeForModel(data[2]),
                    UserName = SanitizeForModel(data[3]),
                    SourceDB = SanitizeForModel(data[4]),
                    TargetDB = SanitizeForModel(data[5]),
                    DateTime = SanitizeForModel(data[6])
                });
            }
        }

        /// <summary>
        /// Populates a PublishQueueViewModel by mapping the related objects to it.
        /// </summary>
        /// <param name="vm">View model to be mapped.</param>
        /// <param name="candidateItems">Enumerable list of PublishingCandidate items from the PublishQueue class.</param>
        public static void MapToViewModel(this PublishQueueViewModel vm, IEnumerable<PublishQueueItem> candidateItems)
        {
            if (candidateItems == null)
                return;

            foreach (PublishQueueItem candidate in candidateItems)
            {
                vm.CandidateItems.Add(new PublishQueueItemViewModel()
                {
                    ItemID = candidate.ItemID.ToString(),
                    Language = candidate.Language,
                    ItemName = candidate.ItemName,
                    FullPath = candidate.FullPath,
                    Action = candidate.Action,
                    SourceDatabase = candidate.SourceDatabase,
                    Targetdatabase = candidate.TargetDatabase
                });
            }
        }

        /// <summary>
        /// Cleans up text from file and ensures it is a valid string for a view model. Includes logic to convert ##PIPE## tokens back to pipe character.
        /// </summary>
        /// <param name="input">Input text to sanitize.</param>
        /// <returns>Sanitized string.</returns>
        private static string SanitizeForModel(string input)
        {
            // Handle empty/null string
            if (String.IsNullOrEmpty(input))
                return string.Empty;

            // Handle escaped characters
            if (input.IndexOf("##PIPE##") > 0)
                return input.Replace("##PIPE##", "|");

            return input;
        }

    }
}