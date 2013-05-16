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
    public class DisputeController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create(string id="")
        {
            if (id !="")
            {
                ViewData["oid"] = id;
            }
            return View();
        }

        [HttpPost]
        public JsonResult Create(DisputeType obj)
        {
            try
            {
                object count = NSession.CreateQuery("select Count(Id) from DisputeType where OrderNo='" + obj.OrderNo + "'").UniqueResult();
                if (Convert.ToInt32(count) > 0)
                    return Json(new { IsSuccess = false, ErrorMsg = "该订单已经存在纠纷列表中" });
                obj.SolveOn=Convert.ToDateTime("2000-01-01");
                obj.DisputeOn = DateTime.Now; 
                obj.Status ="未解决";
                obj.CreateOn = DateTime.Now;
                obj.CreateBy = CurrentUser.Realname;
                NSession.SaveOrUpdate(obj);
                NSession.Flush();
                LoggerUtil.GetDisputeRecord(obj, "发生纠纷"," 创建纠纷信息", CurrentUser, NSession);
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true  });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public  DisputeType GetById(int Id)
        {
            DisputeType obj = NSession.Get<DisputeType>(Id);
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
            DisputeType obj = GetById(id);
            ViewData["OrderNo"] = obj.OrderNo;
            ViewData["SKU"] = obj.SKU;
            ViewData["LogisticsMode"] = obj.LogisticsMode;
            obj.Status = "解决中";
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(DisputeType obj)
        {
          
            try
            {
                DisputeType obj2= GetById(obj.Id);
                NSession.Clear();
                obj.SolveBy = CurrentUser.Realname;
                obj.SolveOn = DateTime.Now;
                string str = Utilities.GetObjEditString(obj2,obj);
                NSession.Update(obj);
                NSession.Flush();
                if (obj.Status == "已解决" && obj.RefundAmount!=0)
                {
                    SaveAmount(obj);
                }
                LoggerUtil.GetDisputeRecord(obj, "处理纠纷",str, CurrentUser, NSession);
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true  });
           
        }
        public void SaveAmount(DisputeType obj)
        {
            RefundAmountType amount = new RefundAmountType
            {
                DId = obj.Id,
                OrderNo = obj.OrderNo,
                OrderExNo = obj.OrderExNo,
                Platform = obj.Platform,
                Account = obj.Account,
                Amount = obj.Amount,
                CreateBy = CurrentUser.Realname,
                CreateOn = DateTime.Now,
                EmailAccount=obj.EmailAccount,
                TransactionNo=obj.TransactionNo,
                Status="未审核",
                AmountType=obj.AmountType,
                AuditOn=Convert.ToDateTime("2000-01-01")
            };
            NSession.Save(amount);
            NSession.Flush();
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
          
            try
            {
                DisputeType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true  });
        }

		public JsonResult List(int page, int rows, string sort, string order, string search)
        {
            string orderby = Utilities.OrdeerBy(sort, order);
            string where = Utilities.SqlWhere(search);
            IList<DisputeType> objList = NSession.CreateQuery("from DisputeType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<DisputeType>();

            object count = NSession.CreateQuery("select count(Id) from DisputeType " + where ).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public JsonResult SearchOrder(string id)
        {
            IList<OrderType> obj = NSession.CreateQuery("from OrderType where OrderNo=:OrderNo or OrderExNo=:OrderNo").SetString("OrderNo", id).List<OrderType>();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchOrderP(string id)
        {
            IList<OrderProductType> obj = NSession.CreateQuery("from OrderProductType where OId=:oid").SetString("oid", id).List<OrderProductType>();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRecord(int id)
        {
            IList<DisputesRecordType> list = NSession.CreateQuery("from DisputesRecordType where DId='"+id+"'").List<DisputesRecordType>();
            return Json(new {rows = list });
        }
        public JsonResult ToExcel(string search)
        {
            try
            {
                List<DisputeType> objList = NSession.CreateQuery("from DisputeType " + Utilities.SqlWhere(search))
                    .List<DisputeType>().ToList();
                Session["ExportDown"] = ExcelHelper.GetExcelXml(Utilities.FillDataTable((objList)));

            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true, ErrorMsg = "导出成功" });
        }
        public JsonResult ToDispute(int id)
        {
            try
            {
                DisputeType obj = GetById(id);
                obj.DisputesType = "纠纷";
                NSession.Update(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" },JsonRequestBehavior.AllowGet);
            }
            return Json(new { IsSuccess = true, ErrorMsg = "转换成功" },JsonRequestBehavior.AllowGet);
        }
    }
}

