using System;
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
    public class MachinController : BaseController
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
        public JsonResult Create(MachinType obj)
        {
            try
            {
                if (obj.StartDate < Convert.ToDateTime("2000-01-01"))
                {
                    obj.StartDate = Convert.ToDateTime("2000-01-01");
                }
                if (obj.EndDate < Convert.ToDateTime("2000-01-01"))
                {
                    obj.EndDate = Convert.ToDateTime("2000-01-01");
                }
                if (obj.StartDateOld < Convert.ToDateTime("2000-01-01"))
                {
                    obj.StartDateOld = Convert.ToDateTime("2000-01-01");
                }
                if (obj.EndDateOld < Convert.ToDateTime("2000-01-01"))
                {
                    obj.EndDateOld = Convert.ToDateTime("2000-01-01");
                }
                NSession.SaveOrUpdate(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public  MachinType GetById(int Id)
        {
            MachinType obj = NSession.Get<MachinType>(Id);
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
            MachinType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(MachinType obj)
        {
           
            try
            {
                NSession.Update(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
           
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
          
            try
            {
                MachinType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }

		public JsonResult List(int page, int rows, string sort, string order, string search)
        {
            string where = "";
            string orderby = " order by Id desc ";
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
            {
                orderby = " order by " + sort + " " + order;
            }

            if (!string.IsNullOrEmpty(search))
            {
                where = Utilities.Resolve(search);
                if (where.Length > 0)
                {
                    where = " where " + where;
                }
            }
            IList<MachinType> objList = NSession.CreateQuery("from MachinType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<MachinType>();
            object count = NSession.CreateQuery("select count(Id) from MachinType " + where ).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

    }
}

