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




        public static void EbayUploadTrackCode(string account, KeWeiOMS.Domain.OrderType orderType)
        {
            Dictionary<string, int> sendNum = new Dictionary<string, int>();
            ApiContext context = GetGenericApiContext("US");
            IList<AccountType> accounts =
                NSession.CreateQuery("from AccountType where AccountName='" + account + "'").SetMaxResults(1).
                    List<AccountType>();
            if (accounts.Count > 0)
            {
                context.ApiCredential.eBayToken = accounts[0].ApiToken;
                eBay.Service.Call.CompleteSaleCall call = null;

                call = new eBay.Service.Call.CompleteSaleCall(context);

                string orderid = orderType.OrderExNo;

                if (orderid.IndexOf("-") == -1 || orderType.IsMerger == 1) return;
                ;
                call.Shipment = new ShipmentType();
                call.Shipment.DeliveryStatus = eBay.Service.Core.Soap.ShipmentDeliveryStatusCodeType.Delivered;
                call.Shipment.ShipmentTrackingDetails = new ShipmentTrackingDetailsTypeCollection();
                call.Shipment.ShipmentTrackingNumber = orderType.TrackCode.ToString();
                call.Shipment.ShippingCarrierUsed = "China post air mail";

                call.Shipment.DeliveryDate = DateTime.Now;
                call.Shipment.DeliveryDateSpecified = true;
                call.Shipment.DeliveryStatus = ShipmentDeliveryStatusCodeType.Delivered;
                try
                {
                    call.CompleteSale(orderid.Substring(0, orderid.IndexOf("-")),
                                      orderid.Substring(orderid.IndexOf("-") + 1), true, true);
                }
                catch (Exception)
                {
                    return;
                    ;
                }
            }
        }


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
                            DeleteItem(ei.ItemId);
                            
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
                            ei.Status = "销售中";
                            ei.SKU = "";
                            if (actitem.SKU != null)
                            {
                                ei.SKU = actitem.SKU;
                                if (ei.NowNum==0)
                                {
                                    ei.Status = "卖完";
                                }
                                NSession.Clear();
                                ei.CreateOn = DateTime.Now;
                                NSession.Save(ei);
                                NSession.Flush();
                            }
                            else
                            {
                                foreach (VariationType v in actitem.Variations.Variation)
                                {
                                    NSession.Clear();
                                    ei.SKU = v.SKU;
                                    ei.StartNum = v.Quantity;
                                    ei.NowNum = v.Quantity - v.SellingStatus.QuantitySold;
                                    if (ei.NowNum == 0)
                                    {
                                        ei.Status = "卖完";
                                    }
                                    ei.ItemTitle = v.VariationTitle;

                                    ei.CreateOn = DateTime.Now;
                                    NSession.Save(ei);
                                    NSession.Flush();
                                }


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

        private static void DeleteItem(string id)
        {
            object obj = NSession.Delete(" from EbayType where ItemId='" + id + "'");
            NSession.Flush();


        }

        public static void syn()
        {
            IList<AccountType> list = NSession.CreateQuery("from AccountType where Platform='Ebay' and AccountName<>'' and ApiToken<>''").List<AccountType>();
            foreach (var item in list)
            {
                GetMyeBaySelling(item);
            }
        }
    }
}