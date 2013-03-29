using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using com.paypal.sdk.profiles;
using System.Data;
using eBay.Service.Call;
using System.Data.SqlClient;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;
using System.Web.Mvc;


namespace KeWeiOMS.Web
{
    public class EbayMessageUtil : Controller
    {
        //
        // GET: /EbayMessageUtil/
        public static ISession NSession = SessionBuilder.CreateSession();

        public ActionResult Index()
        {
            return View();
        }

        public static void syn()
        {
            IList<AccountType> list = NSession.CreateQuery("from AccountType where Platform='Ebay' and AccountName<>'' and ApiToken<>''").List<AccountType>();
            foreach (var item in list)
            {
                DateTime beginDate=DateTime.Now.AddHours(-24);
                DateTime endDate=DateTime.Now.AddHours(1);
                GetEmailByAPI(item, beginDate,endDate);
            }
        }

        public static void GetEmailByAPI(AccountType account, DateTime beginDate, DateTime endDate)
        {
            ApiContext context = AppSettingHelper.GetGenericApiContext("US");
            context.ApiCredential.eBayToken = account.ApiToken;
            GetMemberMessagesCall apicall = new GetMemberMessagesCall(context);
            apicall.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);
            TimeFilter fltr = new TimeFilter();
            fltr.TimeFrom = beginDate;
            fltr.TimeTo = endDate;
            MemberMessageExchangeTypeCollection messages;
            int i = 1;
            do
            {
                apicall.Pagination = new eBay.Service.Core.Soap.PaginationType();
                apicall.Pagination.PageNumber = i;
                apicall.Pagination.EntriesPerPage = 100;
                messages = apicall.GetMemberMessages(fltr, MessageTypeCodeType.All, MessageStatusTypeCodeType.Unanswered);

                for (int k = 0; k < messages.Count; k++)
                {
                    MemberMessageExchangeType mmet = messages[k];
                    EbayMessageType email = new EbayMessageType();

                    email.Body = mmet.Question.Body;
                    email.CreationDate = mmet.CreationDate;
                    email.MessageId = mmet.Question.MessageID;
                    email.MessageStatus = mmet.MessageStatus.ToString();
                    email.MessageType = mmet.Question.MessageType.ToString();
                    email.SenderEmail = mmet.Question.SenderEmail;
                    email.SenderID = mmet.Question.SenderID;
                    email.Subject = mmet.Question.Subject;
                    email.ItemId = mmet.Item.ItemID;
                    email.CreateOn = DateTime.Now;
                    int id = NoExist(email.MessageId);
                    if (id != 0)
                    {
                        email.Id = id;
                        NSession.Update(email);
                        NSession.Flush();
                    }
                    else
                    {
                        NSession.Save(email);
                        NSession.Flush();
                    }
                   
                }
                i++;
            } while (messages != null && messages.Count == 100);
        }

        private static int NoExist(string MessageId)
        {
            int id = 0;
            object obj = NSession.CreateQuery("select count(Id) from EbayMessageType where MessageId='" + MessageId + "'").UniqueResult();
            if (Convert.ToInt32(obj) > 0)
            {

                IList<EbayMessageType> list = NSession.CreateQuery("from EbayMessageType where MessageId='" + MessageId + "'").List<EbayMessageType>();
                foreach (var item in list)
                {
                    id = item.Id;
                }

            }
            return id;

        }

    }
}
