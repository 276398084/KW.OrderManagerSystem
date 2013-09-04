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
    public class ProductController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }

        public ActionResult ProductProfits()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Copy(int id)
        {
            ProductType obj = GetById(id);
            return View("Create", obj);
        }

        public ViewResult Details(int id)
        {
            ProductType obj = GetById(id);
            ViewData["id"] = id;
            return View(obj);
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

        public ActionResult WarningPurchaseList()
        {
            return View();
        }

        public ActionResult ToExcel()
        {
            var list = GetWarning();
            Session["ExportDown"] = ExcelHelper.GetExcelXml(Utilities.FillDataTable(list));
            return Json(new { IsSuccess = true, ErrorMsg = "导出成功" });
        }

        [HttpPost]
        public ActionResult WarningList(string order, string sort)
        {
            var list = GetWarning();
            return Json(new { total = list.Count, rows = list.OrderByDescending(x => x.NeedQty).ToList() });
        }

        private List<PurchaseData> GetWarning()
        {
            List<PurchaseData> list = new List<PurchaseData>();
            IList<WarehouseStockType> stocks = new List<WarehouseStockType>();
            IList<PurchasePlanType> plans = new List<PurchasePlanType>();
            IList<ProductType> products =
                NSession.CreateQuery(
                    @" From ProductType p where (
(round((SevenDay/7*0.5+Fifteen/15*0.3+ThirtyDay/30*0.2),0)*5)>(select count(Id) from SKUCodeType where IsOut=0 and SKU= p.SKU)
Or SKU in(select SKU from OrderProductType where OId In(select Id from OrderType where IsOutOfStock=1 and  Status<>'作废订单'))
)and IsScan=1 and Status not in('滞销','清仓','停产','暂停销售')")
                    .List<ProductType>();
            string ids = "";
            foreach (var p in products)
            {
                ids += "'" + p.SKU + "',";
            }

            stocks =
                NSession.CreateQuery("from WarehouseStockType where SKU in(" + ids.Trim(',') + ")").List<WarehouseStockType>();
            plans =
                NSession.CreateQuery("from PurchasePlanType where Status not in('异常','已收到')  and SKU in(" + ids.Trim(',') + ")")
                    .List<PurchasePlanType>();
            IList<object[]> SKUCount =
                NSession.CreateQuery("select SKU,count(Id) from SKUCodeType where IsOut=0 and SKU In (" + ids.Trim(',') +
                                     ") group by SKU").List<object[]>();
            IList<OrderProductType> orderProducts =
                NSession.CreateQuery("from OrderProductType where SKU in(" + ids.Trim(',') +
                                     ") and IsQue=1 and OId In(select Id from OrderType where IsOutOfStock=1 and Status<>'作废订单')")
                    .List<OrderProductType>();
            foreach (var p in products)
            {
                PurchaseData data = new PurchaseData();
                data.ItemName = p.ProductName;
                data.SKU = p.SKU;
                data.SPic = p.SPicUrl;
                data.SevenDay = p.SevenDay;
                data.FifteenDay = p.Fifteen;
                data.ThirtyDay = p.ThirtyDay;
                data.WarningQty =
                    Convert.ToInt32(Math.Round(((p.SevenDay / 7) * 0.5 + p.Fifteen / 15 * 0.3 + p.ThirtyDay / 30 * 0.2), 0) * 5);
                if (data.WarningQty < 1)
                {
                    data.WarningQty = 1;
                }
                data.IsImportant = 0;
                data.AvgQty = Math.Round(((p.SevenDay / 7) * 0.5 + p.Fifteen / 15 * 0.3 + p.ThirtyDay / 30 * 0.2), 2);
                foreach (var objectes in SKUCount)
                {
                    if (objectes[0].ToString().ToUpper() == data.SKU.ToUpper())
                    {
                        data.NowQty = Convert.ToInt32(objectes[1]);
                        break;
                    }
                }

                if (Math.Round(((p.SevenDay / 7) * 0.5 + p.Fifteen / 15 * 0.3 + p.ThirtyDay / 30 * 0.2), 0) * 3 < data.NowQty)
                {
                    data.IsImportant = 1;
                }
                int buyQty = plans.Where(x => x.SKU.Trim().ToUpper() == p.SKU.Trim().ToUpper()).Sum(x => x.DaoQty);
                data.BuyQty = plans.Where(x => x.SKU.Trim().ToUpper() == p.SKU.Trim().ToUpper()).Sum(x => x.Qty) - buyQty;
                data.QueQty = orderProducts.Where(x => x.SKU.Trim().ToUpper() == p.SKU.Trim().ToUpper()).Sum(x => x.Qty);

                if ((data.NowQty + data.BuyQty - data.WarningQty - data.QueQty) < 0)
                {
                    data.NeedQty = Convert.ToInt32(data.AvgQty * p.DayByStock) + data.QueQty - data.NowQty - data.BuyQty;
                    if (data.NeedQty > 0)
                        list.Add(data);
                }
            }
            return list;
        }

        [HttpPost]
        public ActionResult ImportProduct(string fileName)
        {
            try
            {
                List<ResultInfo> results = new List<ResultInfo>();
                DataTable dt = OrderHelper.GetDataTable(fileName);
                IList<WarehouseType> list = NSession.CreateQuery(" from WarehouseType").List<WarehouseType>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ProductType p = new ProductType { CreateOn = DateTime.Now };
                    p.SKU = dt.Rows[i]["子SKU"].ToString().Trim();
                    if (IsExist(p.SKU))
                    {
                        continue;
                    }
                    p.Status = dt.Rows[i]["销售状态"].ToString();
                    p.ProductName = dt.Rows[i]["名称"].ToString();
                    p.Category = dt.Rows[i]["分类"].ToString();
                    p.Standard = dt.Rows[i]["规格"].ToString();
                    p.Price = Convert.ToDouble(dt.Rows[i]["价格"]);
                    p.Weight = Convert.ToInt16(dt.Rows[i]["重量"]);
                    p.Long = Convert.ToInt16(dt.Rows[i]["长"]);
                    p.Wide = Convert.ToInt16(dt.Rows[i]["宽"]);
                    p.High = Convert.ToInt16(dt.Rows[i]["高"]);
                    p.Location = dt.Rows[i]["库位号"].ToString();
                    p.OldSKU = dt.Rows[i]["SKU"].ToString();
                    p.HasBattery = Convert.ToInt32(dt.Rows[i]["电池"].ToString());
                    p.IsElectronic = Convert.ToInt32(dt.Rows[i]["电子"].ToString());
                    p.IsScan = Convert.ToInt32(dt.Rows[i]["配货扫描"].ToString());
                    p.DayByStock = Convert.ToInt32(dt.Rows[i]["备货天数"].ToString());
                    p.ProductAttribute = dt.Rows[i]["产品属性"].ToString();
                    p.Enabled = 1;
                    if (!HasExsit(p.SKU))
                    {
                        NSession.SaveOrUpdate(p);
                        NSession.Flush();
                        results.Add(OrderHelper.GetResult(p.SKU, "", "导入成功"));

                        //在仓库中添加产品库存
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
                    else
                    {
                        results.Add(OrderHelper.GetResult(p.SKU, "该产品已存在", "导入失败"));
                    }
                }
                Session["Results"] = results;
                return Json(new { IsSuccess = true, Info = true });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, ErrorMsg = ex.Message, Info = true });
            }
        }

        private bool HasExsit(string p)
        {
            object obj = NSession.CreateQuery("select count(Id) from ProductType where SKU ='" + p + "' ").UniqueResult();
            if (Convert.ToInt32(obj) > 0)
            {
                return true;
            }
            return false;
        }

        [HttpPost]
        public ActionResult Export(string search, string c)
        {
            try
            {
                string where = Utilities.SqlWhere(search);

                if (!string.IsNullOrEmpty(c))
                {
                    string cs = "";
                    IList<object> objectes = NSession.CreateSQLQuery(@"with a as(
select * from ProductCategory where ID=" + c + @"
union all
select x.* from ProductCategory x,a
where x.ParentId=a.Id)
select Name from a").List<object>();
                    foreach (object item in objectes)
                    {
                        cs += "'" + item + "',";
                    }
                    if (cs.Length > 0)
                    {
                        cs = cs.Trim(',');
                    }
                    if (!string.IsNullOrEmpty(where))
                        where += " and Category in (" + cs + ")";
                    else
                        where = " where Category in (" + cs + ")";

                }
                List<ProductType> objList = NSession.CreateQuery("from ProductType " + where)
                    .List<ProductType>().ToList();
                Session["ExportDown"] = ExcelHelper.GetExcelXml(Utilities.FillDataTable((objList)));
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true, ErrorMsg = "导出成功" });
        }

        [HttpPost]
        public JsonResult Create(ProductType obj)
        {
            try
            {
                string filePath = Server.MapPath("~");
                obj.CreateOn = DateTime.Now;
                string pic = obj.PicUrl;
                if (!string.IsNullOrEmpty(pic))
                {
                    obj.PicUrl = Utilities.BPicPath + obj.SKU + ".jpg";
                    obj.SPicUrl = Utilities.SPicPath + obj.SKU + ".png";

                    Utilities.DrawImageRectRect(pic, filePath + obj.PicUrl, 310, 310);
                    Utilities.DrawImageRectRect(pic, filePath + obj.SPicUrl, 64, 64);
                }

                List<ProductComposeType> list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductComposeType>>(obj.rows);
                if (list1.Count > 0)
                    obj.IsZu = 1;
                obj.Enabled = 1;
                NSession.SaveOrUpdate(obj);
                NSession.Flush();
                foreach (ProductComposeType productCompose in list1)
                {
                    productCompose.SKU = obj.SKU;
                    productCompose.PId = obj.Id;
                    NSession.Save(productCompose);
                    NSession.Flush();
                }
                List<ProductIsInfractionType> list2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductIsInfractionType>>(obj.rows2);
                foreach (ProductIsInfractionType item in list2)
                {
                    item.OldSKU = obj.OldSKU;
                    item.SKU = obj.SKU;
                    NSession.Save(item);
                    NSession.Flush();
                }

                AddToWarehouse(obj);
                LoggerUtil.GetProductRecord(obj, "商品创建", "创建一件商品", CurrentUser, NSession);
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
        }

        private void AddToWarehouse(ProductType obj)
        {
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
            ViewData["id"] = id;
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(ProductType obj)
        {
            try
            {
                string str = "";
                obj.Enabled = 1;
                ProductType obj2 = GetById(obj.Id);
                List<ProductComposeType> list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductComposeType>>(obj.rows);
                List<ProductComposeType> pis = NSession.CreateQuery("from ProductComposeType where SKU='" + obj.SKU + "'").List<ProductComposeType>().ToList<ProductComposeType>();
                if (list1.Count != pis.Count)
                {
                    str += "组合产品由<br>";
                    foreach (var item in pis)
                    {
                        str += Zu(item);
                    }
                    str += "修改为<br> ";
                    foreach (var item in list1)
                    {
                        str += Zu(item);
                    }
                    str += "<br>";
                }
                else
                {
                    foreach (var item in pis)
                    {
                        int check = 0;
                        foreach (var it in list1)
                        {
                            if (it.SrcSKU == item.SrcSKU && it.SrcQty == item.SrcQty)
                            {
                                check = 1;
                            }
                        }
                        if (check != 1)
                        {
                            str += "组合产品由<br>";
                            foreach (var zu in pis)
                            {
                                str += Zu(zu);
                            }
                            str += "修改为<br> ";
                            foreach (var zu in list1)
                            {
                                str += Zu(zu);
                            }
                            str += "<br>";
                        }
                    }
                }
                NSession.Delete("from ProductComposeType where SKU='" + obj.SKU + "'");
                NSession.Flush();
                NSession.Clear();
                foreach (ProductComposeType productCompose in list1)
                {
                    productCompose.SKU = obj.SKU;
                    productCompose.PId = obj.Id;
                    NSession.Save(productCompose);
                    NSession.Flush();
                    NSession.Clear();
                    obj.IsZu = 1;
                }

                str += Utilities.GetObjEditString(obj2, obj) + "<br>";
                List<ProductIsInfractionType> list2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductIsInfractionType>>(obj.rows2);
                foreach (ProductIsInfractionType item in list2)
                {
                    IList<ProductIsInfractionType> qin = NSession.CreateQuery("from ProductIsInfractionType where SKU='" + obj.SKU + "' and Platform='" + item.Platform + "'").List<ProductIsInfractionType>();
                    if (qin.Count != 0)
                    {
                        foreach (var s in qin)
                        {
                            if (item.Isinfraction != s.Isinfraction)
                            {
                                str += item.Platform + "是否侵权由" + s.Isinfraction + "修改为" + item.Isinfraction + "<br>";
                            }
                        }
                    }
                }
                NSession.Delete("from ProductIsInfractionType where SKU='" + obj.SKU + "'");
                NSession.Flush();
                NSession.Clear();
                foreach (ProductIsInfractionType item in list2)
                {
                    item.OldSKU = obj.OldSKU;
                    item.SKU = obj.SKU;
                    NSession.Save(item);
                    NSession.Flush();
                    NSession.Clear();
                }
                NSession.Update(obj);
                NSession.Flush();
                NSession.Clear();

                //修改库存中的数据
                IList<WarehouseStockType> list = NSession.CreateQuery(" from WarehouseStockType where PId=" + obj.Id).List<WarehouseStockType>();
                //
                //在仓库中添加产品库存
                //
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {

                        item.Pic = obj.SPicUrl;
                        item.SKU = obj.SKU;
                        item.Title = obj.ProductName;
                        item.UpdateOn = DateTime.Now;
                        NSession.Update(item);
                        NSession.Flush();
                    }
                }
                else
                {
                    AddToWarehouse(obj);
                }
                LoggerUtil.GetProductRecord(obj, "商品修改", str, CurrentUser, NSession);
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });

        }

        public string Zu(ProductComposeType item)
        {
            string str = " sku:" + item.SrcSKU + " Qty:" + item.SrcQty + "<br>";
            return str;
        }

        [HttpPost]
        public ActionResult EditStatus(string p, string s)
        {
            List<ProductType> products =
                NSession.CreateQuery("from ProductType where Id in(" + p + ")").List<ProductType>().ToList();
            foreach (ProductType productType in products)
            {
                productType.Status = s;
                NSession.Update(productType);
                NSession.Flush();
                LoggerUtil.GetProductRecord(productType, "订单产品属性修改",
                                                 "产品：" + productType.SKU + "状态修改为“" + s + "”",
                                                 CurrentUser, NSession);
                if (s == "停产")
                {
                    int count = 0;
                    string orderid = "";
                    List<OrderType> orderTypes =
                        NSession.CreateQuery(
                            "from OrderType where Id in(select OId from OrderProductType where SKU='" + productType.SKU + "') and Status='已处理' and Enabled=1")
                            .List<OrderType>().ToList();
                    foreach (OrderType orderType in orderTypes)
                    {
                        orderType.IsStop = 1;
                        NSession.Update(orderType);
                        NSession.Flush();
                        LoggerUtil.GetOrderRecord(orderType, "订单产品属性修改",
                                                  "产品：" + productType.SKU + "状态修改为“" + s + "”，同时订单属性设置为停售订单", CurrentUser, NSession);
                        orderid += orderType.OrderNo + "  ";
                        count++;
                    }
                    NSession.CreateQuery("update OrderProductType set IsQue=2 where SKU='" + productType.SKU + "'").UniqueResult();
                    PlacardType placard = new PlacardType { CardType = "产品", Title = productType.SKU + " 停产", Content = "相关" + count + "条订单编号： " + orderid, CreateBy = "系统自动", CreateOn = DateTime.Now, IsTop = 1 };
                    NSession.Save(placard);
                    NSession.Flush();

                }
            }
            return Json(new { IsSuccess = true });
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                ProductType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
                NSession.Delete("from WarehouseStockType where SKU='" + obj.SKU + "'");
                NSession.Flush();
                LoggerUtil.GetProductRecord(obj, "删除商品", "商品被删除", CurrentUser, NSession);
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
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
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
        }
        public JsonResult ListQ(string q)
        {
            IList<ProductType> objList = NSession.CreateQuery("from ProductType where Status <>'停产' and SKU like '%" + q + "%'")
                .SetFirstResult(0)
                .SetMaxResults(20)
                .List<ProductType>();

            return Json(new { total = objList.Count, rows = objList });
        }

        public ActionResult SKUScan()
        {
            return View();
        }
        public ActionResult SKUScan2()
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

        private string InfractionWhere(string infraction)
        {
            string where = "";
            if (infraction == "全侵权")
            {
                where = " and SKU in(SELECT SKU FROM ProductIsInfractionType group by SKU having min(Isinfraction)=1) ";
            }
            else if (infraction == "无侵权")
            {
                where = " and SKU in(SELECT SKU FROM ProductIsInfractionType group by SKU having max(Isinfraction)=0) ";
            }
            else
            {
                where = " and SKU in(SELECT SKU FROM ProductIsInfractionType where Isinfraction=1 and Platform='" + infraction + "' ) ";
            }
            return where;
        }

        public JsonResult List(int page, int rows, string sort, string order, string search, string infraction = "", string c = "")
        {
            string orderby = Utilities.OrdeerBy(sort, order);
            string where = Utilities.SqlWhere(search);
            if (where != "")
            {
                where += " and  Enabled <>0";
            }
            else
            {
                where = "where Enabled <>0";
            }
            if (infraction != "")
            {
                where += InfractionWhere(infraction);
            }
            if (!string.IsNullOrEmpty(c))
            {
                string cs = "";
                IList<object> objectes = NSession.CreateSQLQuery(@"with a as(
select * from ProductCategory where ID=" + c + @"
union all
select x.* from ProductCategory x,a
where x.ParentId=a.Id)
select Name from a").List<object>();
                foreach (object item in objectes)
                {
                    cs += "'" + item + "',";
                }
                if (cs.Length > 0)
                {
                    cs = cs.Trim(',');
                }
                where += " and Category in (" + cs + ")";

            }
            IList<ProductType> objList = NSession.CreateQuery("from ProductType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<ProductType>();
            foreach (ProductType item in objList)
            {
                item.Infraction = Infraction(item.SKU);
            }
            object count = NSession.CreateQuery("select count(Id) from ProductType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }


        public JsonResult ReP(string p)
        {
            string username = GetCurrentAccount().Username;
            string str = "kelvin,csa,jb";
            if (str.IndexOf(username) != -1)
            {
                string ps = p.Replace("\r", "").Trim('\n').Replace("\n", "','");
                NSession.CreateQuery("update  SKUCodeType set IsOut=1,IsSend=1 where SKU in('" + ps + "') ").ExecuteUpdate();
                NSession.CreateSQLQuery("update WarehouseStock set Qty=(select COUNT(1) from skucode where SKU=WarehouseStock.SKU and IsSend=0) where SKU  in('" + ps + "')").ExecuteUpdate();
                return Json(new { IsSuccess = true });
            }
            else
            {
                return Json(new { IsSuccess = false });
            }
        }

        public JsonResult ZuList(String Id)
        {
            IList<ProductComposeType> objList = NSession.CreateQuery("from ProductComposeType where SKU='" + Id + "'").List<ProductComposeType>();
            return Json(new { total = objList.Count, rows = objList });
        }

        public JsonResult PlList(String Id)
        {
            IList<ProductIsInfractionType> objList = NSession.CreateQuery("from ProductIsInfractionType where SKU='" + Id + "'").List<ProductIsInfractionType>();
            if (objList.Count != 0)
                return Json(new { total = objList.Count, rows = objList });
            else
            {
                List<ProductIsInfractionType> list = new List<ProductIsInfractionType>();
                foreach (string item in Enum.GetNames(typeof(PlatformEnum)))
                {
                    ProductIsInfractionType obj = new ProductIsInfractionType();
                    obj.Platform = item; obj.Isinfraction = 0;
                    list.Add(obj);
                }
                return Json(list);
            }
        }

        public bool IsExist(string sku)
        {
            object count = NSession.CreateQuery("select count(Id) from ProductType where SKU='" + sku + "'").UniqueResult();
            if (Convert.ToInt32(count) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public JsonResult HasExist(string sku)
        {

            if (IsExist(sku))
            {
                return Json(new { IsSuccess = false });
            }
            else
            {
                return Json(new { IsSuccess = true });
            }
        }

        public ActionResult SetSKUCode(int code, string sku)
        {
            return Json(new { IsSuccess = false, Result = "此功能作废，请不要这个功能！" });
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
                    Utilities.StockIn(1, sku, 1, 0, "条码清点入库", CurrentUser.Realname, "", NSession);
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

        public ActionResult SetSKUCode2(int code)
        {
            IList<SKUCodeType> list =
                 NSession.CreateQuery("from SKUCodeType where Code=:p").SetInt32("p", code).SetMaxResults(1).List
                     <SKUCodeType>();
            if (list.Count > 0)
            {
                SKUCodeType sku = list[0];
                if (sku.IsOut == 1 || sku.IsSend == 1)
                {
                    return Json(new { IsSuccess = false, Result = "条码：" + code + " 已经配过货,SKU:" + sku.SKU + " 出库时间：" + sku.PeiOn + ",出库订单:" + sku.OrderNo + " ,请将此产品单独挑出来！" });
                }
                if (sku.IsScan == 1)
                {
                    return Json(new { IsSuccess = false, Result = "条码：" + code + " 已经清点扫描了,SKU:" + sku.SKU + " 刚刚已经扫描过了。你查看下是条码重复扫描了，还是有贴重复的了！" });
                }
                sku.IsScan = 1;
                NSession.Save(sku);
                NSession.Flush();
                object obj =
                    NSession.CreateQuery("select count(Id) from SKUCodeType where SKU=:p and IsScan=1 and IsOut=0").SetString("p", sku.SKU).
                        UniqueResult();
                return Json(new { IsSuccess = true, Result = "条码：" + code + " 的信息.SKU：" + sku.SKU + " 此条码未出库。条码正确！！！", ccc = sku.SKU + "已经扫描了" + obj + "个" });
            }
            else
            {
                return Json(new { IsSuccess = false, Result = "条码：" + code + " 无法找到 ,请查看扫描是否正确！" });
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
        public ActionResult Platform()
        {
            List<ProductIsInfractionType> list = new List<ProductIsInfractionType>();
            foreach (string item in Enum.GetNames(typeof(PlatformEnum)))
            {
                ProductIsInfractionType obj = new ProductIsInfractionType();
                obj.Platform = item; obj.Isinfraction = 0;
                list.Add(obj);
            }
            return Json(list);
        }



        public JsonResult SearchSKU(string id)
        {
            IList<ProductType> obj = NSession.CreateQuery("from ProductType where SKU='" + id + "'").List<ProductType>();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Freight(decimal price, double weight, int qty, decimal onlineprice, string Currency, string LogisticMode, int Country)
        {
            decimal freight = decimal.Parse((OrderHelper.GetFreight(weight * qty, LogisticMode, Country, NSession)).ToString("f6"));
            if (freight == -1)
                return Json(new { IsSuccess = false, ErrorMsg = "cz" }, JsonRequestBehavior.AllowGet);
            decimal currency = decimal.Parse(Math.Round(GetCurrency(Currency), 2).ToString());
            decimal profit = (onlineprice * currency - price) * qty - freight;
            return Json(new { IsSuccess = true, profit = profit, freight = freight }, JsonRequestBehavior.AllowGet);
        }

        public decimal GetCurrency(string code)
        {
            decimal curr = 0;
            IList<CurrencyType> list = NSession.CreateQuery("from CurrencyType where CurrencyCode='" + code + "'").List<CurrencyType>();
            foreach (var s in list)
            {
                curr = s.CurrencyValue;
            }
            return curr;
        }

        public JsonResult Record(int id)
        {
            IList<ProductRecordType> obj = NSession.CreateQuery("from ProductRecordType where Oid='" + id + "'").List<ProductRecordType>();
            return Json(obj.OrderByDescending(p => p.CreateOn), JsonRequestBehavior.AllowGet);
        }
        public string Infraction(string sku)
        {
            string str = "";
            IList<ProductIsInfractionType> objs = NSession.CreateQuery("from ProductIsInfractionType where SKU='" + sku + "' and Isinfraction=1 ").List<ProductIsInfractionType>();
            foreach (var item in objs)
            {
                str += item.Platform + "<br/>";
            }
            if (str == "")
            {
                str = "否";
            }
            return str;
        }

    }
}

