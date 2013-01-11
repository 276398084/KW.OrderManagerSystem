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
    public class PermissionItemController : BaseController
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
        public JsonResult Create(PermissionItemType obj)
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
        public  PermissionItemType GetById(int Id)
        {
            PermissionItemType obj = Session.Get<PermissionItemType>(Id);
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
            PermissionItemType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(PermissionItemType obj)
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
                PermissionItemType obj = GetById(id);
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
            IList<PermissionItemType> objList = Session.CreateQuery("from PermissionItemType")
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows * page)
                .List<PermissionItemType>();

            return Json(new { total = objList.Count, rows = objList });
        }

    }
}

