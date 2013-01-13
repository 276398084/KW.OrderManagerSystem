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
    public class ModuleController : BaseController
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
        public JsonResult Create(ModuleType obj)
        {
            try
            {
                obj = Set<ModuleType>(obj);
                Nsession.SaveOrUpdate(obj);
                Nsession.Flush();
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
        public ModuleType GetById(int Id)
        {
            ModuleType obj = Nsession.Get<ModuleType>(Id);
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
            ModuleType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(ModuleType obj)
        {

            try
            {
                Nsession.Update(obj);
                Nsession.Flush();
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
                ModuleType obj = GetById(id);
                Nsession.Delete(obj);
                Nsession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }

        public JsonResult ParentList()
        {
            IList<ModuleType> objList = Nsession.CreateQuery("from ModuleType where ParentId=0").List<ModuleType>();
            objList.Insert(0, new ModuleType { FullName = "根菜单", Id = 0 });
            return Json(objList);
        }


        public JsonResult List(int page, int rows)
        {
            IList<ModuleType> objList = Nsession.CreateQuery("from ModuleType")
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows * page)
                .List<ModuleType>();
            return Json(new { total = objList.Count, rows = objList });
        }

    }
}

