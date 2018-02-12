using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitecoreSpark.SPRK.ViewModels
{
    public class LogRowViewModel
    {
        public string ItemID { get; set; }

        public string Mode { get; set; }

        public string Result { get; set; }

        public string Username { get; set; }

        public string SourceDB { get; set; }

        public string TargetDB { get; set; }

        public string DateTime { get; set; }

    }
}