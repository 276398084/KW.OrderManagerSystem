using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;
using NHibernate.Criterion;

namespace KeWeiOMS.Web.Controllers
{
    public class LogisticsAreaCountryController : BaseController
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
        public JsonResult Create(LogisticsAreaCountryType obj)
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
        public LogisticsAreaCountryType GetById(int Id)
        {
            LogisticsAreaCountryType obj = NSession.Get<LogisticsAreaCountryType>(Id);
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
            LogisticsAreaCountryType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(LogisticsAreaCountryType obj)
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
                LogisticsAreaCountryType obj = GetById(id);
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
            IList<LogisticsAreaCountryType> objList = NSession.CreateQuery("from LogisticsAreaCountryType")
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<LogisticsAreaCountryType>();

            object count = NSession.CreateQuery("select count(Id) from LogisticsAreaCountryType ").UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public ViewResult SetCountry(int id)
        {
            Session["cid"] = id;
            return View();
        }
        [HttpPost]
        public JsonResult GetUnCountryByAreaCode(string sort,string order)
        {
            string orderby = "";
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
            {
                orderby = "order by " + sort + " " + order;
            }
            IList<CountryType> list = NSession.CreateQuery("from CountryType c where c.Id not in (select CountryCode from LogisticsAreaCountryType where AreaCode=:cid)"+orderby)
              .SetInt32("cid", int.Parse(Session["cid"].ToString()))
              .List<CountryType>();
            return Json(new { total = list.Count, rows = list });

        }
        [HttpPost]
        public JsonResult GetCountryByAreaCode(string sort, string order)
        {
            string orderby = "";
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
            {
                orderby = "order by " + sort + " " + order;
            }
            IList<CountryType> list = NSession.CreateQuery("from CountryType c where c.Id in (select CountryCode from LogisticsAreaCountryType where AreaCode=:cid)" + orderby)
             .SetInt32("cid", int.Parse(Session["cid"].ToString()))
             .List<CountryType>();
            return Json(new { total = list.Count, rows = list });
        }

        public void DelAreaCountry(int id)
        {   
            LogisticsAreaCountryType logc = new LogisticsAreaCountryType { CountryCode = id.ToString(), AreaCode = int.Parse(Session["cid"].ToString()) };
            IList<LogisticsAreaCountryType> list = NSession.CreateCriteria(typeof(LogisticsAreaCountryType))
                .Add(Example.Create(logc))
                .List<LogisticsAreaCountryType>();
            foreach(LogisticsAreaCountryType item in  list)
            {
                NSession.Delete(item);
                NSession.Flush();
            }
        }
        public void AddAreaCountry(int id)
        {
            int tid = int.Parse(Session["cid"].ToString());
            LogisticsAreaCountryType logcountry = new LogisticsAreaCountryType { CountryCode = id.ToString(), AreaCode = tid };
            NSession.Save(logcountry);
        }
    }
}

