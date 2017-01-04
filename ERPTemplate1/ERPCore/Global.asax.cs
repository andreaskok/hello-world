using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ERPCore.App_Start;
using ERPCore.Controllers;

namespace ERPCore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SiteInitialization.ApplicationStart();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            //var app = (MvcApplication)sender;
            //var context = app.Context;
            //var routeData = new RouteData();
            //routeData.Values["controller"] = "Error";
            
            //routeData.Values["action"] = "HttpError";
            try
            {
                Response.Clear();
            }
            catch(Exception ex)
            {
                //routeData.Values["exception"] = ex.Message;
                //IController controller = new ErrorController();
                //controller.Execute(new RequestContext(new HttpContextWrapper(context), routeData));

                System.Console.WriteLine("ex=" + ex.Message);
                //return;
            }
            

            HttpException httpException = exception as HttpException;

            if (httpException != null)
            {
                string action;

                switch (httpException.GetHttpCode())
                {
                    case 404:
                        // page not found
                        action = "HttpError404";
                        break;
                    case 500:
                        // server error
                        action = "HttpError500";
                        break;
                    default:
                        action = "HttpGeneralError";
                        break;
                }

                // clear error on server
                Server.ClearError();

                Response.Redirect(String.Format("~/Error/{0}/?exMessage={1}", action, exception.Message));
            }

            //if (exception != null)
            //{
            //    Server.ClearError();
            //    Response.Redirect(String.Format("~/Error/{0}/?exMessage={1}", "HttpError500", "Internal Server Error"));
            //}
        }
    }
}
