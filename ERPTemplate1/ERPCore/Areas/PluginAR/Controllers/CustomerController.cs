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
using ERPCore.Models;
using System.Collections.Generic;
using System.Web;


namespace PluginAR.Controllers
{
    public class CustomerController : BaseController
    {
        //private EFDbContextAR db = new EFDbContextAR();
        private ICustomerRepository customerRepository;
        private IDimension_SettingRepository dimension_settingRepositoty;
        private IDimension_TableRelationshipRepository dimension_tablerelationshipRepository;
        private ICustomer_Dim_SetupRepository customer_dim_setupRepository;
        private ICustomer_Dim_ValueRepository customer_dim_valueRepository;


        public CustomerController(ICustomerRepository customerRepo, IDimension_SettingRepository dimension_settingRepo, IDimension_TableRelationshipRepository dimension_tablerelationshipRepo, ICustomer_Dim_SetupRepository customer_dim_setupRepo, ICustomer_Dim_ValueRepository customer_dim_valueRepo)
        {
            this.customerRepository = customerRepo;
            this.dimension_settingRepositoty = dimension_settingRepo;
            this.dimension_tablerelationshipRepository = dimension_tablerelationshipRepo;
            this.customer_dim_setupRepository = customer_dim_setupRepo;
            this.customer_dim_valueRepository = customer_dim_valueRepo;
        }

        // GET: Customer
        public PartialViewResult LoadCustomerDimension(Int64? CustId, string CustCode)
        {
            Session["CustId"] = CustId;
            Session["CustCode"] = CustCode.Trim();

            int custID = Int32.Parse(Session["CustID"].ToString());
            List<CustomerDimensionModel> model = new List<CustomerDimensionModel>();
            var q = (from dtr in dimension_tablerelationshipRepository.Dimension_TableRelationship 
                     join ds in dimension_settingRepositoty.Dimension_Setting on dtr.Dimension_SettingID equals ds.ID
                     //join dds in customer_dim_setupRepository.Customer_Dim_Setup on ds. equals dds.ID
                     //join ddv in customer_dim_valueRepository.Customer_Dim_Value on dtr.Dimension_SettingID equals ddv.ID
                     where dtr.DimensionTable.Equals("Customer")
                     select new
                     {
                         dtrID = dtr.ID,
                         dsID = ds.ID,
                         dsDimensionCode = ds.DimensionCode,
                         dsDescription = ds.Description,
                         dsDimensionUsage = ds.DimensionUsage,
                         dsPredefinedValue = ds.PredefinedValue,
                         dsCustomerID = custID
                     }).ToList();//convert to List
            foreach (var item in q) //retrieve each item and assign to model
            {
                model.Add(new CustomerDimensionModel()
                {
                    CustomerID = item.dsCustomerID,
                    DimensionTableRelationshipID = item.dtrID,
                    DimensionSettingID = item.dsID,
                    DimensionCode = item.dsDimensionCode,
                    DimensionSettingDescription = item.dsDescription,
                    DimensionUsage = item.dsDimensionUsage,
                    PredefinedValue = item.dsPredefinedValue
                });
            }

            long lDimID = 0;
            long lDimSetupID = 0;
            string sDimValue = String.Empty;
            foreach (var item in model)
            {
                lDimID = item.DimensionSettingID;
                
                Customer_Dim_Setup cds = customer_dim_setupRepository.Customer_Dim_Setup.FirstOrDefault(p => p.CustomerID == custID && p.Dimension_SettingID == lDimID);
                if (cds != null)
                {
                    lDimSetupID = cds.ID;
                    sDimValue = "";
                    Customer_Dim_Value cdv = customer_dim_valueRepository.Customer_Dim_Value.FirstOrDefault(p => p.Customer_Dim_SetupID == lDimSetupID);
                    if (cdv != null)
                    {
                        sDimValue = cdv.DimensionValue;
                        item.FreeTextValue = sDimValue;
                    }
                    
                }
                
            }            
                //return PartialView("LoadChartOfAccountDimension", model.ToList());
                return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadCustomerDimension(List<CustomerDimensionModel> customerdimensionmodel)
        {
            if (customerdimensionmodel != null)
            {
                foreach (var item in customerdimensionmodel)
                {
                    long lDimSetupID = 0;
                    Customer customer = customerRepository.Customer.FirstOrDefault(p => p.ID == item.CustomerID);
                    int custID = Int32.Parse(item.CustomerID.ToString());
                    Customer_Dim_Setup customer_dim_setup = (from p in customer_dim_setupRepository.Customer_Dim_Setup
                                                             where p.CustomerID.Equals(item.CustomerID) && p.Dimension_SettingID.Equals(item.DimensionSettingID)
                                                             select p).FirstOrDefault();
                    if (customer_dim_setup != null)
                    {
                        customer_dim_setup.CustomerID = custID;
                        customer_dim_setup.Dimension_SettingID = item.DimensionSettingID;
                        customer_dim_setup.Status = "Active";
                        customer_dim_setupRepository.SaveCustomer_Dim_Setup(customer_dim_setup);
                        lDimSetupID = customer_dim_setup.ID;
                    }
                    else
                    {
                        Customer_Dim_Setup customer_dim_setup2 = new Customer_Dim_Setup();
                        customer_dim_setup2.CustomerID = custID;
                        customer_dim_setup2.Dimension_SettingID = item.DimensionSettingID;
                        customer_dim_setup2.Status = "Active";
                        customer_dim_setupRepository.SaveCustomer_Dim_Setup(customer_dim_setup2);
                        lDimSetupID = customer_dim_setup2.ID;
                    }
                    

                    Customer_Dim_Value customer_dim_value = (from p in customer_dim_valueRepository.Customer_Dim_Value
                                                                     where p.Customer_Dim_SetupID.Equals(lDimSetupID)
                                                                     select p).FirstOrDefault();
                    if (customer_dim_value != null)
                    {
                        customer_dim_value.Customer_Dim_SetupID = lDimSetupID;
                        customer_dim_value.DimensionValue =CommonUtility.Null2Empty(item.FreeTextValue).Trim();
                        customer_dim_value.Status = "Active";
                        customer_dim_valueRepository.SaveCustomer_Dim_Value(customer_dim_value);
                    }
                    else
                    {
                        Customer_Dim_Value customer_dim_value2 = new Customer_Dim_Value();
                        customer_dim_value2.Customer_Dim_SetupID = lDimSetupID;
                        customer_dim_value2.DimensionValue = CommonUtility.Null2Empty(item.FreeTextValue).Trim();
                        customer_dim_value2.Status = "Active";
                        customer_dim_valueRepository.SaveCustomer_Dim_Value(customer_dim_value2);
                    }                    
                }
            }

            TempData["Message"] = string.Format("{0} was updated in system !", Session["CustCode"]);
            TempData["DeleteMessage"] = "";
            return RedirectToAction("Edit2", new { id = Session["CustID"] });
        }

        // GET: Customer
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

            var model = from m in customerRepository.Customer
                        select m;

            if (!String.IsNullOrEmpty(searchValue))
            {
                model = model.Where(s => s.CustomerCode.ToUpper().Contains(searchValue.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.CustomerCode);
                    break;
                default:
                    model = model.OrderBy(s => s.CustomerCode);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        // GET: Customer/Details/5
        public ActionResult Details(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from p in customerRepository.Customer
                         where p.ID.Equals(id)
                         orderby p.CustomerCode
                         select p).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView(model);

        }

        //// GET: Customer
        //public PartialViewResult LoadCustomerDimension0(Int64? CustId, string CustCode)
        //{
        //    Session["CustId"] = CustId;
        //    Session["CustCode"] = CustCode.Trim();

        //    int custID = Int32.Parse(Session["CustID"].ToString());
        //    List<CustomerDimensionModel> model = new List<CustomerDimensionModel>();
        //    var q = (from dtr in dimension_tablerelationshipRepository.Dimension_TableRelationship
        //             join ds in dimension_settingRepositoty.Dimension_Setting
        //             on dtr.Dimension_SettingID equals ds.ID
        //             where dtr.DimensionTable.Equals("Customer")
        //             select new
        //             {
        //                 dtrID = dtr.ID,
        //                 dsID = ds.ID,
        //                 dsDimensionCode = ds.DimensionCode,
        //                 dsDescription = ds.Description,
        //                 dsDimensionUsage = ds.DimensionUsage,
        //                 dsPredefinedValue = ds.PredefinedValue,
        //                 dsCustomerID = custID
        //             }).ToList();//convert to List
        //    foreach (var item in q) //retrieve each item and assign to model
        //    {
        //        model.Add(new CustomerDimensionModel()
        //        {
        //            CustomerID = item.dsCustomerID,
        //            DimensionTableRelationshipID = item.dtrID,
        //            DimensionSettingID = item.dsID,
        //            DimensionCode = item.dsDimensionCode,
        //            DimensionSettingDescription = item.dsDescription,
        //            DimensionUsage = item.dsDimensionUsage,
        //            PredefinedValue = item.dsPredefinedValue
        //        });
        //    }

        //    //return PartialView("LoadChartOfAccountDimension", model.ToList());
        //    return PartialView(model);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult LoadCustomerDimension0(List<CustomerDimensionModel> customerdimensionmodel)
        //{
        //    if (customerdimensionmodel != null)
        //    {
        //        foreach (var item in customerdimensionmodel)
        //        {

        //            Customer_Dim_Setup customer_dim_setup = (from p in customer_dim_setupRepository.Customer_Dim_Setup
        //                                                     where p.CustomerID.Equals(item.CustomerID)
        //                                                     select p).FirstOrDefault();
        //            customer_dim_setup.CustomerID = item.CustomerID;
        //            customer_dim_setup.Dimension_SettingID = item.DimensionSettingID;
        //            customer_dim_setup.Status = "Active";
        //            customer_dim_setupRepository.SaveCustomer_Dim_Setup(customer_dim_setup);

        //            //if (customer_dim_setup != null)
        //            //{
        //            //    foreach (var item in customer_dim_setup)
        //            //    {
        //            Customer_Dim_Value customer_dim_value = (from p in customer_dim_valueRepository.Customer_Dim_Value
        //                                                     where p.ID.Equals(customer_dim_setup.ID)
        //                                                     select p).FirstOrDefault();
        //            customer_dim_value.Customer_Dim_SetupID = customer_dim_setup.ID;
        //            customer_dim_value.DimensionValue = item.FreeTextValue.Trim();
        //            customer_dim_value.Status = "Active";
        //            customer_dim_valueRepository.SaveCustomer_Dim_Value(customer_dim_value);
        //            //    }
        //        }
        //        TempData["Message"] = string.Format("{0} was updated in system !", Session["CustCode"]);
        //        TempData["DeleteMessage"] = "";
        //        return RedirectToAction("Edit2", new { id = Session["CustID"] });
        //    }
        //}

        // GET: Customer/Create
        public ActionResult Create()
        {
            //return PartialView();
            TempData["Message"] = "";
            TempData["DeteleMessage"] = "";
            Session["CustID"] = null;
            return View(new CustomerDimensionRelationshipModel());
        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ChartOfAccountID,VendorID,CurrencyID,TaxCodeID,CustomerCode,Name,ContactPerson,Address,Town,State,PostCode,CountryCode,TelNo,FaxNo,Email,AddChrg,AccCode,CreditTerm,TermType,CreditLimit,Status,CreateDate,UpdateDate,UpdateID,VendorCode,CurrencyCode,BillPartyBRN,BillPartyGSTNo,DateGST,TaxCode,FinBillPartyCode")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                //db.Customer.Add(customer);
                //db.SaveChanges();
                customer.CreateDate = DateTime.Now;
                customer.UpdateDate = DateTime.Now;
                customer.UpdateID = Session["UserID"].ToString();
                customer.Status = "Active";
                customerRepository.SaveCustomer(customer);
                TempData["Message"] = string.Format("{0} was added", customer.CustomerCode);
                return RedirectToAction("Index");
            }

            //ViewBag.ChartOfAccountID = new SelectList(db.ChartOfAccount, "ID", "AccCode", customer.ChartOfAccountID);
            return View(customer);
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customerdimensionrelationshipmodel = new CustomerDimensionRelationshipModel();

            customerdimensionrelationshipmodel.Customer = (from p in customerRepository.Customer
                                                           where p.ID.Equals(id)
                                                           orderby p.CustomerCode
                                                           select p).FirstOrDefault();
            if (id != null)
            {
                customerdimensionrelationshipmodel.Dimension_TableRelationship = (from p in dimension_tablerelationshipRepository.Dimension_TableRelationship
                                                                                  where p.DimensionTable.Equals("Customer")
                                                                            select p);
            }

            //chartofaccountdimensionmodel.DimensionSetup = from m in chartofaccountdimensionmodel.ChartOfAccount.DimensionSetup
            //                                              orderby m.DimensionCode
            //                          select m;

            if (customerdimensionrelationshipmodel == null)
            {
                return HttpNotFound();
            }
            return PartialView(customerdimensionrelationshipmodel);

            //var model = (from p in customerRepository.Customer
            //             where p.ID.Equals(id)
            //             orderby p.CustomerCode
            //             select p).FirstOrDefault();

            //if (model == null)
            //{
            //    return HttpNotFound();
            //}
            //return PartialView(model);
            ////Customer customer = db.Customer.Find(id);
            //if (customer == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.ChartOfAccountID = new SelectList(db.ChartOfAccount, "ID", "AccCode", customer.ChartOfAccountID);
            //return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ChartOfAccountID,VendorID,CurrencyID,TaxCodeID,CustomerCode,Name,ContactPerson,Address,Town,State,PostCode,CountryCode,TelNo,FaxNo,Email,AddChrg,AccCode,CreditTerm,TermType,CreditLimit,Status,CreateDate,UpdateDate,UpdateID,VendorCode,CurrencyCode,BillPartyBRN,BillPartyGSTNo,DateGST,TaxCode,FinBillPartyCode")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(customer).State = EntityState.Modified;
                //db.SaveChanges();
                customer.UpdateDate = DateTime.Now;
                customer.CreateDate = DateTime.Now;
                customer.UpdateID = Session["UserID"].ToString();
                customer.Status = "Active";
                customerRepository.SaveCustomer(customer);
                ViewBag.Message = string.Format("{0} was updated", customer.CustomerCode);
                return RedirectToAction("Index");
            }
            //ViewBag.ChartOfAccountID = new SelectList(db.ChartOfAccount, "ID", "AccCode", customer.ChartOfAccountID);
            return View(customer);
        }

        // GET: Customer/Edit/5
        public ActionResult Edit2(int? id)
        {
            Session["CustID"] = String.Empty;
            TempData["Message"] = "";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customerdimensionrelationshipmodel = new CustomerDimensionRelationshipModel();

            //To improve performance
            customerdimensionrelationshipmodel.Customer = customerRepository.GetCustomerByID((long)id).FirstOrDefault();

            if (id != null)
            {
                //improve performance
                customerdimensionrelationshipmodel.Customer_Dim_Setup = customerdimensionrelationshipmodel.Customer.Customer_Dim_Setup;
            }

            if (customerdimensionrelationshipmodel == null)
            {
                return HttpNotFound();
            }
            return View("Edit2", customerdimensionrelationshipmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2([Bind(Include = "ID,ChartOfAccountID,VendorID,CurrencyID,TaxCodeID,CustomerCode,Name,ContactPerson,Address,Town,State,PostCode,CountryCode,TelNo,FaxNo,Email,AddChrg,AccCode,CreditTerm,TermType,CreditLimit,Status,CreateDate,UpdateDate,UpdateID,VendorCode,CurrencyCode,BillPartyBRN,BillPartyGSTNo,DateGST,TaxCode,FinBillPartyCode")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.UpdateDate = DateTime.Now;
                customer.CreateDate = DateTime.Now;
                customer.UpdateID = Session["UserID"].ToString();
                customer.Status = "Active";
                customerRepository.SaveCustomer(customer);
                ViewBag.Message = string.Format("{0} was updated in system !", customer.CustomerCode);
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from p in customerRepository.Customer
                         where p.ID.Equals(id)
                         orderby p.ID
                         select p).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
            //Customer customer = db.Customer.Find(id);
            //if (customer == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Int64 id)
        {
            //Customer customer = db.Customer.Find(id);
            //db.Customer.Remove(customer);
            //db.SaveChanges();
            Customer customer = customerRepository.Customer.FirstOrDefault(p => p.ID == id);
            if (customer != null)
            {
                customerRepository.DeleteCustomer(customer);
                TempData["DeleteMessage"] = string.Format("{0} was deleted", customer.CustomerCode);
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
