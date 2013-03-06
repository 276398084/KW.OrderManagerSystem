using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;

namespace KeWeiOMS.Web.Controllers
{
    public class ProductController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult SKUCodeIndex()
        {
            return View();
        }

        public ActionResult ImportPic()
        {
            return View();
        }

        public ActionResult ImportProduct()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ImportProduct(string fileName)
        {
            DataTable dt = OrderHelper.GetDataTable(fileName);
            IList<WarehouseType> list = NSession.CreateQuery(" from WarehouseType").List<WarehouseType>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ProductType p = new ProductType { CreateOn = DateTime.Now };
                p.SKU = dt.Rows[i]["SKU"].ToString();
                p.Status = ProductStatusEnum.销售中.ToString();
                p.ProductName = dt.Rows[i]["名称"].ToString();
                p.Category = dt.Rows[i]["分类"].ToString();
                p.Standard = dt.Rows[i]["规格"].ToString();
                p.Price = Convert.ToDouble(dt.Rows[i]["价格"]);
                p.Weight = Convert.ToInt16(dt.Rows[i]["重量"]);
                p.Long = Convert.ToInt16(dt.Rows[i]["长"]);
                p.Wide = Convert.ToInt16(dt.Rows[i]["宽"]);
                p.High = Convert.ToInt16(dt.Rows[i]["高"]);
                p.Location = dt.Rows[i]["库位"].ToString();
                p.OldSKU = dt.Rows[i]["旧SKU"].ToString();
                p.HasBattery = Convert.ToInt32(dt.Rows[i]["电池"].ToString());
                p.IsElectronic = Convert.ToInt32(dt.Rows[i]["电子"].ToString());
                p.IsLiquid = Convert.ToInt32(dt.Rows[i]["液体"].ToString());
                p.PackCoefficient = Convert.ToInt32(dt.Rows[i]["包装系数"].ToString());
                p.Manager = dt.Rows[i]["管理人"].ToString();

                NSession.SaveOrUpdate(p);
                //
                //在仓库中添加产品库存
                //
                foreach (var item in list)
                {
                    WarehouseStockType stock = new WarehouseStockType();
                    stock.Pic = p.SPicUrl;
                    stock.WId = item.Id;
                    stock.Warehouse = item.WName;
                    stock.PId = p.Id;
                    stock.SKU = p.SKU;
                    stock.Title = p.ProductName;
                    stock.Qty = 0;
                    stock.UpdateOn = DateTime.Now;
                    NSession.SaveOrUpdate(stock);
                    NSession.Flush();
                }

            }
            return View();
        }

        [HttpPost]
        public JsonResult Create(ProductType obj)
        {
            try
            {
                string filePath = Server.MapPath("~");
                obj.CreateOn = DateTime.Now;
                string pic = obj.PicUrl;
                obj.Status = ProductStatusEnum.销售中.ToString();
                obj.PicUrl = Utilities.BPicPath + obj.SKU + ".jpg";
                obj.SPicUrl = Utilities.SPicPath + obj.SKU + ".png";
                Utilities.DrawImageRectRect(pic, filePath + obj.PicUrl, 310, 310);
                Utilities.DrawImageRectRect(pic, filePath + obj.SPicUrl, 64, 64);
                NSession.SaveOrUpdate(obj);
                NSession.Flush();
                List<ProductComposeType> list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductComposeType>>(obj.rows);
                foreach (ProductComposeType productCompose in list1)
                {
                    productCompose.SKU = obj.SKU;
                    productCompose.PId = obj.Id;
                    NSession.Save(productCompose);
                    NSession.Flush();
                }

                IList<WarehouseType> list = NSession.CreateQuery(" from WarehouseType").List<WarehouseType>();


                //
                //在仓库中添加产品库存
                //
                foreach (var item in list)
                {
                    WarehouseStockType stock = new WarehouseStockType();
                    stock.Pic = obj.SPicUrl;
                    stock.WId = item.Id;
                    stock.Warehouse = item.WName;
                    stock.PId = obj.Id;
                    stock.SKU = obj.SKU;
                    stock.Title = obj.ProductName;
                    stock.Qty = 0;
                    stock.UpdateOn = DateTime.Now;
                    NSession.SaveOrUpdate(stock);
                    NSession.Flush();
                }
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
        public ProductType GetById(int Id)
        {
            ProductType obj = NSession.Get<ProductType>(Id);
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
            ProductType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(ProductType obj)
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
                ProductType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }

        [HttpPost, ActionName("DeleteSKU")]
        public JsonResult DeleteConfirmed2(int id)
        {
            try
            {
                SKUCodeType obj = NSession.Get<SKUCodeType>(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
        }
        public JsonResult ListQ(string q)
        {
            IList<ProductType> objList = NSession.CreateQuery("from ProductType where SKU like '%" + q + "%'")
                .SetFirstResult(0)
                .SetMaxResults(20)
                .List<ProductType>();

            return Json(new { total = objList.Count, rows = objList });
        }


        public ActionResult SKUScan()
        {
            return View();
        }

        public JsonResult SKUCodeList(int page, int rows, string sort, string order, string search)
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
            IList<SKUCodeType> objList = NSession.CreateQuery("from SKUCodeType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<SKUCodeType>();
            object count = NSession.CreateQuery("select count(Id) from SKUCodeType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
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
            IList<ProductType> objList = NSession.CreateQuery("from ProductType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<ProductType>();
            object count = NSession.CreateQuery("select count(Id) from ProductType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public JsonResult ZuList(int page, int rows, string sort, string order, string search)
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
            IList<ProductComposeType> objList = NSession.CreateQuery("from ProductComposeType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<ProductComposeType>();
            object count = NSession.CreateQuery("select count(Id) from ProductComposeType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public JsonResult HasExist(string sku)
        {
            object count = NSession.CreateQuery("select count(Id) from ProductType where SKU='" + sku + "'").UniqueResult();
            if (Convert.ToInt32(count) > 0)
            {
                return Json(new { IsSuccess = "false" });
            }
            else
            {
                return Json(new { IsSuccess = "true" });
            }
        }

        public ActionResult SetSKUCode(int code, string sku)
        {
            object count = NSession.CreateQuery("select count(Id) from ProductType where SKU='" + sku + "'").UniqueResult();

            sku = sku.Trim();
            SqlConnection conn = new SqlConnection("server=122.227.207.204;database=Feidu;uid=sa;pwd=`1q2w3e4r");
            string sql = "select top 1 SKU from SkuCode where Code={0}or Code={1} ";
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand(string.Format(sql, code, (code + 1000000)), conn);
            object objSKU = sqlCommand.ExecuteScalar();
            conn.Close();
            if (objSKU != null)
            {
                if (objSKU.ToString().Trim().ToUpper() != sku.Trim().ToUpper())
                {
                    return Json(new { IsSuccess = false, Result = "这个条码对应是的" + objSKU + ",不是现在的：" + sku + "！" });
                }
            }

            if (Convert.ToInt32(count) > 0)
            {
                object count1 =
                    NSession.CreateQuery("select count(Id) from SKUCodeType where Code=:p").SetInt32("p", code).
                        UniqueResult();
                if (Convert.ToInt32(count1) == 0)
                {
                    SKUCodeType skuCode = new SKUCodeType { Code = code, SKU = sku, IsNew = 0, IsOut = 0 };
                    NSession.Save(skuCode);
                    NSession.Flush();
                    return Json(new { IsSuccess = true, Result = "添加成功！" });
                }
                else
                {
                    return Json(new { IsSuccess = false, Result = "这个条码已经添加！" });
                }
            }
            else
            {
                return Json(new { IsSuccess = false, Result = "没有这个产品！" });
            }

        }
        public ActionResult GetSKUByCode(string code)
        {
            IList<SKUCodeType> list =
                 NSession.CreateQuery("from SKUCodeType where Code=:p").SetString("p", code).SetMaxResults(1).List
                     <SKUCodeType>();
            if (list.Count > 0)
            {
                SKUCodeType sku = list[0];
                if (sku.IsOut == 0)
                {
                    return Json(new { IsSuccess = true, Result = sku.SKU.Trim() });
                }
                else
                {
                    return Json(new { IsSuccess = false, Result = "当前产品已经出库过了！" });
                }
            }
            return Json(new { IsSuccess = false, Result = "没有找到这个产品！" });

        }
    }
}

