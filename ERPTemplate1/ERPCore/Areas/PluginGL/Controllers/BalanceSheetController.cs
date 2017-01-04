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
    public class BalanceSheetController : BaseController
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }
    }
}