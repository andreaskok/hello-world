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
    public class CurrencyController : BaseController
    {
        //private EFDbContext db = new EFDbContext();
        private ICurrencyRepository currencyRepository;

        public CurrencyController(ICurrencyRepository currencyRepo, IErrorLogRepository errorLogRepo) : base(errorLogRepo)
        {
            this.currencyRepository = currencyRepo;
        }

        // GET: Currency
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

            var model = (from m in currencyRepository.Currency
                         select m);

            Int64 iRecCnt = currencyRepository.GetMaxID();

            if (!String.IsNullOrEmpty(searchValue))
            {
                model = (from m in currencyRepository.CurrencyWildSearch("CurrencyCode", searchValue)
                         select m);
                iRecCnt = model.Count();
            }
            else
            {
                //model = model.Skip(((int)page - 1) * pageSize)
                //       .Take(pageSize);
                //improve performance
                model = currencyRepository.GetCurrencyPaging(((((long)page - 1) * pageSize) + 1), (long)page * pageSize);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    if (sortBy.Equals("CurrencyCode"))
                    {
                        model = model.OrderByDescending(s => s.CurrencyCode);
                    }
                    else if (sortBy.Equals("Description"))
                    {
                        model = model.OrderByDescending(s => s.Description);
                    }
                    break;
                default:
                    if (sortBy.Equals("CurrencyCode"))
                    {
                        model = model.OrderBy(s => s.CurrencyCode);
                    }
                    else if (sortBy.Equals("Description"))
                    {
                        model = model.OrderBy(s => s.Description);
                    }
                    break;

            }
            //improve performance
            var model3 = new StaticPagedList<Currency>(model, (int)page, pageSize, (int)iRecCnt);
            return View(model3);
        }

        // GET: Currency/Details/5
        public ActionResult Details(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from p in currencyRepository.Currency
                         where p.ID.Equals(id)
                         orderby p.CurrencyCode
                         select p).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView(model);
        }

        // GET: Currency/Create
        public ActionResult Create()
        {
            TempData["Message"] = "";
            TempData["DeteleMessage"] = "";
            return View();
        }

        // POST: Currency/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CurrencyCode,Description")] Currency currency)
        {
            if (ModelState.IsValid)
            {
                //db.Currency.Add(currency);
                //db.SaveChanges();
                currencyRepository.SaveCurrency(currency);
                TempData["Message"] = string.Format("{0} was added in system !", currency.CurrencyCode);
                return RedirectToAction("Index");
            }

            return View(currency);
        }

        // GET: Currency/Edit/5
        public ActionResult Edit(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Currency currency = db.Currency.Find(id);
            Currency currency = currencyRepository.GetCurrencyByID((long)id).FirstOrDefault();
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        // POST: Currency/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CurrencyCode,Description")] Currency currency)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(currency).State = EntityState.Modified;
                //db.SaveChanges();
                currencyRepository.SaveCurrency(currency);
                ViewBag.Message = string.Format("{0} was updated in system.", currency.CurrencyCode);

                return RedirectToAction("Index");
            }
            return View(currency);
        }

        // GET: Currency/Delete/5
        public ActionResult Delete(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Currency currency = db.Currency.Find(id);
            Currency currency = currencyRepository.GetCurrencyByID((long)id).FirstOrDefault();
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        // POST: Currency/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Int64 id)
        {
            //Currency currency = db.Currency.Find(id);
            //db.Currency.Remove(currency);
            //db.SaveChanges();
            Currency currency = currencyRepository.GetCurrencyByID((long)id).FirstOrDefault();
            if (currency != null)
            {
                currencyRepository.DeleteCurrency(currency);
                TempData["DeleteMessage"] = string.Format("{0} was deleted in system. ", currency.CurrencyCode);
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
