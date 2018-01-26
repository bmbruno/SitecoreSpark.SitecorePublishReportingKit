using SitecoreSpark.SPRK.Interfaces;
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
        private readonly ILogManager _logManager;

        public ReportController(ILogManager logManager)
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

            return View("~/Views/SPRK/Report/Index.cshtml");
        }

        public ActionResult Find()
        {
            return View("~/Views/SPRK/Report/Find.cshtml");
        }

        [HttpPost]
        public ActionResult Find(int id)
        {
            return View("~/Views/SPRK/Report/Find.cshtml");
        }

        public ActionResult ViewLog(string logDate)
        {
            return View("~/Views/SPRK/Report/ViewLog.cshtml");
        }
    }
}