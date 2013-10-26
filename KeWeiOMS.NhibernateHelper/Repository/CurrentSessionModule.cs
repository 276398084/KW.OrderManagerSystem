using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NHibernate.Context;
using NHibernate;

namespace KeWeiOMS.NhibernateHelper
{
    /// <summary>
    /// <remarks>在一次web请求开始时打开NhbSession，结束时关闭NhbSession</remarks>
    /// </summary>
    public class CurrentSessionModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(Application_BeginRequest);
            context.EndRequest += new EventHandler(Application_EndRequest);
        }

        public void Dispose()
        {
        }

        private void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            if (app.Request.CurrentExecutionFilePathExtension == ".js"
                || app.Request.CurrentExecutionFilePathExtension == ".png"
                || app.Request.CurrentExecutionFilePathExtension == ".jpg"
                || app.Request.CurrentExecutionFilePathExtension == ".gif"
                || app.Request.CurrentExecutionFilePathExtension == ".css")
                return;

           ManagedWebSessionContext.Bind(HttpContext.Current, NhbHelper.SessionFactory.OpenSession());
        }

        private void Application_EndRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            if (app.Request.CurrentExecutionFilePathExtension == ".js"
                || app.Request.CurrentExecutionFilePathExtension == ".png"
                || app.Request.CurrentExecutionFilePathExtension == ".jpg"
                || app.Request.CurrentExecutionFilePathExtension == ".gif"
                || app.Request.CurrentExecutionFilePathExtension == ".css")
                return;
            ISession session = ManagedWebSessionContext.Unbind(HttpContext.Current, NhbHelper.SessionFactory);

            if (session.Transaction.IsActive)
            {
                session.Transaction.Rollback();
            }

            if (session != null)
            {
                session.Close();
            }
        }
    }
}
