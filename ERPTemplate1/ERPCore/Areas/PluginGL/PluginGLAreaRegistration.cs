using System.Web.Mvc;

namespace ERPCore.Areas.PluginGL
{
    public class PluginGLAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PluginGL";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //context.MapRoute(
            //    "PluginGL_default",
            //    "PluginGL/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);
            context.MapRoute(
               "PluginGL_default",
               "PluginGL/{controller}/{action}/{id}",
               new { controller = "PluginGL", action = "Index", id = UrlParameter.Optional },
               new string[] { "PluginGL.Controllers" });
        }
    }
}