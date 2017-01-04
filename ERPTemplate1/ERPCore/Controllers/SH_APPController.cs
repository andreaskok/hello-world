using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ERPDomain.Entities;
using ERPDomain.Abstract;
using System.Collections.Generic;
using ERPDomain.Models;
using Newtonsoft.Json;
using System;
using System.Configuration;
using ERPDomain.Logs;
using ERPDomain.Helpers;

namespace ERPCore.Controllers
{
    public class SH_APPController : BaseController
    {
        //private EFDbContext db = new EFDbContext();
        private ISH_APPRepository appRepository;
        private IDebugLogRepository debugLogRepository;
        public SH_APPController(ISH_APPRepository appRepo, IDebugLogRepository debugLogRepo, IErrorLogRepository errorLogRepo) : base(errorLogRepo)
        {
            this.appRepository = appRepo;
            this.debugLogRepository = debugLogRepo;
        }

        // GET: SH_APP
        public ActionResult Index()
        {
            //int x = 0;
            //x /= x; //Test error handling
            //return View(db.SH_APP.ToList());
            var model = from m in appRepository.SH_APP
                        select m;
            return View(model);
        }

        // GET: SH_APP/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //SH_APP sH_APP = db.SH_APP.Find(id);
            //if (sH_APP == null)
            //{
            //    return HttpNotFound();
            //}
            var model = (from p in appRepository.SH_APP
                         where p.ID.Equals(id)
                         orderby p.FunctionName
                         select p).FirstOrDefault();
            return View(model);
        }

        // GET: SH_APP/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SH_APP/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ModuleID,FunctionName")] SH_APP sh_app)
        {
            if (ModelState.IsValid)
            {
                //db.SH_APP.Add(sH_APP);
                //db.SaveChanges();
                appRepository.SaveSH_APP(sh_app);
                TempData["Message"] = string.Format("{0} was added", sh_app.FunctionName);
                return RedirectToAction("Index");
            }

            return View(sh_app);
        }

        // GET: SH_APP/Edit/5
        public ActionResult Edit(int? id)
        {
            //int x = 0;
            //x /= x;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //SH_APP sH_APP = db.SH_APP.Find(id);
            //if (sH_APP == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(sH_APP);
            var model = (from p in appRepository.SH_APP
                         where p.ID.Equals(id)
                         orderby p.FunctionName
                         select p).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: SH_APP/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ModuleID,FunctionName")] SH_APP sh_app)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(sH_APP).State = EntityState.Modified;
                //db.SaveChanges();
                try
                {
                    appRepository.SaveSH_APP(sh_app);
                }
                catch (Exception ex)
                {
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["LogError"].Equals("true"))
                    {
                        LogError(sh_app, "PostEdit", ex);
                    }
                }
                
                ViewBag.Message = string.Format("{0} was updated", sh_app.FunctionName);
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["LogDebug"].Equals("true"))
                {
                    LogDebug(sh_app, "PostEdit");
                }
                return RedirectToAction("Index");
            }
            return View(sh_app);
        }

        private void LogError(SH_APP sh_app, string actionName, Exception ex)
        {
            List<DebugLogModel> logModel = new List<DebugLogModel>();
            logModel.Add(new DebugLogModel
            {
                Name = "ID",
                Value = sh_app.ID.ToString()
            });
            logModel.Add(new DebugLogModel
            {
                Name = "ModuleID",
                Value = sh_app.ModuleID.ToString()
            });
            logModel.Add(new DebugLogModel
            {
                Name = "FunctionName",
                Value = sh_app.FunctionName
            });
            String jsonLog = JsonConvert.SerializeObject(logModel.ToArray());

            String sExMsg = "Exception:" + ex.Message + System.Environment.NewLine;
            sExMsg += "Controller:" + "SH_APP" + System.Environment.NewLine;
            sExMsg += "Action:" + actionName + System.Environment.NewLine;
            sExMsg += "JsonLog:" + jsonLog + System.Environment.NewLine;
            sExMsg += "InnerException:" + CommonUtility.Null2Empty(ex.InnerException) + System.Environment.NewLine;
            sExMsg += "StackTrace:" + ex.StackTrace + System.Environment.NewLine;
            ERPLog.WriteError(sExMsg);
        }
        private void LogDebug(SH_APP sh_app, string actionName)
        {
            List<DebugLogModel> logModel = new List<DebugLogModel>();
            logModel.Add(new DebugLogModel
            {
                Name = "ID",
                Value = sh_app.ID.ToString()
            });
            logModel.Add(new DebugLogModel {
                Name = "ModuleID",
                Value = sh_app.ModuleID.ToString()
            });
            logModel.Add(new DebugLogModel
            {
                Name = "FunctionName",
                Value = sh_app.FunctionName
            });

            String jsonLog = JsonConvert.SerializeObject(logModel.ToArray());
            DebugLog debugLog = new DebugLog();
            debugLog.ControllerName = "SH_APP";
            debugLog.ActionName = actionName;
            debugLog.JsonLog = jsonLog;
            debugLog.UserID = Session["UserID"].ToString();
            debugLog.PluginName = "ERPCore";
            debugLog.CreateDate = System.DateTime.Now;
            debugLogRepository.SaveDebugLog(debugLog);

            
        }

        // GET: SH_APP/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //SH_APP sH_APP = db.SH_APP.Find(id);
            //if (sH_APP == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(sH_APP);
            var model = (from p in appRepository.SH_APP
                         where p.ID.Equals(id)
                         orderby p.ID
                         select p).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: SH_APP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //SH_APP sH_APP = db.SH_APP.Find(id);
            //db.SH_APP.Remove(sH_APP);
            //db.SaveChanges();
            SH_APP sh_app = appRepository.SH_APP.FirstOrDefault(p => p.ID == id);
            if (sh_app != null)
            {
                appRepository.DeleteSH_APP(sh_app);
                TempData["DeleteMessage"] = string.Format("{0} was deleted", sh_app.FunctionName);
            }
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        //db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
