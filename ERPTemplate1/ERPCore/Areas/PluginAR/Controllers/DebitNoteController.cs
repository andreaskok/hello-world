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
using PluginAR.Models;
using System.Web.SessionState;

namespace PluginAR.Controllers
{
    //[SessionState(SessionStateBehavior.Disabled)]
    public class DebitNoteController : BaseController
    {
        //private EFDbContextAR db = new EFDbContextAR();

        private IDebitNoteRepository debitnoteRepository;
        private IDebitNoteLineRepository debitnoteLineRepository;
        private IChartOfAccountRepository chartOfAccountRepository;

        public DebitNoteController(IDebitNoteRepository debitnoteRepo, IDebitNoteLineRepository debitnotelineRepo, IChartOfAccountRepository chartOfAccountRepo)
        {
            this.debitnoteRepository = debitnoteRepo;
            this.debitnoteLineRepository = debitnotelineRepo;
            this.chartOfAccountRepository = chartOfAccountRepo;
        }

        // GET: DebitNote
        public ActionResult Index(string searchValue, string sortOrder, string currentFilter, string searchFieldList, string sortBy, int? page)
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

            var model = (from m in debitnoteRepository.DebitNote
                         select m);

            Int64? iRecCnt = CommonUtility.Null2LongZero(debitnoteRepository.GetMaxID());

            if (!String.IsNullOrEmpty(searchValue))
            {
                model = (from m in debitnoteRepository.DebitNoteWildSearch(searchFieldList, searchValue)
                         select m);
                iRecCnt = model.Count();
            }
            else
            {
                //model = model.Skip(((int)page - 1) * pageSize)
                //       .Take(pageSize);
                //improve performance
                model = debitnoteRepository.GetDebitNotePaging(((((long)page - 1) * pageSize) + 1), (long)page * pageSize);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    if (sortBy.Equals("DebitNoteCode"))
                    {
                        model = model.OrderByDescending(s => s.DebitNoteCode);
                    }
                    else if (sortBy.Equals("CustRef"))
                    {
                        model = model.OrderByDescending(s => s.CustRef);
                    }

                    break;
                default:
                    if (sortBy.Equals("DebitNoteCode"))
                    {
                        model = model.OrderBy(s => s.DebitNoteCode);
                    }
                    else if (sortBy.Equals("CustRef"))
                    {
                        model = model.OrderBy(s => s.CustRef);
                    }
                    break;
            }

            ViewBag.SearchFieldList = GetSearchField("DebitNoteCode");
            //improve performance
            var model3 = new StaticPagedList<DebitNote>(model, (int)page, pageSize, (int)iRecCnt);
            return View(model3);
   
        }

        // GET: DebitNote/Details/5
        public ActionResult Details(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from p in debitnoteRepository.DebitNote
                         where p.ID.Equals(id)
                         orderby p.DebitNoteCode
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

        

        // GET: DebitNote/Create
        public ActionResult Create()
        {
            //return View();
            Session["DebitNoteID"] = null;
            PopulateAccountDropDownList();
            return View(new DebitNoteLineModel());
        }

        // POST: DebitNote/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DebitNoteCode,DocType,CustomerCode,TotalAmount,Remark,LocCode,AccMonth,AccYear,Status,CreateDate,UpdateDate,PrintDate,UpdateID,OutstandingAmount,CustRef,DeliveryRef,PostDate,CurrencyRate,CurrencyAmount,CurrencyOutAmount,DebitNoteDate,InvoiceCode")] DebitNote debitNote)
        {
            var debitnoteLineModel = new DebitNoteLineModel();

            if (ModelState.IsValid)
            {
                debitNote.CreateDate = DateTime.Now;
                debitNote.UpdateDate = DateTime.Now;
                debitNote.PrintDate = DateTime.Now;
                debitNote.PostDate = DateTime.Now;
                debitNote.UpdateID = Session["UserID"].ToString();
                debitNote.Status = "Active";
                var debitnoteRec = from m in debitnoteRepository.DebitNoteWildSearch("DebitNoteCode", debitNote.DebitNoteCode)
                                    select m;
                int iRecCnt = debitnoteRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The Debit Note Code " + debitNote.DebitNoteCode + " already existed in system ! ";
                }
                else
                {
                    debitnoteRepository.SaveDebitNote(debitNote);
                    Session["DebitNoteID"] = debitNote.ID;
                    Session["DebitNoteCode"] = debitNote.DebitNoteCode;

                    debitnoteLineModel.DebitNote = (from p in debitnoteRepository.DebitNote
                                                      where p.ID.Equals(debitNote.ID)
                                                      orderby p.DebitNoteCode
                                                      select p).FirstOrDefault();
                    if (debitNote.ID != 0)
                    {
                        debitnoteLineModel.DebitNoteLine = from p in debitnoteLineRepository.DebitNoteLine
                                                             where p.ID.Equals(debitNote.ID)
                                                             orderby p.DebitNoteCode
                                                            select p;
                    }
                    Session["DebitNoteLineID"] = "";
                    TempData["Message"] = string.Format("{0} was added in system !", debitNote.DebitNoteCode);
                    return RedirectToAction("Edit2", new { id = debitNote.ID });
                }
                //return RedirectToAction("Index");
            }

            return View(debitnoteLineModel);
        }

        // GET 
        public ActionResult CreateDebitNoteAndDebitNoteLine()
        {
            PopulateAccountDropDownList();
            return PartialView(new DebitNoteLine());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDebitNoteAndDebitNoteLine([Bind(Include = "ID,DebitNoteID,ChartOfAccountID,DebitNoteLineCode,DebitNoteCode,AccCode,BlkCode,VehCode,VehExpenseCode,Description,Quantity,UnitPrice,Total,CurrencyCost,CurrencyAmount,TaxRate,TaxRef,TaxInd")] DebitNoteLine debitnoteLine)
        {
            if (ModelState.IsValid)
            {
                int iChartOfAcc = Int32.Parse(Request.Form["ChartOfAcc"].ToString());
                debitnoteLine.DebitNoteID = Int32.Parse(Session["DebitNoteID"].ToString());
                debitnoteLine.DebitNoteCode = Session["DebitNoteCode"].ToString();
                debitnoteLine.ChartOfAccountID = iChartOfAcc;
                ChartOfAccount chartOfAccount = chartOfAccountRepository.ChartOfAccount.FirstOrDefault(p => p.ID == iChartOfAcc);
                debitnoteLine.AccCode = chartOfAccount.AccCode;

                var debitnoteLineRec = from m in debitnoteLineRepository.DebitNoteLineWildSearch("DebitNoteLineCode", debitnoteLine.DebitNoteLineCode)
                                        select m;
                int iRecCnt = debitnoteLineRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The Debit Note Line Code " + debitnoteLine.DebitNoteLineCode + " already existed in system ! ";
                }
                else
                {
                    debitnoteLineRepository.SaveDebitNoteLine(debitnoteLine);
                    UpdateHeaderAmount(debitnoteLine);
                    Session["DebitNoteLineID"] = debitnoteLine.ID;
                    Session["DebitNoteLineCode"] = debitnoteLine.DebitNoteLineCode;
                    //return RedirectToAction("Index");

                    TempData["Message"] = string.Format("{0} was added", debitnoteLine.DebitNoteLineCode);
                }
                PopulateAccountDropDownList(debitnoteLine.ChartOfAccountID);
                return RedirectToAction("Edit2", new { id = debitnoteLine.DebitNoteID });
            }
            return View(debitnoteLine);
        }

        public ActionResult CreateDebitNoteLine(Int64? debitnoteId, string debitnoteCode)
        {
            Session["DebitNoteID"] = debitnoteId;
            Session["DebitNoteCode"] = debitnoteCode;
            PopulateAccountDropDownList();
            return PartialView(new DebitNoteLine());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDebitNoteLine([Bind(Include = "ID,DebitNoteID,ChartOfAccountID,DebitNoteLineCode,DebitNoteCode,AccCode,BlkCode,VehCode,VehExpenseCode,Description,Quantity,UnitPrice,Total,CurrencyCost,CurrencyAmount,TaxRate,TaxRef,TaxInd")] DebitNoteLine debitnoteLine)
        {
            if (ModelState.IsValid)
            {
                int iChartOfAcc = Int32.Parse(Request.Form["ChartOfAcc"].ToString());
                debitnoteLine.DebitNoteID = Int32.Parse(Session["DebitNoteID"].ToString());
                debitnoteLine.DebitNoteCode = Session["DebitNoteCode"].ToString();
                debitnoteLine.ChartOfAccountID = iChartOfAcc;
                ChartOfAccount chartOfAccount = chartOfAccountRepository.ChartOfAccount.FirstOrDefault(p => p.ID == iChartOfAcc);
                debitnoteLine.AccCode = chartOfAccount.AccCode.Trim();

                var debitnoteLineRec = from m in debitnoteLineRepository.DebitNoteLineWildSearch("DebitNoteLineCode", debitnoteLine.DebitNoteLineCode)
                                        select m;
                int iRecCnt = debitnoteLineRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The Debit Note Line Code " + debitnoteLine.DebitNoteLineCode + " already existed in system ! ";
                }
                else
                {
                    debitnoteLineRepository.SaveDebitNoteLine(debitnoteLine);
                    UpdateHeaderAmount(debitnoteLine);
                    Session["DebitNoteLineID"] = debitnoteLine.ID;
                    Session["DebitNoteLineCode"] = debitnoteLine.DebitNoteLineCode;

                    TempData["Message"] = string.Format("{0} was added", debitnoteLine.DebitNoteLineCode);
                }
                PopulateAccountDropDownList(debitnoteLine.ChartOfAccountID);
                return RedirectToAction("Edit2", new { id = debitnoteLine.DebitNoteID });
            }
            return View(debitnoteLine);
        }

        //GET 
        public ActionResult EditDebitNoteLine(Int64? debitnoteId, Int64? debitnoteLineId, string debitnoteCode, string debitnoteLineCode)
        {
            Session["DebitNoteCode"] = debitnoteCode;
            Session["DebitNoteId"] = debitnoteId;
            Session["DebitNoteLineId"] = debitnoteLineId;
            Session["DebitNoteLineCode"] = debitnoteLineCode;

            //improve performance
            DebitNoteLine debitnoteLine = debitnoteLineRepository.GetDebitNoteLineByID((long)debitnoteLineId).SingleOrDefault();
            PopulateAccountDropDownList(debitnoteLine.ChartOfAccountID);
            return PartialView(debitnoteLine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDebitNoteLine([Bind(Include = "ID,DebitNoteID,ChartOfAccountID,DebitNoteLineCode,DebitNoteCode,AccCode,BlkCode,VehCode,VehExpenseCode,Description,Quantity,UnitPrice,Total,CurrencyCost,CurrencyAmount,TaxRate,TaxRef,TaxInd")] DebitNoteLine debitnoteLine)
        {
            if (ModelState.IsValid)
            {
                int iChartOfAcc = Int32.Parse(Request.Form["ChartOfAcc"].ToString());
                debitnoteLine.ChartOfAccountID = iChartOfAcc;
                ChartOfAccount chartOfAccount = chartOfAccountRepository.ChartOfAccount.FirstOrDefault(p => p.ID == iChartOfAcc);
                debitnoteLine.AccCode = chartOfAccount.AccCode.Trim();
                debitnoteLine.DebitNoteID = Int32.Parse(Session["DebitNoteID"].ToString());
                debitnoteLine.DebitNoteCode = Session["DebitNoteCode"].ToString();
                debitnoteLineRepository.SaveDebitNoteLine(debitnoteLine);
                UpdateHeaderAmount(debitnoteLine);
                Session["DebitNoteID"] = debitnoteLine.DebitNoteID;
                //return View("Edit");
                return RedirectToAction("Edit2", new { id = debitnoteLine.DebitNoteID });
            }
            return PartialView(debitnoteLine);
        }


        // GET: DebitNote/Edit/5
        public ActionResult Edit(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var debitnoteLineModel = new DebitNoteLineModel();
            debitnoteLineModel.DebitNote = debitnoteRepository.GetDebitNoteByID((long)id).FirstOrDefault();
            Session["DebitNoteCode"] = debitnoteLineModel.DebitNote.DebitNoteCode;
            Session["DebitNoteId"] = debitnoteLineModel.DebitNote.ID;
            if (id != null)
            {
                //improve performance
                debitnoteLineModel.DebitNoteLine = debitnoteLineRepository.GetDebitNoteLine("DebitNoteID", (long)id);
            }

            if (debitnoteLineModel == null)
            {
                return HttpNotFound();
            }
            GetExcelCart().Clear();
            GetIncrementCart().Clear();
            GetDeletedExcelCart().Clear();
            return PartialView(debitnoteLineModel);
            
        }

        // POST: DebitNote/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DebitNoteCode,DocType,CustomerCode,TotalAmount,Remark,LocCode,AccMonth,AccYear,Status,CreateDate,UpdateDate,PrintDate,UpdateID,OutstandingAmount,CustRef,DeliveryRef,PostDate,CurrencyRate,CurrencyAmount,CurrencyOutAmount,DebitNoteDate,InvoiceCode")] DebitNote debitNote)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form["Save"] != null)
                {
                    System.Console.WriteLine("Save...");
                    debitNote.UpdateDate = DateTime.Now;
                    debitNote.Status = "Active";
                    debitnoteRepository.SaveDebitNote(debitNote);

                    UpdateExcelGrid2Database();

                    GetExcelCart().Clear();
                    GetIncrementCart().Clear();
                    GetDeletedExcelCart().Clear();
                    ViewBag.Message = string.Format("{0} was updated in system. ", debitNote.DebitNoteCode);

                    return RedirectToAction("Index");
                }
                else if (Request.Form["Reset"] != null)
                {
                    System.Console.WriteLine("Reset...");
                    return RedirectToAction("Edit2", new { id = Session["DebitNoteId"] });
                }
                
            }
            return View(debitNote);
        }

        private void UpdateExcelGrid2Database()
        {
            foreach (var item in GetExcelCart().ExcelLines)
            {
                if (item.ID > 0 && item.IsNew == false)
                {
                    //Edit excel record
                    DebitNoteLine dnl = (from p in debitnoteLineRepository.DebitNoteLine
                                         where p.ID.Equals(item.ID)
                                         select p).FirstOrDefault();
                    dnl.DebitNoteLineCode = item.DebitNoteLineCode;
                    dnl.ChartOfAccountID = item.ChartOfAccountID;
                    dnl.AccCode = item.AccCode;
                    dnl.Description = item.Description;
                    dnl.Quantity = item.Quantity;
                    dnl.UnitPrice = item.UnitPrice;
                    dnl.Total = item.Quantity * item.UnitPrice;
                    debitnoteLineRepository.SaveDebitNoteLine(dnl);
                }
                else
                {
                    if (!CommonUtility.Null2Empty(item.DebitNoteLineCode).Equals("") && !CommonUtility.Null2Empty(item.Description).Equals("") && item.Quantity != 0 && item.UnitPrice != 0 && item.IsNew.Equals(true))
                    {   //Add record
                        DebitNoteLine dnl = new DebitNoteLine();
                        dnl.DebitNoteID = item.DebitNoteID;
                        dnl.DebitNoteCode = item.DebitNoteCode;
                        dnl.DebitNoteLineCode = item.DebitNoteLineCode;
                        dnl.ChartOfAccountID = item.ChartOfAccountID;
                        dnl.AccCode = item.AccCode;
                        dnl.Description = item.Description;
                        dnl.Quantity = item.Quantity;
                        dnl.UnitPrice = item.UnitPrice;
                        dnl.Total = item.Quantity * item.UnitPrice;
                        debitnoteLineRepository.SaveDebitNoteLine(dnl);
                        
                    }
                    

                }

            }

            foreach (var item in GetDeletedExcelCart().GetDeletedExcelLines)
            {
                DebitNoteLine dnl = (from p in debitnoteLineRepository.DebitNoteLine
                                     where p.ID.Equals(item.ID)
                                     select p).FirstOrDefault();
                debitnoteLineRepository.DeleteDebitNoteLine(dnl);
            }
        }

        public ActionResult Edit2(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TempData["Message"] = "";
            var debitnoteLineModel = new DebitNoteLineModel();

            //remove the orderBy to improve performance
            debitnoteLineModel.DebitNote = debitnoteRepository.GetDebitNoteByID((long)id).FirstOrDefault();
            Session["DebitNoteCode"] = debitnoteLineModel.DebitNote.DebitNoteCode;
            Session["DebitNoteId"] = debitnoteLineModel.DebitNote.ID;

            if (id != null)
            {
                //improve performance
                debitnoteLineModel.DebitNoteLine = debitnoteLineRepository.GetDebitNoteLine("DebitNoteID", (long)id);
            }

            if (debitnoteLineModel == null)
            {
                return HttpNotFound();
            }
            GetExcelCart().Clear();
            GetIncrementCart().Clear();
            GetDeletedExcelCart().Clear();
            return View(debitnoteLineModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2([Bind(Include = "ID,DebitNoteCode,DocType,CustomerCode,TotalAmount,Remark,LocCode,AccMonth,AccYear,Status,CreateDate,UpdateDate,PrintDate,UpdateID,OutstandingAmount,CustRef,DeliveryRef,PostDate,CurrencyRate,CurrencyAmount,CurrencyOutAmount,DebitNoteDate,InvoiceCode")] DebitNote debitNote)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form["Save"] != null)
                {
                    debitNote.UpdateDate = DateTime.Now;
                    debitnoteRepository.SaveDebitNote(debitNote);
                    ViewBag.Message = string.Format("{0} was updated in system. ", debitNote.DebitNoteCode);
                    UpdateExcelGrid2Database();

                    GetExcelCart().Clear();
                    GetIncrementCart().Clear();
                    GetDeletedExcelCart().Clear();
                    return RedirectToAction("Index");
                }
                else if (Request.Form["Reset"] != null)
                {
                    System.Console.WriteLine("Reset...");
                    return RedirectToAction("Edit2", new { id = debitNote.ID });
                }

            }
            return View(debitNote);
        }


        // GET: DebitNote/Delete/5
        public ActionResult Delete(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var debitnoteLineModel = new DebitNoteLineModel();

            debitnoteLineModel.DebitNote = debitnoteRepository.GetDebitNoteByID((long)id).FirstOrDefault();
            if (id != null)
            {
                //improve performance
                debitnoteLineModel.DebitNoteLine = debitnoteLineRepository.GetDebitNoteLine("DebitNoteID", (long)id);
            }
            

            if (debitnoteLineModel.DebitNoteLine != null)
            {
                foreach (var item in debitnoteLineModel.DebitNoteLine)
                {
                    DebitNoteLine debitnoteLine = debitnoteLineRepository.DebitNoteLine.FirstOrDefault(p => p.ID == item.ID);
                    debitnoteLineRepository.DeleteDebitNoteLine(debitnoteLine);
                    //UpdateHeaderAmount(debitnoteLine);
                }
            }

            DebitNote debitnote = debitnoteRepository.GetDebitNoteByID((long)id).SingleOrDefault();
            if (debitnote != null)
            {
                debitnoteRepository.DeleteDebitNote(debitnote);
                TempData["DeleteMessage"] = string.Format("{0} was deleted", debitnote.DebitNoteCode);
            }

            return RedirectToAction("Index");
            //DebitNote debitNote = db.DebitNote.Find(id);
            //if (debitNote == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(debitNote);
        }

        public ActionResult DeleteDebitNoteLine(Int64? debitnoteId, Int64? debitnoteLineId)
        {

            DebitNoteLine debitnoteLine = debitnoteLineRepository.DebitNoteLine.FirstOrDefault(p => p.ID == debitnoteLineId);

            if (debitnoteLine != null)
            {
                debitnoteLineRepository.DeleteDebitNoteLine(debitnoteLine);
                UpdateHeaderAmount(debitnoteLine);
                TempData["Message"] = "";
                TempData["DeleteMessage"] = string.Format("{0} was deleted in system !", debitnoteLine.DebitNoteLineCode);
            }

            return RedirectToAction("Edit2", new { id = debitnoteId });
        }

        private void UpdateHeaderAmount(DebitNoteLine debitNoteLine)
        {
            long headerId = debitNoteLine.DebitNoteID;
            var model = (from p in debitnoteLineRepository.DebitNoteLine
                         where p.DebitNoteID.Equals(headerId)
                         select p);
            double dTotalAmount = model.Sum(p => p.Total);
            DebitNote debitNote = debitnoteRepository.GetDebitNoteByID(headerId).FirstOrDefault(p => p.ID.Equals(headerId));
            if (debitNote != null)
            {
                debitNote.TotalAmount = dTotalAmount;
                debitnoteRepository.SaveDebitNote(debitNote);
            }
        }

        private void UpdateHeaderAmount2(long debitnoteId)
        {
            var model = (from p in debitnoteLineRepository.DebitNoteLine
                         where p.DebitNoteID.Equals(debitnoteId)
                         select p);
            double dTotalAmount = model.Sum(p => p.Total);
            DebitNote debitNote = debitnoteRepository.GetDebitNoteByID(debitnoteId).FirstOrDefault(p=>p.ID.Equals(debitnoteId));
            if (debitNote != null)
            {
                debitNote.TotalAmount = dTotalAmount;
                debitnoteRepository.SaveDebitNote(debitNote);
            }
        }

        private IEnumerable<SelectListItem> GetSearchField(string defaultValue = "")
        {
            var list = new SelectList(new[]
            {
                new { ID = "DebitNoteCode", Name = "DebitNoteCode" },
                new { ID = "CustRef", Name = "CustRef" }
            },
            "ID", "Name", defaultValue);
            return list;
        }

        #region "Excel Functions"
        public PartialViewResult LoadDebitNoteLineExcel(Int64? debitnoteId)
        {

            Session["DebitNoteId"] = debitnoteId;

            var q = (from p in debitnoteLineRepository.DebitNoteLine
                     where p.DebitNoteID.Equals(debitnoteId)
                     orderby p.ID ascending
                     select new
                     {
                         mID = p.ID,
                         mDebitNoteID = p.DebitNoteID,
                         mChartOfAccountID = p.ChartOfAccountID,
                         mDebitNoteLineCode = p.DebitNoteLineCode,
                         mDebitNoteCode = p.DebitNoteCode,
                         mAccCode = p.AccCode,
                         mDescription = p.Description,
                         mQuantity = p.Quantity,
                         mUnitPrice = p.UnitPrice,
                         mTotal = p.Total
                     }).ToList();//convert to List

            int i = 0;
            foreach (var item in q) //retrieve each item and assign to model
            {
                DebitNoteLineExcelModel lineModel = new DebitNoteLineExcelModel();
                lineModel.ID = item.mID;
                lineModel.DebitNoteID = item.mDebitNoteID;
                lineModel.ChartOfAccountID = item.mChartOfAccountID;
                lineModel.DebitNoteLineCode = item.mDebitNoteLineCode;
                lineModel.DebitNoteCode = item.mDebitNoteCode;
                lineModel.AccCode = item.mAccCode;
                lineModel.Description = item.mDescription;
                lineModel.Quantity = item.mQuantity;
                lineModel.UnitPrice = item.mUnitPrice;
                lineModel.Total = item.mTotal;
                lineModel.IsNew = false;
                GetExcelCart().AddExcelLine(lineModel);
                PopulateAccountDropDownListExcel(i, item.mChartOfAccountID);
                i++;
            }
            

            if (GetIncrementCart().GetCurrentNo() == 0)
            {
                long? lMax = debitnoteLineRepository.DebitNoteLine.Max(x => (long?)x.ID);
                GetIncrementCart().InitializeNumber(lMax + 1);
            }

            GetExcelCart().AddExcelLine(GetEmptyExcelLine());
            
            PopulateAccountDropDownListExcel(i);
            //return PartialView(model.ToList());
            return PartialView(GetExcelCart().ExcelLines.ToList());
        }

        private DebitNoteLineExcelModel GetEmptyExcelLine()
        {
            DebitNoteLineExcelModel emptyExcelLine = new DebitNoteLineExcelModel();
            emptyExcelLine.ID = 0;
            emptyExcelLine.DebitNoteID = Int64.Parse(Session["DebitNoteId"].ToString());
            emptyExcelLine.ChartOfAccountID = 0;
            emptyExcelLine.DebitNoteLineCode = "";
            emptyExcelLine.DebitNoteCode = "*";
            emptyExcelLine.AccCode = "";
            emptyExcelLine.Description = "";
            emptyExcelLine.Quantity = 0;
            emptyExcelLine.UnitPrice = 0;
            emptyExcelLine.Total = 0;
            emptyExcelLine.IsNew = true;
            return emptyExcelLine;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadDebitNoteLineExcel(List<DebitNoteLineExcelModel> excelData, string btnID)
        {
            bool bNeedNewRow = false;
            int i = 0;
            ModelState.Clear();
            if (excelData != null)
            {
                foreach (var item in excelData)
                {
                    if (item.ID > 0 && item.IsNew == false)
                    {
                        //Edit excel row
                        DebitNoteLine dnl = (from p in debitnoteLineRepository.DebitNoteLine
                                             where p.ID.Equals(item.ID)
                                             select p).FirstOrDefault();
                        int iChartOfAcc = Int32.Parse(Request.Form["ChartOfAcc" + i].ToString());
                        item.ChartOfAccountID = iChartOfAcc;
                        ChartOfAccount chartOfAccount = chartOfAccountRepository.ChartOfAccount.FirstOrDefault(p => p.ID == iChartOfAcc);
                        item.Total = item.Quantity * item.UnitPrice;

                        var excelLineUpdate = GetExcelCart().ExcelLines.FirstOrDefault(p => p.ID.Equals(item.ID));
                        if (excelLineUpdate != null)
                        {
                            excelLineUpdate.ChartOfAccountID = iChartOfAcc;
                            excelLineUpdate.AccCode = chartOfAccount.AccCode.Trim();
                            excelLineUpdate.Description = item.Description;
                            excelLineUpdate.Quantity = item.Quantity;
                            excelLineUpdate.UnitPrice = item.UnitPrice;
                            excelLineUpdate.Total = item.Quantity * item.UnitPrice;
                        }
                        PopulateAccountDropDownListExcel(i, iChartOfAcc);
                    }
                    else
                    {
                        if (!CommonUtility.Null2Empty(item.DebitNoteLineCode).Equals("") && !CommonUtility.Null2Empty(item.Description).Equals("") && item.Quantity != 0 && item.UnitPrice != 0)
                        {   //Add excel row
                            item.DebitNoteCode = CommonUtility.Null2Empty(Session["DebitNoteCode"]);// change from * to actual code
                            int iChartOfAcc = Int32.Parse(Request.Form["ChartOfAcc" + i].ToString());
                            item.ChartOfAccountID = iChartOfAcc;
                            ChartOfAccount chartOfAccount = chartOfAccountRepository.ChartOfAccount.FirstOrDefault(p => p.ID == iChartOfAcc);
                            item.Total = item.Quantity * item.UnitPrice;
                            var excelLineNew = GetExcelCart().ExcelLines.FirstOrDefault(p => p.ID.Equals(item.ID));
                            if (excelLineNew != null)
                            {
                                item.ID = GetIncrementCart().GetNewNo();
                                excelLineNew.ID = item.ID;
                                excelLineNew.DebitNoteID = Int64.Parse(Session["DebitNoteId"].ToString());
                                excelLineNew.DebitNoteCode = CommonUtility.Null2Empty(Session["DebitNoteCode"]);
                                excelLineNew.DebitNoteLineCode = item.DebitNoteLineCode;
                                excelLineNew.ChartOfAccountID = iChartOfAcc;
                                excelLineNew.AccCode = chartOfAccount.AccCode.Trim();
                                excelLineNew.Description = item.Description;
                                excelLineNew.Quantity = item.Quantity;
                                excelLineNew.UnitPrice = item.UnitPrice;
                                excelLineNew.Total = item.Quantity * item.UnitPrice;
                                excelLineNew.IsNew = true;
                            }
                            
                            //GetExcelCart().AddExcelLine(excelLineNew);

                            PopulateAccountDropDownListExcel(i, iChartOfAcc);
                            bNeedNewRow = true;
                        }
                        else
                        {
                            //For last unused row
                            PopulateAccountDropDownListExcel(i);
                        }
                        
                    }
                    i++;
                    //Console.Out.WriteLine("AllowRead = " + item.AllowRead);
                }
                UpdateHeaderAmount2(Int64.Parse(Session["DebitNoteId"].ToString()));
            }


            if (bNeedNewRow || excelData == null)
            {
                if (excelData == null)
                {
                    List<DebitNoteLineExcelModel> excelData2 = new List<DebitNoteLineExcelModel>();
                    
                    excelData2.Add(GetEmptyExcelLine());
                    GetExcelCart().AddExcelLine(GetEmptyExcelLine());
                    excelData = excelData2;
                }
                else
                {
                    
                    excelData.Add(GetEmptyExcelLine());
                    GetExcelCart().AddExcelLine(GetEmptyExcelLine());
                }
                
                PopulateAccountDropDownListExcel(i);
            }

            //Handle delete excel line
            if (btnID != null)
            {
                if (excelData != null && btnID.ToLower().Contains("delete"))
                {
                    string[] arrayDelete = btnID.Split('_');
                    long iRecordID = long.Parse(arrayDelete[1]);
                    if (iRecordID > 0)
                    {
                        int j = 0;
                        foreach (var item in excelData)
                        {
                            ClearViewData(j);
                            j++;
                        }

                        var checkExcel = GetExcelCart().ExcelLines.FirstOrDefault(p => p.ID.Equals(iRecordID));
                        if (checkExcel.IsNew.Equals(false))
                        {
                            GetDeletedExcelCart().Add2DeletedCart(checkExcel);
                        }
                        excelData.RemoveAll(x => x.ID.Equals(iRecordID));
                        GetExcelCart().RemoveExcelLine2(iRecordID);
                        int y = 0;
                        foreach (var item in GetExcelCart().ExcelLines)
                        {
                            if (item.ChartOfAccountID > 0)
                            {
                                PopulateAccountDropDownListExcel(y, item.ChartOfAccountID);
                            }
                            else
                            {
                                PopulateAccountDropDownListExcel(y);
                            }
                            y++;
                        }
                    }

                }

            }
            //return PartialView(excelData.ToList());
            return PartialView(GetExcelCart().ExcelLines.ToList());
        }

        private void PopulateAccountDropDownListExcel(int counter, object selectedAcc = null)
        {
            var accQuery = from d in chartOfAccountRepository.ChartOfAccount
                           orderby d.AccCode
                           select d;
            ViewData.Add("ChartOfAcc" + counter, new SelectList(accQuery, "ID", "AccCode", selectedAcc));
            //ViewData.Re
        }

        private void ClearViewData(int counter)
        {
            ViewData.Remove("ChartOfAcc" + counter);
        }

        private ExcelCart GetExcelCart()
        {
            ExcelCart excelCart = (ExcelCart)Session["ExcelCart"];
            if (excelCart == null)
            {
                excelCart = new ExcelCart();
                Session["ExcelCart"] = excelCart;
            }
            return excelCart;
        }

        private IncrementCart GetIncrementCart()
        {
            IncrementCart incrementCart = (IncrementCart)Session["IncrementCart"];
            if (incrementCart == null)
            {
                incrementCart = new IncrementCart();
                Session["IncrementCart"] = incrementCart;
            }
            return incrementCart;
        }

        private DeletedExcelCart GetDeletedExcelCart()
        {
            DeletedExcelCart deletedExcelCart = (DeletedExcelCart)Session["DeletedExcelCart"];
            if (deletedExcelCart == null)
            {
                deletedExcelCart = new DeletedExcelCart();
                Session["DeletedExcelCart"] = deletedExcelCart;
            }
            return deletedExcelCart;
        }
        #endregion
    }
}
