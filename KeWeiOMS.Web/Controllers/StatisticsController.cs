using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using KeWeiOMS.Domain;
using System.Collections;

namespace KeWeiOMS.Web.Controllers
{
    public class StatisticsController : BaseController
    {
        public ActionResult OrderCount()
        {
            return View();
        }

        public ActionResult OutCount()
        {
            return View();
        }

        public ActionResult SellCount()
        {
            return View();
        }
        public ActionResult ScanCount()
        {
            return View();
        }
        public ActionResult PurchaseInfo()
        {
            return View();
        }

        public ActionResult OrderSendInfo()
        {
            return View();
        }


        [HttpPost]
        public ActionResult OrderCount(DateTime st, DateTime et, string a, string p)
        {
            var sqlWhere = SqlWhere(st, et, a, p);
            IList<object[]> objs = NSession.CreateQuery(string.Format("select Account,Count(Id),Platform,Sum(Amount) from OrderType {0} group by Account,Platform", sqlWhere)).List<object[]>();

            List<OrderCount> list = new List<OrderCount>();
            int sum = 0;
            foreach (object[] objectse in objs)
            {
                OrderCount oc = new OrderCount { Account = objectse[0].ToString(), OCount = Convert.ToInt32(objectse[1]), Platform = objectse[2].ToString() };
                sum += Convert.ToInt32(objectse[1]);
                list.Add(oc);
            }
            List<object> footers = new List<object>();
            footers.Add(new { OCount = sum });
            return Json(new { rows = list.OrderByDescending(f => f.OCount), footer = footers, total = list.Count });
        }

        [HttpPost]
        public ActionResult OrderCountryData(DateTime st, DateTime et, string a, string p)
        {
            var sqlWhere = SqlWhere(st, et, a, p);
            IList<object[]> objs = NSession.CreateQuery(string.Format("select Country,COUNT(Id) from OrderType {0} group by Country", sqlWhere)).List<object[]>();
            object obj =
                NSession.CreateQuery(string.Format("select COUNT(Id) from OrderType {0} ", sqlWhere))
                    .UniqueResult();
            decimal sum = Convert.ToDecimal(obj);
            List<ProportionData> list = new List<ProportionData>();
            foreach (object[] objectse in objs)
            {
                ProportionData pd = new ProportionData();
                pd.Count = Convert.ToInt32(objectse[1]);
                pd.Key = objectse[0].ToString();
                pd.Proportion = Math.Round((Convert.ToDecimal(pd.Count) / sum) * 100, 2);
                list.Add(pd);
            }
            List<object> footers = new List<object>();
            footers.Add(new { Count = sum });
            return Json(new { rows = list.OrderByDescending(f => f.Proportion), footer = footers, total = list.Count });
        }

        [HttpPost]
        public ActionResult OrderLeveData(DateTime st, DateTime et, string a, string p)
        {
            int sum = 0;
            var sqlWhere = SqlWhere(st, et, a, p);
            List<LeveData> list = new List<LeveData>();
            IList<OrderType> objs = NSession.CreateQuery(string.Format("from OrderType "+sqlWhere)).List<OrderType>();
            //定义区间
            double[,] arry = { { 0, 5 }, { 5, 10 }, { 10, 20 }, {20, 50 }, { 50, 100 }, { 100,0} };
            for (int i = 0; i < arry.Length/2; i++)
            {
                int count=0;
                LeveData leve = new LeveData();
                foreach (var item in objs)
                {
                    if (arry[i, 1] != Convert.ToDouble(0))
                    {
                        if (item.Amount >= arry[i, 0] && item.Amount < arry[i, 1])
                        {
                            
                            count++;
                        }
                    }
                    else
                    {
                        if (item.Amount >= arry[i, 0])
                        {
                            count++;
                        }
                    }
                }      
                sum = objs.Count;
                if (sum != 0)
                { 
                    if (arry[i, 1] != Convert.ToDouble(0))
                        leve.Platform = arry[i, 0] + "-" + arry[i, 1];
                    else
                        leve.Platform = arry[i, 0] + " 以上";
                    leve.Account = count;
                    leve.OCount = Math.Round((Convert.ToDecimal(count)/ sum) * 100, 2);
                    list.Add(leve);
                }
            }
            List<object> footers = new List<object>();
            footers.Add(new { Account = sum });
            return Json(new { rows = list, footer = footers, total = list.Count });
        }

        #region 出库统计
        /// <summary>
        /// OutCount
        /// </summary>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <param name="a"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OutCount(DateTime st, DateTime et, string a, string p)
        {
            var list = GetOutCount(st, et, a, p);
            List<object> footers = new List<object>();
            return Json(new { rows = list.OrderByDescending(f => f.Qty), total = list.Count });
        }

        private List<ProductData> GetOutCount(DateTime st, DateTime et, string a, string p)
        {
            var sqlWhere = SqlWhere(st, et, a, p);
            IList<object[]> objs =
                NSession.CreateQuery(
                    string.Format(
                        "select SKU,SUM(Qty) as Qty from OrderProductType where OId in(select Id from OrderType {0} and Status='已发货') group by SKU ",
                        sqlWhere)).List<object[]>();
            string sku = string.Empty;
            List<ProductData> list = new List<ProductData>();
            foreach (object[] objectse in objs)
            {
                sku += objectse[0] + ",";
                ProductData pd = new ProductData();
                pd.SKU = objectse[0].ToString();
                pd.Qty = Convert.ToInt32(objectse[1]);
                list.Add(pd);
            }

            List<ProductType> products =
                NSession.CreateQuery("from ProductType where SKU in('" + sku.Trim(',').Replace(",", "','") + "')").List
                    <ProductType>().ToList();
            foreach (ProductData pp in list)
            {
                ProductType product = products.Find(x => pp.SKU.Trim().ToUpper() == x.SKU.Trim().ToUpper());
                if (product != null)
                {
                    pp.Price = product.Price;
                    pp.PicUrl = product.SPicUrl;
                    pp.Title = product.ProductName;
                    pp.TotalPrice = pp.Price * pp.Qty;
                }
            }
            return list;
        }


        /// <summary>
        /// 导出缺货
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public JsonResult ExportOut(DateTime st, DateTime et, string a, string p)
        {
            var list = GetOutCount(st, et, a, p);
            // 设置编码和附件格式 
            Session["ExportDown"] = ExcelHelper.GetExcelXml(Utilities.FillDataTable(list));
            return Json(new { IsSuccess = true });
        }



        #endregion

        #region 销售统计

        [HttpPost]
        public ActionResult SellCount(DateTime st, DateTime et, string a, string p)
        {
            var list = GetSellCount(st, et, a, p);
            List<object> footers = new List<object>();
            return Json(new { rows = list.OrderByDescending(f => f.Qty), total = list.Count });
        }
        [HttpPost]
        private List<ProductData> GetSellCount(DateTime st, DateTime et, string a, string p)
        {
            var sqlWhere = SqlWhere(st, et, a, p);
            IList<object[]> objs =
                NSession.CreateSQLQuery(
                    string.Format(
                        "select SKU,SUM(Qty) as sQty,count(1) as Qty from OrderProducts right join Orders on OId=Orders.Id   {0} group by SKU ",
                        sqlWhere)).List<object[]>();
            string sku = string.Empty;
            List<ProductData> list = new List<ProductData>();
            foreach (object[] objectse in objs)
            {
                sku += objectse[0] + ",";
                ProductData pd = new ProductData();
                pd.SKU = objectse[0].ToString();
                pd.Qty = Convert.ToInt32(objectse[1]);
                pd.OQty = Convert.ToInt32(objectse[2]);
                list.Add(pd);
            }

            List<ProductType> products =
                NSession.CreateQuery("from ProductType where SKU in('" + sku.Trim(',').Replace(",", "','") + "')").List
                    <ProductType>().ToList();
            foreach (ProductData pp in list)
            {
                ProductType product = products.Find(x => pp.SKU.Trim().ToUpper() == x.SKU.Trim().ToUpper());
                if (product != null)
                {
                    pp.Category = product.Category;
                    pp.Status = product.Status;
                    pp.Price = product.Price;
                    pp.PicUrl = product.SPicUrl;
                    pp.Title = product.ProductName;
                    pp.TotalPrice = pp.Price * pp.Qty;
                }
            }
            return list;
        }


        /// <summary>
        /// 导出缺货
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public JsonResult ExportSellCount(DateTime st, DateTime et, string a, string p)
        {
            var list = GetSellCount(st, et, a, p);
            // 设置编码和附件格式 
            Session["ExportDown"] = ExcelHelper.GetExcelXml(Utilities.FillDataTable(list));
            return Json(new { IsSuccess = true });
        }
        #endregion

        #region 缺货统计
        public ActionResult QueCount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult QueData(string order, string sort, string s)
        {
            string orderby = "";
            if (!string.IsNullOrEmpty(order) && !string.IsNullOrEmpty(sort))
            {
                orderby = " order by " + sort + " " + order;
            }
            if (s != null)
            {
                s = " and OP.SKU like '%" + s + "%'";
            }
            IList<object[]> objs = NSession.CreateSQLQuery(string.Format(@"select * ,(Qty-BuyQty) as NeedQty,(select COUNT(Id) from SKUCode where IsOut=0 and SKUCode.SKU=tbl.SKU) as UnPei from (
select SKU,SUM(Qty) as Qty,MIN(CreateOn) as MinDate,isnull(Standard,0) as Standard,(select isnull(SUM(Qty-DaoQty),0) from PurchasePlan where Status<>'异常' and Status<>'已收到' and  SKU=OP.SKU and BuyOn>MIN(O.CreateOn)) as BuyQty from Orders O left join OrderProducts OP On O.Id=OP.OId where O.IsOutOfStock=1 and O.Status<>'作废订单' and  O.Status<>'已发货' and OP.IsQue=1 {0} group by SKU,Standard
) as tbl {1}", s, orderby)).List<object[]>();

            List<QueCount> list = new List<QueCount>();
            foreach (object[] objectse in objs)
            {
                QueCount oc = new QueCount { SKU = objectse[0].ToString(), Qty = Utilities.ToInt(objectse[1]), MinDate = Convert.ToDateTime(objectse[2]), BuyQty = Utilities.ToInt(objectse[4]), UnPeiQty = Utilities.ToInt(objectse[5]) };
                if (objectse[3] is DBNull || objectse[3] == null)
                {
                }
                else
                {
                    oc.Standard = objectse[3].ToString();
                }
                oc.NeedQty = oc.Qty - oc.BuyQty;
                if (ValidateRequest)
                {
                    if (oc.NeedQty <= 0)
                    {
                        oc.NeedQty = 0;
                    }
                }
                list.Add(oc);
            }

            return Json(list);
        }


        public JsonResult ExportQue(string s)
        {
            if (s != null)
            {
                s = " and OP.SKU like '%" + s + "%'";
            }

            string sql = string.Format(@"select * ,(Qty-BuyQty) as NeedQty from (
select SKU,SUM(Qty) as Qty,MIN(CreateOn) as MinDate,isnull(Standard,0) as Standard,(select isnull(SUM(Qty),0) from PurchasePlan where SKU=OP.SKU and BuyOn>MIN(O.CreateOn)) as BuyQty from Orders O left join OrderProducts OP On O.Id=OP.OId where O.IsOutOfStock=1 and  O.Status<>'作废订单' and OP.IsQue=1 {0} group by SKU,Standard
) as tbl", s);

            DataSet ds = new DataSet();
            IDbCommand command = NSession.Connection.CreateCommand();
            command.CommandText = sql;
            SqlDataAdapter da = new SqlDataAdapter(command as SqlCommand);
            da.Fill(ds);
            // 设置编码和附件格式 
            Session["ExportDown"] = ExcelHelper.GetExcelXml(ds);
            return Json(new { IsSuccess = true });
        }
        #endregion

        public ActionResult PurchaseStatistcs(int Id)
        {

            //Id ：1为 三天未采购 2为5天未到货
            IList<PurchasePlanType> list = new List<PurchasePlanType>();
            if (Id == 1)
            {
                list = NSession.CreateQuery("from PurchasePlanType where Status in ('已采购') and  BuyOn <=dateadd(day,-3,GETDATE())").List<PurchasePlanType>();
            }
            else
            {
                list = NSession.CreateQuery("from PurchasePlanType where Status in ('已发货','部分发货') and  BuyOn <=dateadd(day,-5,GETDATE())").List<PurchasePlanType>();
            }
            List<object> footers = new List<object>();

            return Json(new { rows = list.OrderBy(f => f.BuyOn), total = list.Count });
        }

        public ActionResult OrderSendStatistcs(int Id)
        {
            IList<OrderType> orderTypes = new List<OrderType>();
            DateTime dt = DateTime.Now;

            switch (Id)
            {
                case 1:
                    orderTypes = NSession.CreateQuery("from OrderType where Status='已处理' and IsOutOfStock=0 and CreateOn<'" + dt.AddDays(-1).ToString("yyyy/MM/dd HH:mm:ss") + "'").List<OrderType>();
                    break;

                case 2:
                    orderTypes = NSession.CreateQuery("from OrderType where Status='待包装' and IsOutOfStock=0 and CreateOn<'" + dt.AddHours(-12).ToString("yyyy/MM/dd HH:mm:ss") + "'").List<OrderType>();
                    break;
                case 3:
                    orderTypes = NSession.CreateQuery("from OrderType where Status='待发货' and IsOutOfStock=0 and CreateOn<'" + dt.AddHours(-12).ToString("yyyy/MM/dd HH:mm:ss") + "'").List<OrderType>();
                    break;

            }
            return Json(new { rows = orderTypes.OrderBy(f => f.CreateOn), total = orderTypes.Count });
        }


        private string SqlWhere(DateTime st, DateTime et, string a, string p)
        {
            string sqlWhere = " where Status<>'待处理' and IsSplit=0 and IsRepeat=0 and CreateOn between '" + st.ToString("yyyy/MM/dd 00:00:00") + "' and '" +
                              et.AddDays(1).ToString("yyyy/MM/dd 00:00:00") + "' and";
            if (!string.IsNullOrEmpty(p) && p != "ALL")
            {
                sqlWhere += " Platform='" + p + "' and";
            }
            if (!string.IsNullOrEmpty(a) && a != "ALL")
            {
                sqlWhere += " Account='" + a + "' and";
            }
            sqlWhere = sqlWhere.Substring(0, sqlWhere.Length - 3);
            return sqlWhere;
        }

        /// <summary>
        /// 获取中英文星期
        /// </summary>
        /// <param name="lan">中文(zh)or英文(en)</param>
        /// <returns></returns>
        public string GetWeek(string lan, DateTime date)
        {
            string[] weeks = null;
            if (lan == "zh")
                weeks = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            else if (lan == "en")
                weeks = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            else
                weeks = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };

            int intWeek = Convert.ToInt32(date.DayOfWeek);
            return weeks[intWeek];
        }

        [HttpPost]
        public ActionResult GetColumns(DateTime st, DateTime et)
        {

            List<object> cols = new List<object>();
            cols.Add(new { field = "人员", title = "人员", width = "150" });
            DateTime date = st;
            while (date <= et)
            {
                string week = GetWeek("zh", date);
                cols.Add(new { field = date.ToString("MMdd"), title = date.ToString("MM.dd") + "(" + week + ")" });
                date = date.AddDays(1);
            }
            return Json(new { columns = cols });
        }

        [HttpPost]
        public String ScanCount(DateTime st, DateTime et)
        {

            List<string> strData = new List<string>();
            StringBuilder sb = new StringBuilder();
            strData.Add("人员");
            sb.Append("select [ScanningBy] as '扫描人',");
            DateTime date = st;
            while (date <= et)
            {
                string week = GetWeek("zh", date);
                strData.Add(date.ToString("MMdd"));

                sb.Append("SUM(case  when convert(varchar(10),[ScanningOn],120)='" + date.ToString("yyyy-MM-dd") + "' then  rcount else 0 end  ) as '" + date.ToString("MM.dd") + "(" + week + ")' ,");
                date = date.AddDays(1);
            }
            sb = sb.Remove(sb.Length - 1, 1);
            sb.Append("from  (select [ScanningBy] ,convert(varchar(10),[ScanningOn],120) [ScanningOn] ,COUNT(1) as 'rcount'  from Orders where Status='已发货' and [ScanningOn] between '" + st.ToString("yyyy-MM-dd") + "' and '" + et.ToString("yyyy-MM-dd") + " 23:59:59' group by [ScanningBy] ,convert(varchar(10),[ScanningOn],120)) as tbl1  group by [ScanningBy]");
            IList<object[]> objectses = NSession.CreateSQLQuery(sb.ToString()).List<object[]>();
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("[");//转换成多个model的形式
            for (int i = 0; i < objectses.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < strData.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(strData[j]);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(objectses[i][j]);
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            StringBuilder jsonBuilder2 = new StringBuilder();

            jsonBuilder2.Append("[{");
            for (int j = 1; j < strData.Count; j++)
            {
                int sum = 0;

                jsonBuilder2.Append("\"");
                jsonBuilder2.Append(strData[j]);
                jsonBuilder2.Append("\":\"");
                for (int i = 0; i < objectses.Count; i++)
                {
                    sum += Convert.ToInt32(objectses[i][j]);


                }
                jsonBuilder2.Append(sum.ToString());
                jsonBuilder2.Append("\",");


            }
            jsonBuilder2.Remove(jsonBuilder2.Length - 1, 1);
            jsonBuilder2.Append("}]");
            string json = "{\"total\":" + objectses.Count + ",\"rows\":" + jsonBuilder.ToString() + ",\"footer\":" + jsonBuilder2.ToString() + "}";
            return json;
        }

        [HttpPost]
        public String PeiCount(DateTime st, DateTime et)
        {

            List<string> strData = new List<string>();
            StringBuilder sb = new StringBuilder();
            strData.Add("人员");
            sb.Append("select [PeiBy] as '扫描人',");
            DateTime date = st;

            while (date <= et)
            {
                string week = GetWeek("zh", date);
                strData.Add(date.ToString("MMdd"));

                sb.Append("SUM(case  when convert(varchar(10),[CreateOn],120)='" + date.ToString("yyyy-MM-dd") + "' then  rcount else 0 end  ) as '" + date.ToString("MM.dd") + "(" + week + ")' ,");
                date = date.AddDays(1);
            }
            sb = sb.Remove(sb.Length - 1, 1);
            sb.Append("from  (select [PeiBy] ,convert(varchar(10),[CreateOn],120) [CreateOn] ,COUNT(1) as 'rcount'  from OrderPeiRecord where [CreateOn] between '" + st.ToString("yyyy-MM-dd") + "' and '" + et.ToString("yyyy-MM-dd") + " 23:59:59' group by [PeiBy] ,convert(varchar(10),[CreateOn],120)) as tbl1  group by [PeiBy]");
            IList<object[]> objectses = NSession.CreateSQLQuery(sb.ToString()).List<object[]>();
            StringBuilder jsonBuilder = new StringBuilder();

            jsonBuilder.Append("[");//转换成多个model的形式
            for (int i = 0; i < objectses.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < strData.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(strData[j]);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(objectses[i][j]);

                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            StringBuilder jsonBuilder2 = new StringBuilder();

            jsonBuilder2.Append("[{");
            for (int j = 1; j < strData.Count; j++)
            {
                int sum = 0;

                jsonBuilder2.Append("\"");
                jsonBuilder2.Append(strData[j]);
                jsonBuilder2.Append("\":\"");
                for (int i = 0; i < objectses.Count; i++)
                {
                    sum += Convert.ToInt32(objectses[i][j]);


                }
                jsonBuilder2.Append(sum.ToString());
                jsonBuilder2.Append("\",");


            }
            jsonBuilder2.Remove(jsonBuilder2.Length - 1, 1);
            jsonBuilder2.Append("}]");
            string json = "{\"total\":" + objectses.Count + ",\"rows\":" + jsonBuilder.ToString() + ",\"footer\":" + jsonBuilder2.ToString() + "}";
            return json;
        }

        [HttpPost]
        public String JiCount(DateTime st, DateTime et)
        {

            List<string> strData = new List<string>();
            StringBuilder sb = new StringBuilder();
            strData.Add("人员");
            sb.Append("select [PackBy] as '扫描人',");
            DateTime date = st;
            while (date <= et)
            {
                string week = GetWeek("zh", date);
                strData.Add(date.ToString("MMdd"));

                sb.Append("SUM(case  when convert(varchar(10),[PackOn],120)='" + date.ToString("yyyy-MM-dd") + "' then  rcount else 0 end  ) as '" + date.ToString("MM.dd") + "(" + week + ")' ,");
                date = date.AddDays(1);
            }
            sb = sb.Remove(sb.Length - 1, 1);
            sb.Append("from  (select [PackBy] ,convert(varchar(10),[PackOn],120) [PackOn] ,COUNT(1) as 'rcount'  from OrderPackRecord where [PackOn] between '" + st.ToString("yyyy-MM-dd") + "' and '" + et.ToString("yyyy-MM-dd") + " 23:59:59' group by [PackBy] ,convert(varchar(10),[PackOn],120)) as tbl1  group by [PackBy]");
            IList<object[]> objectses = NSession.CreateSQLQuery(sb.ToString()).List<object[]>();
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("[");//转换成多个model的形式
            for (int i = 0; i < objectses.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < strData.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(strData[j]);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(objectses[i][j]);
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            StringBuilder jsonBuilder2 = new StringBuilder();

            jsonBuilder2.Append("[{");
            for (int j = 1; j < strData.Count; j++)
            {
                int sum = 0;

                jsonBuilder2.Append("\"");
                jsonBuilder2.Append(strData[j]);
                jsonBuilder2.Append("\":\"");
                for (int i = 0; i < objectses.Count; i++)
                {
                    sum += Convert.ToInt32(objectses[i][j]);


                }
                jsonBuilder2.Append(sum.ToString());
                jsonBuilder2.Append("\",");


            }
            jsonBuilder2.Remove(jsonBuilder2.Length - 1, 1);
            jsonBuilder2.Append("}]");
            string json = "{\"total\":" + objectses.Count + ",\"rows\":" + jsonBuilder.ToString() + ",\"footer\":" + jsonBuilder2.ToString() + "}";
            return json;
        }


    }
}
