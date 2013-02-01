using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;

namespace KeWeiOMS.Web
{
    public class Utilities
    {
        public const string OrderNo = "OrderNo";

        public const string UserNo = "UserNo";
        /// <summary>
        /// 大图片路劲
        /// </summary>
        public const string BPicPath = "/ProductImg/BPic/";

        /// <summary>
        /// 小图片路劲
        /// </summary>
        public const string SPicPath = "/ProductImg/SPic/";
        public static string GetOrderNo()
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

        public static string GetUserNo()
        {
            string result = string.Empty;
            ISession NSession = NHibernateHelper.CreateSession();
            IList<SerialNumberType> list = NSession.CreateQuery(" from SerialNumberType where Code=:p").SetString("p", UserNo).List<SerialNumberType>();
            if (list.Count > 0)
            {
                list[0].BeginNo = list[0].BeginNo + 1;
                NSession.Update(list[0]);
                NSession.Flush();
                return list[0].BeginNo.ToString();
            }
            return "";

        }

        public static void DrawImageRectRect(string rawImgPath, string newImgPath, int width, int height)
        {
            System.Drawing.Image imageFrom = System.Drawing.Image.FromFile(rawImgPath);
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