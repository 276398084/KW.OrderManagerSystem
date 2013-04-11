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
    public class SqlStatementInterceptor : EmptyInterceptor
    {
        public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
        {
            System.Diagnostics.Trace.WriteLine(sql.ToString());
            return sql;
        }
    }

    public class SessionBuilder
    {
        private static object locker = new object();
        private static FluentConfiguration configuration = null;
        private static ISessionFactory sessionFactory = null;
        public static ISessionStorage sessionStorage { set; get; }
        private static void CreateConfiguration()
        {

            configuration = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("db")))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<OrderType>()).ExposeConfiguration(f => f.SetInterceptor(new SqlStatementInterceptor()));
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
            return SessionFactory.OpenSession();
        }

    }
}
