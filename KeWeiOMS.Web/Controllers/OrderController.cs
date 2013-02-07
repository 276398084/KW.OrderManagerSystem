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



        public ActionResult Import()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Import(FormCollection form)
        {
            string Platform = form["Platform"];
            string Account = form["Account"];
            string file = form["hfile"];
            DataTable dt = OrderHelper.GetDataTable(file);
            List<ResultInfo> results = new List<ResultInfo>();
            switch ((PlatformEnum)Enum.Parse(typeof(PlatformEnum), Platform))
            {
                case PlatformEnum.SMT:
                    results = OrderHelper.ImportBySMT(Account, file);
                    break;
                case PlatformEnum.Ebay:
                    break;
                case PlatformEnum.Amazon:
                    break;
                case PlatformEnum.B2C:
                    break;
                case PlatformEnum.Gmarket:
                    break;
                case PlatformEnum.LT:
                    break;
                default:
                    break;
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
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(OrderType obj)
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
                    where = " where Status" + flag + "'待处理' and" + where;
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

