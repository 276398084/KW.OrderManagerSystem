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
    public class OrderReturnRecordController : BaseController
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
        public JsonResult GetOrderByReturn(string o)
        {
            List<OrderType> orders =
                NSession.CreateQuery("from OrderType where OrderNo='" + o + "'").List<OrderType>().ToList();
            if (orders.Count > 0)
            {
                OrderType order = orders[0];
                if (order.Status != OrderStatusEnum.退货订单.ToString())
                {
                    string html = "<b>订单:{0}  <br/>平台:{5} <br/>账户:{6} <br/>下单时间:{1} <br/>发货时间:{2} <br/>发货渠道:{3} <br/>发货条码:{4}</b>";
                    return Json(new { IsSuccess = true, Result = string.Format(html, order.OrderNo, order.CreateOn, order.ScanningOn, order.LogisticMode, order.TrackCode, order.Platform, order.Account) });
                }
                return Json(new { IsSuccess = false, Result = "该订单已经退货！请不要重复扫描！" });
            }
            return Json(new { IsSuccess = false, Result = "找不到该订单" });
        }

        [HttpPost]
        public JsonResult ReturnOrder(string o, string p)
        {
            List<OrderType> orders =
                NSession.CreateQuery("from OrderType where OrderNo='" + o + "'").List<OrderType>().ToList();
            if (orders.Count > 0)
            {
                OrderType order = orders[0];
                order.Status = OrderStatusEnum.退货订单.ToString();
                order.BuyerMemo = p + "  " + order.BuyerMemo;
                NSession.Update(order);
                NSession.Flush();
                OrderReturnRecordType record = new OrderReturnRecordType();
                record.Account = order.Account;
                record.Platform = order.Platform;
                record.ReturnLogisticsMode = order.LogisticMode;
                record.OrderExNO = order.OrderExNo;
                record.OrderNo = order.OrderNo;
                record.OrderSendOn = order.ScanningOn;
                record.ReturnType = p;
                record.OldTrackCode = order.TrackCode;
                record.CreateOn = DateTime.Now;
                record.Country = order.Country;
                record.CurrencyCode = order.CurrencyCode;
                record.Amount = order.Amount;
                record.BuyerName = order.BuyerName;
                record.OrderCreateOn = order.CreateOn;
                record.OId = order.Id;


                NSession.Save(record);
                NSession.Flush();

                LoggerUtil.GetOrderRecord(order, "订单退货扫描！", "订单设置为退货", CurrentUser, NSession);

                return Json(new { IsSuccess = true, Result = "订单：" + order.OrderNo + "  已经添加到退货~！" });
            }
            return Json(new { IsSuccess = false, Result = "找不到该订单" });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public OrderReturnRecordType GetById(int Id)
        {
            OrderReturnRecordType obj = NSession.Get<OrderReturnRecordType>(Id);
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
            OrderReturnRecordType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(OrderReturnRecordType obj)
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
                OrderReturnRecordType obj = GetById(id);
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
            IList<OrderReturnRecordType> objList = NSession.CreateQuery("from OrderReturnRecordType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<OrderReturnRecordType>();

            object count = NSession.CreateQuery("select count(Id) from OrderReturnRecordType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

    }
}

