using ERPDomain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using ERPDomain.Logs;
using System.Collections.Specialized;
using System.Reflection;
using ERPDomain.Abstract;
using ERPDomain.Entities;
using System.Runtime.CompilerServices;

namespace ERPCore.Controllers
{
    public class BaseController : Controller
    {
        public IErrorLogRepository errorLogRepository;

        protected BaseController(IErrorLogRepository errorLogRepo)
        {
            this.errorLogRepository = errorLogRepo;
        }
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

        #region "Handle Exception"
        private bool IsAjax(ExceptionContext filterContext)
        {
            return filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            
            if (System.Web.Configuration.WebConfigurationManager.AppSettings["LogError"].Equals("true"))
            {
                String sExMsg = "Exception:" + filterContext.Exception.Message + System.Environment.NewLine;
                sExMsg += "Controller:" + (string)filterContext.RouteData.Values["Controller"] + System.Environment.NewLine;
                sExMsg += "Action:" + (string)filterContext.RouteData.Values["Action"] + System.Environment.NewLine;
                sExMsg += "InnerException:" + CommonUtility.Null2Empty(filterContext.Exception.InnerException) + System.Environment.NewLine;
                sExMsg += "StackTrace:" + filterContext.Exception.StackTrace + System.Environment.NewLine;
                ERPLog.WriteError(sExMsg);

                try
                {
                    ErrorLog errorLog = new ErrorLog();
                    errorLog.ControllerName = (string)filterContext.RouteData.Values["Controller"];
                    errorLog.ActionName = (string)filterContext.RouteData.Values["Action"];
                    errorLog.MessageLog = filterContext.Exception.Message;
                    errorLog.InnerExceptionLog = CommonUtility.Null2Empty(filterContext.Exception.InnerException);
                    errorLog.StackTraceLog = filterContext.Exception.StackTrace;
                    errorLog.UserID = CommonUtility.Null2Empty(Session["UserID"]);
                    errorLog.PluginName = "ERPCore";
                    errorLog.CreateDate = DateTime.Now;
                    errorLogRepository.SaveErrorLog(errorLog);
                }
                catch(Exception ex)
                {
                    System.Console.WriteLine("Ex={0}", ex);
                }
                              
                
            }
            
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            // if the request is AJAX return JSON else view.
            if (IsAjax(filterContext))
            {
                //Because its a exception raised after ajax invocation
                //Lets return Json
                filterContext.Result = new JsonResult()
                {
                    Data = filterContext.Exception.Message,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                
            }
            else
            {
                //Normal Exception
                //So let it handle by its default ways.
                base.OnException(filterContext);
                //DisplayOtherException(filterContext.Exception.Message);

            }

            // Write error logging code here if you wish.

            //if want to get different of the request
            //var currentController = (string)filterContext.RouteData.Values["controller"];
            //var currentActionName = (string)filterContext.RouteData.Values["action"];
        }

        private ActionResult HandleException(string sMessage)
        {
            return RedirectToAction("HttpGeneralError", "Error", new { exMessage = sMessage });
        } 

        private ViewResult DisplayOtherException(string sMessage)
        {
            return View("~/Views/Error/WebConfigError.cshtml");
        }
        #endregion

    }
}