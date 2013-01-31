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
    public class SuppliersProductController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ViewResult GetProduct(int id)
        {
            IList<SuppliersProductType> list = NSession.CreateQuery("from SuppliersProductType where SId=:id").SetInt32("id", id).List<SuppliersProductType>();
            return View(list);
        }

        [HttpPost]
        public JsonResult Create(SuppliersProductType obj)
        {
            try
            {
                NSession.SaveOrUpdate(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }

        [HttpPost]
        public JsonResult Save(SuppliersProductType obj)
        {
            try
            {
                List<SuppliersProductType> list = Session["SupplierProducts"] as List<SuppliersProductType>;
                if (list == null)
                    list = new List<SuppliersProductType>();
                SuppliersProductType findOne = list.Find(p => p.SKU == obj.SKU);
                if (findOne != null)
                {
                    // findOne = obj;
                    list.Remove(findOne);
                    list.Add(obj);
                }
                else
                {
                    list.Add(obj);
                }
                Session["SupplierProducts"] = list;
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
        public SuppliersProductType GetById(int Id)
        {
            SuppliersProductType obj = NSession.Get<SuppliersProductType>(Id);
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
            SuppliersProductType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(SuppliersProductType obj)
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
                SuppliersProductType obj = GetById(id);
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
            IList<SuppliersProductType> objList = NSession.CreateQuery("from SuppliersProductType")
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<SuppliersProductType>();

            object count = NSession.CreateQuery("select count(Id) from SuppliersProductType ").UniqueResult();
            return Json(new { total = count, rows = objList });
        }

    }
}

