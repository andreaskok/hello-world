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

namespace ERPCore.Controllers
{
    public class Dimension_SettingController : Controller
    {
        //private EFDbContext db = new EFDbContext();
        private IDimension_SettingRepository dimension_settingRepository;
        private IDimension_ValueRepository dimension_valueRepository;
        private IDimension_TableRelationshipRepository dimension_tablerelationshipRepository;

        public Dimension_SettingController(IDimension_TableRelationshipRepository dimension_tablerelationshipRepo,IDimension_SettingRepository dimension_settingRepo, IDimension_ValueRepository dimension_valueRepo)
        {
            this.dimension_tablerelationshipRepository = dimension_tablerelationshipRepo;
            this.dimension_settingRepository = dimension_settingRepo;
            this.dimension_valueRepository = dimension_valueRepo;
        }

        // GET: Dimension_Setting
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

            var model = (from m in dimension_settingRepository.Dimension_Setting
                         select m);

            Int64 iRecCnt = dimension_settingRepository.GetMaxID();

            if (!String.IsNullOrEmpty(searchValue))
            {
                model = (from m in dimension_settingRepository.Dimension_SettingWildSearch("DimensionCode", searchValue)
                         select m);
                iRecCnt = model.Count();
            }
            else
            {
                //model = model.Skip(((int)page - 1) * pageSize)
                //       .Take(pageSize);
                //improve performance
                model = dimension_settingRepository.GetDimension_SettingPaging(((((long)page - 1) * pageSize) + 1), (long)page * pageSize);
            }

            switch (sortOrder)
            {
                default:
                    //model = model.OrderBy(s => s.ID);
                    break;
            }

            var model3 = new StaticPagedList<Dimension_Setting>(model, (int)page, pageSize, (int)iRecCnt);
            return View(model3);
        }


        // GET: Dimension_Setting/Details/5
        public ActionResult Details(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from p in dimension_settingRepository.Dimension_Setting
                         where p.ID.Equals(id)
                         orderby p.DimensionCode
                         select p).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView(model);
        }

        // GET: Dimension Value
        public PartialViewResult LoadDimensionValue(Int64? DimId, string DimCode)
        {
            Session["DimId"] = DimId;
            Session["DimCode"] = DimCode.Trim();

            int DimID = Int32.Parse(Session["DimID"].ToString());

            List<DimensionValueListModel> model = new List<DimensionValueListModel>();
            var q = (from b in dimension_valueRepository.Dimension_Value
                     where b.Dimension_SettingID.Equals(DimId)
                     select new
                     {
                         DimensionValue = b.DimensionValue,
                         DimensionSettingID = b.Dimension_SettingID,
                         DimensionStatus = b.Status

                     }).ToList();//convert to List
            foreach (var item in q) //retrieve each item and assign to model
            {
                model.Add(new DimensionValueListModel()
                {
                    DimensionValue = item.DimensionValue,
                    DimensionSettingID = item.DimensionSettingID,
                    DimensionStatus = item.DimensionStatus

                });
            }
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadDimensionValue(List<DimensionValueListModel> dimensionvaluelistmodel)
        {
            if (dimensionvaluelistmodel != null)
            {
                foreach (var item in dimensionvaluelistmodel)
                {
                    long lDimSetupID = 0;
                    Dimension_Value dimension_value = (from p in dimension_valueRepository.Dimension_Value
                                                       where p.Dimension_SettingID.Equals(item.DimensionSettingID) && p.Status == "Active"
                                                       select p).FirstOrDefault();
                    if (dimension_value != null)
                    {
                        dimension_value.Dimension_SettingID = item.DimensionSettingID;
                        dimension_value.Status = "Active";
                        dimension_valueRepository.SaveDimension_Value(dimension_value);
                        lDimSetupID = dimension_value.ID;
                    }
                    else
                    {
                        Dimension_Value dimension_value2 = new Dimension_Value();
                        dimension_value2.Dimension_SettingID = item.DimensionSettingID;
                        dimension_value2.Status = "Active";
                        dimension_valueRepository.SaveDimension_Value(dimension_value2);
                        lDimSetupID = dimension_value2.ID;
                    }
                }
            }
                    TempData["Message"] = string.Format("{0} was updated in system !", Session["DimCode"]);
                    TempData["DeleteMessage"] = "";
                    return RedirectToAction("Edit2", new { id = Session["DimID"] });
             
        }

        // GET: Dimension_Setting/Create
        public PartialViewResult LoadDimensionTableRelationship(Int64? DimId, string DimCode)
        {
            Session["DimID"] = DimId;
            Session["DimCode"] = DimCode.Trim();
            var model = from p in dimension_tablerelationshipRepository.Dimension_TableRelationship
                        where p.Dimension_SettingID.Equals(DimId)
                        orderby p.Dimension_SettingID
                        select p;
            return PartialView(model.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadDimensionTableRelationship(List<Dimension_TableRelationship> dimension_tablerelationship)
        {
            if (dimension_tablerelationship != null)
            {
                foreach (var item in dimension_tablerelationship)
                {
                    Dimension_TableRelationship t = (from p in dimension_tablerelationshipRepository.Dimension_TableRelationship
                                                      where p.ID.Equals(item.ID)
                                        select p).FirstOrDefault();
                    //t.Dimension_SettingID = item.Dimension_SettingID;
                    t.Activate = item.Activate;
                    dimension_tablerelationshipRepository.SaveDimension_TableRelationship(t);
                }
            }
            TempData["Message"] = string.Format("{0} was updated", Session["DimCode"]);
            TempData["DeleteMessage"] = "";
            return RedirectToAction("Edit2", new { id = Session["DimID"] });
        }


        // GET: Dimension_Setting/Create
        public ActionResult Create()
        {
            //return View();
            TempData["Message"] = "";
            TempData["DeteleMessage"] = "";
            Session["DimID"] = null;
            return View(new DimensionSettingModel());
        }

        // POST: Dimension_Setting/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DimensionCode,Description,DimensionType,Status,DimensionUsage,PredefinedValue")] Dimension_Setting dimension_Setting)
        {
            if (ModelState.IsValid)
            {
                //db.Dimension_Setting.Add(dimension_Setting);
                //db.SaveChanges();
                dimension_Setting.Status = "Active";
                dimension_settingRepository.SaveDimension_Setting(dimension_Setting);
                TempData["Message"] = string.Format("{0} was added in system.", dimension_Setting.DimensionCode);
                return RedirectToAction("Index");
            }

            return View(dimension_Setting);
        }

        public ActionResult CreateDimensionValue(int? DimId, string DimCode)
        {
            Session["DimID"] = DimId;
            Session["DimCode"] = DimCode;
            return PartialView(new Dimension_Value());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDimensionValue([Bind(Include = "ID,Dimension_SettingID,DimensionValue,Status")] Dimension_Value dimension_value)
        {
            if (ModelState.IsValid)
            {
                dimension_value.Dimension_SettingID = Int32.Parse(Session["DimID"].ToString());
                dimension_value.Status = "Active";
                dimension_valueRepository.SaveDimension_Value(dimension_value);
                return RedirectToAction("Edit2", new { id = dimension_value.Dimension_SettingID });
            }
            return View(dimension_value);
        }

        // GET: Dimension_Setting/Edit/5
        public ActionResult Edit(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dimensionsettingmodel = new DimensionSettingModel();

            dimensionsettingmodel.Dimension_Setting = (from p in dimension_settingRepository.Dimension_Setting
                                                       where p.ID.Equals(id)
                                                           orderby p.DimensionCode
                                                           select p).FirstOrDefault();
            if (id != null)
            {
                dimensionsettingmodel.Dimension_Value = (from p in dimension_valueRepository.Dimension_Value
                                                         where p.Dimension_SettingID.Equals(id)
                                                         select p);
                //dimensionsettingmodel.Dimension_Value = from m in dimensionsettingmodel.Dimension_Setting.Dimension_Value
                //                                        orderby m.Dimension_SettingID
                //                          select m;

            }

            if (dimensionsettingmodel == null)
            {
                return HttpNotFound();
            }
            return PartialView(dimensionsettingmodel);

        }

        // POST: Dimension_Setting/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DimensionCode,Description,DimensionType,Status,DimensionUsage,PredefinedValue")] Dimension_Setting dimension_Setting)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(dimension_Setting).State = EntityState.Modified;
                //db.SaveChanges();
                dimension_Setting.Status = "Active";
                dimension_settingRepository.SaveDimension_Setting(dimension_Setting);
                ViewBag.Message = string.Format("{0} was updated in system. ", dimension_Setting.DimensionCode);
                return RedirectToAction("Index");
            }
            return View(dimension_Setting);
        }

        // GET: Dimension Setting/Edit/5
        public ActionResult Edit2(int? id)
        {
            Session["DimID"] = String.Empty;
            TempData["Message"] = "";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dimensionsettingmodel = new DimensionSettingModel();

            //To improve performance
            dimensionsettingmodel.Dimension_Setting = dimension_settingRepository.GetDimension_SettingByID((long)id).FirstOrDefault();

            if (id != null)
            {
                //improve performance
                dimensionsettingmodel.Dimension_Value= dimensionsettingmodel.Dimension_Setting.Dimension_Value;
            }

            if (dimensionsettingmodel == null)
            {
                return HttpNotFound();
            }
            return View("Edit2", dimensionsettingmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2([Bind(Include = "ID,DimensionCode,Description,DimensionType,Status,DimensionUsage,PredefinedValue")] Dimension_Setting dimension_Setting)
        {
            if (ModelState.IsValid)
            {
                dimension_Setting.Status = "Active";
                dimension_settingRepository.SaveDimension_Setting(dimension_Setting);
                ViewBag.Message = string.Format("{0} was updated in system !", dimension_Setting.DimensionCode);
                return RedirectToAction("Index");
            }
            return View(dimension_Setting);
        }


        // GET: Dimension_Setting/Delete/5
        public ActionResult Delete(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from p in dimension_settingRepository.Dimension_Setting
                         where p.ID.Equals(id)
                         orderby p.ID
                         select p).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Dimension_Setting/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            //Dimension_Setting dimension_Setting = db.Dimension_Setting.Find(id);
            //db.Dimension_Setting.Remove(dimension_Setting);
            //db.SaveChanges();
            Dimension_Setting dimension_Setting = dimension_settingRepository.Dimension_Setting.FirstOrDefault(p => p.ID == id);
            if (dimension_Setting != null)
            {
                dimension_settingRepository.DeleteDimension_Setting(dimension_Setting);
                TempData["DeleteMessage"] = string.Format("{0} was deleted", dimension_Setting.DimensionCode);
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
