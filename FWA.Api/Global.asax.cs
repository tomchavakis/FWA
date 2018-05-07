using Autofac;
using Autofac.Integration.WebApi;
using FWA.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FWA.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();

            MoviesInitializer initializer = new MoviesInitializer();
            Database.SetInitializer<MoviesContext>(initializer);
            initializer.DatabaseInitialization();

            AutoMapperConfig.Initialize();
            AreaRegistration.RegisterAllAreas();

            AutoFacConfig.Initialize(GlobalConfiguration.Configuration);

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
           

        }


    }
}
