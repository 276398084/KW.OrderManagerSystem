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
    public class DisputeController : BaseController
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
        public JsonResult Create(DisputeType obj)
        {
            try
            {
                if (obj.SolveOn < Convert.ToDateTime("2000-01-01"))
                { 
                    obj.SolveOn=Convert.ToDateTime("2000-01-01");
                }
                obj.CreateOn = DateTime.Now;
                obj.CreateBy = CurrentUser.Realname;
                NSession.SaveOrUpdate(obj);
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
        public  DisputeType GetById(int Id)
        {
            DisputeType obj = NSession.Get<DisputeType>(Id);
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
            DisputeType obj = GetById(id);
            ViewData["OrderNo"] = obj.OrderNo;
            ViewData["SKU"] = obj.SKU;
            ViewData["LogisticsMode"] = obj.LogisticsMode;
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(DisputeType obj)
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

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
          
            try
            {
                DisputeType obj = GetById(id);
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
            IList<DisputeType> objList = NSession.CreateQuery("from DisputeType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<DisputeType>();

            object count = NSession.CreateQuery("select count(Id) from DisputeType " + where ).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public JsonResult SearchOrder(string id)
        {
            IList<OrderType> obj = NSession.CreateQuery("from OrderType where OrderNo=:OrderNo").SetString("OrderNo", id).List<OrderType>();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchOrderP(string id)
        {
            IList<OrderProductType> obj = NSession.CreateQuery("from OrderProductType where OrderNo=:OrderNo").SetString("OrderNo", id).List<OrderProductType>();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

    }
}

