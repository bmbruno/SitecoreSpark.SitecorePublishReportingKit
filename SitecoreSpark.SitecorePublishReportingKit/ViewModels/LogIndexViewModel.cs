using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitecoreSpark.SPRK.ViewModels
{
    public class LogIndexViewModel
    {
        List<LogItemViewModel> LogItems { get; set; }

        public LogIndexViewModel()
        {
            LogItems = new List<LogItemViewModel>();
        }
    }
}