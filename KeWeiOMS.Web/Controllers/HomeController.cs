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

namespace KeWeiOMS.Web.Controllers
{
    public class HomeController : Controller
    {
       

        public ActionResult Index()
        {


            var session = NHibernateHelper.CreateSession();
             
                 var ss = new ModulesType { Target = "1", CreateOn = DateTime.Now, ModifiedOn = DateTime.Now };
                 session.Save(ss);
             
            return View();
        }

    }
}
