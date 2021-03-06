﻿using System;
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

        public ActionResult SendDays()
        {
            return View();
        }

        public ActionResult PackScore()
        {
            return View();
        }

        public ActionResult DisputeCount()
        {
            return View();
        }

        public ActionResult RefundAmountCount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult OrderCount(DateTime st, DateTime et, string a, string p, string i)
        {
            var sqlWhere = SqlWhere(st, et, a, p);
            string sql =
                string.Format(
                    "select Account,Count(Id),Platform,Sum(Amount),Min(CurrencyCode) from OrderType {0} group by Account,Platform",
                    sqlWhere);
            if (!string.IsNullOrEmpty(i))
            {
                sql += " ,CurrencyCode";
            }
            IList<object[]> objs = NSession.CreateQuery(sql).List<object[]>();

            List<OrderCount> list = new List<OrderCount>();
            int sum = 0;
            foreach (object[] objectse in objs)
            {
                OrderCount oc = new OrderCount { Account = objectse[0].ToString(), OCount = Convert.ToInt32(objectse[1]), Platform = objectse[2].ToString(), TotalAmount = Math.Round(Convert.ToDouble(objectse[3].ToString()), 2), CurrencyCode = objectse[4].ToString() };
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
            IList<OrderType> objs = NSession.CreateQuery(string.Format("from OrderType " + sqlWhere)).List<OrderType>();
            //定义区间
            double[,] arry = { { 0, 5 }, { 5, 10 }, { 10, 20 }, { 20, 50 }, { 50, 100 }, { 100, 0 } };
            for (int i = 0; i < arry.Length / 2; i++)
            {
                int count = 0;
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
                    leve.OCount = Math.Round((Convert.ToDecimal(count) / sum) * 100, 2);
                    list.Add(leve);
                }
            }
            List<object> footers = new List<object>();
            footers.Add(new { Account = sum });
            return Json(new { rows = list, footer = footers, total = list.Count });
        }

        [HttpPost]
        public ActionResult SendDays(DateTime st, DateTime et, string a, string p)
        {
            int sum = 0;
            var sqlWhere = SqlWhere(st, et, a, p);
            List<LeveDays> list = new List<LeveDays>();
            IList<OrderType> objs = NSession.CreateQuery(string.Format("from OrderType " + sqlWhere + " and Status='已发货'")).List<OrderType>();
            //定义区间
            int[,] arry = { { 0, 1 }, { 1, 3 }, { 3, 5 }, { 5, 7 }, { 7, 9 }, { 9, 11 }, { 11, 0 } };
            for (int i = 0; i < arry.Length / 2; i++)
            {
                int count = 0;
                LeveDays leve = new LeveDays();
                foreach (var item in objs)
                {
                    if (arry[i, 1] != 0)
                    {
                        if ((item.ScanningOn - item.CreateOn).Days >= arry[i, 0] && (item.ScanningOn - item.CreateOn).Days < arry[i, 1])
                        {
                            count++;
                        }
                    }
                    else
                    {
                        if ((item.ScanningOn - item.CreateOn).Days >= arry[i, 0])
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
                    leve.OCount = Math.Round((Convert.ToDecimal(count) / sum) * 100, 2);
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
        public ActionResult SellCount(DateTime st, DateTime et, string a, string p, string s, int page, int rows)
        {
            var list = GetSellCount(st, et, a, p, s, page, rows);
            List<object> footers = new List<object>();
            var sqlWhere = SqlWhere(st, et, a, p, s);
            if (sqlWhere.Length > 3)
            {
                sqlWhere += " and SKU is not null ";
            }
            else
            {
                sqlWhere = " where SKU is not null ";
            }
            object obj = NSession.CreateSQLQuery(string.Format(
                          "select COUNT(1) from ( select SKU from OrderProducts right join Orders on OId=Orders.Id   {0} group by SKU ) as tbl",
                          sqlWhere)).UniqueResult();
            return Json(new { rows = list.OrderByDescending(f => f.Qty), total = obj });
        }
        public JsonResult GetOrder(string id)
        {
            string s = sub(id);
            id = subid(id);
            DateTime st = Convert.ToDateTime(sub(id));
            id = subid(id);
            DateTime et = Convert.ToDateTime(sub(id));
            id = subid(id);
            string p = sub(id);
            id = subid(id);
            string a = id;

            IList<OrderType> orderlist = new List<OrderType>();

            IList<OrderType> order = NSession.CreateQuery("from OrderType where Enabled=1 and Id in(select OId  from OrderProductType where SKU='" + s + "') and CreateOn >='" + st + "' and CreateOn <'" + et.AddDays(1) + "'" + pa(p, a)).List<OrderType>();
            //if (order.Count != 0)
            //{

            //    //order[0].Qty = item.Qty;
            //    orderlist = order;
            //}

            return Json(order, JsonRequestBehavior.AllowGet);
        }

        private string pa(string p, string a)
        {
            string str = "";

            if (p != "ALL")
            {
                str += " and Platform='" + p + "'";
            }
            if (a != "ALL")
            {
                str += " and Account='" + a + "'";
            }
            return str;
        }



        private string subid(string id)
        {
            string str = id.Substring(id.IndexOf("$") + 1);
            return str;
        }

        private string sub(string id)
        {
            string str = id.Substring(0, id.IndexOf("$"));
            return str;
        }

        [HttpPost]
        private List<ProductData> GetSellCount(DateTime st, DateTime et, string a, string p, string s, int page = 0, int rows = 0)
        {
            var sqlWhere = SqlWhere(st, et, a, p, s);
            if (sqlWhere.Length > 3)
            {
                sqlWhere += " and SKU is not null ";
            }
            else
            {
                sqlWhere = " where SKU is not null ";
            }
            IList<object[]> objs =
                NSession.CreateSQLQuery(
                    string.Format(
                        "select SKU,SUM(Qty) as sQty,count(Orders.Id) as Qty from OrderProducts right join Orders on OId=Orders.Id   {0} group by SKU Order By sQty desc",
                        sqlWhere))
              .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows).
            List<object[]>();
            if (page == 0)
            {
                objs =
                NSession.CreateSQLQuery(
                    string.Format(
                        "select SKU,SUM(Qty) as sQty,count(Orders.Id) as Qty from OrderProducts right join Orders on OId=Orders.Id   {0} group by SKU Order By sQty desc",
                        sqlWhere)).
            List<object[]>();
            }
            else
            {
                objs =
                NSession.CreateSQLQuery(
                    string.Format(
                        "select SKU,SUM(Qty) as sQty,count(Orders.Id) as Qty from OrderProducts right join Orders on OId=Orders.Id   {0} group by SKU Order By sQty desc",
                        sqlWhere))
              .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows).
            List<object[]>();
            }
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
        public JsonResult ExportSellCount(DateTime st, DateTime et, string a, string p, string ss)
        {
            var list = GetSellCount(st, et, a, p, ss);
            //设置编码和附件格式 
            Session["ExportDown"] = ExcelHelper.GetExcelXml(Utilities.FillDataTable(list));
            return Json(new { IsSuccess = true });
        }

        public JsonResult ExportSore(DateTime st, DateTime et)
        {
            var list = SoreList(st, et);
            // 设置编码和附件格式 
            Session["ExportDown"] = ExcelHelper.GetExcelXml(Utilities.FillDataTable(list));
            return Json(new { IsSuccess = true });
        }

        [HttpPost]
        public ContentResult GetSaleChart(string s, DateTime st, string p)
        {


            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<chart caption='{0} 的销售记录({1}-{2})' subCaption='销量'  showLabels='1' showColumnShadow='1' animation='1' showAlternateHGridColor='1' AlternateHGridColor='ff5904' divLineColor='ff5904' divLineAlpha='20' alternateHGridAlpha='5' canvasBorderColor='666666' baseFontColor='666666'  lineAlpha='85' showValues='1' rotateValues='0' valuePosition='auto' canvaspadding='8' lineThickness='3'>");
            //sb.AppendLine("<chart palette='2'  caption='{0} 的销售记录({1}-{2})' subCaption='销量' caption='Sales Comparison' showValues='0' numVDivLines='10' drawAnchors='0' numberPrefix='$' divLineAlpha='30' alternateHGridAlpha='20'  setAdaptiveYMin='1'  canvaspadding='10' labelDisplay='ROTATE'>");

            DateTime et = st.AddDays(15);
            List<string> strData = new List<string>();
            StringBuilder sb2 = new StringBuilder();
            DateTime date = st;
            sb2.Append("select {0} as 'Y',");
            if (et > DateTime.Now)
            {
                et = DateTime.Now;
            }
            while (date <= et)
            {
                string week = GetWeek("zh", date);
                strData.Add(date.ToString("MM.dd") + "(" + week + ")");

                sb2.Append(" SUM(case  when convert(varchar(10),[CreateOn],120)='" + date.ToString("yyyy-MM-dd") + "' then  rcount else 0 end  ) as '" + date.ToString("MM.dd") + "(" + week + ")' ,");
                date = date.AddDays(1);
            }
            sb2 = sb2.Remove(sb2.Length - 1, 1);
            sb2.Append("  from  ( select {0} ,convert(varchar(10),[CreateOn],120) [CreateOn] ,sum(op.Qty) as 'rcount'  from Orders O left join OrderProducts OP on O.Id=OP.OId where [CreateOn] between '" + st.ToString("yyyy-MM-dd") + "' and '" + et.ToString("yyyy-MM-dd") + " 23:59:59' and SKU='{1}' {2}  group by {0} ,convert(varchar(10),[CreateOn],120)) as tbl1  group by {0} ");

            string sql = "";
            if (p != "ALL")
            {
                sql = string.Format(sb2.ToString(), "Account", s, " and Platform='" + p + "'");
            }
            else
            {
                sql = string.Format(sb2.ToString(), "Platform", s, "");
            }
            IList<object[]> objectes = NSession.CreateSQLQuery(sql).List<object[]>();
            sb.AppendLine("<categories>");
            foreach (string foo in strData)
            {
                sb.Append("<category label='" + foo + "'/>");
            }
            //sb = sb.Remove(sb.Length - 1, 1);
            sb.AppendLine("</categories>");
            foreach (object[] objs in objectes)
            {
                sb.Append("<dataset lineThickness='3' seriesName=\"" + objs[0] + "\">");

                for (int i = 1; i < objs.Length; i++)
                {
                    sb.Append("<set value='" + objs[i] + "' />");
                }


                sb.AppendLine("</dataset>");
            }

            foreach (object[] objs in objectes)
            {
                sb.Append("<dataset lineThickness='3' seriesName='ALL'>");

                for (int i = 1; i < objs.Length; i++)
                {
                    int n = 0;
                    for (int j = 0; j < objectes.Count; j++)
                    {
                        n += Convert.ToInt32(objectes[j][i]);
                    }
                    sb.Append("<set value='" + n + "' />");
                }
                sb.AppendLine("</dataset>");
                break;

            }
            sb.AppendLine("</chart>");
            return Content(string.Format(sb.ToString(), s, st.ToShortDateString(), et.ToShortDateString()));

        }
        #endregion

        #region 缺货统计
        public ActionResult QueCount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult QueData(string order, string sort, string s, string p, string a, string c)
        {
            string orderby = "";
            if (!string.IsNullOrEmpty(order) && !string.IsNullOrEmpty(sort))
            {
                orderby = " order by " + sort + " " + order;
            }

            s = SqlWhere(a, p, s, "");
            s = s.Replace("SKU", "OP.SKU");
            if (s.Length > 0)
            {
                s = " and " + s;
            }
            string sqlss = "";
            if (!string.IsNullOrEmpty(c))
            {
                string cs = "";
                IList<object> objectes = NSession.CreateSQLQuery(@"with a as(
select * from ProductCategory where ID=" + c + @"
union all
select x.* from ProductCategory x,a
where x.ParentId=a.Id)
select Name from a").List<object>();
                foreach (object item in objectes)
                {
                    cs += "'" + item + "',";
                }
                if (cs.Length > 0)
                {
                    cs = cs.Trim(',');
                }
                sqlss = " where OP.SKU in (select SKU from Products where Category in (" + cs + "))";

            }
            IList<object[]> objs = NSession.CreateSQLQuery(string.Format(@"select * from (
select *,(Qty-BuyQty-UnPeiQty) as NeedQty,(SQty-BuyQty-UnPeiQty) as SNeedQty from( select * ,(select COUNT(Id) from SKUCode where IsOut=0 and SKUCode.SKU=tbl1.SKU) as UnPeiQty,
isnull((select SUM(Qty) from   Orders O left join OrderProducts OP On O.Id=OP.OId where O.IsOutOfStock=1 and O.Enabled=1 and O.Status in ('已处理','待拣货') and OP.IsQue=1 and OP.SKU=tbl1.SKU),0) as 'SQty'
from (
select OP.SKU,SUM(OP.Qty) as Qty,MIN(O.CreateOn) as MinDate,P.Standard,
(select isnull(SUM(Qty-DaoQty),0) from PurchasePlan 
where Status<>'异常' and Status<>'已收到' and  SKU=OP.SKU ) as BuyQty
from Orders O left join OrderProducts OP On O.Id=OP.OId 
left join Products P on OP.SKU=P.SKU 
where  O.Enabled=1 and O.IsStop=0 and O.Status in ('已处理','待拣货') and OP.SKU is not null {0} group by OP.SKU,P.Standard)
 as tbl1 ) as tbl2 ) as tbl3 where SQty>0 or (Qty-UnPeiQty)>0 {1} {2}", s, sqlss, orderby)).List<object[]>();
            List<QueCount> list = new List<QueCount>();
            foreach (object[] objectse in objs)
            {
                QueCount oc = new QueCount { SKU = objectse[0].ToString(), Qty = Utilities.ToInt(objectse[1]), MinDate = Convert.ToDateTime(objectse[2]), BuyQty = Utilities.ToInt(objectse[4]), NeedQty = Utilities.ToInt(objectse[7]), SNeedQty = Utilities.ToInt(objectse[8]), UnPeiQty = Utilities.ToInt(objectse[5]), SQty = Utilities.ToInt(objectse[6]) };
                if (objectse[3] is DBNull || objectse[3] == null)
                {
                }
                else
                {
                    oc.Standard = objectse[3].ToString();
                }
                if (!(objectse[2] is DBNull) && !(objectse[2] == null))
                {
                    oc.Field1 = objectse[2].ToString();
                }
                //oc.NeedQty -= oc.UnPeiQty;
                if (oc.NeedQty <= 0)
                {
                    oc.NeedQty = 0;
                }
                if (oc.SNeedQty <= 0)
                {
                    oc.SNeedQty = 0;
                }
                list.Add(oc);
            }

            return Json(list);
        }

        public JsonResult ExportQue(string s, string p, string a, string c)
        {
            s = SqlWhere(a, p, s, "");
            s = s.Replace("SKU", "OP.SKU");
            if (s.Length > 0)
            {
                s = " and " + s;
            }
            string sqlss = "";
            if (!string.IsNullOrEmpty(c))
            {
                string cs = "";
                IList<object> objectes = NSession.CreateSQLQuery(@"with a as(
select * from ProductCategory where ID=" + c + @"
union all
select x.* from ProductCategory x,a
where x.ParentId=a.Id)
select Name from a").List<object>();
                foreach (object item in objectes)
                {
                    cs += "'" + item + "',";
                }
                if (cs.Length > 0)
                {
                    cs = cs.Trim(',');
                }
                sqlss = " where OP.SKU in (select SKU from Products where Category in (" + cs + "))";

            }
            string sql = string.Format(@"select * from (
select *,(Qty-BuyQty-UnPeiQty) as NeedQty,(SQty-BuyQty-UnPeiQty) as SNeedQty from( select * ,(select COUNT(Id) from SKUCode where IsOut=0 and SKUCode.SKU=tbl1.SKU) as UnPeiQty,
isnull((select SUM(Qty) from   Orders O left join OrderProducts OP On O.Id=OP.OId where O.IsOutOfStock=1 and O.Enabled=1 and O.Status in ('已处理','待拣货') and OP.IsQue=1 and OP.SKU=tbl1.SKU),0) as 'SQty'
from (
select OP.SKU,SUM(OP.Qty) as Qty,MIN(O.CreateOn) as MinDate,P.Standard,
(select isnull(SUM(Qty-DaoQty),0) from PurchasePlan 
where Status<>'异常' and Status<>'已收到' and  SKU=OP.SKU ) as BuyQty
from Orders O left join OrderProducts OP On O.Id=OP.OId 
left join Products P on OP.SKU=P.SKU 
where  O.Enabled=1 and O.IsStop=0 and O.Status in ('已处理','待拣货') and OP.SKU is not null {0} group by OP.SKU,P.Standard)
 as tbl1 ) as tbl2 ) as tbl3 where SQty>0 or (Qty-UnPeiQty)>0 {1} ", s, sqlss);

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

        private string SqlWhere(DateTime st, DateTime et, string a, string p, string ss = "")
        {
            string sqlWhere = " where Status<>'待处理' and MId=0 and Enabled=1 and CreateOn between '" + st.ToString("yyyy/MM/dd 00:00:00") + "' and '" + et.AddDays(1).ToString("yyyy/MM/dd 00:00:00") + "' and";
            sqlWhere = SqlWhere(a, p, ss, sqlWhere);
            return sqlWhere;
        }

        private static string SqlWhere(string a, string p, string ss, string sqlWhere)
        {

            if (!string.IsNullOrEmpty(p) && p != "ALL")
            {
                sqlWhere += " Platform='" + p + "' and";
            }
            if (!string.IsNullOrEmpty(a) && a != "ALL")
            {
                sqlWhere += " Account='" + a + "' and";
            }
            if (!string.IsNullOrEmpty(ss) && ss != "ALL")
            {
                sqlWhere += " SKU like '%" + ss + "%' and";
            }
            if (sqlWhere.Length > 4)
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
        public JsonResult GetScore(DateTime st, DateTime et)
        {
            IList<Sores> sores = SoreList(st, et);
            return Json(new { rows = sores.OrderByDescending(p => p.PackSores) });
        }
        public IList<Sores> SoreList(DateTime st, DateTime et)
        {
            IList<Sores> sores = new List<Sores>();
            IList<object[]> objectses = NSession.CreateSQLQuery("select COUNT(Id) as Qcount, PackBy as PackBy,SUM(PackCoefficient) as PackCoefficient from OrderPackRecord where [PackOn] between '" + st.ToString("yyyy-MM-dd") + "' and '" + et.ToString("yyyy-MM-dd") + " 23:59:59' group by [PackBy]").List<object[]>();
            foreach (var item in objectses)
            {
                object soreadd = NSession.CreateQuery("select SUM(Sore) from SoresAddType where Worker='" + item[1].ToString() + "' and WorkDate between '" + st.ToString("yyyy-MM-dd") + "' and '" + et.ToString("yyyy-MM-dd") + " 23:59:59'").UniqueResult();
                decimal avg = Convert.ToDecimal((Convert.ToDouble(item[2]).ToString("f1"))) / Convert.ToDecimal(item[0]);
                decimal packsores = Convert.ToDecimal((Convert.ToDouble(item[2]).ToString("f1")));
                decimal sore = Convert.ToDecimal(soreadd);
                decimal totalsore = sore + packsores;
                sores.Add(new Sores { PackBy = item[1].ToString(), PackSores = packsores, Qcount = Convert.ToDecimal(item[0]), Avg = Convert.ToDecimal(avg.ToString("f1")), Sore = sore, TotalSores = totalsore });
            }
            return sores;
        }
        [HttpPost]
        public ActionResult DisputeCount(DateTime st, DateTime et, string p, string a)
        {
            IList<DisputeCount> sores = new List<DisputeCount>();
            string where = Where(st, et, p, a);
            IList<object[]> objectses = NSession.CreateQuery("select Count(Id),DisputeCategory from DisputeType " + where + " group by DisputeCategory").List<object[]>();
            foreach (var item in objectses)
            {

                string dtype = item[1].ToString();
                decimal count = Convert.ToDecimal(item[0]);
                sores.Add(new DisputeCount { DType = dtype, Count = count });
            }
            return Json(new { rows = sores.OrderByDescending(x => x.Count) });
        }

        [HttpPost]
        public ActionResult DisputeTypeCount(DateTime st, DateTime et, string p, string a)
        {
            IList<DisputeCount> sores = new List<DisputeCount>();
            string where = Where(st, et, p, a);
            IList<object[]> objectses = NSession.CreateQuery("select count(Id),Solution  from DisputeType " + where + " group by Solution").List<object[]>();
            foreach (var item in objectses)
            {
                string dtype = "未开始处理";
                if (item[1] != null)
                {
                    dtype = item[1].ToString();
                }
                decimal count = Convert.ToDecimal(item[0]);
                sores.Add(new DisputeCount { DType = dtype, Count = count });
            }
            return Json(new { rows = sores.OrderByDescending(x => x.Count) });
        }
        public ActionResult AmountCount(DateTime st, DateTime et, string p, string a)
        {
            IList<AmountCount> sores = new List<AmountCount>();
            string where = Where(st, et, p, a);
            IList<object[]> obj = NSession.CreateQuery("select Account,Count(Account),sum(Amount) from RefundAmountType " + where + " group by Account").List<object[]>();
            foreach (var item in obj)
            {
                string account = item[0].ToString();
                int count = Convert.ToInt32(item[1]);
                decimal qcount = Convert.ToDecimal(item[2]);
                sores.Add(new AmountCount { Account = account, Count = count, Qcount = qcount });
            }
            return Json(new { rows = sores.OrderByDescending(x => x.Count) });
        }

        public string Where(DateTime st, DateTime et, string p, string a)
        {
            string where = "where CreateOn between '" + st.ToString("yyyy-MM-dd") + "' and '" + et.ToString("yyyy-MM-dd") + " 23:59:59'";
            if (p != "ALL")
            {
                where += " and Platform='" + p + "'";
            }
            if (a != "ALL")
            {
                where += " and Account='" + a + "'";
            }
            return where;
        }



    }
}
