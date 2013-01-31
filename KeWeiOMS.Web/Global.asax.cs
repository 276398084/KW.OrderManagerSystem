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
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // 参数默认值
            );
           

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // 默认情况下对 Entity Framework 使用 LocalDB
            NHibernateHelper.CreateDatabase();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            //每次执行时间间隔
            System.Timers.Timer myTimer = new System.Timers.Timer(60000);
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            myTimer.Interval =1800000;
            myTimer.Enabled = true;
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }

        private static void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            //邮箱的账号信息
            string username = "feidutest@163.com";
            string password = "feiduq";
            string mailServer = "imap.163.com";

            IMAP_Client IMAPService = new IMAP_Client();
            try
            {
                IMAPService.Connect(mailServer, 143);
                IMAPService.Login(username, password);
                IMAPService.SelectFolder("INBOX");
                var flolder = IMAPService.SelectedFolder;
                var seqSet = LumiSoft.Net.IMAP.IMAP_t_SeqSet.Parse("1:*");
                var imapfetch = new IMAP_t_Fetch_i[]{
                new IMAP_t_Fetch_i_Uid(),
                new IMAP_t_Fetch_i_InternalDate(),
                new IMAP_t_Fetch_i_Rfc822(),
                };
                EventHandler<LumiSoft.Net.EventArgs<IMAP_r_u>> eventhandler = new EventHandler<LumiSoft.Net.EventArgs<IMAP_r_u>>(fetchback);
                IMAPService.Fetch(false, seqSet, imapfetch, eventhandler);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                IMAPService.Disconnect();
            }

        }
        //读出邮件中的内容
        public static void fetchback(object sender, LumiSoft.Net.EventArgs<IMAP_r_u> eventargs)
        {
            EmailType mailtype = new EmailType();
            IMAP_r_u_Fetch x = eventargs.Value as IMAP_r_u_Fetch;
            if (check(x.UID.UID.ToString()).Count == 0)
            {
                var stream = x.Rfc822.Stream;
                stream.Position = 0;
                LumiSoft.Net.Mail.Mail_Message mime = LumiSoft.Net.Mail.Mail_Message.ParseFromStream(stream);
                mailtype.MessageId = x.UID.UID.ToString();
                mailtype.BuyerEmail = mime.From.ToString().Substring(mime.From.ToString().LastIndexOf("<") + 1).Replace(">", "");
                mailtype.Subject = mime.Subject.ToString();
                mailtype.Content = mime.BodyText;
                mailtype.GenerateOn = mime.Date;
                mailtype.CreateOn = DateTime.Now;
                mailtype.ReplyOn = DateTime.Now;
                creat(mailtype);
            }
        }
        //检查并写入数据库 
        public static void creat(EmailType mail)
        {
            ISession NSession = NHibernateHelper.CreateSession();
            if (check(mail.MessageId).Count == 0)
            {
                NSession.Save(mail);
            }
        }
        //检查该UID邮件是否已存在
        public static IList check(string euid)
        {
            ISession NSession = NHibernateHelper.CreateSession();
            return NSession.CreateQuery("from EmailType c where c.MessageId=:euid")
                .SetString("euid", euid)
                .List();
        }
    }
}