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
    public class AttendController : BaseController
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
        public JsonResult Create(AttendType obj)
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
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
           
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
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }

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
            IList<AttendType> objList = NSession.CreateQuery("from AttendType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<AttendType>();

            object count = NSession.CreateQuery("select count(Id) from AttendType " + where ).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public JsonResult AttendOn(int id)
        {
            string ip = GetIP();
            try
            {
                AttendType obj = new AttendType() {CurrentDate=DateTime.Now.Date,UserId=CurrentUser.Id,UserCode=CurrentUser.Code,RealName=CurrentUser.Realname };
                IList<AttendType> list = NSession.CreateQuery("from AttendType").List<AttendType>();
                foreach (var item in list)
                {
                    if (item.CurrentDate == obj.CurrentDate && item.UserId == obj.UserId)
                    {
                        obj = item;
                    }
                }
                //if (IsOK(ip))
                //    obj.IP = ip;
                //else
                //    return Json(new { Msg = "不会没起床吧，请使用公司网络打卡哦！" }, JsonRequestBehavior.AllowGet);
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

        public string GetIP()
        {

            string ip = string.Empty;

            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"]))
            ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
            if (string.IsNullOrEmpty(ip))
            ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
             return ip;

        }
        public bool IsOK(string ip)
        {
            string[] strs = new string[] { "115.238.181.255", "115.238.181.251", "115.238.181.252", "115.238.181.253", "115.238.181.254", "122.227.207.205", "122.227.207.202", "122.227.207.203", "122.227.207.204", "122.227.207.206" };
            return true;
        }
    }
}

