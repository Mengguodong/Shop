using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SFO2O.M.Controllers;
using SFO2O.Utility.Uitl;
using SolrNet;
using SFO2O.BLL.Search;
using SFO2O.M.ViewModel.Search;

namespace SFO2O.M.Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
        void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            var code = (exception is HttpException) ? (exception as HttpException).GetHttpCode() : 500;

            if (code != 404)
            {
                LogHelper.Error("MvcApplication", exception);


                Response.Clear();
                Server.ClearError();

                string path = Request.Path;
                Context.Response.Redirect("/Home/Error", true);
            }
            else
            {
                LogHelper.Error("MvcApplication-404", exception);
                Response.Clear();
                Server.ClearError();

                string path = Request.Path;
                Context.Response.Redirect("/Home/NotFound", true);
            }

        }
    }
}