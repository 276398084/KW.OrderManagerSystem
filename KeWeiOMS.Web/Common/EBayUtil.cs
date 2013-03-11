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


namespace KeWeiOMS.Web
{
    public class EBayUtil
    {
        public const string VERSION = "Version";
        public const string TIME_OUT = "TimeOut";
        public const string ENABLE_METRICS = "EnableMetrics";
        public const string API_SERVER_URL = "Environment.ApiServerUrl";
        public const string EPS_SERVER_URL = "Environment.EpsServerUrl";
        public const string SIGNIN_URL = "Environment.SignInUrl";
        public static ISession NSession = SessionBuilder.CreateSession();

        public static ApiContext GetGenericApiContext(string site)
        {
            ApiContext apiContext = new ApiContext();
            apiContext.Version = System.Configuration.ConfigurationManager.AppSettings.Get(VERSION);
            apiContext.Timeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get(TIME_OUT));
            apiContext.SoapApiServerUrl = System.Configuration.ConfigurationManager.AppSettings.Get(API_SERVER_URL);
            apiContext.EPSServerUrl = System.Configuration.ConfigurationManager.AppSettings.Get(EPS_SERVER_URL);
            apiContext.SignInUrl = System.Configuration.ConfigurationManager.AppSettings.Get(SIGNIN_URL);

            ApiAccount apiAccount = new ApiAccount();
            apiAccount.Developer = System.Configuration.ConfigurationManager.AppSettings["Environment.DevId"];
            apiAccount.Application = System.Configuration.ConfigurationManager.AppSettings["Environment.AppId"];
            apiAccount.Certificate = System.Configuration.ConfigurationManager.AppSettings["Environment.CertId"];

            ApiCredential apiCredential = new ApiCredential();
            apiCredential.ApiAccount = apiAccount;

            apiContext.ApiCredential = apiCredential;

            apiContext.EnableMetrics = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get(ENABLE_METRICS));


            if (!string.IsNullOrEmpty(site))
            {
                apiContext.Site = (SiteCodeType)Enum.Parse(typeof(SiteCodeType), site, true);
            }

            apiContext.RuleName = System.Configuration.ConfigurationManager.AppSettings["RuName"];// EBayPriceChanges.Config.RuName;
            apiContext.RuName = System.Configuration.ConfigurationManager.AppSettings["RuName"];// EBayPriceChanges.Config.RuName;
            return apiContext;
        }


      

//        public static void SetShip()
//        {
//            DataTable dt = db.RunTable<OrderInfo>(@"select b.id,OrderExNo,b.TrackCode ,a.UserNameForm from SP_Orders a 
// left join SP_Package b on a.Id=b.OrderNo
//  where b.[Enabled]=0 and b.TrackCode is not null and b.TrackCode <>'LK'");
//            Dictionary<string, int> sendNum = new Dictionary<string, int>();
//            ApiContext context = GetGenericApiContext("US");

//            foreach (DataRow dr in dt.Rows)
//            {
//                try
//                {
//                    SaleAccount sa = SaleAccount.find("UserName='" + dr["UserNameForm"] + "'").list(1)[0];
//                    if (sendNum.ContainsKey(dr["UserNameForm"].ToString()))
//                    {
//                        sendNum[dr["UserNameForm"].ToString()]++;
//                    }
//                    else
//                    {
//                        sendNum.Add(dr["UserNameForm"].ToString(), 1);
//                    }
//                    context.ApiCredential.eBayToken = sa.ApiToken; ;
//                    eBay.Service.Call.CompleteSaleCall call = null;

//                    call = new eBay.Service.Call.CompleteSaleCall(context);

//                    string orderid = dr["OrderExNo"].ToString();

//                    if (orderid.IndexOf("-") == -1) continue;
//                    call.Shipment = new ShipmentType();
//                    call.Shipment.DeliveryStatus = eBay.Service.Core.Soap.ShipmentDeliveryStatusCodeType.Delivered;
//                    call.Shipment.ShipmentTrackingDetails = new ShipmentTrackingDetailsTypeCollection();
//                    call.Shipment.ShipmentTrackingNumber = dr["TrackCode"].ToString();
//                    call.Shipment.ShippingCarrierUsed = "China post air mail";

//                    call.Shipment.DeliveryDate = DateTime.Now;
//                    call.Shipment.DeliveryDateSpecified = true;
//                    call.Shipment.DeliveryStatus = ShipmentDeliveryStatusCodeType.Delivered;
//                    if (!(dr["Id"] is DBNull))
//                    {
//                        PackageInfo.updateBatch("[Enabled]=1", "Id=" + dr["Id"]);
//                        call.CompleteSale(orderid.Substring(0, orderid.IndexOf("-")), orderid.Substring(orderid.IndexOf("-") + 1), true, true);
//                    }
//                }
//                catch (Exception)
//                {

//                    throw;
//                }
//            }
//            string str = "本次跟踪号上传信息如下：";
//            foreach (string item in sendNum.Keys)
//            {
//                str += "账户：" + item + "    数量：" + sendNum[item] + ".";
//            }
//            str += "                  时间:" + DateTime.Now.ToShortDateString();
//            // SMSUtil.SendSmsAPI("15957489764,15968967876,15958200472", str);
//        }


        public void GetOrderByFile()
        {
        }



        public static void GetMyeBaySelling(AccountType sa)
        {
            if (sa == null) return;
            ApiContext context = EBayUtil.GetGenericApiContext("US");
            context.ApiCredential.eBayToken = sa.ApiToken;
            GetMyeBaySellingCall apicall = new GetMyeBaySellingCall(context);
            apicall.ActiveList = new ItemListCustomizationType();
            int i = 1;
            //EbayItem.deleteBatch("AccountFrom='" + sa.UserName + "' and CompanyId='" + sa.CompanyId + "'");
            do
            {
                apicall.ActiveList.Pagination = new PaginationType();
                apicall.ActiveList.Pagination.EntriesPerPage = 200;
                apicall.ActiveList.Pagination.PageNumber = i;
                apicall.GetMyeBaySelling();
                if (apicall.ActiveListReturn != null && apicall.ActiveListReturn.ItemArray != null && apicall.ActiveListReturn.ItemArray.Count > 0)
                {
                    foreach (ItemType actitem in apicall.ActiveListReturn.ItemArray)
                    {
                        if (actitem.SellingStatus != null)
                        {
                            EbayType ei = new EbayType();
                            ei.ItemId = actitem.ItemID;
                            ei.ItemTitle = actitem.Title;
                            ei.Price = actitem.SellingStatus.CurrentPrice.Value.ToString();
                            ei.Currency = actitem.SellingStatus.CurrentPrice.currencyID.ToString();
                            ei.StartNum = actitem.Quantity;
                            ei.NowNum = actitem.QuantityAvailable;
                            ei.ProductUrl = actitem.ListingDetails.ViewItemURL;
                            if (actitem.PictureDetails != null && actitem.PictureDetails.GalleryURL != null)
                            {
                                ei.PicUrl = actitem.PictureDetails.GalleryURL;
                            }
                            ei.StartTime = actitem.ListingDetails.StartTime;
                            ei.Account = sa.AccountName;
                            if (NoExist(ei.ItemId,ei.Price,ei.NowNum))
                            {
                                ei.CreateOn = DateTime.Now;
                                NSession.Save(ei);
                                NSession.Flush();
                            }
                        }

                    }
                    i++;
                    if (i > apicall.ActiveListReturn.PaginationResult.TotalNumberOfPages)
                    {
                        break;
                    }
                }


            } while (apicall.ActiveListReturn != null && apicall.ActiveListReturn.ItemArray != null && apicall.ActiveListReturn.ItemArray.Count == 200);

        }

        private static bool NoExist(string id,string price,int num)
        {
            object obj = NSession.CreateQuery("select count(Id) from EbayType where ItemId='" + id + "'").UniqueResult();
            if (Convert.ToInt32(obj) > 0)
            { 
                IList<EbayType> ebay = NSession.CreateQuery("from EbayType where ItemId='" + id + "'").List<EbayType>();
                foreach (EbayType item in ebay)
                {
                    item.Price = price;
                    item.NowNum = num;
                    item.CreateOn = DateTime.Now;
                    NSession.Update(item);
                    NSession.Flush();
                }
                return false;
            }
            else
            {
                return true;
            }
            
        }

        public static void syn()
        {
            IList<AccountType> list = NSession.CreateQuery("from AccountType where Platform='Ebay' and AccountName<>'' and ApiToken<>''").List<AccountType>();
            foreach(var item in list)
            {
                GetMyeBaySelling(item);
            }
        }
    }
}