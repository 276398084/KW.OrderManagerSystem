using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;

namespace KeWeiOMS.Web.Controllers
{
    public class OrderController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
        public ActionResult UnHandle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OrderReplace(string ids, string oldField, string newField, string FieldName)
        {

            IQuery Query = NSession.CreateQuery(string.Format("update OrderType set {0}='{1}' where {0}='{2}'", FieldName, newField, oldField));
            int num = Query.ExecuteUpdate();
            return Json(new { IsSuccess = "true" });
        }



        public ActionResult Import()
        {
            return View();
        }

        public ActionResult OrderVali()
        {

            IList<OrderType> orders = NSession.CreateQuery(" from OrderType where Status='待处理'").List<OrderType>();

            foreach (var order in orders)
            {
                OrderHelper.ValiOrder(order);
            }
            return Json(new { IsSuccess = "true" });
        }

        [HttpPost]
        public ActionResult Import(FormCollection form)
        {
            string Platform = form["Platform"];
            string Account = form["Account"];
            AccountType account = NSession.Get<AccountType>(Convert.ToInt32(Account));
            string file = form["hfile"];
            DataTable dt = OrderHelper.GetDataTable(file);
            List<ResultInfo> results = new List<ResultInfo>();
            switch ((PlatformEnum)Enum.Parse(typeof(PlatformEnum), Platform))
            {
                case PlatformEnum.SMT:
                    results = OrderHelper.ImportBySMT(account, file);
                    break;
                case PlatformEnum.Ebay:
                    break;
                case PlatformEnum.Amazon:
                    results = OrderHelper.ImportByAmazon(account, file);
                    break;
                case PlatformEnum.B2C:
                    results = OrderHelper.ImportByB2C(account, file);
                    break;
                case PlatformEnum.Gmarket:
                    results = OrderHelper.ImportByGmarket(account, file);
                    break;
                case PlatformEnum.LT:
                    break;

                default:
                    break;
            }
            return Json(new { IsSuccess = "true" });
        }

        [HttpPost]
        public ActionResult Synchronous(string Platform, int Account, DateTime st, DateTime et)
        {

            AccountType account = NSession.Get<AccountType>(Convert.ToInt32(Account));


            List<ResultInfo> results = new List<ResultInfo>();
            switch ((PlatformEnum)Enum.Parse(typeof(PlatformEnum), Platform))
            {

                case PlatformEnum.Ebay:
                    results = OrderHelper.APIByEbay(account, st, et);
                    break;
                case PlatformEnum.Amazon:

                    break;
                case PlatformEnum.B2C:
                    results = OrderHelper.APIByB2C(account, st, et);
                    break;
                case PlatformEnum.Gmarket:
                case PlatformEnum.LT:
                case PlatformEnum.SMT:
                default:
                    return Json(new { ErrorMsg = "该平台没有同步功能！" });

            }
            return Json(new { IsSuccess = "true" });
        }


        [HttpPost]
        public JsonResult Create(OrderType obj)
        {
            try
            {
                NSession.Save(obj.AddressInfo);
                obj.AddressId = obj.AddressInfo.Id;
                obj.GenerateOn = obj.ScanningOn = obj.CreateOn = DateTime.Now;
                List<OrderProductType> list = Session["OrderProducts"] as List<OrderProductType>;
                NSession.Save(obj);
                foreach (var item in list)
                {
                    item.OId = obj.Id;
                    item.OrderNo = obj.OrderNo;
                    NSession.Save(item);
                }

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
        public OrderType GetById(int Id)
        {
            OrderType obj = NSession.Get<OrderType>(Id);
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
            OrderType obj = GetById(id);
            obj.AddressInfo = NSession.Get<OrderAddressType>(obj.AddressId);
            ViewData["id"] = id;
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(OrderType obj)
        {

            try
            {
                NSession.Update(obj.AddressInfo);
                obj.Country = obj.AddressInfo.Country;
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
                OrderType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }

        public JsonResult UnHandleList(int page, int rows, string sort, string order, string search)
        {
            return List(page, rows, sort, order, search, 1);
        }

        public JsonResult List(int page, int rows, string sort, string order, string search, int isUn = 0)
        {

            string where = "";
            string orderby = "";
            string flag = "<>";
            if (isUn == 1)
                flag = "=";
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
            {
                orderby = " order by " + sort + " " + order;
            }

            if (!string.IsNullOrEmpty(search))
            {
                where = Utilities.Resolve(search);
                if (where.Length > 0)
                {
                    where = " where Status" + flag + "'待处理' and " + where;
                }
            }
            if (where.Length == 0)
            {
                where = " where Status" + flag + "'待处理'";
            }
            IList<OrderType> objList = NSession.CreateQuery("from OrderType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<OrderType>();

            object count = NSession.CreateQuery("select count(Id) from OrderType " + where + orderby).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

    }
}

