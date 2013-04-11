using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace KeWeiOMS.Web
{
    /// <summary>
    /// OMSWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class OMSWebService : System.Web.Services.WebService
    {
        public ISession NSession = SessionBuilder.CreateSession();


        [WebMethod]
        public bool HasExisitOrderExNo(string OrderExNo)
        {
            return OrderHelper.IsExist(OrderExNo);
        }

        [WebMethod]
        public string GetOrderNo()
        {
            return Utilities.GetOrderNo();
        }
        [WebMethod]
        public OrderType GetOrder(string OrderExNo)
        {
            IList<OrderType> orderTypes =
                NSession.CreateQuery("from OrderType where OrderExNo='" + OrderExNo + "'").List<OrderType>();
            if (orderTypes.Count > 0)
            {
                return orderTypes[0];
            }
            else
            {
                return null;
            }
        }
        [WebMethod]
        public bool UpdateOrder(OrderType order)
        {
            NSession.Update(order);
            NSession.Flush();
            return false;

        }

        [WebMethod]
        public bool CreateOrder(OrderType order)
        {
            NSession.Save(order.AddressInfo);
            NSession.Flush();
            order.AddressId = order.AddressInfo.Id;
            NSession.Save(order);
            NSession.Flush();
            foreach (var p in order.Products)
            {
                p.OId = order.Id;
                p.OrderNo = order.OrderNo;
                OrderHelper.CreateOrderPruduct(p);

            }
            return true;
        }



        [WebMethod]
        public string GMarket(string ItemId, string ItemTitle, decimal Price, string PicUrl, string Nownum, string ProductUrl, string Account)
        {
            try
            {
                GMarketType obj = new GMarketType();
                obj.ItemId = ItemId;
                obj.ItemTitle = ItemTitle;
                obj.Price = Price;
                obj.PicUrl = PicUrl;
                obj.NowNum = Nownum;
                obj.ProductUrl = ProductUrl;
                obj.Account = Account;
                obj.CreateOn = DateTime.Now;
                int check = GetId(ItemId);
                if (check > 0)
                {
                    obj.Id = check;
                    NSession.Update(obj);
                    NSession.Flush();
                    return "更新一条记录成功";
                }
                NSession.Save(obj);
                NSession.Flush();
            }
            catch (Exception ex)
            {
                return "保存失败";
            }
            return "保存一条记录成功";
        }

        public int GetId(string ItemId)
        {
            IList<GMarketType> list = NSession.CreateQuery("from GMarketType where ItemId='" + ItemId + "'").List<GMarketType>();
            NSession.Clear();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    return item.Id;
                }
            }
            return 0;
        }
    }
}
