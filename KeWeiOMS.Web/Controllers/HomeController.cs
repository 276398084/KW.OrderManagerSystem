using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeWeiOMS.NhibernateHelper;
using NHibernate;
using KeWeiOMS.Domain;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Web.UI;

namespace KeWeiOMS.Web.Controllers
{
    public class HomeController : BaseController
    {


        //lim
        // GET: /User/

        public ViewResult Index()
        {
            //var ss = new ModuleType { FullName = "系统管理", CreateOn = DateTime.Now, CreateBy = "系统管理员" };
            //NSession.Save(ss);
            //var dd = new ModuleType { ParentId = ss.Id, FullName = "菜单管理", NavigateUrl = "/Module/Index", CreateOn = DateTime.Now, CreateBy = "系统管理员" };
            //NSession.Save(dd);
            return View();
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        }

        //





    }


}
