using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

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
            string type = "";
            string where = "";
            string orderby = " order by Id desc ";
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
            {
                orderby = " order by " + sort + " " + order;
            }

            if (!string.IsNullOrEmpty(search))
            {
                string date = search.Substring(0,search.IndexOf("$"));
                string key = Utilities.Resolve(search.Substring(search.IndexOf("$") + 1, search.LastIndexOf("$") - search.IndexOf("$")- 1));
                type = search.Substring(search.LastIndexOf("$")+1);
                where = GetSearch(date);
                if (!string.IsNullOrEmpty(where) && !string.IsNullOrEmpty(key))
                    where += " and " + key;
                else
                {
                    if (!string.IsNullOrEmpty(key))
                        where = " where ((" + key;
                }

            }
            string account = FindAccount();
            if (account != "")
            {
                if (!string.IsNullOrEmpty(where))
                {
                    where += " and " + account;
                }
                else
                {
                    where = " where ((" + account;
                }
            }
            if (!string.IsNullOrEmpty(where))
            {
                where += " and ReplayOnlyBy is null) ";
            }
            else
            {
                where = " where ReplayOnlyBy is null ";
            }
            where += " or ReplayOnlyBy ='"+CurrentUser.Realname+"')";
            if (!string.IsNullOrEmpty(type))
            {
                string pid = type.Substring(0, type.IndexOf("~"));
                string cid = type.Substring(type.IndexOf("~") + 1);
                if (pid == "待处理消息")
                {
                    where += " and MessageStatus ='未回复'";
                    if (cid == "询问物品")
                    {
                        where += " and Subject like '%sent a question%'";

                    }
                    if (cid == "质量问题")
                    {
                        where += " and MessageStatus =''";
                    }
                    if (cid == "退货")
                    {
                        where += " and MessageStatus =''";
                    }
                }
                if (pid == "已处理消息")
                {
                    where += " and MessageStatus ='已回复'";
                    if (cid == "询问物品")
                    {
                        where += " and Subject like '%sent a question%'";

                    }
                    if (cid == "质量问题")
                    {
                        where += " and MessageStatus =''";
                    }
                    if (cid == "退货")
                    {
                        where += " and MessageStatus =''";
                    }
                }
                if (pid == "所有消息")
                {
                    if (cid == "询问物品")
                    {
                        where += " and Subject like '%sent a question%'";

                    }
                    if (cid == "质量问题")
                    {
                        where += " and MessageStatus =''";
                    }
                    if (cid == "退货")
                    {
                        where += " and MessageStatus =''";
                    }
                }
                if (pid == "未分配消息")
                {
                    where += " and MessageStatus =''";
                }
            }


            Session["ToExcel"] = where + orderby;
            IList<EbayMessageType> objList = NSession.CreateQuery("from EbayMessageType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<EbayMessageType>();

            object count = NSession.CreateQuery("select count(Id) from EbayMessageType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });

            //IList<EbayMessageType> listtype = new List<EbayMessageType>();
            //IList<EbayMessageType> listleve = new List<EbayMessageType>();
            //if(!string.IsNullOrEmpty(type))
            //{
            //    string pid = type.Substring(0,type.IndexOf("~"));
            //    string cid = type.Substring(type.IndexOf("~") + 1);
            //    if (pid != "所有消息")
            //    {
            //        if (pid == "待处理消息")
            //        {
            //            foreach(var item in objList)
            //            {
            //                if (item.MessageStatus == "未回复")
            //                {
            //                    listtype.Add(item);
            //                }
            //            }
            //          if (cid == "询问物品")
            //            {
            //                    foreach (var item in listtype)
            //                    {
            //                        if (init("sent a question", item.Subject)!="")
            //                        {
            //                            listleve.Add(item);
            //                        }
            //                    }
            //                    return Json(new { total = listleve.Count, rows = listleve });
            //            }
            //          if (cid == "质量问题")
            //          {
            //              return Json(new { total = 0, rows = "" });
            //          }
            //          if (cid == "退货")
            //          {
            //              return Json(new { total = 0, rows = "" });
            //          }

            //            return Json(new { total = listtype.Count, rows = listtype });
            //        }
            //        if (pid == "已处理消息")
            //        {
            //            foreach (var item in objList)
            //            {
            //                if (item.MessageStatus == "已回复")
            //                {
            //                    listtype.Add(item);
            //                }
            //            }
            //            if (cid == "询问物品")
            //            {
            //                foreach (var item in listtype)
            //                {
            //                    if (init("sent a question", item.Subject) != "")
            //                    {
            //                        listleve.Add(item);
            //                    }
            //                }
            //                return Json(new { total = listleve.Count, rows = listleve });
            //            }
            //            if (cid == "质量问题")
            //            {
            //                return Json(new { total = 0, rows = "" });
            //            }
            //            if (cid == "退货")
            //            {
            //                return Json(new { total = 0, rows = "" });
            //            }

            //            return Json(new { total = listtype.Count, rows = listtype });
            //        }
            //        if (pid == "未分配消息")
            //        {
            //            return Json(new { total = 0, rows = "" });
            //        }
                
            //    }
            
            //}


        }

        public string init(string key,string all)
        {
            string qty = "";
            Regex regex = new Regex(key, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant);
            if (regex.IsMatch(all))
            {
                    qty ="0";
            }
            return qty;
        }

        private string FindAccount()
        {
            string where = "";
            string name = CurrentUser.Realname;
            IList<EbayReplayType> ac = NSession.CreateQuery("from EbayReplayType where ReplayBy='"+name+"'").List<EbayReplayType>();
            foreach(var item in ac)
            { 
                if(where=="")
                {
                    where += " Shop='" + item.ReplayAccount + "' ";
                }
                else
                {
                    where += " or Shop='" + item.ReplayAccount + "' ";
                }
            }
            return where;
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
                where = " where ((" + where;
            }
            return where;
        }

        public JsonResult IsRead(int id) 
        {
            EbayMessageType obj =GetById(id);
            if (obj.MessageStatus != "未回复")
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

        public JsonResult Forward(string id) 
        {
            try
            {
                int mid = int.Parse(id.Substring(0,id.IndexOf("$")).ToString());
                string name = id.Substring(id.IndexOf("$") + 1, id.IndexOf("~")-id.IndexOf("$")-1);
                string remark = id.Substring(id.IndexOf("~")+1);
                EbayMessageType obj = GetById(mid);
                obj.ReplayOnlyBy = name;
                obj.ForwardWhy = remark;
                NSession.Update(obj);
                NSession.Flush();
                return Json(new { Msg =0}, JsonRequestBehavior.AllowGet); ;
            }
            catch (Exception e)
            { 
            
            }
            return Json(new { Msg =1}, JsonRequestBehavior.AllowGet);
        }



    }
}

