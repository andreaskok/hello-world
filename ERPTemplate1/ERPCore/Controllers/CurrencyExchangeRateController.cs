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
    public class CurrencyExchangeRateController : BaseController
    {
        //private EFDbContext db = new EFDbContext();
        private ICurrencyRepository currencyRepository;
        private ICurrencyExchangeRateRepository currencyexchangerateRepository;

        public CurrencyExchangeRateController(ICurrencyRepository currencyRepo, ICurrencyExchangeRateRepository currencyexchangerateRepo, IErrorLogRepository errorLogRepo) : base(errorLogRepo)
        {
            this.currencyRepository = currencyRepo;
            this.currencyexchangerateRepository = currencyexchangerateRepo;
        }

        // GET: CurrencyExchangeRate
        public ActionResult Index(string searchValue, string sortOrder, string currentFilter, int? page)
        {
            //TempData["Message"] = "";
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

            var model = (from m in currencyexchangerateRepository.CurrencyExchangeRate
                         select m);

            Int64 iRecCnt = currencyexchangerateRepository.GetMaxID();

            if (!String.IsNullOrEmpty(searchValue))
            {
                model = (from m in currencyexchangerateRepository.CurrencyExchangeRateWildSearch("ForeignCurrencyCode", searchValue)
                         select m);
                iRecCnt = model.Count();
            }
            else
            {
                //model = model.Skip(((int)page - 1) * pageSize)
                //       .Take(pageSize);
                //improve performance
                model = currencyexchangerateRepository.GetCurrencyExchangeRatePaging(((((long)page - 1) * pageSize) + 1), (long)page * pageSize);
            }

            switch (sortOrder)
            {
                default:
                    //model = model.OrderBy(s => s.ID);
                    break;
            }
            //improve performance
            var model3 = new StaticPagedList<CurrencyExchangeRate>(model, (int)page, pageSize, (int)iRecCnt);
            return View(model3);
        }

        // GET: CurrencyExchangeRate/Details/5
        public ActionResult Details(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from p in currencyexchangerateRepository.CurrencyExchangeRate
                         where p.ID.Equals(id)
                         orderby p.ForeignCurrencyCode
                         select p).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView(model);
        }

        private void PopulateBaseCurrencyDropDownList(object selectedAcc = null)
        {
            var basecurrencyQuery = from d in currencyRepository.Currency
                               orderby d.CurrencyCode
                               select d;
            ViewBag.BaseCurrencyCode = new SelectList(basecurrencyQuery, "ID", "CurrencyCode", selectedAcc);
        }
        private void PopulateForeignCurrencyDropDownList(object selectedAcc = null)
        {
            var foreigncurrencyQuery = from d in currencyRepository.Currency
                                orderby d.CurrencyCode
                                select d;
            ViewBag.ForeignCurrencyCode = new SelectList(foreigncurrencyQuery, "ID", "CurrencyCode", selectedAcc);
        }

        // GET: CurrencyExchangeRate/Create
        public ActionResult Create()
        {
            TempData["Message"] = "";
            TempData["DeteleMessage"] = "";
            PopulateBaseCurrencyDropDownList();
            PopulateForeignCurrencyDropDownList();
            return View();
        }

        // POST: CurrencyExchangeRate/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ForeignCurrencyCode,BaseCurrencyCode,ExchangeRate,EffectiveDate,UpdateID")] CurrencyExchangeRate currencyExchangeRate)
        {
            if (ModelState.IsValid)
            {
                //db.CurrencyExchangeRate.Add(currencyExchangeRate);
                //db.SaveChanges();
                int iforeigncurrency = Int32.Parse(Request.Form["ForeignCurrencyCode"].ToString());
                Currency foreigncurrency = currencyRepository.Currency.FirstOrDefault(p => p.ID == iforeigncurrency);
                currencyExchangeRate.ForeignCurrencyCode = foreigncurrency.CurrencyCode.Trim();
                int iBaseCurrency = Int32.Parse(Request.Form["BaseCurrencyCode"].ToString());
                Currency basecurrency = currencyRepository.Currency.FirstOrDefault(p => p.ID == iBaseCurrency);
                currencyExchangeRate.BaseCurrencyCode = basecurrency.CurrencyCode.Trim();
                currencyExchangeRate.UpdateID = Session["UserID"].ToString();
                currencyexchangerateRepository.SaveCurrencyExchangeRate(currencyExchangeRate);
                TempData["Message"] = string.Format("{0} was added in system !", currencyExchangeRate.ForeignCurrencyCode);
                return RedirectToAction("Index");
            }

            return View(currencyExchangeRate);
        }

        // GET: CurrencyExchangeRate/Edit/5
        public ActionResult Edit(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //CurrencyExchangeRate currencyExchangeRate = db.CurrencyExchangeRate.Find(id);
            //if (currencyExchangeRate == null)
            //{
            //    return HttpNotFound();
            //}
            CurrencyExchangeRate currencyExchangeRate = currencyexchangerateRepository.GetCurrencyExchangeRateByID((long)id).FirstOrDefault();
            
            if (currencyExchangeRate == null)
            {
                return HttpNotFound();
            }
            //PopulateCustomerDropDownList(organizationbranchModel.Organization.CustomerID);
            return View(currencyExchangeRate);
        }

        // POST: CurrencyExchangeRate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ForeignCurrencyCode,BaseCurrencyCode,ExchangeRate,EffectiveDate,UpdateID")] CurrencyExchangeRate currencyExchangeRate)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(currencyExchangeRate).State = EntityState.Modified;
                //db.SaveChanges();
                currencyExchangeRate.UpdateID = Session["UserID"].ToString();
                PopulateBaseCurrencyDropDownList(Session["BaseCurrencyCode"].ToString());
                PopulateForeignCurrencyDropDownList(Session["ForeignCurrencyCode"].ToString());
                currencyexchangerateRepository.SaveCurrencyExchangeRate(currencyExchangeRate);
                ViewBag.Message = string.Format("{0} was updated in system.", currencyExchangeRate.ForeignCurrencyCode);

                return RedirectToAction("Index");
            }
            return View(currencyExchangeRate);
        }

        // GET: CurrencyExchangeRate/Delete/5
        public ActionResult Delete(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CurrencyExchangeRate currencyExchangeRate = currencyexchangerateRepository.GetCurrencyExchangeRateByID((long)id).FirstOrDefault();
            if (currencyExchangeRate == null)
            {
                return HttpNotFound();
            }
            return View(currencyExchangeRate);
        }

        // POST: CurrencyExchangeRate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Int64 id)
        {
            //CurrencyExchangeRate currencyExchangeRate = db.CurrencyExchangeRate.Find(id);
            CurrencyExchangeRate currencyExchangeRate = currencyexchangerateRepository.GetCurrencyExchangeRateByID((long)id).FirstOrDefault();
            if (currencyExchangeRate != null)
            {
                currencyexchangerateRepository.DeleteCurrencyExchangeRate(currencyExchangeRate);
                TempData["DeleteMessage"] = string.Format("{0} was deleted in system. ", currencyExchangeRate.ForeignCurrencyCode);
            }
            //db.CurrencyExchangeRate.Remove(currencyExchangeRate);
            //db.SaveChanges();
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

