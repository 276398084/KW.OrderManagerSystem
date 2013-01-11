using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KeWeiOMS.Domain;
using System.Web.Mvc;
using System.Text;
using System.EnterpriseServices;
using System.Configuration;

namespace KeWeiOMS.Web.Controllers
{
    //[SupportFilter]//此处如果去掉注释，则全部继承BaseController的Controller，都将执行SupportFilter过滤
    public class BaseController : Controller
    {


        public T Set<T>(T t, int isM = 0)
        {
            Type type = t.GetType();
            PropertyInfo[] pi = type.GetProperties();

            foreach (PropertyInfo p in pi)
            {
                if (isM == 0)
                {
                    switch (p.Name)
                    {
                        case "CreateOn":
                        case "ModifiedOn":
                            p.SetValue(t, DateTime.Now, null);
                            break;
                        case "CreateBy":
                        case "ModifiedBy":
                            p.SetValue(t, GetCurrentAccount().Realname, null);
                            break;
                        case "CreateUserId":
                        case "ModifiedUserId":
                            p.SetValue(t, GetCurrentAccount().Id, null);
                            break;
                        default:
                            break;
                    }
                }
                if (isM == 1)
                {
                    switch (p.Name)
                    {
                        case "ModifiedOn":
                            p.SetValue(t, DateTime.Now, null);
                            break;
                        case "ModifiedBy":
                            p.SetValue(t, GetCurrentAccount().Realname, null);
                            break;
                        case "ModifiedUserId":
                            p.SetValue(t, GetCurrentAccount().Id, null);
                            break;
                        default:
                            break;
                    }
                }

            }
            return t;
        }

        /// <summary>
        /// 获取当前登陆人的名称
        /// </summary>
        /// <returns></returns>
        public string GetCurrentPerson()
        {
            //Account account = GetCurrentAccount();
            //if (account != null && !string.IsNullOrWhiteSpace(account.PersonName))
            //{
            //    return account.PersonName;
            //}
            return string.Empty;
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
            //return null;

            return new UserType { Id = 0, Realname = "邵锡栋" };
        }

        public BaseController()
        {

        }

    }
}
