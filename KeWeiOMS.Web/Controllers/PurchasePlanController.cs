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
    public class PurchasePlanController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }
        public ViewResult Import()
        {
            return View();
        }

        public ActionResult CreateByW(string Id)
        {
            ViewData["No"] = Utilities.GetPlanNo(NSession);
            IList<PurchasePlanType> obj =
                  NSession.CreateQuery("from PurchasePlanType where SKU=:sku order by Id desc").SetString("sku", Id)
                      .SetFirstResult(0)
                      .SetMaxResults(1).List<PurchasePlanType>();
            if (obj.Count > 0)
            {
                return View(obj[0]);
            }
            PurchasePlanType p = new PurchasePlanType();
            p.SKU = Id;
            return View(p);
        }

        [HttpPost]
        public ActionResult ImportPlan(string fileName)
        {
            DataTable dt = OrderHelper.GetDataTable(fileName);
            IList<WarehouseType> list = NSession.CreateQuery(" from WarehouseType").List<WarehouseType>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PurchasePlanType p = new PurchasePlanType { CreateOn = DateTime.Now, BuyOn = DateTime.Now, ReceiveOn = DateTime.Now, SendOn = DateTime.Now };
                p.PlanNo = Utilities.GetPlanNo(NSession);
                p.SKU = dt.Rows[i]["SKU"].ToString();
                p.Price = Convert.ToDouble(dt.Rows[i]["单价"].ToString());
                p.Qty = Convert.ToInt32(dt.Rows[i]["Qty"].ToString());
                p.DaoQty = 0;
                p.ProductName = "";
                p.Freight = Convert.ToDouble(dt.Rows[i]["运费"].ToString());
                p.ProductUrl = dt.Rows[i]["产品链接"].ToString();
                p.PicUrl = dt.Rows[i]["图片链接"].ToString();
                p.Suppliers = dt.Rows[i]["供应商"].ToString();
                p.LogisticsMode = dt.Rows[i]["发货方式"].ToString();
                p.TrackCode = dt.Rows[i]["追踪码"].ToString();
                p.Status = dt.Rows[i]["状态"].ToString();
                p.Memo = dt.Rows[i]["备注"].ToString();

                NSession.Save(p);
                NSession.Flush();


            }
            return Json(new { IsSuccess = true });
        }

        public ActionResult Create()
        {
            ViewData["No"] = Utilities.GetPlanNo(NSession);
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
                LoggerUtil.GetPurchasePlanRecord(obj, "新建计划", "创建采购计划", CurrentUser, NSession);
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
                string str = "";
                PurchasePlanType obj2 = GetById(obj.Id);
                str += Utilities.GetObjEditString(obj2, obj) + "<br>";
                NSession.Update(obj);
                NSession.Flush();
                LoggerUtil.GetPurchasePlanRecord(obj, "修改采购计划", str, CurrentUser, NSession);
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

        public JsonResult ExportPlan(string st, string et, string s)
        {
            string sql = @"select * from  PurchasePlan";

            if (!string.IsNullOrEmpty(s))
            {
                sql += " where Status='" + s + "' and CreateOn  between '" + st + "' and '" + et + "'";
            }
            else
            {
                sql += " where CreateOn  between '" + st + "' and '" + et + "'";
            }
            DataSet ds = new DataSet();
            IDbCommand command = NSession.Connection.CreateCommand();
            command.CommandText = sql + " order by CreateOn asc";
            SqlDataAdapter da = new SqlDataAdapter(command as SqlCommand);
            da.Fill(ds);

            DataTable dt = ds.Tables[0];

            List<string> list = new List<string>();
            string str = "";
            // 设置编码和附件格式 
            Session["ExportDown"] = ExcelHelper.GetExcelXml(ds);
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

                where = Utilities.Resolve(search);
                if (where.Length > 0)
                {
                    where = " where " + where;
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

