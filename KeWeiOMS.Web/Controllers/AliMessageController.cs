using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;

namespace KeWeiOMS.Web.Controllers
{
    public class AliMessageController : BaseController
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
        public JsonResult Create(AliMessageType obj)
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
            return Json(new { IsSuccess = true });
        }

        [HttpPost]
        public JsonResult EditProcessed(int m)
        {
            AliMessageType obj = NSession.Get<AliMessageType>(m);
            obj.IsReplay = true;
            obj.IsUpload = true;
            obj.ReplayBy = GetCurrentAccount().Realname;
            obj.ReplayOn = DateTime.Now;
            NSession.Update(obj);
            NSession.Flush();
            return Json(new { IsSuccess = true });
        }
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AliMessageType GetById(int Id)
        {
            AliMessageType obj = NSession.Get<AliMessageType>(Id);
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
            AliMessageType obj = GetById(id);
            List<AliMessageType> list = new List<AliMessageType>();
            if (!string.IsNullOrEmpty(obj.OrderId) && obj.MessageType == "order")
            {
                list = NSession.CreateQuery("from AliMessageType where OrderId='" + obj.OrderId + "' Order By CreateOn Desc").List<AliMessageType>().ToList();
                ViewData["messages"] = list;
            }
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(int m, string c)
        {

            try
            {
                string shop = FindAccount();
                if (shop.Length > 0)
                    shop = " and " + shop;
                AliMessageType obj = GetById(m);
                obj.ReplayContent = c;
                obj.IsReplay = true;
                obj.IsUpload = false;
                obj.ReplayBy = GetCurrentAccount().Realname;
                obj.ReplayOn = DateTime.Now;
                NSession.Update(obj);
                NSession.Flush();
                object id = NSession.CreateQuery("select min(Id) from AliMessageType where IsReplay=0 and IsOurs=0 " + shop + " ").UniqueResult();
                if (id is DBNull)
                {
                    id = 0;
                }
                return Json(new { IsSuccess = true, Next = id });
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
        }
        [HttpPost]
        public JsonResult GetOrder(string v)
        {

            IList<OrderType> list = NSession.CreateQuery("from OrderType where BuyerName='" + v + "' order by Id desc").List<OrderType>();
            return Json(list);

        }

        public JsonResult GetOldMail(string Id)
        {
            IList<EbayMessageType> list = NSession.CreateQuery("from AliMessageType where SenderID='" + Id + "' order by Id desc").List<EbayMessageType>();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                AliMessageType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }

        public JsonResult List(int page, int rows, string sort, string order, string search, string s)
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
            string shop = FindAccount();
            if (shop.Length > 0)
                shop = " and " + shop;
            if (where.Length == 0)
                where = " where 1=1 ";
            IList<AliMessageType> objList = NSession.CreateQuery("from AliMessageType " + where + shop + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<AliMessageType>();

            object count = NSession.CreateQuery("select count(Id) from AliMessageType " + where + shop).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        private string FindAccount()
        {
            string roleName = CurrentUser.RoleName;
            if (roleName == "运营经理助理" || roleName == "运营经理" || CurrentUser.Username.ToLower() == "admin") { return ""; }
            string where = " Shop in (";
            string name = CurrentUser.Realname;
            IList<EbayReplayType> ac = NSession.CreateQuery("from EbayReplayType where ReplayBy='" + name + "'").List<EbayReplayType>();

            foreach (var item in ac)
            {
                where += "'" + item.ReplayAccount + "',";

            }
            if (ac.Count == 0)
            {
                where += "''";
            }
            where = where.Trim(',') + ")";
            return where;
        }

    }
}

