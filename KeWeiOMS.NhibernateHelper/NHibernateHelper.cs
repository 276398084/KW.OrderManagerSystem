using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeWeiOMS.Domain;
using NHibernate.Cfg;
using NHibernate;
using FluentNHibernate.Cfg;
using System.IO;
using System.Reflection;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;

namespace KeWeiOMS.NhibernateHelper
{
    public class NHibernateHelper
    {
        private static ISessionFactory sessionFactory = null;
        public static ISessionFactory SessionFactory
        {
            get
            {
                if (sessionFactory == null)
                {
                    FluentConfiguration fluentConfiguration = Fluently.Configure().Database(
        MsSqlConfiguration.MsSql2008.ConnectionString(
        c => c.FromConnectionStringWithKey("db")));
                    string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                    string assemblyFile = Path.Combine(path, "bin/KeWeiOMS.Domain.dll");
                    fluentConfiguration.Mappings(delegate(MappingConfiguration m)
                    {
                        Assembly assembly = Assembly.LoadFrom(assemblyFile);
                        m.HbmMappings.AddFromAssembly(assembly);
                        m.FluentMappings.AddFromAssembly(assembly).Conventions.AddAssembly(assembly);
                    });
                    sessionFactory = fluentConfiguration.BuildSessionFactory();
                }

                // return fluentConfiguration.ExposeConfiguration(BuildSchema).BuildSessionFactory();
                return sessionFactory;

            }
        }

        public static ISession CreateSession()
        {

            ISession session = SessionFactory.OpenSession();
            session.Clear();
            return session;
        }

        public static void CreateDatabase()
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string assemblyFile = Path.Combine(path, "bin/KeWeiOMS.Domain.dll");

            Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("db")))
                .Mappings(m =>
                    m.FluentMappings.AddFromAssemblyOf<OrderType>())
                .ExposeConfiguration(CreateSchema)
                .BuildConfiguration();
        }

        private static void CreateSchema(Configuration cfg)
        {
            var schemaExport = new SchemaUpdate(cfg);

            schemaExport.Execute(true, true);

        }

    }
}
