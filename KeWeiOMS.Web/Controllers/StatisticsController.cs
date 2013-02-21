using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeWeiOMS.Web.Controllers
{
    public class StatisticsController : BaseController
    {
        public ActionResult OrderCount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult OrderCount(DateTime dt)
        {
            IList<object[]> objs = NSession.CreateQuery("select Account,Count(Id) from OrderType   where CreateOn between '" + dt.ToString("yyyy/MM/dd 00:00:00") + "' and '" + dt.AddDays(1).ToString("yyyy/MM/dd 00:00:00") + "' group by Account").List<object[]>();

            List<OrderCount> list = new List<OrderCount>();
            foreach (object[] objectse in objs)
            {
                OrderCount oc = new OrderCount { Account = objectse[0].ToString(), OCount = Convert.ToInt32(objectse[1]) };
                list.Add(oc);
            }
            return Json(list.OrderByDescending(f => f.OCount));
        }

    }
}
