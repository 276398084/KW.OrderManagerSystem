using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;
using System.Data.SqlClient;
using NHibernate.Criterion;

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
            OrderHelper.countrys = NSession.CreateQuery("from CountryType").List<CountryType>().ToList();

            OrderHelper.products = NSession.CreateQuery("from ProductType").List<ProductType>().ToList();

            OrderHelper.currencys = NSession.CreateQuery("from CurrencyType").List<CurrencyType>().ToList();

            OrderHelper.logistics = NSession.CreateQuery("from LogisticsModeType").List<LogisticsModeType>().ToList();
            IList<OrderType> orders = NSession.CreateQuery(" from OrderType where Status='待处理'").List<OrderType>();

            foreach (var order in orders)
            {
                OrderHelper.ValiOrder(order);
            }
            return Json(new { IsSuccess = "true" });
        }

        [HttpPost]
        public ActionResult ImportAmount(FormCollection form)
        {
            string Platform = form["Platform"];
            string Account = form["Account"];
            AccountType account = NSession.Get<AccountType>(Convert.ToInt32(Account));
            string file = form["hfile"];
            OrderHelper.ImportByAmount(account, file);
            return Json(new { IsSuccess = "true" });
        }

        [HttpPost]
        public ActionResult Import(FormCollection form)
        {
            try
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
                return Json(new { IsSuccess = true });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = true, ErrorMsg = ex.Message });

            }

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

        public ActionResult ScanExport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ScanExport(DateTime st, DateTime et, string u, string key)
        {
            if (u != "")
            {
                u = "ScanningBy= '" + u + "' and";
            }
            if (key != "")
            {
                key = "and ScanningBy= '" + key + "' ";
            }
            string sql = "select OrderNo,Weight,ScanningBy,trackCode,ScanningOn,LogisticMode,Country from Orders where {0}  ScanningOn between '{1}' and '{2}'  {3}";
            sql = string.Format(sql, u, st.ToString("yyyy/MM/dd HH:mm:ss"), et.ToString("yyyy/MM/dd HH:mm:ss"), key);
            DataSet ds = new DataSet();
            IDbCommand command = NSession.Connection.CreateCommand();
            command.CommandText = sql;
            SqlDataAdapter da = new SqlDataAdapter(command as SqlCommand);
            da.Fill(ds);
            // 设置编码和附件格式 
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            System.Web.HttpContext.Current.Response.Charset = "gb2312";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + st.ToShortDateString() + "-" + et.ToShortDateString() + ".xls");
            return File(System.Text.Encoding.UTF8.GetBytes(ExcelHelper.GetExcelXml(ds)), "attachment;filename=" + st.ToShortDateString() + "-" + et.ToShortDateString() + ".xls");
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
                List<OrderProductType> list = Session["OrderProducts"] as List<OrderProductType>;
                foreach (OrderProductType orderProductType in list)
                {
                    NSession.Update(orderProductType);
                    NSession.Flush();
                }
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });

        }
        [HttpPost]
        public JsonResult SplitOrder(string o, string rows)
        {
            NSession.Clear();
            OrderType obj = GetById(Utilities.ToInt(o));
            obj.IsSplit = 1;
            NSession.Update(obj);
            NSession.Flush();
            List<OrderProductType> ps = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderProductType>>(rows);
            NSession.Clear();
            obj.Amount = 0;
            obj.IsPrint = 0;
            obj.OrderNo = Utilities.GetOrderNo();
            NSession.Save(obj);
            NSession.Flush();
            foreach (var orderProductType in ps)
            {
                OrderProductType opt = NSession.Get<OrderProductType>(orderProductType.Id);
                opt.Qty = opt.Qty - orderProductType.Qty;
                NSession.Update(opt);
                NSession.Flush();
                orderProductType.OId = obj.Id;
                orderProductType.OrderNo = obj.OrderNo;
                NSession.Save(orderProductType);
                NSession.Flush();
            }
            return Json(new { IsSuccess = true });
        }

        [HttpPost]
        public JsonResult ReOrder(string o, string rows)
        {
            NSession.Clear();
            OrderType obj = GetById(Utilities.ToInt(o));
            obj.IsRepeat = 1;

            NSession.Update(obj);
            NSession.Flush();
            List<OrderProductType> ps = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderProductType>>(rows);
            NSession.Clear();
            obj.Amount = 0;
            obj.IsPrint = 0;
            obj.IsRepeat = 1;
            obj.OrderNo = Utilities.GetOrderNo();
            NSession.Save(obj);

            NSession.Flush();
            foreach (var orderProductType in ps)
            {
                orderProductType.OId = obj.Id;
                orderProductType.OrderNo = obj.OrderNo;
                NSession.Save(orderProductType);
                NSession.Flush();
            }
            return Json(new { IsSuccess = true });
        }
        [HttpPost]
        public JsonResult ReSend(string o)
        {
            IList<OrderType> list = NSession.CreateQuery(" from OrderType where Id in(" + o + ")").List<OrderType>();
            foreach (OrderType orderType in list)
            {
                if (orderType.Status == OrderStatusEnum.已发货.ToString())
                {
                    IList<OrderProductType> ps = NSession.CreateQuery("from OrderProductType where OId=" + orderType.Id).List<OrderProductType>();
                    foreach (OrderProductType orderProductType in ps)
                    {
                        Utilities.StockIn(1, orderProductType.SKU.Trim(), orderProductType.Qty, 0, "重新发货",
                                          CurrentUser.Realname, "");
                    }
                    orderType.Status = OrderStatusEnum.待发货.ToString();
                    NSession.Save(orderType);
                    NSession.Flush();
                }

            }
            return Json(new { IsSuccess = true });
        }

        [HttpGet]
        public ActionResult ExportDown()
        {
            string str = "";
            object sb = Session["ExportDown"];
            if (sb != null)
            {
                str = sb.ToString();
            }
            System.Web.HttpContext.Current.Response.ContentType = "text/plain";
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            System.Web.HttpContext.Current.Response.Charset = "gb2312";
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=zm.txt");
            return File(System.Text.Encoding.UTF8.GetBytes(str), "attachment;filename=zm.txt");
        }
        [HttpPost]
        public ActionResult ExportZM(string o)
        {
            IList<OrderType> list = NSession.CreateQuery(" from OrderType where Id in(" + o + ")").List<OrderType>();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("order-id	buyer-name	buyer-phone-number	sku	product-name	quantity-purchased	recipient-name	ship-address-1	ship-address-2	ship-city	ship-state	ship-postal-code	ship-country");
            string tmpValues = "{0}\t{1}\t{2}\t{3}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}";
            foreach (OrderType foo in list)
            {
                IList<OrderProductType> ps =
                    NSession.CreateQuery("from OrderProductType where OId=" + foo.Id).List<OrderProductType>();
                if (ps.Count > 0)
                {
                    OrderAddressType orderAddress = NSession.Get<OrderAddressType>(foo.AddressId);
                    sb.AppendLine(string.Format(tmpValues, foo.OrderNo, foo.BuyerName, orderAddress.Tel, ps[0].SKU,
                                                ps[0].Qty, orderAddress.Addressee.Replace("\t", " ").Replace(",", " ").Replace("\r", " ").Replace("\n", " "), orderAddress.Street.Replace("\t", " ").Replace(",", " ").Replace("\r", " ").Replace("\n", " "), "",
                                                orderAddress.City.Replace("\t", " ").Replace(",", " ").Replace("\r", " ").Replace("\n", " "), orderAddress.Province.Replace("\t", " ").Replace(",", " ").Replace("\r", " ").Replace("\n", " "), orderAddress.PostCode,
                                                orderAddress.Country));
                }

            }
            Session["ExportDown"] = sb.ToString();
            return Json(new { IsSuccess = true });
        }

        [HttpPost]
        public ActionResult ErrorOrder(string o)
        {
            int t = NSession.CreateQuery(" Update OrderType set Status='" + OrderStatusEnum.作废订单.ToString() + "',IsError=1 where Id in(" + o + ")").ExecuteUpdate();
            if (t > 0)
                return Json(new { IsSuccess = true });
            else
            {
                return Json(new { IsSuccess = false });
            }
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


        public JsonResult GetOrderBySend(string o)
        {
            List<OrderType> orders = NSession.CreateQuery("from OrderType where OrderNo='" + o + "'").List<OrderType>().ToList();
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
                    if (l != "")
                    {
                        order.LogisticMode = l;
                    }
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
                    return Json(new { IsSuccess = true, Result = html, OId = order.Id });
                }
                return Json(new { IsSuccess = false, Result = " 无法出库！ 当前状态为：" + order.Status + "，需要订单状态为“待发货”方可扫描！" });
            }
            return Json(new { IsSuccess = false, Result = "找不到该订单" });
        }
        public JsonResult GetOrderByQue(string orderNo)
        {
            List<OrderType> orders = NSession.CreateQuery("from OrderType where OrderNo='" + orderNo + "'").List<OrderType>().ToList();
            if (orders.Count > 0)
            {
                OrderType order = orders[0];
                if (order.Status == OrderStatusEnum.待拣货.ToString() || (order.Status == OrderStatusEnum.已处理.ToString()))
                {
                    if (order.IsOutOfStock != 1)
                    {
                        string html = "<table width='100%' border='1'><tr><td width='100px' align='right'><b>选择</b></td><td width='120px'><b>SKU</b></td><td  width='120px'><b>Qty</b></td><td><b>Desc</b></td></tr>";
                        foreach (OrderProductType item in NSession.CreateQuery(" from OrderProductType where OId=" + order.Id).List<OrderProductType>())
                        {
                            html += string.Format("<tr><td align='right'><input type='checkbox'  name='ck_{0}' code='{0}' checked=checked /></td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", item.Id, item.SKU, item.Qty, item.Standard);
                        }
                        html += "</table>";
                        return Json(new { IsSuccess = true, Result = html });
                    }
                    return Json(new { IsSuccess = false, Result = "已经是缺货订单！" });
                }
                return Json(new { IsSuccess = false, Result = "订单状态不符！" });
            }
            return Json(new { IsSuccess = false, Result = "找不到该订单" });
        }

        public JsonResult OutStockByQue(string o, string ids)
        {
            List<OrderType> orders = NSession.CreateQuery("from OrderType where OrderNo='" + o + "'").List<OrderType>().ToList();
            if (orders.Count > 0)
            {
                OrderType order = orders[0];
                if (order.Status == OrderStatusEnum.待拣货.ToString() || (order.Status == OrderStatusEnum.已处理.ToString()))
                {
                    OrderRecordType orderRecord = new OrderRecordType();
                    orderRecord.OId = order.Id;
                    orderRecord.OrderNo = order.OrderNo;
                    orderRecord.RecordType = "缺货扫描";
                    orderRecord.CreateBy = CurrentUser.Realname;
                    orderRecord.Content = CurrentUser.Realname + "将订单添加到缺货中";
                    orderRecord.CreateOn = DateTime.Now;
                    NSession.Save(orderRecord);
                    NSession.Flush();
                    order.IsOutOfStock = 1;
                    NSession.Update(order);
                    NSession.Flush();
                    foreach (OrderProductType item in NSession.CreateQuery(" from OrderProductType where OId=" + order.Id).List<OrderProductType>())
                    {
                        if (ids.IndexOf(item.Id.ToString()) != -1)
                        {
                            item.IsQue = 1;
                            NSession.Update(item);
                            NSession.Flush();
                        }

                    }
                    string html = "订单：" + order.OrderNo + " 添加到缺货！";
                    return Json(new { IsSuccess = true, Result = html });
                }
                return Json(new { IsSuccess = false, Result = "订单状态不符！" });
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
                                                            <th width='300px' >图片</th><td width='200px'>SKU*数量</td><td>规格</td><td>名称</td><td>描述</td>
                                                        </tr>";
                    string html2 = @"<tr style='font-weight:bold; font-size:30px;'><td><img width='220px' src='/imgs/pic/{0}/1.jpg' /></td><td>{0}*{1}</td><td>{4}</td><td>{2}</td><td>{3}</td></tr>";
                    order.Products =
                        NSession.CreateQuery("from OrderProductType where OId=" + order.Id).List<OrderProductType>();
                    foreach (var p in order.Products)
                    {
                        html += string.Format(html2, p.SKU, p.Qty, p.Title, p.Remark, p.Standard);
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
                    return Json(new { IsSuccess = true, Result = html });
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

            object count = NSession.CreateQuery("select count(Id) from OrderType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }



    }
}

