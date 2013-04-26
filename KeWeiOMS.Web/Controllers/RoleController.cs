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
    public class RoleController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult GetCompetence(string id)
        {
            ViewData["uid"] = id;
            return View();
        }

        [HttpPost]
        public JsonResult Create(RoleType obj)
        {
            try
            {
                obj.CreateBy = CurrentUser.Realname;
                obj.CreateOn = DateTime.Now;
                NSession.SaveOrUpdate(obj);
                NSession.Flush();
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
        public RoleType GetById(int Id)
        {
            RoleType obj = NSession.Get<RoleType>(Id);
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
            RoleType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(RoleType obj)
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
            return Json(new { IsSuccess = true  });

        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {

            try
            {
                RoleType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true  });
        }

        public JsonResult RootList()
        {
            IList<RoleType> objList = NSession.CreateQuery("from RoleType")
                .List<RoleType>();
            return Json(objList);
        }



        public JsonResult List(int page, int rows, string sort, string order, string search)
        {
            string orderby = Utilities.OrdeerBy(sort, order);
            string where = Utilities.SqlWhere(search);
            IList<RoleType> objList = NSession.CreateQuery("from RoleType" + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows * page)
                .List<RoleType>();
            object count = NSession.CreateQuery("select count(Id) from RoleType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }


        public ActionResult SetMP(string m, string p, int uid)
        {
            string[] ms = m.Split(',');
            string[] ps = p.Split(',');

            PermissionScopeType sc = null;
            NSession.CreateQuery("delete from PermissionScopeType where ResourceCategory='" +
                                  ResourceCategoryEnum.Role.ToString() + "' and ResourceId=" + uid).ExecuteUpdate();
            foreach (var item in ms)
            {
                sc = new PermissionScopeType();
                sc.ResourceCategory = ResourceCategoryEnum.Role.ToString();
                sc.ResourceId = uid;
                sc.TargetCategory = TargetCategoryEnum.Module.ToString();
                sc.TargetId = Convert.ToInt32(item);
                NSession.Save(sc);
                NSession.Flush();
            }

            foreach (var item in ms)
            {
                sc = new PermissionScopeType();
                sc.ResourceCategory = ResourceCategoryEnum.Role.ToString();
                sc.ResourceId = uid;
                sc.TargetCategory = TargetCategoryEnum.PermissionItem.ToString();
                sc.TargetId = Convert.ToInt32(item);
                NSession.Save(sc);
                NSession.Flush();
            }
            return Json(new { IsSuccess = true  });
        }

    }
}

