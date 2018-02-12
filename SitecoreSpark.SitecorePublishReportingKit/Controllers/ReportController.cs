using SitecoreSpark.SPRK.Interfaces;
using SitecoreSpark.SPRK.Models;
using System;
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

            // TODO: handle "null" result from GetLogItems
            // TODO: implement model
            ViewBag.LogItems = logItems;

            return View("~/Views/SPRK/Report/Index.cshtml");
        }

        public ActionResult ViewRaw(string log)
        {
            string contents = _logManager.GetLogContentsRaw(log);

            if (String.IsNullOrEmpty(contents))
            {
                TempData["Error"] = $"Log file is not a valid SPRK log file: {log}";
                return Redirect("/sitecore/sprk");
            }

            return Content(contents, "text/plain");
        }
    }
}