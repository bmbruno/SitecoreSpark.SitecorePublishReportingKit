using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitecoreSpark.SPRK.Models
{
    public class PublishQueueItem
    {
        public Guid ItemID { get; set; }

        public string ItemName { get; set; }

        public string FullPath { get; set; }

        public string Language { get; set; }

        public string Action { get; set; }

        public string SourceDatabase { get; set; }

        public string TargetDatabase { get; set; }
    }
}