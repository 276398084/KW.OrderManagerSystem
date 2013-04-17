using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;
using System;
using System.Collections;
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
        public ISession NSession = NhbHelper.GetCurrentSession();


        [WebMethod]
        public bool HasExisitOrderExNo(string OrderExNo)
        {
            return OrderHelper.IsExist(OrderExNo, NSession);
        }

        [WebMethod]
        public string GetOrderNo()
        {
            return Utilities.GetOrderNo(NSession);
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
                OrderHelper.CreateOrderPruduct(p, NSession);

            }
            return true;
        }

        [WebMethod]
        public List<OrderType> GetOrders(string s, DateTime st, DateTime et, string Account, string key)
        {
            if (key == "feidu")
            {
                return NSession.CreateQuery("from OrderType where ScanningOn between '" + st.ToString("yyyy-MM-dd") + "' and '" +
                                       et.ToString("yyyy-MM-dd") + "' and Account='" + Account + "'").List<OrderType>().ToList<OrderType>();

            }
            return null;
        }

        [WebMethod]
        public List<ProductType> GetProducts()
        {
            List<ProductType> productTypes = NSession.CreateQuery(" from ProductType").List<ProductType>().ToList();
            return productTypes;
        }

        [WebMethod]
        public List<CurrencyType> GetCurrencys()
        {
            List<CurrencyType> productTypes = NSession.CreateQuery(" from CurrencyType").List<CurrencyType>().ToList();
            return productTypes;
        }



        [WebMethod]
        public string GMarket(string ItemId, string ItemTitle, decimal Price, string PicUrl, string Nownum, int Qty, string ProductUrl, string Account)
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
                obj.Qty = Qty;
                obj.CreateOn = DateTime.Now;
                int check = GetId(ItemId, Nownum);
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

        public int GetId(string ItemId, string Nownum)
        {
            IList<GMarketType> list = NSession.CreateQuery("from GMarketType where ItemId='" + ItemId + "' and NowNum='" + Nownum + "'").List<GMarketType>();
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



        [WebMethod]
        public string EbayMessageDown(EbayMessageType obj)
        {
            try
            {
                obj.CreateOn = DateTime.Now;
                obj.ReplayOn = Convert.ToDateTime("2000-01-01");
                int id = NoExist(obj.MessageId);
                if (id != 0)
                {
                    return "该条邮件已同步！";
                }
                else
                {
                    NSession.Save(obj);
                    NSession.Flush();
                    return "同步一条邮件！";
                }
            }
            catch (Exception ex)
            {
                return "保存失败";
            }
        }


        private int NoExist(string MessageId)
        {

            int id = 0;
            object obj = NSession.CreateQuery("select count(Id) from EbayMessageType where MessageId='" + MessageId + "'").UniqueResult();
            if (Convert.ToInt32(obj) > 0)
            {

                IList<EbayMessageType> list = NSession.CreateQuery("from EbayMessageType where MessageId='" + MessageId + "'").List<EbayMessageType>();
                NSession.Clear();
                foreach (var item in list)
                {
                    id = item.Id;
                }

            }
            return id;

        }

        [WebMethod]
        public List<AccountType> ApiToken(string psw)
        {
            List<AccountType> account = new List<AccountType>();
            ArrayList arry = new ArrayList();
            if (psw == "feidujingbostore")
            {

                try
                {
                    account = NSession.CreateQuery("from AccountType where Platform ='Ebay' and ApiToken <>''").List<AccountType>().ToList();

                }
                catch (Exception ex)
                {

                }
            }
            return account;
        }

        [WebMethod]
        public AccountType GetTokenByAccount(string account)
        {
            AccountType accountname = new AccountType();
            IList<AccountType> ac = NSession.CreateQuery("from AccountType where AccountName ='" + account + "' and ApiToken <>''").List<AccountType>();
            foreach (var item in ac)
            {
                accountname = item;
            }
            return accountname;
        }

        [WebMethod]
        public List<EbayMessageReType> GetUnUplod()
        {
            List<EbayMessageReType> ebay = new List<EbayMessageReType>();
            List<EbayMessageReType> account = NSession.CreateQuery("from EbayMessageReType where IsUpload <>'1'").List<EbayMessageReType>().ToList();
            foreach (var item in account)
            {
                ebay.Add(item);
            }
            return ebay;
        }

        [WebMethod]
        public string ChangeStatus(EbayMessageReType obj)
        {
            try
            {
                NSession.Update(obj);
                NSession.Flush();
                return "状态修改成功";
            }
            catch (Exception e)
            {
                return "状态修改出错";
            }

        }

    }
}
