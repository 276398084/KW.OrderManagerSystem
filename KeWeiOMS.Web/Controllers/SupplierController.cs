using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;
using System.Data.SqlClient;
using System.Data;

namespace KeWeiOMS.Web.Controllers
{
    public class SupplierController : BaseController
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
        public JsonResult Create(SupplierType obj)
        {
            try
            {
                int id=(int)NSession.Save(obj);
                 List<SuppliersProductType> list = Session["SupplierProducts"] as List<SuppliersProductType>;
                foreach (var item in list)
                {
                    item.SId =id;
                    NSession.Save(item);
                }
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
        public  SupplierType GetById(int Id)
        {
            SupplierType obj = NSession.Get<SupplierType>(Id);
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
            SupplierType obj = GetById(id);
            ViewData["SuppliewsId"]=id;
            IList<SuppliersProductType> list = NSession.CreateQuery("from SuppliersProductType where SId=:id")
                .SetInt32("id",id)
                .List<SuppliersProductType>();
            //List<SuppliersProductType> list = Session["SupplierProducts"] as List<SuppliersProductType>;
            //if (list == null)
            //    list = new List<SuppliersProductType>();
            //foreach (var item in objlist)
            //{
            //    list.Add(item);
            //}
            Session["SupplierProducts"] = list;
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult EditE(SupplierType obj)
        {
           
            try
            {
                NSession.Update(obj);
                int id =obj.Id;
                IList<SuppliersProductType> li = NSession.CreateQuery("from SuppliersProductType where SId=:id")
                .SetInt32("id",id).List<SuppliersProductType>();
                foreach (var item in li)
                {
                    NSession.Delete(item);
                }
                List<SuppliersProductType> list = Session["SupplierProducts"] as List<SuppliersProductType>;
                foreach (var item in list)
                {
                    item.SId = id;
                    NSession.Save(item);
                }
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
                SupplierType obj = GetById(id);
                NSession.Delete(obj);
                IList<SuppliersProductType> li = NSession.CreateQuery("from SuppliersProductType where SId=:id")
.SetInt32("id", id).List<SuppliersProductType>();
                foreach (var item in li)
                {
                    NSession.Delete(item);
                }
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true  });
        }

        public JsonResult List(int page, int rows, string sort, string order, string search)
        {
            string orderby = Utilities.OrdeerBy(sort, order);
            string where = Utilities.SqlWhere(search);
            IList<SupplierType> objList = NSession.CreateQuery("from SupplierType"+where+orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<SupplierType>();
			
            object count = NSession.CreateQuery("select count(Id) from SupplierType "+where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public JsonResult GetProductE(int id)
        {
            IList<SuppliersProductType> list = NSession.CreateQuery("from SuppliersProductType where SId=:id").SetInt32("id", id).List<SuppliersProductType>();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ToExcel(string search)
        {
            try
            {
                List<SuppliersProductType> objList = NSession.CreateQuery("from SuppliersProductType " + Utilities.SqlWhere(search))
                    .List<SuppliersProductType>().ToList();
                if (objList.Count == 0)
                {
                    Session["ExportDown"] = "";
                    return Json(new { IsSuccess = false, ErrorMsg = "条件记录为空" });
                }
                Session["ExportDown"] = ExcelHelper.GetExcelXml(Utilities.FillDataTable((objList)));

            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true, ErrorMsg = "导出成功" });
        }
    }
}

