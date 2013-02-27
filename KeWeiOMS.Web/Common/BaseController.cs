using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KeWeiOMS.Domain;
using System.Web.Mvc;
using System.Text;
using System.EnterpriseServices;
using System.Configuration;
using KeWeiOMS.NhibernateHelper;
using NHibernate;
using KeWeiOMS.Domain;
using System.Web.UI;

namespace KeWeiOMS.Web.Controllers
{
    [SupportFilter]//此处如果去掉注释，则全部继承BaseController的Controller，都将执行SupportFilter过滤
    public class BaseController : Controller
    {
        public ISession NSession = NHibernateHelper.CreateSession();

        private UserType currentUser;

        public UserType CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    UserType account = GetCurrentAccount();
                    return account;
                }
                return currentUser;
            }
        }

        /// <summary>
        /// 获取当前登陆人的账户信息
        /// </summary>
        /// <returns>账户信息</returns>
        public UserType GetCurrentAccount()
        {
            if (Session["account"] != null)
            {
                UserType account = (UserType)Session["account"];
                return account;
            }
            return null;
            //return new UserType { Id = 0, Realname = "邵锡栋" };
        }

        public BaseController()
        {

        }

        
    }
}
