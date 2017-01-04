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
    public class LanguageController : BaseController
    {
        //private EFDbContext db = new EFDbContext();
        private ILanguageRepository languageRepository;
     
        public LanguageController(ILanguageRepository languageRepo, IErrorLogRepository errorLogRepo) : base(errorLogRepo)
        {
            this.languageRepository = languageRepo;
        }

        // GET: Language
        public ActionResult Index(string searchValue, string sortOrder, string currentFilter, int? page, string sortBy)
        {
            sortBy = CommonUtility.Null2Empty(sortBy);
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
            if (pageSize == 0) pageSize = 20;
            int pageNumber = (page ?? 1);
            if (page == null)
            {
                page = 1;
            }

            var model = (from m in languageRepository.Language
                         select m);

            Int64 iRecCnt = languageRepository.GetMaxID();

            if (!String.IsNullOrEmpty(searchValue))
            {
                model = (from m in languageRepository.LanguageWildSearch("LanguageCode", searchValue)
                         select m);
                iRecCnt = model.Count();
            }
            else
            {
                //model = model.Skip(((int)page - 1) * pageSize)
                //       .Take(pageSize);
                //improve performance
                model = languageRepository.GetLanguagePaging(((((long)page - 1) * pageSize) + 1), (long)page * pageSize);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    if (sortBy.Equals("LanguageCode"))
                    {
                        model = model.OrderByDescending(s => s.LanguageCode);
                    }
                    else if (sortBy.Equals("Description"))
                    {
                        model = model.OrderByDescending(s => s.Description);
                    }
                    break;
                default:
                    if (sortBy.Equals("LanguageCode"))
                    {
                        model = model.OrderBy(s => s.LanguageCode);
                    }
                    else if (sortBy.Equals("Description"))
                    {
                        model = model.OrderBy(s => s.Description);
                    }
                    break;
            }
            //improve performance
            var model3 = new StaticPagedList<Language>(model, (int)page, pageSize, (int)iRecCnt);
            return View(model3);
        }

        // GET: Language/Details/5
        public ActionResult Details(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from p in languageRepository.Language
                         where p.ID.Equals(id)
                         orderby p.LanguageCode
                         select p).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView(model);
        }

        // GET: Language/Create
        public ActionResult Create()
        {
            TempData["Message"] = "";
            TempData["DeteleMessage"] = "";
            return View();
        }

        // POST: Language/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LanguageCode,Description")] Language language)
        {
            if (ModelState.IsValid)
            {
                //db.Language.Add(language);
                //db.SaveChanges();
                languageRepository.SaveLanguage(language);
                TempData["Message"] = string.Format("{0} was added in system !", language.LanguageCode);
                return RedirectToAction("Index");
            }

            return View(language);
        }

        // GET: Language/Edit/5
        public ActionResult Edit(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Language language = languageRepository.GetLanguageByID((long)id).FirstOrDefault();

            if (language == null)
            {
                return HttpNotFound();
            }
            //PopulateCustomerDropDownList(organizationbranchModel.Organization.CustomerID);
            return View(language);
        }

        // POST: Language/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LanguageCode,Description")] Language language)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(language).State = EntityState.Modified;
                //db.SaveChanges();
                languageRepository.SaveLanguage(language);
                ViewBag.Message = string.Format("{0} was updated in system.", language.LanguageCode);

                return RedirectToAction("Index");
            }
            return View(language);
        }

        // GET: Language/Delete/5
        public ActionResult Delete(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Language language = languageRepository.GetLanguageByID((long)id).FirstOrDefault();
            if (language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }

        // POST: Language/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Int64 id)
        {
            //Language language = db.Language.Find(id);
            //db.Language.Remove(language);
            //db.SaveChanges();
            Language language = languageRepository.GetLanguageByID((long)id).FirstOrDefault();
            if (language != null)
            {
                languageRepository.DeleteLanguage(language);
                TempData["DeleteMessage"] = string.Format("{0} was deleted in system. ", language.LanguageCode);
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
