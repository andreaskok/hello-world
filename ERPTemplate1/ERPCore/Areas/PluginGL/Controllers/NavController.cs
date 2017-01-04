using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PluginGL.Controllers
{
    public class NavController : Controller
    {
        // GET: Nav
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult LeftMenuSummary()
        {

            return PartialView();
        }
    }
}