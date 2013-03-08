using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace KeWeiOMS.Web.Controllers
{
    public class StatisticsController : BaseController
    {
        public ActionResult OrderCount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OrderCount(DateTime dt)
        {
            IList<object[]> objs = NSession.CreateQuery("select Account,Count(Id),Platform from OrderType   where CreateOn between '" + dt.ToString("yyyy/MM/dd 00:00:00") + "' and '" + dt.AddDays(1).ToString("yyyy/MM/dd 00:00:00") + "' group by Account,Platform").List<object[]>();

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
            IList<object[]> objs = NSession.CreateSQLQuery(string.Format(@"select * ,(Qty-BuyQty) as NeedQty from (
select SKU,SUM(Qty) as Qty,MIN(CreateOn) as MinDate,isnull(Standard,0) as Standard,(select isnull(SUM(Qty),0) from PurchasePlan where SKU=OP.SKU and BuyOn>MIN(O.CreateOn)) as BuyQty from Orders O left join OrderProducts OP On O.Id=OP.OId where O.IsOutOfStock=1 and OP.IsQue=1 {0} group by SKU,Standard
) as tbl {1}", s, orderby)).List<object[]>();

            List<QueCount> list = new List<QueCount>();
            foreach (object[] objectse in objs)
            {
                QueCount oc = new QueCount { SKU = objectse[0].ToString(), Qty = Utilities.ToInt(objectse[1]), MinDate = Convert.ToDateTime(objectse[2]), BuyQty = Utilities.ToInt(objectse[4]) };
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
select SKU,SUM(Qty) as Qty,MIN(CreateOn) as MinDate,isnull(Standard,0) as Standard,(select isnull(SUM(Qty),0) from PurchasePlan where SKU=OP.SKU and BuyOn>MIN(O.CreateOn)) as BuyQty from Orders O left join OrderProducts OP On O.Id=OP.OId where O.IsOutOfStock=1 and OP.IsQue=1 {0} group by SKU,Standard
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

    }
}
