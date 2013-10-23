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
    public class SendPackageOrderController : BaseController
    {
        public ViewResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View();
            else
            {
                ViewData["Id"] = id;
                return View("OrderList");
            }
        }

        public ActionResult Create()
        {
            return View();
        }


        public ActionResult OrderList(int p)
        {
            IList<SendPackageOrderType> list = NSession.CreateQuery(" from SendPackageOrderType where PackId=" + p).List<SendPackageOrderType>();
            return Json(new { total = list.Count, rows = list });
        }



        [HttpPost]
        public JsonResult Create(SendPackageOrderType obj)
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
        public SendPackageOrderType GetById(int Id)
        {
            SendPackageOrderType obj = NSession.Get<SendPackageOrderType>(Id);
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
            SendPackageOrderType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(SendPackageOrderType obj)
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
                SendPackageOrderType obj = GetById(id);
                SendPackageType pack = NSession.Get<SendPackageType>(obj.PackId);
                OrderType order = NSession.Get<OrderType>(obj.OId);
                pack.PCount = pack.PCount - 1;
                pack.PWeight = pack.PWeight - order.Weight;
                NSession.Delete(obj);
                NSession.Update(pack);
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
            IList<SendPackageOrderType> objList = NSession.CreateQuery("from SendPackageOrderType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<SendPackageOrderType>();

            object count = NSession.CreateQuery("select count(Id) from SendPackageOrderType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

    }
}

