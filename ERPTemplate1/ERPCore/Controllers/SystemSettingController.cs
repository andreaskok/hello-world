using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERPDomain.Models;

namespace ERPCore.Controllers
{
    public class SystemSettingController : Controller
    {
        // GET: SystemSetting
        public ActionResult Index()
        {
            SystemSettingModel model = new SystemSettingModel();
            string sLogDebug = System.Web.Configuration.WebConfigurationManager.AppSettings["LogDebug"];
            string sLogError = System.Web.Configuration.WebConfigurationManager.AppSettings["LogError"];
            if (sLogDebug.Equals("true"))
            {
                model.LogDebug = true;
            }
            else
            {
                model.LogDebug = false;
            }
            if (sLogError.Equals("true"))
            {
                model.LogError = true;
            }
            else
            {
                model.LogError = false;
            }
            model.ReportServerUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["MvcReportViewer.ReportServerUrl"];
            model.Language = System.Web.Configuration.WebConfigurationManager.AppSettings["Language"];
            model.Currency = System.Web.Configuration.WebConfigurationManager.AppSettings["Currency"];
            model.Theme = System.Web.Configuration.WebConfigurationManager.AppSettings["Theme"];
            model.RowPerPage = System.Web.Configuration.WebConfigurationManager.AppSettings["RowPerPage"];
            model.DateFormat = System.Web.Configuration.WebConfigurationManager.AppSettings["DateFormat"];

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(SystemSettingModel model)
        {
            Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            if (model.LogDebug == true)
            {
                config.AppSettings.Settings["LogDebug"].Value = "true";
            }
            else
            {
                config.AppSettings.Settings["LogDebug"].Value = "false";
            }
            if (model.LogError == true)
            {
                config.AppSettings.Settings["LogError"].Value = "true";
            }
            else
            {
                config.AppSettings.Settings["LogError"].Value = "false";
            }
            config.AppSettings.Settings["MvcReportViewer.ReportServerUrl"].Value = model.ReportServerUrl;
            config.AppSettings.Settings["Language"].Value = model.Language;
            config.AppSettings.Settings["Currency"].Value = model.Currency;
            config.AppSettings.Settings["Theme"].Value = model.Theme;
            config.AppSettings.Settings["RowPerPage"].Value = model.RowPerPage;
            config.AppSettings.Settings["DateFormat"].Value = model.DateFormat;

            config.Save();
            return View(model);
        }
    }
}
