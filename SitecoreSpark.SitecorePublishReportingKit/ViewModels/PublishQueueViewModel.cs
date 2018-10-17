using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitecoreSpark.SPRK.ViewModels
{
    public class PublishQueueViewModel
    {
        public List<PublishQueueItemViewModel> CandidateItems { get; set; }

        public PublishQueueViewModel()
        {
            CandidateItems = new List<PublishQueueItemViewModel>();
        }
    }
}