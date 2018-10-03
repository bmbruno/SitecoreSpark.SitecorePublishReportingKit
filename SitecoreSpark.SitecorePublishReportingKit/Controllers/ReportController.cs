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
    public class ReportController : BaseController
    {
        private readonly ILogManager<LogItem> _logManager;

        public ReportController(ILogManager<LogItem> logManager)
        {
            _logManager = logManager;

            string folderPath = Sitecore.Configuration.Settings.GetSetting("SitecoreSpark.SPRK.LogFolder");
            string filePrefix = Sitecore.Configuration.Settings.GetSetting("SitecoreSpark.SPRK.LogPrefix");
            _logManager.Initialize(folderPath, filePrefix);
        }

        public ActionResult Index()
        {
            return View("~/Views/SPRK/Report/Index.cshtml");
        }

        public ActionResult PublishLogs()
        {
            LogIndexViewModel viewModel = new LogIndexViewModel();
            LogItem[] logItems = _logManager.GetLogItems();

            if (logItems != null)
            {
                viewModel.MapToViewModel(logItems);
            }

            return View("~/Views/SPRK/Report/PublishLogs.cshtml", viewModel);
        }

        public ActionResult ViewLog(string log, bool modified = false)
        {
            LogDetailViewModel viewModel = new LogDetailViewModel();
            string[] contents = _logManager.GetLogContents(log, modified);

            viewModel.Title = log;
            viewModel.FileName = log;
            viewModel.ModifiedOnly = modified;
            viewModel.MapToViewModel(contents);

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

        public FileResult Download(string log, bool modified = false)
        {
            string csv = _logManager.GetFileForCSV(log, modified);
            return File(fileContents: new UTF8Encoding().GetBytes(csv), contentType: "text/csv", fileDownloadName: $"{log}.csv");
        }

        public ActionResult IncrementalPublishQueue()
        {
            // Get report data
            ReportService reportService = new ReportService();
            IEnumerable<PublishQueueItem> reportData = reportService.IncrementalPublishQueue_GetData();

            // Build viewmodel
            PublishQueueViewModel viewModel = new PublishQueueViewModel();

            if (reportData != null)
                viewModel.MapToViewModel(reportData);

            return View("~/Views/SPRK/Report/IncrementalPublishQueue.cshtml", viewModel);
        }
    }
}