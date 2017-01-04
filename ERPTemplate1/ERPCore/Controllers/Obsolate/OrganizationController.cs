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
    public class OrganizationController : BaseController
    {
        //private EFDbContext db = new EFDbContext();
        private IOrganizationRepository organizationRepository;
        private IBranchRepository branchRepository;
        private ICountryRepository countryRepository;
        private ICountryStateRepository countrystateRepository;
        private ICurrencyRepository currencyRepository;

        public OrganizationController(IOrganizationRepository organizationRepo, IBranchRepository branchRepo, ICountryRepository countryRepo, ICountryStateRepository countrystateRepo, ICurrencyRepository currencyRepo, IErrorLogRepository errorLogRepo) : base(errorLogRepo)
        {
            this.organizationRepository = organizationRepo;
            this.branchRepository = branchRepo;
            this.countryRepository = countryRepo;
            this.countrystateRepository = countrystateRepo;
            this.currencyRepository = currencyRepo;
        }

        // GET: Organization
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

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            if (page == null)
            {
                page = 1;
            }

            var model = (from m in organizationRepository.Organization
                         select m);

            Int64 iRecCnt = organizationRepository.GetMaxID();

            if (!String.IsNullOrEmpty(searchValue))
            {
                model = (from m in organizationRepository.OrganizationWildSearch("OrganizationCode", searchValue)
                         select m);
                iRecCnt = model.Count();
            }
            else
            {
                //model = model.Skip(((int)page - 1) * pageSize)
                //       .Take(pageSize);
                //improve performance
                model = organizationRepository.GetOrganizationPaging(((((long)page - 1) * pageSize) + 1), (long)page * pageSize);
            }

            switch (sortOrder)
            {
                default:
                    //model = model.OrderBy(s => s.ID);
                    break;
            }

            //return View(model.ToPagedList(pageNumber, pageSize));
            //improve performance
            var model3 = new StaticPagedList<Organization>(model, (int)page, pageSize, (int)iRecCnt);
            return View(model3);
            //return View(db.Invoice.ToList());
        }

        // GET: Organization/Details/5
        public ActionResult Details(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from p in organizationRepository.Organization
                         where p.ID.Equals(id)
                         orderby p.OrganizationCode
                         select p).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView(model);
        }

        private void PopulateCountryDropDownList(object selectedAcc = null)
        {
            var countryQuery = from d in countryRepository.Country
                                orderby d.CountryCode
                               select d;
            ViewBag.CountryCode = new SelectList(countryQuery, "ID", "CountryCode", selectedAcc);
        }

        private void PopulateCountryStateDropDownList(object selectedAcc = null)
        {
            var countrystateQuery = from d in countrystateRepository.CountryState
                               orderby d.StateName
                               select d;
            ViewBag.StateName = new SelectList(countrystateQuery, "ID", "StateName", selectedAcc);
        }

        private void PopulateCurrencyDropDownList(object selectedAcc = null)
        {
            var currencyQuery = from d in currencyRepository.Currency
                                orderby d.CurrencyCode
                                select d;
            ViewBag.CurrencyCode = new SelectList(currencyQuery, "ID", "CurrencyCode", selectedAcc);
        }

        // GET: Organization/Create
        public ActionResult Create()
        {
            TempData["Message"] = "";
            TempData["DeteleMessage"] = "";
            PopulateCountryDropDownList();
            PopulateCountryStateDropDownList();
            PopulateCurrencyDropDownList();
            return View(new OrganizationBranchModel());
        }

        // POST: Organization/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,OrganizationCode,OrganizationName,Address,PostCode,State,City,CountryCode,TelNo,FaxNo,EPFRef,SocsoRef,Status,CreateDate,UpdateDate,UpdateID,LogoUrl,CurrencyCode,CompRegNo,GSTRegNo,GAFVersion,ParentOrganization")] Organization organization)
        {
            var organizationBranchModel = new OrganizationBranchModel();

            if (ModelState.IsValid)
            {
                //db.Organization.Add(organization);
                //db.SaveChanges();
                organization.CreateDate = DateTime.Now;
                organization.UpdateDate = DateTime.Now;
                int iCountry = Int32.Parse(Request.Form["Country"].ToString());
                Country country = countryRepository.Country.FirstOrDefault(p => p.ID == iCountry);
                organization.CountryCode = country.CountryCode.Trim();
                organization.UpdateID = Session["UserID"].ToString();
                organization.Status = "Active";
                var organizationRec = from m in organizationRepository.OrganizationWildSearch("OrganizationCode", organization.OrganizationCode)
                                 select m;
                int iRecCnt = organizationRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The Organization Code " + organization.OrganizationCode + " already existed in system ! ";
                }
                else
                {
                    organizationRepository.SaveOrganization(organization);
                    Session["OrganizationID"] = organization.ID;
                    Session["OrganizationCode"] = organization.OrganizationCode;
                    Session["CountryCode"] = organization.CountryCode;
                    Session["CountryState"] = organization.State;
                    Session["CurrencyCode"] = organization.CurrencyCode;

                    organizationBranchModel.Organization = (from p in organizationRepository.Organization
                                                where p.ID.Equals(organization.ID)
                                                orderby p.OrganizationCode
                                                            select p).FirstOrDefault();
                    if (organization.ID != 0)
                    {
                        organizationBranchModel.Branch = from p in branchRepository.Branch
                                                       where p.ID.Equals(organization.ID)
                                                       orderby p.OrganizationID
                                                       select p;
                    }
                    Session["BranchID"] = "";
                    TempData["Message"] = string.Format("{0} was added in system !", organization.OrganizationCode);
                }
                PopulateCountryDropDownList(organization.CountryCode);
                PopulateCountryStateDropDownList(organization.State);
                PopulateCurrencyDropDownList(organization.CurrencyCode);
                return RedirectToAction("Index");
            }

            return View(organization);
        }

        public ActionResult CreateOrganizationAndBranch()
        {
            PopulateCountryDropDownList();
            PopulateCountryStateDropDownList();
            PopulateCurrencyDropDownList();
            return PartialView(new OrganizationBranchModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrganizationAndBranch([Bind(Include = "ID,OrganizationID, BranchCode, BranchName, Status")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                branch.OrganizationID = Int32.Parse(Session["OrganizationID"].ToString());
                var branchRec = from m in branchRepository.BranchWildSearch("BranchCode", branch.BranchCode)
                                     select m;
                int iRecCnt = branchRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The Branch Code " + branch.BranchCode + " already existed in system ! ";
                }
                else
                {
                    branchRepository.SaveBranch(branch);
                    Session["BranchID"] = branch.ID;
                    Session["BranchCode"] = branch.BranchCode;
                    //return RedirectToAction("Index");

                    TempData["Message"] = string.Format("{0} was added in system.", branch.BranchCode);
                }
                PopulateCountryDropDownList(Session["CountryCode"].ToString());
                PopulateCountryStateDropDownList(Session["CountryState"].ToString());
                PopulateCurrencyDropDownList(Session["CurrencyCode"].ToString());
                return RedirectToAction("Edit2", new { id = branch.OrganizationID });
            }
            return View(branch);
        }

        public ActionResult CreateBranch(Int64? branchId, string branchCode)
        {
            Session["BranchID"] = branchId;
            Session["BranchCode"] = branchCode;
            PopulateCountryDropDownList();
            PopulateCountryStateDropDownList();
            PopulateCurrencyDropDownList();
            return PartialView(new OrganizationBranchModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBranch([Bind(Include = "ID,OrganizationID, BranchCode, BranchName, Status")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                branch.OrganizationID = Int32.Parse(Session["OrganizationID"].ToString());
                var branchRec = from m in branchRepository.BranchWildSearch("BranchCode", branch.BranchCode)
                                     select m;
                int iRecCnt = branchRec.Count();
                if (iRecCnt > 0)
                {
                    TempData["Message"] = "The Branch Code " + branch.BranchCode + " already existed in system ! ";
                }
                else
                {
                    branchRepository.SaveBranch(branch);
                    Session["BranchID"] = branch.ID;
                    Session["BranchCode"] = branch.BranchCode;

                    TempData["Message"] = string.Format("{0} was added in sytem. ", branch.BranchCode);
                }
                PopulateCountryDropDownList(Session["CountryCode"].ToString());
                PopulateCountryStateDropDownList(Session["CountryState"].ToString());
                PopulateCurrencyDropDownList(Session["CurrencyCode"].ToString());
                return RedirectToAction("Edit2", new { id = branch.OrganizationID });
            }
            return View(branch);
        }

        public ActionResult EditBranch(Int64? organizationId, Int64? branchId, string organizationCode, string branchCode)
        {
            Session["OrganizationCode"] = organizationCode;
            Session["OrganizationID"] = organizationId;
            Session["BranchId"] = branchId;
            Session["BranchCode"] = branchCode;

            //improve performance
            Branch branch = branchRepository.GetBranchByID((long)branchId).SingleOrDefault();
            return PartialView(branch);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBranch([Bind(Include = "ID, OrganizationID, BranchCode, BranchName, Status")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                branch.OrganizationID = Int32.Parse(Session["OrganizationID"].ToString());
                branchRepository.SaveBranch(branch);
                Session["OrganizationID"] = branch.OrganizationID;
                //return View("Edit");
                return RedirectToAction("Edit2", new { id = branch.OrganizationID });
            }
            return PartialView(branch);
        }

        // GET: Organization/Edit/5
        public ActionResult Edit(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var organizationbranchModel = new OrganizationBranchModel();
            organizationbranchModel.Organization = organizationRepository.GetOrganizationByID((long)id).FirstOrDefault();

            if (id != null)
            {
                //improve performance
                organizationbranchModel.Branch = branchRepository.GetBranch("OrganizationID", (long)id);
            }

            if (organizationbranchModel == null)
            {
                return HttpNotFound();
            }
            //PopulateCustomerDropDownList(organizationbranchModel.Organization.CustomerID);
            return PartialView(organizationbranchModel);
            //Organization organization = db.Organization.Find(id);
            //if (organization == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(organization);
        }

        // POST: Organization/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,OrganizationCode,OrganizationName,Address,PostCode,State,City,CountryCode,TelNo,FaxNo,EPFRef,SocsoRef,Status,CreateDate,UpdateDate,UpdateID,LogoUrl,CurrencyCode,CompRegNo,GSTRegNo,GAFVersion,ParentOrganization")] Organization organization)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(organization).State = EntityState.Modified;
                //db.SaveChanges();
                organization.UpdateDate = DateTime.Now;
                organization.CreateDate = DateTime.Now;
                organization.UpdateID = Session["UserID"].ToString();
                organization.Status = "Active";
                PopulateCountryDropDownList(Session["CountryCode"].ToString());
                PopulateCountryStateDropDownList(Session["CountryState"].ToString());
                PopulateCurrencyDropDownList(Session["CurrencyCode"].ToString());

                organizationRepository.SaveOrganization(organization);
                ViewBag.Message = string.Format("{0} was updated in system.", organization.OrganizationCode);

                return RedirectToAction("Index");
            }
            return View(organization);
        }

        public ActionResult Edit2(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TempData["Message"] = "";
            var organizationbranchModel = new OrganizationBranchModel();

            //remove the orderBy to improve performance
            organizationbranchModel.Organization = organizationRepository.GetOrganizationByID((long)id).FirstOrDefault();
            if (id != null)
            {
                //improve performance
                organizationbranchModel.Branch = branchRepository.GetBranch("OrganizationID", (long)id);
            }

            if (organizationbranchModel == null)
            {
                return HttpNotFound();
            }
            return View(organizationbranchModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2([Bind(Include = "ID,OrganizationCode,OrganizationName,Address,PostCode,State,City,CountryCode,TelNo,FaxNo,EPFRef,SocsoRef,Status,CreateDate,UpdateDate,UpdateID,LogoUrl,CurrencyCode,CompRegNo,GSTRegNo,GAFVersion,ParentOrganization")] Organization organization)
        {
            if (ModelState.IsValid)
            {
                organization.UpdateDate = DateTime.Now;
                organizationRepository.SaveOrganization(organization);
                ViewBag.Message = string.Format("{0} was updated", organization.OrganizationCode);
                return RedirectToAction("Index");
            }
            return View(organization);
        }

        // GET: Organization/Delete/5
        public ActionResult Delete(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var organizationBranchModel = new OrganizationBranchModel();

            organizationBranchModel.Organization = organizationRepository.GetOrganizationByID((long)id).FirstOrDefault();
            if (id != null)
            {
                //improve performance
                organizationBranchModel.Branch = branchRepository.GetBranch("OrganizationID", (long)id);
            }
            Organization organization = organizationRepository.GetOrganizationByID((long)id).SingleOrDefault();

            if (organizationBranchModel.Branch != null)
            {
                foreach (var item in organizationBranchModel.Branch)
                {
                    Branch branch = branchRepository.Branch.FirstOrDefault(p => p.ID == item.ID);
                    branchRepository.DeleteBranch(branch);
                }
            }

            if (organization != null)
            {
                organizationRepository.DeleteOrganization(organization);
                TempData["DeleteMessage"] = string.Format("{0} was deleted in system. ", organization.OrganizationCode);
            }

            return RedirectToAction("Index");
            //Organization organization = db.Organization.Find(id);
            //if (organization == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(organization);
        }

        // POST: Organization/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(long id)
        //{
        //    Organization organization = db.Organization.Find(id);
        //    db.Organization.Remove(organization);
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
