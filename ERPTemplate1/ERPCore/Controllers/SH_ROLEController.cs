using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ERPDomain.Entities;
using ERPDomain.Abstract;
using System;
using ERPDomain.Models;
using ERPDomain.Helpers;
using System.Collections.Generic;

namespace ERPCore.Controllers
{
    public class SH_ROLEController : BaseController
    {
        //private EFDbContext db = new EFDbContext();
        private ISH_ROLERepository roleRepository;
        private ISH_ROLEACCESSRepository roleAccessRepository;
        private ISH_APPRepository appRepository;
        private IDynamicMenuRepository dynamicMenuRepository;

        public SH_ROLEController(ISH_ROLERepository roleRepo, ISH_ROLEACCESSRepository roleAccessRepo, ISH_APPRepository appRepo, IDynamicMenuRepository dynamicMenuRepo, IErrorLogRepository errorLogRepo) : base(errorLogRepo)
        {
            this.roleRepository = roleRepo;
            this.roleAccessRepository = roleAccessRepo;
            this.appRepository = appRepo;
            this.dynamicMenuRepository = dynamicMenuRepo;
        }

        // GET: SH_ROLE
        public ActionResult Index()
        {
            TempData["Message"] = "";
            TempData["DeleteMessage"] = "";
            Session["RoleID"] = null;
            var model = from m in roleRepository.SH_ROLE
                        select m;
            return View(model);
            //return View(db.SH_ROLE.ToList());
        }

        // GET: SH_ROLE/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var roleAccessModel = new RoleAccessModel();

            roleAccessModel.SH_ROLE = (from p in roleRepository.SH_ROLE
                                       where p.ID.Equals(id)
                                       orderby p.RoleName
                                       select p).FirstOrDefault();
            if (id != null)
            {
                roleAccessModel.SH_ROLEACCESSS = roleAccessModel.SH_ROLE.SH_ROLEACCESS;
            }

            roleAccessModel.SH_APPS = from m in appRepository.SH_APP
                                      orderby m.FunctionName
                                      select m;

            if (roleAccessModel == null)
            {
                return HttpNotFound();
            }
            return PartialView(roleAccessModel);
        }

        // GET: SH_ROLE/Create
        public ActionResult Create()
        {
            Session["RoleID"] = null;
            return View(new RoleAccessModel());
        }

        // POST: SH_ROLE/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RoleName,RoleDescription,AdminFlag,CreateDate,UpdateDate")] SH_ROLE sh_role)
        {
            var roleAccessModel = new RoleAccessModel();
            if (ModelState.IsValid)
            {
                //db.SH_ROLE.Add(sH_ROLE);
                //db.SaveChanges();
                sh_role.CreateDate = DateTime.Now;
                sh_role.UpdateDate = DateTime.Now;
                roleRepository.SaveSH_ROLE(sh_role);
                Session["RoleID"] = sh_role.ID;
                Session["RoleName"] = sh_role.RoleName;

                var dynamicMenuModel = from m in dynamicMenuRepository.DynamicMenu
                               where m.MenuLevel.Equals(2)
                               orderby m.MenuName
                               select m;



                foreach (var item in dynamicMenuModel)
                {
                    SH_ROLEACCESS sh_roleaccess = new SH_ROLEACCESS();
                    sh_roleaccess.SH_ROLEID = sh_role.ID;
                    sh_roleaccess.DynamicMenuID = item.ID;
                    sh_roleaccess.IsChecker = false;
                    sh_roleaccess.AllowRead = false;
                    sh_roleaccess.AllowInsert = false;
                    sh_roleaccess.AllowUpdate = false;
                    sh_roleaccess.AllowDelete = false;
                    sh_roleaccess.UpdateDate = DateTime.Now;
                    sh_roleaccess.CreateDate = DateTime.Now;
                    roleAccessRepository.SaveSH_ROLEACCESS(sh_roleaccess);
                }

                roleAccessModel.SH_ROLE = (from p in roleRepository.SH_ROLE
                                           where p.ID.Equals(sh_role.ID)
                                           orderby p.RoleName
                                           select p).FirstOrDefault();
                if (sh_role.ID != 0)
                {
                    roleAccessModel.SH_ROLEACCESSS = from p in roleAccessRepository.SH_ROLEACCESS
                                                     where p.SH_ROLEID.Equals(sh_role.ID)
                                                     select p;
                    //roleAccessModel.SH_ROLEACCESSS = roleAccessModel.SH_ROLE.SH_ROLEACCESS;
                }

                roleAccessModel.DynamicMenus = from m in dynamicMenuRepository.DynamicMenu
                                          where m.MenuLevel.Equals(2)
                                          orderby m.MenuName
                                          select m;

                TempData["Message"] = string.Format("{0} was added", sh_role.RoleName);
                //return RedirectToAction("Index");
            }

            return View(roleAccessModel);

        }

        private void PopulateAppDropDownList(object selectedApp = null)
        {
            var appQuery = from d in appRepository.SH_APP
                           orderby d.FunctionName
                           select d;
            ViewBag.SH_APPID2 = new SelectList(appQuery, "ID", "FunctionName", selectedApp);
        }

        private void PopulateDynamicMenuDropDownList(object selectedDynamicMenu = null)
        {
            var dynamicMenuQuery = from d in dynamicMenuRepository.DynamicMenu
                           where d.MenuLevel.Equals(2)
                           orderby d.MenuName
                           select d;
            ViewBag.DynamicMenuID2 = new SelectList(dynamicMenuQuery, "ID", "MenuName", selectedDynamicMenu);
        }

        public ActionResult CreateRoleAndRoleAccess(string roleName)
        {

            PopulateAppDropDownList();
            return PartialView(new SH_ROLEACCESS());
        }

        public ActionResult CreateRoleAndRoleAccess2(string roleName)
        {

            PopulateDynamicMenuDropDownList();
            return PartialView(new SH_ROLEACCESS());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRoleAndRoleAccess(SH_ROLEACCESS sh_roleaccess)
        {
            if (ModelState.IsValid)
            {
                int iSH_APPID2 = Int32.Parse(Request.Form["SH_APPID2"].ToString());
                sh_roleaccess.SH_ROLEID = Int32.Parse(Session["RoleID"].ToString());
                sh_roleaccess.SH_APPID = iSH_APPID2;
                sh_roleaccess.UpdateDate = DateTime.Now;
                sh_roleaccess.CreateDate = DateTime.Now;
                roleAccessRepository.SaveSH_ROLEACCESS(sh_roleaccess);
                //ViewBag.Message = string.Format("{0} was updated", sh_role.RoleName);
                //return RedirectToAction("Index");
                TempData["Message"] = "Record was added";
                TempData["DeleteMessage"] = "";
                PopulateAppDropDownList(sh_roleaccess.SH_APPID);
                return RedirectToAction("Edit2",  new { id = sh_roleaccess.SH_ROLEID });
            }
            return View(sh_roleaccess);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRoleAndRoleAccess2(SH_ROLEACCESS sh_roleaccess)
        {
            if (ModelState.IsValid)
            {
                int iDynamicMenuID2 = Int32.Parse(Request.Form["DynamicMenuID2"].ToString());
                sh_roleaccess.SH_ROLEID = Int32.Parse(Session["RoleID"].ToString());
                sh_roleaccess.DynamicMenuID = iDynamicMenuID2;
                sh_roleaccess.UpdateDate = DateTime.Now;
                sh_roleaccess.CreateDate = DateTime.Now;
                roleAccessRepository.SaveSH_ROLEACCESS(sh_roleaccess);
                //ViewBag.Message = string.Format("{0} was updated", sh_role.RoleName);
                //return RedirectToAction("Index");
                TempData["Message"] = "Record was added";
                TempData["DeleteMessage"] = "";
                PopulateDynamicMenuDropDownList(sh_roleaccess.DynamicMenuID);
                return RedirectToAction("Edit2", new { id = sh_roleaccess.SH_ROLEID });
            }
            return View(sh_roleaccess);
        }

        public ActionResult CreateRoleAccess(int? roleId, string roleName)
        {
            Session["RoleID"] = roleId;
            Session["RoleName"] = roleName;
            PopulateAppDropDownList();
            return PartialView(new SH_ROLEACCESS());
        }

        public ActionResult CreateRoleAccess2(int? roleId, string roleName)
        {
            Session["RoleID"] = roleId;
            Session["RoleName"] = roleName;
            PopulateDynamicMenuDropDownList();
            return PartialView(new SH_ROLEACCESS());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRoleAccess(SH_ROLEACCESS sh_roleaccess)
        {
            if (ModelState.IsValid)
            {
                int iSH_APPID2 = Int32.Parse(Request.Form["SH_APPID2"].ToString());
                sh_roleaccess.SH_ROLEID = Int32.Parse(Session["RoleID"].ToString());
                sh_roleaccess.SH_APPID = iSH_APPID2;
                sh_roleaccess.UpdateDate = DateTime.Now;
                sh_roleaccess.CreateDate = DateTime.Now;
                bool isExist = isRoleAppExist(sh_roleaccess.SH_ROLEID, sh_roleaccess.SH_APPID);
                if (isExist == true)
                {
                    TempData["Message"] = "Record already exist !";
                }
                else
                {
                    TempData["Message"] = "";
                    roleAccessRepository.SaveSH_ROLEACCESS(sh_roleaccess);
                }

                //ViewBag.Message = string.Format("{0} was updated", sh_role.RoleName);
                //return RedirectToAction("Index");
                PopulateAppDropDownList(sh_roleaccess.SH_APPID);
                return RedirectToAction("Edit2", new { id = sh_roleaccess.SH_ROLEID });
            }
            return View(sh_roleaccess);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRoleAccess2(SH_ROLEACCESS sh_roleaccess)
        {
            if (ModelState.IsValid)
            {
                int iDynamicMenuID2 = Int32.Parse(Request.Form["DynamicMenuID2"].ToString());
                sh_roleaccess.SH_ROLEID = Int32.Parse(Session["RoleID"].ToString());
                sh_roleaccess.DynamicMenuID = iDynamicMenuID2;
                sh_roleaccess.UpdateDate = DateTime.Now;
                sh_roleaccess.CreateDate = DateTime.Now;
                //20161130
                bool isExist = isRoleDynamicMenuExist(sh_roleaccess.SH_ROLEID, sh_roleaccess.DynamicMenuID);
                if (isExist == true)
                {
                    TempData["Message"] = "Record already exist !";
                }
                else
                {
                    TempData["Message"] = "";
                    roleAccessRepository.SaveSH_ROLEACCESS(sh_roleaccess);
                }

                //ViewBag.Message = string.Format("{0} was updated", sh_role.RoleName);
                //return RedirectToAction("Index");
                PopulateAppDropDownList(sh_roleaccess.SH_APPID);
                return RedirectToAction("Edit2", new { id = sh_roleaccess.SH_ROLEID });
            }
            return View(sh_roleaccess);
        }

        private bool isRoleAppExist(int? roleId, int? appId)
        {
            bool recordExist = false;
            SH_ROLEACCESS sh_roleaccess = roleAccessRepository.SH_ROLEACCESS.FirstOrDefault(p => p.SH_ROLEID == roleId && p.SH_APPID == appId);
            if (sh_roleaccess != null)
            {
                recordExist = true;
            }
            return recordExist;
        }

        private bool isRoleDynamicMenuExist(int? roleId, int? dynamicMenuId)
        {
            bool recordExist = false;
            SH_ROLEACCESS sh_roleaccess = roleAccessRepository.SH_ROLEACCESS.FirstOrDefault(p => p.SH_ROLEID == roleId && p.DynamicMenuID == dynamicMenuId);
            if (sh_roleaccess != null)
            {
                recordExist = true;
            }
            return recordExist;
        }

        public ActionResult EditRoleAccess(int? roleId, int? roleAccessId, string roleName)
        {
            Session["RoleName"] = roleName;
            SH_ROLEACCESS sh_roleaccess = roleAccessRepository.SH_ROLEACCESS.FirstOrDefault(p => p.ID == roleAccessId && p.SH_ROLEID == roleId);
            PopulateAppDropDownList(sh_roleaccess.SH_APPID);
            return PartialView(sh_roleaccess);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRoleAccess(SH_ROLEACCESS sh_roleaccess)
        {
            if (ModelState.IsValid)
            {
                int iSH_APPID2 = Int32.Parse(Request.Form["SH_APPID2"].ToString());
                sh_roleaccess.SH_APPID = iSH_APPID2;
                sh_roleaccess.UpdateDate = DateTime.Now;

                roleAccessRepository.SaveSH_ROLEACCESS(sh_roleaccess);
                //ViewBag.Message = string.Format("{0} was updated", sh_role.RoleName);
                //return View("~/Views/SH_ROLE/Edit/" + sh_roleaccess.SH_ROLEID);
                Session["RoleID"] = sh_roleaccess.SH_ROLEID;
                //return View("Edit");
                TempData["Message"] = "Record was updated";
                TempData["DeleteMessage"] = "";
                return RedirectToAction("Edit2", new { id = sh_roleaccess.SH_ROLEID });
            }
            return PartialView(sh_roleaccess);
            //return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadRoleAccessForBatchUpdate(List<SH_ROLEACCESS> sh_roleaccesss)
        {
            if (sh_roleaccesss != null)
            {
                foreach (var item in sh_roleaccesss)
                {
                    SH_ROLEACCESS ra = (from p in roleAccessRepository.SH_ROLEACCESS
                                        where p.ID.Equals(item.ID)
                                        select p).FirstOrDefault();
                    ra.IsChecker = item.IsChecker;
                    ra.AllowRead = item.AllowRead;
                    ra.AllowInsert = item.AllowInsert;
                    ra.AllowUpdate = item.AllowUpdate;
                    ra.AllowDelete = item.AllowDelete;
                    ra.AllowPrint = item.AllowPrint;
                    roleAccessRepository.SaveSH_ROLEACCESS(ra);
                    //Console.Out.WriteLine("AllowRead = " + item.AllowRead);
                }
            }

            TempData["Message"] = string.Format("{0} was updated", Session["RoleName"]);
            TempData["DeleteMessage"] = "";
            return RedirectToAction("Edit2", new { id = Session["RoleID"] });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadRoleAccessForBatchUpdate2(List<SH_ROLEACCESS> sh_roleaccesss)
        {
            if (sh_roleaccesss != null)
            {
                foreach (var item in sh_roleaccesss)
                {
                    SH_ROLEACCESS ra = (from p in roleAccessRepository.SH_ROLEACCESS
                                        where p.ID.Equals(item.ID)
                                        select p).FirstOrDefault();
                    ra.IsChecker = item.IsChecker;
                    ra.AllowRead = item.AllowRead;
                    ra.AllowInsert = item.AllowInsert;
                    ra.AllowUpdate = item.AllowUpdate;
                    ra.AllowDelete = item.AllowDelete;
                    ra.AllowPrint = item.AllowPrint;
                    roleAccessRepository.SaveSH_ROLEACCESS(ra);
                    //Console.Out.WriteLine("AllowRead = " + item.AllowRead);
                }
            }

            TempData["Message"] = string.Format("{0} was updated", Session["RoleName"]);
            TempData["DeleteMessage"] = "";
            return RedirectToAction("Edit2", new { id = Session["RoleID"] });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult LoadRoleAccessForBatchUpdate0(int? roleId, string roleName)
        {
            Session["RoleID"] = roleId;
            Session["RoleName"] = roleName;
            var model = from p in roleAccessRepository.SH_ROLEACCESS
                        where p.SH_ROLEID.Equals(roleId)
                        orderby p.SH_ROLE.RoleName
                        select p;
            return PartialView("LoadRoleAccessForBatchUpdate0", model.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult LoadRoleAccessForBatchUpdate02(int? roleId, string roleName)
        {
            Session["RoleID"] = roleId;
            Session["RoleName"] = roleName;
            var model = from p in roleAccessRepository.SH_ROLEACCESS
                        where p.SH_ROLEID.Equals(roleId)
                        orderby p.SH_ROLE.RoleName
                        select p;
            return PartialView("LoadRoleAccessForBatchUpdate02", model.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult LoadRoleAccess0(int? roleId, string roleName)
        {
            Session["RoleID"] = roleId;
            Session["RoleName"] = roleName;
            var model = from p in roleAccessRepository.SH_ROLEACCESS
                        where p.SH_ROLEID.Equals(roleId)
                        orderby p.SH_ROLE.RoleName
                        select p;
            return PartialView("LoadRoleAccess0", model.ToList());
        }

        public PartialViewResult LoadRoleAccess1(int? roleId, string roleName)
        {
            Session["RoleID"] = roleId;
            Session["RoleName"] = roleName;
            var model = from p in roleAccessRepository.SH_ROLEACCESS
                        where p.SH_ROLEID.Equals(roleId)
                        orderby p.SH_ROLE.RoleName
                        select p;
            return PartialView("LoadRoleAccess1", model.ToList());
        }

        public PartialViewResult LoadRoleAccessReadOnly(int? roleId, string roleName)
        {
            Session["RoleID"] = roleId;
            Session["RoleName"] = roleName;
            var model = from p in roleAccessRepository.SH_ROLEACCESS
                        where p.SH_ROLEID.Equals(roleId)
                        orderby p.SH_ROLE.RoleName
                        select p;
            return PartialView("LoadRoleAccessReadOnly", model.ToList());
        }

        public PartialViewResult LoadRoleAccessReadOnly2(int? roleId, string roleName)
        {
            Session["RoleID"] = roleId;
            Session["RoleName"] = roleName;
            var model = from p in roleAccessRepository.SH_ROLEACCESS
                        where p.SH_ROLEID.Equals(roleId)
                        orderby p.SH_ROLE.RoleName
                        select p;
            return PartialView("LoadRoleAccessReadOnly2", model.ToList());
        }

        public PartialViewResult LoadRoleAccessForBatchUpdate(int? roleId, string roleName)
        {
            Session["RoleID"] = roleId;
            Session["RoleName"] = roleName;
            var model = from p in roleAccessRepository.SH_ROLEACCESS
                        where p.SH_ROLEID.Equals(roleId)
                        orderby p.SH_ROLE.RoleName
                        select p;
            return PartialView(model.ToList());
        }

        public PartialViewResult LoadRoleAccessForBatchUpdate2(int? roleId, string roleName)
        {
            Session["RoleID"] = roleId;
            Session["RoleName"] = roleName;
            var model = from p in roleAccessRepository.SH_ROLEACCESS
                        where p.SH_ROLEID.Equals(roleId)
                        orderby p.SH_ROLE.RoleName
                        select p;
            return PartialView(model.ToList());
        }



        // GET: SH_ROLE/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var roleAccessModel = new RoleAccessModel();

            roleAccessModel.SH_ROLE = (from p in roleRepository.SH_ROLE
                                       where p.ID.Equals(id)
                                       orderby p.RoleName
                                       select p).FirstOrDefault();
            if (id != null)
            {
                roleAccessModel.SH_ROLEACCESSS = roleAccessModel.SH_ROLE.SH_ROLEACCESS;
            }

            //roleAccessModel.SH_APPS = from m in appRepository.SH_APP
            //                          orderby m.FunctionName
            //                          select m;
            roleAccessModel.DynamicMenus = from m in dynamicMenuRepository.DynamicMenu
                                           where m.MenuLevel.Equals(2)
                                      orderby m.MenuName
                                      select m;

            if (roleAccessModel == null)
            {
                return HttpNotFound();
            }
            return PartialView(roleAccessModel);
        }

        // POST: SH_ROLE/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RoleName,RoleDescription,AdminFlag,UpdateDate")] SH_ROLE sh_role)
        {
            if (ModelState.IsValid)
            {
                sh_role.UpdateDate = DateTime.Now;
                roleRepository.SaveSH_ROLE(sh_role);

                TempData["Message"] = string.Format("{0} was updated", sh_role.RoleName);
                return RedirectToAction("Index");
            }
            return PartialView(sh_role);
        }

        //public ActionResult Edit(RoleAccessModel roleAccessModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //db.Entry(sH_ROLE).State = EntityState.Modified;
        //        //db.SaveChanges();
        //        roleAccessModel.SH_ROLE.UpdateDate = DateTime.Now;
        //        //sh_role.Status = "D";
        //        foreach (var item in roleAccessModel.SH_ROLEACCESSS)
        //        {
        //            Console.Out.WriteLine("AllowRead = " + item.AllowRead);
        //        }
        //        roleRepository.SaveSH_ROLE(roleAccessModel.SH_ROLE);


        //        ViewBag.Message = string.Format("{0} was updated", roleAccessModel.SH_ROLE.RoleName);
        //        return RedirectToAction("Index");
        //    }
        //    return PartialView(roleAccessModel);
        //}
        public ActionResult Edit2(int? id)
        {
            Session["RoleID"] = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var roleAccessModel = new RoleAccessModel();

            roleAccessModel.SH_ROLE = (from p in roleRepository.SH_ROLE
                                       where p.ID.Equals(id)
                                       orderby p.RoleName
                                       select p).FirstOrDefault();
            if (id != null)
            {
                roleAccessModel.SH_ROLEACCESSS = roleAccessModel.SH_ROLE.SH_ROLEACCESS;
            }

            if (roleAccessModel == null)
            {
                return HttpNotFound();
            }
            return View("Edit2", roleAccessModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2([Bind(Include = "ID,RoleName,RoleDescription,AdminFlag,UpdateDate")] SH_ROLE sh_role)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(sH_ROLE).State = EntityState.Modified;
                //db.SaveChanges();
                sh_role.UpdateDate = DateTime.Now;
                //sh_role.Status = "D";
                roleRepository.SaveSH_ROLE(sh_role);
                ViewBag.Message = string.Format("{0} was updated", sh_role.RoleName);
                return RedirectToAction("Index");
            }
            return View(sh_role);
        }

        // GET: SH_ROLE/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    //SH_ROLE sH_ROLE = db.SH_ROLE.Find(id);
        //    //if (sH_ROLE == null)
        //    //{
        //    //    return HttpNotFound();
        //    //}
        //    //return View(sH_ROLE);
        //    var model = (from p in roleRepository.SH_ROLE
        //                 where p.ID.Equals(id)
        //                 orderby p.ID
        //                 select p).FirstOrDefault();

        //    if (model == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return PartialView(model);
        //}

        // POST: SH_ROLE/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {

            SH_ROLE sh_role = roleRepository.SH_ROLE.FirstOrDefault(p => p.ID == id);

            foreach (var item in sh_role.SH_ROLEACCESS)
            {
                SH_ROLEACCESS sh_roleaccess = roleAccessRepository.SH_ROLEACCESS.FirstOrDefault(p => p.ID == item.ID);
                roleAccessRepository.DeleteSH_ROLEACCESS(sh_roleaccess);
            }

            if (sh_role != null)
            {
                roleRepository.DeleteSH_ROLE(sh_role);
                TempData["DeleteMessage"] = string.Format("{0} was deleted", sh_role.RoleName);
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteRoleAccess(int roleId, int roleAccessId)
        {

            SH_ROLEACCESS sh_roleaccess = roleAccessRepository.SH_ROLEACCESS.FirstOrDefault(p => p.ID == roleAccessId);

            if (sh_roleaccess != null)
            {
                string functionName = sh_roleaccess.SH_APP.FunctionName;

                roleAccessRepository.DeleteSH_ROLEACCESS(sh_roleaccess);
                TempData["Message"] = "";
                TempData["DeleteMessage"] = string.Format("{0} was deleted", functionName);

            }

            return RedirectToAction("Edit2", new { id = roleId });
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
