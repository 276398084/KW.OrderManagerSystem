using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;
using eBay.Service.Core.Sdk;
using eBay.Service.Call;
using eBay.Service.Core.Soap;
using System.Data.SqlClient;
using System.Data;

namespace KeWeiOMS.Web.Controllers
{
    public class EbayMessageController : BaseController
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
        public JsonResult Create(EbayMessageType obj)
        {
            try
            {
                NSession.SaveOrUpdate(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { ErrorMsg = "出错了", IsSuccess = false });
            }
            return Json(new { IsSuccess = "true" });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EbayMessageType GetById(int Id)
        {
            EbayMessageType obj = NSession.Get<EbayMessageType>(Id);
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
            EbayMessageType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(EbayMessageType obj)
        {

            try
            {
                NSession.Update(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { ErrorMsg = "出错了", IsSuccess = false });
            }
            return Json(new { IsSuccess = "true" });

        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {

            try
            {
                EbayMessageType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { ErrorMsg = "出错了", IsSuccess = false });
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
            IList<EbayMessageType> objList = NSession.CreateQuery("from EbayMessageType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<EbayMessageType>();

            object count = NSession.CreateQuery("select count(Id) from EbayMessageType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }
        public JsonResult ToExcel()
        {
            try
            {
                SqlConnection con = new SqlConnection("server=122.227.207.204;database=KeweiBackUp;uid=sa;pwd=`1q2w3e4r");
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("select * from EbayMessage " + Session["ToExcel"].ToString(), con);
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
                    where += "CreationDate >=\'" + Convert.ToDateTime(startdate) + "\'";
                if (!string.IsNullOrEmpty(enddate))
                {
                    if (where != "")
                        where += " and ";
                    where += "CreationDate <=\'" + Convert.ToDateTime(enddate) + "\'";
                }
                where = " where " + where;
            }
            return where;
        }

        public JsonResult IsRead(int id) 
        {
            EbayMessageType obj =GetById(id);
            if (obj.MessageStatus != "Unanswered")
            { 
                return Json(new { Msg =1}, JsonRequestBehavior.AllowGet);
            }
                 return Json(new { Msg =0}, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Syn()
        {
            try
            {
                EbayMessageUtil.syn();
            }
            catch (Exception ee)
            {
                return Json(new { ErrorMsg = "出错了", IsSuccess = false });
            }
            return Json(new { Msg = "同步成功" }, JsonRequestBehavior.AllowGet);
        }




    }
}

