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
    public class MoudleController : BaseController
    {
        protected ISession Session = NHibernateHelper.CreateSession();

        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(ModulesType user)
        {
            try
            {
                Session.SaveOrUpdate(user);
                Session.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { erroeMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ModulesType GetById(string Id)
        {
            ModulesType customer = Session.Get<ModulesType>(Id);
            if (customer == null)
            {
                throw new Exception("返回实体为空");
            }
            else
            {
                return customer;
            }
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(decimal id)
        {
            ModulesType user = GetById(id.ToString());
            return View(user);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(ModulesType user)
        {
            JsonResult json = new JsonResult();
            try
            {
                Session.Update(user);
                Session.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { erroeMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
           
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(decimal id)
        {
            JsonResult json = new JsonResult();
            json.Data = true;
            try
            {
                ModulesType customer = GetById(id.ToString());
                Session.Delete(customer);
                Session.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { erroeMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }

        public JsonResult List(int page, int rows)
        {
            IList<ModulesType> customerList = Session.CreateQuery("from ModulesType")
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows * page)
                .List<ModulesType>();
            return Json(new { total = customerList.Count, rows = customerList });
        }

    }
}
