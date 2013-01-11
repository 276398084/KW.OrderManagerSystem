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
    public class HomeController : Controller
    {
        protected ISession Session = NHibernateHelper.CreateSession();

        //lim
        // GET: /User/

        public ViewResult Index()
        {
        
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
