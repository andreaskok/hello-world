using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERPDomain.Abstract;
using ERPDomain.Entities;
using PagedList;

namespace PluginAP.Controllers
{
    public class VendorController : BaseController
    {
        private IVendorRepository vendorRepository;

        public VendorController(IVendorRepository vendorRepo, IErrorLogRepository errorLogRepo) : base(errorLogRepo)
        {
            this.vendorRepository = vendorRepo;
        }

        // GET: Vendor
        public ActionResult Index(string searchValue, string sortOrder, string currentFilter, int? page)
        {
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

            var model = from m in vendorRepository.Vendor
                        select m;

            if (!String.IsNullOrEmpty(searchValue))
            {
                model = model.Where(s => s.VendorCode.ToUpper().Contains(searchValue.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.VendorCode);
                    break;
                default:
                    model = model.OrderBy(s => s.VendorCode);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        // GET: Vendor/Details/5
        public ActionResult Details(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from p in vendorRepository.Vendor
                         where p.ID.Equals(id)
                         orderby p.VendorCode
                         select p).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView(model);
        }

        // GET: Vendor/Create
        public ActionResult Create()
        {
            //ViewBag.ChartOfAccountID = new SelectList(db.ChartOfAccount, "ID", "AccCode");
            return View();
        }

        // POST: Vendor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ChartOfAccountID,BankID,CurrencyID,TaxCodeID,VendorCode,Name,ContactPerson,Address,Town,State,PostCode,CountryCode,TelNo,FaxNo,Email,FinAccCode,AccCode,CreditTerm,TermType,CreditLimit,BankCode,BankAccName,BankAccNo,Status,CreateDate,UpdateDate,UpdateID,SuppType,TermCond,MobileTel,CurrencyCode,ComRegisterNum,SuppBRN,SuppGSTNo,DateGST,TaxCode,FinSupplierCode,IsEnabledSelfBilled,RMCDApproveNo")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                //db.Vendor.Add(vendor);
                //db.SaveChanges();
                vendor.CreateDate = DateTime.Now;
                vendor.UpdateDate = DateTime.Now;
                vendor.UpdateID = Session["UserID"].ToString();
                vendor.Status = "Active";
                vendorRepository.SaveVendor(vendor);
                TempData["Message"] = string.Format("{0} was added", vendor.VendorCode);
                return RedirectToAction("Index");
            }
            //ViewBag.ChartOfAccountID = new SelectList(db.ChartOfAccount, "ID", "AccCode", vendor.ChartOfAccountID);
            return View(vendor);
        }

        // GET: Vendor/Edit/5
        public ActionResult Edit(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Vendor vendor = db.Vendor.Find(id);
            //if (vendor == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.ChartOfAccountID = new SelectList(db.ChartOfAccount, "ID", "AccCode", vendor.ChartOfAccountID);
            //return View(vendor);
            var model = (from p in vendorRepository.Vendor
                         where p.ID.Equals(id)
                         orderby p.VendorCode
                         select p).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView(model);
        }

        // POST: Vendor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ChartOfAccountID,BankID,CurrencyID,TaxCodeID,VendorCode,Name,ContactPerson,Address,Town,State,PostCode,CountryCode,TelNo,FaxNo,Email,FinAccCode,AccCode,CreditTerm,TermType,CreditLimit,BankCode,BankAccName,BankAccNo,Status,CreateDate,UpdateDate,UpdateID,SuppType,TermCond,MobileTel,CurrencyCode,ComRegisterNum,SuppBRN,SuppGSTNo,DateGST,TaxCode,FinSupplierCode,IsEnabledSelfBilled,RMCDApproveNo")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(vendor).State = EntityState.Modified;
                //db.SaveChanges();
                vendor.UpdateDate = DateTime.Now;
                vendor.CreateDate = DateTime.Now;
                vendor.UpdateID = Session["UserID"].ToString();
                vendor.Status = "Active";
                vendorRepository.SaveVendor(vendor);
                ViewBag.Message = string.Format("{0} was updated", vendor.VendorCode);
                return RedirectToAction("Index");
            }
            //ViewBag.ChartOfAccountID = new SelectList(db.ChartOfAccount, "ID", "AccCode", vendor.ChartOfAccountID);
            return View(vendor);
        }

        // GET: Vendor/Delete/5
        public ActionResult Delete(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from p in vendorRepository.Vendor
                         where p.ID.Equals(id)
                         orderby p.ID
                         select p).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
            //Vendor vendor = db.Vendor.Find(id);
            //if (vendor == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(vendor);
        }

        // POST: Vendor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Int64 id)
        {
            //Vendor vendor = db.Vendor.Find(id);
            //db.Vendor.Remove(vendor);
            //db.SaveChanges();
            Vendor vendor = vendorRepository.Vendor.FirstOrDefault(p => p.ID == id);
            if (vendor != null)
            {
                vendorRepository.DeleteVendor(vendor);
                TempData["DeleteMessage"] = string.Format("{0} was deleted", vendor.VendorCode);
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