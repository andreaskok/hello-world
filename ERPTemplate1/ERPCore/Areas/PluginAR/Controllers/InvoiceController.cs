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
    public class InvoiceController : BaseController
    {
        //private EFDbContextAR db = new EFDbContextAR();
        private IInvoiceRepository invoiceRepository;
        private IInvoiceLineRepository invoiceLineRepository;
        private IChartOfAccountRepository chartOfAccountRepository;
        private ICustomerRepository customerRepository;

        public InvoiceController(IInvoiceRepository invoiceRepo, IInvoiceLineRepository invoiceLineRepo, IChartOfAccountRepository chartOfAccountRepo, ICustomerRepository customerRepository)
        {
            this.invoiceRepository = invoiceRepo;
            this.invoiceLineRepository = invoiceLineRepo;
            this.chartOfAccountRepository = chartOfAccountRepo;
            this.customerRepository = customerRepository;
        }

        // GET: Invoice
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

            var model = (from m in invoiceRepository.Invoice
                         select m);

            //int iRecCnt = invoiceRepository.InvoiceCount();
            Int64? iRecCnt = CommonUtility.Null2LongZero(invoiceRepository.GetMaxID());

            if (!String.IsNullOrEmpty(searchValue))
            {
                model = (from m in invoiceRepository.InvoiceWildSearch("InvoiceCode", searchValue)
                         select m);
                iRecCnt = model.Count();
            }
            else
            {
                //model = model.Skip(((int)page - 1) * pageSize)
                //       .Take(pageSize);
                //improve performance
                model = invoiceRepository.GetInvoicePaging(((((long)page - 1) * pageSize) + 1), (long)page * pageSize);
            }

            switch (sortOrder)
            {
                default:
                    //model = model.OrderBy(s => s.ID);
                    break;
            }
            
            //return View(model.ToPagedList(pageNumber, pageSize));
            //improve performance
            var model3 = new StaticPagedList<Invoice>(model, (int)page, pageSize, (int)iRecCnt);
            return View(model3);
        //return View(db.Invoice.ToList());
    }

    // GET: Invoice/Details/5
    public ActionResult Details(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from p in invoiceRepository.Invoice
                         where p.ID.Equals(id)
                         orderby p.InvoiceCode
                         select p).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView(model);

            //Invoice invoice = db.Invoice.Find(id);
            //if (invoice == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(invoice);
        }

        // GET: Invoice/Create
        public ActionResult Create()
        {
            TempData["Message"] = "";
            TempData["DeteleMessage"] = "";
            Session["InvoiceCode"] = null;
            //Session["InvoiceID"] = "";
            Session["InvoiceLineId"] = null;
            Session["InvoiceLineCode"] = null;
            Session["InvoiceID"] = null;
            PopulateCustomerDropDownList();
            return View(new InvoiceLineModel());
            //return View();
        }

        // POST: Invoice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CustomerID,TaxCodeID,CurrencyID,InvoiceCode,LocCode,DocType,CustomerCode,OutstandingAmount,TotalAmount,CustRef,DeliveryRef,Remark,AccMonth,AccYear,Status,CreateDate,UpdateDate,PrintDate,UpdateID,PostDate,ServiceTax,SalesTax,GrandAmount,CurrencyRate,CurrencyAmount,CurrencyOutAmount,InvoiceDate,SelfBilledRefNo,SelfBilledRefDate,InvoiceType,InvoiceRefNo")] Invoice invoice)
        {
            var invoiceLineModel = new InvoiceLineModel();

            if (ModelState.IsValid)
            {
                //db.Invoice.Add(invoice);
                //db.SaveChanges();
                invoice.CreateDate = DateTime.Now;
                invoice.UpdateDate = DateTime.Now;
                invoice.PrintDate = DateTime.Now;
                invoice.PostDate = DateTime.Now;
                //invoice.DocDate = DateTime.Now;
                int iCustomer = Int32.Parse(Request.Form["Customer"].ToString());
                invoice.CustomerID = iCustomer;
                Customer customer = customerRepository.Customer.FirstOrDefault(p => p.ID == iCustomer);
                invoice.CustomerCode = customer.CustomerCode.Trim();
                invoice.UpdateID = Session["UserID"].ToString();
                invoice.Status = "Active";
                var invoiceRec = from m in invoiceRepository.InvoiceWildSearch("InvoiceCode", invoice.InvoiceCode)
                                 select m;
                int iRecCnt = invoiceRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The Invoice Code " + invoice.InvoiceCode + " already existed in system ! ";
                }
                else
                {
                    invoiceRepository.SaveInvoice(invoice);
                    Session["InvoiceID"] = invoice.ID;
                    Session["InvoiceCode"] = invoice.InvoiceCode;
                    Session["CustomerID"] = invoice.CustomerID;
                    Session["CustomerCode"] = invoice.CustomerCode;
                    
                    invoiceLineModel.Invoice = (from p in invoiceRepository.Invoice
                                                where p.ID.Equals(invoice.ID)
                                                orderby p.InvoiceCode
                                                select p).FirstOrDefault();
                    if (invoice.ID != 0)
                    {
                        invoiceLineModel.InvoiceLine = from p in invoiceLineRepository.InvoiceLine
                                                       where p.ID.Equals(invoice.ID)
                                                       orderby p.InvoiceCode
                                                       select p;
                    }
                    Session["InvoiceLineID"] = "";
                    TempData["Message"] = string.Format("{0} was added in system !", invoice.InvoiceCode);
                }
                PopulateCustomerDropDownList(invoice.CustomerID);
                //return RedirectToAction("Index");
            }

            return View(invoiceLineModel);
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

        private void PopulateCustomerDropDownList(object selectedAcc = null)
        {
            var customerQuery = from d in customerRepository.Customer
                              orderby d.CustomerCode
                              select d;
            ViewBag.Customer = new SelectList(customerQuery, "ID", "CustomerCode", selectedAcc);
        }

        public ActionResult CreateInvoiceAndInvoiceLine(string roleName)
        {

            PopulateAccountDropDownList();
            PopulateCustomerDropDownList();
            return PartialView(new InvoiceLine());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateInvoiceAndInvoiceLine([Bind(Include = "ID,InvoiceID,ChartOfAccountID,CurrencyID,TaxCodeID,InvoiceLineCode,InvoiceCode,AccCode,BlkCode,VehCode,VehExpenseCode,Description,Quantity,UnitPrice,Total,LineInd,DisRef,Discnt,DisInd,DisType,CurrencyCost,CurrencyAmount,TaxRate,TaxRef,TaxInd")] InvoiceLine invoiceLine)
        {
            
            if (ModelState.IsValid)
            {
                int iChartOfAcc = Int32.Parse(Request.Form["ChartOfAcc"].ToString());
                invoiceLine.InvoiceID = Int32.Parse(Session["InvoiceID"].ToString());
                invoiceLine.InvoiceCode = Session["InvoiceCode"].ToString();
                invoiceLine.ChartOfAccountID = iChartOfAcc;
                invoiceLine.TaxCodeID = 0;
                ChartOfAccount chartOfAccount = chartOfAccountRepository.ChartOfAccount.FirstOrDefault(p => p.ID == iChartOfAcc);
                invoiceLine.AccCode = chartOfAccount.AccCode;
                var invoiceLineRec = from m in invoiceLineRepository.InvoiceLineWildSearch("InvoiceLineCode", invoiceLine.InvoiceLineCode)
                                 select m;
                int iRecCnt = invoiceLineRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The Invoice Line Code " + invoiceLine.InvoiceLineCode + " already existed in system ! ";
                }
                else
                {
                    invoiceLineRepository.SaveInvoiceLine(invoiceLine);
                    Session["InvoiceLineID"] = invoiceLine.ID;
                    Session["InvoiceLineCode"] = invoiceLine.InvoiceLineCode;
                    //return RedirectToAction("Index");

                    TempData["Message"] = string.Format("{0} was added", invoiceLine.InvoiceLineCode);
                }
                PopulateAccountDropDownList(invoiceLine.ChartOfAccountID);
                PopulateCustomerDropDownList(Session["CustomerID"].ToString());
                return RedirectToAction("Edit2", new { id = invoiceLine.InvoiceID });
            }
            return View(invoiceLine);
        }

        public ActionResult CreateInvoiceLine(Int64? invoiceId, string invoiceCode)
        {
            Session["InvoiceID"] = invoiceId;
            Session["InvoiceCode"] = invoiceCode;
            PopulateAccountDropDownList();
            PopulateCustomerDropDownList();
            return PartialView(new InvoiceLine());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateInvoiceLine([Bind(Include = "ID,InvoiceID,ChartOfAccountID,CurrencyID,TaxCodeID,InvoiceLineCode,InvoiceCode,AccCode,BlkCode,VehCode,VehExpenseCode,Description,Quantity,UnitPrice,Total,LineInd,DisRef,Discnt,DisInd,DisType,CurrencyCost,CurrencyAmount,TaxRate,TaxRef,TaxInd")] InvoiceLine invoiceLine)
        {
            if (ModelState.IsValid)
            {
                int iChartOfAcc = Int32.Parse(Request.Form["ChartOfAcc"].ToString());
                invoiceLine.InvoiceID = Int32.Parse(Session["InvoiceID"].ToString());
                invoiceLine.ChartOfAccountID = iChartOfAcc;
                invoiceLine.InvoiceCode = Session["InvoiceCode"].ToString();
                invoiceLine.TaxCodeID = 0;
                //journalLine.AccCode = Session["ChartOfAcc"].ToString();
                ChartOfAccount chartOfAccount = chartOfAccountRepository.ChartOfAccount.FirstOrDefault(p => p.ID == iChartOfAcc);
                invoiceLine.AccCode = chartOfAccount.AccCode.Trim();
                var invoiceLineRec = from m in invoiceLineRepository.InvoiceLineWildSearch("InvoiceLineCode", invoiceLine.InvoiceLineCode)
                                     select m;
                int iRecCnt = invoiceLineRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The Invoice Line Code " + invoiceLine.InvoiceLineCode + " already existed in system ! ";
                }
                else
                {
                    invoiceLineRepository.SaveInvoiceLine(invoiceLine);
                    Session["InvoiceLineID"] = invoiceLine.ID;
                    Session["InvoiceLineCode"] = invoiceLine.InvoiceLineCode;

                    TempData["Message"] = string.Format("{0} was added", invoiceLine.InvoiceLineCode);
                }
                PopulateAccountDropDownList(invoiceLine.ChartOfAccountID);
                return RedirectToAction("Edit2", new { id = invoiceLine.InvoiceID });
            }
            return View(invoiceLine);
        }

        //private bool isRoleAppExist(int? roleId, int? appId)
        //{
        //    bool recordExist = false;
        //    SH_ROLEACCESS sh_roleaccess = roleAccessRepository.SH_ROLEACCESS.FirstOrDefault(p => p.SH_ROLEID == roleId && p.SH_APPID == appId);
        //    if (sh_roleaccess != null)
        //    {
        //        recordExist = true;
        //    }
        //    return recordExist;
        //}

        public ActionResult EditInvoiceLine(Int64? invoiceId, Int64? invoiceLineId, string invoiceCode, string invoiceLineCode)
        {
            Session["InvoiceCode"] = invoiceCode;
            Session["InvoiceID"] = invoiceId;
            Session["InvoiceLineId"] = invoiceLineId;
            Session["InvoiceLineCode"] = invoiceLineCode;

            //improve performance
            InvoiceLine invoiceLine = invoiceLineRepository.GetInvoiceLineByID((long)invoiceLineId).SingleOrDefault();
            PopulateAccountDropDownList(invoiceLine.ChartOfAccountID);
            return PartialView(invoiceLine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditInvoiceLine([Bind(Include = "ID,InvoiceID,ChartOfAccountID,CurrencyID,TaxCodeID,InvoiceLineCode,InvoiceCode,AccCode,BlkCode,VehCode,VehExpenseCode,Description,Quantity,UnitPrice,Total,LineInd,DisRef,Discnt,DisInd,DisType,CurrencyCost,CurrencyAmount,TaxRate,TaxRef,TaxInd")] InvoiceLine invoiceLine)
        {
            if (ModelState.IsValid)
            {
                int iChartOfAcc = Int32.Parse(Request.Form["ChartOfAcc"].ToString());
                invoiceLine.InvoiceID = Int32.Parse(Session["InvoiceID"].ToString());
                invoiceLine.ChartOfAccountID = iChartOfAcc;
                invoiceLine.InvoiceCode = Session["InvoiceCode"].ToString();
                invoiceLine.TaxCodeID = 0;
                ChartOfAccount chartOfAccount = chartOfAccountRepository.ChartOfAccount.FirstOrDefault(p => p.ID == iChartOfAcc);
                invoiceLine.AccCode = chartOfAccount.AccCode.Trim();
                invoiceLineRepository.SaveInvoiceLine(invoiceLine);
                Session["InvoiceID"] = invoiceLine.InvoiceID;
                //return View("Edit");
                return RedirectToAction("Edit2", new { id = invoiceLine.InvoiceID });
            }
            return PartialView(invoiceLine);
        }

        // GET: Invoice/Edit/5
        public ActionResult Edit(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Invoice invoice = db.Invoice.Find(id);
            //if (invoice == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(invoice);
            var invoiceLineModel = new InvoiceLineModel();
            invoiceLineModel.Invoice = invoiceRepository.GetInvoiceByID((long)id).FirstOrDefault();

            if (id != null)
            {
                //improve performance
                invoiceLineModel.InvoiceLine = invoiceLineRepository.GetInvoiceLine("InvoiceID", (long)id);
            }

            if (invoiceLineModel == null)
            {
                return HttpNotFound();
            }
            PopulateCustomerDropDownList(invoiceLineModel.Invoice.CustomerID);
            return PartialView(invoiceLineModel);

        }

        // POST: Invoice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,InvoiceCode,LocCode,DocType,CustomerCode,OutstandingAmount,TotalAmount,CustRef,DeliveryRef,Remark,AccMonth,AccYear,Status,CreateDate,UpdateDate,PrintDate,UpdateID,PostDate,ServiceTax,SalesTax,GrandAmount,CurrencyRate,CurrencyAmount,CurrencyOutAmount,InvoiceDate,SelfBilledRefNo,SelfBilledRefDate,InvoiceType,InvoiceRefNo")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(invoice).State = EntityState.Modified;
                //db.SaveChanges();
                invoice.UpdateDate = DateTime.Now;
                invoice.Status = "Active";
                int iCustomer = Int32.Parse(Request.Form["Customer"].ToString());
                invoice.CustomerID = iCustomer;
                Customer customer = customerRepository.Customer.FirstOrDefault(p => p.ID == iCustomer);
                invoice.CustomerCode = customer.CustomerCode.Trim();
                PopulateCustomerDropDownList(invoice.CustomerID);

                invoiceRepository.SaveInvoice(invoice);
                ViewBag.Message = string.Format("{0} was updated", invoice.InvoiceCode);

                return RedirectToAction("Index");
            }
            return PartialView(invoice);
        }

        public ActionResult Edit2(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TempData["Message"] = "";
            var invoiceLineModel = new InvoiceLineModel();

            //remove the orderBy to improve performance
            invoiceLineModel.Invoice = invoiceRepository.GetInvoiceByID((long)id).FirstOrDefault();
            if (id != null)
            {
                //improve performance
                invoiceLineModel.InvoiceLine = invoiceLineRepository.GetInvoiceLine("InvoiceID", (long)id);
            }

            if (invoiceLineModel == null)
            {
                return HttpNotFound();
            }
            return View(invoiceLineModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2([Bind(Include = "ID,CustomerID,TaxCodeID,CurrencyID,InvoiceCode,LocCode,DocType,CustomerCode,OutstandingAmount,TotalAmount,CustRef,DeliveryRef,Remark,AccMonth,AccYear,Status,CreateDate,UpdateDate,PrintDate,UpdateID,PostDate,ServiceTax,SalesTax,GrandAmount,CurrencyRate,CurrencyAmount,CurrencyOutAmount,InvoiceDate,SelfBilledRefNo,SelfBilledRefDate,InvoiceType,InvoiceRefNo")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                invoice.UpdateDate = DateTime.Now;
                invoiceRepository.SaveInvoice(invoice);
                ViewBag.Message = string.Format("{0} was updated", invoice.InvoiceCode);
                return RedirectToAction("Index");
            }
            return View(invoice);
        }
        
        // GET: Invoice/Delete/5
        public ActionResult Delete(Int64? id)
        {
            var invoiceLineModel = new InvoiceLineModel();

            invoiceLineModel.Invoice = invoiceRepository.GetInvoiceByID((long)id).FirstOrDefault();
            if (id != null)
            {
                //improve performance
                invoiceLineModel.InvoiceLine = invoiceLineRepository.GetInvoiceLine("InvoiceID", (long)id);
            }
            Invoice invoice = invoiceRepository.GetInvoiceByID((long)id).SingleOrDefault();

            if (invoiceLineModel.InvoiceLine != null)
            {
                foreach (var item in invoiceLineModel.InvoiceLine)
                {
                    InvoiceLine invoiceLine = invoiceLineRepository.InvoiceLine.FirstOrDefault(p => p.ID == item.ID);
                    invoiceLineRepository.DeleteInvoiceLine(invoiceLine);
                }
            }

            if (invoice != null)
            {
                invoiceRepository.DeleteInvoice(invoice);
                TempData["DeleteMessage"] = string.Format("{0} was deleted", invoice.InvoiceCode);
            }

            return RedirectToAction("Index");

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Invoice invoice = db.Invoice.Find(id);
            //if (invoice == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(invoice);
        }

        public ActionResult DeleteInvoiceLine(Int64? invoiceId, Int64? invoiceLineId)
        {

            InvoiceLine invoiceLine = invoiceLineRepository.InvoiceLine.FirstOrDefault(p => p.ID == invoiceLineId);
           
            if (invoiceLine != null)
            {
                invoiceLineRepository.DeleteInvoiceLine(invoiceLine);
                TempData["Message"] = "";
                TempData["DeleteMessage"] = string.Format("{0} was deleted", invoiceLine.InvoiceLineCode);
            }

            return RedirectToAction("Edit2", new { id = invoiceId });
        }

        // POST: Invoice/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(long id)
        //{
        //    Invoice invoice = db.Invoice.Find(id);
        //    db.Invoice.Remove(invoice);
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
