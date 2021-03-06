﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;

namespace KeWeiOMS.Web.Controllers
{
    public class ProjectController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(ProjectType obj)
        {
            try
            {
                if (obj.RealEndDate < Convert.ToDateTime("2000-01-01"))
                {
                    obj.RealEndDate = Convert.ToDateTime("2000-01-01");
                }
                obj.CreateOn = DateTime.Now;
                obj.CreateBy = CurrentUser.Realname;
                NSession.Save(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true  });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public  ProjectType GetById(int Id)
        {
            ProjectType obj = NSession.Get<ProjectType>(Id);
            if (obj == null)
            {
                throw new Exception("返回实体为空");
            }
            else
            {
                return obj;
            }
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(int id)
        {
            ProjectType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(ProjectType obj)
        {
           
            try
            {
                NSession.Update(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true  });
           
        }


        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Details(int id)
        {
            ProjectType obj = GetById(id);
            Session["Content"] = obj.Content;
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Details(ProjectType obj)
        {

            try
            {
                obj.Content = Session["Content"]+DateTime.Now.Date.ToShortDateString()+": "+obj.Content+"<br>";
                NSession.Update(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true  });

        }
        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
          
            try
            {
                ProjectType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true  });
        }

		public JsonResult List(int page, int rows, string sort, string order, string search)
        {
            string orderby = Utilities.OrdeerBy(sort, order);
            string where = Utilities.SqlWhere(search);
            IList<ProjectType> objList = NSession.CreateQuery("from ProjectType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<ProjectType>();

            object count = NSession.CreateQuery("select count(Id) from ProjectType " + where ).UniqueResult();
            return Json(new { total = count, rows = objList });
        }
        public void DelSession()
        {
            Session.Contents.Remove("Content");
        }
    }
}

