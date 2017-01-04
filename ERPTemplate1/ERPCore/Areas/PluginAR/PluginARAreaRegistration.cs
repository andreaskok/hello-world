using System.Web.Mvc;

namespace ERPCore.Areas.PluginAR
{
    public class PluginARAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PluginAR";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PluginAR_default",
                "PluginAR/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "PluginAR.Controllers" }
            );
        }
    }
}