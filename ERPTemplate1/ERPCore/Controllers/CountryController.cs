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
    public class CountryController : BaseController 
    {
        //private EFDbContext db = new EFDbContext();
        private ICountryRepository countryRepository;
        private ICountryStateRepository countryStateRepository;
        private ICurrencyRepository currencyRepository;

        public CountryController(ICountryRepository countryRepo, ICountryStateRepository countryStateRepo, ICurrencyRepository currencyRepo, IErrorLogRepository errorLogRepo) : base(errorLogRepo)
        {
            this.countryRepository = countryRepo;
            this.countryStateRepository = countryStateRepo;
            this.currencyRepository = currencyRepo;
        }

        // GET: Country
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

            var model = (from m in countryRepository.Country
                         select m);

            Int64 iRecCnt = countryRepository.GetMaxID();

            if (!String.IsNullOrEmpty(searchValue))
            {
                model = (from m in countryRepository.CountryWildSearch("CountryCode", searchValue)
                         select m);
                iRecCnt = model.Count();
            }
            else
            {
                //model = model.Skip(((int)page - 1) * pageSize)
                //       .Take(pageSize);
                //improve performance
                model = countryRepository.GetCountryPaging(((((long)page - 1) * pageSize) + 1), (long)page * pageSize);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    if (sortBy.Equals("CountryCode"))
                    {
                        model = model.OrderByDescending(s => s.CountryCode);
                    }
                    else if (sortBy.Equals("Description"))
                    {
                        model = model.OrderByDescending(s => s.Description);
                    }
                    break;
                default:
                    if (sortBy.Equals("CountryCode"))
                    {
                        model = model.OrderBy(s => s.CountryCode);
                    }
                    else if (sortBy.Equals("Description"))
                    {
                        model = model.OrderBy(s => s.Description);
                    }
                    break;

            }
            //improve performance
            var model3 = new StaticPagedList<Country>(model, (int)page, pageSize, (int)iRecCnt);
            return View(model3);
        }

        // GET: Country/Details/5
        public ActionResult Details(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Country country = db.Country.Find(id);
            var model = (from p in countryRepository.Country
                         where p.ID.Equals(id)
                         orderby p.CountryCode
                         select p).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView(model);
        }

        private void PopulateCurrencyDropDownList(object selectedAcc = null)
        {
            var currencyQuery = from d in currencyRepository.Currency
                                orderby d.CurrencyCode
                                select d;
            ViewBag.Currency = new SelectList(currencyQuery, "ID", "CurrencyCode", selectedAcc);
        }

        // GET: Country/Create
        public ActionResult Create()
        {
            TempData["Message"] = "";
            TempData["DeteleMessage"] = "";
            PopulateCurrencyDropDownList();
            return View(new CountryStateModel());
        }

        // POST: Country/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CurrencyID,CountryCode,Description,CountryISO3Code")] Country country)
        {

            var countryStateModel = new CountryStateModel();
            
            if (ModelState.IsValid)
            {
                int iCurrency = Int32.Parse(Request.Form["Currency"].ToString());
                country.CurrencyID = iCurrency;
                var countryRec = from m in countryRepository.CountryWildSearch("CountryCode", country.CountryCode)
                                 select m;
                int iRecCnt = countryRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The Country Code " + country.CountryCode + " already existed in system ! ";
                }
                else
                {
                    countryRepository.SaveCountry(country);
                    Session["CountryID"] = country.ID;
                    Session["CountryCode"] = country.CountryCode;

                    countryStateModel.Country = (from p in countryRepository.Country
                                                where p.ID.Equals(country.ID)
                                                orderby p.CountryCode
                                                 select p).FirstOrDefault();
                    if (country.ID != 0)
                    {
                        countryStateModel.CountryState = from p in countryStateRepository.CountryState
                                                         where p.CountryID.Equals(country.ID)
                                                       orderby p.StateName
                                                       select p;
                    }
                    Session["CountryStateID"] = "";
                    TempData["Message"] = string.Format("{0} was added in system !", country.CountryCode);
                }
                return RedirectToAction("Index");
            }

            return View(countryStateModel);
        }

        public ActionResult CreateCountryAndCountryState()
        {

            PopulateCurrencyDropDownList();
            return PartialView(new CountryStateModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCountryAndCountryState([Bind(Include = "ID, CountryID, StateName")] CountryState countryState)
        {

            if (ModelState.IsValid)
            {
                countryState.CountryID = Int32.Parse(Session["CountryID"].ToString());
                var countryStateRec = from m in countryStateRepository.CountryStateWildSearch("StateName", countryState.StateName)
                                     select m;
                int iRecCnt = countryStateRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The State Name " + countryState.StateName + " already existed in system ! ";
                }
                else
                {
                    countryStateRepository.SaveCountryState(countryState);
                    Session["CountryStateID"] = countryState.ID;
                    Session["CountryStateCode"] = countryState.StateName;
                    //return RedirectToAction("Index");

                    TempData["Message"] = string.Format("{0} was added", countryState.StateName);
                }
                return RedirectToAction("Edit2", new { id = countryState.CountryID });
            }
            return View(countryState);
        }

        public ActionResult CreateCountryState(Int64? countryId, string countryCode)
        {
            Session["CountryID"] = countryId;
            Session["CountryCode"] = countryCode;
            //PopulateCurrencyDropDownList();
            return PartialView(new CountryState());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCountryState([Bind(Include = "ID, CountryID, StateName")] CountryState countryState)
        {
            if (ModelState.IsValid)
            {
                countryState.CountryID = Int32.Parse(Session["CountryID"].ToString());
                var countryStateRec = from m in countryStateRepository.CountryStateWildSearch("StateName", countryState.StateName)
                                     select m;
                int iRecCnt = countryStateRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The State Name " + countryState.StateName + " already existed in system ! ";
                }
                else
                {
                    countryStateRepository.SaveCountryState(countryState);
                    Session["CountryStateID"] = countryState.ID;
                    Session["CountryStateCode"] = countryState.StateName;

                    TempData["Message"] = string.Format("{0} was added", countryState.StateName);
                }
                return RedirectToAction("Edit2", new { id = countryState.CountryID });
            }
            return View(countryState);
        }

        public ActionResult EditCountryState(Int64? countryId, Int64? countryStateId, string countryCode, string countryStateCode)
        {
            Session["CountryCode"] = countryCode;
            Session["CountryID"] = countryId;
            Session["CountryStateID"] = countryStateId;
            Session["CountryStateCode"] = countryStateCode;

            //improve performance
            CountryState countryState = countryStateRepository.GetCountryStateByID((long)countryStateId).SingleOrDefault();
            return PartialView(countryState);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCountryState([Bind(Include = "ID, CountryID, StateName")] CountryState countryState)
        {
            if (ModelState.IsValid)
            {
                countryState.CountryID = Int32.Parse(Session["CountryID"].ToString());
                countryStateRepository.SaveCountryState(countryState);
                Session["CountryID"] = countryState.CountryID;
                //return View("Edit");
                return RedirectToAction("Edit2", new { id = countryState.CountryID });
            }
            return PartialView(countryState);
        }

        // GET: Country/Edit/5
        public ActionResult Edit(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var countryStateModel = new CountryStateModel();

            countryStateModel.Country = countryRepository.GetCountryByID((long)id).FirstOrDefault();

            if (id != null)
            {
                //improve performance
                countryStateModel.CountryState = countryStateRepository.GetCountryState("CountryID", (long)id);
            }

            if (countryStateModel == null)
            {
                return HttpNotFound();
            }
            PopulateCurrencyDropDownList(countryStateModel.Country.CurrencyID);
            return PartialView(countryStateModel);

        }

        // POST: Country/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CurrencyID,CountryCode,Description,CountryISO3Code")] Country country)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(country).State = EntityState.Modified;
                //db.SaveChanges();
                int iCurrency = Int32.Parse(Request.Form["Currency"].ToString());
                country.CurrencyID = iCurrency;
                Currency currency = currencyRepository.Currency.FirstOrDefault(p => p.ID == iCurrency);
                PopulateCurrencyDropDownList(country.CurrencyID);
                countryRepository.SaveCountry(country);
                ViewBag.Message = string.Format("{0} was updated in system.", country.CountryCode);

                return RedirectToAction("Index");
            }
            return PartialView(country);
        }

        public ActionResult Edit2(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TempData["Message"] = "";
            var countryStateModel = new CountryStateModel();

            //remove the orderBy to improve performance
            countryStateModel.Country = countryRepository.GetCountryByID((long)id).FirstOrDefault();
            if (id != null)
            {
                //improve performance
                countryStateModel.CountryState = countryStateRepository.GetCountryState("CountryID", (long)id);
            }

            if (countryStateModel == null)
            {
                return HttpNotFound();
            }
            return View(countryStateModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2([Bind(Include = "ID,CurrencyID,CountryCode,Description,CountryISO3Code")] Country country)
        {
            if (ModelState.IsValid)
            {
                countryRepository.SaveCountry(country);
                ViewBag.Message = string.Format("{0} was updated in the system.", country.CountryCode);
                return RedirectToAction("Index");
            }
            return View(country);
        }

        // GET: Country/Delete/5
        public ActionResult Delete(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var countryStateModel = new CountryStateModel();

            countryStateModel.Country = countryRepository.GetCountryByID((long)id).FirstOrDefault();
            if (id != null)
            {
                //improve performance
                countryStateModel.CountryState = countryStateRepository.GetCountryState("CountryID",(long)id);
            }
            Country country = countryRepository.GetCountryByID((long)id).SingleOrDefault();

            if (countryStateModel.CountryState != null)
            {
                foreach (var item in countryStateModel.CountryState)
                {
                    CountryState countryState = countryStateRepository.CountryState.FirstOrDefault(p => p.ID == item.ID);
                    countryStateRepository.DeleteCountryState(countryState);
                }
            }

            if (country != null)
            {
                countryRepository.DeleteCountry(country);
                TempData["DeleteMessage"] = string.Format("{0} was deleted in system.", country.CountryCode);
            }

            return RedirectToAction("Index");

        }

        public ActionResult DeleteCountryState(Int64? countryId, Int64? countryStateId)
        {

            CountryState countryState = countryStateRepository.CountryState.FirstOrDefault(p => p.ID == countryStateId);

            if (countryState != null)
            {
                countryStateRepository.DeleteCountryState(countryState);
                TempData["Message"] = "";
                TempData["DeleteMessage"] = string.Format("{0} was deleted in the system.", countryState.StateName);
            }

            return RedirectToAction("Edit2", new { id = countryId });
        }
        
        // POST: Country/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(long id)
        //{
        //    Country country = db.Country.Find(id);
        //    db.Country.Remove(country);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
