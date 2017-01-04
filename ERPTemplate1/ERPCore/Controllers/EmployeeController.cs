using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ERPDomain.Abstract;
using ERPDomain.Entities;

namespace ERPCore.Controllers
{
    public class EmployeeController : Controller
    {
        private IEmployeeRepository employeeRepository;
        public EmployeeController(IEmployeeRepository appRepo) 
        {
            this.employeeRepository = appRepo;
        }

        public ActionResult Index()
        {
           var model = from m in employeeRepository.Employee
                        select m;
            return View(model);
        }

        // GET: SH_APP/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //SH_APP sH_APP = db.SH_APP.Find(id);
            //if (sH_APP == null)
            //{
            //    return HttpNotFound();
            //}
            var model = (from p in employeeRepository.Employee
                         where p.ID.Equals(id)
                         select p).FirstOrDefault();
            return View(model);
        }

        // GET: SH_APP/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SH_APP/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                Employee employee2 = employeeRepository.Employee.FirstOrDefault(p => p.EmployeeCode.Equals(employee.EmployeeCode));
                if (employee2 != null)
                {
                    TempData["Message"] = string.Format("{0} already added before !", employee.EmployeeName);
                }
                else
                {
                    employeeRepository.SaveEmployee(employee);
                    TempData["Message"] = string.Format("{0} was added", employee.EmployeeName);
                }
                
                
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from p in employeeRepository.Employee
                         where p.ID.Equals(id)
                         select p).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employeeRepository.SaveEmployee(employee);
                ViewBag.Message = string.Format("{0} was updated", employee.EmployeeName);
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        public ActionResult Delete(int id)
        {
            Employee employee = employeeRepository.Employee.FirstOrDefault(p => p.ID == id);
            if (employee != null)
            {
                employeeRepository.DeleteEmployee(employee);
                TempData["DeleteMessage"] = string.Format("{0} was deleted", employee.EmployeeName);
            }
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        //db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}