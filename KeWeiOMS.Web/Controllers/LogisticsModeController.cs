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
    public class LogisticsModeController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create(int id)
        {
            ViewData["pid"] = id;
            return View();
        }

        [HttpPost]
        public JsonResult Create(LogisticsModeType obj)
        {
            try
            {
                if (IsCreateOk(obj.ParentID, obj.LogisticsCode))
                    return Json(new { errorMsg="此代码已存在！"});
                NSession.Save(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }

        private bool IsCreateOk(int pid, string code)
        {
            object obj = NSession.CreateQuery("select count(Id) from LogisticsModeType where ParentID='"+pid+"' and LogisticsCode='"+code+"'").UniqueResult();
            if (Convert.ToInt32(obj) > 0)
            {
                return true;
            } 
            return false;
        }

        public JsonResult GetMode(int id)
        {
            IList<LogisticsModeType> list = NSession.CreateQuery("from LogisticsModeType where ParentID=:id").SetInt32("id", id).List<LogisticsModeType>();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public LogisticsModeType GetById(int Id)
        {
            LogisticsModeType obj = NSession.Get<LogisticsModeType>(Id);
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
            LogisticsModeType obj = GetById(id);
            ViewData["checked"] = obj.Discount;
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(LogisticsModeType obj)
        {

            try
            {
                if (IsOk(obj.Id, obj.ParentID, obj.LogisticsCode))
                    return Json(new { errorMsg="此代码已存在！"});
                NSession.Update(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });

        }

        private bool IsOk(int id, int pid, string code)
        {
            object obj = NSession.CreateQuery("select count(Id) from LogisticsModeType where ParentID='" + pid + "' and LogisticsCode='" + code + "' and Id<>'"+id+"'").UniqueResult();
            if (Convert.ToInt32(obj) > 0)
            {
                return true;
            }

            return false;
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {

            try
            {
                LogisticsModeType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }

        public JsonResult List(int page, int rows)
        {
            IList<LogisticsModeType> objList = NSession.CreateQuery("from LogisticsModeType")
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<LogisticsModeType>();

            object count = NSession.CreateQuery("select count(Id) from LogisticsModeType ").UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public JsonResult ALLList()
        {
            IList<LogisticsModeType> objList = NSession.CreateQuery("from LogisticsModeType")
                .List<LogisticsModeType>();

            return Json(new { total = objList.Count, rows = objList });
        }

        public JsonResult GetLogistics()
        {
            IList<LogisticsType> list = NSession.CreateQuery("from LogisticsType")
                .List<LogisticsType>();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

    }
}

