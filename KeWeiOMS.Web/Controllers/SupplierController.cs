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
                List<SuppliersProductType> list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SuppliersProductType>>(obj.rows);  
                NSession.Save(obj);
                NSession.Flush();
                foreach (SuppliersProductType product in list1)
                {
                    product.SId = obj.Id;
                    NSession.Save(product);
                    NSession.Flush();
                }
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
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(SupplierType obj)
        {
           
            try
            {
                List<SuppliersProductType> list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SuppliersProductType>>(obj.rows);
                NSession.Update(obj);
                NSession.Flush();
                NSession.Delete("from SuppliersProductType where SId='" + obj.Id + "'");
                NSession.Flush();
                NSession.Clear();
                foreach (SuppliersProductType product in list1)
                {
                    product.SId = obj.Id;
                    NSession.Save(product);
                    NSession.Flush();
                }
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
                NSession.Delete("from SuppliersProductType where SId='" + obj.Id + "'");
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

