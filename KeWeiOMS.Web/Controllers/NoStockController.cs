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
    public class NoStockController : BaseController
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
        public JsonResult Create(NoStockType obj)
        {
            try
            {
                obj.Enabled = 1;
                obj.CreateBy = CurrentUser.Realname;
                obj.CreateOn = DateTime.Now;
                NSession.SaveOrUpdate(obj);
                NSession.Flush();
                List<NoStockLinkType> list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NoStockLinkType>>(obj.rows);
                foreach (NoStockLinkType link in list1)
                {
                    link.OldSKU = obj.OldSKU;
                    link.SKU = obj.SKU;
                    link.PId = obj.Id;
                    link.CreateBy = CurrentUser.Realname;
                    link.CreateOn = DateTime.Now;
                    NSession.Save(link);
                    NSession.Flush();
                }
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
        public  NoStockType GetById(int Id)
        {
            NoStockType obj = NSession.Get<NoStockType>(Id);
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
            NoStockType obj = GetById(id);
            ViewData["id"] = id;
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(NoStockType obj)
        {
           
            try
            {
                NSession.Update(obj);
                NSession.Flush();
                List<NoStockLinkType> list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NoStockLinkType>>(obj.rowse);
                NSession.Delete("from NoStockLinkType where PId='" + obj.Id + "'");
                NSession.Flush();
                NSession.Clear();
                foreach (NoStockLinkType link in list1)
                {
                    link.OldSKU = obj.OldSKU;
                    link.SKU = obj.SKU;
                    link.PId = obj.Id;
                    link.CreateBy = CurrentUser.Realname;
                    link.CreateOn = DateTime.Now;
                    NSession.Save(link);
                    NSession.Flush();
                }
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
                NoStockType obj = GetById(id);
                obj.Enabled = 0;
                NSession.Update(obj);
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
            IList<NoStockType> objList = NSession.CreateQuery("from NoStockType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<NoStockType>();

            object count = NSession.CreateQuery("select count(Id) from NoStockType " + where ).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public JsonResult Linklist(int id) 
        {
            IList<NoStockLinkType> objList = NSession.CreateQuery("from NoStockLinkType where PId='"+id+"'").List<NoStockLinkType>();
            return Json(objList,JsonRequestBehavior.AllowGet);
        }
        public ViewResult ProductLink(int id)
        {
            IList<NoStockLinkType> objList = NSession.CreateQuery("from NoStockLinkType where PId='" + id + "' order by QPrice ").List<NoStockLinkType>();
            foreach(var item in objList )
            {
                string days = "";
                IList<PurchasePlanType> planList = NSession.CreateQuery("from PurchasePlanType where SKU='" +item.SKU+ "' and Suppliers='"+item.Supplier+"'").List<PurchasePlanType>();
                if (planList.Count != 0)
                { 
                    foreach(var plan in planList)
                    {
                        TimeSpan time = plan.ReceiveOn - plan.CreateOn;
                        days= time.Days.ToString();
                        item.Received = days;
                    }
                }
            }
            return View(objList);
        }
        public ViewResult Received(int id)
        {
            NoStockType obj = GetById(id); 
            ProductType product= new ProductType();
            product.OldSKU = obj.OldSKU;
            product.SKU = obj.SKU;
            product.ProductName = obj.Name;
            product.Standard = obj.Standard;
            ViewData["nid"] = id;
            return View(product);
        }

    }
}

