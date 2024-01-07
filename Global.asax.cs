using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ExamOn
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static string plateformError;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var urlHeler = new UrlHelper(HttpContext.Current.Request.RequestContext);
            Exception exception = Server.GetLastError();
            plateformError = exception.Message;
            Response.Redirect(urlHeler.Action("Go", "SignOut", new { isError = exception is null ? "" : exception.Message  }));
        }
    }
}
