using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERPDomain.Abstract;
using ERPDomain.Entities;
using ERPDomain.Helpers;
using ERPDomain.Logs;

namespace ERPCore.Controllers
{
    public class ErrorController : Controller
    {

        private IDebugLogRepository debugLogRepository;
        private IErrorLogRepository errorLogRepository;
        public ErrorController(IDebugLogRepository debugLogRepo, IErrorLogRepository errorLogRepo)
        {
            this.debugLogRepository = debugLogRepo;
            this.errorLogRepository = errorLogRepo;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HttpError404(string exMessage)
        {
            TempData["exMessage"] = exMessage;
            if (System.Web.Configuration.WebConfigurationManager.AppSettings["LogError"].Equals("true"))
            {
                String sExMsg = "Exception:" + exMessage + System.Environment.NewLine;
                ERPLog.WriteError(sExMsg);

                try
                {
                    ErrorLog errorLog = new ErrorLog();
                    errorLog.ControllerName = "";
                    errorLog.ActionName = "HttpError404";
                    errorLog.MessageLog = exMessage;
                    errorLog.InnerExceptionLog = "";
                    errorLog.StackTraceLog = "";
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
            return View();
        }

        public ActionResult HttpError500(string exMessage)
        {
            TempData["exMessage"] = exMessage;
            if (System.Web.Configuration.WebConfigurationManager.AppSettings["LogError"].Equals("true"))
            {
                String sExMsg = "Exception:" + exMessage + System.Environment.NewLine;
                ERPLog.WriteError(sExMsg);

                try
                {
                    ErrorLog errorLog = new ErrorLog();
                    errorLog.ControllerName = "";
                    errorLog.ActionName = "HttpError500";
                    errorLog.MessageLog = exMessage;
                    errorLog.InnerExceptionLog = "";
                    errorLog.StackTraceLog = "";
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
            return View();
        }

        public ActionResult HttpGeneralError(string exMessage)
        {
            TempData["exMessage"] = exMessage;
            if (System.Web.Configuration.WebConfigurationManager.AppSettings["LogError"].Equals("true"))
            {
                String sExMsg = "Exception:" + exMessage + System.Environment.NewLine;
                ERPLog.WriteError(sExMsg);

                try
                {
                    ErrorLog errorLog = new ErrorLog();
                    errorLog.ControllerName = "";
                    errorLog.ActionName = "HttpGeneralError";
                    errorLog.MessageLog = exMessage;
                    errorLog.InnerExceptionLog = "";
                    errorLog.StackTraceLog = "";
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
            return View();
        }

        public ActionResult HttpError()
        {
            return View();
        }

    }
}