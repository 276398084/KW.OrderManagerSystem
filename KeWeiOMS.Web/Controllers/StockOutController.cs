﻿using System;
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
    public class StockOutController : BaseController
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
        public JsonResult Create(StockOutType obj)
        {
            try
            {
                obj.CreateBy = CurrentUser.Realname;
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

        [HttpPost]
        public JsonResult CreateByScan(string o, string w2, int w, string m, string t, int s)
        {
            try
            {
                IList<SKUCodeType> list =
                    NSession.CreateQuery("from SKUCodeType where Code=:p").SetInt32("p", s).List<SKUCodeType>();
                if (list.Count > 0)
                {
                    if (list[0].IsOut == 1 || list[0].IsSend == 1)
                    {

                        return Json(new { IsSuccess = false, Result = "该条码已经出库！" });
                    }
                    Utilities.StockOut(w, list[0].SKU, 1, t, CurrentUser.Realname, m, o);
                    NSession.CreateQuery("update SKUCodeType set IsOut=1,IsSend=1,PeiOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "',SendOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "',OrderNo='扫描出库' where Code=" + s).ExecuteUpdate();
                    return Json(new { IsSuccess = true, Result = "扫描完成！产品：" + list[0].SKU + "已经出库，出数量为1!!" });
                }
                return Json(new { IsSuccess = false, Result = "条码错误！无法找到这个产品" });
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, Result = "出错了" });
            }

        }



        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public StockOutType GetById(int Id)
        {
            StockOutType obj = NSession.Get<StockOutType>(Id);
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
            StockOutType obj = GetById(id);
            ViewData["sku"] = obj.SKU;
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(StockOutType obj)
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
                StockOutType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
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
                string key = search.Substring(search.IndexOf("$") + 1);
                where = Utilities.Resolve(key);
                if (where.Length > 0)
                {
                    where = " where " + where;
                }
                string GetDate = search.Substring(0, search.IndexOf("$"));
                string SearchDate = GetSearch(GetDate);
                if (!string.IsNullOrEmpty(SearchDate))
                {
                    if (string.IsNullOrEmpty(where))
                    {
                        where = " where " + SearchDate;
                    }
                    else
                    {
                        where += " and " + SearchDate;
                    }
                }
            }
            IList<StockOutType> objList = NSession.CreateQuery("from StockOutType" + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<StockOutType>();

            object count = NSession.CreateQuery("select count(Id) from StockOutType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }
        public static string GetSearch(string search)
        {
            string where = "";
            string startdate = search.Substring(0, search.IndexOf("&"));
            string enddate = search.Substring(search.IndexOf("&") + 1);
            if (!string.IsNullOrEmpty(startdate) || !string.IsNullOrEmpty(enddate))
            {
                if (!string.IsNullOrEmpty(startdate))
                    where += "CreateOn >=\'" + Convert.ToDateTime(startdate) + "\'";
                if (!string.IsNullOrEmpty(enddate))
                {
                    if (where != "")
                        where += " and ";
                    where += "CreateOn <\'" + Convert.ToDateTime(enddate).AddDays(1) + "\'";
                }
            }
            return where;
        }

    }
}

