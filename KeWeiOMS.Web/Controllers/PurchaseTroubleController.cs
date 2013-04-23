using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;
using System.Data;

namespace KeWeiOMS.Web.Controllers
{
    public class PurchaseTroubleController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create(int id)
        {
            PurchaseTroubleType obj = new PurchaseTroubleType(); 
            PurchasePlanController a = new PurchasePlanController();
            PurchasePlanType pu = a.GetById(id);
            obj.PurchaseId = pu.Id;
            obj.PurchaseCode = pu.PlanNo;
            obj.Qty = pu.Qty;
            obj.Price = pu.Price;
            obj.SKU = pu.SKU;
            obj.Supplier = pu.Suppliers;
            obj.LogisticsMode = pu.LogisticsMode;
            obj.LogisticsCode = pu.TrackCode;
            obj.ReceiveOn = pu.ReceiveOn;
            obj.Freight = pu.Freight;
            obj.BuyOn = pu.BuyOn;
            obj.Status = "未解决";
            return View(obj);
        }

        [HttpPost]
        public JsonResult Create(PurchaseTroubleType obj)
        {
            try
            {
                obj.CreateBy = CurrentUser.Realname;
                obj.CreateOn = DateTime.Now;
                obj.DealOn = Convert.ToDateTime("2000-01-01");
                NSession.Save(obj);
                NSession.Flush();
                PurchasePlanController a = new PurchasePlanController();
                PurchasePlanType pu = a.GetById(obj.PurchaseId);
                LoggerUtil.GetPurchasePlanRecord(pu,"采购问题","采购出现问题："+obj.TroubleDetail,CurrentUser,NSession);
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
        public  PurchaseTroubleType GetById(int Id)
        {
            PurchaseTroubleType obj = NSession.Get<PurchaseTroubleType>(Id);
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
            PurchaseTroubleType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(PurchaseTroubleType obj)
        {
           
            try
            {
                obj.DealBy = CurrentUser.Realname;
                obj.DealOn = DateTime.Now;
                obj.Status = "已解决";
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
                PurchaseTroubleType obj = GetById(id);
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
                    where = " where " + where;
                }
            }
            Session["ToExcel"] = where + orderby;
            IList<PurchaseTroubleType> objList = NSession.CreateQuery("from PurchaseTroubleType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<PurchaseTroubleType>();

            object count = NSession.CreateQuery("select count(Id) from PurchaseTroubleType " + where ).UniqueResult();
            return Json(new { total = count, rows = objList });
        }
        public JsonResult ToExcel()
        {
            IList<PurchaseTroubleType> signout = new List<PurchaseTroubleType>();
            try
            {
                signout = NSession.CreateQuery("from PurchaseTroubleType " + Session["ToExcel"].ToString()).List<PurchaseTroubleType>();
                DataSet ds = ConvertToDataSet<PurchaseTroubleType>(signout);
                Session["ExportDown"] = ExcelHelper.GetExcelXml(ds);
            }
            catch (Exception ee)
            {
                return Json(new { Msg = "出错了" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Msg = "导出成功" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTroubleing(int id)
        {
            object obj = NSession.CreateQuery("select count(Id) from PurchaseTroubleType where PurchaseId='"+id+"'").UniqueResult();
            if(Convert.ToInt32(obj)>0)
            return Json("是", JsonRequestBehavior.AllowGet);
            return Json("否", JsonRequestBehavior.AllowGet);
        }
        //IList转DataSet
        public static DataSet ConvertToDataSet<PurchaseTroubleType>(IList<PurchaseTroubleType> list)
        {
            if (list == null || list.Count <= 0)
            {
                return null;
            }

            DataSet ds = new DataSet();
            DataTable dt = new DataTable(typeof(PurchaseTroubleType).Name);
            DataColumn column;
            DataRow row;

            System.Reflection.PropertyInfo[] myPropertyInfo = typeof(PurchaseTroubleType).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (PurchaseTroubleType t in list)
            {
                if (t == null)
                {
                    continue;
                }

                row = dt.NewRow();

                for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
                {
                    System.Reflection.PropertyInfo pi = myPropertyInfo[i];

                    string name = pi.Name;

                    if (dt.Columns[name] == null)
                    {
                        column = new DataColumn(name, pi.PropertyType);
                        dt.Columns.Add(column);
                    }

                    row[name] = pi.GetValue(t, null);
                }

                dt.Rows.Add(row);
            }

            ds.Tables.Add(dt);

            return ds;
        }
    }
}

