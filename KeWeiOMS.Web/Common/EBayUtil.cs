using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using NHibernate.Context;
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

        public static string GetSessionID()
        {
            ApiContext context = AppSettingHelper.GetGenericApiContext("US");
            GetSessionIDCall calla = new GetSessionIDCall(context);
            return calla.GetSessionID(context.RuName);
        }

        public static string GetToKen(string session)
        {
            ApiContext context = AppSettingHelper.GetGenericApiContext("US");
            FetchTokenCall call = new FetchTokenCall(context);
            return call.FetchToken(session);
        }




        public static void EbayUploadTrackCode(string account, KeWeiOMS.Domain.OrderType orderType)
        {
            ISession NSession = NhbHelper.OpenSession();
            Dictionary<string, int> sendNum = new Dictionary<string, int>();
            IList<AccountType> accounts = NSession.CreateQuery("from AccountType where AccountName='" + account + "'").SetMaxResults(1).
                    List<AccountType>();
            if (accounts.Count > 0)
            {

                IList<KeWeiOMS.Domain.OrderType> orderList = new List<KeWeiOMS.Domain.OrderType>();
                if (orderType.IsMerger == 1 || orderType.OrderExNo.IndexOf("|") != -1)
                {
                    orderList = NSession.CreateQuery("from OrderType where MId='" + orderType.Id + "' Or Id ='" + orderType.Id + "'").List<KeWeiOMS.Domain.OrderType>();
                }
                else
                {
                    orderList.Add(orderType);
                }
                ApiContext context = GetGenericApiContext("US");

                context.ApiCredential.eBayToken = accounts[0].ApiToken;
                eBay.Service.Call.CompleteSaleCall call = null;
                string CarrierUsed = "";
                // IList<logisticsSetupType> setups = NSession.CreateQuery("from  logisticsSetupType where LId in (select ParentID from LogisticsModeType where LogisticsCode='" + orderType.LogisticMode + "')").List<logisticsSetupType>();
                //if (setups != null)
                //{
                //    CarrierUsed = setups[0].SetupName;
                //}
                CarrierUsed = "China Post";
                call = new eBay.Service.Call.CompleteSaleCall(context);
                foreach (KeWeiOMS.Domain.OrderType order in orderList)
                {
                    string orderid = "";
                    string itemid = "";

                    if (order.OrderExNo.IndexOf("-") == -1)
                    {
                        orderid = order.OrderExNo;

                        GetOrdersCall apicall = new GetOrdersCall(context);
                        OrderTypeCollection orders = null;
                        try
                        {
                            orders = apicall.GetOrders(new StringCollection { order.OrderExNo });
                        }
                        catch (Exception)
                        {
                            orders = new OrderTypeCollection();

                        }

                        if (orders.Count > 0)
                        {
                            foreach (TransactionType trans in orders[0].TransactionArray)
                            {
                                itemid = trans.Item.ItemID;
                                orderid = trans.TransactionID;
                                call.Shipment = new ShipmentType();
                                call.Shipment.DeliveryStatus = eBay.Service.Core.Soap.ShipmentDeliveryStatusCodeType.Delivered;
                                call.Shipment.ShipmentTrackingDetails = new ShipmentTrackingDetailsTypeCollection();

                                if (orderType.OrderNo == order.TrackCode || order.TrackCode == "" || order.TrackCode == null)
                                {
                                    //call.Shipment.ShipmentTrackingNumber = "";
                                }
                                else
                                {
                                    call.Shipment.ShippingCarrierUsed = CarrierUsed;
                                    call.Shipment.ShipmentTrackingNumber = orderType.TrackCode.ToString();
                                }
                                call.Shipment.DeliveryDate = DateTime.Now;
                                call.Shipment.DeliveryDateSpecified = true;
                                call.Shipment.DeliveryStatus = ShipmentDeliveryStatusCodeType.Delivered;
                                try
                                {
                                    call.CompleteSale(itemid, orderid, true, true);
                                }
                                catch (Exception ex)
                                {
                                    break;
                                }
                                break;

                            }
                        }
                    }
                    else
                    {
                        itemid = order.OrderExNo.Substring(0, order.OrderExNo.IndexOf("-"));
                        orderid = order.OrderExNo.Substring(order.OrderExNo.IndexOf("-") + 1);
                        call.Shipment = new ShipmentType();
                        call.Shipment.DeliveryStatus = eBay.Service.Core.Soap.ShipmentDeliveryStatusCodeType.Delivered;
                        call.Shipment.ShipmentTrackingDetails = new ShipmentTrackingDetailsTypeCollection();
                        if (orderType.OrderNo == order.TrackCode || order.TrackCode == "" || order.TrackCode == null)
                        {
                            //call.Shipment.ShipmentTrackingNumber = "";
                        }
                        else
                        {
                            call.Shipment.ShippingCarrierUsed = CarrierUsed;
                            call.Shipment.ShipmentTrackingNumber = orderType.TrackCode.ToString();
                        }

                        call.Shipment.DeliveryDate = DateTime.Now;
                        call.Shipment.DeliveryDateSpecified = true;
                        call.Shipment.DeliveryStatus = ShipmentDeliveryStatusCodeType.Delivered;
                        try
                        {
                            call.CompleteSale(itemid, orderid, true, true);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        finally
                        {


                        }
                    }
                }
            }
        }


        public void GetOrderByFile()
        {
        }



        public static void GetMyeBaySelling(AccountType sa, ISession NSession)
        {
            if (sa == null) return;
            ApiContext context = EBayUtil.GetGenericApiContext("US");
            context.ApiCredential.eBayToken = sa.ApiToken;
            GetMyeBaySellingCall apicall = new GetMyeBaySellingCall(context);
            apicall.ActiveList = new ItemListCustomizationType();
            int i = 1;
            DeleteALL(sa.AccountName, NSession);

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

                        try
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
                                ei.Status = "销售中";
                                ei.SKU = "";
                                if (actitem.SKU != null)
                                {
                                    ei.SKU = actitem.SKU;
                                    if (ei.NowNum == 0)
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
                                    if (actitem.Variations != null)
                                    {
                                        foreach (VariationType v in actitem.Variations.Variation)
                                        {
                                            NSession.Clear();
                                            ei.SKU = v.SKU;
                                            ei.StartNum = v.Quantity;
                                            ei.NowNum = v.Quantity - v.SellingStatus.QuantitySold;
                                            ei.Status = "销售中";
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
                                    else
                                    {
                                        ei.SKU = "";
                                        if (ei.NowNum == 0)
                                        {
                                            ei.Status = "卖完";
                                        }
                                        NSession.Clear();
                                        ei.CreateOn = DateTime.Now;
                                        NSession.Save(ei);
                                        NSession.Flush();
                                    }

                                }



                            }
                        }
                        catch (Exception)
                        {
                            continue;

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

        private static void DeleteALL(string accountName, ISession NSession)
        {
            object obj = NSession.Delete(" from EbayType where Account='" + accountName + "'");
            NSession.Flush();
        }

        //public static void syn(ISession NSession)
        //{
        //    IList<AccountType> list = NSession.CreateQuery("from AccountType where Platform='Ebay' and AccountName<>'' and ApiToken<>''").List<AccountType>();
        //    foreach (var item in list)
        //    {
        //        GetMyeBaySelling(item, NSession);
        //    }
        //}
    }
}