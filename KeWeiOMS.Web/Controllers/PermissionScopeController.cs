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
    public class PermissionScopeController : BaseController
    {
        protected ISession Session = NHibernateHelper.CreateSession();

        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(PermissionScopeType obj)
        {
            try
            {
                Session.SaveOrUpdate(obj);
                Session.Flush();
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
        public  PermissionScopeType GetById(int Id)
        {
            PermissionScopeType obj = Session.Get<PermissionScopeType>(Id);
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
            PermissionScopeType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(PermissionScopeType obj)
        {
           
            try
            {
                Session.Update(obj);
                Session.Flush();
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
                PermissionScopeType obj = GetById(id);
                Session.Delete(obj);
                Session.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }

        public JsonResult List(int page, int rows)
        {
            IList<PermissionScopeType> objList = Session.CreateQuery("from PermissionScopeType")
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows * page)
                .List<PermissionScopeType>();

            return Json(new { total = objList.Count, rows = objList });
        }

    }
}

