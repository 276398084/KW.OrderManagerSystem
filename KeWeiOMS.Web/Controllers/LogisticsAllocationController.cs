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
    public class LogisticsAllocationController : BaseController
    {
        public ViewResult Index(string Id)
        {
            
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(LogisticsAllocationType obj)
        {
            try
            {
                obj.CreateBy = CurrentUser.Realname;
                obj.CreateOn = DateTime.Now;
                if(!string.IsNullOrEmpty(obj.Content))
                {
                    obj.Content = "'" + obj.Content.Replace("\r", "").Replace("\n", ",").Replace(",", "','") + "'";
                }
                switch (obj.AllocationType)
                {
                    case 1:
                        if(obj.NBegin>obj.NEnd)
                        {
                            obj.QuerySql = " Amount between " + obj.NBegin + " and " + obj.NEnd + " ";
                        }
                        break;
                    case 2:
                        obj.QuerySql = " Country in ("+obj.Content+") ";
                        break;
                    case 3:
                        obj.QuerySql= " Id in(select OId from OrderProducts where SKU in (" + obj.Content + ")  ) ";
                        break;
                    default:
                        break;
                        
                }
               
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
        public  LogisticsAllocationType GetById(int Id)
        {
            LogisticsAllocationType obj = NSession.Get<LogisticsAllocationType>(Id);
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
            LogisticsAllocationType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(LogisticsAllocationType obj)
        {
            try
            {
                if (!string.IsNullOrEmpty(obj.Content))
                {
                    obj.Content = "'" + obj.Content.Replace("\r", "").Replace("\n", ",").Replace(",", "','") + "'";
                }
                switch (obj.AllocationType)
                {
                    case 1:
                        if (obj.NBegin > obj.NEnd)
                        {
                            obj.QuerySql = " Amount between " + obj.NBegin + " and " + obj.NEnd + " ";
                        }
                        break;
                    case 2:
                        obj.QuerySql = " Country in (" + obj.Content + ") ";
                        break;
                    case 3:
                        obj.QuerySql = " Id in(select OId from OrderProducts where SKU in (" + obj.Content + ")  ) ";
                        break;
                    default:
                        break;

                }
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
                LogisticsAllocationType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }

        public JsonResult List()
        {
            string orderby = " order by Id desc ";
            
            IList<LogisticsAllocationType> objList = NSession.CreateQuery("from LogisticsAllocationType "  + orderby)
              
                .List<LogisticsAllocationType>();
		    foreach (LogisticsAllocationType logisticsAllocationType in objList)
		    {
		        logisticsAllocationType.Content = null;
		        logisticsAllocationType.QuerySql = null;
		    }

            return Json(new { total = objList.Count, rows = objList });
        }

      

    }
}

