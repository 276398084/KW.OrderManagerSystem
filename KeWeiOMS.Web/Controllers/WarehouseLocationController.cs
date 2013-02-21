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
    public class WarehouseLocationController : BaseController
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
        public JsonResult Create(WarehouseLocationType obj)
        {
            try
            {
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
        public WarehouseLocationType GetById(int Id)
        {
            WarehouseLocationType obj = NSession.Get<WarehouseLocationType>(Id);
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
            WarehouseLocationType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(WarehouseLocationType obj)
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
                WarehouseLocationType obj = GetById(id);
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
            string orderby = "";
            string where = "";
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
            IList<WarehouseLocationType> objList = NSession.CreateQuery("from WarehouseLocationType" + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<WarehouseLocationType>();

            object count = NSession.CreateQuery("select count(Id) from WarehouseLocationType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public JsonResult QList(string Q)
        {
            IList<WarehouseLocationType> objList = NSession.CreateQuery("from WarehouseLocationType where PositionsName like '" + Q + "%'")
                .SetMaxResults(10)
                .List<WarehouseLocationType>();
            return Json(new { total = objList.Count, rows = objList });
        }

    }
}

