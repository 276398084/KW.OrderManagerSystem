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
            return Json(new { IsSuccess = true });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AttendType GetById(int Id)
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
            return Json(new { IsSuccess = true });

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
            return Json(new { IsSuccess = true });
        }

        public JsonResult List(int page, int rows, string search)
        {
            string orderby = " order by CurrentDate desc ";
            string where = Utilities.SqlWhere(search);
            IList<AttendType> objList = NSession.CreateQuery("from AttendType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<AttendType>();

            object count = NSession.CreateQuery("select count(Id) from AttendType " + where).UniqueResult();
            return Json(new { total = count, rows = objList.OrderByDescending(p => p.CurrentDate) });
        }

        //首页显示   ListToday
        public JsonResult ListToday()
        {
            string where = " where CurrentDate=\'" + DateTime.Now.Date + "\' and RealName=\'" + CurrentUser.Realname + "\' ";
            string orderby = " order by CurrentDate desc ";
            IList<AttendType> objList = NSession.CreateQuery("from AttendType " + where + orderby)
                .List<AttendType>();

            return Json(objList, JsonRequestBehavior.AllowGet);
        }

        //签到操作
        public JsonResult AttendOn(int id, string code = "")
        {
            string ip = GetIP();
            DateTime AttentTime = DateTime.Now;
            int userid = CurrentUser.Id;
            string usercode = CurrentUser.Code;
            string realname = CurrentUser.Realname;
            if (code != "")
            {
                IList<UserType> users = NSession.CreateQuery("from UserType where Code='" + code + "'").List<UserType>();
                if (users.Count > 0)
                {
                    userid = users[0].Id;
                    usercode = users[0].Code;
                    realname = users[0].Realname;
                }
                else
                {
                    return Json(new { Msg = code + " 编号不存在！" }, JsonRequestBehavior.AllowGet);
                }
            }
            AttendType obj = new AttendType() { CurrentDate = AttentTime.Date, UserId = userid, UserCode = usercode, RealName = realname };
            try
            {

                IList<AttendType> list = NSession.CreateQuery("from AttendType where UserId='" + obj.UserId + "' and CurrentDate='" + obj.CurrentDate + "'").List<AttendType>();
                if (IsOK(ip))
                {
                    //IList<AttendType> objList = NSession.CreateQuery("from AttendType " + " where UserId=\'" + obj.UserId + "\'  order by CurrentDate desc ")
                    //.List<AttendType>();
                    //if (objList.Count > 0)
                    //{
                    //   //NoAttend(objList[0].CurrentDate);
                    //}
                    if (list.Count > 0)
                    {
                        obj = list[0];
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
                            obj.AMStart = AttentTime.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                            return Json(new { Msg = "请不要重复打卡！" }, JsonRequestBehavior.AllowGet);
                        break;
                    case 1:
                        if (string.IsNullOrEmpty(obj.AMEnd))
                        {
                            obj.AMEnd = AttentTime.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                            return Json(new { Msg = "请不要重复打卡！" }, JsonRequestBehavior.AllowGet);
                        break;
                    case 2:
                        if (string.IsNullOrEmpty(obj.PMStart))
                        {
                            obj.PMStart = AttentTime.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                            return Json(new { Msg = "请不要重复打卡！" }, JsonRequestBehavior.AllowGet);
                        break;
                    case 3:
                        if (string.IsNullOrEmpty(obj.PMEnd))
                        {
                            obj.PMEnd = AttentTime.ToString("yyyy-MM-dd HH:mm:ss");
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
                return Json(new { Msg = "出错了" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Msg = obj.RealName + " 签到成功:" + AttentTime.ToString("yyyy-MM-dd HH:mm:ss") }, JsonRequestBehavior.AllowGet);

        }

        public void NoAttend(DateTime LastAttend)
        {
            TimeSpan day = DateTime.Now.Date - LastAttend;
            for (int i = 1; i < day.Days; i++)
            {
                AttendType obj = new AttendType() { CurrentDate = DateTime.Now.Date.AddDays(-i), UserId = CurrentUser.Id, UserCode = CurrentUser.Code, RealName = CurrentUser.Realname };
                NSession.Save(obj);
                NSession.Flush();
            }
        }

        public JsonResult ToExcel(string search)
        {
            try
            {
                List<AttendType> objList = NSession.CreateQuery("from AttendType " + Utilities.SqlWhere(search))
                    .List<AttendType>().ToList();
                Session["ExportDown"] = ExcelHelper.GetExcelXml(Utilities.FillDataTable((objList)));

            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true, ErrorMsg = "导出成功" });
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
            string[] strs = new string[] { "127.0.0.1", "115.238.181.250", "115.238.181.251", "115.238.181.252", "115.238.181.253", "115.238.181.254", "122.227.207.205", "122.227.207.202", "122.227.207.203", "122.227.207.204", "122.227.207.206" };
            for (int i = 0; i < strs.Length; i++)
            {
                if (ip == strs[i])
                    return true;
            }
            return false;
        }

    }
}

