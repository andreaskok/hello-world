using ERPDomain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace PluginAR.Controllers
{
    public class BaseController : Controller
    {
        public new RedirectToRouteResult RedirectToAction(string action, string controller)
        {
            return base.RedirectToAction(action, controller);
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (!filterContext.Controller.ToString().Contains("Home") && !filterContext.Controller.ToString().Contains("ParentMenu") && !filterContext.ActionDescriptor.ActionName.Contains("Login"))
            {
                if (Session["UserEmail"] == null || Session["UserEmail"].ToString() == "")
                {
                    //filterContext.HttpContext.Response.Redirect("~/Views/Home/Index");
                    filterContext.Result = RedirectToAction("UnauthorizedAccess", "home");
                }

                if (Session["AdminFlag"] == null && filterContext.Controller.ToString().Contains("SH_"))
                {
                    filterContext.Result = RedirectToAction("UnauthorizedAccess", "home");
                }
                else
                {
                    if (Session["AdminFlag"].ToString() != "True" && filterContext.Controller.ToString().Contains("SH_") && filterContext.ActionDescriptor.ActionName.Contains("Index"))
                    {
                        filterContext.Result = RedirectToAction("UnauthorizedAccess", "home");
                    }
                }
            }
        }
        // GET: Base
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
                        Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
                        null;
            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }

    }
}