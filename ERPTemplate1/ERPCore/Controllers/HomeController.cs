using ERPDomain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERPDomain.Abstract;

namespace ERPCore.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IErrorLogRepository errorLogRepo) : base(errorLogRepo)
        {

        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UnauthorizedAccess()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Index_en()
        {
            SetCulture("en");
            return RedirectToAction("Index");
        }

        public ActionResult Index_id()
        {
            SetCulture("id");
            return RedirectToAction("Index");
        }

        public ActionResult Index_zh()
        {
            SetCulture("zh-CN");
            return RedirectToAction("Index");
        }

        public void SetCulture(string culture)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            //return RedirectToAction("Index");
        }

        public ActionResult TestReport2()
        {
            return View();
        }

        public ActionResult TestLineGraph()
        {
            return View();
        }

        public ActionResult TestJournalReport2()
        {
            return View();
        }

    }
}