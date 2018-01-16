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
        public ActionResult Index()
        {
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