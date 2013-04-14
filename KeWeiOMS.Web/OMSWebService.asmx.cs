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
        public string EbayMessageDown(string Body, DateTime CreationDate, string MessageID, string Status, string MessageType, string SenderEmail, string SenderID, string Subject, string ItemID, string Shop)
        {
            try
            {
                EbayMessageType email = new EbayMessageType();
                email.Body = Body;
                email.CreationDate = CreationDate;
                email.MessageId = MessageID;
                email.MessageStatus = Status;
                email.MessageType = MessageType;
                email.SenderEmail = SenderEmail;
                email.SenderID = SenderID;
                email.Subject = Subject;
                email.ItemId = ItemID;
                email.Shop = Shop;
                email.CreateOn = DateTime.Now;
                email.ReplayOn = Convert.ToDateTime("2000-01-01");
                int id = NoExist(email.MessageId);
                if (id != 0)
                {
                    return "该条邮件已同步！";
                }
                else
                {
                    NSession.Save(email);
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
        public ArrayList ApiToken(string psw)
        {

            ArrayList arry = new ArrayList();
            if (psw == "feidujingbostore")
            {
                try
                {
                    IList<AccountType> account = NSession.CreateQuery("from AccountType where Platform ='Ebay' and ApiToken <>''").List<AccountType>();
                    foreach (var item in account)
                    {
                        arry.Add(item.ApiToken);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return arry;
        }

        [WebMethod]
        public string GetTokenByAccount(string account)
        {
            string apitoken = "";
            IList<AccountType> ac = NSession.CreateQuery("from AccountType where AccountName ='account' and ApiToken <>''").List<AccountType>();
            foreach (var item in ac)
            {
                apitoken = item.ApiToken;
            }

            return apitoken;
        }


        [WebMethod]
        public EbayMessageReType GetUnUplod()
        {
            EbayMessageReType e = new EbayMessageReType();
            IList<EbayMessageReType> account = NSession.CreateQuery("from EbayMessageReType where IsUpload <>'1'").List<EbayMessageReType>();
            foreach (var item in account)
            {
                return item;
            }
            return e;
        }

        [WebMethod]
        public int GetCount()
        {
            object count = NSession.CreateQuery("select count(Id) from EbayMessageReType where IsUpload <>'1'").UniqueResult();
            return Convert.ToInt32(count);
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
