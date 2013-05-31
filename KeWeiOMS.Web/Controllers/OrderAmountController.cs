using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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
        public ViewResult StockIndex()
        {
            return View();
        }
        public ViewResult FreightIndex()
        {
            return View();
        }
        public ViewResult FreightCount()
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
        public ActionResult GetFreights(string Id)
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


        public ActionResult StockList(int page, int rows, string sort, string order, string search)
        {
            string where = "";
            string orderby = Utilities.OrdeerBy(sort, order);
            if (!string.IsNullOrEmpty(search))
            {
                where = Utilities.Resolve(search);
                if (where.Length > 0)
                {
                    where = " where " + where;
                }
            }
            IList<object[]> objList = NSession.CreateSQLQuery("select SKU,OldSKU,ProductName as Title,Price,(select COUNT(1) from SKUCode where IsOut=0 and SKU=P.SKU ) as Qty,(Price*(select COUNT(1) from SKUCode where IsOut=0 and SKU=P.SKU )) as TotalPrice from Products P " + where + " " + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<object[]>();
            List<ProductData> list = new List<ProductData>();
            foreach (object[] objectse in objList)
            {
                ProductData pd = new ProductData();
                pd.SKU = objectse[0].ToString();
                pd.Title = objectse[2].ToString();
                pd.Price = Convert.ToDouble(objectse[3]);
                pd.Qty = Convert.ToInt32(objectse[4]);
                pd.TotalPrice = Math.Round(Convert.ToDouble(objectse[5]), 2);

                list.Add(pd);
            }
            object count = NSession.CreateQuery("select count(Id) from ProductType " + where).UniqueResult();
            object total = NSession.CreateSQLQuery("select SUM(qty) from (select (Price*(select COUNT(1) from SKUCode where IsOut=0 and SKU=P.SKU)) as Qty from Products P " + where + ") as t").UniqueResult();
            List<object> footers = new List<object>();
            footers.Add(new { TotalPrice = Math.Round(Convert.ToDouble(total), 2), SKU = "合计：" });
            return Json(new { total = count, rows = list, footer = footers });
        }

        public ActionResult ResetFreight(string search, decimal z)
        {
            string orderby;
            var where = GetWhere(null, null, search, out @orderby);
            IList<OrderType> objList = NSession.CreateQuery("from OrderType " + where + orderby)

                .List<OrderType>();
            foreach (OrderType orderType in objList)
            {
                orderType.Freight = Convert.ToDouble(OrderHelper.GetFreight(orderType.Weight, orderType.LogisticMode, orderType.Country, NSession, z));
                NSession.SaveOrUpdate(orderType);
                NSession.Flush();
                OrderHelper.SaveAmount(orderType, NSession);
            }
            return Json(new { IsSuccess = true });
        }

        public ActionResult ExportStockList()
        {
            string sql =
                "select SKU,OldSKU,ProductName as Title,Price,(select COUNT(1) from SKUCode where IsOut=0 and SKU=P.SKU ) as Qty,(Price*(select COUNT(1) from SKUCode where IsOut=0 and SKU=P.SKU )) as TotalPrice from Products P";
            DataSet ds = new DataSet();
            IDbCommand command = NSession.Connection.CreateCommand();
            command.CommandText = sql;
            SqlDataAdapter da = new SqlDataAdapter(command as SqlCommand);
            da.Fill(ds);
            // 设置编码和附件格式 
            Session["ExportDown"] = ExcelHelper.GetExcelXml(ds);
            return Json(new { IsSuccess = true });
        }

        [HttpPost]
        public ActionResult GetFreightSumList(string search, int isa, int isl)
        {
            string orderby;
            var where = GetWhere("", "", search, out @orderby);
            string[] strs = new string[] { "", "", "", "" };
            string sql =
                "select COUNT(1) as count,SUM(Freight) as total {0} {1} from Orders where CreateOn between '2013-03-01' and '2013-04-01' and Status='已发货' group by {2} {3}";
            if (isa == 1)
            {
                strs[0] = ",platform,account";
                strs[2] = "platform,account";
            }
            if (isl == 1)
            {
                strs[1] = ",LogisticMode ";
                if (isa == 1)
                    strs[3] = ",LogisticMode";
                else
                    strs[3] = " LogisticMode";
            }
            sql = string.Format(sql, strs[0], strs[1], strs[2], strs[3]);
            IList<object[]> objList = NSession.CreateSQLQuery(sql)
                .List<object[]>();
            List<OrderData> os = new List<OrderData>();
           
            object count = NSession.CreateQuery("select count(Id) from OrderType " + where).UniqueResult();

            return Json(new { total = count, rows = os });
        }

        [HttpPost]
        public ActionResult GetFreightList(int page, int rows, string sort, string order, string search)
        {
            string orderby;
            var where = GetWhere(sort, order, search, out @orderby);
            IList<OrderType> objList = NSession.CreateQuery("from OrderType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<OrderType>();
            List<OrderData> os = new List<OrderData>();
            foreach (var o in objList)
            {
                AddToOrderData(o, os);
            }
            object count = NSession.CreateQuery("select count(Id) from OrderType " + where).UniqueResult();
            return Json(new { total = count, rows = os });
        }

        private static string GetWhere(string sort, string order, string search, out string @orderby)
        {
            string where = "";
            @orderby = " order by Id desc";
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
            {
                @orderby = " order by " + sort + " " + order;
            }
            if (!string.IsNullOrEmpty(search))
            {
                @where = Utilities.Resolve(search);
                if (@where.Length > 0)
                {
                    @where = " where Enabled=1 and  Status <> '待处理' and " + @where;
                }
            }
            if (@where.Length == 0)
            {
                @where = " where Enabled=1 and  Status <> '待处理'";
            }
            return @where;
        }

        private static void AddToOrderData(OrderType order, List<OrderData> os)
        {
            OrderData o = new OrderData();

            if (order != null)
            {
                o.OrderNo = order.OrderNo;
                o.OrderExNo = order.OrderExNo;
                o.TrackCode = order.TrackCode;
                o.Weight = order.Weight;
                o.RMB = order.RMB;
                o.Country = order.Country;
                o.CurrencyCode = order.CurrencyCode;
                o.LogisticMode = order.LogisticMode;
                o.OrderAmount = order.Amount;

                o.Status = order.Status;
                o.OrderType = "正常";
                if (order.IsRepeat == 1)
                    o.OrderType = "重发";
                if (order.IsSplit == 1)
                    o.OrderType += "拆包";
                o.Country = order.Country;
                o.SendOn = order.ScanningOn;
                o.Freight = order.Freight;
                o.Account = order.Account;
                o.Platform = order.Platform;
            }
            os.Add(o);
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
                AddToOrderData(order, os);
                os[os.Count - 1].TotalCost = orderAmountType.TotalCosts;
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
            string orderby = Utilities.OrdeerBy(sort, order);
            if (!string.IsNullOrEmpty(search))
            {
                where = Utilities.Resolve(search);
                if (where.Length > 0)
                {
                    where = " where IsSplit=0 and IsRepeat=0 and " + where;
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

        [HttpPost]
        public ActionResult AccountFreightCount(DateTime st, DateTime et, string p, string a, string t)
        {
            IList<AccountFreigheCount> sores = new List<AccountFreigheCount>();
            string where = Where(st, et, p, a,t);
            IList<object[]> objectses = NSession.CreateQuery("select Platform,Account,Count(Id),Sum(Freight) from OrderType " + where + "  group by Account,Platform").List<object[]>();
            decimal sum = 0;
            decimal freight = 0;
            foreach (var item in objectses)
            {
                string pp = item[0].ToString();
                string aa = item[1].ToString();
                decimal co = Convert.ToDecimal(item[2]);
                decimal am = decimal.Round(decimal.Parse(item[3].ToString()),2);
                sores.Add(new AccountFreigheCount { Platform = pp,Account=aa, Count =co,Amount=am });
                sum += co;
                freight += am;
            }
            List<object> footers = new List<object>();
            footers.Add(new { Count = sum,Amount=decimal.Round(freight,2)});
            return Json(new { rows = sores.OrderByDescending(x => x.Amount), footer = footers, total = sores.Count });
        }
        public ActionResult TypeFreightCount(DateTime st, DateTime et, string p, string a, string t)
        {
            IList<AccountFreigheCount> sores = new List<AccountFreigheCount>();
            string where = Where(st, et, p, a, t);
            IList<object[]> objectses = NSession.CreateQuery("select IsRepeat,IsSplit,Count(Id),Sum(Freight) from OrderType " + where + "  group by IsRepeat,IsSplit").List<object[]>();
            decimal sum = 0;
            decimal freight = 0;
            foreach (var item in objectses)
            {
                string str = "正常";
                if(item[0].ToString()=="1")
                {
                    str = "重发";
                }
                if (item[1].ToString() == "1")
                {
                    str += "拆包";
                }
                string aa = str;
                decimal co = Convert.ToDecimal(item[2]);
                decimal am = decimal.Round(decimal.Parse(item[3].ToString()), 2);
                sores.Add(new AccountFreigheCount { Account = aa, Count = co, Amount = am });
                sum += co;
                freight += am;
            }
            List<object> footers = new List<object>();
            footers.Add(new { Count = sum, Amount = decimal.Round(freight, 2) });
            return Json(new { rows = sores.OrderByDescending(x => x.Amount), footer = footers, total = sores.Count });
        }

        public string Where(DateTime st, DateTime et, string p, string a, string t)
        {
            string where = "where Status='已发货' and CreateOn between '" + st.ToString("yyyy-MM-dd") + "' and '" + et.ToString("yyyy-MM-dd") + " 23:59:59'";
            if (p != "ALL")
            {
                where += " and Platform='" + p + "'";
            }
            if (a != "ALL")
            {
                where += " and Account='" + a + "'";
            }
            if (t != "ALL")
            {
                if (t == "正常")
                {
                    where += " and IsRepeat=0 ";
                }
                if (t == "重发")
                {
                    where += " and IsRepeat=1 ";
                }
            }
            return where;
        }
    }
}

