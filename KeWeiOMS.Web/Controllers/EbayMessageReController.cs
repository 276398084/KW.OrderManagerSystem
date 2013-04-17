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
using System.Collections;

namespace KeWeiOMS.Web.Controllers
{
    public class EbayMessageReController : BaseController
    {           
        EbayMessageController ebay = new EbayMessageController();
        public ViewResult Index()
        {
            return View();
        }

        public ViewResult Top()
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
                obj.Account = ebaymessage.Shop;
                obj.ReplayOn = DateTime.Now;
                obj.UploadTime = Convert.ToDateTime("2000-01-01");
                NSession.Save(obj);
                NSession.Flush();

                ebaymessage.ReplayBy = obj.ReplayBy;
                ebaymessage.ReplayOn = obj.ReplayOn;
                ebaymessage.MessageStatus = "已回复";
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
                where = Utilities.Resolve(search);
                if (where.Length > 0)
                {
                    where = " where " + where;
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
        public JsonResult GetNext(int id)
        {
            IList<EbayMessageType> list = NSession.CreateQuery("from EbayMessageType where MessageStatus='未回复'  order by Id ").List<EbayMessageType>();
            foreach(var item in list)
            {
                    return Json(new { Msg =item.Id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Msg =0}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Syn()
        {
            try
            {
                EbayMessageUtil.uploadsyn(NSession);
            }
            catch (Exception ee)
            {
                return Json(new { ErrorMsg = "出错了", IsSuccess = false });
            }
            return Json(new { Msg = "同步成功" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrder(string id)
        {
            IList<OrderType> list = NSession.CreateQuery("from OrderType where BuyerName='" + id + "' order by Id desc").List<OrderType>();
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetOldMail(string id)
        {
            IList<EbayMessageType> list = NSession.CreateQuery("from EbayMessageType where SenderID='" + id + "' order by Id desc").List<EbayMessageType>();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOld(string id)
        {
            IList<EbayMessageReType> list = NSession.CreateQuery("from EbayMessageReType where EbayId='" + id + "' order by Id desc").List<EbayMessageReType>();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetail(int id)
        {
            EbayMessageReType de = GetById(id);
            return Json(de.BodyRe,JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTop(string search)
        {
            string where = "";
            if(!string.IsNullOrEmpty(search))
            {
                where = Utilities.Resolve(search);
                if (where.Length > 0)
                {
                  return TopTime(where);
                }
                return Json("");
            }
            else
            {
               DateTime time = DateTime.Now;
                 return TopToday(time);
            }
        }

        private JsonResult TopToday(DateTime time)
        {
            ArrayList obj =new ArrayList();
            ArrayList arry=new ArrayList();
            IList<EbayMessageReType> list= NSession.CreateQuery("from EbayMessageReType where ReplayOn>'" + time.Date.ToString()+ "'").List<EbayMessageReType>();
            foreach (var item in list)
            {
                int c = 0;
                if(arry.Count!=0)
                {
                    foreach (var name in arry)
                    {
                        if (item.ReplayBy==Convert.ToString(name))
                        {
                            c=1;
                        }
                    }
                }
                if (c == 0)
                {
                    arry.Add(item.ReplayBy);
                }
            }
            myCompare compare = new myCompare();
            if (arry.Count != 0)
            {
                foreach (var name in arry)
                {
                    object count = NSession.CreateQuery("select count(Id) from EbayMessageReType where ReplayBy='" + name + "' and ReplayOn>'" + time.Date.ToString() + "'").UniqueResult();
                    obj.Add(new eployee{ Count = Convert.ToInt32(count), Name = Convert.ToString(name) });
                }
            }
            if(obj.Count!=0)
            {
                obj.Sort(compare);
            }
            return Json(obj);
        }  
 
       private JsonResult TopTime(string where)
        {

            ArrayList obj =new ArrayList();
            ArrayList arry=new ArrayList();
            IList<EbayMessageReType> list= NSession.CreateQuery("from EbayMessageReType where " + where ).List<EbayMessageReType>();
            foreach (var item in list)
            {
                int c = 0;
                if(arry.Count!=0)
                {
                    foreach (var name in arry)
                    {
                        if (item.ReplayBy==Convert.ToString(name))
                        {
                            c=1;
                        }
                    }
                }
                if (c == 0)
                {
                    arry.Add(item.ReplayBy);
                }
            }
            myCompare compare = new myCompare();
            if (arry.Count != 0)
            {
                foreach (var name in arry)
                {
                    object count = NSession.CreateQuery("select count(Id) from EbayMessageReType where ReplayBy='" + name + "' and "+where).UniqueResult();
                    obj.Add(new eployee{ Count = Convert.ToInt32(count), Name = Convert.ToString(name) });
                }
            }
            if(obj.Count!=0)
            {
                obj.Sort(compare);
            }
            return Json(obj);
        }   

    }


   public struct eployee
    {
        public string Name;
        public int Count;
    }

    public class myCompare : IComparer
    {
        public int Compare(object x, object y)
        {
            return ((eployee)y).Count - ((eployee)x).Count;
        }
    }
}

