using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;
using System.Data.SqlClient;

namespace KeWeiOMS.Web.Controllers
{
    public class StockInController : BaseController
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
        public JsonResult Create(StockInType obj)
        {
            try
            {

                IList<WarehouseStockType> list =
                    NSession.CreateQuery(" from WarehouseStockType where WId=:p1 and SKU=:p2").SetInt32("p1", obj.WId).
                        SetString("p2", obj.SKU).List<WarehouseStockType>();
                if (list.Count > 0)
                {
                    obj.IsAudit = 0;
                    obj.CreateBy = CurrentUser.Realname;
                    obj.CreateOn = DateTime.Now;
                    NSession.SaveOrUpdate(obj);
                    NSession.Flush();
                }
                else
                {
                    return Json(new { IsSuccess = false, ErrorMsg = "该产品没有在系统中。请先添加产品！" });
                }

            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
        }

        [HttpPost]
        public JsonResult DoAudit(int Id)
        {
            try
            {
                StockInType obj = GetById(Id);
                obj.IsAudit = 1;
                NSession.SaveOrUpdate(obj);
                NSession.Flush();
                IList<WarehouseStockType> list = NSession.CreateQuery(" from WarehouseStockType where WId=:p1 and SKU=:p2").SetInt32("p1", obj.WId).SetString("p2", obj.SKU).List<WarehouseStockType>();
                if (list.Count > 0)
                {
                    WarehouseStockType ws = list[0];
                    ws.Qty = ws.Qty + obj.Qty;
                    NSession.SaveOrUpdate(ws);
                    NSession.Flush();
                    Utilities.CreateSKUCode(obj.SKU, obj.Qty, obj.Id.ToString(), NSession);
                }

            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
        }

        public ActionResult PrintSKU(int Id)
        {
            StockInType obj = NSession.Get<StockInType>(Id);
            if (obj != null)
            {
                NSession.Flush();
                IList<SKUCodeType> list =
                     NSession.CreateQuery("from SKUCodeType where PlanNo='" + obj.Id + "'").
                       List<SKUCodeType>();
                DataTable dt = new DataTable();
                dt.Columns.Add("sku");
                dt.Columns.Add("name");
                dt.Columns.Add("num");
                dt.Columns.Add("date");
                dt.Columns.Add("people");
                dt.Columns.Add("desc");
                dt.Columns.Add("code");
                int i = 1;
                foreach (SKUCodeType skuCodeType in list)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = obj.SKU;
                    dr[1] = obj.SKU;
                    dr[2] = i + "/" + obj.Qty;
                    dr[3] = obj.CreateOn;
                    dr[4] = obj.CreateBy;
                    dr[5] = "手动入库";
                    dr[6] = skuCodeType.Code;
                    dt.Rows.Add(dr);
                    i++;
                }
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                string xml = ds.GetXml();
                Session["data"] = xml;
            }
            return Json(new { IsSuccess = true });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public StockInType GetById(int Id)
        {
            StockInType obj = NSession.Get<StockInType>(Id);
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
            StockInType obj = GetById(id);
            ViewData["sku"] = obj.SKU;
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(StockInType obj)
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
                StockInType obj = GetById(id);
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
                string date = search.Substring(0, search.IndexOf("$"));
                string key = Utilities.Resolve(search.Substring(search.IndexOf("$") + 1));
                where = GetSearch(date);
                if (!string.IsNullOrEmpty(where) && !string.IsNullOrEmpty(key))
                    where += " and " + key;
                else
                {
                    if (!string.IsNullOrEmpty(key))
                        where = " where " + key;
                }

            }
            Session["ToExcel"] = where + orderby;
            IList<StockInType> objList = NSession.CreateQuery("from StockInType" + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<StockInType>();

            object count = NSession.CreateQuery("select count(Id) from StockInType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public JsonResult ToExcel()
        {
            try
            {
                SqlConnection con = new SqlConnection("server=122.227.207.204;database=KeweiBackUp;uid=sa;pwd=`1q2w3e4r");
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("select * from StockIn " + Session["ToExcel"].ToString(), con);
                DataSet ds = new DataSet();
                da.Fill(ds, "content");
                con.Close();
                Session["ExportDown"] = ExcelHelper.GetExcelXml(ds);
            }
            catch (Exception ee)
            {
                return Json(new { Msg = "出错了" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Msg = "导出成功" }, JsonRequestBehavior.AllowGet);
        }

        public static string GetSearch(string search)
        {
            string where = "";
            string startdate = search.Substring(0, search.IndexOf("&"));
            string enddate = search.Substring(search.IndexOf("&") + 1);
            if (!string.IsNullOrEmpty(startdate) || !string.IsNullOrEmpty(enddate))
            {
                if (!string.IsNullOrEmpty(startdate))
                    where += "CreateOn >=\'" + Convert.ToDateTime(startdate) + "\'";
                if (!string.IsNullOrEmpty(enddate))
                {
                    if (where != "")
                        where += " and ";
                    where += "CreateOn <=\'" + Convert.ToDateTime(enddate) + "\'";
                }
                where = " where " + where;
            }
            return where;
        }

    }
}

