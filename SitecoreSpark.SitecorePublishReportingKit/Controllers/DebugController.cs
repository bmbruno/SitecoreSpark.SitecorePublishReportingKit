using System;
using System.Web.Mvc;
using System.Text;
using System.Collections.Generic;
using Sitecore.Data;

using SitecoreSpark.SPRK.Interfaces;
using SitecoreSpark.SPRK.Models;
using SitecoreSpark.SPRK.ViewModels;
using SitecoreSpark.SPRK.Mapping;
using SitecoreSpark.SPRK.Services;

namespace SitecoreSpark.SPRK.Controllers
{
    [Authorize]
    public class DebugController : BaseController
    {
        public DebugController() { }

        public ActionResult Index()
        {
            DebugViewModel viewModel = GetDebugViewModel();            
            return View("~/Views/SPRK/Debug/Index.cshtml", viewModel);
        }

        private DebugViewModel GetDebugViewModel()
        {
            DebugViewModel viewModel = new DebugViewModel();

            viewModel.Enabled = Convert.ToBoolean(Sitecore.Configuration.Settings.GetSetting("SitecoreSpark.SPRK.Enabled"));
            viewModel.DebugMode = Convert.ToBoolean(Sitecore.Configuration.Settings.GetSetting("SitecoreSpark.SPRK.DebugMode"));
            viewModel.LogFolder = Sitecore.Configuration.Settings.GetSetting("SitecoreSpark.SPRK.LogFolder");
            viewModel.LogPrefix = Sitecore.Configuration.Settings.GetSetting("SitecoreSpark.SPRK.LogPrefix");
            viewModel.MaxLogBufferSize = Convert.ToInt32(Sitecore.Configuration.Settings.GetSetting("SitecoreSpark.SPRK.MaxLogBufferSize"));
            viewModel.SourceDatabase = Sitecore.Configuration.Settings.GetSetting("SitecoreSpark.SPRK.SourceDatabase");
            viewModel.TargetDatabase = Sitecore.Configuration.Settings.GetSetting("SitecoreSpark.SPRK.TargetDatabase");

            return viewModel;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}