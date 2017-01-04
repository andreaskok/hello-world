using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ERPDomain.Entities;
using ERPDomain.Abstract;
using System;
using PagedList;
using ERPDomain.Models;
using ERPDomain.Helpers;


namespace PluginGL.Controllers
{
    public class AccountGroupController : BaseController
    {
        //private EFDbContext db = new EFDbContext();
        private IAccountGroupRepository accountgroupRepository;

        public AccountGroupController(IAccountGroupRepository accountgroupRepo)
        {
            this.accountgroupRepository = accountgroupRepo;
        }

        // GET: AccountGroup
        public ActionResult Index(string searchValue, string sortOrder, string currentFilter, int? page, string sortBy)
        {
            //TempData["Message"] = "";
            sortBy = CommonUtility.Null2Empty(sortBy);
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

            int pageSize = Int32.Parse(CommonUtility.Empty2Zero(System.Web.Configuration.WebConfigurationManager.AppSettings["RowPerPage"]));
            int pageNumber = (page ?? 1);
            if (page == null)
            {
                page = 1;
            }

            var model = (from m in accountgroupRepository.AccountGroup
                         select m);

            Int64? iRecCnt = CommonUtility.Null2LongZero(accountgroupRepository.GetMaxID());

            if (!String.IsNullOrEmpty(searchValue))
            {
                model = (from m in accountgroupRepository.AccountGroupWildSearch("AccGrpCode", searchValue)
                         select m);
                iRecCnt = model.Count();
            }
            else
            {
                //model = model.Skip(((int)page - 1) * pageSize)
                //       .Take(pageSize);
                //improve performance
                model = accountgroupRepository.GetAccountGroupPaging(((((long)page - 1) * pageSize) + 1), (long)page * pageSize);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    if (sortBy.Equals("AccGrpCode"))
                    {
                        model = model.OrderByDescending(s => s.AccGrpCode);
                    }
                    else if (sortBy.Equals("Description"))
                    {
                        model = model.OrderByDescending(s => s.Description);
                    }
                    break;
                default:
                    if (sortBy.Equals("AccGrpCode"))
                    {
                        model = model.OrderBy(s => s.AccGrpCode);
                    }
                    else if (sortBy.Equals("Description"))
                    {
                        model = model.OrderBy(s => s.Description);
                    }
                    break;

            }
            //improve performance
            var model3 = new StaticPagedList<AccountGroup>(model, (int)page, pageSize, (int)iRecCnt);
            return View(model3);
        }

        // GET: AccountGroup/Details/5
        public ActionResult Details(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //AccountGroup accountGroup = db.AccountGroup.Find(id);
            //if (accountGroup == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(accountGroup);
            var model = (from p in accountgroupRepository.AccountGroup
                         where p.ID.Equals(id)
                         orderby p.AccGrpCode
                         select p).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView(model);
        }

        // GET: AccountGroup/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountGroup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AccGrpCode,Description,Status,CreateDate,UpdateDate,UpdateID")] AccountGroup accountgroup)
        {
            if (ModelState.IsValid)
            {
                //db.AccountGroup.Add(accountgroup);
                //db.SaveChanges();
                accountgroup.CreateDate = DateTime.Now;
                accountgroup.UpdateDate = DateTime.Now;
                accountgroup.UpdateID = Session["UserID"].ToString();
                accountgroup.Status = "Active";
                var accountgroupRec = from m in accountgroupRepository.AccountGroupWildSearch("AccGrpCode", accountgroup.AccGrpCode)
                                 select m;
                int iRecCnt = accountgroupRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The Account Group Code " + accountgroup.AccGrpCode + " already existed in system ! ";
                }
                else
                { 
                    accountgroupRepository.SaveAccountGroup(accountgroup);
                    TempData["Message"] = string.Format("{0} was added in system !", accountgroup.AccGrpCode);
                }

                return RedirectToAction("Index");
            }

            return PartialView(accountgroup);
        }

        // GET: AccountGroup/Edit/5
        public ActionResult Edit(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //AccountGroup accountGroup = db.AccountGroup.Find(id);
            //if (accountGroup == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(accountGroup);
            var model = (from p in accountgroupRepository.AccountGroup
                         where p.ID.Equals(id)
                         orderby p.AccGrpCode
                         select p).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView(model);
        }

        // POST: AccountGroup/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //ID,AccGrpCode,Description,Status,CreateDate,UpdateDate,UpdateID,AccClsCode,CreateID
        public ActionResult Edit([Bind(Include = "ID,AccGrpCode,Description,Status,UpdateDate,UpdateID")] AccountGroup accountgroup)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(accountGroup).State = EntityState.Modified;
                //db.SaveChanges();
                accountgroup.UpdateDate = DateTime.Now;
                accountgroup.CreateDate = DateTime.Now;
                accountgroup.UpdateID = Session["UserID"].ToString();
                accountgroup.Status = "Active";
                accountgroupRepository.SaveAccountGroup(accountgroup);
                ViewBag.Message = string.Format("{0} was updated in system !", accountgroup.AccGrpCode);

                return RedirectToAction("Index");
            }
            return PartialView(accountgroup);
        }

        // GET: AccountGroup/Delete/5
        public ActionResult Delete(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //AccountGroup accountGroup = db.AccountGroup.Find(id);
            //if (accountGroup == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(accountGroup);
            var model = (from p in accountgroupRepository.AccountGroup
                         where p.ID.Equals(id)
                         orderby p.ID
                         select p).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: AccountGroup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Int64 id)
        {
            //AccountGroup accountGroup = db.AccountGroup.Find(id);
            //db.AccountGroup.Remove(accountGroup);
            //db.SaveChanges();
            AccountGroup accountgroup = accountgroupRepository.AccountGroup.FirstOrDefault(p => p.ID == id);
            if (accountgroup != null)
            {
                accountgroupRepository.DeleteAccountGroup(accountgroup);
                TempData["DeleteMessage"] = string.Format("{0} was deleted", accountgroup.AccGrpCode);
            }
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //       db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
