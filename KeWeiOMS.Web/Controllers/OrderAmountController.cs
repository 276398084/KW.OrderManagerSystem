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
    public class OrderAmountController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetOrders(string Id)
        {
            ViewData["Id"] = Id;
            return View();
        }
        [HttpGet]
        public ActionResult GetProducts(string Id)
        {
            ViewData["Id"] = Id;
            return View();
        }
        /// <summary>
        /// 订单表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetOrderList(string Id)
        {
            IList<OrderAmountType> amountlist = NSession.CreateQuery("from OrderAmountType where OId=" + Id + " or  OId in(select Id  from OrderType where MId=" + Id + ")").List<OrderAmountType>();
            List<OrderType> orderList = NSession.CreateQuery("from OrderType where  MId=" + Id + " or Id=" + Id).List<OrderType>().ToList();
            List<OrderData> os = new List<OrderData>();
            foreach (var orderAmountType in amountlist)
            {
                OrderType order = orderList.Find(x => x.Id == orderAmountType.OId);
                OrderData o = new OrderData(); ;
                o.TotalCost = orderAmountType.TotalCosts;

                if (order != null)
                {
                    o.OrderNo = order.OrderNo;
                    o.OrderExNo = order.OrderExNo;
                    o.RMB = order.RMB;
                    o.CurrencyCode = order.CurrencyCode;
                    o.LogisticMode = order.LogisticMode;
                    o.OrderAmount = order.Amount;
                    o.Status = order.Status;
                    if (order.IsRepeat == 1)
                        o.OrderType = "重发";
                    if (order.IsSplit == 1)
                        o.OrderType += "拆包";
                    o.Country = order.Country;
                    o.SendOn = order.ScanningOn;
                    o.Freight = order.Freight;

                }
                os.Add(o);
            }


            return Json(new { total = os.Count, rows = os });

        }

        /// <summary>
        /// 订单产品
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProductList(string Id)
        {
            IList<OrderProductType> list = NSession.CreateQuery("from OrderProductType where OId=" + Id).List<OrderProductType>();
            string ids = "";
            foreach (var orderProductType in list)
            {
                ids += orderProductType.SKU + ",";
            }
            if (ids.Length > 0)
            {
                ids = ids.Trim(',');
            }
            List<ProductType> products =
                NSession.CreateQuery("from ProductType where SKU in ('" + ids.Replace(",", "','") + "')").List<ProductType>().ToList();
            List<ProductData> ps = new List<ProductData>();
            double total = 0;
            foreach (var orderProductType in list)
            {
                ProductData p = new ProductData();
                p.SKU = orderProductType.SKU;
                p.Qty = orderProductType.Qty;
                ProductType product = products.Find(x => x.SKU.Trim().ToUpper() == orderProductType.SKU.Trim().ToUpper());
                if (product != null)
                {
                    p.Standard = product.Standard;
                    p.Status = product.Status;
                    p.Title = product.ProductName;
                    p.Price = product.Price;
                    p.PicUrl = product.SPicUrl;
                    p.TotalPrice = p.Price * p.Qty;
                    total += p.TotalPrice;
                }
                ps.Add(p);
            }
            List<object> footers = new List<object>();
            footers.Add(new { TotalPrice = total });
            return Json(new { total = ps.Count, rows = ps, footer = footers });
        }

        public JsonResult GetProduct(string o)
        {
            IList<OrderProductType> list = NSession.CreateQuery("from OrderProductType where OId=" + o).List<OrderProductType>();
            return Json(new { total = list.Count, rows = list });
        }

        public JsonResult GetOrderRecord(string o)
        {
            IList<OrderType> list = NSession.CreateQuery("from OrderType where MId=" + o).List<OrderType>();
            return Json(new { total = list.Count, rows = list });
        }
        public JsonResult GetOrderRecordAmount(string o)
        {
            IList<OrderAmountType> list = NSession.CreateQuery("from OrderAmountType where OId in( select Id  from OrderType where MId=" + o + ")").List<OrderAmountType>();
            return Json(new { total = list.Count, rows = list });
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(OrderAmountType obj)
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
        public OrderAmountType GetById(int Id)
        {
            OrderAmountType obj = NSession.Get<OrderAmountType>(Id);
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
            OrderAmountType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(OrderAmountType obj)
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
                OrderAmountType obj = GetById(id);
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
                    where = " where IsSplit=0 and IsRepeat=0 and" + where;
                }
                else
                {
                    where = " where IsSplit=0 and IsRepeat=0 ";
                }
            }
            IList<OrderAmountType> objList = NSession.CreateQuery("from OrderAmountType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<OrderAmountType>();
            object count = NSession.CreateQuery("select count(Id) from OrderAmountType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

    }
}

