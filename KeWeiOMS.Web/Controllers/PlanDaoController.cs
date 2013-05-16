using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;

namespace KeWeiOMS.Web.Controllers
{
    public class PlanDaoController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create(int Id)
        {
            PurchasePlanType plan = NSession.Get<PurchasePlanType>(Id);
            ViewData["plan"] = plan;
            return View();
        }

        [HttpPost]
        public JsonResult Create(PlanDaoType obj)
        {
            try
            {
                IList<PurchasePlanType> plan =
                    NSession.CreateQuery("from PurchasePlanType where Id=:p").SetInt32("p", Convert.ToInt32(obj.PlanNo)).
                        SetMaxResults(1)
                        .List<PurchasePlanType>();
                if (plan.Count > 0)
                {
                    if (plan[0].Status != "已收到")
                    {
                        obj.PlanId = plan[0].Id;
                        obj.PlanNo = plan[0].PlanNo;
                        obj.DaoOn = DateTime.Now;
                        obj.SendOn = DateTime.Now;
                        obj.IsAudit = 0;
                        NSession.SaveOrUpdate(obj);
                        NSession.Flush();
                        plan[0].Status = obj.Status;
                        plan[0].ReceiveOn = DateTime.Now;
                        plan[0].DaoQty += obj.RealQty;
                        NSession.SaveOrUpdate(plan[0]);
                        NSession.Flush();
                        LoggerUtil.GetPurchasePlanRecord(plan[0], "采购到货", "采购到货" + obj.Status + obj.RealQty, CurrentUser, NSession);
                    }
                }
                else
                {
                    return Json(new { ErrorMsg = "出错了", IsSuccess = false });
                }
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
        }

        public JsonResult AuditDao(int Id)
        {
            PlanDaoType obj = NSession.Get<PlanDaoType>(Id);
            if (obj != null)
            {
                if (obj.IsAudit == 0)
                {
                    //obj.DaoOn = DateTime.Now;
                    obj.IsAudit = 1;
                    obj.SKUCode = Utilities.CreateSKUCode(obj.SKU, obj.RealQty, obj.PlanNo, NSession);

                    NSession.SaveOrUpdate(obj);
                    NSession.Flush();
                    Utilities.StockIn(1, obj.SKU, obj.RealQty, obj.Price, "采购到货", CurrentUser.Realname, obj.Memo, NSession, true);
                    return Json(new { IsSuccess = true });
                }
                return Json(new { ErrorMsg = "已经审核了", IsSuccess = false });
            }
            return Json(new { ErrorMsg = "状态出错!", IsSuccess = false });
        }


        public JsonResult ExportDao(string st, string et)
        {
            string sql = @"select * from  PlanDao";
            sql += " where IsAudit=1 and DaoOn  between '" + st + "' and '" + et + "'";

            DataSet ds = new DataSet();
            IDbCommand command = NSession.Connection.CreateCommand();
            command.CommandText = sql + " order by DaoOn asc";
            SqlDataAdapter da = new SqlDataAdapter(command as SqlCommand);
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            List<string> list = new List<string>();
            string str = "";

            // 设置编码和附件格式 
            Session["ExportDown"] = ExcelHelper.GetExcelXml(ds);
            return Json(new { IsSuccess = true });
        }

        public JsonResult PrintSKU(int Id)
        {
            PlanDaoType obj = NSession.Get<PlanDaoType>(Id);
            if (obj != null)
            {
                NSession.Flush();
                IList<PurchasePlanType> plans = NSession.CreateQuery("from PurchasePlanType where PlanNo=:p and SKU=:p2").SetString("p", obj.PlanNo).SetString("p2", obj.SKU).SetMaxResults(1).List<PurchasePlanType>();
                PurchasePlanType plan = plans[0];
                IList<SKUCodeType> list =
                     NSession.CreateQuery("from SKUCodeType where SKU=:p1 and PlanNo=:p2 and Code >=:p3 order by Id").SetString("p1", obj.SKU).
                         SetString("p2", obj.PlanNo).SetInt32("p3", obj.SKUCode).SetMaxResults(obj.RealQty).List<SKUCodeType>();
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
                    dr[0] = plan.SKU;
                    dr[1] = plan.ProductName;
                    dr[2] = i + "/" + obj.RealQty;
                    dr[3] = plan.BuyOn;
                    dr[4] = plan.BuyBy;
                    dr[5] = plan.PlanNo;
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
        public PlanDaoType GetById(int Id)
        {
            PlanDaoType obj = NSession.Get<PlanDaoType>(Id);
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
            PlanDaoType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(PlanDaoType obj)
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

                PlanDaoType obj = GetById(id);
                PurchasePlanType plan =
                NSession.Get<PurchasePlanType>(obj.PlanId);
                if (plan != null)
                {
                    plan.Status = "已发货";
                    plan.DaoQty = plan.DaoQty - obj.RealQty;
                    NSession.Update(plan);
                    NSession.Flush();
                }
                NSession.Delete(obj);
                NSession.Flush();
                NSession.Delete("from SKUCodeType where PlanNo='" + obj.PlanNo + "'");
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
            string orderby = Utilities.OrdeerBy(sort, order);
            string where = Utilities.SqlWhere(search);
            IList<PlanDaoType> objList = NSession.CreateQuery("from PlanDaoType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<PlanDaoType>();

            object count = NSession.CreateQuery("select count(Id) from PlanDaoType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

    }
}

