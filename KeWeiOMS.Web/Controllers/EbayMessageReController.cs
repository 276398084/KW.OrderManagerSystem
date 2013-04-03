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
    public class EbayMessageReController : BaseController
    {           
        EbayMessageController ebay = new EbayMessageController();
        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create(int id)
        {
            ViewData["mid"] = id;
            EbayMessageType obj = ebay.GetById(id);
            ViewData["sub"] = obj.Subject;
            ViewData["bod"]=obj.Body;
            ViewData["buyer"] = obj.SenderID;
            ViewData["creation"] = obj.CreationDate;
            ViewData["email"] = obj.SenderEmail;
            return View();
        }

        [HttpPost]
        public JsonResult Create(EbayMessageReType obj)
        {
            try
            {
                EbayMessageType ebaymessage = ebay.GetById(obj.MessageId);

                obj.ItemId = ebaymessage.ItemId;
                obj.EbayId = ebaymessage.MessageId;
                obj.ReplayBy = CurrentUser.Realname;
                obj.SenderEmail = ebaymessage.SenderEmail;
                obj.SenderID = ebaymessage.SenderID;
                obj.ReplayOn = DateTime.Now;
                obj.UploadTime = Convert.ToDateTime("2000-01-01");
                NSession.Save(obj);
                NSession.Flush();

                ebaymessage.ReplayBy = obj.ReplayBy;
                ebaymessage.ReplayOn = obj.ReplayOn;
                ebaymessage.MessageStatus = "Answered";
                NSession.Update(ebaymessage);
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
        public  EbayMessageReType GetById(int Id)
        {
            EbayMessageReType obj = NSession.Get<EbayMessageReType>(Id);
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
            EbayMessageController ebay = new EbayMessageController();
            EbayMessageType obj = ebay.GetById(id);
            ViewData["messageid"] = obj.MessageId;
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(EbayMessageReType obj)
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
                EbayMessageReType obj = GetById(id);
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
            IList<EbayMessageReType> objList = NSession.CreateQuery("from EbayMessageReType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<EbayMessageReType>();

            object count = NSession.CreateQuery("select count(Id) from EbayMessageReType " + where ).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public JsonResult ToExcel()
        {
            try
            {
                SqlConnection con = new SqlConnection("server=122.227.207.204;database=KeweiBackUp;uid=sa;pwd=`1q2w3e4r");
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("select * from EbayMessageRe " + Session["ToExcel"].ToString(), con);
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
                    where += "ReplayOn >=\'" + Convert.ToDateTime(startdate) + "\'";
                if (!string.IsNullOrEmpty(enddate))
                {
                    if (where != "")
                        where += " and ";
                    where += "ReplayOn<=\'" + Convert.ToDateTime(enddate) + "\'";
                }
                where = " where " + where;
            }
            return where;
        }
        public JsonResult GetNext(int id)
        {
            int check = 0;
            IList<EbayMessageType> list = NSession.CreateQuery("from EbayMessageType order by Id ").List<EbayMessageType>();
            foreach(var item in list)
            {
                if (check == 1 && item.MessageStatus== "Unanswered")
                {
                    return Json(new { Msg =item.Id }, JsonRequestBehavior.AllowGet);
                }
                if (item.Id==id)
                {
                    check = 1;
                }

            }
            return Json(new { Msg =0}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Syn()
        {
            try
            {
                EbayMessageUtil.uploadsyn();
            }
            catch (Exception ee)
            {
                return Json(new { ErrorMsg = "出错了", IsSuccess = false });
            }
            return Json(new { Msg = "同步成功" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrder(string id)
        {
            IList<OrderType> list = NSession.CreateQuery("from OrderType where BuyerName='" + id + "'").List<OrderType>();
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetOldMail(string id)
        {
            string name = id.Substring(0,id.IndexOf("$"));
            string item = id.Substring(id.IndexOf("$")+1);
            IList<EbayMessageType> list = NSession.CreateQuery("from EbayMessageType where SenderID='" + name+ "' and Id<>'"+item+"'").List<EbayMessageType>();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOld(string id)
        {
            IList<EbayMessageReType> list = NSession.CreateQuery("from EbayMessageReType where EbayId='"+id+"'").List<EbayMessageReType>();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }

}

