using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;
using System.Data.SqlClient;
using System.Data;

namespace KeWeiOMS.Web.Controllers
{
    public class GMarketController : BaseController
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
        public JsonResult Create(GMarketType obj)
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
        public  GMarketType GetById(int Id)
        {
            GMarketType obj = NSession.Get<GMarketType>(Id);
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
            GMarketType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(GMarketType obj)
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
                GMarketType obj = GetById(id);
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
            IList<GMarketType> objList = NSession.CreateQuery("from GMarketType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<GMarketType>();

            object count = NSession.CreateQuery("select count(Id) from GMarketType " + where ).UniqueResult();
            return Json(new { total = count, rows = objList });
        }
        public JsonResult ToExcel()
        {
            try
            {
                SqlConnection con = new SqlConnection("server=122.227.207.204;database=KeweiBackUp;uid=sa;pwd=`1q2w3e4r");
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("select * from GMarket " + Session["ToExcel"].ToString(), con);
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

    }
}

