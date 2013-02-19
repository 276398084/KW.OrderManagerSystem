using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;

namespace KeWeiOMS.Web.Controllers
{
    public class OrderController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
        public ActionResult UnHandle()
        {
            return View();
        }
        public ActionResult TodayIndex()
        {
            return View();

        }
        public ActionResult QueScan()
        {
            return View();

        }
        public ActionResult Merge()
        {
            return View();

        }
        public ActionResult PeiScan()
        {
            return View();

        }
        public ActionResult SendScan()
        {
            return View();

        }
        public ActionResult JiScan()
        {
            return View();
        }
        [HttpPost]
        public ActionResult OrderReplace(string ids, string oldField, string newField, string fieldName)
        {
            switch (fieldName)
            {
                case "Country":
                    OrderHelper.ReplaceByCountry(ids, oldField, newField);
                    break;
                case "SKU":
                    OrderHelper.ReplaceBySKU(ids, oldField, newField);
                    break;
                case "CurrencyCode":
                    OrderHelper.ReplaceByCurrencyOrLogistic(ids, oldField, newField, 1);
                    break;
                case "LogisticMode":
                    OrderHelper.ReplaceByCurrencyOrLogistic(ids, oldField, newField, 0);
                    break;
                default:
                    break;

            }
            IQuery Query = NSession.CreateQuery(string.Format("update OrderType set {0}='{1}' where {0}='{2}'", fieldName, newField, oldField));
            int num = Query.ExecuteUpdate();
            return Json(new { IsSuccess = "true" });
        }





        public ActionResult Import()
        {
            return View();
        }

        public ActionResult OrderVali()
        {

            IList<OrderType> orders = NSession.CreateQuery(" from OrderType where Status='待处理'").List<OrderType>();

            foreach (var order in orders)
            {
                OrderHelper.ValiOrder(order);
            }
            return Json(new { IsSuccess = "true" });
        }

        [HttpPost]
        public ActionResult Import(FormCollection form)
        {
            string Platform = form["Platform"];
            string Account = form["Account"];
            AccountType account = NSession.Get<AccountType>(Convert.ToInt32(Account));
            string file = form["hfile"];

            List<ResultInfo> results = new List<ResultInfo>();
            switch ((PlatformEnum)Enum.Parse(typeof(PlatformEnum), Platform))
            {
                case PlatformEnum.SMT:
                    results = OrderHelper.ImportBySMT(account, file);
                    break;
                case PlatformEnum.Ebay:
                    break;
                case PlatformEnum.Amazon:
                    results = OrderHelper.ImportByAmazon(account, file);
                    break;
                case PlatformEnum.B2C:
                    results = OrderHelper.ImportByB2C(account, file);
                    break;
                case PlatformEnum.Gmarket:
                    results = OrderHelper.ImportByGmarket(account, file);
                    break;
                case PlatformEnum.LT:
                    break;

                default:
                    break;
            }
            return Json(new { IsSuccess = "true" });
        }

        [HttpPost]
        public ActionResult Synchronous(string Platform, int Account, DateTime st, DateTime et)
        {

            AccountType account = NSession.Get<AccountType>(Convert.ToInt32(Account));


            List<ResultInfo> results = new List<ResultInfo>();
            switch ((PlatformEnum)Enum.Parse(typeof(PlatformEnum), Platform))
            {

                case PlatformEnum.Ebay:
                    results = OrderHelper.APIByEbay(account, st, et);
                    break;
                case PlatformEnum.Amazon:

                    break;
                case PlatformEnum.B2C:
                    results = OrderHelper.APIByB2C(account, st, et);
                    break;
                case PlatformEnum.Gmarket:
                case PlatformEnum.LT:
                case PlatformEnum.SMT:
                default:
                    return Json(new { ErrorMsg = "该平台没有同步功能！" });

            }
            return Json(new { IsSuccess = "true" });
        }


        [HttpPost]
        public JsonResult Create(OrderType obj)
        {
            try
            {
                NSession.Save(obj.AddressInfo);
                obj.AddressId = obj.AddressInfo.Id;
                obj.GenerateOn = obj.ScanningOn = obj.CreateOn = DateTime.Now;
                List<OrderProductType> list = Session["OrderProducts"] as List<OrderProductType>;
                NSession.Save(obj);
                foreach (var item in list)
                {
                    item.OId = obj.Id;
                    item.OrderNo = obj.OrderNo;
                    NSession.Save(item);
                }

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
        public OrderType GetById(int Id)
        {
            OrderType obj = NSession.Get<OrderType>(Id);
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
            OrderType obj = GetById(id);
            obj.AddressInfo = NSession.Get<OrderAddressType>(obj.AddressId);
            ViewData["id"] = id;
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(OrderType obj)
        {

            try
            {
                NSession.Update(obj.AddressInfo);
                obj.Country = obj.AddressInfo.Country;
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
                OrderType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }


        public JsonResult GetOrderBySend(string orderNo)
        {
            List<OrderType> orders = NSession.CreateQuery("from OrderType where OrderNo='" + orderNo + "'").List<OrderType>().ToList();
            if (orders.Count > 0)
            {
                OrderType order = orders[0];
                if (order.Status == OrderStatusEnum.待发货.ToString())
                {
                    string html = "订单:" + order.OrderNo + ", 当前状态：待发货，可以发货。<br>发货方式：" +
                                 "<s id='logisticsMode'>" + order.LogisticMode + "</s>";
                    return Json(new { IsSuccess = true, Result = html });
                }
                return Json(new { IsSuccess = false, Result = " 无法出库！ 当前状态为：" + order.Status + "，需要订单状态为“待发货”方可扫描！" });
            }
            return Json(new { IsSuccess = false, Result = "找不到该订单" });
        }

        public JsonResult OutStockBySend(string o, string t, int s, string l, double w)
        {
            List<OrderType> orders = NSession.CreateQuery("from OrderType where OrderNo='" + o + "'").List<OrderType>().ToList();
            if (orders.Count > 0)
            {
                OrderType order = orders[0];
                if (order.Status == OrderStatusEnum.待发货.ToString())
                {
                    order.TrackCode = t;
                    order.Weight = Convert.ToInt32(w);
                    order.LogisticMode = l;
                    order.ScanningOn = DateTime.Now;
                    order.Status = OrderStatusEnum.已发货.ToString();
                    order.ScanningBy = CurrentUser.Realname;
                    NSession.Update(order);
                    NSession.Flush();
                    IList<OrderProductType> ps =
                        NSession.CreateQuery("from OrderProductType where OId=" + order.Id).List<OrderProductType>();
                    foreach (var orderProductType in ps)
                    {
                        Utilities.StockOut(s, orderProductType.SKU, orderProductType.Qty, "扫描出库", CurrentUser.Realname, "", order.OrderNo);
                    }
                    string html = "订单： " + order.OrderNo + "已经发货";
                    return Json(new { IsSuccess = true, Result = "" });
                }
                return Json(new { IsSuccess = false, Result = " 无法出库！ 当前状态为：" + order.Status + "，需要订单状态为“待发货”方可扫描！" });
            }
            return Json(new { IsSuccess = false, Result = "找不到该订单" });
        }


        public JsonResult GetOrderByPei(string orderNo)
        {
            List<OrderType> orders = NSession.CreateQuery("from OrderType where OrderNo='" + orderNo + "'").List<OrderType>().ToList();
            if (orders.Count > 0)
            {
                OrderType order = orders[0];
                if (order.Status == OrderStatusEnum.待拣货.ToString() || (order.Status == OrderStatusEnum.已处理.ToString()))
                {
                    string html = @"  <table width='100%' class='dataTable'>
                                                        <tr class='dataTableHead'>
                                                            <th width='300px' >图片</th><td width='200px'>SKU*数量</td><td>名称</td><td>描述</td>
                                                        </tr>";
                    string html2 = @"<tr style='font-weight:bold; font-size:30px;'><td><img width='220px' src='/imgs/pic/{0}/1.jpg' /></td><td>{0}*{1}</td><td>{2}</td><td>{3}</td></tr>";
                    order.Products =
                        NSession.CreateQuery("from OrderProductType where OId=" + order.Id).List<OrderProductType>();
                    foreach (var p in order.Products)
                    {
                        html += string.Format(html2, p.SKU, p.Qty, p.Title, p.Standard);
                    }
                    html += "</table>";
                    return Json(new { IsSuccess = true, Result = html });
                }
                return Json(new { IsSuccess = false, Result = "订单状态不符！" });
            }
            return Json(new { IsSuccess = false, Result = "找不到该订单" });
        }

        public JsonResult OutStockByPei(string p1, string p2, string o)
        {
            List<OrderType> orders = NSession.CreateQuery("from OrderType where OrderNo='" + o + "'").List<OrderType>().ToList();
            if (orders.Count > 0)
            {
                OrderType order = orders[0];
                if (order.Status == OrderStatusEnum.待拣货.ToString() || (order.Status == OrderStatusEnum.已处理.ToString()))
                {
                    OrderPeiRecordType orderPeiRecord = new OrderPeiRecordType { OrderNo = order.OrderNo, PeiBy = p1, ValiBy = p2, CreateOn = DateTime.Now, OId = order.Id, ScanBy = CurrentUser.Realname };
                    NSession.Save(orderPeiRecord);
                    NSession.Flush();
                    order.Status = OrderStatusEnum.待包装.ToString();
                    NSession.Update(order);
                    NSession.Flush();
                    string html = "订单：" + order.OrderNo + " 配货完成！";
                    return Json(new { IsSuccess = true, Result = html });
                }
                return Json(new { IsSuccess = false, Result = "订单状态不符！" });
            }
            return Json(new { IsSuccess = false, Result = "找不到该订单" });
        }


        public JsonResult OutStockByJi(string p, string o)
        {
            List<OrderType> orders = NSession.CreateQuery("from OrderType where OrderNo='" + o + "'").List<OrderType>().ToList();
            if (orders.Count > 0)
            {
                OrderType order = orders[0];
                if (order.Status == OrderStatusEnum.待包装.ToString())
                {

                    order.Status = OrderStatusEnum.待发货.ToString();
                    NSession.Update(order);
                    NSession.Flush();

                    OrderPackRecordType orderPackRecord = new OrderPackRecordType
                                                              {
                                                                  OId = order.Id,
                                                                  OrderNo = order.OrderNo,
                                                                  PackBy = p,
                                                                  PackOn = DateTime.Now,
                                                                  ScanBy = CurrentUser.Realname
                                                              };
                    NSession.Save(orderPackRecord);
                    NSession.Flush();
                    string html = "订单： " + order.OrderNo + "计件成功！包装人：" + p;
                    return Json(new { IsSuccess = true, Result = "" });
                }
                return Json(new { IsSuccess = false, Result = " 无法出库！ 当前状态为：" + order.Status + "，需要订单状态为“待发货”方可扫描！" });
            }
            return Json(new { IsSuccess = false, Result = "找不到该订单" });

        }

        public JsonResult UnHandleList(int page, int rows, string sort, string order, string search)
        {
            return List(page, rows, sort, order, search, 1);
        }

        public JsonResult List(int page, int rows, string sort, string order, string search, int isUn = 0)
        {
            string where = "";
            string orderby = " order by Id Desc";
            string flag = "<>";
            if (isUn == 1)
                flag = "=";
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
            {
                orderby = " order by " + sort + " " + order;
            }

            if (!string.IsNullOrEmpty(search))
            {
                where = Utilities.Resolve(search);
                if (where.Length > 0)
                {
                    where = " where Status" + flag + "'待处理' and " + where;
                }
            }
            if (where.Length == 0)
            {
                where = " where Status" + flag + "'待处理'";
            }
            IList<OrderType> objList = NSession.CreateQuery("from OrderType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<OrderType>();

            object count = NSession.CreateQuery("select count(Id) from OrderType " + where ).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

    }
}

