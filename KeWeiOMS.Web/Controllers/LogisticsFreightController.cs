using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;

namespace KeWeiOMS.Web.Controllers
{
    public class LogisticsFreightController : BaseController
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
        public JsonResult Create(LogisticsFreightType obj)
        {
            try
            {
                NSession.SaveOrUpdate(obj);
                NSession.Flush();
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
        public  LogisticsFreightType GetById(int Id)
        {
            LogisticsFreightType obj = NSession.Get<LogisticsFreightType>(Id);
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
            LogisticsFreightType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(LogisticsFreightType obj)
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
                LogisticsFreightType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }

        public JsonResult List(int page, int rows)
        {
            IList<LogisticsFreightType> objList = NSession.CreateQuery("from LogisticsFreightType")
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<LogisticsFreightType>();
			
            object count = NSession.CreateQuery("select count(Id) from LogisticsFreightType ").UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public ViewResult SetFreight(int id)
        {
            Session["fid"] = id;
            return View();
        }
        public JsonResult GetFreight()
        { 
            IList<LogisticsFreightType> list=NSession.CreateQuery("from LogisticsFreightType c where c.AreaCode=:ad")
                .SetInt32("ad",int.Parse(Session["fid"].ToString()))
                .List<LogisticsFreightType>();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public void SaveFeight(LogisticsFreightType log)
        {
            log.AreaCode =int.Parse(Session["fid"].ToString());
            NSession.SaveOrUpdate(log);
            NSession.Flush();
        }
        public void DelFreight(int id)
        {
            LogisticsFreightType log = GetById(id);
            NSession.Delete(log);
            NSession.Flush();
        }

    }
}

