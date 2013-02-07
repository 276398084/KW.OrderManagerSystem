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

        public ActionResult Platform()
        {
            List<object> list = new List<object>();
            foreach (string item in Enum.GetNames(typeof(PlatformEnum)))
            {
                list.Add(new { id = item, text = item });
            }
            return Json(list);
        }
        public ActionResult OrderStatus()
        {
            List<object> list = new List<object>();
            foreach (string item in Enum.GetNames(typeof(OrderStatusEnum)))
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

        public ActionResult PrintSetup(string ids, string type)
        {
            ViewData["ids"] = Session["ids"];
            ViewData["type"] = type;
            return View();

        }

        public ActionResult PostData(string ids)
        {
            Session["ids"] = ids;
            Session["type"] = "ttt";
            return Json(new { IsSuccess = "true" });
        }

        public ContentResult PrintData()
        {
            object obj = Session["data"];
            return Content(obj.ToString(), "text/xml");
        }

        public ActionResult PrintDetail()
        {
            DataSet ds = new DataSet();
            IDbCommand command = NSession.Connection.CreateCommand();
            command.CommandText = "select * from Orders";
            IDataReader reader = command.ExecuteReader();
            DataTable result = new DataTable();
            DataTable schemaTable = reader.GetSchemaTable();
            for (int i = 0; i < schemaTable.Rows.Count; i++)
            {
                result.Columns.Add(schemaTable.Rows[i][0].ToString());
            }
            while (reader.Read())
            {
                int fieldCount = reader.FieldCount;
                object[] values = new Object[fieldCount];
                for (int i = 0; i < fieldCount; i++)
                {
                    values[i] = reader.GetValue(i);
                }
                result.Rows.Add(values);
            }
            ds.Tables.Add(result);
            Session["data"] = ds.GetXml();
            ViewData["Data"] = "/Home/PrintData/";
            ViewData["grf"] = "/Content/grf/p1.grf";
            return View();
        }





    }


}
