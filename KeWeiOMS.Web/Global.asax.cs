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


        protected virtual void OnStart()
        {

            initSessionBuilder();
            RegisterRoutes(RouteTable.Routes);
        }

        private void initSessionBuilder()
        {
            SessionBuilder.sessionStorage = new HttpSessionStorage();//这里创建Session实例
        }

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
            NHibernateHelper.CreateDatabase();
            OnStart();
            //每次执行时间间隔
            System.Timers.Timer myTimer = new System.Timers.Timer(60000);
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            myTimer.Interval = 900000;
            myTimer.Enabled = true;
        }

        private static void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            EbayMessageUtil.syn();
        }

    }
}