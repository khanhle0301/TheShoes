using MyShop.Web.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MyShop.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutoMapperConfiguration.Configure();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    Exception ex = Server.GetLastError();
        //    HttpException httpex = ex as HttpException;
        //    RouteData data = new RouteData();
        //    data.Values.Add("controller", "Error");
        //    if (httpex == null)
        //    {
        //        data.Values.Add("action", "index");
        //    }
        //    else
        //    {
        //        switch (httpex.GetHttpCode())
        //        {
        //            case 403:
        //                data.Values.Add("action", "HttpError403");
        //                break;
        //            case 404:
        //                data.Values.Add("action", "HttpError404");
        //                break;
        //            case 500:
        //                data.Values.Add("action", "HttpError500");
        //                break;
        //        }
        //    }
        //    Server.ClearError();
        //    Response.TrySkipIisCustomErrors = true;
        //    IController error = new Controllers.ErrorController();
        //    error.Execute(new RequestContext(new HttpContextWrapper(Context), data));
        //}
    }
}
