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
    public class PlacardController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }
        public ActionResult ShowIndex()
        {
            if (!string.IsNullOrEmpty(Request["id"]))
            {
                ViewData["uid"] = int.Parse(Request["id"].ToString());
            }
            return View();
        }


        public ActionResult Create()
        {
            return View();
        }

        public ActionResult IndexShow()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(PlacardType obj)
        {
            try
            {
                obj.CreateOn = DateTime.Now;
                obj.CreateBy = CurrentUser.Realname;
                NSession.Save(obj);
                NSession.Flush();
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
        public PlacardType GetById(int Id)
        {
            PlacardType obj = NSession.Get<PlacardType>(Id);
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
            PlacardType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(PlacardType obj)
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
                PlacardType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错" + ee.Message });
            }
            return Json(new { IsSuccess = "true" });
        }

        public JsonResult ListS(string search)
        {
            string where = "";
            string orderby = " order by IsTop desc,CreateOn desc ";
            if (!string.IsNullOrEmpty(search))
            {
                where = Utilities.Resolve(search);
                if (where.Length > 0)
                {
                    where = " where " + where;
                }
            }
            IList<PlacardType> objList = NSession.CreateQuery("from PlacardType " + where + orderby)
                .List<PlacardType>();

            return Json(new { total = objList.Count, rows = objList });
        }
        public JsonResult ListQ()
        {
            IList<PlacardType> objList = NSession.CreateQuery("from PlacardType " + " order by IsTop desc,CreateOn desc ")
            .SetMaxResults(8)
            .List<PlacardType>();
            return Json(objList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Detail(int id)
        {
            PlacardType obj = GetById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
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
            IList<PlacardType> objList = NSession.CreateQuery("from PlacardType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<PlacardType>();

            object count = NSession.CreateQuery("select count(Id) from PlacardType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }
    }
}

