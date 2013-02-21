using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;

namespace KeWeiOMS.Web
{
    public class Utilities
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
        public const string CookieName = "KeWeiOMS_User";
        public const string BPicPath = "/ProductImg/BPic/";
        public const string SPicPath = "/ProductImg/SPic/";
        public static object obj1 = new object();
        public static object obj2 = new object();
        public static object obj3 = new object();

        public static string GetOrderNo()
        {
            lock (obj1)
            {
                string result = string.Empty;
                ISession NSession = NHibernateHelper.CreateSession();
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

        public static string GetUserNo()
        {
            lock (obj2)
            {
                string result = string.Empty;
                ISession NSession = NHibernateHelper.CreateSession();
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

        public static string GetPlanNo()
        {
            lock (obj3)
            {
                string result = string.Empty;
                ISession NSession = NHibernateHelper.CreateSession();

                IList<SerialNumberType> list = NSession.CreateQuery(" from SerialNumberType where Code=:p").SetString("p", PlanNo).List<SerialNumberType>();
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

            System.Drawing.Image imageFrom = System.Drawing.Image.FromFile(rawImgPath);
            DrawImageRectRect(imageFrom, newImgPath, width, height);
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
                    if ((!string.IsNullOrEmpty(ss[0])) && (!string.IsNullOrEmpty(ss[1])))
                    {
                        queryDictionary.Add(ss[0], ss[1]);
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
                    where += item.Key + " like '%" + item.Value + "%'";
                }
            }
            return where;
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

        public static bool LoginByUser(string p, string u)
        {
            ISession NSession = NHibernateHelper.CreateSession();
            IList<UserType> list = NSession.CreateQuery(" from  UserType where Username=:p1 and Password=:p2").SetString("p1", p).SetString("p2", u).List<UserType>();
            if (list.Count > 0)
            {   //登录成功
                UserType user = list[0];
                user.LastVisit = DateTime.Now;
                NSession.Update(user);
                NSession.Flush();
                System.Web.HttpContext.Current.Session["account"] = user;
                return true;
            }
            return false;
        }
        #endregion


        public static bool StockOut(int wid, string sku, int num, string outType, string user, string memo, string orderNo)
        {
            ISession NSession = NHibernateHelper.CreateSession();
            IList<WarehouseStockType> list = NSession.CreateQuery(" from WarehouseStockType where WId=:p1 and SKU=:p2").SetInt32("p1", wid).SetString("p2", sku).List<WarehouseStockType>();
            if (list.Count > 0)
            {
                WarehouseStockType ws = list[0];
                ws.Qty = ws.Qty - num;
                NSession.SaveOrUpdate(ws);
                NSession.Flush();

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
        public static bool StockIn(int wid, string sku, int num, double price, string InType, string user, string memo)
        {
            ISession NSession = NHibernateHelper.CreateSession();
            IList<WarehouseStockType> list = NSession.CreateQuery(" from WarehouseStockType where WId=:p1 and SKU=:p2").SetInt32("p1", wid).SetString("p2", sku).List<WarehouseStockType>();
            if (list.Count > 0)
            {
                WarehouseStockType ws = list[0];
                ws.Qty = ws.Qty + num;
                NSession.SaveOrUpdate(ws);
                NSession.Flush();
                if (price != 0)
                {
                    IList<ProductType> foo =
                        NSession.CreateQuery(" from ProductType where WId=:p1 and SKU=:p2").SetInt32("p1", wid).
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

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Memo { get; set; }
    }
}