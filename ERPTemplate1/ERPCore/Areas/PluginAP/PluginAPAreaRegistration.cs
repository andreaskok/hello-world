using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPCore.Areas.PluginAP
{
    public class PluginAPAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PluginAP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PluginAP_default",
                "PluginAP/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "PluginAP.Controllers" }
            );
        }
    }
}