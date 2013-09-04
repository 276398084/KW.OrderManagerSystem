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
    public class EmailTemplateController : BaseController
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
        public JsonResult Create(EmailTemplateType obj)
        {
            try
            {
                obj.Enable = 1;
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
        public EmailTemplateType GetById(int Id)
        {
            EmailTemplateType obj = NSession.Get<EmailTemplateType>(Id);
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
            EmailTemplateType obj = GetById(id);
            return View(obj);
        }

       [HttpPost]
        public ActionResult Up(string ids)
        {
         
           NSession.CreateSQLQuery("update EmailTemplate set Enable=1 where Id in(" + ids + ")").UniqueResult();
           NSession.Flush();
            return Json(new { IsSuccess = true  });
        }
       [HttpPost]
       public ActionResult Down(string ids)
       {
        
           NSession.CreateSQLQuery("update EmailTemplate set Enable=0 where Id in(" + ids + ")").UniqueResult();
           NSession.Flush();
           return Json(new { IsSuccess = true });
       }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(EmailTemplateType obj)
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
                EmailTemplateType obj = GetById(id);
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
            string orderby = Utilities.OrdeerBy(sort, order);
            string where = Utilities.SqlWhere(search);
            IList<EmailTemplateType> objList = NSession.CreateQuery("from EmailTemplateType" + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows * page)
                .List<EmailTemplateType>();
            object count = NSession.CreateQuery("select count(Id) from EmailTemplateType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public ActionResult Details(int id)
        {
            EmailTemplateType obj = GetById(id);
            return View(obj);

        }

    }
}

