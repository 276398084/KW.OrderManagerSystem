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
    public class AccountFeeController : BaseController
    {
        public ViewResult Index(int id)
        {
            ViewData["Id"] = id;
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Save(AccountFeeType obj)
        {
            try
            {
                if (obj.AmountBegin >= obj.AmountEnd)
                {
                    return Json(new { IsSuccess = false, ErrorMsg = "开始金额不能大于结束金额" });
                }
                new System.Data.DataTable().Compute(obj.FeeFormula.Replace("T", "10"), "");
                NSession.SaveOrUpdate(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "公式错误" });
            }
            return Json(new { IsSuccess = true });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AccountFeeType GetById(int Id)
        {
            AccountFeeType obj = NSession.Get<AccountFeeType>(Id);
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
            AccountFeeType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(AccountFeeType obj)
        {

            try
            {
                NSession.Update(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });

        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {

            try
            {
                AccountFeeType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
        }

        public JsonResult List(string code)
        {
            IList<AccountFeeType> objList = NSession.CreateQuery("from AccountFeeType where AccountId=" + code)

                .List<AccountFeeType>();
            return Json(new { total = objList.Count, rows = objList });
        }

    }
}

