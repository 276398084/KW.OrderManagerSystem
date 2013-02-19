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
    public class PurchasePlanController : BaseController
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
        public JsonResult Create(PurchasePlanType obj)
        {
            try
            {
                obj.CreateOn = DateTime.Now;
                obj.CreateBy = CurrentUser.Realname;
                obj.BuyBy = CurrentUser.Realname;
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
        public  PurchasePlanType GetById(int Id)
        {
            PurchasePlanType obj = NSession.Get<PurchasePlanType>(Id);
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
            PurchasePlanType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(PurchasePlanType obj)
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
                PurchasePlanType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }

        public JsonResult List(int page, int rows,string sort,string order)
        {
            string orderby = "";
            if(!string.IsNullOrEmpty(sort)&&!string.IsNullOrEmpty(order))
            {
                orderby = " order by " + sort + " " + order;
            }
            IList<PurchasePlanType> objList = NSession.CreateQuery("from PurchasePlanType" + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<PurchasePlanType>();
			
            object count = NSession.CreateQuery("select count(Id) from PurchasePlanType ").UniqueResult();
            return Json(new { total = count, rows = objList });
        }
        public JsonResult SearchSKU(string id)
        {
            IList<ProductType> obj = NSession.CreateQuery("from ProductType where SKU=:sku").SetString("sku", id).List<ProductType>();
            return Json(obj,JsonRequestBehavior.AllowGet);
        }

    }
}

