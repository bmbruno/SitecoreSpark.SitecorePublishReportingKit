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
                // TODO: handle escaped pipe characters
                string[] data = line.Split('|');

                vm.Rows.Add(new LogRowViewModel()
                {
                    // TODO: handle null cases when reading from array
                    ItemID = data[0],
                    Mode = data[1],
                    Result = data[2],
                    UserName = data[3],
                    SourceDB = data[4],
                    TargetDB = data[5],
                    DateTime = data[6]
                });
            }
        }
    }
}