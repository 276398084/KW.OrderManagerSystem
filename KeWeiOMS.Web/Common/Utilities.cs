using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;

namespace KeWeiOMS.Web
{
    public static class Utilities
    {
        public const string OrderNo = "OrderNo";
        public const string UserNo = "UserNo";
        public const string PlanNo = "PlanNo";
        public const string Start_Time = "_st";
        public const string End_Time = "_et";
        public const string Start_Int = "_si";
        public const string End_Int = "_ei";
        public const string End_String = "_es";
        public const string DDL_String = "_ds";
        public const string DDL_UnString = "_un";
        public const string CookieName = "KeWeiOMS_User";
        public const string BPicPath = "/ProductImg/BPic/";
        public const string SPicPath = "/ProductImg/SPic/";
        public static object obj1 = new object();
        public static object obj2 = new object();
        public static object obj3 = new object();
        public static object obj4 = new object();

        public static string GetOrderNo(ISession NSession)
        {
            lock (obj1)
            {
                string result = string.Empty;
                NSession.Clear();
                IList<SerialNumberType> list = NSession.CreateQuery(" from SerialNumberType where Code=:p").SetString("p", OrderNo).List<SerialNumberType>();
                if (list.Count > 0)
                {
                    list[0].BeginNo = list[0].BeginNo + 1;
                    NSession.Update(list[0]);
                    NSession.Flush();
                    return list[0].BeginNo.ToString();
                }
                return "";
            }
        }
        public static string GetUserNo(ISession NSession)
        {
            lock (obj2)
            {
                string result = string.Empty;
                NSession.Clear();
                IList<SerialNumberType> list =
                    NSession.CreateQuery(" from SerialNumberType where Code=:p").SetString("p", UserNo).List
                        <SerialNumberType>();
                if (list.Count > 0)
                {
                    list[0].BeginNo = list[0].BeginNo + 1;
                    NSession.Update(list[0]);
                    NSession.Flush();
                    return list[0].BeginNo.ToString();
                }
                return "";
            }
        }

        public static string GetPlanNo(ISession NSession)
        {
            lock (obj3)
            {
                string result = string.Empty;

                NSession.Clear();
                IList<SerialNumberType> list = NSession.CreateQuery(" from SerialNumberType where Code=:p").SetString("p", PlanNo).List<SerialNumberType>();
                if (list.Count > 0)
                {
                    list[0].BeginNo = list[0].BeginNo + 1;
                    NSession.Update(list[0]);
                    NSession.Flush();
                    return "SP" + list[0].BeginNo.ToString();
                }
                return "";
            }
        }
        public static int GetSKUCode(int count, ISession NSession)
        {
            lock (obj4)
            {
                string result = string.Empty;
                int no = 0;
                NSession.Clear();
                IList<SerialNumberType> list = NSession.CreateQuery(" from SerialNumberType where Code=:p").SetString("p", "SKUNo").List<SerialNumberType>();
                if (list.Count > 0)
                {
                    no = list[0].BeginNo + 1;
                    list[0].BeginNo = list[0].BeginNo + count;
                    NSession.Update(list[0]);
                    NSession.Flush();
                    return no;
                }
                return 0;
            }
        }

        public static int CreateSKUCode(string sku, int count, string planNo, ISession NSession)
        {
            int code = GetSKUCode(count, NSession);
            string create = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            using (var tr = NSession.BeginTransaction())
            {
                for (int i = code; i < code + count; i++)
                {
                    SKUCodeType SKUCode = new SKUCodeType();
                    SKUCode.Code = i;
                    SKUCode.SKU = sku;
                    SKUCode.IsOut = 0;
                    SKUCode.IsNew = 1;
                    SKUCode.IsSend = 0;
                    SKUCode.IsScan = 0;
                    SKUCode.CreateOn = create;
                    SKUCode.PlanNo = planNo;
                    SKUCode.SendOn = "";
                    SKUCode.PeiOn = "";
                    NSession.Save(SKUCode);
                }
                tr.Commit();
            }


            return code;
        }



        #region 缩略图生成
        public static void DrawImageRectRect(Image imageFrom, string newImgPath, int width, int height)
        {

            // 源图宽度及高度 
            int imageFromWidth = imageFrom.Width;
            int imageFromHeight = imageFrom.Height;
            //在原画布中的位置
            int X, Y;
            //在原画布中取得的长宽
            int bitmapWidth, bitmapHeight;
            //// 根据源图及欲生成的缩略图尺寸,计算缩略图的实际尺寸及其在"画布"上的位置 
            if (imageFromWidth / width > imageFromHeight / height)
            {
                bitmapWidth = (width * imageFromHeight) / height;
                bitmapHeight = imageFromHeight;
                X = (imageFromWidth - bitmapWidth) / 2;
                Y = 0;
            }
            else
            {
                bitmapWidth = imageFromWidth;
                bitmapHeight = (height * imageFromWidth) / width;
                X = 0;
                Y = (imageFromHeight - bitmapHeight) / 2;

            }
            // 创建画布 
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            // 用白色清空 
            g.Clear(Color.White);
            // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。 
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // 指定高质量、低速度呈现。 
            g.SmoothingMode = SmoothingMode.HighQuality;
            // 在指定位置并且按指定大小绘制指定的 Image 的指定部分。 
            g.DrawImage(imageFrom, new Rectangle(0, 0, width, height), new Rectangle(X, Y, bitmapWidth, bitmapHeight), GraphicsUnit.Pixel);
            try
            {
                //经测试 .jpg 格式缩略图大小与质量等最优 
                bmp.Save(newImgPath, ImageFormat.Jpeg);
            }
            catch
            {
            }
            finally
            {
                //显示释放资源 
                imageFrom.Dispose();
                bmp.Dispose();
                g.Dispose();
            }
        }

        public static void DrawImageRectRect(string rawImgPath, string newImgPath, int width, int height)
        {
            try
            {
                System.Drawing.Image imageFrom = System.Drawing.Image.FromFile(rawImgPath);
                DrawImageRectRect(imageFrom, newImgPath, width, height);
            }
            catch (Exception)
            {

            }

        }
        #endregion

        /// <summary>
        /// 将String转换为Dictionary类型，过滤掉为空的值，首先 6 分割，再 7 分割
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<string, string> StringToDictionary(string value)
        {
            Dictionary<string, string> queryDictionary = new Dictionary<string, string>();
            string[] s = value.Split('^');
            for (int i = 0; i < s.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(s[i]) && !s[i].Contains("undefined"))
                {
                    var ss = s[i].Split('&');
                    if (ss.Length == 2)
                    {
                        if ((!string.IsNullOrEmpty(ss[0])) && (!string.IsNullOrEmpty(ss[1])))
                        {
                            queryDictionary.Add(ss[0], ss[1]);
                        }
                    }
                }

            }
            return queryDictionary;
        }

        public static string XmlSerialize<T>(T obj)
        {
            string xmlString = string.Empty;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                xmlSerializer.Serialize(ms, obj);
                xmlString = Encoding.UTF8.GetString(ms.ToArray());
            }
            return xmlString;
        }
        public static string Resolve(string search)
        {
            string where = string.Empty;
            int flagWhere = 0;

            Dictionary<string, string> queryDic = StringToDictionary(search);
            if (queryDic != null && queryDic.Count > 0)
            {
                foreach (var item in queryDic)
                {
                    if (flagWhere != 0)
                    {
                        where += " and ";
                    }
                    flagWhere++;
                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value) && item.Key.Contains("SKU_OrderProduct")) //需要查询的列名
                    {
                        where += " Id in (select OId from OrderProductType where SKU='" + item.Value + "')";
                        continue;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value) && item.Key.Contains(Start_Time)) //需要查询的列名
                    {
                        where += item.Key.Remove(item.Key.IndexOf(Start_Time)) + " >= '" + item.Value + "'";
                        continue;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value) && item.Key.Contains(End_Time)) //需要查询的列名
                    {
                        where += item.Key.Remove(item.Key.IndexOf(End_Time)) + " <=  '" + item.Value + "'";
                        continue;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value) && item.Key.Contains(Start_Int)) //需要查询的列名
                    {
                        where += item.Key.Remove(item.Key.IndexOf(Start_Int)) + " >= " + item.Value;
                        continue;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value) && item.Key.Contains(End_Int)) //需要查询的列名
                    {
                        where += item.Key.Remove(item.Key.IndexOf(End_Int)) + " <= " + item.Value;
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value) && item.Key.Contains(End_String)) //需要查询的列名
                    {
                        where += item.Key.Remove(item.Key.IndexOf(End_String)) + " = '" + item.Value + "'";
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value) && item.Key.Contains(DDL_String)) //需要查询的列名
                    {
                        where += item.Key.Remove(item.Key.IndexOf(DDL_String)) + " = '" + item.Value + "'";
                        continue;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value) && item.Key.Contains(DDL_UnString)) //需要查询的列名
                    {
                        where += item.Key.Remove(item.Key.IndexOf(DDL_String)) + " <> '" + item.Value + "'";
                        continue;
                    }
                    where += item.Key + " like '%" + item.Value + "%'";
                }
            }
            return where;
        }

        public static string GetObjEditString(object o1, object o2)
        {
            StringBuilder sb = new StringBuilder();
            System.Reflection.PropertyInfo[] properties = o1.GetType().GetProperties();

            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name;
                if (name.StartsWith("rows"))
                {
                    continue;
                }
                object value = item.GetValue(o1, null);
                object value2 = item.GetValue(o2, null);
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    if (value == null)
                        value = "";
                    if (value2 == null)
                        value2 = "";
                    if (value.ToString() != value2.ToString())
                    {
                        sb.Append(" " + name + "从“" + value + "” 修改为 “" + value2 + "”<br>");
                    }

                }
            }
            return sb.ToString();
        }


        #region 登陆和Cookie

        /// <summary>
        /// Create or Set Cookies Values
        /// </summary>
        /// <param name="Obj">[0]:Name,[1]:Value</param>
        public static void CreateCookies(string u, string p)
        {


            try
            {
                HttpCookie cookie = new HttpCookie(CookieName)
                {
                    Expires = DateTime.Now.AddDays(1),
                };
                cookie.Value = u + "&" + p;
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch
            {


            }
        }

        /// <summary>
        /// Get Cookies Values
        /// </summary>
        /// <param name="name">Cookies的Name</param>
        /// <returns></returns>
        public static string GetCookies()
        {
            try
            {
                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[Utilities.CookieName];
                return cookie.Value;
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Clear Cookies Values
        /// </summary>
        /// <param name="name">Cookies的Name</param>
        public static void ClearCookies()
        {
            HttpCookie cookie = new HttpCookie(Utilities.CookieName)
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            System.Web.HttpContext.Current.Session["account"] = null;
        }

        public static bool LoginByUser(string p, string u, ISession NSession)
        {

            IList<UserType> list = NSession.CreateQuery(" from  UserType where Username=:p1 and Password=:p2").SetString("p1", p).SetString("p2", u).List<UserType>();
            if (list.Count > 0)
            {   //登录成功
                UserType user = list[0];
                user.LastVisit = DateTime.Now;
                NSession.Update(user);
                NSession.Flush();
                GetPM(user, NSession);
                System.Web.HttpContext.Current.Session["account"] = user;
                return true;
            }
            return false;
        }

        public static void GetPM(UserType currentUser, ISession NSession)
        {
            List<PermissionScopeType> listByModules = new List<PermissionScopeType>();
            List<PermissionScopeType> listByPermissions = new List<PermissionScopeType>();
            List<PermissionScopeType> listByAccount = new List<PermissionScopeType>();

            foreach (var item in GetUserScope(currentUser.Id, NSession))
            {
                GetValue(item, listByModules, listByPermissions, listByAccount);

            }
            foreach (var item in GetRoleScope(currentUser.RoleId, NSession))
            {
                GetValue(item, listByModules, listByPermissions, listByAccount);
            }
            foreach (var item in GetDepartmentScope(currentUser.Id, NSession))
            {
                GetValue(item, listByModules, listByPermissions, listByAccount);
            }
            string mids = "";
            string pids = "";
            string aids = "";

            foreach (var item in listByModules)
            {
                mids += item.TargetId + ",";
            }
            foreach (var item in listByPermissions)
            {
                pids += item.TargetId + ",";
            }
            foreach (var item in listByPermissions)
            {
                pids += item.TargetId + ",";
            }
            mids = mids.Trim(',');
            pids = pids.Trim(',');
            aids = aids.Trim(',');
            if (mids.Length == 0)
                mids = "''";
            if (pids.Length == 0)
                pids = "''";
            if (aids.Length == 0)
                aids = "''";
            List<ModuleType> Modules = NSession.CreateQuery("from ModuleType where Id in(" + mids + ")").List<ModuleType>().ToList<ModuleType>();
            List<PermissionItemType> Permissions = NSession.CreateQuery("from PermissionItemType where Id in(" + pids + ")").List<PermissionItemType>().ToList<PermissionItemType>();

            List<AccountType> Accounts = NSession.CreateQuery("from AccountType where Id in(" + aids + ")").List<AccountType>().ToList<AccountType>();
            currentUser.Modules = Modules;
            currentUser.Permissions = Permissions;
            currentUser.Accounts = Accounts;
            System.Web.HttpContext.Current.Session["account"] = currentUser;
        }

        /// <summary>
        /// 实体类转换成DataTable
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public static DataTable FillDataTable<T>(IList<T> modelList)
        {
            if (modelList == null || modelList.Count == 0)
            {
                return new DataTable();
            }
            DataTable dt = CreateData<T>(modelList[0]);

            foreach (T model in modelList)
            {
                DataRow dataRow = dt.NewRow();
                foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                {
                    dataRow[propertyInfo.Name] = propertyInfo.GetValue(model, null);
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        /// <summary>
        /// 根据实体类得到表结构
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        private static DataTable CreateData<T>(T model)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                dataTable.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType));
            }
            return dataTable;
        }

        #endregion

        private static void GetValue(PermissionScopeType item, List<PermissionScopeType> listByModules, List<PermissionScopeType> listByPermissions, List<PermissionScopeType> listByAccount)
        {
            if (item.TargetCategory == TargetCategoryEnum.Module.ToString())
            {
                listByModules.Add(item);
            }
            else if (item.TargetCategory == TargetCategoryEnum.PermissionItem.ToString())
            {
                listByPermissions.Add(item);
            }
            else
            {
                listByAccount.Add(item);
            }
        }

        public static List<PermissionScopeType> GetUserScope(int id, ISession NSession)
        {
            List<PermissionScopeType> list = NSession.CreateQuery("from PermissionScopeType where ResourceCategory=:p1 and ResourceId=:p2").SetString("p1", ResourceCategoryEnum.User.ToString()).SetInt32("p2", id).List<PermissionScopeType>().ToList<PermissionScopeType>();
            return list;
        }

        public static List<PermissionScopeType> GetRoleScope(int id, ISession NSession)
        {
            List<PermissionScopeType> list = NSession.CreateQuery("from PermissionScopeType where ResourceCategory=:p1 and ResourceId=:p2").SetString("p1", ResourceCategoryEnum.Role.ToString()).SetInt32("p2", id).List<PermissionScopeType>().ToList<PermissionScopeType>();
            return list;
        }

        public static List<PermissionScopeType> GetDepartmentScope(int id, ISession NSession)
        {
            List<PermissionScopeType> list = NSession.CreateQuery("from PermissionScopeType where ResourceCategory=:p1 and ResourceId=:p2").SetString("p1", ResourceCategoryEnum.User.ToString()).SetInt32("p2", id).List<PermissionScopeType>().ToList<PermissionScopeType>();
            return list;
        }

        public static void SetComposeStock(string sku, ISession NSession)
        {
            //这个产品有几个产品组成
            List<ProductComposeType> products = NSession.CreateQuery(" from ProductComposeType where SKU in (select SKU from ProductComposeType where SrcSKU='" + sku + "')").List<ProductComposeType>().ToList();
            string skulist = "";
            if (products.Count == 0)
            {
                return;

            }
            foreach (ProductComposeType productComposeType in products)
            {
                skulist += productComposeType.SrcSKU + ",";
            }

            skulist = skulist.Trim(',').Replace(",", "','");
            IList<WarehouseStockType> stocklist = NSession.CreateQuery("from WarehouseStockType where SKU in ('" + skulist + "')").List<WarehouseStockType>();

            int min = 0;
            foreach (WarehouseStockType warehouseStockType in stocklist)
            {
                ProductComposeType composeType = products.Find(p => p.SrcSKU.Trim().ToUpper() == warehouseStockType.SKU.ToUpper());
                int j = warehouseStockType.Qty / composeType.SrcQty;
                if (min == 0 || j < min)
                {
                    min = j;
                }
            }


            IList<WarehouseStockType> list = NSession.CreateQuery("from WarehouseStockType where SKU ='" + products[0].SKU + "'").List<WarehouseStockType>();
            if (list.Count > 0)
            {
                list[0].Qty = min;
                NSession.Update(list[0]);
                NSession.Flush();
            }
            else
            {
                IList<ProductType> productTypes = NSession.CreateQuery("from ProductType where SKU ='" + products[0].SKU + "'").List<ProductType>();
                if (products.Count > 0)
                {
                    AddToWarehouse(productTypes[0], NSession, min);
                }
            }

        }
        private static void AddToWarehouse(ProductType obj, ISession NSession, int Qty = 0)
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
                stock.Qty = Qty;
                stock.UpdateOn = DateTime.Now;
                NSession.SaveOrUpdate(stock);
                NSession.Flush();
            }
        }


        public static bool StockOut(int wid, string sku, int num, string outType, string user, string memo, string orderNo, ISession NSession)
        {

            IList<WarehouseStockType> list = NSession.CreateQuery(" from WarehouseStockType where WId=:p1 and SKU=:p2").SetInt32("p1", wid).SetString("p2", sku).List<WarehouseStockType>();
            if (list.Count > 0)
            {
                WarehouseStockType ws = list[0];
                ws.Qty = ws.Qty - num;
                NSession.SaveOrUpdate(ws);
                NSession.Flush();
                SetComposeStock(sku, NSession);
                StockOutType stockOutType = new StockOutType();
                stockOutType.CreateBy = user;
                stockOutType.CreateOn = DateTime.Now;
                stockOutType.OrderNo = orderNo;
                stockOutType.Qty = num;
                stockOutType.SKU = sku;
                stockOutType.OutType = outType;
                stockOutType.SourceQty = ws.Qty;
                stockOutType.WId = wid;
                stockOutType.Memo = memo;
                NSession.Save(stockOutType);
                NSession.Flush();
                return true;
            }
            return false;
        }

        public static bool StockIn(int wid, string sku, int num, double price, string InType, string user, string memo, ISession NSession, bool isAudit = false)
        {

            IList<WarehouseStockType> list = NSession.CreateQuery(" from WarehouseStockType where WId=:p1 and SKU=:p2").SetInt32("p1", wid).SetString("p2", sku).List<WarehouseStockType>();
            if (list.Count > 0)
            {
                WarehouseStockType ws = list[0];
                ws.Qty = ws.Qty + num;
                NSession.SaveOrUpdate(ws);
                NSession.Flush();
                SetComposeStock(sku, NSession);
                if (price != 0)
                {
                    IList<ProductType> foo =
                        NSession.CreateQuery(" from ProductType where  SKU=:p2").
                            SetString("p2", sku).List<ProductType>();
                    if (foo.Count > 0)
                    {
                        ProductType p = foo[0];
                        p.Price = price;
                        NSession.SaveOrUpdate(p);
                        NSession.Flush();

                    }
                }
                StockInType stockInType = new StockInType();

                stockInType.IsAudit = 0;
                if (isAudit)
                    stockInType.IsAudit = 1;
                stockInType.Price = price;
                stockInType.Qty = num;
                stockInType.SKU = sku;
                stockInType.WId = wid;
                stockInType.InType = InType;
                stockInType.Memo = memo;
                stockInType.WName = ws.Warehouse;
                stockInType.SourceQty = ws.Qty;
                stockInType.CreateBy = user;
                stockInType.CreateOn = DateTime.Now;
                NSession.SaveOrUpdate(stockInType);
                NSession.Flush();
                return true;
            }
            return false;
        }

        public static String ToDBC(String input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new String(c);
        }

        public static int ToInt(string str)
        {
            try
            {
                return Convert.ToInt32(str);
            }
            catch (Exception)
            {
                return 0;
            }

        }
        public static int ToInt(object obj)
        {
            try
            {
                if (obj == null || obj is DBNull)
                {
                    return 0;
                }
                return ToInt(obj.ToString());
            }
            catch (Exception)
            {
                return 0;
            }

        }
        public static double ToDouble(string str)
        {
            try
            {
                return Convert.ToDouble(str);
            }
            catch (Exception)
            {
                return 0;
            }

        }


        public static string ToStr(this object obj)
        {
            try
            {
                if (obj is DBNull || obj == null)
                    return "";
                return obj.ToString();
            }
            catch (Exception)
            {
                return "";
            }

        }
        public static string OrdeerBy(string sort, string order)
        {
            string orderby = " order by Id desc ";
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
            {
                orderby = " order by " + sort + " " + order;
            }
            return orderby;
        }
        public static string SqlWhere(string search)
        {
            //search=HttpUtility.UrlDecode(search);
            string where = string.Empty;
            if (!string.IsNullOrEmpty(search))
            {
                where = Utilities.Resolve(search);
                if (where.Length > 0)
                {
                    where = " where " + where;
                }
            }
            return where;
        }
    }



    public class ResultInfo
    {
        public virtual string Key { get; set; }
        public virtual string Field1 { get; set; }
        public virtual string Field2 { get; set; }
        public virtual string Field3 { get; set; }
        public virtual string Field4 { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public virtual string Info { get; set; }
        /// <summary>
        /// 结果
        /// </summary>
        public virtual string Result { get; set; }


        public virtual DateTime CreateOn { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Memo { get; set; }
    }

}