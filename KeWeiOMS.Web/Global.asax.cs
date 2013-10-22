using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using KeWeiOMS.NhibernateHelper;
using LumiSoft.Net.IMAP.Client;
using LumiSoft.Net.IMAP;
using KeWeiOMS.Domain;
using NHibernate;
using System.Collections;

namespace KeWeiOMS.Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );
        }
        protected void Application_Start()
        {
            // Run();
            // OnStart();
            System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();//配置文件中取
            var isji = appReader.GetValue("IsJi", typeof(bool));
            Config.IsJi = Convert.ToBoolean(isji);
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
            // 在应用程序启动时运行的代码
            System.Timers.Timer myTimer = new System.Timers.Timer();
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            myTimer.Interval = 20000;
            myTimer.Enabled = true;
        }

        private static void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            Utilities.updateCurreny();
        }

    }
}