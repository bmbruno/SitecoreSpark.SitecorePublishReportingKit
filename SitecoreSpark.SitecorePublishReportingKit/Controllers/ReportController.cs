using SitecoreSpark.SPRK.Interfaces;
using SitecoreSpark.SPRK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SitecoreSpark.SPRK.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly ILogManager<LogItem> _logManager;

        public ReportController(ILogManager<LogItem> logManager)
        {
            _logManager = logManager;

            string folderPath = Sitecore.Configuration.Settings.GetSetting("SitecoreSpark.SPRK.LogFolder");
            string filePrefix = Sitecore.Configuration.Settings.GetSetting("SitecoreSpark.SPRK.LogPrefix");
            _logManager.Initialize(folderPath, filePrefix);
        }

        /// <summary>
        /// Determines if the current user is an admin. Checks against current Sitecore context.
        /// </summary>
        /// <returns>True or false.</returns>
        private bool UserIsAdmin()
        {
            return Sitecore.Context.User.IsAdministrator;
        }

        public ActionResult Index()
        {
            // Load list of log files
            LogItem[] logItems = _logManager.GetLogItems();

            ViewBag.LogItems = logItems;

            return View("~/Views/SPRK/Report/Index.cshtml");
        }

        public ActionResult ViewLog(string logDate)
        {
            return View("~/Views/SPRK/Report/ViewLog.cshtml");
        }
    }
}