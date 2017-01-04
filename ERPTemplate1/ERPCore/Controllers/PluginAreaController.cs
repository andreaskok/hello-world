using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ERPDomain.Entities;
using ERPDomain.Abstract;
using PagedList;
using ERPDomain.Models;
using ERPDomain.Helpers;

namespace ERPCore.Controllers
{
    public class PluginAreaController : BaseController
    {
        //private EFDbContextGL db = new EFDbContextGL();
        private IPluginAreaRepository pluginAreaRepository;
        private IParentMenuRepository parentMenuRepository;

        public PluginAreaController(IPluginAreaRepository pluginAreaRepo, IParentMenuRepository parentMenuRepo, IErrorLogRepository errorLogRepo) : base(errorLogRepo)
        {
            this.pluginAreaRepository = pluginAreaRepo;
            this.parentMenuRepository = parentMenuRepo;
        }

        // GET: PluginArea
        public ActionResult Index()
        {
            var model = from m in pluginAreaRepository.PluginArea
                        select m;
            return View(model);
        }

        // GET: PluginArea/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //PluginArea pluginArea = db.PluginAreas.Find(id);
            //if (pluginArea == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(pluginArea);
            var model = (from p in pluginAreaRepository.PluginArea
                         where p.ID.Equals(id)
                         orderby p.AreaCode
                         select p).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: PluginArea/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PluginArea/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AreaCode,AreaName,Buy,CreateDate,UpdateDate")] PluginArea pluginArea)
        {
            if (ModelState.IsValid)
            {
                //db.PluginAreas.Add(pluginArea);
                //db.SaveChanges();
                pluginAreaRepository.SavePluginArea(pluginArea);
                TempData["Message"] = string.Format("{0} was added", pluginArea.AreaCode);
                return RedirectToAction("Index");
            }

            return View(pluginArea);
        }

        // GET: PluginArea/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //PluginArea pluginArea = db.PluginAreas.Find(id);
            //if (pluginArea == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(pluginArea);
            var model = (from p in pluginAreaRepository.PluginArea
                         where p.ID.Equals(id)
                         orderby p.AreaCode
                         select p).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: PluginArea/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AreaCode,AreaName,Buy,CreateDate,UpdateDate")] PluginArea pluginArea)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(pluginArea).State = EntityState.Modified;
                //db.SaveChanges();
                pluginAreaRepository.SavePluginArea(pluginArea);
                ViewBag.Message = string.Format("{0} was updated", pluginArea.AreaCode);
                return RedirectToAction("Index");
            }
            return View(pluginArea);
        }

        // GET: PluginArea/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //PluginArea pluginArea = db.PluginAreas.Find(id);
            //if (pluginArea == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(pluginArea);
            var model = (from p in pluginAreaRepository.PluginArea
                         where p.ID.Equals(id)
                         orderby p.ID
                         select p).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: PluginArea/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //PluginArea pluginArea = db.PluginAreas.Find(id);
            //db.PluginAreas.Remove(pluginArea);
            //db.SaveChanges();
            PluginArea pluginArea = pluginAreaRepository.PluginArea.FirstOrDefault(p => p.ID == id);
            if (pluginArea != null)
            {
                pluginAreaRepository.DeletePluginArea(pluginArea);
                TempData["DeleteMessage"] = string.Format("{0} was deleted", pluginArea.AreaCode);
            }
            return RedirectToAction("Index");
        }

        public ActionResult UpdatePlugin(string plugin)
        {
            String sResultValue = "Failed";
            PluginArea pluginArea = pluginAreaRepository.PluginArea.FirstOrDefault(p => p.AreaName.Trim().ToUpper().Equals(plugin.Trim().ToUpper()));
            if (pluginArea != null)
            {
                pluginArea.Buy = true;
                pluginAreaRepository.SavePluginArea(pluginArea);
                sResultValue = "Success";
            }
            ParentMenu parentMenu = parentMenuRepository.ParentMenu.FirstOrDefault(p => p.Name.Trim().ToUpper().Equals(plugin.Trim().ToUpper()));
            if (parentMenu != null)
            {
                parentMenu.Buy = true;
                parentMenuRepository.SaveParentMenu(parentMenu);
            }
            return Json(new { result = sResultValue });
        }

        public ActionResult RemovePlugin(string plugout)
        {
            String sResultValue = "Failed";
            PluginArea pluginArea = pluginAreaRepository.PluginArea.FirstOrDefault(p => p.AreaName.Trim().ToUpper().Equals(plugout.Trim().ToUpper()));
            if (pluginArea != null)
            {
                pluginArea.Buy = false;
                pluginAreaRepository.SavePluginArea(pluginArea);
                sResultValue = "Success";
            }
            ParentMenu parentMenu = parentMenuRepository.ParentMenu.FirstOrDefault(p => p.Name.Trim().ToUpper().Equals(plugout.Trim().ToUpper()));
            if (parentMenu != null)
            {
                parentMenu.Buy = false;
                parentMenuRepository.SaveParentMenu(parentMenu);
            }
            return Json(new { result = sResultValue });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();

            }
            base.Dispose(disposing);
        }
    }
}
