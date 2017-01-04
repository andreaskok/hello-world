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
using System.Collections.Generic;

namespace ERPCore.Controllers
{
    public class JobScheduleController : BaseController
    {
        //private EFDbContext db = new EFDbContext();
        private IJobScheduleRepository jobscheduleRepository;

        public JobScheduleController(IJobScheduleRepository jobscheduleRepo, IErrorLogRepository errorLogRepo) : base(errorLogRepo)
        {
            this.jobscheduleRepository = jobscheduleRepo;
        }

        // GET: JobSchedule
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

            var model = (from m in jobscheduleRepository.JobSchedule
                         select m);
            
            Int64? iRecCnt = CommonUtility.Null2LongZero(jobscheduleRepository.GetMaxID());

            if (!String.IsNullOrEmpty(searchValue))
            {
                model = (from m in jobscheduleRepository.JobScheduleWildSearch("JobCode", searchValue)
                         select m);
                iRecCnt = model.Count();
            }
            else
            {
                //model = model.Skip(((int)page - 1) * pageSize)
                //       .Take(pageSize);
                //improve performance
                model = jobscheduleRepository.GetJobSchedulePaging(((((long)page - 1) * pageSize) + 1), (long)page * pageSize);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    if (sortBy.Equals("JobCode"))
                    {
                        model = model.OrderByDescending(s => s.JobCode);
                    }
                    else if (sortBy.Equals("JobName"))
                    {
                        model = model.OrderByDescending(s => s.JobName);
                    }
                    break;
                default:
                    if (sortBy.Equals("JobCode"))
                    {
                        model = model.OrderBy(s => s.JobCode);
                    }
                    else if (sortBy.Equals("JobName"))
                    {
                        model = model.OrderBy(s => s.JobName);
                    }
                    break;

            }
            //improve performance
            var model3 = new StaticPagedList<JobSchedule>(model, (int)page, pageSize, (int)iRecCnt);
            return View(model3);
        }

        // GET: JobSchedule/Details/5
        public ActionResult Details(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from p in jobscheduleRepository.JobSchedule
                         where p.ID.Equals(id)
                         orderby p.JobCode
                         select p).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView(model);
        }

        // GET: JobSchedule/Create
        public ActionResult Create()
        {
            TempData["Message"] = "";
            TempData["DeteleMessage"] = "";
            return View();
        }

        // POST: JobSchedule/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,JobCode,JobName,JobType,Description,ExecuteFrequence,ExecuteDateStart,ExecuteDateEnd,LastRunDateTime,NextRunDateTime,ExecuteFlag,EnabledFlag")] JobSchedule jobSchedule)
        {
            if (ModelState.IsValid)
            {
                //db.JobSchedule.Add(jobSchedule);
                //db.SaveChanges();
                jobscheduleRepository.SaveJobSchedule(jobSchedule);
                TempData["Message"] = string.Format("{0} was added in system !", jobSchedule.JobCode);
                return RedirectToAction("Index");
            }
            return View(jobSchedule);
        }

        // GET: JobSchedule/Edit/5
        public ActionResult Edit(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //JobSchedule jobSchedule = db.JobSchedule.Find(id);
            JobSchedule jobSchedule = jobscheduleRepository.GetJobScheduleByID((long)id).FirstOrDefault();
            if (jobSchedule == null)
            {
                return HttpNotFound();
            }
            return PartialView(jobSchedule);
        }

        // POST: JobSchedule/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,JobCode,JobName,JobType,Description,ExecuteFrequence,ExecuteDateStart,ExecuteDateEnd,LastRunDateTime,NextRunDateTime,ExecuteFlag,EnabledFlag")] JobSchedule jobSchedule)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(jobSchedule).State = EntityState.Modified;
                //db.SaveChanges();
                jobscheduleRepository.SaveJobSchedule(jobSchedule);
                ViewBag.Message = string.Format("{0} was updated in system.", jobSchedule.JobCode);
                return RedirectToAction("Index");
            }
            return PartialView(jobSchedule);
        }

        // GET: JobSchedule/Delete/5
        public ActionResult Delete(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //JobSchedule jobSchedule = db.JobSchedule.Find(id);
            JobSchedule jobSchedule = jobscheduleRepository.GetJobScheduleByID((long)id).FirstOrDefault();
            if (jobSchedule == null)
            {
                return HttpNotFound();
            }
            return View(jobSchedule);
        }

        // POST: JobSchedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            //JobSchedule jobSchedule = db.JobSchedule.Find(id);
            //db.JobSchedule.Remove(jobSchedule);
            //db.SaveChanges();
            JobSchedule jobSchedule = jobscheduleRepository.GetJobScheduleByID((long)id).FirstOrDefault();
            if (jobSchedule != null)
            {
                jobscheduleRepository.DeleteJobSchedule(jobSchedule);
                TempData["DeleteMessage"] = string.Format("{0} was deleted in system. ", jobSchedule.JobCode);
            }
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

    }
}
