using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeWeiOMS.NhibernateHelper;
using NHibernate;
using KeWeiOMS.Domain;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Web.UI;
using System.IO;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace KeWeiOMS.Web.Controllers
{
    public class HomeController : BaseController
    {

        public ViewResult Index()
        {
            ViewData["Username"] = CurrentUser.Realname;
            return View();
        }

        //
        // GET: /User/Create
        //
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Default()
        {
            return View();
        }

        public ActionResult Platform(string Id)
        {
            List<object> list = new List<object>();
            foreach (string item in Enum.GetNames(typeof(PlatformEnum)))
            {
                list.Add(new { id = item, text = item });
            }
            return Json(list);
        }

        public ActionResult OrderStatus(string Id)
        {
            List<object> list = new List<object>();
            foreach (string item in Enum.GetNames(typeof(OrderStatusEnum)))
            {
                list.Add(new { id = item, text = item });
            }
            return Json(list);
        }

        public ActionResult PrintCategory()
        {
            List<object> list = new List<object>();
            foreach (string item in Enum.GetNames(typeof(PrintCategoryEnum)))
            {
                list.Add(new { id = item, text = item });
            }
            return Json(list);
        }

        public ActionResult ProductStatus()
        {
            List<object> list = new List<object>();
            foreach (string item in Enum.GetNames(typeof(ProductStatusEnum)))
            {
                list.Add(new { id = item, text = item });
            }
            return Json(list);
        }

        public ActionResult GetCompetence()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SavePic(HttpPostedFileBase fileData)
        {
            if (fileData != null)
            {
                try
                {
                    // 文件上传后的保存路径
                    string filePath;
                    string fileName;
                    string saveName;
                    SaveFile(fileData, out filePath, out fileName, out saveName);
                    filePath = Server.MapPath("~");
                    IList<ProductType> list = NSession.CreateQuery(" from ProductType where SKU='" + fileName + "' ").List<ProductType>();
                    if (list.Count > 0)
                    {
                        list[0].PicUrl = Utilities.BPicPath + list[0].SKU + ".jpg";
                        list[0].SPicUrl = Utilities.SPicPath + list[0].SKU + ".png";
                        Utilities.DrawImageRectRect(saveName, filePath + list[0].PicUrl, 310, 310);
                        Utilities.DrawImageRectRect(saveName, filePath + list[0].SPicUrl, 64, 64);
                        NSession.SaveOrUpdate(list[0]);
                        NSession.Flush();
                    }
                    return Json(new { Success = true, FileName = fileName, SaveName = filePath + saveName });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                return Json(new { Success = false, Message = "请选择要上传的文件！" }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveFile(HttpPostedFileBase fileData)
        {
            if (fileData != null)
            {
                try
                {
                    // 文件上传后的保存路径
                    string filePath;
                    string fileName;
                    string saveName;
                    SaveFile(fileData, out filePath, out fileName, out saveName);

                    return Json(new { Success = true, FileName = fileName, SaveName = saveName });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                return Json(new { Success = false, Message = "请选择要上传的文件！" }, JsonRequestBehavior.AllowGet);
            }
        }

        private void SaveFile(HttpPostedFileBase fileData, out string filePath, out string fileName, out string saveName)
        {
            filePath = Server.MapPath("~/Uploads/");
            filePath += DateTime.Now.ToString("yyyyMMdd") + "/";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            fileName = Path.GetFileName(fileData.FileName);// 原始文件名称
            string fileExtension = Path.GetExtension(fileName); // 文件扩展名
            fileName = Path.GetFileNameWithoutExtension(fileName);
            saveName = filePath + Guid.NewGuid().ToString() + fileExtension; // 保存文件名称
            fileData.SaveAs(saveName);
        }
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult PrintSetup(string ids, string type)
        {
            ViewData["ids"] = Session["ids"];
            return View();

        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult PostData(string ids)
        {
            Session["ids"] = ids;

            return Json(new { IsSuccess = true  });
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult SetPrintData(string m, string r, string d, string t)
        {
            string sql = "";
            sql = @"select (select COUNT(1) from OrderProducts where OrderProducts.OId=O.id) as 'GCount',O.IsPrint as 'PCount' ,O.OrderNo,o.OrderExNo,O.Account,O.Platform,O.Amount,O.CurrencyCode,O.BuyerEmail,O.BuyerName,O.LogisticMode,
O.BuyerMemo,O.SellerMemo,O.Freight,O.Weight,O.Country,OA.Addressee,OA.Street,OA.County,OA.City,OA.Province,
OA.Phone,OA.Tel,OA.PostCode,OA.CountryCode,OP.SKU,OP.Standard,OP.Title,OP.Qty,OP.ExSKU,P.OldSKU,P.Category,P.SPicUrl,
R.RetuanName ,R.City as 'RCity',R.Street as 'RStreet',R.Phone as 'RPhone',R.Tel as 'RTel',R.County as 'RCounty',(select top 1 CCountry from Country where ECountry=O.Country) as CCountry,
R.Country as 'RCountry',R.PostCode as 'RPostCode',R.Province as 'RProvince' from Orders O 
left join OrderProducts OP on o.Id=op.OId
left join OrderAddress OA on o.AddressId=oa.Id
Left Join Products P ON OP.SKU=P.SKU
left join ReturnAddress R On r.Id=" + r;
            sql += " where O.OrderNo IN('" + d.Replace(",", "','") + "')";
            DataSet ds = new DataSet();
            IDbCommand command = NSession.Connection.CreateCommand();
            command.CommandText = sql;
            SqlDataAdapter da = new SqlDataAdapter(command as SqlCommand);



            da.Fill(ds);
            ds.Tables[0].DefaultView.Sort = "OrderNo Asc";
            if (t == "多物品订单")
                ds.Tables[0].DefaultView.RowFilter = " GCount >1";
            DataTable dt = ds.Tables[0].DefaultView.ToTable();
            dt.Columns.Add("PrintName");
            foreach (DataRow dr in dt.Rows)
            {
                dr["PrintName"] = CurrentUser.Realname;
            }
            //标记打印
            NSession.CreateQuery("update OrderType set IsPrint=IsPrint+1 where OrderNo in ('" + d.Replace(",", "','") + "')").ExecuteUpdate();
            ds.Tables.Clear();
            ds.Tables.Add(dt);
            Session["data"] = ds.GetXml();
            return Json(new { IsSuccess = true  });
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ContentResult PrintData()
        {
            object obj = Session["data"];
            //Session["data"] = null;
            return Content(obj.ToString(), "text/xml");
        }

        public ContentResult PrintOrder(int Id)
        {

            string sql = "select * from Orders O left join OrderAddress OA ON O.AddressId=OA.Id  where O.id=" + Id;
            DataSet ds = new DataSet();
            IDbCommand command = NSession.Connection.CreateCommand();
            command.CommandText = sql;
            SqlDataAdapter da = new SqlDataAdapter(command as SqlCommand);
            da.Fill(ds);
            return Content(ds.GetXml(), "text/xml");
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ContentResult PrintGrf(string Id)
        {
            NSession.Clear();
            object obj = NSession.CreateQuery("select Content from PrintTemplateType where Id=" + Id).UniqueResult();
            return Content(obj.ToString(), "text/xml", Encoding.Default);
        }
        [OutputCache(Location = OutputCacheLocation.None)]
        public JsonResult PrintSave(string Id)
        {
            NSession.Clear();
            PrintTemplateType obj = NSession.Get<PrintTemplateType>(Convert.ToInt32(Id));
            byte[] FormData = Request.BinaryRead(Request.TotalBytes);
            obj.Content = System.Text.Encoding.Default.GetString(FormData);
            NSession.Update(obj);
            NSession.Flush();


            return Json(new { IsSuccess = 1 });
        }
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult PrintDesign(string Id)
        {
            ViewData["id"] = Id;

            //object obj = NSession.CreateQuery("select Content from PrintTemplateType where Id=" + Id).UniqueResult();
            return View();
        }
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult PrintDetail(string Id)
        {

            ViewData["grf"] = Id;
            return View();
        }





    }


}
