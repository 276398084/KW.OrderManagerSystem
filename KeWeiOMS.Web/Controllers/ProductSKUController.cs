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
    public class ProductSKUController : BaseController
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
        public JsonResult Create(ProductSKUType obj)
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

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ProductSKUType GetById(int Id)
        {
            ProductSKUType obj = NSession.Get<ProductSKUType>(Id);
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
            ProductSKUType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(ProductSKUType obj)
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
                ProductSKUType obj = GetById(id);
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
            IList<ProductSKUType> objList = NSession.CreateQuery("from ProductSKUType")
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<ProductSKUType>();

            object count = NSession.CreateQuery("select count(Id) from ProductSKUType ").UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public JsonResult QList(string Q)
        {
            IList<ProductSKUType> objList = NSession.CreateQuery("from ProductSKUType where SKU like '" + Q + "%'")
                .SetMaxResults(10)
                .List<ProductSKUType>();
            return Json(new { total = objList.Count, rows = objList });
        }

    }
}

