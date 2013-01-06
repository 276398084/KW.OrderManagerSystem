using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
using NHibernate;
using FluentNHibernate.Cfg;
using System.IO;
using System.Reflection;

namespace KeWeiOMS.NhibernateHelper
{
    public class NHibernateHelper
    {
        private static Configuration configuration = null;
        private static ISessionFactory sessionFactory = null;

        public static void CreateConfiguration()
        {
            configuration = new Configuration().Configure();
        }

        public static Configuration Configuration
        {
            get
            {
                if (configuration == null)
                {
                    CreateConfiguration();
                }
                return configuration;
            }
            set { configuration = value; }
        }

        public static ISessionFactory SessionFactory
        {
            get
            {
                if (sessionFactory == null)
                {
                    if (Configuration == null)
                    {
                        CreateConfiguration();
                    }
                    FluentConfiguration fluentConfiguration = Fluently.Configure(Configuration);
                    string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                    string assemblyFile = Path.Combine(path, "bin/NhibernateDemo.Data.dll");
                    fluentConfiguration.Mappings(delegate(MappingConfiguration m)
                    {
                        Assembly assembly = Assembly.LoadFrom(assemblyFile);
                        m.HbmMappings.AddFromAssembly(assembly);
                        m.FluentMappings.AddFromAssembly(assembly).Conventions.AddAssembly(assembly);
                    });
                    return fluentConfiguration.BuildSessionFactory();
                }
                else
                {
                    return sessionFactory;
                }
            }
        }

        public static ISession CreateSession()
        {
            return SessionFactory.OpenSession();
        }
        
    }
}
