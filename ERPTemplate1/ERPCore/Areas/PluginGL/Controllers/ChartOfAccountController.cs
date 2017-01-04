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

namespace PluginGL.Controllers
{
    public class ChartOfAccountController : BaseController
    {
        //private EFDbContext db = new EFDbContext();
        private IChartOfAccountRepository chartofaccountRepository;
        private IAccountGroupRepository accountgroupRepository;
        private IDimension_TableRelationshipRepository dimension_tablerelationshipRepository;
        private IChartOfAccount_Dim_SetupRepository chartofaccount_dim_setupRepository;
        private IChartOfAccount_Dim_ValueRepository chartofaccount_dim_valueRepository;
        private IDimension_SettingRepository dimension_settingRepositoty;
        private IDimension_ValueRepository dimension_valueRepository;
        private List<DimensionValueModel> modelDB = new List<DimensionValueModel>();
        
        public ChartOfAccountController(IChartOfAccountRepository chartofaccountRepo, IAccountGroupRepository accountgroupRepo, IDimension_TableRelationshipRepository dimension_tablerelationshipRepo, IChartOfAccount_Dim_SetupRepository chartofaccount_dim_setupRepo, IChartOfAccount_Dim_ValueRepository chartofaccount_dim_valueRepo, IDimension_SettingRepository dimension_settingRepo, IDimension_ValueRepository dimension_valueRepo)
        {
            this.chartofaccountRepository = chartofaccountRepo;
            this.accountgroupRepository = accountgroupRepo;
            this.dimension_tablerelationshipRepository = dimension_tablerelationshipRepo;
            this.chartofaccount_dim_setupRepository = chartofaccount_dim_setupRepo;
            this.chartofaccount_dim_valueRepository = chartofaccount_dim_valueRepo;
            this.dimension_settingRepositoty = dimension_settingRepo;
            this.dimension_valueRepository = dimension_valueRepo;
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadChartOfAccountDimension(List<Dimension_TableRelationship> dimension_tablerelationship)
        {
            if (dimension_tablerelationship != null)
            {
                foreach (var item in dimension_tablerelationship)
                {
                    Dimension_TableRelationship a = (from p in dimension_tablerelationshipRepository.Dimension_TableRelationship
                                                     where p.ID.Equals(item.ID)
                                                       select p).FirstOrDefault();
                    a.DimensionTable = item.DimensionTable.Trim();
                    dimension_tablerelationshipRepository.SaveDimension_TableRelationship(a);
                }
            }
            TempData["Message"] = string.Format("{0} was updated in system !", Session["AccCode"]);
            TempData["DeleteMessage"] = "";
            return RedirectToAction("Edit2", new { id = Session["AccID"] });
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult LoadChartOfAccountDimension(List<ChartOfAccount_DimensionValue> chartofaccount_dimensionvalue)
        //{
        //    if (chartofaccount_dimensionvalue != null)
        //    {
        //        foreach (var item in chartofaccount_dimensionvalue)
        //        {
        //            ChartOfAccount_DimensionValue d = (from p in chartofaccount_dimensionvalueRepository.ChartOfAccount_DimensionValue
        //                                               where p.ID.Equals(item.ID)
        //                                               select p).FirstOrDefault();
        //            d.AccCode = item.AccCode;
        //            d.DimensionCode = item.DimensionCode;
        //            d.DimensionValue = item.DimensionValue;
        //            chartofaccount_dimensionvalueRepository.SaveChartOfAccount_DimensionValue(d);
        //        }
        //    }
        //}

        public PartialViewResult LoadChartOfAccountDimension(Int64? AccId, string AccCode)
        {
            Session["AccID"] = AccId;
            Session["AccCode"] = AccCode.Trim();
            
            List<DimensionModel> model = new List<DimensionModel>();
            var q = (from dtr in dimension_tablerelationshipRepository.Dimension_TableRelationship
                         join ds in dimension_settingRepositoty.Dimension_Setting
                         on dtr.Dimension_SettingID equals ds.ID
                     where dtr.DimensionTable.Equals("ChartOfAccount")
                     select new
                     {
                         dtrID = dtr.ID,
                         dsID = ds.ID,
                         dsDimensionCode = ds.DimensionCode,
                         dsDescription = ds.Description
                     }).ToList();//convert to List
            foreach (var item in q) //retrieve each item and assign to model
            {
                model.Add(new DimensionModel()
                {
                    DimensionTableRelationshipID = item.dtrID,
                    DimensionSettingID = item.dsID,
                    DimensionCode = item.dsDimensionCode,
                    DimensionSettingDescription = item.dsDescription
                });
            }

            //return PartialView("LoadChartOfAccountDimension", model.ToList());
            return PartialView(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public PartialViewResult LoadChartOfAccountDimension1(string AccCode)
        //{
        //    Session["AccCode"] = AccCode;
        //    var model = from p in chartofaccount_dimensionvalueRepository.ChartOfAccount_DimensionValue
        //                where p.AccCode.Equals(AccCode)
        //                orderby p.AccCode
        //                select p;
        //    return PartialView("LoadChartOfAccountDimension1", model.ToList());
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        private PartialViewResult LoadChartOfAccountDimension0(Int64? AccId, string AccCode)
        {
            Session["AccId"] = AccId;
            Session["AccCode"] = AccCode;
            var model = from p in dimension_tablerelationshipRepository.Dimension_TableRelationship
                        where p.DimensionTable.Equals("ChartOfAccount")
                        select p;
            return PartialView("LoadChartOfAccountDimension0", model.ToList());
        }


        // GET: ChartOfAccount
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

            int pageSize = Int32.Parse(CommonUtility.Empty2Zero(System.Web.Configuration.WebConfigurationManager.AppSettings["RowPerPage"]));
            int pageNumber = (page ?? 1);
            if (page == null)
            {
                page = 1;
            }

            var model = (from m in chartofaccountRepository.ChartOfAccount
                         select m);

            //int iRecCnt = invoiceRepository.InvoiceCount();
            Int64 iRecCnt = chartofaccountRepository.GetMaxID();

            if (!String.IsNullOrEmpty(searchValue))
            {
                model = (from m in chartofaccountRepository.ChartOfAccountWildSearch("AccCode", searchValue)
                         select m);
                iRecCnt = model.Count();
            }
            else
            {
                //model = model.Skip(((int)page - 1) * pageSize)
                //       .Take(pageSize);
                //improve performance
                model = chartofaccountRepository.GetChartOfAccountPaging(((((long)page - 1) * pageSize) + 1), (long)page * pageSize);
            }

            switch (sortOrder)
            {
                default:
                    //model = model.OrderBy(s => s.ID);
                    break;
            }

            //return View(model.ToPagedList(pageNumber, pageSize));
            //improve performance
            var model3 = new StaticPagedList<ChartOfAccount>(model, (int)page, pageSize, (int)iRecCnt);
            return View(model3);
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            //if (searchValue != null)
            //{
            //    page = 1;
            //}
            //else
            //{
            //    searchValue = currentFilter;
            //}

            //ViewBag.CurrentFilter = searchValue;

            //var model = from m in chartofaccountRepository.ChartOfAccount
            //            select m;

            //if (!String.IsNullOrEmpty(searchValue))
            //{
            //    model = model.Where(s => s.AccCode.ToUpper().Contains(searchValue.ToUpper()));
            //}

            //switch (sortOrder)
            //{
            //    case "name_desc":
            //        model = model.OrderByDescending(s => s.AccCode);
            //        break;
            //    //case "Date":
            //    //    students = students.OrderBy(s => s.EnrollmentDate);
            //    //    break;
            //    //case "date_desc":
            //    //    students = students.OrderByDescending(s => s.EnrollmentDate);
            //    //    break;
            //    default:
            //        //model = model.OrderBy(s => s.AccCode);
            //        break;
            //}

            //int pageSize = 10;
            //int pageNumber = (page ?? 1);
            //return View(model.ToPagedList(pageNumber, pageSize));

            //if (model == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(model);
            //return View(db.ChartOfAccounts.ToList());
        }

        // GET: ChartOfAccount/Details/5
        public ActionResult Details(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from p in chartofaccountRepository.ChartOfAccount
                         where p.ID.Equals(id)
                         orderby p.AccCode
                         select p).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView(model);
            //ChartOfAccount chartOfAccount = db.ChartOfAccounts.Find(id);
            //if (chartOfAccount == null)
            //{
            //   return HttpNotFound();
            //}
            //return View(chartOfAccount);
        }
                  
        // GET: ChartOfAccount/Create
        public ActionResult Create()
        {
            TempData["Message"] = "";
            TempData["DeteleMessage"] = "";
            //Session["AccCode"] = "";
            //Session["AccID"] = "";
            Session["AccID"] = null;
            PopulateAccountGroupDropDownList();
            //PopulateDimensionSetupDropDownList();
            return View(new ChartOfAccountDimensionModel());
            //return View();
        }

        // POST: ChartOfAccount/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AccountGroupID,AccCode,Description,AccType,AccGrpCode,ActCode,SubActCode,ExpenseCode,AccPurpose,Status,CreateDate,UpdateDate,UpdateID,FinAccCode,CreateID,WSAccDistUse,ControlAcc,SuspendAcc,Blocked,EffDateFrom,EffDateTo,ReportDESC,SubAccType")] ChartOfAccount chartOfAccount)
        {
            TempData["Message"] = "";
            TempData["DeteleMessage"] = "";

            var chartofaccountdimensionModel = new ChartOfAccountDimensionModel();

            if (ModelState.IsValid)
            {
                //db.ChartOfAccounts.Add(chartOfAccount);
                //db.SaveChanges();
                chartOfAccount.CreateDate = DateTime.Now;
                chartOfAccount.UpdateDate = DateTime.Now;
                chartOfAccount.UpdateID = Session["UserID"].ToString();
                chartOfAccount.Status = "Active";
                int iAccountGroup = Int32.Parse(Request.Form["AccountGroup"].ToString());
                chartOfAccount.AccountGroupID = iAccountGroup;
                AccountGroup accountgroup = accountgroupRepository.AccountGroup.FirstOrDefault(p => p.ID == iAccountGroup);
                chartOfAccount.AccGrpCode = accountgroup.AccGrpCode.Trim();
                var accountRec = from m in chartofaccountRepository.ChartOfAccountWildSearch("AccCode", chartOfAccount.AccCode)
                                 select m;
                int iRecCnt = accountRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The Account Code " + chartOfAccount.AccCode + " already existed in system ! ";
                }
                else
                {
                    chartofaccountRepository.SaveChartOfAccount(chartOfAccount);
                    Session["AccID"] = chartOfAccount.ID;
                    Session["AccCode"] = chartOfAccount.AccCode;

                    chartofaccountdimensionModel.ChartOfAccount = (from p in chartofaccountRepository.ChartOfAccount
                                                                   where p.ID.Equals(chartOfAccount.ID)
                                                                    orderby p.AccCode
                                                                   select p).FirstOrDefault();
                    if (chartOfAccount.ID != 0)
                    {
                        //chartofaccountdimensionModel.Dimension_TableRelationship = from p in dimension_tablerelationshipRepository.Dimension_TableRelationship
                        //                               //where p.ID.Equals(chartOfAccount.ID)
                        //                               select p;
                        
                    }
                    Session["DimensionID"] = "";

                    TempData["Message"] = string.Format("{0} was added in the system !", chartOfAccount.AccCode);

                }
                return RedirectToAction("Index");
            }

            return View(chartofaccountdimensionModel);
        }

        public ActionResult CreateChartOfAccountDimension(string AccCode)
        {
            PopulateAccountGroupDropDownList();
            //return PartialView(new ChartOfAccount_DimensionValue());
            return View(new ChartOfAccount_DimensionValue());
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateChartOfAccountDimension([Bind(Include = "ID,ChartOfAccountID,AccCode,DimensionCode,DimensionValue,Status")] ChartOfAccount_DimensionValue chartofaccount_dimensionvalue)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        chartofaccount_dimensionvalue.ChartOfAccountID = Int32.Parse(Session["AccID"].ToString());
        //        //chartofaccount_dimensionvalueRepository.SaveChartOfAccount_DimensionValue(chartofaccount_dimensionvalue);
        //        ////return RedirectToAction("Index");
        //        TempData["Message"] = "Record was added in system !";
        //        TempData["DeleteMessage"] = "";
        //        //PopulateAccountGroupDropDownList(ChartOfAccount_DimensionValue);
        //        return RedirectToAction("Edit", new { id = chartofaccount_dimensionvalue.ChartOfAccountID });
        //    }
        //    return View(chartofaccount_dimensionvalue);
        //}

        private void PopulateAccountGroupDropDownList(object selectedAcc = null)
        {
            var accountgroupQuery = from d in accountgroupRepository.AccountGroup
                              orderby d.AccGrpCode
                              select d;
            ViewBag.AccountGroup = new SelectList(accountgroupQuery, "ID", "AccGrpCode", selectedAcc);
        }

        // GET: ChartOfAccount/Edit/5
        public ActionResult Edit(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Session["AccID"] = id;

            var chartofaccountdimensionmodel = new ChartOfAccountDimensionModel();

            chartofaccountdimensionmodel.ChartOfAccount = (from p in chartofaccountRepository.ChartOfAccount
                                       where p.ID.Equals(id)
                                       orderby p.AccCode
                                       select p).FirstOrDefault();
            if (id != null)
            {
                //chartofaccountdimensionmodel.Dimension_TableRelationship = chartofaccountdimensionmodel.ChartOfAccount.Dimension_TableRelationship;
                //chartofaccountdimensionmodel.Dimension_TableRelationship = (from p in chartofaccountRepository.ChartOfAccount
                //                                                            where p.ID.Equals(id)
                //                                                            orderby p.AccCode
                //                                                            select p);
                chartofaccountdimensionmodel.Dimension_TableRelationship = (from p in dimension_tablerelationshipRepository.Dimension_TableRelationship
                                                                            select p);
                PopulateAccountGroupDropDownList(chartofaccountdimensionmodel.ChartOfAccount.AccountGroupID);
            }

            //chartofaccountdimensionmodel.DimensionSetup = from m in chartofaccountdimensionmodel.ChartOfAccount.DimensionSetup
            //                                              orderby m.DimensionCode
            //                          select m;

            if (chartofaccountdimensionmodel == null)
            {
                return HttpNotFound();
            }
            return PartialView(chartofaccountdimensionmodel);

            
        }

        // POST: ChartOfAccount/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AccountGroupID,AccCode,Description,AccType,AccGrpCode,ActCode,SubActCode,ExpenseCode,AccPurpose,Status,CreateDate,UpdateDate,UpdateID,FinAccCode,CreateID,WSAccDistUse,ControlAcc,SuspendAcc,Blocked,EffDateFrom,EffDateTo,ReportDESC,SubAccType")] ChartOfAccount chartOfAccount)
        {
            TempData["Message"] = "";
            TempData["DeteleMessage"] = "";

            if (ModelState.IsValid)
            {
                //db.Entry(chartOfAccount).State = EntityState.Modified;
                //db.SaveChanges();
                chartOfAccount.UpdateDate = DateTime.Now;
                chartOfAccount.CreateDate = DateTime.Now;
                chartOfAccount.UpdateID = Session["UserID"].ToString();
                chartOfAccount.Status = "Active";
                int iAccountGroup = Int32.Parse(Request.Form["AccountGroup"].ToString());
                chartOfAccount.AccountGroupID = iAccountGroup;
                AccountGroup accountgroup = accountgroupRepository.AccountGroup.FirstOrDefault(p => p.ID == iAccountGroup);
                chartOfAccount.AccGrpCode = accountgroup.AccGrpCode.Trim();
                PopulateAccountGroupDropDownList(chartOfAccount.AccountGroupID);
                chartofaccountRepository.SaveChartOfAccount(chartOfAccount);
                ViewBag.Message = string.Format("{0} was updated in system !", chartOfAccount.AccCode);
                
                return RedirectToAction("Index");
            }
            //return View(chartOfAccount);
            return PartialView(chartOfAccount);            
        }

        // GET: ChartOfAccount/Edit/5
        public ActionResult Edit2(int? id)
        {
            Session["AccID"] = id;
            TempData["Message"] = "";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var chartofaccountdimensionmodel = new ChartOfAccountDimensionModel();

            //chartofaccountdimensionmodel.ChartOfAccount = (from p in chartofaccountRepository.ChartOfAccount
            //                                               where p.ID.Equals(id)
            //                                               orderby p.AccCode
            //                                               select p).FirstOrDefault();
            //To improve performance
            chartofaccountdimensionmodel.ChartOfAccount = chartofaccountRepository.GetChartOfAccountByID((long)id).FirstOrDefault();

            if (id != null)
            {
                //chartofaccountdimensionmodel.ChartOfAccount_DimensionValue = chartofaccountdimensionmodel.ChartOfAccount.ChartOfAccount_DimensionValue;
                //improve performance
                //chartofaccountdimensionmodel. = chartofaccount_dimensionvalueRepository.GetChartOfAccount_DimensionValue("ChartOfAccountID", (long)id);
                PopulateAccountGroupDropDownList(chartofaccountdimensionmodel.ChartOfAccount.AccountGroupID);
            }

            if (chartofaccountdimensionmodel == null)
            {
                return HttpNotFound();
            }
            return View("Edit2", chartofaccountdimensionmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2([Bind(Include = "ID, AccountGroupID, AccCode, Description, AccType, AccGrpCode, ActCode, SubActCode, ExpenseCode, AccPurpose, Status, CreateDate, UpdateDate, UpdateID, FinAccCode, CreateID, WSAccDistUse, ControlAcc, SuspendAcc, Blocked, EffDateFrom, EffDateTo, ReportDESC, SubAccType")] ChartOfAccount chartofaccount)
        {
            if (ModelState.IsValid)
            {
                chartofaccount.UpdateDate = DateTime.Now;
                chartofaccount.CreateDate = DateTime.Now;
                chartofaccount.UpdateID = Session["UserID"].ToString();
                chartofaccount.Status = "Active";
                int iAccountGroup = Int32.Parse(Request.Form["AccountGroup"].ToString());
                chartofaccount.AccountGroupID = iAccountGroup;
                AccountGroup accountgroup = accountgroupRepository.AccountGroup.FirstOrDefault(p => p.ID == iAccountGroup);
                chartofaccount.AccGrpCode = accountgroup.AccGrpCode.Trim();
                PopulateAccountGroupDropDownList(chartofaccount.AccountGroupID);
                chartofaccountRepository.SaveChartOfAccount(chartofaccount);
                ViewBag.Message = string.Format("{0} was updated in system !", chartofaccount.AccCode);
                return RedirectToAction("Index");
            }
            return View(chartofaccount);
        }
        
        // GET: ChartOfAccount/Delete/5
        public ActionResult Delete(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from p in chartofaccountRepository.ChartOfAccount
                         where p.ID.Equals(id)
                         orderby p.ID
                         select p).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
            //ChartOfAccount chartOfAccount = db.ChartOfAccounts.Find(id);
            //if (chartOfAccount == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(chartOfAccount);
        }

        // POST: ChartOfAccount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Int64 id)
        {
            //ChartOfAccount chartOfAccount = db.ChartOfAccounts.Find(id);
            //db.ChartOfAccounts.Remove(chartOfAccount);
            //db.SaveChanges();
            ChartOfAccount chartOfAccount = chartofaccountRepository.ChartOfAccount.FirstOrDefault(p => p.ID == id);
            if (chartOfAccount != null)
            {
                chartofaccountRepository.DeleteChartOfAccount(chartOfAccount);
                TempData["DeleteMessage"] = string.Format("{0} was deleted", chartOfAccount.AccCode);
            }
            return RedirectToAction("Index");
        }

        //public ActionResult DeleteChartOfAccountDimension(Int64 AccId, Int64 DimId )
        //{

        //    ChartOfAccount_DimensionValue chartofaccount_dimensionvalue = chartofaccount_dimensionvalueRepository.ChartOfAccount_DimensionValue.FirstOrDefault(p => p.ID == DimId);

        //    if (chartofaccount_dimensionvalue != null)
        //    {
        //        string functionName = chartofaccount_dimensionvalue.DimensionValue;

        //        chartofaccount_dimensionvalueRepository.DeleteChartOfAccount_DimensionValue(chartofaccount_dimensionvalue);
        //        TempData["Message"] = "";
        //        TempData["DeleteMessage"] = string.Format("{0} was deleted", functionName);

        //    }
        //    return RedirectToAction("Edit2", new { id = AccId });
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        public ActionResult EditDimensionPredefinedValue(Int64 dimensionSettingId)
        {
            Session["DimensionSettingId"] = dimensionSettingId;

            modelDB.Clear();
            List<DimensionValueModel> model = new List<DimensionValueModel>();
            List<DimensionValueModel> modelLeft = new List<DimensionValueModel>();
            List<DimensionValueModel> modelRight = new List<DimensionValueModel>();
            var q = (from dv in dimension_valueRepository.Dimension_Value
                     where dv.Dimension_SettingID.Equals(dimensionSettingId)
                     select new
                     {
                         dvDimensionValueId = dv.ID,
                         dvDimensionSettingId = dimensionSettingId,
                         dvDimensionValue = dv.DimensionValue
                     }).ToList();//convert to List
            foreach (var item in q) //retrieve each item and assign to model
            {
                model.Add(new DimensionValueModel()
                {
                    DimensionValueID = item.dvDimensionValueId,
                    DimensionSettingID = item.dvDimensionSettingId,
                    DimensionValue = item.dvDimensionValue,
                    RequestedDimensionValue = "",
                    ChartOfAccountID = (long)Session["AccID"]
                });
            }

            var q2 = (from dv in chartofaccount_dim_valueRepository.ChartOfAccount_Dim_Value
                     join st in chartofaccount_dim_setupRepository.ChartOfAccount_Dim_Setup
                     on dv.ChartOfAccount_Dim_SetupID equals st.ID
                     where st.Dimension_SettingID.Equals(dimensionSettingId) && st.ChartOfAccountID.Equals((long)Session["AccID"])
                     select new
                     {
                         dvDimensionValueID = dv.ID,
                         stDimensionSettingID = st.Dimension_SettingID,
                         stChartOfAccountID = st.ChartOfAccountID,
                         dvDimensionValue = dv.DimensionValue
                     }).ToList();//convert to List

            foreach (var item in q2) //retrieve each item and assign to model
            {
                modelDB.Add(new DimensionValueModel()
                {
                    DimensionSettingID = item.stDimensionSettingID,
                    DimensionValue = item.dvDimensionValue,
                    RequestedDimensionValue = "",
                    ChartOfAccountID = item.stChartOfAccountID
                });
            }
            if (modelDB.Count() > 0)
            {
                
                foreach (var item in modelDB)
                {
                    Dimension_Value dimensionValue = dimension_valueRepository.Dimension_Value.FirstOrDefault(p => p.DimensionValue == item.DimensionValue);
                    item.DimensionValueID = dimensionValue.ID;
                }
            }
            
            long[] availableSelected = new long[modelDB.Count()];
            ViewDimensionValueModel viewModel = null;
            if (modelDB.Count() > 0)
            {
                int i = 0;
                foreach (var item in modelDB)
                {
                    model.RemoveAll(a => a.DimensionValue == item.DimensionValue);
                    availableSelected[i] = item.DimensionValueID;
                    i++;
                }
                viewModel = new ViewDimensionValueModel { AvailableDimensionValues = model, RequestedDimensionValues = modelDB };
                viewModel.SavedRequested = string.Join(",", viewModel.RequestedDimensionValues.Select(p => p.DimensionValueID.ToString()).ToArray());
            }
            else
            {
                viewModel = new ViewDimensionValueModel { AvailableDimensionValues = model, RequestedDimensionValues = new List<DimensionValueModel>() };

            }
            

            

            return PartialView(viewModel);
        }

        [HttpPost]
        public ActionResult EditDimensionPredefinedValue(ViewDimensionValueModel model, string add, string remove, string save, string cancel)
        {
            //Need to clear model state or it will interfere with the updated model
            ModelState.Clear();
            RestoreSavedState(model);
            if (!string.IsNullOrEmpty(add))
                AddProducts(model);
            else if (!string.IsNullOrEmpty(remove))
                RemoveProducts(model);
            else if (!string.IsNullOrEmpty(save))
            {
                SaveState(model);
                SaveDimensionPredefinedValue(model);
            }
            SaveState(model);
            return PartialView(model);
        }


        public void SaveDimensionPredefinedValue(ViewDimensionValueModel model)
        {
            long cdsID = 0;
            foreach (var item in model.RequestedDimensionValues)
            {
                System.Console.WriteLine("ChartOfAccountID=" + item.ChartOfAccountID);
                System.Console.WriteLine("DimensionSettingID=" + item.DimensionSettingID);
                System.Console.WriteLine("DimensionValueID=" + item.DimensionValueID);
                System.Console.WriteLine("DimensionValue=" + item.DimensionValue);
                ChartOfAccount_Dim_Setup cds = chartofaccount_dim_setupRepository.ChartOfAccount_Dim_Setup.FirstOrDefault(p => p.ChartOfAccountID.Equals(item.ChartOfAccountID) && p.Dimension_SettingID.Equals(item.DimensionSettingID));
                if (cds != null)
                {
                    cdsID = cds.ID;
                    ChartOfAccount_Dim_Value cdv = chartofaccount_dim_valueRepository.ChartOfAccount_Dim_Value.FirstOrDefault(p => p.ChartOfAccount_Dim_SetupID.Equals(cdsID) && p.DimensionValue.Equals(item.DimensionValue));
                    if (cdv == null)
                    {
                        ChartOfAccount_Dim_Value newCdv = new ChartOfAccount_Dim_Value();
                        newCdv.ChartOfAccount_Dim_SetupID = cdsID;
                        newCdv.DimensionValue = item.DimensionValue;
                        chartofaccount_dim_valueRepository.SaveChartOfAccount_Dim_Value(newCdv);
                    }
                }
            }

            if (cdsID != 0)
            {
                var cdvModel = (from p in chartofaccount_dim_valueRepository.ChartOfAccount_Dim_Value
                             where p.ChartOfAccount_Dim_SetupID.Equals(cdsID)
                             select p);
                foreach(var itemDB in cdvModel)
                {
                    long dvID = itemDB.ID;
                    string dvDB = itemDB.DimensionValue;
                    string dv = "";
                    bool matchFound = false;
                    foreach(var item in model.RequestedDimensionValues)
                    {
                        dv = item.DimensionValue;
                        if (dvDB.Equals(dv))
                        {
                            matchFound = true;
                        }
                    }
                    if (matchFound == false)
                    {
                        ChartOfAccount_Dim_Value cdv = chartofaccount_dim_valueRepository.ChartOfAccount_Dim_Value.FirstOrDefault(p => p.ID.Equals(dvID));
                        if (cdv != null)
                        {
                            chartofaccount_dim_valueRepository.DeleteChartOfAccount_Dim_Value(cdv);
                        }
                    }
                }
            }

        }

        public List<DimensionValueModel> GetAllDimensionValues()
        {
            List<DimensionValueModel> model = new List<DimensionValueModel>();
            var q = (from dv in dimension_valueRepository.Dimension_Value
                     where dv.Dimension_SettingID.Equals(Session["DimensionSettingId"])
                     select new
                     {
                         dvDimensionValueId = dv.ID,
                         dvDimensionSettingId = (long)Session["DimensionSettingId"],
                         dvDimensionValue = dv.DimensionValue
                     }).ToList();//convert to List
            foreach (var item in q) //retrieve each item and assign to model
            {
                model.Add(new DimensionValueModel()
                {
                    DimensionValueID = item.dvDimensionValueId,
                    DimensionSettingID = item.dvDimensionSettingId,
                    DimensionValue = item.dvDimensionValue,
                    RequestedDimensionValue = "",
                    ChartOfAccountID = (long)Session["AccID"]
                });
            }

            var q2 = (from dv in chartofaccount_dim_valueRepository.ChartOfAccount_Dim_Value
                      join st in chartofaccount_dim_setupRepository.ChartOfAccount_Dim_Setup
                      on dv.ChartOfAccount_Dim_SetupID equals st.ID
                      where st.Dimension_SettingID.Equals((long)Session["DimensionSettingId"]) && st.ChartOfAccountID.Equals((long)Session["AccID"])
                      select new
                      {
                          dvDimensionValueID = dv.ID,
                          stDimensionSettingID = st.Dimension_SettingID,
                          stChartOfAccountID = st.ChartOfAccountID,
                          dvDimensionValue = dv.DimensionValue
                      }).ToList();//convert to List
            foreach (var item in q2) //retrieve each item and assign to model
            {
                modelDB.Add(new DimensionValueModel()
                {
                    //DimensionValueID = item.dvDimensionValueID,
                    DimensionSettingID = item.stDimensionSettingID,
                    DimensionValue = item.dvDimensionValue,
                    RequestedDimensionValue = "",
                    ChartOfAccountID = item.stChartOfAccountID
                });
            }
            foreach (var item in modelDB)
            {
                Dimension_Value dimensionValue = dimension_valueRepository.Dimension_Value.FirstOrDefault(p => p.DimensionValue == item.DimensionValue);
                item.DimensionValueID = dimensionValue.ID;
                //model.RemoveAll(a => a.DimensionValue == item.DimensionValue);
            }
            

            return model;
        }

        #region "ListboxFor Support"

        private void Validate(ViewDimensionValueModel model)
        {
            
        }
        void SaveState(ViewDimensionValueModel model)
        {
            //create comma delimited list of product ids
            model.SavedRequested = string.Join(",", model.RequestedDimensionValues.Select(p => p.DimensionValueID.ToString()).ToArray());

            //Available products = All - Requested
            //model.AvailableDimensionValues = GetAllDimensionValues().Except(model.RequestedDimensionValues).ToList();
            List<DimensionValueModel> dvm = GetAllDimensionValues();
            foreach (var item in model.RequestedDimensionValues)
            {
                dvm.RemoveAll(a => a.DimensionValueID == item.DimensionValueID);
            }
            model.AvailableDimensionValues = dvm.ToList();
            //snippets.RemoveAll(a => a.Code == "1");
            //foreach (var money in myMoney)
            //{
            //    Console.WriteLine("Amount is {0} and type is {1}", money.amount, money.type);
            //}
        }

        void RemoveProducts(ViewDimensionValueModel model)
        {
            if (model.RequestedSelected != null)
            {
                model.RequestedDimensionValues.RemoveAll(p => model.RequestedSelected.Contains(p.DimensionValueID));
                model.RequestedSelected = null;
            }
        }

        void AddProducts(ViewDimensionValueModel model)
        {
            if (model.AvailableSelected != null)
            {
                var dimValues = GetAllDimensionValues().Where(p => model.AvailableSelected.Contains(p.DimensionValueID));
                model.RequestedDimensionValues.AddRange(dimValues);
                model.AvailableSelected = null;
            }
        }

        void RestoreSavedState(ViewDimensionValueModel model)
        {
            model.RequestedDimensionValues = new List<DimensionValueModel>();

            //get the previously stored items
            if (!string.IsNullOrEmpty(model.SavedRequested))
            {
                string[] ids = model.SavedRequested.Split(',');
                var dimValues = GetAllDimensionValues().Where(p => ids.Contains(p.DimensionValueID.ToString()));
                model.RequestedDimensionValues.AddRange(dimValues);
            }
        }
        #endregion
    }
}
