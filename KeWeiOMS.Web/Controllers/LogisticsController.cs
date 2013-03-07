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
    public class LogisticsController : BaseController
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
        public JsonResult Create(LogisticsType obj)
        {
            try
            {
                if (IsCreateOk(obj.LogisticsCode))
                    return Json(new {errorMsg="此代码已存在，请检查后再创建！"});
                obj.CreateOn = DateTime.Now;
                NSession.SaveOrUpdate(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }

        private bool IsCreateOk(string code)
        {
            object obj = NSession.CreateQuery("select count(Id) from LogisticsType where LogisticsCode='"+code+"'").UniqueResult();
            if (Convert.ToInt32(obj) > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public  LogisticsType GetById(int Id)
        {
            LogisticsType obj = NSession.Get<LogisticsType>(Id);
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
            LogisticsType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(LogisticsType obj)
        {
           
            try
            {
                if (IsOk(obj.Id, obj.LogisticsCode))
                    return Json(new {errorMsg="此代码已存在，请检查后再作修改！" });
                NSession.Update(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
           
        }

        private bool IsOk(int id, string code)
        {
            object obj = NSession.CreateQuery("select count(Id) from LogisticsType where LogisticsCode='"+code+"' and Id<>'"+id+"'").UniqueResult();
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
                LogisticsType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }

        public JsonResult List(int page, int rows,string sort,string order,string search)
        {
            string orderby = " order by Id desc ";
            string where = "";
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
            IList<LogisticsType> objList = NSession.CreateQuery("from LogisticsType" + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<LogisticsType>();

            object count = NSession.CreateQuery("select count(Id) from LogisticsType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }
        public JsonResult ShowList()
        { 
            IList<LogisticsType> objList = NSession.CreateQuery("from LogisticsType") .List<LogisticsType>();
            return Json(objList, JsonRequestBehavior.AllowGet);
        
        }

    }
}

