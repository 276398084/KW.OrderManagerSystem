using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;
using System.Data;

namespace KeWeiOMS.Web.Controllers
{
    public class AttendController : BaseController
    {

        public ViewResult Index()
        {
            return View();
        }
        public ActionResult IndexShow()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(AttendType obj)
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

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public  AttendType GetById(int Id)
        {
            AttendType obj = NSession.Get<AttendType>(Id);
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
            AttendType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(AttendType obj)
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
                AttendType obj = GetById(id);
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
            string where =" where  RealName=\'"+CurrentUser.Realname+"\' ";
            string orderby = " order by UserCode ,CurrentDate desc ";
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
            {
                orderby = " order by " + sort + " " + order;
            }
            if (!string.IsNullOrEmpty(search))
            {
                where = GetSearch(search);    
            } 
            Session["ToExcel"] = where + orderby;
            IList<AttendType> objList = NSession.CreateQuery("from AttendType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<AttendType>();
            
            object count = NSession.CreateQuery("select count(Id) from AttendType " + where ).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

       //首页显示   ListToday
        public JsonResult ListToday()
        {
            string where = " where CurrentDate=\'" + DateTime.Now.Date + "\' and RealName=\'" + CurrentUser.Realname + "\' ";
            string orderby = " order by CurrentDate desc ";
            IList<AttendType> objList = NSession.CreateQuery("from AttendType " + where + orderby)
                .List<AttendType>();

            return Json(objList,JsonRequestBehavior.AllowGet);
        }

        //签到操作
        public JsonResult AttendOn(int id)
        {
            string ip = GetIP();
            try
            {
                AttendType obj = new AttendType() { CurrentDate = DateTime.Now.Date, UserId = CurrentUser.Id, UserCode = CurrentUser.Code, RealName = CurrentUser.Realname };
                IList<AttendType> list = NSession.CreateQuery("from AttendType").List<AttendType>();
                if (IsOK(ip))
                {
                    foreach (var item in list)
                    {
                        if (item.UserId == obj.UserId)
                        {
                            IList<AttendType> objList = NSession.CreateQuery("from AttendType " + " where UserId=\'" + CurrentUser.Id + "\'  order by CurrentDate desc ")
                            .List<AttendType>();
                            NoAttend(objList[0].CurrentDate);
                            if (item.CurrentDate == obj.CurrentDate)
                            {
                                obj = item;
                            }
                        }
                    }
                    obj.IP = ip;
                }
                else
                    return Json(new { Msg = "请使用公司网络打卡！" }, JsonRequestBehavior.AllowGet);

                switch (id)  
                    {
                        case 0:
                            if (string.IsNullOrEmpty(obj.AMStart)) 
                            { 
                                obj.AMStart = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); 
                            }
                            else
                                return Json(new { Msg = "请不要重复打卡！" }, JsonRequestBehavior.AllowGet);
                            break;
                        case 1:
                            if (string.IsNullOrEmpty(obj.AMEnd))
                            {
                                obj.AMEnd = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else
                                return Json(new { Msg = "请不要重复打卡！" }, JsonRequestBehavior.AllowGet);
                            break;
                        case 2:
                            if (string.IsNullOrEmpty(obj.PMStart))
                            {
                                obj.PMStart = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else
                                return Json(new { Msg = "请不要重复打卡！" }, JsonRequestBehavior.AllowGet);
                            break;
                        case 3:
                            if (string.IsNullOrEmpty(obj.PMEnd))
                            {
                                obj.PMEnd = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else 
                                return Json(new { Msg = "请不要重复打卡！" }, JsonRequestBehavior.AllowGet);
                            break;
                    }
                NSession.SaveOrUpdate(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { Msg = "出错了" },JsonRequestBehavior.AllowGet);
            }
            return Json(new { Msg = "签到成功" }, JsonRequestBehavior.AllowGet);

        }

        public void NoAttend(DateTime LastAttend)
        {
            TimeSpan day = DateTime.Now.Date - LastAttend;
            for (int i = 1; i < day.Days; i++)
            {
                AttendType obj = new AttendType() { CurrentDate = DateTime.Now.Date.AddDays(-i), UserId = CurrentUser.Id, UserCode = CurrentUser.Code, RealName = CurrentUser.Realname };
                NSession.Save(obj);
            }
        }

        //导出为Excel
        public JsonResult ToExcel() 
        {
            IList<AttendType> signout = new List<AttendType>();
            try
            {
                signout = NSession.CreateQuery("from AttendType " + Session["ToExcel"].ToString()).List<AttendType>();
                DataSet ds = ConvertToDataSet<AttendType>(signout);
                Session["ExportDown"] = ExcelHelper.GetExcelXml(ds);
            }
            catch (Exception ee)
            {
                return Json(new { Msg = "出错了" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Msg = "导出成功" }, JsonRequestBehavior.AllowGet);
        }

        //获取IP地址
        public string GetIP()
        {

            string ip = string.Empty;

            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"]))
            ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
            if (string.IsNullOrEmpty(ip))
            ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
             return ip;

        }

        //IP地址进行验证
        public bool IsOK(string ip)
        {
            return true;
            string[] strs = new string[] {"127.0.0.1", "115.238.181.250", "115.238.181.251", "115.238.181.252", "115.238.181.253", "115.238.181.254", "122.227.207.205", "122.227.207.202", "122.227.207.203", "122.227.207.204", "122.227.207.206" };
            for (int i = 0; i < strs.Length; i++)
            {
                if (ip == strs[i])
                    return true;
            }
            return false;
        }

        //获取搜索条件
        public static string GetSearch(string search)
        {
            string where="";
            string startdate = search.Substring(0, search.IndexOf("&")).Replace("&", "").Replace("$", "");
            string enddate = search.Substring(0, search.IndexOf("$")).Substring(search.IndexOf("&")).Replace("&", "").Replace("$", "");
            string key = search.Substring(search.IndexOf("$") + 1).Replace("&", "").Replace("$", "");
            if(!string.IsNullOrEmpty(startdate)||!string.IsNullOrEmpty(enddate)||!string.IsNullOrEmpty(key))
            {
                if (!string.IsNullOrEmpty(startdate))
                    where += "CurrentDate >=\'" + Convert.ToDateTime(startdate) + "\'";
                if (!string.IsNullOrEmpty(enddate))
                {
                    if (where != "")
                        where += " and ";
                    where += "CurrentDate <=\'" + Convert.ToDateTime(enddate) + "\'";
                }
                if (!string.IsNullOrEmpty(key))
                {
                    if (where != "")
                        where += " and ";
                    where += "RealName like\'%" + key + "%\'";
                }
                where = " where " + where;
            }
            return where;
        }

        //IList转DataSet
        public static DataSet ConvertToDataSet<T>(IList<T> list)
        {
            if (list == null || list.Count <= 0)
            {
                return null;
            }

            DataSet ds = new DataSet();
            DataTable dt = new DataTable(typeof(T).Name);
            DataColumn column;
            DataRow row;

            System.Reflection.PropertyInfo[] myPropertyInfo = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (T t in list)
            {
                if (t == null)
                {
                    continue;
                }

                row = dt.NewRow();

                for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
                {
                    System.Reflection.PropertyInfo pi = myPropertyInfo[i];

                    string name = pi.Name;

                    if (dt.Columns[name] == null)
                    {
                        column = new DataColumn(name, pi.PropertyType);
                        dt.Columns.Add(column);
                    }

                    row[name] = pi.GetValue(t, null);
                }

                dt.Rows.Add(row);
            }

            ds.Tables.Add(dt);

            return ds;
        }

    }
}

