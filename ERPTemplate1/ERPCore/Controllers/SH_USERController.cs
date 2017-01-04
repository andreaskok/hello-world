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
using DoddleReport;
using DoddleReport.Writers;
using DoddleReport.Web;
using System.Collections.Generic;

namespace ERPCore.Controllers
{
    public class SH_USERController : BaseController
    {
        //private EFDbContext db = new EFDbContext();
        private ISH_USERRepository userRepository;
        private ISH_ROLERepository roleRepository;
        private ISH_ROLEMEMBERRepository roleMemberRepository;
        private ISH_ROLEACCESSRepository roleAccessRepository;
        private ISH_USERROLERepository userRoleRepository;
        private ISH_APPRepository appRepository;
        private IUserPreferenceRepository userPreferenceRepository;
        private ILanguageRepository languageRepository;
        private IDynamicMenuRepository dynamicMenuRepository;

        public SH_USERController(ISH_USERRepository userRepo, ISH_ROLEMEMBERRepository roleMemberRepo, ISH_ROLEACCESSRepository roleAccessRepo, ISH_ROLERepository roleRepo, ISH_USERROLERepository userRoleRepo, ISH_APPRepository appRepo, IUserPreferenceRepository userPreferenceRepo, IDynamicMenuRepository dynamicMenuRepo, IErrorLogRepository errorLogRepo) : base(errorLogRepo)
        {
            this.userRepository = userRepo;
            this.roleMemberRepository = roleMemberRepo;
            this.roleAccessRepository = roleAccessRepo;
            this.roleRepository = roleRepo;
            this.userRoleRepository = userRoleRepo;
            this.appRepository = appRepo;
            this.userPreferenceRepository = userPreferenceRepo;
            this.dynamicMenuRepository = dynamicMenuRepo;
        }
        // GET: SH_USER
        
        public ActionResult Index(string searchValue, string sortOrder, string currentFilter, int? page)
        {
            TempData["Message"] = "";
            TempData["DeleteMessage"] = "";
            Session["UserID"] = null;
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

            var model = from m in userRepository.SH_USER
                        select m;

            if (!String.IsNullOrEmpty(searchValue))
            {
                model = model.Where(s => s.UserName.ToUpper().Contains(searchValue.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.UserName);
                    break;
                //case "Date":
                //    students = students.OrderBy(s => s.EnrollmentDate);
                //    break;
                //case "date_desc":
                //    students = students.OrderByDescending(s => s.EnrollmentDate);
                //    break;
                default:
                    model = model.OrderBy(s => s.UserName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
            //return View(model);
            //return View(db.SH_USER.ToList());
            //return View(userRepository.SH_USER.ToList());
        }

        public ActionResult Details2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from p in userRepository.SH_USER
                         where p.ID.Equals(id)
                         orderby p.UserID
                         select p).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }
            PopulateRoleDropDownList(model.SH_ROLEID);
            return PartialView(model);
        }
        // GET: SH_USER/Details/5
        public ActionResult Details(int? id, int? roleId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //COMPLEX MODEL
            var userRoleModel = new UserRoleModel();

            userRoleModel.SH_USER = (from p in userRepository.SH_USER
                                     where p.ID.Equals(id)
                                     orderby p.UserID
                                     select p).FirstOrDefault();

            userRoleModel.SH_USERS = (from u in userRepository.SH_USER
                                      where u.ID.Equals(id)
                                      orderby u.UserID
                                      select u);

            if (id != null)
            {
                ViewBag.UserID = id.Value;
                userRoleModel.SH_ROLEMEMBERS = userRoleModel.SH_USERS.Where(
                    i => i.ID == id.Value).Single().SH_ROLEMEMBER;
            }

            if (roleId != null)
            {
                ViewBag.RoleID = roleId.Value;

                userRoleModel.SH_ROLE = userRoleModel.SH_ROLEMEMBERS.Where(
                    i => i.SH_ROLEID == roleId.Value).Single().SH_ROLE;

                userRoleModel.SH_ROLEACCESSS = (from ra in roleAccessRepository.SH_ROLEACCESS
                                                where ra.SH_ROLEID.Equals(roleId.Value)
                                                orderby ra.SH_APPID
                                                select ra);
            }
            return View(userRoleModel);
        }

        public ActionResult Login()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session["UserPK"] = "";
            Session["UserID"] = "";
            Session["UserName"] = "";
            Session["UserEmail"] = "";
            return View("~/Views/Home/Index.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ViewResult Login(UserViewModel uvm)
        {

            SH_USER userAccount = userRepository.SH_USER.FirstOrDefault(p => p.UserEmail == uvm.LoginEmail && p.UserPwd == uvm.LoginPassword);
            if (userAccount != null)
            {
                if (userAccount.UserEmail != "")
                {
                    Session["UserPK"] = userAccount.ID;
                    Session["UserID"] = userAccount.UserID;
                    Session["UserName"] = userAccount.UserName;
                    Session["UserEmail"] = userAccount.UserEmail;

                    Session["AdminFlag"] = "False";
                    Session["SystemFlag"] = "False";
                    var userRoleModel = (from m in userRoleRepository.SH_USERROLE
                                         where m.SH_USERID.Equals(userAccount.ID) && m.Assign.Equals(true)
                                         select m);
                    foreach(var item in userRoleModel)
                    {
                        SH_ROLE sh_role = roleRepository.SH_ROLE.FirstOrDefault(p => p.ID == item.SH_ROLEID);
                        if (sh_role != null)
                        {
                            Session["AdminFlag"] = sh_role.AdminFlag;
                            Session["SystemFlag"] = sh_role.SystemFlag;
                            break;
                        }
                    }
                    //SH_ROLE sh_role = roleRepository.SH_ROLE.FirstOrDefault(p => p.ID == userAccount.SH_ROLEID);
                    //if (sh_role != null)
                    //{
                    //    Session["AdminFlag"] = sh_role.AdminFlag;
                    //    Session["SystemFlag"] = sh_role.SystemFlag;
                    //}
                    //else
                    //{
                    //    Session["AdminFlag"] = "False";
                    //    Session["SystemFlag"] = "False";
                    //}



                }
                return View("~/Views/Home/Index.cshtml");
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid login, please check !";
                return View();
            }



        }

        private bool IsAdmin(int userID)
        {
            var model = from m in userRoleRepository.SH_USERROLE
                        where m.ID.Equals(userID)
                        select m;
            foreach(var item in model)
            {
                SH_ROLE sh_role = item.SH_ROLE;
                if (sh_role.AdminFlag == true)
                {
                    break;
                }
            }
            return false;
        }
        // GET: SH_USER/Create
        public ActionResult Create()
        {
            Session["UserID"] = null;
            PopulateRoleDropDownList();
            return View();
        }

        // POST: SH_USER/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserID,SH_ROLEID,UserPwd,UserName,UserEmail")] SH_USER sh_user)
        {
            var userRoleModel = new UserRoleModel();
            if (ModelState.IsValid)
            {
                //db.SH_USER.Add(sH_USER);
                //db.SaveChanges();
                //int iSH_ROLEID2 = Int32.Parse(Request.Form["SH_ROLEID2"].ToString());
                //sh_user.SH_ROLEID = iSH_ROLEID2;
                sh_user.CreateDate = DateTime.Now;
                sh_user.UpdateDate = DateTime.Now;
                userRepository.SaveSH_USER(sh_user);
                Session["UserID"] = sh_user.UserID;
                Session["SH_USERID"] = sh_user.ID;
                ViewBag.Message = string.Format("{0} was added", sh_user.UserID);

                var roleModel = from m in roleRepository.SH_ROLE
                               orderby m.RoleName
                               select m;



                foreach (var item in roleModel)
                {
                    SH_USERROLE sh_userrole = new SH_USERROLE();
                    sh_userrole.SH_USERID = sh_user.ID;
                    sh_userrole.SH_ROLEID = item.ID;
                    sh_userrole.Assign = false;
                    userRoleRepository.SaveSH_USERROLE(sh_userrole);
                }

                userRoleModel.SH_USER = (from p in userRepository.SH_USER
                                           where p.ID.Equals(sh_user.ID)
                                           orderby p.UserName
                                           select p).FirstOrDefault();
                if (sh_user.ID != 0)
                {
                    userRoleModel.SH_USERROLE = from p in userRoleRepository.SH_USERROLE
                                                     where p.SH_USERID.Equals(sh_user.ID)
                                                     select p;
                }

                userRoleModel.SH_ROLES = from m in roleRepository.SH_ROLE
                                          orderby m.RoleName
                                          select m;
                //TempData["Msg"] = "Data has been saved succeessfully";
                TempData["Message"] = "Data has been saved succeessfully";
                //return RedirectToAction("Index");
            }
            //return PartialView(sh_user);
            return View(userRoleModel);
        }

        // GET: SH_USER/Edit/5
       
        // POST: SH_USER/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserID,SH_ROLEID,UserPwd,UserName,UserEmail")] SH_USER sh_user)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(sH_USER).State = EntityState.Modified;
                //db.SaveChanges();
                int iSH_ROLEID = Int32.Parse(Request.Form["SH_ROLEID"].ToString());
                sh_user.SH_ROLEID = iSH_ROLEID;
                sh_user.UpdateDate = DateTime.Now;
                userRepository.SaveSH_USER(sh_user);
                ViewBag.Message = string.Format("{0} was updated", sh_user.UserID);
                TempData["Msg"] = "Data has been updated succeessfully";
                return RedirectToAction("Index");
            }
            return PartialView(sh_user);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = (from p in userRepository.SH_USER
                         where p.ID.Equals(id)
                         orderby p.UserID
                         select p).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }
            PopulateRoleDropDownList(model.SH_ROLEID);
            return PartialView(model);
        }

        public ActionResult Edit2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = (from p in userRepository.SH_USER
                         where p.ID.Equals(id)
                         orderby p.UserID
                         select p).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }
            PopulateRoleDropDownList(model.SH_ROLEID);
            return View(model);
        }


        // GET: SH_USER/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    //SH_USER sH_USER = db.SH_USER.Find(id);
        //    //if (sH_USER == null)
        //    //{
        //    //    return HttpNotFound();
        //    //}
        //    //return View(sH_USER);
        //    var model = (from p in userRepository.SH_USER
        //                 where p.ID.Equals(id)
        //                 orderby p.UserID
        //                 select p).FirstOrDefault();

        //    if (model == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(model);
        //}

        // POST: SH_USER/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            //SH_USER sH_USER = db.SH_USER.Find(id);
            //db.SH_USER.Remove(sH_USER);
            //db.SaveChanges();
            SH_USER sh_user = userRepository.SH_USER.FirstOrDefault(p => p.ID == id);
            if (sh_user != null)
            {
                userRepository.DeleteSH_USER(sh_user);
                ViewBag.DeleteMessage = string.Format("{0} was deleted", sh_user.UserName);
                TempData["Msg"] = "Data has been deleted succeessfully";
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteUserRole(int userId, int userRoleId)
        {

            SH_USERROLE sh_userrole = userRoleRepository.SH_USERROLE.FirstOrDefault(p => p.ID == userRoleId);

            if (sh_userrole != null)
            {
                string roleName = sh_userrole.SH_ROLE.RoleName;

                userRoleRepository.DeleteSH_USERROLE(sh_userrole);
                TempData["Message"] = "";
                TempData["DeleteMessage"] = string.Format("{0} was deleted", roleName);

            }

            return RedirectToAction("Edit2", new { id = userId });
        }

        public PartialViewResult ViewRoleAccess(int roleId)
        {
            var roleAccessModel = new RoleAccessModel();

            roleAccessModel.SH_ROLE = (from p in roleRepository.SH_ROLE
                                       where p.ID.Equals(roleId)
                                       orderby p.RoleName
                                       select p).FirstOrDefault();
            if (roleId != null)
            {
                roleAccessModel.SH_ROLEACCESSS = roleAccessModel.SH_ROLE.SH_ROLEACCESS;
            }

            roleAccessModel.SH_APPS = from m in appRepository.SH_APP
                                      orderby m.FunctionName
                                      select m;
            roleAccessModel.DynamicMenus = from m in dynamicMenuRepository.DynamicMenu
                                           where m.MenuLevel.Equals(2)
                                           orderby m.MenuName
                                           select m;
            return PartialView("ViewRoleAccess", roleAccessModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult LoadUserRole0(int? userId, string userName)
        {
            Session["UserID"] = userId;
            Session["UserName"] = userName;
            var model = from p in userRoleRepository.SH_USERROLE
                        where p.SH_USERID.Equals(userId)
                        orderby p.SH_USER.UserName
                        select p;
            return PartialView("LoadUserRole0", model.ToList());
        }

        private void PopulateRoleDropDownList(object selectedRole = null)
        {
            var roleQuery = from d in roleRepository.SH_ROLE
                            orderby d.RoleName
                            select d;
            ViewBag.SH_ROLEID2 = new SelectList(roleQuery, "ID", "RoleName", selectedRole);
        }

        public PartialViewResult LoadRoleAccess(int? roleId, string roleName)
        {
            var model = from p in roleAccessRepository.SH_ROLEACCESS
                        where p.SH_ROLEID.Equals(roleId)
                        orderby p.SH_ROLE.RoleName
                        select p;
            return PartialView("LoadRoleAccess", model.ToList());
        }

        public PartialViewResult LoadUserRole(int? id, string userName)
        {
            Session["UserID"] = id;
            Session["UserName"] = userName;
            var model = from p in userRoleRepository.SH_USERROLE
                        where p.SH_USERID.Equals(id)
                        orderby p.SH_ROLE.RoleName
                        select p;
            return PartialView(model.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadUserRole(List<SH_USERROLE> sh_userrole)
        {
            if (sh_userrole != null)
            {
                foreach (var item in sh_userrole)
                {
                    SH_USERROLE ur = (from p in userRoleRepository.SH_USERROLE
                                        where p.ID.Equals(item.ID)
                                        select p).FirstOrDefault();
                    ur.Assign = item.Assign;

                    userRoleRepository.SaveSH_USERROLE(ur);
                    //Console.Out.WriteLine("AllowRead = " + item.AllowRead);
                }
            }

            TempData["Message"] = string.Format("{0} was updated", Session["UserName"]);
            TempData["DeleteMessage"] = "";
            return RedirectToAction("Edit2", new { id = Session["UserID"] });
        }

        private void PopulateLanguageDropDownList(object selectedLng = null)
        {
            var languageQuery = from d in languageRepository.Language
                            orderby d.LanguageCode
                            select d;
            ViewBag.Language = new SelectList(languageQuery, "ID", "LanguageCode", selectedLng);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult LoadUserPreference0([Bind(Include = "ID,SH_USERID,Language,Theme,RowPerPage,DateFormat")] UserPreference userPreference)
        public PartialViewResult LoadUserPreference0(Int64? userid, string userName)
        {
                UserPreference userPreference = new UserPreference();
                int iUserId = Int32.Parse(Session["SH_USERID"].ToString());
                userPreference.SH_USERID = iUserId;
                userPreference.Language = System.Web.Configuration.WebConfigurationManager.AppSettings["Language"];
                userPreference.Theme = System.Web.Configuration.WebConfigurationManager.AppSettings["Theme"];
                userPreference.RowPerPage = System.Web.Configuration.WebConfigurationManager.AppSettings["RowPerPage"];
                userPreference.DateFormat = System.Web.Configuration.WebConfigurationManager.AppSettings["DateFormat"];
                userPreferenceRepository.SaveUserPreference(userPreference);
                Session["SH_USERID"] = userPreference.SH_USERID;
                //return View("Edit");
                //return RedirectToAction("Edit2", new { id = userPreference.SH_USERID });
                return PartialView(userPreference);
        }

        public PartialViewResult LoadUserPreference(Int64?userid, string userName)
        {
            Session["SH_USERID"] = userid;
            Session["UserName"] = userName;
            //PopulateLanguageDropDownList();
            //improve performance
            UserPreference userPreference = userPreferenceRepository.GetUserPreferenceBySH_USERID((long)userid).FirstOrDefault();
            return PartialView(userPreference);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadUserPreference([Bind(Include = "ID,SH_USERID,Language,Theme,RowPerPage,DateFormat")] UserPreference userPreference)
        {
            if (ModelState.IsValid)
            {
                userPreference.SH_USERID = Int32.Parse(Session["SH_USERID"].ToString());
                userPreferenceRepository.SaveUserPreference(userPreference);
                Session["SH_USERID"] = userPreference.SH_USERID;
                //return View("Edit");
                return RedirectToAction("Edit2", new { id = userPreference.SH_USERID });
            }
            return PartialView(userPreference);
        }

        public ActionResult CreateUserAndUserRole(string userName)
        {
            PopulateRoleDropDownList();
            return PartialView(new SH_USERROLE());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUserAndUserRole([Bind(Include = "ID,SH_USERID,SH_ROLEID,Assign,UpdateDate")] SH_USERROLE sh_userrole)
        {
            if (ModelState.IsValid)
            {
                int iSH_ROLEID2 = Int32.Parse(Request.Form["SH_ROLEID2"].ToString());
                sh_userrole.SH_USERID = Int32.Parse(Session["UserID"].ToString());
                sh_userrole.SH_ROLEID = iSH_ROLEID2;
                sh_userrole.UpdateDate = DateTime.Now;
                sh_userrole.CreateDate = DateTime.Now;
                userRoleRepository.SaveSH_USERROLE(sh_userrole);
                //ViewBag.Message = string.Format("{0} was updated", sh_role.RoleName);
                //return RedirectToAction("Index");
                TempData["Message"] = "Record was added";
                TempData["DeleteMessage"] = "";
                return RedirectToAction("Edit2", new { id = sh_userrole.SH_USERID });
            }
            return View(sh_userrole);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        //db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        public ReportResult Export2Excel()
        {
            //https://github.com/matthidinger/DoddleReport/wiki/Building-your-first-report
            var model = (from m in userRepository.SH_USER
                         select new { m.UserID, m.UserName, m.UserEmail, m.CreateDate }).ToList();
            var report = new Report(model.ToReportSource());
            report.TextFields.Title = "Prototype: ERP";
            report.TextFields.SubTitle = "User Report";
            report.TextFields.Footer = "Copyright " + DateTime.Now.Year + " &copy; iTech Workwide Prototype";
            //report.TextFields.Header = string.Format(@"
            //    Report Generated: {0}
            //    Total Products: {1}
            //    Total Orders: {2}
            //    Total Sales: {3:c}", DateTime.Now, totalProducts, totalOrders, totalProducts * totalOrders);


            // Render hints allow you to pass additional hints to the reports as they are being rendered
            report.RenderHints.BooleanCheckboxes = true;


            // Customize the data fields
            //report.DataFields["Id"].Hidden = true;
            //report.DataFields["Price"].DataFormatString = "{0:c}";
            //report.DataFields["LastPurchase"].DataFormatString = "{0:d}";

            //var writer = new DoddleReport.OpenXml.ExcelReportWriter();
            //writer.WriteReport(report, HttpContext.Response.OutputStream);

            //return View();
            DoddleReport.OpenXml.ExcelReportWriter writer = new DoddleReport.OpenXml.ExcelReportWriter();

            var report2 = new Report(model.ToReportSource(), writer);
            //var child = new Report(childquery.ToReportSource(), writer);
            //parent.AppendReport(child);
            report2.TextFields.Title = "Prototype: ERP";
            report2.TextFields.SubTitle = "User Repository";
            report2.TextFields.Footer = "Copyright " + DateTime.Now.Year + " &copy; iTech Workwide Prototype";
            return new ReportResult(report2, writer) { FileName = "UserRepository.xlsx" };
            //return View();
        }

        public ReportResult Export2Excel1()
        {
            var model = (from m in userRepository.SH_USER
                         select new { m.UserID, m.UserName, m.UserEmail, m.CreateDate }).ToList();
            DoddleReport.OpenXml.ExcelReportWriter writer = new DoddleReport.OpenXml.ExcelReportWriter();
            var report2 = new Report(model.ToReportSource(), writer);
            report2.TextFields.Title = "Prototype: ERP";
            report2.TextFields.SubTitle = "User Repository";
            report2.TextFields.Footer = "Copyright " + DateTime.Now.Year + " &copy; iTech Workwide Prototype";
            return new ReportResult(report2, writer) { FileName = "UserRepository.xlsx" };
        }
    }
}
