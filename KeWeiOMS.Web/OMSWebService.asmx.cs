﻿using KeWeiOMS.Domain;
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
        public string GMarket(string ItemId, string ItemTitle, decimal Price, string PicUrl, string Nownum,int Qty, string ProductUrl, string Account)
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
                int check = GetId(ItemId,Nownum);
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
            IList<GMarketType> list = NSession.CreateQuery("from GMarketType where ItemId='"+ItemId+"' and NowNum='"+Nownum+"'").List<GMarketType>();
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
        public string EbayMessageDown(string Body, DateTime CreationDate, string MessageID, string Status, string MessageType,string SenderEmail,string SenderID,string Subject,string ItemID,string Shop)
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
            if(psw=="feidujinbostore")
            { 

            IList<AccountType> account = NSession.CreateQuery("from AccountType where Platfrom ='Ebay' and ApiToken <>''").List<AccountType>();
            foreach (var item in account)
            {
                arry.Add(item.ApiToken);
            }
            }
            return arry;
        }
    }
}
