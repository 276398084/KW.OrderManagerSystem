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
            ViewData["No"] = Utilities.GetPlanNo();
            return View();
        }

        [HttpPost]
        public JsonResult Create(PurchasePlanType obj)
        {
            try
            {
                if (obj.SendOn < Convert.ToDateTime("2000-01-01"))
                {
                    obj.SendOn = Convert.ToDateTime("2000-01-01");
                }
                if (obj.ReceiveOn < Convert.ToDateTime("2000-01-01"))
                {
                    obj.ReceiveOn = Convert.ToDateTime("2000-01-01");
                }
                obj.CreateOn = DateTime.Now;
                obj.BuyOn = DateTime.Now;
                obj.CreateBy = CurrentUser.Realname;
                obj.BuyBy = CurrentUser.Realname;
                NSession.SaveOrUpdate(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public PurchasePlanType GetById(int Id)
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
            ViewData["sku"] = obj.SKU;
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
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });

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
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
        }

        public JsonResult List(int page, int rows, string sort, string order, string search)
        {
            string orderby = " order by Id desc ";
            string where = "";
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
            {
                orderby = " order by " + sort + " " + order;
            }
            if (!string.IsNullOrEmpty(search))
            {
                string key = search.Substring(search.IndexOf("$") + 1);
                where = Utilities.Resolve(key);
                if (where.Length > 0)
                {
                    where = " where " + where;
                }
                string GetDate = search.Substring(0, search.IndexOf("$"));
                string SearchDate = GetSearch(GetDate);
                if (!string.IsNullOrEmpty(SearchDate))
                {
                    if (string.IsNullOrEmpty(where))
                    {
                        where = " where " + SearchDate;
                    }
                    else
                    {
                        where += " and " + SearchDate;
                    }
                }
            }
            IList<PurchasePlanType> objList = NSession.CreateQuery("from PurchasePlanType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<PurchasePlanType>();

            object count = NSession.CreateQuery("select count(Id) from PurchasePlanType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }
        public JsonResult SearchSKU(string id)
        {
            IList<PurchasePlanType> obj = NSession.CreateQuery("from PurchasePlanType where SKU=:sku order by Id desc").SetString("sku", id)
           .SetFirstResult(0)
                .SetMaxResults(1).List<PurchasePlanType>();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSuppliers()
        {
            IList<SupplierType> obj = NSession.CreateQuery("from SupplierType").List<SupplierType>();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public static string GetSearch(string search)
        {
            string where = "";
            string startdate = search.Substring(0, search.IndexOf("&"));
            string enddate = search.Substring(search.IndexOf("&") + 1);
            if (!string.IsNullOrEmpty(startdate) || !string.IsNullOrEmpty(enddate))
            {
                if (!string.IsNullOrEmpty(startdate))
                    where += "BuyOn >=\'" + Convert.ToDateTime(startdate) + "\'";
                if (!string.IsNullOrEmpty(enddate))
                {
                    if (where != "")
                        where += " and ";
                    where += "BuyOn <\'" + Convert.ToDateTime(enddate).AddDays(1) + "\'";
                }
            }
            return where;
        }

    }
}

