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

namespace KeWeiOMS.Web.Controllers
{
    public class HomeController : BaseController
    {

        public ViewResult Index()
        {
            //var ss = new ModuleType { FullName = "系统管理", CreateOn = DateTime.Now, CreateBy = "系统管理员" };
            //NSession.Save(ss);
            //var dd = new ModuleType { ParentId = ss.Id, FullName = "菜单管理", NavigateUrl = "/Module/Index", CreateOn = DateTime.Now, CreateBy = "系统管理员" };
            //NSession.Save(dd);
            return View();
        }

        //
        // GET: /User/Create

        public ActionResult Create()
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


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveFile(HttpPostedFileBase fileData)
        {
            if (fileData != null)
            {
                try
                {
                    // 文件上传后的保存路径
                    string filePath = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string fileName = Path.GetFileName(fileData.FileName);// 原始文件名称
                    string fileExtension = Path.GetExtension(fileName); // 文件扩展名
                    string saveName = Guid.NewGuid().ToString() + fileExtension; // 保存文件名称

                    fileData.SaveAs(filePath + saveName);

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







    }


}
