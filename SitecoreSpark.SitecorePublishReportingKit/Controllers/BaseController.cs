using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SitecoreSpark.SPRK.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Determines if the current user is an admin. Checks against current Sitecore context.
        /// </summary>
        /// <returns>True or false.</returns>
        protected bool UserIsAdmin()
        {
            return Sitecore.Context.User.IsAdministrator;
        }
    }
}