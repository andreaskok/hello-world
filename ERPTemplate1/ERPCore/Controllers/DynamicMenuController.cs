using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoddleReport;
using DoddleReport.Web;
using ERPDomain.Abstract;
using ERPDomain.Entities;
using ERPDomain.Helpers;
using ERPDomain.Models;
using PagedList;

namespace ERPCore.Controllers
{
    //Designed for unlimited generation
    public class DynamicMenuController : BaseController
    {
        private IDynamicMenuRepository dynamicMenuRepository;
        private ISH_USERROLERepository userRoleRepository;
        private ISH_ROLEACCESSRepository roleAccessRepository;
        public DynamicMenuController(IDynamicMenuRepository dynamicMenuRepo,ISH_USERROLERepository userRoleRepo, ISH_ROLEACCESSRepository roleAccessRepo, IErrorLogRepository errorLogRepo) : base(errorLogRepo)
        {
            this.dynamicMenuRepository = dynamicMenuRepo;
            this.userRoleRepository = userRoleRepo;
            this.roleAccessRepository = roleAccessRepo;
        }
        public ActionResult Index(string searchValue, string sortOrder, string currentFilter, int? page, string sortBy)
        {
            sortBy = CommonUtility.Null2Empty(sortBy);
            TempData["Message"] = "";
            TempData["DeleteMessage"] = "";
            Session["MenuName"] = null;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchValue != null)
            {
                page = 1;
            }
            else
            {
                searchValue = currentFilter;
            }

            ViewBag.CurrentFilter = searchValue;

            var model = from m in dynamicMenuRepository.DynamicMenu
                        select m;

            if (!String.IsNullOrEmpty(searchValue))
            {
                model = model.Where(s => s.MenuName.ToUpper().Contains(searchValue.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    if (sortBy.Equals("ID"))
                    {
                        model = model.OrderByDescending(s => s.ID);
                    }
                    else if (sortBy.Equals("ParentID"))
                    {
                        model = model.OrderByDescending(s => s.ParentID);
                    }
                    else if (sortBy.Equals("MenuLevel"))
                    {
                        model = model.OrderByDescending(s => s.MenuLevel);
                    }
                    else if (sortBy.Equals("MenuName"))
                    {
                        model = model.OrderByDescending(s => s.MenuName);
                    }
                    else if (sortBy.Equals("ControllerName"))
                    {
                        model = model.OrderByDescending(s => s.ControllerName);
                    }
                    else if (sortBy.Equals("MethodName"))
                    {
                        model = model.OrderByDescending(s => s.MethodName);
                    }
                    else if (sortBy.Equals("AreaName"))
                    {
                        model = model.OrderByDescending(s => s.AreaName);
                    }

                    break;
                default:
                    if (sortBy.Equals("ID"))
                    {
                        model = model.OrderBy(s => s.ID);
                    }
                    else if (sortBy.Equals("ParentID"))
                    {
                        model = model.OrderBy(s => s.ParentID);
                    }
                    else if (sortBy.Equals("MenuLevel"))
                    {
                        model = model.OrderBy(s => s.MenuLevel);
                    }
                    else if (sortBy.Equals("MenuName"))
                    {
                        model = model.OrderBy(s => s.MenuName);
                    }
                    else if (sortBy.Equals("ControllerName"))
                    {
                        model = model.OrderBy(s => s.ControllerName);
                    }
                    else if (sortBy.Equals("MethodName"))
                    {
                        model = model.OrderBy(s => s.MethodName);
                    }
                    else if (sortBy.Equals("AreaName"))
                    {
                        model = model.OrderBy(s => s.AreaName);
                    }
                    break;
            }

            int pageSize = Int32.Parse(CommonUtility.Empty2Zero(System.Web.Configuration.WebConfigurationManager.AppSettings["RowPerPage"]));
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        private void PopulateParentDropDownList(object selectedParent = null)
        {
            var parentQuery = from m in dynamicMenuRepository.DynamicMenu
                            orderby m.MenuName
                            select m;
            ViewBag.ParentID2 = new SelectList(parentQuery, "ID", "MenuName", selectedParent);
        }

        public ActionResult Create()
        {
            PopulateParentDropDownList();
            return View(new DynamicMenu());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ParentID,MenuLevel,MenuName,ControllerName,MethodName,AreaName,Plugin,Buy")] DynamicMenu dynamicMenu)
        {
            if (ModelState.IsValid)
            {
                int iParentID2 = Int32.Parse(CommonUtility.Empty2Zero(Request.Form["ParentID2"].ToString()));
                dynamicMenu.ParentID = iParentID2;
                dynamicMenu.CreateDate = DateTime.Now;
                dynamicMenu.UpdateDate = DateTime.Now;
                dynamicMenu.ControllerName = CommonUtility.Null2Empty(dynamicMenu.ControllerName);
                dynamicMenu.MethodName = CommonUtility.Null2Empty(dynamicMenu.MethodName);
                dynamicMenu.PluginName = CommonUtility.Null2Empty(dynamicMenu.PluginName);
                dynamicMenuRepository.SaveDynamicMenu(dynamicMenu);
                Session["MenuName"] = dynamicMenu.MenuName;
                ViewBag.Message = string.Format("{0} was added", dynamicMenu.MenuName);

                TempData["Message"] = "Data has been saved succeessfully";
                return RedirectToAction("Index");
            }

            //return PartialView(sh_user);
            return View(dynamicMenu);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = (from p in dynamicMenuRepository.DynamicMenu
                         where p.ID.Equals(id)
                         select p).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }
            PopulateParentDropDownList(model.ParentID);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ParentID,MenuLevel,MenuName,ControllerName,MethodName,AreaName,Plugin,Buy")] DynamicMenu dynamicMenu)
        {
            if (ModelState.IsValid)
            {
                int iParentID2 = Int32.Parse(CommonUtility.Empty2Zero(Request.Form["ParentID2"].ToString()));
                dynamicMenu.ParentID = iParentID2;
                dynamicMenu.UpdateDate = DateTime.Now;
                dynamicMenu.PluginName = CommonUtility.Null2Empty(dynamicMenu.PluginName);
                dynamicMenu.ControllerName = CommonUtility.Null2Empty(dynamicMenu.ControllerName);
                dynamicMenu.MethodName = CommonUtility.Null2Empty(dynamicMenu.MethodName);
                dynamicMenuRepository.SaveDynamicMenu(dynamicMenu);
                ViewBag.Message = string.Format("{0} was updated", dynamicMenu.MenuName);
                TempData["Msg"] = "Data has been updated succeessfully";
                return RedirectToAction("Index");
            }
            return PartialView(dynamicMenu);
        }

        public ActionResult Delete(int id)
        {
            //SH_USER sH_USER = db.SH_USER.Find(id);
            //db.SH_USER.Remove(sH_USER);
            //db.SaveChanges();
            DynamicMenu dynamicMenu = dynamicMenuRepository.DynamicMenu.FirstOrDefault(p => p.ID == id);
            if (dynamicMenu != null)
            {
                dynamicMenuRepository.DeleteDynamicMenu(dynamicMenu);
                ViewBag.DeleteMessage = string.Format("{0} was deleted", dynamicMenu.MenuName);
                TempData["Msg"] = "Data has been deleted succeessfully";
            }

            return RedirectToAction("Index");
        }

        public ReportResult Export2Excel()
        {
            //https://github.com/matthidinger/DoddleReport/wiki/Building-your-first-report
            var model = (from m in dynamicMenuRepository.DynamicMenu
                         select new { m.ID, m.ParentID, m.MenuLevel, m.MenuName }).ToList();
            var report = new Report(model.ToReportSource());
            report.TextFields.Title = "Prototype: ERP";
            report.TextFields.SubTitle = "Dynamic Menu Report";
            report.TextFields.Footer = "Copyright " + DateTime.Now.Year + " &copy; iTech Workwide Prototype";
            

            // Render hints allow you to pass additional hints to the reports as they are being rendered
            report.RenderHints.BooleanCheckboxes = true;


            DoddleReport.OpenXml.ExcelReportWriter writer = new DoddleReport.OpenXml.ExcelReportWriter();

            var report2 = new Report(model.ToReportSource(), writer);

            report2.TextFields.Title = "Prototype: ERP";
            report2.TextFields.SubTitle = "Dynamic Menu";
            report2.TextFields.Footer = "Copyright " + DateTime.Now.Year + " &copy; iTech Workwide Prototype";
            return new ReportResult(report2, writer) { FileName = "DynamicMenuRepository.xlsx" };

        }

        public PartialViewResult LoadLevel1Menu()
        {
            var model = (from m in dynamicMenuRepository.DynamicMenu
                         where m.MenuLevel.Equals(1) && m.Buy.Equals(true)
                         orderby m.MenuName
                         select m);
            return PartialView(model);
        }

        public PartialViewResult LoadLevel2Menu(int parentId)
        {
            var userRoleModel = (from m in userRoleRepository.SH_USERROLE
                                 where m.SH_USERID.Equals(int.Parse(Session["UserPK"].ToString())) && m.Assign.Equals(true)
                                 select m);
            var dmModel = (from m in dynamicMenuRepository.DynamicMenu
                         where m.MenuLevel.Equals(2) && m.ParentID.Equals(parentId) && m.Buy.Equals(true)
                         orderby m.MenuName
                         select m);
            List<DynamicMenuModel> dmList = new List<DynamicMenuModel>();
            foreach(var dmItem in dmModel)
            {
                foreach(var urItem in userRoleModel)
                {
                    SH_ROLEACCESS ra = roleAccessRepository.SH_ROLEACCESS.FirstOrDefault(p => p.SH_ROLEID.Equals(urItem.SH_ROLEID) && p.DynamicMenuID.Equals(dmItem.ID) && p.AllowRead.Equals(true));
                    if (ra != null)
                    {
                        DynamicMenuModel dmm = new DynamicMenuModel();
                        dmm.ID = dmItem.ID;
                        dmm.ParentID = dmItem.ParentID;
                        dmm.MenuLevel = dmItem.MenuLevel;
                        dmm.MenuName = dmItem.MenuName;
                        dmm.ControllerName = dmItem.ControllerName;
                        dmm.MethodName = dmItem.MethodName;
                        dmm.AreaName = dmItem.AreaName;
                        dmm.ResourceName = dmItem.ResourceName;
                        dmList.Add(dmm);
                        break;
                    }
                }
                
            }
            return PartialView(dmList.ToList());
        }

    }
}