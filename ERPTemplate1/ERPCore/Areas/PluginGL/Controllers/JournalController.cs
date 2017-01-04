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

namespace PluginGL.Controllers
{
    public class JournalController : BaseController
    {
        //private EFDbContextGL db = new EFDbContextGL();
        private IJournalRepository journalRepository;
        private IJournalLineRepository journalLineRepository;
        private IChartOfAccountRepository chartOfAccountRepository;
        private IMonthEndTransactionRepository monthendTransactionRepository;

        public JournalController(IJournalRepository journalRepo, IJournalLineRepository journalLineRepo, IChartOfAccountRepository chartOfAccountRepo, IMonthEndTransactionRepository monthendTransactionRepo)
        {
            this.journalRepository = journalRepo;
            this.journalLineRepository = journalLineRepo;
            this.chartOfAccountRepository = chartOfAccountRepo;
            this.monthendTransactionRepository = monthendTransactionRepo;

        }

        // GET: Journal
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

            //var model = (from m in journalRepository.Journal
            //             select m);

            var model = (from m in journalRepository.Journal
                        select m);

            Int64? iRecCnt = CommonUtility.Null2LongZero(journalRepository.GetMaxID());//journalRepository.JournalCount();

            if (!String.IsNullOrEmpty(searchValue))
            {
                model = (from m in journalRepository.JournalWildSearch("JournalCode", searchValue)
                         select m);
                iRecCnt = model.Count();
                //model = model.Where(s => s.JournalCode.ToUpper().Contains(searchValue.ToUpper()));
                //model = model.FirstOrDefault(p => p.JournalCode == searchValue);
                //.FirstOrDefault(p => p.ID == journalLineId

            }
            else
            {
                //model = model.Skip(((int)page - 1) * pageSize)
                //       .Take(pageSize);
                //improve performance
                model = journalRepository.GetJournalPaging(((((long)page - 1) * pageSize) + 1), (long)page * pageSize);
            }

            switch (sortOrder)
            {
                default:
                    //model = model.OrderBy(s => s.ID);
                    break;
            }
           
            
            //return View(model.ToPagedList(pageNumber, pageSize));
            //improve performance
            var model3 = new StaticPagedList<Journal>(model, (int)page, pageSize, (int)iRecCnt);
            return View(model3);
            //return View(db.Journal.ToList());
        }

        // GET: Journal/Details/5
        public ActionResult Details(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from p in journalRepository.Journal
                         where p.ID.Equals(id)
                         orderby p.JournalCode
                         select p).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView(model);
            //Journal journal = db.Journal.Find(id);
            //if (journal == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(journal);
        }

        // GET: Journal/Create
        public ActionResult Create()
        {
            TempData["Message"] = "";
            TempData["DeteleMessage"] = "";
            Session["JournalCode"] = "";
            //Session["JournalID"] = "";
            Session["JournalLineId"] = "";
            Session["JournalLineCode"] = "";
            return View(new JournalLineModel());
            //return View();
        }

        // POST: Journal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,JournalCode,Description,ReceiveFrom,TransactType,RefNo,DocDate,DocAmt,GrandTotal,AccMonth,AccYear,LocCode,Status,PrintDate,CreateDate,UpdateDate,UpdateID,PostDate,ReverseID,ChargeFrom,FromJrnID,IsPosted")] Journal journal)
        {
            var journalLineModel = new JournalLineModel();
           
            if (ModelState.IsValid)
            {               
                //db.Journal.Add(journal);
                //db.SaveChanges();
                journal.CreateDate = DateTime.Now;
                journal.UpdateDate = DateTime.Now;
                journal.PrintDate = DateTime.Now;
                journal.PostDate = DateTime.Now;
                journal.DocDate = DateTime.Now;
             
                journal.UpdateID = Session["UserID"].ToString();
                journal.Status = "Active";
                var journalRec = from m in journalRepository.JournalWildSearch("JournalCode", journal.JournalCode)
                                 select m;
                int iRecCnt = journalRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The Journal Code " + journal.JournalCode + " already existed in system ! ";
                }
                else
                {
                    journalRepository.SaveJournal(journal);
                    Session["JournalID"] = journal.ID;
                    Session["JournalCode"] = journal.JournalCode;

                    journalLineModel.Journal = (from p in journalRepository.Journal
                                                where p.ID.Equals(journal.ID)
                                                orderby p.JournalCode
                                                select p).FirstOrDefault();
                    if (journal.ID != 0)
                    {
                        journalLineModel.JournalLine = from p in journalLineRepository.JournalLine
                                                       where p.ID.Equals(journal.ID)
                                                       orderby p.JournalCode
                                                       select p;
                    }
                    Session["JournalLineID"] = "";
                    TempData["Message"] = string.Format("{0} was added in system !", journal.JournalCode);
                }
                //return RedirectToAction("Index");
            }

            return View(journalLineModel);
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

        public ActionResult CreateJournalAndJournalLine(string roleName)
        {

            PopulateAccountDropDownList();
            return PartialView(new JournalLine());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateJournalAndJournalLine([Bind(Include = "ID,JournalID,AccountID,TaxCodeID,JournalLineCode,JournalCode,Description,Quantity,UnitPrice,Total,AccCode,BlkCode,VehCode,VehExpCode,LocCode,ItemCode,AutoGenRecords,CreditInd,SourceLineNo,TargetLineNo,ExportTO,TaxRate,TaxRef,TaxInd,DebitCreditIndicator")] JournalLine journalLine)
        {
            if (ModelState.IsValid)
            {
                int iChartOfAcc = Int32.Parse(Request.Form["ChartOfAcc"].ToString());
                journalLine.JournalID = Int32.Parse(Session["JournalID"].ToString());
                journalLine.JournalCode = Session["JournalCode"].ToString();
                journalLine.ChartOfAccountID = iChartOfAcc;
                journalLine.TaxCodeID = 0;
                ChartOfAccount chartOfAccount = chartOfAccountRepository.ChartOfAccount.FirstOrDefault(p => p.ID == iChartOfAcc);
                journalLine.AccCode = chartOfAccount.AccCode;

                var journalLineRec = from m in journalLineRepository.JournalLineWildSearch("JournalCode", journalLine.JournalLineCode)
                                 select m;
                int iRecCnt = journalLineRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The Journal Line Code " + journalLine.JournalLineCode + " already existed in system ! ";
                }
                else
                {
                    journalLineRepository.SaveJournalLine(journalLine);
                    UpdateHeaderAmount(journalLine);
                    Session["JournalLineID"] = journalLine.ID;
                    Session["JournalLineCode"] = journalLine.JournalLineCode;
                    //ViewBag.Message = string.Format("{0} was updated", sh_role.RoleName);
                    //return RedirectToAction("Index");

                    TempData["Message"] = string.Format("{0} was added in system !", journalLine.JournalLineCode);
                }
                PopulateAccountDropDownList(journalLine.ChartOfAccountID);

                return RedirectToAction("Edit2", new { id = journalLine.JournalID });
            }
            return View(journalLine);
        }

        public ActionResult CreateJournalLine(Int64? journalId, string journalCode)
        {
            Session["JournalID"] = journalId;
            Session["JournalCode"] = journalCode;
            PopulateAccountDropDownList();
            return PartialView(new JournalLine());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateJournalLine([Bind(Include = "ID,JournalID,AccountID,TaxID,JournalLineCode,JournalCode,Description,Quantity,UnitPrice,Total,AccCode,BlkCode,VehCode,VehExpCode,LocCode,ItemCode,AutoGenRecords,CreditInd,SourceLineNo,TargetLineNo,ExportTO,TaxRate,TaxRef,TaxInd,DebitCreditIndicator")] JournalLine journalLine)
        {
            if (ModelState.IsValid)
            {
                int iChartOfAcc = Int32.Parse(Request.Form["ChartOfAcc"].ToString());
                journalLine.JournalID = Int32.Parse(Session["JournalID"].ToString());
                journalLine.ChartOfAccountID = iChartOfAcc;
                journalLine.JournalCode = Session["JournalCode"].ToString();
                journalLine.TaxCodeID = 0;
                //journalLine.AccCode = Session["ChartOfAcc"].ToString();
                ChartOfAccount chartOfAccount = chartOfAccountRepository.ChartOfAccount.FirstOrDefault(p => p.ID == iChartOfAcc);
                journalLine.AccCode = chartOfAccount.AccCode.Trim();
                var journalLineRec = from m in journalLineRepository.JournalLineWildSearch("JournalLineCode", journalLine.JournalLineCode)
                                 select m;
                int iRecCnt = journalLineRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The Journal Line Code "+ journalLine.JournalLineCode + " already existed in system ! ";
                }
                else
                {
                    journalLineRepository.SaveJournalLine(journalLine);
                    UpdateHeaderAmount(journalLine);
                    Session["JournalLineID"] = journalLine.ID;
                    Session["JournalLineCode"] = journalLine.JournalLineCode;

                    TempData["Message"] = string.Format("{0} was added in system !", journalLine.JournalLineCode);
                }
                    PopulateAccountDropDownList(journalLine.ChartOfAccountID);
                    return RedirectToAction("Edit2", new { id = journalLine.JournalID });
                
            }
            return View(journalLine);
        }

        //private bool isJournalIdExist(string? journalCode)
        //{
        //   bool recordExist = false;
        //    Journal journal = journalRepository.Journal.FirstOrDefault(p => p.JournalCode == journalCode);
        //    if (journal != null)
        //    {
        //        recordExist = true;
        //    }
        //    return recordExist;
        //}
        //GetJournalLine("JournalID", (long)headerId)
        private void UpdateHeaderAmount(JournalLine journalLine)
        {
            long headerId = journalLine.JournalID;
            var model = (from p in journalLineRepository.JournalLine
                         where p.JournalID.Equals(headerId)
                         select p);
            double dTotalAmount = model.Sum(p => p.Total);
            var journalModel = journalRepository.GetJournalByID(headerId);
            foreach (var item in journalModel)
            {
                item.DocAmt = dTotalAmount;
                journalRepository.SaveJournal(item);
            }

        }

        public ActionResult EditJournalLine(Int64? journalId, Int64? journalLineId, string journalCode, string journalLineCode)
        {
            Session["JournalCode"] = journalCode;
            Session["JournalID"] = journalId;
            Session["JournalLineId"] = journalLineId;
            Session["JournalLineCode"] = journalLineCode;
            
            //JournalLine journalLine = journalLineRepository.JournalLine.FirstOrDefault(p => p.ID == journalLineId && p.JournalID == journalId);
            //improve performance
            JournalLine journalLine = journalLineRepository.GetJournalLineByID((long)journalLineId).SingleOrDefault();
            PopulateAccountDropDownList(journalLine.ChartOfAccountID);
            return PartialView(journalLine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditJournalLine([Bind(Include = "ID,JournalID,AccountID,TaxID,JournalLineCode,JournalCode,Description,Quantity,UnitPrice,Total,AccCode,BlkCode,VehCode,VehExpCode,LocCode,ItemCode,AutoGenRecords,CreditInd,SourceLineNo,TargetLineNo,ExportTO,TaxRate,TaxRef,TaxInd,DebitCreditIndicator")] JournalLine journalLine)
        {
            if (ModelState.IsValid)
            {
                int iChartOfAcc = Int32.Parse(Request.Form["ChartOfAcc"].ToString());
                journalLine.JournalID = Int32.Parse(Session["JournalID"].ToString());
                journalLine.ChartOfAccountID = iChartOfAcc;
                journalLine.JournalCode = Session["JournalCode"].ToString();
                journalLine.TaxCodeID = 0;
                //journalLine.AccCode = Session["ChartOfAcc"].ToString();
                ChartOfAccount chartOfAccount = chartOfAccountRepository.ChartOfAccount.FirstOrDefault(p => p.ID == iChartOfAcc);
                journalLine.AccCode = chartOfAccount.AccCode.Trim();
                journalLineRepository.SaveJournalLine(journalLine);
                UpdateHeaderAmount(journalLine);
                //ViewBag.Message = string.Format("{0} was updated", sh_role.RoleName);
                //return View("~/Views/SH_ROLE/Edit/" + sh_roleaccess.SH_ROLEID);
                Session["JournalID"] = journalLine.JournalID;
                //return View("Edit");
                return RedirectToAction("Edit2", new { id = journalLine.JournalID });
            }
            return PartialView(journalLine);
            //return Json(new { success = true });
        }

        // GET: Journal/Edit/5
        public ActionResult Edit(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var journalLineModel = new JournalLineModel();

            //remove orderBy to improve speed
            //journalLineModel.Journal = (from p in journalRepository.Journal
            //                           where p.ID.Equals(id)
            //                           select p).FirstOrDefault();
            //improve performance
            journalLineModel.Journal = journalRepository.GetJournalByID((long)id).FirstOrDefault();
            if (id != null)
            {
                //journalLineModel.JournalLine = journalLineModel.Journal.JournalLine;
                //improve performance
                journalLineModel.JournalLine = journalLineRepository.GetJournalLine("JournalID", (long)id);
            }

            if (journalLineModel == null)
            {
                return HttpNotFound();
            }
            return PartialView(journalLineModel);
            // Journal journal = db.Journal.Find(id);
            //if (journal == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(journal);
        }

        // POST: Journal/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,JournalCode,Description,ReceiveFrom,TransactType,RefNo,DocDate,DocAmt,GrandTotal,AccMonth,AccYear,LocCode,Status,PrintDate,CreateDate,UpdateDate,UpdateID,PostDate,ReverseID,ChargeFrom,FromJrnID,IsPosted")] Journal journal)
        {
            if (ModelState.IsValid)
            {
                if (journal.IsPosted == true) 
                {
                    ViewBag.Message = string.Format("{0} was posted, you are not allow to save or post ", journal.JournalCode);
                    return RedirectToAction("Edit2", new { id = journal.ID });
                }

                if (Request.Form["Post"] != null)
                {
                    try
                    {
                        //Post into mthend transaction table
                        var journalPostingModel = journalRepository.GetJournalMonthEndPostingModel(journal.ID);
                        
                        foreach (var item in journalPostingModel) //retrieve each item and assign to model
                        {
                            MonthEndTransaction monthendTransaction = new MonthEndTransaction();
                            monthendTransaction.OrganizationID = Int32.Parse(item.OrganizationID.ToString());
                            monthendTransaction.VoucherID = Int32.Parse(item.VoucherID.ToString());
                            monthendTransaction.VoucherLineID = Int32.Parse(item.VoucherLineID.ToString());
                            monthendTransaction.VoucherCode = item.VoucherCode;
                            monthendTransaction.VoucherLineCode = item.VoucherLineCode;
                            monthendTransaction.Description = item.Description;
                            monthendTransaction.AccYear = item.AccYear;
                            monthendTransaction.AccMonth = item.AccMonth;
                            monthendTransaction.AccCode = item.AccCode;
                            monthendTransaction.TransactType = item.TransactType;
                            monthendTransaction.DocDate = item.DocDate;
                            monthendTransaction.DocAmt = item.DocAmt;
                            monthendTransaction.Quantity = item.Quantity;
                            monthendTransaction.UnitPrice = item.UnitPrice;
                            monthendTransaction.Total = item.Total;
                            monthendTransaction.DebitCreditIndicator = item.DebitCreditIndicator;
                            monthendTransaction.PostingTable = item.PostingTable;
                            monthendTransaction.PostingDate = item.PostingDate;
                            monthendTransactionRepository.SaveMonthEndTransaction(monthendTransaction);
                        }

                        journal.IsPosted = true;
                        journal.PostDate = DateTime.Now;
                        journalRepository.SaveJournal(journal);
                        ViewBag.Message = string.Format("{0} was posted", journal.JournalCode);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Ex={0}", ex.Message);
                    }                    
                }

                else
                {
                    //db.Entry(journal).State = EntityState.Modified;
                    //db.SaveChanges();
                    //return RedirectToAction("Index");
                    journal.UpdateDate = DateTime.Now;
                    journalRepository.SaveJournal(journal);
                    ViewBag.Message = string.Format("{0} was updated", journal.JournalCode);
                }
                return RedirectToAction("Index");
            }
            return PartialView(journal);
        }

        public ActionResult Edit2(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //TempData["Message"] = "";
            var journalLineModel = new JournalLineModel();

            //remove the orderBy to improve performance
            //journalLineModel.Journal = (from p in journalRepository.Journal
            //                           where p.ID.Equals(id)
            //                           select p).FirstOrDefault();
            journalLineModel.Journal = journalRepository.GetJournalByID((long)id).FirstOrDefault();
            if (id != null)
            {
                //journalLineModel.JournalLine = journalLineModel.Journal.JournalLine;
                //improve performance                
                journalLineModel.JournalLine = journalLineRepository.GetJournalLine("JournalID", (long)id);
            }

            if (journalLineModel == null)
            {
                return HttpNotFound();
            }
            return View(journalLineModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2([Bind(Include = "ID,JournalCode,Description,ReceiveFrom,TransactType,RefNo,DocDate,DocAmt,GrandTotal,AccMonth,AccYear,LocCode,Status,PrintDate,CreateDate,UpdateDate,UpdateID,PostDate,ReverseID,ChargeFrom,FromJrnID,IsPosted")] Journal journal)
        {
            if (ModelState.IsValid)
            {
                if (journal.IsPosted == true)
                {
                    ViewBag.Message = string.Format("{0} was posted, you are not allow to save or post ", journal.JournalCode);
                    return RedirectToAction("Index");
                }

                if (Request.Form["Post"] != null)
                {
                    //Post into mthend transaction table
                    journal.IsPosted = true;
                    journalRepository.SaveJournal(journal);
                    ViewBag.Message = string.Format("{0} was posted", journal.JournalCode);
                }
                else
                {
                    journal.UpdateDate = DateTime.Now;
                    journalRepository.SaveJournal(journal);
                    ViewBag.Message = string.Format("{0} was updated", journal.JournalCode);
                }
                return RedirectToAction("Index");
            }
            return View(journal);
        }
        
        // GET: Journal/Delete/5
        //public ActionResult Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Journal journal = db.Journal.Find(id);
        //    if (journal == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(journal);
        //}

        // POST: Journal/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(Int64? id)
        {
            //Journal journal = db.Journal.Find(id);
            //db.Journal.Remove(journal);
            //db.SaveChanges();
            //Journal journal = journalRepository.Journal.FirstOrDefault(p => p.ID == id);

            //if (journal != null)
            //{
            //    journalRepository.DeleteJournal(journal);
            //    TempData["DeleteMessage"] = string.Format("{0} was deleted", journal.JournalCode);
            //}

            //return RedirectToAction("Index");

            //Journal journal = journalRepository.Journal.FirstOrDefault(p => p.ID == id);
            //Journal journal = journalRepository.Journal.FirstOrDefault(p => p.ID == id);
            var journalLineModel = new JournalLineModel();

            journalLineModel.Journal = journalRepository.GetJournalByID((long)id).FirstOrDefault();
            if (id != null)
            {
                //journalLineModel.JournalLine = journalLineModel.Journal.JournalLine;
                //improve performance
                journalLineModel.JournalLine = journalLineRepository.GetJournalLine("JournalID", (long)id);
            }
            Journal journal = journalRepository.GetJournalByID((long)id).SingleOrDefault();

            if (journalLineModel.JournalLine != null)
            {
                foreach (var item in journalLineModel.JournalLine)
                {
                    JournalLine journalLine = journalLineRepository.JournalLine.FirstOrDefault(p => p.ID == item.ID);
                    journalLineRepository.DeleteJournalLine(journalLine);
                    UpdateHeaderAmount(journalLine);
                }
            }

            if (journal != null)
            {
                journalRepository.DeleteJournal(journal);
                TempData["DeleteMessage"] = string.Format("{0} was deleted", journal.JournalCode);
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteJournalLine(Int64? journalId, Int64? journalLineId)
        {

            JournalLine journalLine = journalLineRepository.JournalLine.FirstOrDefault(p => p.ID == journalLineId);
            //JournalLine journalLine = journalLineRepository.GetJournalLineByID((long)journalLineId).SingleOrDefault();

            if (journalLine != null)
            {
                //string functionName = journalLine.Journal.JournalCode;
                //string functionName = journalLine.JournalLineCode;
                journalLineRepository.DeleteJournalLine(journalLine);
                UpdateHeaderAmount(journalLine);
                TempData["Message"] = "";
                TempData["DeleteMessage"] = string.Format("{0} was deleted", journalLine.JournalLineCode);
            }

            return RedirectToAction("Edit2", new { id = journalId });
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
