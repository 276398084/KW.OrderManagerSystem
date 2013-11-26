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
            ViewData["bod"] = obj.Body;
            ViewData["buyer"] = obj.SenderID;
            ViewData["creation"] = obj.CreationDate;
            ViewData["email"] = obj.SenderEmail;
            ViewData["ItemId"] = obj.ItemId;
            ViewData["Title"] = obj.Title;
            ViewData["Shop"] = obj.Shop;
            return View();
        }

        public JsonResult EditProcessed(int Id)
        {
            EbayMessageType ebaymessage = ebay.GetById(Id);
            EbayMessageReType obj = new EbayMessageReType();
            obj.ItemId = ebaymessage.ItemId;
            obj.EbayId = ebaymessage.MessageId;
            obj.Account = ebaymessage.Shop;
            obj.BodyRe = "";
            obj.ItemId = ebaymessage.ItemId;
            obj.SenderID = ebaymessage.SenderID;
            obj.SenderEmail = ebaymessage.SenderEmail;
            obj.EbayId = ebaymessage.MessageId;
            obj.ReplayBy = CurrentUser.Realname;
            obj.SenderEmail = ebaymessage.SenderEmail;
            obj.SenderID = ebaymessage.SenderID;
            obj.Account = ebaymessage.Shop;
            obj.ReplayOn = DateTime.Now;
            obj.UploadTime = Convert.ToDateTime("2000-01-01");
            NSession.Save(obj);
            NSession.Flush();
            ebaymessage.ReplayBy = CurrentUser.Realname;
            ebaymessage.ReplayOn = DateTime.Now;
            ebaymessage.MessageStatus = "已回复";
            NSession.Update(ebaymessage);
            NSession.Flush();
            return Json(new { IsSuccess = "true" });
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
        public EbayMessageReType GetById(int Id)
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
            string orderby = Utilities.OrdeerBy(sort, order);
            string where = Utilities.SqlWhere(search);
            IList<EbayMessageReType> objList = NSession.CreateQuery("from EbayMessageReType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<EbayMessageReType>();

            object count = NSession.CreateQuery("select count(Id) from EbayMessageReType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public JsonResult ToExcel(string search)
        {
            try
            {
                List<EbayMessageReType> objList = NSession.CreateQuery("from EbayMessageReType " + Utilities.SqlWhere(search))
                    .List<EbayMessageReType>().ToList();
                Session["ExportDown"] = ExcelHelper.GetExcelXml(Utilities.FillDataTable((objList)));

            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true, ErrorMsg = "导出成功" });
        }

        public JsonResult GetNext(int id)
        {
            IList<EbayMessageType> list = NSession.CreateQuery("from EbayMessageType where MessageStatus='未回复' and ReplayOnlyBy='" + CurrentUser.Realname + "'  order by Id ").List<EbayMessageType>();
            foreach (var item in list)
            {
                return Json(new { Msg = item.Id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Msg = 0 }, JsonRequestBehavior.AllowGet);
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

        public JsonResult GetOrder(int id)
        {
            EbayMessageType ebayMessage = NSession.Get<EbayMessageType>(id);
            IList<OrderType> list = NSession.CreateQuery("from OrderType where BuyerName='" + ebayMessage.SenderID + "' order by Id desc").List<OrderType>();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Status = Language.GetString(list[i].Status);
                list[i].ErrorInfo = Language.GetString(list[i].ErrorInfo);
            }
            return Json(list, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GetOldMail(string id)
        {
            IList<EbayMessageType> list = NSession.CreateQuery("from EbayMessageType where SenderID='" + id + "' order by Id desc").List<EbayMessageType>();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].MessageStatus = Language.GetString(list[i].MessageStatus);
                list[i].MessageType = Language.GetString(list[i].MessageType);
            }
        }

        public JsonResult GetOld(string id)
        {
            IList<EbayMessageReType> list = NSession.CreateQuery("from EbayMessageReType where EbayId='" + id + "' order by Id desc").List<EbayMessageReType>();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetail(int id)
        {
            EbayMessageReType de = GetById(id);
            return Json(de.BodyRe, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTop(string search)
        {
            string where = "";
            if (!string.IsNullOrEmpty(search))
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
            ArrayList obj = new ArrayList();
            ArrayList arry = new ArrayList();
            IList<EbayMessageReType> list = NSession.CreateQuery("from EbayMessageReType where ReplayOn>'" + time.Date.ToString() + "'").List<EbayMessageReType>();
            foreach (var item in list)
            {
                int c = 0;
                if (arry.Count != 0)
                {
                    foreach (var name in arry)
                    {
                        if (item.ReplayBy == Convert.ToString(name))
                        {
                            c = 1;
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
                    obj.Add(new eployee { Count = Convert.ToInt32(count), Name = Convert.ToString(name) });
                }
            }
            if (obj.Count != 0)
            {
                obj.Sort(compare);
            }
            return Json(obj);
        }

        private JsonResult TopTime(string where)
        {

            ArrayList obj = new ArrayList();
            ArrayList arry = new ArrayList();
            IList<EbayMessageReType> list = NSession.CreateQuery("from EbayMessageReType where " + where).List<EbayMessageReType>();
            foreach (var item in list)
            {
                int c = 0;
                if (arry.Count != 0)
                {
                    foreach (var name in arry)
                    {
                        if (item.ReplayBy == Convert.ToString(name))
                        {
                            c = 1;
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
                    object count = NSession.CreateQuery("select count(Id) from EbayMessageReType where ReplayBy='" + name + "' and " + where).UniqueResult();
                    obj.Add(new eployee { Count = Convert.ToInt32(count), Name = Convert.ToString(name) });
                }
            }
            if (obj.Count != 0)
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

