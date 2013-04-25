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
    public class QuestionOrderController : BaseController
    {



        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public QuestionOrderType GetById(int Id)
        {
            QuestionOrderType obj = NSession.Get<QuestionOrderType>(Id);
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
            QuestionOrderType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(QuestionOrderType obj)
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
                QuestionOrderType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }


        public JsonResult GetOrder(string o)
        {

            List<QuestionOrderType> orders = NSession.CreateQuery("from QuestionOrderType where OrderNo='" + o + "' and Status=0").List<QuestionOrderType>().ToList();
            if (orders.Count > 0)
            {
                QuestionOrderType order = orders[0];
                if (order.Status == 0)
                {
                    string html = "";
                    html += "操作订单：" + order.OrderNo;
                    html += "<br/>操作类型：" + order.Subjest;
                    html += "<br/>备注：" + order.Content;
                    html += "<br/>请详细核对订单操作，在确认后再次扫描订单号确认。";
                    return Json(new { IsSuccess = true, Result = html });
                }
                return Json(new { IsSuccess = false, Result = "此订单已经确认过了，请不要重复操作。" });
            }
            return Json(new { IsSuccess = false, Result = "这个不是问题订单，请联系客服人员核实" });
        }

        public JsonResult ReOrder(string o)
        {
            List<QuestionOrderType> orders = NSession.CreateQuery("from QuestionOrderType where OrderNo='" + o + "' and Status=0").List<QuestionOrderType>().ToList();
            if (orders.Count > 0)
            {
                QuestionOrderType order = orders[0];
                if (order.Status == 0)
                {
                    OrderType orderType = NSession.Get<OrderType>(order.OId);
                    if (orderType != null)
                    {
                        orderType.IsAudit = 1;
                        orderType.IsError = 0;
                        orderType.CutOffMemo = "";

                        if (order.Subjest.IndexOf("重置") != -1)
                        {
                            if (order.Subjest.IndexOf("作废") == -1 && orderType.Status.IndexOf("作废") == -1)
                            {
                                orderType.Status = "已处理";
                                orderType.IsPrint = 0;
                            }
                            NSession.CreateQuery("update SKUCodeType set IsOut=1,IsSend=1,SendOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "',OrderNo='拦截报废出库' where OrderNo='" + orderType.OrderNo + "'")
                               .ExecuteUpdate();
                        }

                        NSession.Update(orderType);
                        NSession.Flush();
                        order.Status = 1;
                        NSession.Update(order);
                        NSession.Flush();
                        OrderHelper.GetOrderRecord(orderType, "取消拦截订单！", "仓库人员：" + CurrentUser.Realname + "确认", CurrentUser.Realname, NSession);
                    }
                    return Json(new { IsSuccess = true, Result = "订单：" + order.OrderNo + "取消拦截成功" });
                }
                return Json(new { IsSuccess = false, Result = "此订单已经确认过了，请不要重复操作。" });
            }
            return Json(new { IsSuccess = false, Result = "这个不是问题订单，请联系客服人员核实" });
        }

        public JsonResult List(int page, int rows, string sort, string order, string search)
        {
            string orderby = Utilities.OrdeerBy(sort, order);
            string where = Utilities.SqlWhere(search);
            IList<QuestionOrderType> objList = NSession.CreateQuery("from QuestionOrderType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<QuestionOrderType>();

            object count = NSession.CreateQuery("select count(Id) from QuestionOrderType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

    }
}

