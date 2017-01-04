using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERPDomain.Abstract;
using ERPDomain.Entities;
using ERPDomain.Helpers;
using ERPDomain.Models;
using PagedList;

namespace PluginAP.Controllers
{
    public class PaymentController : BaseController
    {
        #region "Repository Interface"
        private IPaymentRepository paymentRepository;
        private IPaymentLineRepository paymentLineRepository;
        private IChartOfAccountRepository chartOfAccountRepository;
        private IVendorRepository vendorRepository;
        private ISH_USERRepository shUserRepository;
        private IWfProcessRepository wfProcessRepository;
        private IWfProcessAdminRepository wfProcessAdminRepository;
        private IWfRequestRepository wfRequestRepository;
        private IWfRequestDataRepository wfRequestDataRepository;
        private IWfRequestActionRepository wfRequestActionRepository;
        private IWfRequestNoteRepository wfRequestNoteRepository;
        private IWfGroupRepository wfGroupRepository;
        private IWfGroupMemberRepository wfGroupMemberRepository;
        private IWfEscalationRepository wfEscalationRepository;
        private IDfMasterRepository dfMasterRepository;
        private IDfMasterDataRepository dfMasterDataRepository;
        private IDfRequestRepository dfRequestRepository;
        private IDfRequestDataRepository dfRequestDataRepository;
        private IDfExemptItemRepository dfExemptItemRepository;
        private IDfAppliedModuleRepository dfAppliedModuleRepository;
        private bool bApplyGstCharges = false;
        #endregion

        public PaymentController(IPaymentRepository paymentRepo, IPaymentLineRepository paymentLineRepo, IChartOfAccountRepository chartOfAccountRepo, IVendorRepository vendorRepo, ISH_USERRepository shUserRepo, IWfProcessRepository wfProcessRepo, IWfProcessAdminRepository wfProcessAdminRepo, IWfRequestRepository wfRequestRepo, IWfRequestDataRepository wfRequestDataRepo, IWfRequestActionRepository wfRequestActionRepo, IWfRequestNoteRepository wfRequestNoteRepo, IWfGroupRepository wfGroupRepo, IWfGroupMemberRepository wfGroupMemberRepo, IWfEscalationRepository wfEscalationRepo, IDfMasterRepository dfMasterRepo, IDfMasterDataRepository dfMasterDataRepo, IDfRequestRepository dfRequestRepo, IDfRequestDataRepository dfRequestDataRepo, IDfExemptItemRepository dfExemptItemRepo, IDfAppliedModuleRepository dfAppliedModuleRepo, IErrorLogRepository errorLogRepo) : base(errorLogRepo)
        {
            this.paymentRepository = paymentRepo;
            this.paymentLineRepository = paymentLineRepo;
            this.chartOfAccountRepository = chartOfAccountRepo;
            this.vendorRepository = vendorRepo;
            this.shUserRepository = shUserRepo;
            this.wfProcessRepository = wfProcessRepo;
            this.wfProcessAdminRepository = wfProcessAdminRepo;
            this.wfRequestRepository = wfRequestRepo;
            this.wfRequestDataRepository = wfRequestDataRepo;
            this.wfRequestActionRepository = wfRequestActionRepo;
            this.wfRequestNoteRepository = wfRequestNoteRepo;
            this.wfGroupRepository = wfGroupRepo;
            this.wfGroupMemberRepository = wfGroupMemberRepo;
            this.wfEscalationRepository = wfEscalationRepo;
            this.dfMasterRepository = dfMasterRepo;
            this.dfMasterDataRepository = dfMasterDataRepo;
            this.dfRequestRepository = dfRequestRepo;
            this.dfRequestDataRepository = dfRequestDataRepo;
            this.dfExemptItemRepository = dfExemptItemRepo;
            this.dfAppliedModuleRepository = dfAppliedModuleRepo;

            DfAppliedModule dfAppliedModule = dfAppliedModuleRepository.DfAppliedModule.FirstOrDefault(p => p.DfMasterID.Equals(1) && p.AppliedModule.Equals("Payment"));
            if (dfAppliedModule != null)
            {
                bApplyGstCharges = true;
            }
            else
            {
                bApplyGstCharges = false;
            }
        }

        // GET: Payment
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

            var model = (from m in paymentRepository.Payment
                         select m);

            Int64? iRecCnt = CommonUtility.Null2LongZero(paymentRepository.GetMaxID());

            if (!String.IsNullOrEmpty(searchValue))
            {
                model = (from m in paymentRepository.PaymentWildSearch("PaymentCode", searchValue)
                         select m);
                iRecCnt = model.Count();
            }
            else
            {
                //model = model.Skip(((int)page - 1) * pageSize)
                //       .Take(pageSize);
                //improve performance
                model = paymentRepository.GetPaymentPaging(((((long)page - 1) * pageSize) + 1), (long)page * pageSize);
            }

            switch (sortOrder)
            {
                default:
                    //model = model.OrderBy(s => s.ID);
                    break;
            }

            foreach(var item in model)
            {
                long lPaymentID = item.ID;
                item.WorkflowStatus = GetWorkflowStatus(lPaymentID);
            }
            //return View(model.ToPagedList(pageNumber, pageSize));
            //improve performance
            var model3 = new StaticPagedList<Payment>(model, (int)page, pageSize, (int)iRecCnt);
            

            return View(model3);
            //return View(db.Payment.ToList());
        }

        // GET: Payment/Details/5
        public ActionResult Details(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from p in paymentRepository.Payment
                         where p.ID.Equals(id)
                         orderby p.PaymentCode
                         select p).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView(model);
        }

        // GET: Payment/Create
        public ActionResult Create()
        {
            TempData["Message"] = "";
            TempData["DeteleMessage"] = "";
            Session["PaymentCode"] = "";
            //Session["PaymentID"] = "";
            Session["PaymentLineId"] = "";
            Session["PaymentLineCode"] = "";
            PopulateVendorDropDownList();
            PaymentLineModel model = new PaymentLineModel();
            //model.Payment.TotalAmount = 0;
            //model.Payment.TotalGstAmount = 0;
            return View(model);
            //return View();
        }

        // POST: Payment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,VendorID,BankID,CurrencyID,PaymentCode,VendorCode,PaymentType,BankCode,ChequeNo,TotalAmount,Remark,LocCode,AccMonth,AccYear,Status,CreateDate,UpdateDate,PrintDate,UpdateID,ChequePrintDate,OutstandingAmount,CreditBankCode,CreditDate,PostDate,ProposalID,CurrencyRate,CurrencyAmount,CurrencyOutAmount,ChequeDate,PaymentRefNo,PaymentRefDate")] Payment payment)
        {
            var paymentLineModel = new PaymentLineModel();
            TempData["Message"] = "";
            TempData["DeteleMessage"] = "";

            if (ModelState.IsValid)
            {
                //db.SaveChanges();
                payment.CreateDate = DateTime.Now;
                payment.UpdateDate = DateTime.Now;
                payment.PrintDate = DateTime.Now;
                payment.PostDate = DateTime.Now;
                payment.ChequeDate = DateTime.Now;
                payment.ChequePrintDate = DateTime.Now;
                payment.CreditDate = DateTime.Now;
                int iUserID = Int32.Parse(Session["UserPK"].ToString());
                payment.UserID = iUserID;
                int iVendor = Int32.Parse(Request.Form["Vendor"].ToString());
                payment.VendorID = iVendor;
                
                Vendor vendor = vendorRepository.Vendor.FirstOrDefault(p => p.ID == iVendor);
                payment.VendorCode = vendor.VendorCode.Trim();

                payment.UpdateID = Session["UserEmail"].ToString();
                payment.Status = "Active";
                var paymentRec = from m in paymentRepository.PaymentWildSearch("PaymentCode", payment.PaymentCode)
                                 select m;
                int iRecCnt = paymentRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The Payment Code " + payment.PaymentCode + " already existed in system ! ";
                }
                else
                {
                    try
                    {
                        paymentRepository.SavePayment(payment);
                        UpdateWorkflowCreatePayment(payment);
                        Session["PaymentID"] = payment.ID;
                        Session["PaymentCode"] = payment.PaymentCode;
                        Session["VendorID"] = payment.VendorID;
                        Session["VendorCode"] = payment.VendorCode;

                        paymentLineModel.Payment = (from p in paymentRepository.Payment
                                                    where p.ID.Equals(payment.ID)
                                                    orderby p.PaymentCode
                                                    select p).FirstOrDefault();
                        if (payment.ID != 0)
                        {
                            paymentLineModel.PaymentLine = from p in paymentLineRepository.PaymentLine
                                                           where p.ID.Equals(payment.ID)
                                                           orderby p.PaymentCode
                                                           select p;
                        }
                        Session["PaymentLineID"] = "";
                        TempData["Message"] = string.Format("{0} was added in the system !", payment.PaymentCode);
                    }
                    catch(Exception ex)
                    {
                        System.Console.WriteLine("Ex=" + ex.Message);
                    }
                    
                }
                PopulateVendorDropDownList(payment.VendorID);
                //return RedirectToAction("Index");
            }

            return View(paymentLineModel);
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

        private void PopulateVendorDropDownList(object selectedAcc = null)
        {
            var vendorQuery = from d in vendorRepository.Vendor
                              orderby d.VendorCode
                              select d;
            ViewBag.Vendor = new SelectList(vendorQuery, "ID", "VendorCode",  selectedAcc);
        }

        public ActionResult CreatePaymentAndPaymentLine(string roleName)
        {
            PopulateAccountDropDownList();
            PopulateVendorDropDownList();
            return PartialView(new PaymentLine());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePaymentAndPaymentLine([Bind(Include = "ID,PaymentID,ChartOfAccountID,CurrencyID,PaymentLineCode,PaymentCode,ItemCode,DocType,AccCode,Amount,CurrencyAmount,PrevCurrRate,PrevCurrAmount,PrevAmount")] PaymentLine paymentLine)
        {

            if (ModelState.IsValid)
            {
                int iChartOfAcc = Int32.Parse(Request.Form["ChartOfAcc"].ToString());
                paymentLine.PaymentID = Int32.Parse(Session["PaymentID"].ToString());
                paymentLine.PaymentCode = Session["PaymentCode"].ToString();
                paymentLine.ChartOfAccountID = iChartOfAcc;
                //paymentLine.TaxCodeID = 0;
                ChartOfAccount chartOfAccount = chartOfAccountRepository.ChartOfAccount.FirstOrDefault(p => p.ID == iChartOfAcc);
                paymentLine.AccCode = chartOfAccount.AccCode.Trim();
                var paymentLineRec = from m in paymentLineRepository.PaymentLineWildSearch("PaymentLineCode", paymentLine.PaymentLineCode)
                                     select m;
                int iRecCnt = paymentLineRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The Payment Line Code " + paymentLine.PaymentLineCode + " already existed in system ! ";
                }
                else
                {
                    paymentLineRepository.SavePaymentLine(paymentLine);
                    UpdateHeaderAmount(paymentLine);
                    
                    if (bApplyGstCharges)
                    {
                        DfExemptItem dfExemptItem = dfExemptItemRepository.DfExemptItem.FirstOrDefault(p => p.Code.Equals(paymentLine.ItemCode));
                        if (dfExemptItem == null)
                        {
                            ApplyDynamicFormulaForGstNewLine(paymentLine);
                        }
                        
                    }
                    Session["PaymentLineID"] = paymentLine.ID;
                    Session["PaymentLineCode"] = paymentLine.PaymentLineCode;
                    //return RedirectToAction("Index");

                    TempData["Message"] = string.Format("{0} was added in the system ! ", paymentLine.PaymentLineCode);
                }
                PopulateAccountDropDownList(paymentLine.ChartOfAccountID);
                
                return RedirectToAction("Edit2", new { id = paymentLine.PaymentID });
            }
            return View(paymentLine);
        }

        public ActionResult CreatePaymentLine(Int64? paymentId, string paymentCode)
        {
            Session["PaymentID"] = paymentId;
            Session["PaymentCode"] = paymentCode;
            PopulateAccountDropDownList();
            PopulateVendorDropDownList();
            return PartialView(new PaymentLine());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePaymentLine([Bind(Include = "ID,PaymentID,ChartOfAccountID,CurrencyID,PaymentLineCode,PaymentCode,ItemCode,DocType,AccCode,Amount,CurrencyAmount,PrevCurrRate,PrevCurrAmount,PrevAmount")] PaymentLine paymentLine)
        {
            if (ModelState.IsValid)
            {
                int iChartOfAcc = Int32.Parse(Request.Form["ChartOfAcc"].ToString());
                paymentLine.PaymentID = Int32.Parse(Session["PaymentID"].ToString());
                paymentLine.ChartOfAccountID = iChartOfAcc;
                paymentLine.PaymentCode = Session["PaymentCode"].ToString();
                ChartOfAccount chartOfAccount = chartOfAccountRepository.ChartOfAccount.FirstOrDefault(p => p.ID == iChartOfAcc);
                paymentLine.AccCode = chartOfAccount.AccCode.Trim();
                var paymentLineRec = from m in paymentLineRepository.PaymentLineWildSearch("PaymentLineCode", paymentLine.PaymentLineCode)
                                     select m;
                int iRecCnt = paymentLineRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The Payment Line Code " + paymentLine.PaymentLineCode + " already existed in system ! ";
                }
                else
                {
                    paymentLineRepository.SavePaymentLine(paymentLine);
                    UpdateHeaderAmount(paymentLine);
                     
                    if (bApplyGstCharges)
                    {
                        DfExemptItem dfExemptItem = dfExemptItemRepository.DfExemptItem.FirstOrDefault(p => p.Code.Equals(paymentLine.ItemCode));
                        if (dfExemptItem == null)
                        {
                            ApplyDynamicFormulaForGstNewLine(paymentLine);
                        }
                        
                    }

                    Session["PaymentLineID"] = paymentLine.ID;
                    Session["PaymentLineCode"] = paymentLine.PaymentLineCode;

                    TempData["Message"] = string.Format("{0} was added", paymentLine.PaymentLineCode);
                }
                PopulateAccountDropDownList(paymentLine.ChartOfAccountID);
                
                return RedirectToAction("Edit2", new { id = paymentLine.PaymentID });
            }
            return View(paymentLine);
        }

        private void UpdateHeaderAmount(PaymentLine paymentLine)
        {
            long headerId = paymentLine.PaymentID;
            var model = (from p in paymentLineRepository.PaymentLine
                         where p.PaymentID.Equals(headerId)
                         select p);
            double dTotalAmount = model.Sum(p=>p.Amount);
            var paymentModel = paymentRepository.GetPaymentByID(headerId);
            foreach(var item in paymentModel)
            {
                item.TotalAmount = dTotalAmount;
                paymentRepository.SavePayment(item);
            }

        }

        public ActionResult EditPaymentLine(Int64? paymentId, Int64? paymentLineId, string paymentCode, string paymentLineCode)
        {
            Session["PaymentCode"] = paymentCode;
            Session["PaymentID"] = paymentId;
            Session["PaymentLineId"] = paymentLineId;
            Session["PaymentLineCode"] = paymentLineCode;

            //improve performance
            PaymentLine paymentLine = paymentLineRepository.GetPaymentLineByID((long)paymentLineId).SingleOrDefault();
            PopulateAccountDropDownList(paymentLine.ChartOfAccountID);
            //PopulateVendorDropDownList(Session["VendorID"].ToString());
            return PartialView(paymentLine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPaymentLine([Bind(Include = "ID,PaymentID,ChartOfAccountID,CurrencyID,PaymentLineCode,PaymentCode,ItemCode,DocType,AccCode,Amount,CurrencyAmount,PrevCurrRate,PrevCurrAmount,PrevAmount")] PaymentLine paymentLine)
        {
            if (ModelState.IsValid)
            {
                int iChartOfAcc = Int32.Parse(Request.Form["ChartOfAcc"].ToString());
                paymentLine.PaymentID = Int32.Parse(Session["PaymentID"].ToString());
                paymentLine.ChartOfAccountID = iChartOfAcc;
                paymentLine.PaymentCode = Session["PaymentCode"].ToString();
                ChartOfAccount chartOfAccount = chartOfAccountRepository.ChartOfAccount.FirstOrDefault(p => p.ID == iChartOfAcc);
                paymentLine.AccCode = chartOfAccount.AccCode.Trim();
                paymentLineRepository.SavePaymentLine(paymentLine);
                UpdateHeaderAmount(paymentLine);

                if (bApplyGstCharges)
                {
                    DfExemptItem dfExemptItem = dfExemptItemRepository.DfExemptItem.FirstOrDefault(p => p.Code.Equals(paymentLine.ItemCode));
                    if (dfExemptItem == null)
                    {
                        ApplyDynamicFormulaForGstEditLine(paymentLine);
                    }
                    
                }
                Session["PaymentID"] = paymentLine.PaymentID;
                //return View("Edit");
               
                return RedirectToAction("Edit2", new { id = paymentLine.PaymentID });
            }
            return PartialView(paymentLine);
        }

        // GET: Payment/Edit/5
        public ActionResult Edit(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var paymentLineModel = new PaymentLineModel();
            paymentLineModel.Payment = paymentRepository.GetPaymentByID((long)id).FirstOrDefault();

            if (id != null)
            {
                //improve performance
                paymentLineModel.PaymentLine = paymentLineRepository.GetPaymentLine("PaymentID", (long)id);
                
                if (bApplyGstCharges)
                {
                    
                    foreach (var item in paymentLineModel.PaymentLine)
                    {
                        item.GstPct = GetLineGstPct(item);
                        item.GstAmount = GetLineGstAmount(item);
                        item.AmountAfterGst = GetLineAmount(item) + item.GstAmount;
                    }
                    paymentLineModel.Payment.TotalAmount = GetTotalLineAmount(paymentLineModel);
                    paymentLineModel.Payment.TotalGstAmount = GetTotalGstAmount2(paymentLineModel);
                    //paymentLineModel.Payment.TotalAmountAfterGst = paymentLineModel.Payment.TotalAmount + paymentLineModel.Payment.TotalGstAmount;
                    paymentLineModel.Payment.TotalAmountAfterGst = GetTotalAmountAfterGst(paymentLineModel);
                }
            }

            if (paymentLineModel == null)
            {
                return HttpNotFound();
            }
            PopulateVendorDropDownList(paymentLineModel.Payment.VendorID);
            return PartialView(paymentLineModel);

        }

        // POST: Payment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,VendorID,BankID,CurrencyID,PaymentCode,VendorCode,PaymentType,BankCode,ChequeNo,TotalAmount,Remark,LocCode,AccMonth,AccYear,Status,CreateDate,UpdateDate,PrintDate,UpdateID,ChequePrintDate,OutstandingAmount,CreditBankCode,CreditDate,PostDate,ProposalID,CurrencyRate,CurrencyAmount,CurrencyOutAmount,ChequeDate,PaymentRefNo,PaymentRefDate")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                payment.UpdateDate = DateTime.Now;
                int iVendor = Int32.Parse(Request.Form["Vendor"].ToString());
                payment.VendorID = iVendor;
                Vendor vendor = vendorRepository.Vendor.FirstOrDefault(p => p.ID == iVendor);
                payment.VendorCode = vendor.VendorCode.Trim();
                PopulateVendorDropDownList(payment.VendorID);
                paymentRepository.SavePayment(payment);
                ViewBag.Message = string.Format("{0} was updated", payment.PaymentCode);

                return RedirectToAction("Index");
            }
            return PartialView(payment);
        }

        public ActionResult Edit2(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TempData["Message"] = "";
            var paymentLineModel = new PaymentLineModel();

            //remove the orderBy to improve performance
            paymentLineModel.Payment = paymentRepository.GetPaymentByID((long)id).FirstOrDefault();
            if (id != null)
            {
                //improve performance
                paymentLineModel.PaymentLine = paymentLineRepository.GetPaymentLine("PaymentID", (long)id);

                if (bApplyGstCharges)
                {
                    
                    foreach (var item in paymentLineModel.PaymentLine)
                    {
                        item.GstPct = GetLineGstPct(item);
                        item.GstAmount = GetLineGstAmount(item);
                        item.AmountAfterGst = GetLineAmount(item) + item.GstAmount;
                    }
                    paymentLineModel.Payment.TotalAmount = GetTotalLineAmount(paymentLineModel);
                    paymentLineModel.Payment.TotalGstAmount = GetTotalGstAmount2(paymentLineModel);
                    //paymentLineModel.Payment.TotalAmountAfterGst = paymentLineModel.Payment.TotalAmount + paymentLineModel.Payment.TotalGstAmount;
                    paymentLineModel.Payment.TotalAmountAfterGst = GetTotalAmountAfterGst(paymentLineModel);
                }
            }

            if (paymentLineModel == null)
            {
                return HttpNotFound();
            }
            
            PopulateVendorDropDownList(paymentLineModel.Payment.VendorID);
            return View(paymentLineModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2([Bind(Include = "ID,VendorID,BankID,CurrencyID,PaymentCode,VendorCode,PaymentType,BankCode,ChequeNo,TotalAmount,Remark,LocCode,AccMonth,AccYear,Status,CreateDate,UpdateDate,PrintDate,UpdateID,ChequePrintDate,OutstandingAmount,CreditBankCode,CreditDate,PostDate,ProposalID,CurrencyRate,CurrencyAmount,CurrencyOutAmount,ChequeDate,PaymentRefNo,PaymentRefDate")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                payment.UpdateDate = DateTime.Now;
                paymentRepository.SavePayment(payment);
                ViewBag.Message = string.Format("{0} was updated", payment.PaymentCode);
                return RedirectToAction("Index");
            }
            return View(payment);
        }

        // GET: Payment/Delete/5
        public ActionResult Delete(Int64? id)
        {
            var paymentLineModel = new PaymentLineModel();

            paymentLineModel.Payment = paymentRepository.GetPaymentByID((long)id).FirstOrDefault();
            if (id != null)
            {
                //improve performance
                paymentLineModel.PaymentLine = paymentLineRepository.GetPaymentLine("PaymentID", (long)id);
            }
            Payment payment = paymentRepository.GetPaymentByID((long)id).SingleOrDefault();

            if (paymentLineModel.PaymentLine != null)
            {
                foreach (var item in paymentLineModel.PaymentLine)
                {
                    PaymentLine paymentLine = paymentLineRepository.PaymentLine.FirstOrDefault(p => p.ID == item.ID);
                    paymentLineRepository.DeletePaymentLine(paymentLine);
                    UpdateHeaderAmount(paymentLine);
                }
            }

            if (payment != null)
            {
                paymentRepository.DeletePayment(payment);
                TempData["DeleteMessage"] = string.Format("{0} was deleted", payment.PaymentCode);
            }

            return RedirectToAction("Index");

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

        }

        public ActionResult DeletePaymentLine(Int64? paymentId, Int64? paymentLineId)
        {

            PaymentLine paymentLine = paymentLineRepository.PaymentLine.FirstOrDefault(p => p.ID == paymentLineId);

            if (paymentLine != null)
            {

                if (bApplyGstCharges)
                {
                    DfExemptItem dfExemptItem = dfExemptItemRepository.DfExemptItem.FirstOrDefault(p => p.Code.Equals(paymentLine.ItemCode));
                    if (dfExemptItem == null)
                    {
                        ApplyDynamicFormulaForGstDeleteLine(paymentLine);
                    }
                    
                }
                paymentLineRepository.DeletePaymentLine(paymentLine);
                UpdateHeaderAmount(paymentLine);

                TempData["Message"] = "";
                TempData["DeleteMessage"] = string.Format("{0} was deleted", paymentLine.PaymentLineCode);
            }

            return RedirectToAction("Edit2", new { id = paymentId });
        }

        #region "Workflow"
        private void UpdateWorkflowCreatePayment(Payment payment)
        {
            //Update table WfRequest
            WfRequest wfRequest = new WfRequest();
            wfRequest.WfProcessID = 1; //1=Payment Process
            wfRequest.Title = "Payment Request";
            wfRequest.DateRequested = DateTime.Now;
            wfRequest.SH_USERID = Int32.Parse(Session["UserPK"].ToString());
            wfRequest.CurrentStateID = 1;//1=Start Payment
            wfRequest.CreateDate = DateTime.Now;
            wfRequest.UpdateDate = DateTime.Now;
            wfRequestRepository.SaveWfRequest(wfRequest);

            //Update table WfRequestData 1st record: PaymentID
            WfRequestData wfRequestData = new WfRequestData();
            wfRequestData.WfRequestID = wfRequest.ID;
            wfRequestData.Name = "PaymentID";
            wfRequestData.Value = payment.ID.ToString();
            wfRequestDataRepository.SaveWfRequestData(wfRequestData);

            WfEscalation wfEscalation = wfEscalationRepository.WfEscalation.FirstOrDefault(p => p.SH_USERID == Int32.Parse(Session["UserPK"].ToString()));
            int iWfEscalationGroupID = wfEscalation.EscalationGroupID;
            double dWfMandate = wfEscalation.Mandate;

            //Update table WfRequestData 2nd record: EscalationGroup
            WfRequestData wfRequestData2 = new WfRequestData();
            wfRequestData2.WfRequestID = wfRequest.ID;
            wfRequestData2.Name = "EscalationGroupID";
            wfRequestData2.Value = iWfEscalationGroupID.ToString();
            wfRequestDataRepository.SaveWfRequestData(wfRequestData2);

            //Update table WfRequestData 3rd record: UOM
            WfRequestData wfRequestData3 = new WfRequestData();
            wfRequestData3.WfRequestID = wfRequest.ID;
            wfRequestData3.Name = "UOM";
            wfRequestData3.Value = "RM";
            wfRequestDataRepository.SaveWfRequestData(wfRequestData3);

            //Update table WfRequestAction
            WfRequestAction wfRequestAction = new WfRequestAction();
            wfRequestAction.WfRequestID = wfRequest.ID;
            wfRequestAction.WfActionID = 1; //1=Start
            wfRequestAction.WfTransitionID = 1;//1=Start->Submit
            wfRequestAction.IsActive = true;
            wfRequestAction.IsComplete = false;
            wfRequestAction.UserID = Int32.Parse(Session["UserPK"].ToString());
            wfRequestAction.CreateDate = DateTime.Now;
            wfRequestAction.UpdateDate = DateTime.Now;
            wfRequestActionRepository.SaveWfRequestAction(wfRequestAction);
            if (payment.TotalAmount > dWfMandate)
            {

            }

        }

        public ActionResult WorkflowApprove(long id)
        {
            var modelPayment = paymentRepository.GetPaymentByID(id);
            foreach (var item in modelPayment)
            {
                Session["PaymentID"] = item.ID.ToString();
                break;
            }
            //Payment payment = paymentRepository.Payment.FirstOrDefault(p => p.ID == id);

            UserViewModel model = new UserViewModel();
            model.LoginEmail = Session["UserEmail"].ToString();
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WorkflowApprove(UserViewModel model, string approve, string deny, string cancel)
        {
            try
            {
                SH_USER shUser = shUserRepository.SH_USER.FirstOrDefault(p => p.UserEmail.Equals(model.LoginEmail));
                string sDbPassw = shUser.UserPwd;
                if (!sDbPassw.Equals(model.LoginPassword) && (!approve.Equals("") || !deny.Equals("")))
                {
                    ViewBag.Message = "Invalid password !";
                    return PartialView();
                }
                if (approve.ToUpper().Equals("APPROVE"))
                {
                    UpdateWorkflowApprovePayment();
                    ViewBag.Message = "Payment approved !";
                }
                if (deny.ToUpper().Equals("DENY"))
                {
                    UpdateWorkflowDenyApprovePayment();
                    ViewBag.Message = "Payment approval denied !";
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }

            return Json(new { success = true });
        }


        public ActionResult WorkflowRecommend(long id)
        {
            var modelPayment = paymentRepository.GetPaymentByID(id);
            foreach (var item in modelPayment)
            {
                Session["PaymentID"] = item.ID.ToString();
                break;
            }
            //Payment payment = paymentRepository.Payment.FirstOrDefault(p => p.ID == id);

            UserViewModel model = new UserViewModel();
            model.LoginEmail = Session["UserEmail"].ToString();
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WorkflowRecommend(UserViewModel model, string recommend, string deny, string cancel)
        {
            try
            {
                SH_USER shUser = shUserRepository.SH_USER.FirstOrDefault(p => p.UserEmail.Equals(model.LoginEmail));
                string sDbPassw = shUser.UserPwd;
                if (!sDbPassw.Equals(model.LoginPassword) && (!recommend.Equals("") || !deny.Equals("")))
                {
                    ViewBag.Message = "Invalid password !";
                    return PartialView();
                }
                if (recommend.ToUpper().Equals("RECOMMEND"))
                {
                    UpdateWorkflowRecommendPayment();
                    ViewBag.Message = "Payment recommended !";
                }
                if (deny.ToUpper().Equals("DENY"))
                {
                    UpdateWorkflowDenyRecommendPayment();
                    ViewBag.Message = "Payment recommendation denied !";
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }

            return Json(new { success = true });
        }

        public ActionResult WorkflowSubmit(long id)
        {
            var modelPayment = paymentRepository.GetPaymentByID(id);
            foreach(var item in modelPayment)
            {
                Session["PaymentID"] = item.ID.ToString();
                break;
            }
            //Payment payment = paymentRepository.Payment.FirstOrDefault(p => p.ID == id);

            UserViewModel model = new UserViewModel();
            model.LoginEmail = Session["UserEmail"].ToString();
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WorkflowSubmit(UserViewModel model, string save, string cancel)
        {
            try
            {
                SH_USER shUser = shUserRepository.SH_USER.FirstOrDefault(p => p.UserEmail.Equals(model.LoginEmail));
                string sDbPassw = shUser.UserPwd;
                if (!sDbPassw.Equals(model.LoginPassword) && !save.Equals(""))
                {
                    ViewBag.Message = "Invalid password !";
                    return PartialView();
                }
                if (save.ToUpper().Equals("SAVE"))
                {
                    UpdateWorkflowSubmitPayment();
                    ViewBag.Message = "Payment submitted !";
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }

            return Json(new { success = true });
        }

        private void UpdateWorkflowSubmitPayment()
        {
            long lWfRequestID = 0;
            int iShUserID = 0;
            long lEscalationGroupID = 0;
            WfRequestData wfRequestData = wfRequestDataRepository.WfRequestData.FirstOrDefault(p => p.Name.Equals("PaymentID") && p.Value.Equals(Session["PaymentID"].ToString()));
            if (wfRequestData != null)
            {
                lWfRequestID = wfRequestData.WfRequestID;
                WfRequest wfRequest = wfRequestRepository.WfRequest.FirstOrDefault(p => p.ID.Equals(lWfRequestID));
                iShUserID = wfRequest.SH_USERID;
                WfEscalation wfEscalation = wfEscalationRepository.WfEscalation.FirstOrDefault(p => p.SH_USERID.Equals(iShUserID));
                lEscalationGroupID = wfEscalation.EscalationGroupID;
                var raModel = (from p in wfRequestActionRepository.WfRequestAction
                               where p.WfRequestID.Equals(lWfRequestID) && p.WfTransitionID.Equals(1) 
                               select p);
                foreach (var item in raModel)
                {
                    WfRequestAction wfRequestAction = wfRequestActionRepository.WfRequestAction.FirstOrDefault(p => p.ID.Equals(item.ID));
                    wfRequestAction.IsActive = false;
                    wfRequestAction.IsComplete = true;
                    wfRequestAction.UpdateDate = DateTime.Now;
                    wfRequestActionRepository.SaveWfRequestAction(wfRequestAction);
                }
                WfRequestAction wfRequestAction2 = new WfRequestAction();
                wfRequestAction2.WfRequestID = lWfRequestID;
                wfRequestAction2.WfActionID = 2; //Action=Submit
                wfRequestAction2.WfTransitionID = 3; //Submit->Recommendation
                wfRequestAction2.IsActive = true;
                wfRequestAction2.IsComplete = false;
                wfRequestAction2.UserID = Int32.Parse(Session["UserPK"].ToString());
                wfRequestAction2.CreateDate = DateTime.Now;
                wfRequestAction2.UpdateDate = DateTime.Now;
                wfRequestActionRepository.SaveWfRequestAction(wfRequestAction2);
            }
        }

        private void UpdateWorkflowApprovePayment()
        {
            long lWfRequestID = 0;
            int iShUserID = 0;
            long lEscalationGroupID = 0;
            WfRequestData wfRequestData = wfRequestDataRepository.WfRequestData.FirstOrDefault(p => p.Name.Equals("PaymentID") && p.Value.Equals(Session["PaymentID"].ToString()));
            if (wfRequestData != null)
            {
                lWfRequestID = wfRequestData.WfRequestID;
                WfRequest wfRequest = wfRequestRepository.WfRequest.FirstOrDefault(p => p.ID.Equals(lWfRequestID));
                iShUserID = wfRequest.SH_USERID;
                WfEscalation wfEscalation = wfEscalationRepository.WfEscalation.FirstOrDefault(p => p.SH_USERID.Equals(iShUserID));
                lEscalationGroupID = wfEscalation.EscalationGroupID;
                var raModel = (from p in wfRequestActionRepository.WfRequestAction
                               where p.WfRequestID.Equals(lWfRequestID) && p.WfTransitionID.Equals(5) //Recommendation to approval
                               select p);
                foreach (var item in raModel)
                {
                    WfRequestAction wfRequestAction = wfRequestActionRepository.WfRequestAction.FirstOrDefault(p => p.ID.Equals(item.ID));
                    wfRequestAction.IsActive = false;
                    wfRequestAction.IsComplete = true;
                    wfRequestAction.UpdateDate = DateTime.Now;
                    wfRequestActionRepository.SaveWfRequestAction(wfRequestAction);
                }
                WfRequestAction wfRequestAction2 = new WfRequestAction();
                wfRequestAction2.WfRequestID = lWfRequestID;
                wfRequestAction2.WfActionID = 5; //Action=Approve
                wfRequestAction2.WfTransitionID = 8; //Approoval->Close
                wfRequestAction2.IsActive = true;
                wfRequestAction2.IsComplete = false;
                wfRequestAction2.UserID = Int32.Parse(Session["UserPK"].ToString());
                wfRequestAction2.CreateDate = DateTime.Now;
                wfRequestAction2.UpdateDate = DateTime.Now;
                wfRequestActionRepository.SaveWfRequestAction(wfRequestAction2);
            }
        }

        private void UpdateWorkflowDenyApprovePayment()
        {
            long lWfRequestID = 0;
            int iShUserID = 0;
            long lEscalationGroupID = 0;
            WfRequestData wfRequestData = wfRequestDataRepository.WfRequestData.FirstOrDefault(p => p.Name.Equals("PaymentID") && p.Value.Equals(Session["PaymentID"].ToString()));
            if (wfRequestData != null)
            {
                lWfRequestID = wfRequestData.WfRequestID;
                WfRequest wfRequest = wfRequestRepository.WfRequest.FirstOrDefault(p => p.ID.Equals(lWfRequestID));
                iShUserID = wfRequest.SH_USERID;
                WfEscalation wfEscalation = wfEscalationRepository.WfEscalation.FirstOrDefault(p => p.SH_USERID.Equals(iShUserID));
                lEscalationGroupID = wfEscalation.EscalationGroupID;
                var raModel = (from p in wfRequestActionRepository.WfRequestAction
                               where p.WfRequestID.Equals(lWfRequestID) && p.WfTransitionID.Equals(5) //Recommendation to approval
                               select p);
                foreach (var item in raModel)
                {
                    WfRequestAction wfRequestAction = wfRequestActionRepository.WfRequestAction.FirstOrDefault(p => p.ID.Equals(item.ID));
                    wfRequestAction.IsActive = false;
                    wfRequestAction.IsComplete = true;
                    wfRequestAction.UpdateDate = DateTime.Now;
                    wfRequestActionRepository.SaveWfRequestAction(wfRequestAction);
                }

                WfRequestAction wfRequestAction2 = new WfRequestAction();
                wfRequestAction2.WfRequestID = lWfRequestID;
                wfRequestAction2.WfActionID = 3; //Action=Deny
                wfRequestAction2.WfTransitionID = 9; //Approval->Deny
                wfRequestAction2.IsActive = false;
                wfRequestAction2.IsComplete = true;
                wfRequestAction2.UserID = Int32.Parse(Session["UserPK"].ToString());
                wfRequestAction2.CreateDate = DateTime.Now;
                wfRequestAction2.UpdateDate = DateTime.Now;
                wfRequestActionRepository.SaveWfRequestAction(wfRequestAction2);

                WfRequestAction wfRequestAction3 = new WfRequestAction();
                wfRequestAction3.WfRequestID = lWfRequestID;
                wfRequestAction3.WfActionID = 1; //Action=Start
                wfRequestAction3.WfTransitionID = 1; //Start->Submit
                wfRequestAction3.IsActive = true;
                wfRequestAction3.IsComplete = false;
                wfRequestAction3.UserID = Int32.Parse(Session["UserPK"].ToString());
                wfRequestAction3.CreateDate = DateTime.Now;
                wfRequestAction3.UpdateDate = DateTime.Now;
                wfRequestActionRepository.SaveWfRequestAction(wfRequestAction3);
            }
        }
        private void UpdateWorkflowRecommendPayment()
        {
            long lWfRequestID = 0;
            int iShUserID = 0;
            long lEscalationGroupID = 0;
            WfRequestData wfRequestData = wfRequestDataRepository.WfRequestData.FirstOrDefault(p => p.Name.Equals("PaymentID") && p.Value.Equals(Session["PaymentID"].ToString()));
            if (wfRequestData != null)
            {
                lWfRequestID = wfRequestData.WfRequestID;
                WfRequest wfRequest = wfRequestRepository.WfRequest.FirstOrDefault(p => p.ID.Equals(lWfRequestID));
                iShUserID = wfRequest.SH_USERID;
                WfEscalation wfEscalation = wfEscalationRepository.WfEscalation.FirstOrDefault(p => p.SH_USERID.Equals(iShUserID));
                lEscalationGroupID = wfEscalation.EscalationGroupID;

                var raModel = (from p in wfRequestActionRepository.WfRequestAction
                               where p.WfRequestID.Equals(lWfRequestID) && p.WfTransitionID.Equals(3) //submit to Recommendation
                               select p);
                foreach (var item in raModel)
                {
                    WfRequestAction wfRequestAction = wfRequestActionRepository.WfRequestAction.FirstOrDefault(p => p.ID.Equals(item.ID));
                    wfRequestAction.IsActive = false;
                    wfRequestAction.IsComplete = true;
                    wfRequestAction.UpdateDate = DateTime.Now;
                    wfRequestActionRepository.SaveWfRequestAction(wfRequestAction);
                }

                WfRequestAction wfRequestAction2 = new WfRequestAction();
                wfRequestAction2.WfRequestID = lWfRequestID;
                wfRequestAction2.WfActionID = 4; //Action=Recommend
                wfRequestAction2.WfTransitionID = 5; //Recommendation->Approval
                wfRequestAction2.IsActive = true;
                wfRequestAction2.IsComplete = false;
                wfRequestAction2.UserID = Int32.Parse(Session["UserPK"].ToString());
                wfRequestAction2.CreateDate = DateTime.Now;
                wfRequestAction2.UpdateDate = DateTime.Now;
                wfRequestActionRepository.SaveWfRequestAction(wfRequestAction2);
            }
        }

        private void UpdateWorkflowDenyRecommendPayment()
        {
            long lWfRequestID = 0;
            int iShUserID = 0;
            long lEscalationGroupID = 0;
            WfRequestData wfRequestData = wfRequestDataRepository.WfRequestData.FirstOrDefault(p => p.Name.Equals("PaymentID") && p.Value.Equals(Session["PaymentID"].ToString()));
            if (wfRequestData != null)
            {
                lWfRequestID = wfRequestData.WfRequestID;
                WfRequest wfRequest = wfRequestRepository.WfRequest.FirstOrDefault(p => p.ID.Equals(lWfRequestID));
                iShUserID = wfRequest.SH_USERID;
                WfEscalation wfEscalation = wfEscalationRepository.WfEscalation.FirstOrDefault(p => p.SH_USERID.Equals(iShUserID));
                lEscalationGroupID = wfEscalation.EscalationGroupID;
                
                var raModel = (from p in wfRequestActionRepository.WfRequestAction
                               where p.WfRequestID.Equals(lWfRequestID) && p.WfTransitionID.Equals(3) //Submit to Recommendation
                               select p);
                foreach(var item in raModel)
                {
                    WfRequestAction wfRequestAction = wfRequestActionRepository.WfRequestAction.FirstOrDefault(p => p.ID.Equals(item.ID));
                    wfRequestAction.IsActive = false;
                    wfRequestAction.IsComplete = true;
                    wfRequestAction.UpdateDate = DateTime.Now;
                    wfRequestActionRepository.SaveWfRequestAction(wfRequestAction);
                }
                

                WfRequestAction wfRequestAction2 = new WfRequestAction();
                wfRequestAction2.WfRequestID = lWfRequestID;
                wfRequestAction2.WfActionID = 3; //Action=Deny
                wfRequestAction2.WfTransitionID = 4; //Recommendation->Deny
                wfRequestAction2.IsActive = false;
                wfRequestAction2.IsComplete = true;
                wfRequestAction2.UserID = Int32.Parse(Session["UserPK"].ToString());
                wfRequestAction2.CreateDate = DateTime.Now;
                wfRequestAction2.UpdateDate = DateTime.Now;
                wfRequestActionRepository.SaveWfRequestAction(wfRequestAction2);

                WfRequestAction wfRequestAction3 = new WfRequestAction();
                wfRequestAction3.WfRequestID = lWfRequestID;
                wfRequestAction3.WfActionID = 1; //Action=Start
                wfRequestAction3.WfTransitionID = 1; //Start->Submit
                wfRequestAction3.IsActive = true;
                wfRequestAction3.IsComplete = false;
                wfRequestAction3.UserID = Int32.Parse(Session["UserPK"].ToString());
                wfRequestAction3.CreateDate = DateTime.Now;
                wfRequestAction3.UpdateDate = DateTime.Now;
                wfRequestActionRepository.SaveWfRequestAction(wfRequestAction3);
            }
        }

        private string GetWorkflowStatus(long paymentId)
        {
            string workflowStatus = "No Action";
            long lWfRequestID = 0;
            int iWfActionID = 0;

            WfRequestData wfRequestData = wfRequestDataRepository.WfRequestData.FirstOrDefault(p => p.Name.Equals("PaymentID") && p.Value.Equals(paymentId.ToString()));
            if (wfRequestData != null)
            {
                lWfRequestID = wfRequestData.WfRequestID;
                
                WfRequestAction wfRequestAction = wfRequestActionRepository.WfRequestAction.FirstOrDefault(p => p.WfRequestID.Equals(lWfRequestID) && p.IsActive.Equals(true));
                if (wfRequestAction != null)
                {
                    iWfActionID = wfRequestAction.WfActionID;
                    if (iWfActionID == 1)
                    {
                        workflowStatus = "Started";
                    }
                    else if (iWfActionID == 2)
                    {
                        workflowStatus = "Submitted";
                    }
                    else if (iWfActionID == 3)
                    {
                        workflowStatus = "Denied";
                    }
                    else if (iWfActionID == 4)
                    {
                        workflowStatus = "Recommended";
                    }
                    else if (iWfActionID == 5)
                    {
                        workflowStatus = "Approved";
                    }
                    else if (iWfActionID == 6)
                    {
                        workflowStatus = "Cancelled";
                    }
                    else if (iWfActionID == 7)
                    {
                        workflowStatus = "Completed";
                    }
                    else
                    {
                        workflowStatus = "No Action";
                    }
                }
                else
                {
                    workflowStatus = "No Action";
                }
                
            }
            return workflowStatus;
        }

        public ActionResult WorkflowInboxIndex(string searchValue, string sortOrder, string currentFilter, int? page)
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

            int pageSize = Int32.Parse(CommonUtility.Empty2Zero(System.Web.Configuration.WebConfigurationManager.AppSettings["RowPerPage"]));
            int pageNumber = (page ?? 1);
            if (page == null)
            {
                page = 1;
            }

            var model = (from m in paymentRepository.Payment
                         select m);

            Int64? iRecCnt = CommonUtility.Null2LongZero(paymentRepository.GetMaxID());

            if (!String.IsNullOrEmpty(searchValue))
            {
                model = (from m in paymentRepository.PaymentWildSearch("PaymentCode", searchValue)
                         select m);
                iRecCnt = model.Count();
            }
            else
            {
                //model = model.Skip(((int)page - 1) * pageSize)
                //       .Take(pageSize);
                //improve performance
                model = paymentRepository.GetPaymentPaging(((((long)page - 1) * pageSize) + 1), (long)page * pageSize);
            }

            switch (sortOrder)
            {
                default:
                    //model = model.OrderBy(s => s.ID);
                    break;
            }

           
            //return View(model.ToPagedList(pageNumber, pageSize));
            //improve performance
            var model3 = new StaticPagedList<Payment>(model, (int)page, pageSize, (int)iRecCnt);

            return View(model3);
        }

        //This method handles both Recommendation and Approval
        public PartialViewResult LoadPaymentForApproval(long paymentId)
        {
            int iEscalationGroupID = 0;
            long lWfRequestID = 0;
            WorkflowPaymentModel workflowPaymentModel = new WorkflowPaymentModel();
            WfRequestData wfRequestData = wfRequestDataRepository.WfRequestData.FirstOrDefault(p => p.Name.Equals("PaymentID") && p.Value.Equals(paymentId.ToString()));
            if (wfRequestData == null)
            {
                return PartialView(workflowPaymentModel);
            }
            lWfRequestID = wfRequestData.WfRequestID;
            WfRequestAction wfRequestAction = wfRequestActionRepository.WfRequestAction.FirstOrDefault(p => p.WfRequestID.Equals(lWfRequestID) && p.IsComplete.Equals(false));
            if (wfRequestAction == null)
            {
                return PartialView(workflowPaymentModel);
            }
            int iLastActionUserID = wfRequestAction.UserID;
            
            var paymentModel = paymentRepository.GetPaymentByID(paymentId);
            foreach (var item in paymentModel)
            {
                item.WorkflowStatus = GetWorkflowStatus(item.ID);

                WfEscalation wfEscalation = wfEscalationRepository.WfEscalation.FirstOrDefault(p => p.SH_USERID.Equals(iLastActionUserID));
                iEscalationGroupID = wfEscalation.EscalationGroupID;
                item.WorkflowEscalationGroupID = iEscalationGroupID;

                var groupMemberModel = (from p in wfGroupMemberRepository.WfGroupMember
                                                      where p.WfGroupID.Equals(iEscalationGroupID)
                                                      select p);
                bool validApprover = false;
                foreach(var item2 in groupMemberModel)
                {
                    if (item2.SH_USERID.Equals(Int32.Parse(Session["UserPK"].ToString())))
                    {
                        validApprover = true;
                    }
                }
                if (validApprover == true)
                {
                    workflowPaymentModel.Payment = item;
                }
                
                
            }
            
            return PartialView(workflowPaymentModel);
        }
        #endregion

        #region "Dynamic Formula"
        private void ApplyDynamicFormulaForGstDeleteLine(PaymentLine paymentLine)
        {
            long lDfRequestId = 0;
            DfRequestData dfRequestData0 = dfRequestDataRepository.DfRequestData.FirstOrDefault(p => p.TableID.Equals(1) && p.HeaderID.Equals(paymentLine.PaymentID));
            if (dfRequestData0 == null)
            {
                return;
            }
            else
            {
                lDfRequestId = dfRequestData0.DfRequestID;
            }


            //Get GST %
            double dGstPct = 0;
            DfMasterData dfMasterData = dfMasterDataRepository.DfMasterData.FirstOrDefault(p => p.DfMasterID.Equals(1) && p.Name.Equals("GstPct"));
            if (dfMasterData != null)
            {
                dGstPct = double.Parse(dfMasterData.Value);
            }

            //Get Header Rounding
            int iHeaderRounding = 0;
            DfMasterData dfMasterData2 = dfMasterDataRepository.DfMasterData.FirstOrDefault(p => p.DfMasterID.Equals(1) && p.Name.Equals("HeaderRounding"));
            if (dfMasterData2 != null)
            {
                iHeaderRounding = int.Parse(dfMasterData2.Value);
            }

            //Get Line Rounding
            int iLineRounding = 0;
            DfMasterData dfMasterData3 = dfMasterDataRepository.DfMasterData.FirstOrDefault(p => p.DfMasterID.Equals(1) && p.Name.Equals("LineRounding"));
            if (dfMasterData3 != null)
            {
                iLineRounding = int.Parse(dfMasterData3.Value);
            }

            //Get GstAmount
            double dGstAmount = paymentLine.Amount * dGstPct / 100;
            if (iLineRounding > 0)
            {
                dGstAmount = Math.Round(dGstAmount, iLineRounding);
            }

            DfRequestData dfRequestData1 = dfRequestDataRepository.DfRequestData.FirstOrDefault(p => p.DfRequestID.Equals(lDfRequestId) && p.TableID.Equals(2) && p.HeaderID.Equals(paymentLine.PaymentID) && p.LineID.Equals(paymentLine.ID) && p.Name.Equals("PaymentAmount"));
            if (dfRequestData1 != null)
            {
                //dfRequestData1.Value = paymentLine.Amount.ToString();
                dfRequestDataRepository.DeleteDfRequestData(dfRequestData1);
            }


            DfRequestData dfRequestData2 = dfRequestDataRepository.DfRequestData.FirstOrDefault(p => p.DfRequestID.Equals(lDfRequestId) && p.TableID.Equals(2) && p.HeaderID.Equals(paymentLine.PaymentID) && p.LineID.Equals(paymentLine.ID) && p.Name.Equals("GstPct"));
            if (dfRequestData2 != null)
            {
                //dfRequestData2.Value = dGstPct.ToString();
                dfRequestDataRepository.DeleteDfRequestData(dfRequestData2);
            }


            DfRequestData dfRequestData3 = dfRequestDataRepository.DfRequestData.FirstOrDefault(p => p.DfRequestID.Equals(lDfRequestId) && p.TableID.Equals(2) && p.HeaderID.Equals(paymentLine.PaymentID) && p.LineID.Equals(paymentLine.ID) && p.Name.Equals("GstAmount"));
            if (dfRequestData3 != null)
            {
                //dfRequestData3.Value = dGstAmount.ToString();
                dfRequestDataRepository.DeleteDfRequestData(dfRequestData3);
            }


            decimal dGstSum = 0;
            DfRequestData dfRequestData4 = dfRequestDataRepository.DfRequestData.FirstOrDefault(p => p.DfRequestID.Equals(lDfRequestId) && p.TableID.Equals(1) && p.HeaderID.Equals(paymentLine.PaymentID));
            if (dfRequestData4 != null)
            {

                dGstSum = GetDynamicFormulaGstSum(lDfRequestId, 2, paymentLine.PaymentID);
                //if (iHeaderRounding > 0)
                //{
                //    dGstSum = Math.Round(dGstSum, iHeaderRounding);
                //}
                dfRequestData4.Value = dGstSum.ToString();
                dfRequestDataRepository.SaveDfRequestData(dfRequestData4);
            }


        }
        private void ApplyDynamicFormulaForGstEditLine(PaymentLine paymentLine)
        {
            long lDfRequestId = 0;
            DfRequestData dfRequestData0 = dfRequestDataRepository.DfRequestData.FirstOrDefault(p => p.TableID.Equals(1) && p.HeaderID.Equals(paymentLine.PaymentID));
            if (dfRequestData0 == null)
            {
                return;
            }
            else
            {
                lDfRequestId = dfRequestData0.DfRequestID;
            }


            //Get GST %
            double dGstPct = 0;
            DfMasterData dfMasterData = dfMasterDataRepository.DfMasterData.FirstOrDefault(p => p.DfMasterID.Equals(1) && p.Name.Equals("GstPct"));
            if (dfMasterData != null)
            {
                dGstPct = double.Parse(dfMasterData.Value);
            }

            //Get Header Rounding
            int iHeaderRounding = 0;
            DfMasterData dfMasterData2 = dfMasterDataRepository.DfMasterData.FirstOrDefault(p => p.DfMasterID.Equals(1) && p.Name.Equals("HeaderRounding"));
            if (dfMasterData2 != null)
            {
                iHeaderRounding = int.Parse(dfMasterData2.Value);
            }

            //Get Line Rounding
            int iLineRounding = 0;
            DfMasterData dfMasterData3 = dfMasterDataRepository.DfMasterData.FirstOrDefault(p => p.DfMasterID.Equals(1) && p.Name.Equals("LineRounding"));
            if (dfMasterData3 != null)
            {
                iLineRounding = int.Parse(dfMasterData3.Value);
            }

            //Get GstAmount
            double dGstAmount = paymentLine.Amount * dGstPct / 100;
            //if (iLineRounding > 0)
            //{
            //    dGstAmount = Math.Round(dGstAmount, iLineRounding);
            //}

            DfRequestData dfRequestData1 = dfRequestDataRepository.DfRequestData.FirstOrDefault(p => p.DfRequestID.Equals(lDfRequestId) && p.TableID.Equals(2) && p.HeaderID.Equals(paymentLine.PaymentID) && p.LineID.Equals(paymentLine.ID) && p.Name.Equals("PaymentAmount"));
            if (dfRequestData1 != null)
            {
                dfRequestData1.Value = paymentLine.Amount.ToString();
                dfRequestDataRepository.SaveDfRequestData(dfRequestData1);
            }
            

            DfRequestData dfRequestData2 = dfRequestDataRepository.DfRequestData.FirstOrDefault(p => p.DfRequestID.Equals(lDfRequestId) && p.TableID.Equals(2) && p.HeaderID.Equals(paymentLine.PaymentID) && p.LineID.Equals(paymentLine.ID) && p.Name.Equals("GstPct"));
            if (dfRequestData2 != null)
            {
                dfRequestData2.Value = dGstPct.ToString();
                dfRequestDataRepository.SaveDfRequestData(dfRequestData2);
            }
            

            DfRequestData dfRequestData3 = dfRequestDataRepository.DfRequestData.FirstOrDefault(p => p.DfRequestID.Equals(lDfRequestId) && p.TableID.Equals(2) && p.HeaderID.Equals(paymentLine.PaymentID) && p.LineID.Equals(paymentLine.ID) && p.Name.Equals("GstAmount"));
            if (dfRequestData3 != null)
            {
                dfRequestData3.Value = dGstAmount.ToString();
                dfRequestDataRepository.SaveDfRequestData(dfRequestData3);
            }
            

            decimal dGstSum = 0;
            DfRequestData dfRequestData4 = dfRequestDataRepository.DfRequestData.FirstOrDefault(p => p.DfRequestID.Equals(lDfRequestId) && p.TableID.Equals(1) && p.HeaderID.Equals(paymentLine.PaymentID));
            if (dfRequestData4 != null)
            {
                
                dGstSum = GetDynamicFormulaGstSum(lDfRequestId,  2, paymentLine.PaymentID);
                //if (iHeaderRounding > 0)
                //{
                //    dGstSum = Math.Round(dGstSum, iHeaderRounding);
                //}
                dfRequestData4.Value = dGstSum.ToString();
                dfRequestDataRepository.SaveDfRequestData(dfRequestData4);
            }


        }

        private void ApplyDynamicFormulaForGstNewLine(PaymentLine paymentLine)
        {
            long lDfRequestId = 0;
            DfRequestData dfRequestData0 = dfRequestDataRepository.DfRequestData.FirstOrDefault(p => p.TableID.Equals(1) && p.HeaderID.Equals(paymentLine.PaymentID)); 
            if (dfRequestData0 == null)
            {
                DfRequest dfRequest = new DfRequest();
                dfRequest.DfMasterID = 1; //1=Gst
                dfRequest.Title = "Apply GST Charges";
                dfRequest.DateRequested = DateTime.Now;
                dfRequest.UserID = Int32.Parse(Session["UserPK"].ToString());
                dfRequest.CreateDate = DateTime.Now;
                dfRequest.UpdateDate = DateTime.Now;
                dfRequestRepository.SaveDfRequest(dfRequest);
                lDfRequestId = dfRequest.ID;
            }
            else
            {
                lDfRequestId = dfRequestData0.DfRequestID;
            }
            

            //Get GST %
            double dGstPct = 0;
            DfMasterData dfMasterData = dfMasterDataRepository.DfMasterData.FirstOrDefault(p => p.DfMasterID.Equals(1) && p.Name.Equals("GstPct"));
            if (dfMasterData != null)
            {
                dGstPct = double.Parse(dfMasterData.Value);
            }

            //Get Header Rounding
            int iHeaderRounding = 0;
            DfMasterData dfMasterData2 = dfMasterDataRepository.DfMasterData.FirstOrDefault(p => p.DfMasterID.Equals(1) && p.Name.Equals("HeaderRounding"));
            if (dfMasterData2 != null)
            {
                iHeaderRounding = int.Parse(dfMasterData2.Value);
            }

            //Get Line Rounding
            int iLineRounding = 0;
            DfMasterData dfMasterData3 = dfMasterDataRepository.DfMasterData.FirstOrDefault(p => p.DfMasterID.Equals(1) && p.Name.Equals("LineRounding"));
            if (dfMasterData3 != null)
            {
                iLineRounding = int.Parse(dfMasterData3.Value);
            }

            //Get GstAmount
            double dGstAmount = paymentLine.Amount * dGstPct / 100;
            //if (iLineRounding > 0)
            //{
            //    dGstAmount = Math.Round(dGstAmount, iLineRounding);
            //}

            DfRequestData dfRequestData1 = new DfRequestData();
            dfRequestData1.DfRequestID = lDfRequestId;
            dfRequestData1.TableID = 2;//1=PaymentLine
            dfRequestData1.HeaderID = paymentLine.PaymentID;
            dfRequestData1.LineID = paymentLine.ID;
            dfRequestData1.Name = "PaymentAmount";
            dfRequestData1.Value = paymentLine.Amount.ToString();
            dfRequestDataRepository.SaveDfRequestData(dfRequestData1);

            DfRequestData dfRequestData2 = new DfRequestData();
            dfRequestData2.DfRequestID = lDfRequestId;
            dfRequestData2.TableID = 2;//1=PaymentLine
            dfRequestData2.HeaderID = paymentLine.PaymentID;
            dfRequestData2.LineID = paymentLine.ID;
            dfRequestData2.Name = "GstPct";
            dfRequestData2.Value = dGstPct.ToString();
            dfRequestDataRepository.SaveDfRequestData(dfRequestData2);

            DfRequestData dfRequestData3 = new DfRequestData();
            dfRequestData3.DfRequestID = lDfRequestId;
            dfRequestData3.TableID = 2;//1=PaymentLine
            dfRequestData3.HeaderID = paymentLine.PaymentID;
            dfRequestData3.LineID = paymentLine.ID;
            dfRequestData3.Name = "GstAmount";
            dfRequestData3.Value = dGstAmount.ToString();
            dfRequestDataRepository.SaveDfRequestData(dfRequestData3);

            decimal dGstSum = 0;
            DfRequestData dfRequestData4 = dfRequestDataRepository.DfRequestData.FirstOrDefault(p => p.DfRequestID.Equals(lDfRequestId) && p.TableID.Equals(1) && p.HeaderID.Equals(paymentLine.PaymentID));
            if (dfRequestData4 == null)
            {
                DfRequestData dfRequestData5 = new DfRequestData();
                dfRequestData5.DfRequestID = lDfRequestId;
                dfRequestData5.TableID = 1;//1=Payment
                dfRequestData5.HeaderID = paymentLine.PaymentID;
                dfRequestData5.LineID = 1;
                dfRequestData5.Name = "TotalGstAmount";
                dGstSum = GetDynamicFormulaGstSum(lDfRequestId, 2, paymentLine.PaymentID);
                //if (iHeaderRounding > 0)
                //{
                //    dGstSum = Math.Round(dGstSum, iHeaderRounding);
                //}
                dfRequestData5.Value = dGstSum.ToString();
                dfRequestDataRepository.SaveDfRequestData(dfRequestData5);
            }
            else
            {
                dGstSum = GetDynamicFormulaGstSum(lDfRequestId, 2, paymentLine.PaymentID);
                //if (iHeaderRounding > 0)
                //{
                //    dGstSum = Math.Round(dGstSum, iHeaderRounding);
                //}
                dfRequestData4.Value = dGstSum.ToString();
                dfRequestDataRepository.SaveDfRequestData(dfRequestData4);
            }
            

        }

        private decimal GetDynamicFormulaGstSum(long dfRequestId, int tableId, long headerId)
        {
            decimal dGstSum = 0;
            var model = (from p in dfRequestDataRepository.DfRequestData
                         where p.DfRequestID.Equals(dfRequestId) && p.TableID.Equals(tableId) && p.HeaderID.Equals(headerId) && p.Name.Equals("GstAmount")
                         select p);
            dGstSum = model.Sum(p => CommonUtility.Null2Zero(p.Value));
            
            return dGstSum;
        }

        private double GetLineGstPct(PaymentLine paymentLine)
        {
            double dGstPct = 0;
            long lDfRequestId = 0;
            DfRequestData dfRequestData0 = dfRequestDataRepository.DfRequestData.FirstOrDefault(p => p.TableID.Equals(1) && p.HeaderID.Equals(paymentLine.PaymentID));
            if (dfRequestData0 != null)
            {
                lDfRequestId = dfRequestData0.DfRequestID;
                DfRequestData dfRequestData1 = dfRequestDataRepository.DfRequestData.FirstOrDefault(p => p.DfRequestID.Equals(lDfRequestId) && p.TableID.Equals(2) && p.HeaderID.Equals(paymentLine.PaymentID) && p.LineID.Equals(paymentLine.ID) && p.Name.Equals("GstPct"));
                if (dfRequestData1 != null)
                {
                    dGstPct = double.Parse(dfRequestData1.Value);
                }
            }
            
            return dGstPct;
        }

        private double GetLineAmount(PaymentLine paymentLine)
        {
            double dGstAmount = 0;
            long lDfRequestId = 0;
            int iLineRounding = 0;
            DfMasterData dfMasterDataLR = dfMasterDataRepository.DfMasterData.FirstOrDefault(p => p.DfMasterID.Equals(1) && p.Name.Equals("LineRounding"));
            if (dfMasterDataLR != null)
            {
                iLineRounding = int.Parse(dfMasterDataLR.Value);
            }
            string sLineRoundingMode = "";
            DfMasterData dfMasterDataLRM = dfMasterDataRepository.DfMasterData.FirstOrDefault(p => p.DfMasterID.Equals(1) && p.Name.Equals("LineRoundingMode"));
            if (dfMasterDataLRM != null)
            {
                sLineRoundingMode = dfMasterDataLRM.Value;
            }
            DfRequestData dfRequestData0 = dfRequestDataRepository.DfRequestData.FirstOrDefault(p => p.TableID.Equals(1) && p.HeaderID.Equals(paymentLine.PaymentID));
            if (dfRequestData0 != null)
            {
                lDfRequestId = dfRequestData0.DfRequestID;
                DfRequestData dfRequestData1 = dfRequestDataRepository.DfRequestData.FirstOrDefault(p => p.DfRequestID.Equals(lDfRequestId) && p.TableID.Equals(2) && p.HeaderID.Equals(paymentLine.PaymentID) && p.LineID.Equals(paymentLine.ID) && p.Name.Equals("PaymentAmount"));
                if (dfRequestData1 != null)
                {
                    dGstAmount = double.Parse(dfRequestData1.Value);
                    if (iLineRounding > 0)
                    {
                        if (sLineRoundingMode.Equals("AwayFromZero"))
                        {
                            dGstAmount = Math.Round(dGstAmount, iLineRounding, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            dGstAmount = Math.Round(dGstAmount, iLineRounding, MidpointRounding.ToEven);
                        }
                    }
                }
            }

            return dGstAmount;
        }
        private double GetLineGstAmount(PaymentLine paymentLine)
        {
            double dGstAmount = 0;
            long lDfRequestId = 0;
            int iLineRounding = 0;
            DfMasterData dfMasterDataLR = dfMasterDataRepository.DfMasterData.FirstOrDefault(p => p.DfMasterID.Equals(1) && p.Name.Equals("LineRounding"));
            if (dfMasterDataLR != null)
            {
                iLineRounding = int.Parse(dfMasterDataLR.Value);
            }
            string sLineRoundingMode = "";
            DfMasterData dfMasterDataLRM = dfMasterDataRepository.DfMasterData.FirstOrDefault(p => p.DfMasterID.Equals(1) && p.Name.Equals("LineRoundingMode"));
            if (dfMasterDataLRM != null)
            {
                sLineRoundingMode = dfMasterDataLRM.Value;
            }
            DfRequestData dfRequestData0 = dfRequestDataRepository.DfRequestData.FirstOrDefault(p => p.TableID.Equals(1) && p.HeaderID.Equals(paymentLine.PaymentID));
            if (dfRequestData0 != null)
            {
                lDfRequestId = dfRequestData0.DfRequestID;
                DfRequestData dfRequestData1 = dfRequestDataRepository.DfRequestData.FirstOrDefault(p => p.DfRequestID.Equals(lDfRequestId) && p.TableID.Equals(2) && p.HeaderID.Equals(paymentLine.PaymentID) && p.LineID.Equals(paymentLine.ID) && p.Name.Equals("GstAmount"));
                if (dfRequestData1 != null)
                {
                    dGstAmount = double.Parse(dfRequestData1.Value);
                    if (iLineRounding > 0)
                    {
                        if (sLineRoundingMode.Equals("AwayFromZero"))
                        {
                            dGstAmount = Math.Round(dGstAmount, iLineRounding, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            dGstAmount = Math.Round(dGstAmount, iLineRounding, MidpointRounding.ToEven);
                        }
                    }
                }
            }

            return dGstAmount;
        }

        private double GetTotalGstAmount(PaymentLineModel paymentLineModel)
        {
            double dGstSum = 0;
            long lDfRequestId = 0;
            int iHeaderRounding = 0;
            DfMasterData dfMasterDataHR = dfMasterDataRepository.DfMasterData.FirstOrDefault(p => p.DfMasterID.Equals(1) && p.Name.Equals("HeaderRounding"));
            if (dfMasterDataHR != null)
            {
                iHeaderRounding = int.Parse(dfMasterDataHR.Value);
            }
            string sHeaderRoundingMode = "";
            DfMasterData dfMasterDataHRM = dfMasterDataRepository.DfMasterData.FirstOrDefault(p => p.DfMasterID.Equals(1) && p.Name.Equals("HeaderRoundingMode"));
            if (dfMasterDataHRM != null)
            {
                sHeaderRoundingMode = dfMasterDataHRM.Value;
            }

            DfRequestData dfRequestData0 = dfRequestDataRepository.DfRequestData.FirstOrDefault(p => p.TableID.Equals(1) && p.HeaderID.Equals(paymentLineModel.Payment.ID));
            if (dfRequestData0 != null)
            {
                lDfRequestId = dfRequestData0.DfRequestID;
                DfRequestData dfRequestData1 = dfRequestDataRepository.DfRequestData.FirstOrDefault(p => p.DfRequestID.Equals(lDfRequestId) && p.TableID.Equals(1) && p.HeaderID.Equals(paymentLineModel.Payment.ID) && p.LineID.Equals(1) && p.Name.Equals("TotalGstAmount"));
                if (dfRequestData1 != null)
                {
                    dGstSum = double.Parse(dfRequestData1.Value);
                    if (iHeaderRounding > 0)
                    {
                        if (sHeaderRoundingMode.Equals("AwayFromZero"))
                        {
                            dGstSum = Math.Round(dGstSum, iHeaderRounding, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            dGstSum = Math.Round(dGstSum, iHeaderRounding, MidpointRounding.ToEven);
                        }
                    }
                    else
                    {
                        dGstSum = paymentLineModel.PaymentLine.Sum(p => p.GstAmount);
                    }
                }
            }

            return dGstSum;
        }

        private double GetTotalGstAmount2(PaymentLineModel paymentLineModel)
        {
            double dGstSum = paymentLineModel.PaymentLine.Sum(p=>p.GstAmount);
 
            return dGstSum;
        }

        private double GetTotalLineAmount(PaymentLineModel paymentLineModel)
        {
            double dPaymentAmountSum = paymentLineModel.PaymentLine.Sum(p => p.Amount);

            return dPaymentAmountSum;
        }

        private double GetTotalAmountAfterGst(PaymentLineModel paymentLineModel)
        {
            double dPaymentAmountSum = paymentLineModel.PaymentLine.Sum(p => p.AmountAfterGst);

            return dPaymentAmountSum;
        }
        #endregion
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