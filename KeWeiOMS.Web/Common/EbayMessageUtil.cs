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


        public ActionResult Index()
        {
            return View();
        }

        public static void syn(ISession NSession)
        {

            IList<AccountType> list = NSession.CreateQuery("from AccountType where Platform='Ebay' and AccountName='jinbostore' and ApiToken<>''").List<AccountType>();
            foreach (var item in list)
            {
                DateTime beginDate = DateTime.Now.AddDays(-30);
                DateTime endDate = DateTime.Now.AddMinutes(1);
                GetEmailByAPI(item, beginDate, endDate, NSession);
            }
        }

        public static void GetEmailByAPI(AccountType account, DateTime beginDate, DateTime endDate, ISession NSession)
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
                    email.MessageStatus = "未回复";
                    email.MessageType = MessageType(mmet.Question.MessageType.ToString());
                    email.SenderEmail = mmet.Question.SenderEmail;
                    email.SenderID = mmet.Question.SenderID;
                    email.Subject = mmet.Question.Subject;
                    if (mmet.Item != null)
                    {
                        email.ItemId = mmet.Item.ItemID;
                    }
                    email.Shop = mmet.Question.RecipientID[0];
                    email.CreateOn = DateTime.Now;
                    email.ReplayOn = Convert.ToDateTime("2000-01-01");
                    if (HasExist(email.MessageId, NSession))
                    {

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

        private static bool HasExist(string MessageId, ISession NSession)
        {
            object obj = NSession.CreateQuery("select count(Id) from EbayMessageType where MessageId='" + MessageId + "'").UniqueResult();
            if (Convert.ToInt32(obj) > 0)
            {
                return true;
            }
            return false;

        }

        public static void Upload(AccountType account, ISession NSession)
        {
            ApiContext context = AppSettingHelper.GetGenericApiContext("US");
            context.ApiCredential.eBayToken = account.ApiToken;
            AddMemberMessageRTQCall addMsgApicall = new AddMemberMessageRTQCall(context);
            ReviseMyMessagesCall revMsgApicall = new ReviseMyMessagesCall(context);
            IList<EbayMessageReType> list = NSession.CreateQuery("from EbayMessageReType where IsUpload<>'1'").List<EbayMessageReType>();
            if (list.Count != 0)
            {
                foreach (var item in list)
                {
                    MemberMessageType mm = new MemberMessageType();
                    mm.SenderID = item.SenderID;
                    mm.SenderEmail = item.SenderEmail;
                    mm.MessageID = item.EbayId;
                    mm.Body = item.BodyRe;
                    mm.ParentMessageID = item.EbayId;

                    addMsgApicall.AddMemberMessageRTQ(item.ItemId, mm);
                    revMsgApicall.ReviseMyMessages(true, false, new StringCollection(new string[] { mm.MessageID }));

                    item.IsUpload = 1;
                }
            }
        }

        public static void uploadsyn(ISession NSession)
        {
            IList<AccountType> list = NSession.CreateQuery("from AccountType where Platform='Ebay' and AccountName='jinbostore' and ApiToken<>''").List<AccountType>();
            foreach (var item in list)
            {
                Upload(item, NSession);
            }
        }

        public static string MessageType(string type)
        {
            string mt = "";
            switch (type)
            {
                case "AskSellerQuestion": mt = "向卖家提问"; break;
                case "ResponseToASQQuestion": mt = "回复已回复的问题"; break;
                case "ContactTransactionPartner": mt = "联系交易伙伴"; break;
            }
            return mt;
        }
    }

}
