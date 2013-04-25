using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;

namespace KeWeiOMS.Web.Controllers
{
    public class WarehouseStockController : BaseController
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
        public JsonResult Create(WarehouseStockType obj)
        {
            try
            {
                NSession.SaveOrUpdate(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public WarehouseStockType GetById(int Id)
        {
            WarehouseStockType obj = NSession.Get<WarehouseStockType>(Id);
            if (obj == null)
            {
                throw new Exception("返回实体为空");
            }
            else
            {
                return obj;
            }
        }

        [HttpPost]
        public ActionResult Export(string o)
        {
            StringBuilder sb = new StringBuilder();
            string sql = "select  Warehouse,SKU,Qty,(select count(1) from SkuCode where SKU =WarehouseStock.SKU) as '未配货',Title from WarehouseStock";
            DataSet ds = new DataSet();
            IDbCommand command = NSession.Connection.CreateCommand();
            command.CommandText = sql;
            SqlDataAdapter da = new SqlDataAdapter(command as SqlCommand);
            da.Fill(ds);
            // 设置编码和附件格式 
            Session["ExportDown"] = ExcelHelper.GetExcelXml(ds);
            return Json(new { IsSuccess = true });
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(int id)
        {
            WarehouseStockType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(WarehouseStockType obj)
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
            return Json(new { IsSuccess = true });

        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {

            try
            {
                WarehouseStockType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
        }


        public JsonResult List(int page, int rows, string sort, string order, string search)
        {
            string orderby = Utilities.OrdeerBy(sort, order);
            string where = Utilities.SqlWhere(search);
            List<WarehouseStockType> objList = NSession.CreateQuery(" from WarehouseStockType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<WarehouseStockType>().ToList();
            string ids = "";
            foreach (var warehouseStockType in objList)
            {
                ids += warehouseStockType.SKU + ",";
            }
            if (ids.Length > 0)
            {
                ids = ids.Trim(',');
            }
            IList<object[]> objs =
                NSession.CreateQuery("select SKU,COUNT(Id) from SKUCodeType where SKU in('" + ids.Replace(",", "','") + "') and IsOut=0 group by SKU ").List<object[]>();
            foreach (var objectse in objs)
            {
                WarehouseStockType warehouse =
                objList.Find(x => x.SKU.Trim().ToUpper() == objectse[0].ToString().Trim().Trim());
                if (warehouse != null)
                    warehouse.UnPeiQty = Convert.ToInt32(objectse[1]);
            }
            object count = NSession.CreateQuery("select count(Id) from WarehouseStockType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

    }
}

