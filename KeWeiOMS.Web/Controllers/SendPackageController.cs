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
    public class SendPackageController : BaseController
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
        public JsonResult Create(SendPackageType obj)
        {
            try
            {
                obj.CreateBy = GetCurrentAccount().Realname;
                obj.CreateOn = DateTime.Now;

                obj.PackageName = obj.CreateOn.ToString("yyyy-MM-dd") + obj.PackageName;

                object count = NSession.CreateQuery("select count(Id) from SendPackageType where PackageName='" + obj.PackageName + "'").UniqueResult();
                if (Convert.ToInt16(count) > 0)
                {
                    return Json(new { IsSuccess = false, Result = "名称已经存在!" });
                }
                obj.PCount = 0;
                obj.PWeight = 0;

                NSession.SaveOrUpdate(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false });
            }
            return Json(new { IsSuccess = true });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public SendPackageType GetById(int Id)
        {
            SendPackageType obj = NSession.Get<SendPackageType>(Id);
            if (obj == null)
            {
                throw new Exception("返回实体为空");
            }
            else
            {
                return obj;
            }
        }



        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {

            try
            {

                SendPackageType obj = GetById(id);
                NSession.Delete("from SendPackageOrderType where PackId=" + obj.Id);
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
            IList<SendPackageType> objList = NSession.CreateQuery("from SendPackageType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<SendPackageType>();

            object count = NSession.CreateQuery("select count(Id) from SendPackageType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public ActionResult PackageAddOrder(string ids, string p)
        {
            string[] strs = ids.Replace(" ", "").Replace("\r", "").Trim().Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            int c = 0;
            double w = 0;
            string str = ids.Replace(" ", "").Replace("\r", "").Trim().Replace("\n", "','").Replace("''", "");
            IList<OrderType> list = NSession.CreateQuery(" from OrderType where OrderNo in('" + str + "')").List<OrderType>();
            List<SendPackageOrderType> list2 = NSession.CreateQuery(" from SendPackageOrderType where OrderNo in('" + str + "')").List<SendPackageOrderType>().ToList<SendPackageOrderType>();
            SendPackageType package = NSession.Get<SendPackageType>(Convert.ToInt32(p));

            foreach (var item in list)
            {
                if (list2.Find(x => x.OId == item.Id) != null)
                {
                    continue;
                }
                SendPackageOrderType foo = new SendPackageOrderType();
                foo.OId = item.Id;
                foo.OrderNo = item.OrderNo;
                foo.PackId = package.Id;
                foo.PackageName = package.PackageName;
                NSession.Save(foo);
                NSession.Flush();
                c++;
                w += item.Weight;
            }
            package.PCount += c;
            package.PWeight += w;
            NSession.Update(package);
            NSession.Flush();
            return Json(new { IsSuccess = true, c = c, e = list2.Count, t = strs.Length - list.Count });
        }


    }
}

