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
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
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
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });

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
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
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
            ViewData["Id"] = id;
            return View();
        }
        [HttpPost]
        public JsonResult GetUnCountryByAreaCode(string sort, string order, int id)
        {
            string orderby = " order by Id desc ";
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
            {
                orderby = "order by " + sort + " " + order;
            }
            IList<object[]> list = NSession.CreateSQLQuery("select Id,CCountry,ECountry,CountryCode,(select AreaName from LogisticsArea where ID=(select top 1 AreaCode from LogisticsAreaCountry la where c.Id= la.CountryCode and la.AreaCode in (select ID from LogisticsArea where LId =(select LId from LogisticsArea where LogisticsArea.Id=:cid)))) as AreaNane from Country c where c.Id not in (select CountryCode from LogisticsAreaCountry where AreaCode=:cid)" + orderby)
              .SetInt32("cid", id)
              .List<object[]>();
            List<CountryType> l = new List<CountryType>();

            foreach (object[] foo in list)
            {
                CountryType c = new CountryType();
                c.Id = Utilities.ToInt(foo[0]);
                c.CCountry = foo[1].ToStr();
                c.ECountry = foo[2].ToStr();
                c.CountryCode = foo[3].ToStr();
                c.AreaName = foo[4].ToStr();
                l.Add(c);
            }

            return Json(new { total = l.Count, rows = l });

        }
        [HttpPost]
        public JsonResult GetCountryByAreaCode(string sort, string order)
        {
            string orderby = " order by Id desc ";
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
            {
                orderby = "order by " + sort + " " + order;
            }
            IList<CountryType> list = NSession.CreateQuery("from CountryType c where c.Id in (select CountryCode from LogisticsAreaCountryType where AreaCode=:cid)" + orderby)
             .SetInt32("cid", int.Parse(Session["cid"].ToString()))
             .List<CountryType>();
            return Json(new { total = list.Count, rows = list });
        }

        public void DelAreaCountry(string id, int tid)
        {
            LogisticsAreaCountryType logc = new LogisticsAreaCountryType { CountryCode = id.ToString(), AreaCode = tid };
            IList<LogisticsAreaCountryType> list = NSession.CreateCriteria(typeof(LogisticsAreaCountryType))
                .Add(Example.Create(logc))
                .List<LogisticsAreaCountryType>();
            foreach (LogisticsAreaCountryType item in list)
            {
                NSession.Delete(item);
                NSession.Flush();
            }
        }
        public void AddAreaCountry(string id, int tid)
        {
            NSession.Delete("from LogisticsAreaCountryType where  CountryCode=" + id +
                            " and AreaCode in (select Id from LogisticsAreaType where LId =(select LId from LogisticsAreaType where Id=" +
                            tid + "))");
            NSession.Flush();
            LogisticsAreaCountryType logcountry = new LogisticsAreaCountryType { CountryCode = id, AreaCode = tid };
            NSession.Save(logcountry);
        }
    }
}

