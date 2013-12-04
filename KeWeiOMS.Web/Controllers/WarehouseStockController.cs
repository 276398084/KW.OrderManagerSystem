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

        public ActionResult StockMove()
        {
            return View();
        }

        public ActionResult StockVali()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult StockMove(string o, int w)
        {
            IList<SKUCodeType> skucodes = NSession.CreateQuery("From SKUCodeType where Code='" + o + "'").List<SKUCodeType>();
            if (skucodes.Count > 0)
            {
                if (skucodes[0].IsOut == 0)
                {
                    skucodes[0].WId = w;
                    WarehouseType warehouse = NSession.Get<WarehouseType>(w);
                    skucodes[0].WName = warehouse.WName;
                    skucodes[0].IsReceive = 1;
                    skucodes[0].ReceiveOn = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    NSession.Update(skucodes[0]);
                    NSession.Flush();

                    //到货检验后添加产品库存
                    //IList<WarehouseStockType> warehouses = NSession.CreateQuery("From WarehouseStockType where SKU='" + skucodes[0].SKU
                    //    + "'  and WId=" + skucodes[0].WId).List<WarehouseStockType>();

                    ////在仓库中添加产品库存
                    //if (warehouses.Count == 0)
                    //{
                    //    ProductType p = NSession.CreateQuery("From ProductType where SKU='" + skucodes[0].SKU
                    //    + "'").List<ProductType>()[0];
                    //    WarehouseStockType stock = new WarehouseStockType();
                    //    stock.Pic = p.SPicUrl;
                    //    stock.WId = skucodes[0].WId;
                    //    stock.Warehouse = skucodes[0].WName;
                    //    stock.PId = p.Id;
                    //    stock.SKU = p.SKU;
                    //    stock.Title = p.ProductName;
                    //    stock.Qty = 0;
                    //    stock.UpdateOn = DateTime.Now;
                    //    NSession.SaveOrUpdate(stock);
                    //    NSession.Flush();
                    //}
                    return Json(new { IsSuccess = true, Result = "条码：" + o + "  已经转移" });
                }
                else
                    return Json(new { IsSuccess = false, Result = "条码已经出库，请检查该产品！" });

            }
            return Json(new { IsSuccess = false, Result = "没有找到这个条码！" });
        }
        [HttpPost]
        public ActionResult GetSKU(string o)
        {
            IList<SKUCodeType> skucodes = NSession.CreateQuery("From SKUCodeType where Code='" + o + "'").List<SKUCodeType>();
            if (skucodes.Count > 0)
            {
                if (skucodes[0].IsOut == 0)
                    return Json(new { IsSuccess = true, SKU = skucodes[0].SKU, WId = skucodes[0].WId, WName = skucodes[0].WName });
                else
                    return Json(new { IsSuccess = false, Result = "条码已经出库，请检查该产品！" });
            }
            return Json(new { IsSuccess = false, Result = "没有找到这个条码！" });
        }

        [HttpPost]
        public ActionResult SKUVali(string o, int w)
        {
            IList<SKUCodeType> skucodes = NSession.CreateQuery("From SKUCodeType where Code='" + o + "'").List<SKUCodeType>();
            if (skucodes.Count > 0)
            {

                if (skucodes[0].WId==w)
                {

                  
                    skucodes[0].IsReceive = 0;
                    skucodes[0].ReceiveOn = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    NSession.Update(skucodes[0]);
                    NSession.Flush();

                    IList<WarehouseStockType> warehouses = NSession.CreateQuery("From WarehouseStockType where SKU='" + skucodes[0].SKU
                        + "'  and WId=" + skucodes[0].WId).List<WarehouseStockType>();

                    //在仓库中添加产品库存
                    if (warehouses.Count == 0)
                    {
                        ProductType p = NSession.CreateQuery("From ProductType where SKU='" + skucodes[0].SKU
                        + "'").List<ProductType>()[0];

                        WarehouseStockType stock = new WarehouseStockType();
                        stock.Pic = p.SPicUrl;
                        stock.WId = skucodes[0].WId;
                        stock.Warehouse = skucodes[0].WName;
                        stock.PId = p.Id;
                        stock.SKU = p.SKU;
                        stock.Title = p.ProductName;
                        stock.Qty = 0;
                        stock.UpdateOn = DateTime.Now;
                        NSession.SaveOrUpdate(stock);
                        NSession.Flush();

                    }
                    return Json(new { IsSuccess = true, Result = "条码：" + o + "  已经转移" });
                }
                else
                    return Json(new { IsSuccess = false, Result = "仓库不符" });

            }
            return Json(new { IsSuccess = false, Result = "没有找到这个条码！" });
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
        [HttpPost]
        public JsonResult EditReset(string o)
        {
            try
            {
                o = o.Replace("\r", "").Replace("\t", " ").Replace("  ", " ");
                string[] rows = o.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                string updateOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                foreach (string row in rows)
                {
                    string[] cels = row.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (cels.Length == 2)
                    {
                        try
                        {
                            NSession.CreateSQLQuery(" update WarehouseStock set Qty=" + cels[1] + " , UpdateOn='" + updateOn + "'  where SKU='" + cels[0] +
                                             "'").UniqueResult();
                            NSession.Flush();
                        }
                        catch (Exception)
                        {
                            continue;
                        }

                    }
                }
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, Message = "出错了" });
            }
            return Json(new { IsSuccess = true, Message = "成功！" });
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
            string sql = "select  Warehouse,SKU,Qty,(select count(1) from SkuCode where SKU =WarehouseStock.SKU and IsOut=0) as '未配货',Title from WarehouseStock";
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


            List<WarehouseStockType> objList = NSession.CreateSQLQuery("select *,(select COUNT(Id) from SKUCode S where S.SKU = WS.sku and IsOut=0 group by SKU) as UnPeiQty from WarehouseStock WS" + where + orderby).AddEntity(typeof(WarehouseStockType))
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
                NSession.CreateQuery("select SKU,COUNT(Id),WId from SKUCodeType where SKU in('" + ids.Replace(",", "','") + "') and IsOut=0 group by SKU,WId ").List<object[]>();
            foreach (var objectse in objs)
            {
                WarehouseStockType warehouse =
                objList.Find(x => x.SKU.Trim().ToUpper() == objectse[0].ToString().Trim().ToUpper() && x.WId == Convert.ToInt32(objectse[2]));
                if (warehouse != null)
                {
                    warehouse.UnPeiQty = Convert.ToInt32(objectse[1]);
                    if (warehouse.UnPeiQty == 0)
                    {
                        warehouse.UnPeiQty = warehouse.Qty;
                    }
                }
            }
            object count = NSession.CreateQuery("select count(Id) from WarehouseStockType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

    }
}

