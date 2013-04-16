using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;
using System.Data.SqlClient;
using System.Data;

namespace KeWeiOMS.Web.Controllers
{
    public class NewGuideController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(NewGuideType obj)
        {
            try
            {
                obj.CreateOn = DateTime.Now;
                NSession.SaveOrUpdate(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
        }

        /// <summary>
        /// ����Id��ȡ
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public NewGuideType GetById(int Id)
        {
            NewGuideType obj = NSession.Get<NewGuideType>(Id);
            if (obj == null)
            {
                throw new Exception("出错");
            }
            else
            {
                return obj;
            }
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(int id)
        {
            NewGuideType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(NewGuideType obj)
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
            return Json(new { IsSuccess = true  });

        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {

            try
            {
                NewGuideType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true  });
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Copy(int id)
        {
            NewGuideType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Copy(NewGuideType obj)
        {

            try
            {
                obj.CreateOn = DateTime.Now;
                NSession.Save(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true  });

        }

        //public JsonResult List(int page, int rows, string sort, string order)
        //{
        //    string orderby = " order by Id desc ";
        //    if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
        //    {
        //        orderby = "order by " + sort + " " + order;
        //    }
        //    IList<NewGuideType> objList = NSession.CreateQuery("from NewGuideType " + orderby)
        //        .SetFirstResult(rows * (page - 1))
        //        .SetMaxResults(rows * page)
        //        .List<NewGuideType>();

        //    object count = NSession.CreateQuery("select count(Id) from NewGuideType ").UniqueResult();
        //    return Json(new { total = count, rows = objList });
        //}

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
            Session["ToExcel"] = where + orderby;
            IList<NewGuideType> objList = NSession.CreateQuery("from NewGuideType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<NewGuideType>();

            object count = NSession.CreateQuery("select count(Id) from NewGuideType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public JsonResult ToExcel()
        {
            try
            {
                SqlConnection con = new SqlConnection("server=122.227.207.204;database=KeweiBackUp;uid=sa;pwd=`1q2w3e4r");
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("select * from NewGuide" + Session["ToExcel"].ToString(), con);
                DataSet ds = new DataSet();
                da.Fill(ds, "content");
                con.Close();
                Session["ExportDown"] = ExcelHelper.GetExcelXml(ds);
            }
            catch (Exception ee)
            {
                return Json(new { Msg = "出错了" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Msg = "导出成功" }, JsonRequestBehavior.AllowGet);
        }

    }
}

