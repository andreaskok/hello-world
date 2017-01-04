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

namespace PluginAR.Controllers
{
    public class CreditNoteController : BaseController
    {
        // private EFDbContextAR db = new EFDbContextAR();

        private ICreditNoteRepository creditnoteRepository;
        private ICreditNoteLineRepository creditnoteLineRepository;
        private IChartOfAccountRepository chartOfAccountRepository;

        public CreditNoteController(ICreditNoteRepository creditnoteRepo, ICreditNoteLineRepository creditnotelineRepo, IChartOfAccountRepository chartOfAccountRepo)
        {
            this.creditnoteRepository = creditnoteRepo;
            this.creditnoteLineRepository = creditnotelineRepo;
            this.chartOfAccountRepository = chartOfAccountRepo;
        }

        // GET: CreditNote
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

            var model = (from m in creditnoteRepository.CreditNote
                         select m);

            Int64? iRecCnt = CommonUtility.Null2LongZero(creditnoteRepository.GetMaxID());

            if (!String.IsNullOrEmpty(searchValue))
            {
                model = (from m in creditnoteRepository.CreditNoteWildSearch("CreditNoteCode", searchValue)
                         select m);
                iRecCnt = model.Count();
            }
            else
            {
                //model = model.Skip(((int)page - 1) * pageSize)
                //       .Take(pageSize);
                //improve performance
                model = creditnoteRepository.GetCreditNotePaging(((((long)page - 1) * pageSize) + 1), (long)page * pageSize);
            }

            switch (sortOrder)
            {
                default:
                    //model = model.OrderBy(s => s.ID);
                    break;
            }

            //return View(model.ToPagedList(pageNumber, pageSize));
            //improve performance
            var model3 = new StaticPagedList<CreditNote>(model, (int)page, pageSize, (int)iRecCnt);
            return View(model3);
            //return View(db.CreditNote.ToList());
        }

        // GET: CreditNote/Details/5
        public ActionResult Details(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from p in creditnoteRepository.CreditNote
                         where p.ID.Equals(id)
                         orderby p.CreditNoteCode
                         select p).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView(model);

        }

        private void PopulateAccountDropDownList(object selectedAcc = null)
        {
            var accQuery = from d in chartOfAccountRepository.ChartOfAccount
                           orderby d.AccCode
                           select d;
            //            var query = (from s in db.Pricelists
            //                       select s.CustId).Distinct();
            ViewBag.ChartOfAcc = new SelectList(accQuery, "ID", "AccCode", selectedAcc);
        }

        // GET: CreditNote/Create
        public ActionResult Create()
        {
            PopulateAccountDropDownList();
            Session["CreditNoteID"] = null;
            return View(new CreditNoteLineModel());
            //return View();
        }

        // POST: CreditNote/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CreditNoteCode,DocType,CustomerCode,TotalAmount,Remark,LocCode,AccMonth,AccYear,Status,CreateDate,UpdateDate,PrintDate,UpdateID,OutstandingAmount,CustRef,DeliveryRef,PostDate,CurrencyRate,CurrencyAmount,CurrencyOutAmount,CreditNoteDate,InvoiceCode")] CreditNote creditNote)
        {
            var creditnoteLineModel = new CreditNoteLineModel();

            if (ModelState.IsValid)
            {
                creditNote.CreateDate = DateTime.Now;
                creditNote.UpdateDate = DateTime.Now;
                creditNote.PrintDate = DateTime.Now;
                creditNote.PostDate = DateTime.Now;
                creditNote.UpdateID = Session["UserID"].ToString();
                creditNote.Status = "Active";
                var creditnoteRec = from m in creditnoteRepository.CreditNoteWildSearch("CreditNoteCode", creditNote.CreditNoteCode)
                                 select m;
                int iRecCnt = creditnoteRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The Credit Note Code " + creditNote.CreditNoteCode + " already existed in system ! ";
                }
                else
                {
                    creditnoteRepository.SaveCreditNote(creditNote);
                    Session["CreditNoteID"] = creditNote.ID;
                    Session["CreditNoteCode"] = creditNote.CreditNoteCode;
                    
                    creditnoteLineModel.CreditNote = (from p in creditnoteRepository.CreditNote
                                                where p.ID.Equals(creditNote.ID)
                                                orderby p.CreditNoteCode
                                                select p).FirstOrDefault();
                    if (creditNote.ID != 0)
                    {
                        creditnoteLineModel.CreditNoteLine = from p in creditnoteLineRepository.CreditNoteLine
                                                          where p.ID.Equals(creditNote.ID)
                                                       orderby p.CreditNoteCode
                                                             select p;
                    }
                    Session["CreditNoteLineID"] = "";
                    TempData["Message"] = string.Format("{0} was added in system !", creditNote.CreditNoteCode);
                }
                //return RedirectToAction("Index");
            }

            return View(creditnoteLineModel);

        }

        // GET 
        public ActionResult CreateCreditNoteAndCreditNoteLine()
        {
            PopulateAccountDropDownList();
            return PartialView(new CreditNoteLine());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCreditNoteAndCreditNoteLine([Bind(Include = "ID,CreditNoteID,CreditNoteLineCode,CreditNoteCode,AccCode,BlkCode,VehCode,VehExpenseCode,Description,Quantity,UnitPrice,Total,CurrencyCost,CurrencyAmount,TaxRate,TaxRef,TaxInd")] CreditNoteLine creditnoteLine)
        {
            if (ModelState.IsValid)
            {
                int iChartOfAcc = Int32.Parse(Request.Form["ChartOfAcc"].ToString());
                creditnoteLine.CreditNoteID = Int32.Parse(Session["CreditNoteID"].ToString());
                creditnoteLine.CreditNoteCode = Session["CreditNoteCode"].ToString();
                creditnoteLine.ChartOfAccountID = iChartOfAcc;
                ChartOfAccount chartOfAccount = chartOfAccountRepository.ChartOfAccount.FirstOrDefault(p => p.ID == iChartOfAcc);
                creditnoteLine.AccCode = chartOfAccount.AccCode;

                var creditnoteLineRec = from m in creditnoteLineRepository.CreditNoteLineWildSearch("CreditNoteLineCode", creditnoteLine.CreditNoteLineCode)
                                     select m;
                int iRecCnt = creditnoteLineRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The Credit Note Line Code " + creditnoteLine.CreditNoteLineCode + " already existed in system ! ";
                }
                else
                {
                    creditnoteLineRepository.SaveCreditNoteLine(creditnoteLine);
                    Session["CreditNoteLineID"] = creditnoteLine.ID;
                    Session["CreditNoteLineCode"] = creditnoteLine.CreditNoteLineCode;
                    //return RedirectToAction("Index");

                    TempData["Message"] = string.Format("{0} was added", creditnoteLine.CreditNoteLineCode);
                }
                PopulateAccountDropDownList(creditnoteLine.ChartOfAccountID);
                return RedirectToAction("Edit2", new { id = creditnoteLine.CreditNoteID });
            }
            return View(creditnoteLine);
        }

        public ActionResult CreateCreditNoteLine(Int64? creditnoteId, string creditnoteCode)
        {
            Session["CreditNoteID"] = creditnoteId;
            Session["CreditNoteCode"] = creditnoteCode;
            PopulateAccountDropDownList();
            return PartialView(new CreditNoteLine());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCreditNoteLine([Bind(Include = "ID,CreditNoteID,CreditNoteLineCode,CreditNoteCode,AccCode,BlkCode,VehCode,VehExpenseCode,Description,Quantity,UnitPrice,Total,CurrencyCost,CurrencyAmount,TaxRate,TaxRef,TaxInd")] CreditNoteLine creditnoteLine)
        {
            if (ModelState.IsValid)
            {
                int iChartOfAcc = Int32.Parse(Request.Form["ChartOfAcc"].ToString());
                creditnoteLine.CreditNoteID = Int32.Parse(Session["CreditNoteID"].ToString());
                creditnoteLine.CreditNoteCode = Session["CreditNoteCode"].ToString();
                creditnoteLine.ChartOfAccountID = iChartOfAcc;
                ChartOfAccount chartOfAccount = chartOfAccountRepository.ChartOfAccount.FirstOrDefault(p => p.ID == iChartOfAcc);
                creditnoteLine.AccCode = chartOfAccount.AccCode.Trim();

                var creditnoteLineRec = from m in creditnoteLineRepository.CreditNoteLineWildSearch("CreditNoteLineCode", creditnoteLine.CreditNoteLineCode)
                                     select m;
                int iRecCnt = creditnoteLineRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The Credit Note Line Code " + creditnoteLine.CreditNoteLineCode + " already existed in system ! ";
                }
                else
                {
                    creditnoteLineRepository.SaveCreditNoteLine(creditnoteLine);
                    Session["CreditNoteLineID"] = creditnoteLine.ID;
                    Session["CreditNoteLineCode"] = creditnoteLine.CreditNoteLineCode;

                    TempData["Message"] = string.Format("{0} was added in system.", creditnoteLine.CreditNoteLineCode);
                }
                PopulateAccountDropDownList(creditnoteLine.ChartOfAccountID);
                return RedirectToAction("Edit2", new { id = creditnoteLine.CreditNoteID });
            }
            return View(creditnoteLine);
        }

        public ActionResult EditCreditNoteLine(Int64? creditnoteId, Int64? creditnoteLineId, string creditnoteCode, string creditnoteLineCode)
        {
            Session["CreditNoteCode"] = creditnoteCode;
            Session["CreditNoteId"] = creditnoteId;
            Session["CreditNoteLineId"] = creditnoteLineId;
            Session["CreditNoteLineCode"] = creditnoteLineCode;

            //improve performance
            CreditNoteLine creditnoteLine = creditnoteLineRepository.GetCreditNoteLineByID((long)creditnoteLineId).SingleOrDefault();
            PopulateAccountDropDownList(creditnoteLine.ChartOfAccountID);
            return PartialView(creditnoteLine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCreditNoteLine([Bind(Include = "ID,CreditNoteID,CreditNoteLineCode,CreditNoteCode,AccCode,BlkCode,VehCode,VehExpenseCode,Description,Quantity,UnitPrice,Total,CurrencyCost,CurrencyAmount,TaxRate,TaxRef,TaxInd")] CreditNoteLine creditnoteLine)
        {
            if (ModelState.IsValid)
            {
                int iChartOfAcc = Int32.Parse(Request.Form["ChartOfAcc"].ToString());
                creditnoteLine.ChartOfAccountID = iChartOfAcc;
                creditnoteLine.CreditNoteID = Int32.Parse(Session["CreditNoteID"].ToString());
                creditnoteLine.CreditNoteCode = Session["CreditNoteCode"].ToString();
                ChartOfAccount chartOfAccount = chartOfAccountRepository.ChartOfAccount.FirstOrDefault(p => p.ID == iChartOfAcc);
                creditnoteLine.AccCode = chartOfAccount.AccCode.Trim();
                creditnoteLineRepository.SaveCreditNoteLine(creditnoteLine);
                Session["CreditNoteID"] = creditnoteLine.CreditNoteID;
                //return View("Edit");
                return RedirectToAction("Edit2", new { id = creditnoteLine.CreditNoteID });
            }
            return PartialView(creditnoteLine);
        }

        // GET: CreditNote/Edit/5
        public ActionResult Edit(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var creditnoteLineModel = new CreditNoteLineModel();
            creditnoteLineModel.CreditNote = creditnoteRepository.GetCreditNoteByID((long)id).FirstOrDefault();

            if (id != null)
            {
                //improve performance
                creditnoteLineModel.CreditNoteLine = creditnoteLineRepository.GetCreditNoteLine("CreditNoteID", (long)id);
            }

            if (creditnoteLineModel == null)
            {
                return HttpNotFound();
            }
            return PartialView(creditnoteLineModel);
            //CreditNote creditNote = db.CreditNote.Find(id);
            //if (creditNote == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(creditNote);
        }

        // POST: CreditNote/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CreditNoteCode,DocType,CustomerCode,TotalAmount,Remark,LocCode,AccMonth,AccYear,Status,CreateDate,UpdateDate,PrintDate,UpdateID,OutstandingAmount,CustRef,DeliveryRef,PostDate,CurrencyRate,CurrencyAmount,CurrencyOutAmount,CreditNoteDate,InvoiceCode")] CreditNote creditNote)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(creditNote).State = EntityState.Modified;
                //db.SaveChanges();
                creditNote.UpdateDate = DateTime.Now;
                creditNote.Status = "Active";
                creditnoteRepository.SaveCreditNote(creditNote);
                ViewBag.Message = string.Format("{0} was updated in system. ", creditNote.CreditNoteCode);
                
            return RedirectToAction("Index");
            }
            return View(creditNote);
        }

        public ActionResult Edit2(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TempData["Message"] = "";
            var creditnoteLineModel = new CreditNoteLineModel();

            //remove the orderBy to improve performance
            creditnoteLineModel.CreditNote = creditnoteRepository.GetCreditNoteByID((long)id).FirstOrDefault();
            if (id != null)
            {
                //improve performance
                creditnoteLineModel.CreditNoteLine = creditnoteLineRepository.GetCreditNoteLine("CreditNoteID", (long)id);
            }

            if (creditnoteLineModel == null)
            {
                return HttpNotFound();
            }
            return View(creditnoteLineModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2([Bind(Include = "ID,CreditNoteCode,DocType,CustomerCode,TotalAmount,Remark,LocCode,AccMonth,AccYear,Status,CreateDate,UpdateDate,PrintDate,UpdateID,OutstandingAmount,CustRef,DeliveryRef,PostDate,CurrencyRate,CurrencyAmount,CurrencyOutAmount,CreditNoteDate,InvoiceCode")] CreditNote creditNote)
        {
            if (ModelState.IsValid)
            {
                creditNote.UpdateDate = DateTime.Now;
                creditnoteRepository.SaveCreditNote(creditNote);
                ViewBag.Message = string.Format("{0} was updated in system. ", creditNote.CreditNoteCode);
                return RedirectToAction("Index");
            }
            return View(creditNote);
        }

        // GET: CreditNote/Delete/5
        public ActionResult Delete(Int64? id)
        {
            var creditnoteLineModel = new CreditNoteLineModel();

            creditnoteLineModel.CreditNote = creditnoteRepository.GetCreditNoteByID((long)id).FirstOrDefault();
            if (id != null)
            {
                //improve performance
                creditnoteLineModel.CreditNoteLine = creditnoteLineRepository.GetCreditNoteLine("CreditNoteID", (long)id);
            }
            CreditNote creditnote = creditnoteRepository.GetCreditNoteByID((long)id).SingleOrDefault();

            if (creditnoteLineModel.CreditNoteLine != null)
            {
                foreach (var item in creditnoteLineModel.CreditNoteLine)
                {
                    CreditNoteLine creditnoteLine = creditnoteLineRepository.CreditNoteLine.FirstOrDefault(p => p.ID == item.ID);
                    creditnoteLineRepository.DeleteCreditNoteLine(creditnoteLine);
                }
            }

            if (creditnote != null)
            {
                creditnoteRepository.DeleteCreditNote(creditnote);
                TempData["DeleteMessage"] = string.Format("{0} was deleted", creditnote.CreditNoteCode);
            }

            return RedirectToAction("Index");
            //CreditNote creditNote = db.CreditNote.Find(id);
            //if (creditNote == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(creditNote);
        }

        public ActionResult DeleteCreditNoteLine(Int64? creditnoteId, Int64? creditnoteLineId)
        {

            CreditNoteLine creditnoteLine = creditnoteLineRepository.CreditNoteLine.FirstOrDefault(p => p.ID == creditnoteLineId);

            if (creditnoteLine != null)
            {
                creditnoteLineRepository.DeleteCreditNoteLine(creditnoteLine);
                TempData["Message"] = "";
                TempData["DeleteMessage"] = string.Format("{0} was deleted in system !", creditnoteLine.CreditNoteLineCode);
            }

            return RedirectToAction("Edit2", new { id = creditnoteId });
        }

        // POST: CreditNote/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(long id)
        //{
        //    //CreditNote creditNote = db.CreditNote.Find(id);
        //    //db.CreditNote.Remove(creditNote);
        //    //db.SaveChanges();
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
