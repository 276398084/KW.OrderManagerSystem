using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using KeWeiOMS.Domain;
using NHibernate;

namespace KeWeiOMS.NhibernateHelper
{
    public class SessionBuilder
    {
        private static object locker = new object();
        private static FluentConfiguration configuration = null;
        private static ISessionFactory sessionFactory = null;
        public static ISessionStorage sessionStorage { set; get; }

        private static void CreateConfiguration()
        {
            // HibernatingRhinos.NHibernate.Profiler.Appender.NHibernateProfiler.Initialize();//查看HQL生成的SQL
            //configuration = new Configuration().Configure(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "Configuration\\hibernate.cfg.xml");
            configuration = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("db")))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<OrderType>());
        }

        public static FluentConfiguration Configuration
        {
            get
            {
                lock (locker)
                {
                    if (configuration == null)
                    {
                        CreateConfiguration();
                    }
                    return configuration;
                }
            }
            set { configuration = value; }
        }

        internal static ISessionFactory SessionFactory
        {
            get
            {
                if (sessionFactory == null)
                {
                    if (Configuration == null)
                    {
                        CreateConfiguration();
                    }
                    lock (locker)
                    {
                        sessionFactory = Configuration.BuildSessionFactory();
                    }
                }

                return sessionFactory;
            }
        }

        public static ISession CreateSession()
        {
            ISession s = sessionStorage.Get();
            if (s == null)
            {
                s = SessionFactory.OpenSession();
                sessionStorage.Set(s);
            }
            return s;
        }

    }
}
