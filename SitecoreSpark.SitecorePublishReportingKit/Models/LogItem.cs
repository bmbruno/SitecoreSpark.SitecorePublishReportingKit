using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitecoreSpark.SPRK.Models
{
    public class LogItem
    {
        public DateTime Date { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public int? FileSizeKB { get; set; }
    }
}