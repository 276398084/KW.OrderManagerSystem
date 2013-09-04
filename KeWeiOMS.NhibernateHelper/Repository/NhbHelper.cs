using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;
using System.Reflection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;

namespace KeWeiOMS.NhibernateHelper
{
    public sealed class NhbHelper
    {
        public static readonly ISessionFactory SessionFactory;
        static NhbHelper()
        {
            var configuration = CreateConfiguration();

            SessionFactory = configuration.BuildSessionFactory();
        }

        public static bool IsBuildScheme { get; set; }
        /// <summary>
        /// 创建NHibernate配置
        /// </summary>
        private static Configuration CreateConfiguration()
        {
            //HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();//NHProf工具
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;//配置文件中取
            Configuration cfg = FluentNHibernate.Cfg.Fluently.Configure()

                           .Database(FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2008
                               .Dialect<MsSql2008Dialect>()
                               .Driver<NHibernate.Driver.SqlClientDriver>().ShowSql()
                               .QuerySubstitutions("true 1, false 0, yes 'Y', no 'N'")
                               .UseOuterJoin()
                               .ConnectionString(connectionString))
                           .Mappings(m => m.FluentMappings
                               .AddFromAssembly(Assembly.Load("KeWeiOMS.Domain"))
                               .Conventions.Add<EnumConvention>()
                               .Conventions.Add<HasManyConvention>())
                               .ExposeConfiguration(build_schema)
                           .BuildConfiguration();

            //Session绑定配置
            System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();//配置文件中取
            var strAppType = appReader.GetValue("AppType", typeof(String));
            switch (strAppType.ToString())
            {
                case "Web":
                    cfg.CurrentSessionContext<NHibernate.Context.ManagedWebSessionContext>();
                    break;
                case "Wcf":
                    cfg.CurrentSessionContext<NHibernate.Context.WcfOperationSessionContext>();
                    break;
                case "Win":
                    cfg.CurrentSessionContext<NHibernate.Context.ThreadStaticSessionContext>();
                    break;
            }
            return cfg;
        }

        public static void build_schema(Configuration configuration)
        {
            if (true)
            {
                new SchemaUpdate(configuration)
                    .Execute(true, true);
            }
        }

        /// <summary>
        /// Session被绑定在架构中一定生命周期的时候使用
        /// </summary>
        /// <returns></returns>
        public static ISession GetCurrentSession()
        {
            return SessionFactory.GetCurrentSession();
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

    }
}
