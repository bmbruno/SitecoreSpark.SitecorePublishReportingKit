using SitecoreSpark.SPRK.Interfaces;
using SitecoreSpark.SPRK.Models;
using SitecoreSpark.SPRK.ViewModels;
using System;
using System.Collections.Generic;
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
            LogIndexViewModel viewModel = new LogIndexViewModel();
            LogItem[] logItems = _logManager.GetLogItems();

            // TODO: handle "null" result from GetLogItems

            // TODO: move mapping code to service/support/mapper class
            for (var i = 0; i < logItems.Length; i++)
            {
                viewModel.LogItems.Add(new LogItemViewModel()
                {
                    Date = logItems[i].Date.ToString("yyyy-MM-dd"),
                    FileSizeKB = (logItems[i].FileSizeKB.HasValue) ? logItems[i].FileSizeKB.Value.ToString() : "0",
                    FileName = logItems[i].FileName
                });
            }
            
            return View("~/Views/SPRK/Report/Index.cshtml", viewModel);
        }

        public ActionResult ViewLog(string log)
        {
            LogDetailViewModel viewModel = new LogDetailViewModel();
            string[] contents = _logManager.GetLogContents(log);

            // TODO: move mapping code to service/support/mapper class
            foreach (string line in contents)
            {
                // TODO: handle escaped pipe characters
                string[] data = line.Split('|');

                viewModel.Rows.Add(new LogRowViewModel()
                {
                    // TODO: handle null cases when reading from array
                    ItemID = data[0],
                    Mode = data[1],
                    Result = data[2],
                    Username = data[3],
                    SourceDB = data[4],
                    TargetDB = data[5],
                    DateTime = data[6]
                });
            }

            return View("~/Views/SPRK/Report/ViewLog.cshtml", viewModel);
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