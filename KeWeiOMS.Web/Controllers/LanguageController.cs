using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;
using Newtonsoft.Json;

namespace KeWeiOMS.Web.Controllers
{
    [ValidateInput(false)]
    public class LanguageController : BaseController
    {
        #region 页面指向

        public ViewResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(int id)
        {
            LanguageType obj = GetById(id);

            if (obj == null)
            {
                throw new Exception("返回实体为空");
            }
            else
            {
                return View(obj);
            }
        }
        #endregion
        [HttpPost]
        public JsonResult List(int page, int rows, string sort, string order, string search)
        {
            string orderby = Utilities.OrdeerBy(sort, order, "Id desc");
            string where = Utilities.SqlWhere(search);
            //string where = string.IsNullOrWhiteSpace(search) ? string.Empty : " where " + search + " "; //Utilities.SqlWhere(search);
            List<LanguageType> objList =
                NSession.CreateQuery(
                    "from LanguageType " + where + orderby)
                    .SetFirstResult(rows * (page - 1))
                    .SetMaxResults(rows)
                    .List<LanguageType>().ToList();
            object count = NSession.CreateQuery("select count(Id) from LanguageType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        [HttpPost]
        public JsonResult Create(LanguageType obj)
        {
            try
            {
                NSession.SaveOrUpdate(obj);
                NSession.Flush();
                Language.ReLoadLanguage();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
        }
        [HttpPost]
        public ActionResult Edit(LanguageType obj)
        {

            try
            {
                NSession.Clear();
                NSession.Update(obj);
                NSession.Flush();
                Language.ReLoadLanguage();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                LanguageType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
                Language.ReLoadLanguage();
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
        public LanguageType GetById(int Id)
        {
            LanguageType obj = NSession.Get<LanguageType>(Id);
            if (obj == null)
            {
                throw new Exception("返回实体为空");
            }
            else
            {
                return obj;
            }
        }
    }
}