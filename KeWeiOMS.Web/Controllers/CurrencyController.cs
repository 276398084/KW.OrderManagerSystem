using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;

namespace KeWeiOMS.Web.Controllers
{
    public class CurrencyController : BaseController
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
        public JsonResult Create(CurrencyType obj)
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
            return Json(new { IsSuccess = true  });
        }

        [HttpPost]
        public JsonResult UpdateRate()
        {
            try
            {
                cn.com.webxml.webservice.ForexRmbRateWebService server = new cn.com.webxml.webservice.ForexRmbRateWebService();
                DataSet ds = server.getForexRmbRate();

                NSession.Delete(" from CurrencyType");
                NSession.Flush();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CurrencyType c = new CurrencyType();
                    c.CurrencyCode = dr["Symbol"].ToString();
                    c.CurrencyName = dr["Name"].ToString();
                    c.CurrencySign = "";
                    string str = dr["fBuyPrice"].ToString();
                    if (string.IsNullOrEmpty(str))
                    {
                        str = "0";
                    }
                    c.CurrencyValue = Math.Round(Convert.ToDouble(str) / 100, 5);
                    c.CreateOn = DateTime.Now; ;
                    NSession.Save(c);
                }
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true  });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CurrencyType GetById(int Id)
        {
            CurrencyType obj = NSession.Get<CurrencyType>(Id);
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
            CurrencyType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(CurrencyType obj)
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
                CurrencyType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true  });
        }

        public JsonResult List(int page, int rows, string sort, string order, string search)
        {
            string orderby = " order by Id desc ";
            string where = "";
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
            IList<CurrencyType> objList = NSession.CreateQuery("from CurrencyType" + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows * page)
                .List<CurrencyType>();
            object count = NSession.CreateQuery("select count(Id) from CurrencyType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public JsonResult QList()
        {

            IList<CurrencyType> objList = NSession.CreateQuery("from CurrencyType")

                .List<CurrencyType>();

            return Json(new { total = objList.Count, rows = objList });
        }

    }
}

