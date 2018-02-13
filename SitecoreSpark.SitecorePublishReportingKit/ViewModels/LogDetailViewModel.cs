using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitecoreSpark.SPRK.ViewModels
{
    public class LogDetailViewModel
    {
        public string Title { get; set; }

        public string FileName { get; set; }

        public bool ModifiedOnly { get; set; }

        public List<LogRowViewModel> Rows { get; set; }

        public LogDetailViewModel()
        {
            Rows = new List<LogRowViewModel>();
        }
    }
}