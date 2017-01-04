using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using ERPDomain.Abstract;

namespace ERPCore.Controllers
{
    public class ParentMenuController : BaseController
    {
        // GET: ParentMenu
        private IParentMenuRepository repository;
        private ISubMenuRepository subMenuRepository;
        public ParentMenuController(IParentMenuRepository parentMenuRepo, ISubMenuRepository subMenuRepo, IErrorLogRepository errorLogRepo) : base(errorLogRepo)
        {
            this.repository = parentMenuRepo;
            this.subMenuRepository = subMenuRepo;
        }

        public ViewResult List()
        {
            return View(repository.ParentMenu);
        }

        public PartialViewResult ListMenu()
        {
            try
            {
                Session["ParentMenusList"] = repository.ParentMenu.ToList();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Ex:" + ex.Message);
                //DisplayOtherException();
                //return;
            }
            
            return PartialView(repository.ParentMenu);
        }

        private ViewResult DisplayOtherException()
        {
            return View("~/Views/Error/WebConfigError.cshtml");
        }

        public PartialViewResult LoadSubMenuGL(int parentMenuID)
        {
            var model = (from p in subMenuRepository.SubMenu
                         where p.ParentMenuID.Equals(parentMenuID)
                         select p);
            Session["SubMenuListGL"] = model.ToList();
            return PartialView(model);
        }

        public PartialViewResult LoadSubMenuAR(int parentMenuID)
        {
            var model = (from p in subMenuRepository.SubMenu
                         where p.ParentMenuID.Equals(parentMenuID)
                         select p);
            Session["SubMenuListAR"] = model.ToList();
            return PartialView(model);
        }

        public PartialViewResult LoadSubMenuAP(int parentMenuID)
        {
            var model = (from p in subMenuRepository.SubMenu
                         where p.ParentMenuID.Equals(parentMenuID)
                         select p);
            Session["SubMenuListAP"] = model.ToList();
            return PartialView(model);
        }

        public PartialViewResult LoadSubMenuPU(int parentMenuID)
        {
            var model = (from p in subMenuRepository.SubMenu
                         where p.ParentMenuID.Equals(parentMenuID)
                         select p);
            Session["SubMenuListPU"] = model.ToList();
            return PartialView(model);
        }

        public PartialViewResult LoadSubMenuIN(int parentMenuID)
        {
            var model = (from p in subMenuRepository.SubMenu
                         where p.ParentMenuID.Equals(parentMenuID)
                         select p);
            Session["SubMenuListIN"] = model.ToList();
            return PartialView(model);
        }

        public PartialViewResult LoadSubMenuCT(int parentMenuID)
        {
            var model = (from p in subMenuRepository.SubMenu
                         where p.ParentMenuID.Equals(parentMenuID)
                         select p);
            Session["SubMenuListCT"] = model.ToList();
            return PartialView(model);
        }

        public PartialViewResult LoadSubMenuWS(int parentMenuID)
        {
            var model = (from p in subMenuRepository.SubMenu
                         where p.ParentMenuID.Equals(parentMenuID)
                         select p);
            Session["SubMenuListWS"] = model.ToList();
            return PartialView(model);
        }

        public PartialViewResult LoadSubMenuHR(int parentMenuID)
        {
            var model = (from p in subMenuRepository.SubMenu
                         where p.ParentMenuID.Equals(parentMenuID)
                         select p);
            Session["SubMenuListHR"] = model.ToList();
            return PartialView(model);
        }

        public PartialViewResult LoadSubMenuPR(int parentMenuID)
        {
            var model = (from p in subMenuRepository.SubMenu
                         where p.ParentMenuID.Equals(parentMenuID)
                         select p);
            Session["SubMenuListPR"] = model.ToList();
            return PartialView(model);
        }

        public PartialViewResult LoadSubMenuNU(int parentMenuID)
        {
            var model = (from p in subMenuRepository.SubMenu
                         where p.ParentMenuID.Equals(parentMenuID)
                         select p);
            Session["SubMenuListNU"] = model.ToList();
            return PartialView(model);
        }

        public PartialViewResult LoadSubMenuFA(int parentMenuID)
        {
            var model = (from p in subMenuRepository.SubMenu
                         where p.ParentMenuID.Equals(parentMenuID)
                         select p);
            Session["SubMenuListFA"] = model.ToList();
            return PartialView(model);
        }

        public PartialViewResult LoadSubMenuBD(int parentMenuID)
        {
            var model = (from p in subMenuRepository.SubMenu
                         where p.ParentMenuID.Equals(parentMenuID)
                         select p);
            Session["SubMenuListBD"] = model.ToList();
            return PartialView(model);
        }

        public PartialViewResult LoadSubMenuAG(int parentMenuID)
        {
            var model = (from p in subMenuRepository.SubMenu
                         where p.ParentMenuID.Equals(parentMenuID)
                         select p);
            Session["SubMenuListAG"] = model.ToList();
            return PartialView(model);
        }

        public PartialViewResult LoadSubMenuAD(int parentMenuID)
        {
            var model = (from p in subMenuRepository.SubMenu
                         where p.ParentMenuID.Equals(parentMenuID)
                         select p);
            Session["SubMenuListAD"] = model.ToList();
            return PartialView(model);
        }

        public PartialViewResult ListMenu2()
        {
            return PartialView();
        }

        public ViewResult ListMenuByName(string name = null)
        {
            ViewBag.SelectedName = name;
            IEnumerable<string> names = repository.ParentMenu
                                    .Select(x => x.Name)
                                    .Distinct()
                                    .OrderBy(x => x);
            return View(names);
        }
        //public ActionResult Index()
        //{
        //    return View();
        //}
    }
}