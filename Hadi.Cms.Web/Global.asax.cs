using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Hadi.Cms.Model.Mappings.Configuration;
using Hadi.Cms.Web.App_Start;
using Hadi.Cms.Web.Utilities;
using System;
using System.Globalization;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Hadi.Cms.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 500;
        }

        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            MapperConfig.Config();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBindingConfig.RegisterModelBinders(ModelBinders.Binders);
            SerializitionSettings(GlobalConfiguration.Configuration);
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            CultureInfo culture = null;

            if (SessionData.Current != null)
                culture = SessionData.Current.Culture;
            else
                culture = Infrastructure.Utilities.Utility.GetCultureFromCookie();

            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
        }
        protected void Application_EndRequest()
        {
            var containerObject = HttpContext.Current.Items["_EntityFrameworkContainerObject"] as Repository.UnitOfWork.DataContext;

            if (containerObject != null)
                containerObject.Dispose();
        }

        protected void SerializitionSettings(HttpConfiguration config)
        {
            JsonSerializerSettings jsonSetting = new JsonSerializerSettings();
            jsonSetting.DateFormatString = "yyyy/MM/dd HH:mm:ss";
            jsonSetting.Converters.Add(new StringEnumConverter());
            jsonSetting.Converters.Add(new Hadi.Cms.Infrastructure.Utilities.DbGeographyConverter());
            //jsonSetting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //jsonSetting.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            config.Formatters.JsonFormatter.SerializerSettings = jsonSetting;

            // SignalR json settings
            JsonSerializer serializer = JsonSerializer.Create(jsonSetting);
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);
        }
    }
}
