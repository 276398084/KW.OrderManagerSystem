using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using OrderType = KeWeiOMS.Domain.OrderType;
using ProductType = KeWeiOMS.Domain.ProductType;

namespace KeWeiOMS.Web
{
    public class OrderHelper
    {



        public static DataTable GetDataTable(string fileName)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";" +
            "Extended Properties='Excel 8.0;IMEX=1'";
            DataSet ds = new DataSet();
            OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$]", strConn);
            oada.Fill(ds);
            return ds.Tables[0];
        }

        #region 订单数据导入
        public static List<ResultInfo> ImportByAmount(AccountType account, string fileName)
        {
            ISession NSession = SessionBuilder.CreateSession();
            List<ResultInfo> results = new List<ResultInfo>();
            foreach (DataRow dr in GetDataTable(fileName).Rows)
            {
                NSession.CreateQuery("update OrderType set Amount=:p1 where OrderExNo=:p2").SetDouble("p1", Utilities.ToDouble(dr[1].ToString())).SetString("p2", dr[0].ToString()).ExecuteUpdate();
            }
            return results;

        }

        public static List<ResultInfo> ImportBySMT(AccountType account, string fileName)
        {
            ISession NSession = SessionBuilder.CreateSession();

            List<ResultInfo> results = new List<ResultInfo>();
            foreach (DataRow dr in GetDataTable(fileName).Rows)
            {

                string OrderExNo = dr["订单号"].ToString();
                string o = dr["订单状态"].ToString();
                if (o != "等待您发货")
                {
                    results.Add(GetResult(OrderExNo, "订单已经发货", "导入失败"));
                    continue;
                }

                bool isExist = IsExist(OrderExNo);
                if (isExist)
                {
                    OrderType order = new OrderType { IsMerger = 0, IsOutOfStock = 0, IsRepeat = 0, IsSplit = 0, Status = OrderStatusEnum.待处理.ToString(), IsPrint = 0, CreateOn = DateTime.Now, ScanningOn = DateTime.Now };
                    try
                    {
                        order.OrderNo = Utilities.GetOrderNo();
                        order.CurrencyCode = "USD";
                        order.OrderExNo = OrderExNo;
                        order.Amount = Utilities.ToDouble(dr["订单金额"].ToString());
                        order.BuyerMemo = dr["订单备注"].ToString();
                        order.Country = dr["收货国家"].ToString();
                        order.BuyerName = dr["买家名称"].ToString();
                        order.BuyerEmail = dr["买家邮箱"].ToString();
                        order.TId = "";
                        order.Account = account.AccountName;
                        order.GenerateOn = Convert.ToDateTime(dr["付款时间"]);
                        order.Platform = PlatformEnum.SMT.ToString();
                        //舍弃原来的客户表
                        //下面地址
                        order.AddressId = CreateAddress(dr["收货人名称"].ToString(), dr["地址"].ToString(), dr["城市"].ToString(), dr["州/省"].ToString(), dr["收货国家"].ToString(), dr["收货国家"].ToString(), dr["联系电话"].ToString(), dr["手机"].ToString(), dr["买家邮箱"].ToString(), dr["邮编"].ToString(), 0);


                        NSession.Save(order);
                        NSession.Flush();
                        //
                        //添加产品
                        //
                        string info = dr["产品信息_（双击单元格展开所有产品信息！）"].ToString();
                        string[] cels = info.Split(new char[] { '【' }, StringSplitOptions.RemoveEmptyEntries);
                        if (cels.Length == 0)
                        {
                            results.Add(GetResult(OrderExNo, "没有产品信息", "导入失败"));
                            continue;//物品信息出错
                        }
                        for (int i = 0; i < cels.Length; i++)
                        {
                            string Str = cels[i];
                            System.Text.RegularExpressions.Regex r2 = new System.Text.RegularExpressions.Regex(@"】(?<title>.*)\n", System.Text.RegularExpressions.RegexOptions.None);
                            System.Text.RegularExpressions.Regex r4 = new System.Text.RegularExpressions.Regex(@"\(产品属性:(?<ppp>.*)\n", System.Text.RegularExpressions.RegexOptions.None);
                            System.Text.RegularExpressions.Regex r5 = new System.Text.RegularExpressions.Regex(@"\(产品数量:(?<quantity>\d+)", System.Text.RegularExpressions.RegexOptions.None);
                            System.Text.RegularExpressions.Regex r3 = new System.Text.RegularExpressions.Regex(@"\(商家编码:(?<sku>.*)\)", System.Text.RegularExpressions.RegexOptions.None);
                            // System.Text.RegularExpressions.Regex r6 = new System.Text.RegularExpressions.Regex(@"\(物流等级&买家选择物流:(?<wuliu>.+)\)", System.Text.RegularExpressions.RegexOptions.None);
                            System.Text.RegularExpressions.Match mc2 = r2.Match(Str);
                            System.Text.RegularExpressions.Match mc3 = r3.Match(Str);
                            System.Text.RegularExpressions.Match mc4 = r4.Match(Str);
                            System.Text.RegularExpressions.Match mc5 = r5.Match(Str);

                            order.LogisticMode = dr["买家选择物流"].ToString();
                            if (order.LogisticMode.IndexOf("\n") != -1)
                            {
                                order.LogisticMode = order.LogisticMode.Substring(0, order.LogisticMode.IndexOf("\n"));
                            }
                            NSession.Update(order);
                            NSession.Flush();
                            CreateOrderPruduct(mc3.Groups["sku"].Value, Utilities.ToInt(mc5.Groups["quantity"].Value.Trim(')').Trim()), mc2.Groups["title"].Value, mc4.Groups["ppp"].Value.Replace("(产品属性: ", "").Replace(")", ""), 0, "", order.Id, order.OrderNo);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    results.Add(GetResult(OrderExNo, "", "导入成功"));
                }
                else
                {
                    results.Add(GetResult(OrderExNo, "订单已存在", "导入失败"));
                }
            }
            return results;

        }

        public static List<ResultInfo> ImportByAmazon(AccountType account, string fileName)
        {
            ISession NSession = SessionBuilder.CreateSession();
            List<ResultInfo> results = new List<ResultInfo>();
            CsvReader csv = new CsvReader(fileName, Encoding.Default);

            List<Dictionary<string, string>> lsitss = csv.ReadAllData();

            foreach (Dictionary<string, string> item in lsitss)
            {
                if (item.Count < 10)//判断列数
                    continue;
                string OrderExNo = item["order-id"];
                bool isExist = IsExist(OrderExNo);
                if (isExist)
                {
                    OrderType order = new OrderType { IsMerger = 0, IsOutOfStock = 0, IsRepeat = 0, IsSplit = 0, Status = OrderStatusEnum.待处理.ToString(), IsPrint = 0, CreateOn = DateTime.Now, ScanningOn = DateTime.Now };
                    order.OrderNo = Utilities.GetOrderNo();
                    // order.CurrencyCode = "USD";
                    order.OrderExNo = item["order-id"];
                    //order.Amount =Utilities.ToDouble(dr["订单金额"]);
                    // order.BuyerMemo = dr["订单备注"].ToString();
                    order.Country = item["ship-country"];
                    order.BuyerName = item["buyer-name"];
                    order.BuyerEmail = item["buyer-email"];
                    order.TId = "";
                    order.Account = account.AccountName;
                    order.GenerateOn = Convert.ToDateTime(item["payments-date"]);
                    order.Platform = PlatformEnum.Amazon.ToString();

                    order.AddressId = CreateAddress(item["recipient-name"], item["ship-address-1"] + item["ship-address-2"] + item["ship-address-3"], item["ship-city"], item["ship-state"], item["ship-country"], item["ship-country"], item["buyer-phone-number"], item["buyer-phone-number"], item["buyer-email"], item["ship-postal-code"], 0);
                    NSession.Save(order);
                    NSession.Flush();
                    CreateOrderPruduct(item["sku"], Utilities.ToInt(item["quantity-purchased"]), item["sku"], "", 0, "", order.Id, order.OrderNo);
                    results.Add(GetResult(OrderExNo, "", "导入成功"));
                }
                else
                {
                    results.Add(GetResult(OrderExNo, "订单已存在", "导入失败"));
                }
            }
            return results;
        }

        public static List<ResultInfo> ImportByGmarket(AccountType account, string fileName)
        {
            ISession NSession = SessionBuilder.CreateSession();
            List<ResultInfo> results = new List<ResultInfo>();
            CsvReader csv = new CsvReader(fileName, Encoding.Default);

            List<Dictionary<string, string>> lsitss = csv.ReadAllData();
            Dictionary<string, int> listOrder = new Dictionary<string, int>();
            foreach (Dictionary<string, string> item in lsitss)
            {
                try
                {
                    if (item.Count < 10)//判断列数
                        continue;
                    string OrderExNo = item["Cart no."];
                    if (listOrder.ContainsKey(OrderExNo))
                    {
                        CreateOrderPruduct(item["Item code"], item["Option Code"], Utilities.ToInt(item["Qty."]), item["Item"], item["Options"], 0, "", listOrder[OrderExNo], OrderExNo);
                        continue;

                    }
                    bool isExist = IsExist(OrderExNo);
                    if (isExist)
                    {
                        OrderType order = new OrderType { IsMerger = 0, IsOutOfStock = 0, IsRepeat = 0, IsSplit = 0, Status = OrderStatusEnum.待处理.ToString(), IsPrint = 0, CreateOn = DateTime.Now, ScanningOn = DateTime.Now };
                        order.OrderNo = Utilities.GetOrderNo();
                        order.CurrencyCode = item["Currency"];
                        order.OrderExNo = OrderExNo;
                        order.Amount = Utilities.ToDouble(item["Total Price"]);
                        order.BuyerMemo = item["Memo to Seller"];
                        order.Country = item["Nation"];
                        order.BuyerName = item["Customer"];
                        order.BuyerEmail = "";
                        order.TId = item["Order no."];
                        order.Account = account.AccountName;
                        order.GenerateOn = Convert.ToDateTime(item["Payment Complete"]);
                        order.Platform = PlatformEnum.Gmarket.ToString();
                        order.BuyerMemo = item["Memo to Seller"] + item["Options"];
                        order.AddressId = CreateAddress(item["Recipient"], item["Address"], "", "", item["Nation"], item["Nation"], item["Recipient Phone number"], item["Recipient mobile Phone number"], "", item["Postal code"], 0);
                        NSession.Save(order);
                        NSession.Flush();
                        CreateOrderPruduct(item["Item code"], item["Option Code"], Utilities.ToInt(item["Qty."]), item["Item"], item["Options"], 0, "", order.Id, order.OrderNo);
                        results.Add(GetResult(OrderExNo, "", "导入成功"));
                        listOrder.Add(OrderExNo, order.Id);
                    }
                    else
                    {
                        results.Add(GetResult(OrderExNo, "订单已存在", "导入失败"));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw ex;
                }
            }
            return results;
        }


        public static List<ResultInfo> ImportByB2C(AccountType account, string fileName)
        {
            ISession NSession = SessionBuilder.CreateSession();
            List<ResultInfo> results = new List<ResultInfo>();
            foreach (DataRow item in OrderHelper.GetDataTable(fileName).Rows)
            {
                try
                {


                    string OrderExNo = item["订单编号"].ToString();
                    bool isExist = IsExist(OrderExNo);
                    if (isExist)
                    {
                        OrderType order = new OrderType { IsMerger = 0, IsOutOfStock = 0, IsRepeat = 0, IsSplit = 0, Status = OrderStatusEnum.待处理.ToString(), IsPrint = 0, CreateOn = DateTime.Now, ScanningOn = DateTime.Now };
                        order.OrderNo = Utilities.GetOrderNo();
                        order.OrderExNo = OrderExNo;
                        order.Amount = Utilities.ToDouble(item["金额"].ToString());
                        order.Country = item["国家"].ToString();
                        order.BuyerName = item["用户名"].ToString();
                        order.CurrencyCode = "USD";
                        order.Account = account.AccountName;
                        order.GenerateOn = DateTime.Now;
                        order.Platform = PlatformEnum.B2C.ToString();

                        order.AddressId = CreateAddress(item["收件人"].ToString(), item["街道"].ToString(), item["城市"].ToString(), item["省"].ToString(), item["国家"].ToString(), item["国家"].ToString(), item["电话"].ToString(), item["电话"].ToString(), item["邮箱"].ToString(), item["邮编"].ToString(), 0);

                        NSession.Save(order);
                        NSession.Flush();


                        CreateOrderPruduct(item["商品"].ToString(), item["商品"].ToString(), Utilities.ToInt(item["数量"].ToString()), "", "", 0, item["属性"].ToString(), order.Id, order.OrderNo);
                        results.Add(GetResult(OrderExNo, "", "导入成功"));


                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw ex;
                }
            }
            return results;
        }
        #endregion

        #region 订单数据ＡＰＩ同步
        public static List<ResultInfo> APIByB2C(AccountType account, DateTime st, DateTime et)
        {
            List<ResultInfo> results = new List<ResultInfo>();
            ISession NSession = SessionBuilder.CreateSession();
            string s = DownHtml("http://www.gamesalor.com/GetOrdersHandler.ashx?startTime=" + st.ToShortDateString() + "&endTime=" + et.ToShortDateString() + "", System.Text.Encoding.UTF8);
            System.Collections.Generic.List<Order> orders = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<Order>>(s);
            foreach (Order foo in orders)
            {
                try
                {
                    bool isExist = IsExist(foo.GoodsDataWare.ItemNumber);
                    if (isExist)
                    {
                        OrderType order = new OrderType
                                              {
                                                  IsMerger = 0,
                                                  IsOutOfStock = 0,
                                                  IsRepeat = 0,
                                                  IsSplit = 0,
                                                  Status = OrderStatusEnum.待处理.ToString(),
                                                  IsPrint = 0,
                                                  CreateOn = DateTime.Now,
                                                  ScanningOn = DateTime.Now
                                              };
                        order.OrderNo = Utilities.GetOrderNo();
                        order.CurrencyCode = foo.GoodsDataWare.McCurrency;
                        order.OrderExNo = foo.GoodsDataWare.ItemNumber;
                        order.Amount = Utilities.ToDouble(foo.GoodsDataWare.McGross.ToString());
                        order.BuyerMemo = "客户付款账户" + foo.GoodsDataWare.Business + "  " + foo.GoodsDataWare.Memo;
                        order.Country = foo.GoodsDataWare.AddressCountry;
                        order.BuyerName = foo.GoodsDataWare.FirstName + " " + foo.GoodsDataWare.LastName;
                        order.BuyerEmail = foo.GoodsDataWare.PayerEmail;
                        order.TId = foo.GoodsDataWare.TxnId;
                        order.Account = account.AccountName;
                        order.GenerateOn = foo.GoodsDataWare.PaymentDate;
                        order.Platform = PlatformEnum.B2C.ToString();
                        order.LogisticMode = foo.GoodsDataWare.EMS;
                        order.AddressId = CreateAddress(foo.GoodsDataWare.AddressName, foo.GoodsDataWare.AddressStreet,
                                                        foo.GoodsDataWare.AddressCity, foo.GoodsDataWare.AddressState,
                                                        foo.GoodsDataWare.AddressCountry,
                                                        foo.GoodsDataWare.AddressCountryCode, foo.GoodsDataWare.ContactPhone,
                                                        foo.GoodsDataWare.ContactPhone, foo.GoodsDataWare.PayerEmail,
                                                        foo.GoodsDataWare.AddressZip, 0);
                        NSession.Save(order);
                        NSession.Flush();
                        foreach (GoodsDataOrder item in foo.GoodsDataOrderList)
                        {
                            CreateOrderPruduct(item.ItemID, item.Quantity, item.ItemID, "", 0, item.Url, order.Id,
                                               order.OrderNo);
                        }
                        //  results.Add(GetResult(OrderExNo, "", "导入成功"));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw ex;
                }
            }
            return results;
        }

        public static List<ResultInfo> APIByAmazon(AccountType account, DateTime st, DateTime et)
        {
            ISession NSession = SessionBuilder.CreateSession();
            List<ResultInfo> results = new List<ResultInfo>();
            return results;
        }

        public static List<ResultInfo> APIBySMT(AccountType account, DateTime st, DateTime et)
        {
            ISession NSession = SessionBuilder.CreateSession();
            List<ResultInfo> results = new List<ResultInfo>();
            return results;
        }

        public static List<ResultInfo> APIByEbay(AccountType account, DateTime st, DateTime et)
        {
            ISession NSession = SessionBuilder.CreateSession();
            List<ResultInfo> results = new List<ResultInfo>();
            ApiContext context = AppSettingHelper.GetGenericApiContext("US");
            context.ApiCredential.eBayToken = account.ApiToken;
            eBay.Service.Call.GetOrdersCall apicall = new eBay.Service.Call.GetOrdersCall(context);
            apicall.IncludeFinalValueFee = true;
            apicall.DetailLevelList.Add(eBay.Service.Core.Soap.DetailLevelCodeType.ReturnAll);
            eBay.Service.Core.Soap.OrderTypeCollection m = null;
            int i = 1;
            do
            {
                apicall.Pagination = new eBay.Service.Core.Soap.PaginationType();
                apicall.Pagination.PageNumber = i;
                apicall.Pagination.EntriesPerPage = 50;
                apicall.OrderRole = eBay.Service.Core.Soap.TradingRoleCodeType.Seller;
                apicall.OrderStatus = eBay.Service.Core.Soap.OrderStatusCodeType.Completed;
                apicall.ModTimeFrom = st;
                apicall.ModTimeTo = et;
                apicall.Execute();
                m = apicall.OrderList;
                for (int k = 0; k < m.Count; k++)
                {
                    eBay.Service.Core.Soap.OrderType ot = m[k];
                    if (ot.OrderStatus == eBay.Service.Core.Soap.OrderStatusCodeType.Authenticated ||
                        ot.OrderStatus == eBay.Service.Core.Soap.OrderStatusCodeType.CustomCode ||
                        ot.OrderStatus == eBay.Service.Core.Soap.OrderStatusCodeType.Default ||
                        ot.OrderStatus == eBay.Service.Core.Soap.OrderStatusCodeType.Inactive ||
                        ot.OrderStatus == eBay.Service.Core.Soap.OrderStatusCodeType.InProcess)
                    {
                        //去除别的订单状态
                        continue;
                    }
                    if (ot.PaidTime == DateTime.MinValue || ot.ShippedTime != DateTime.MinValue)
                    {
                        continue;
                    }
                    //查看是不是在订单系统里面存在
                    bool isExist = IsExist(ot.OrderID);
                    if (isExist)
                    {
                        OrderType order = new OrderType
                                              {
                                                  IsMerger = 0,
                                                  IsOutOfStock = 0,
                                                  IsRepeat = 0,
                                                  IsSplit = 0,
                                                  Status = OrderStatusEnum.待处理.ToString(),
                                                  IsPrint = 0,
                                                  CreateOn = DateTime.Now,
                                                  ScanningOn = DateTime.Now
                                              };
                        order.OrderNo = Utilities.GetOrderNo();
                        order.CurrencyCode = ot.AmountPaid.currencyID.ToString();
                        order.OrderExNo = ot.OrderID;
                        order.Amount = ot.AmountPaid.Value;
                        // order.BuyerMemo = dr["订单备注"].ToString();
                        order.Country = ot.ShippingAddress.CountryName;
                        order.BuyerName = ot.BuyerUserID;
                        order.BuyerEmail = ot.TransactionArray[0].Buyer.Email;
                        order.BuyerMemo = ot.BuyerCheckoutMessage;

                        order.TId = ot.ExternalTransaction[0].ExternalTransactionID;
                        order.Account = account.AccountName;
                        order.GenerateOn = ot.PaidTime;
                        order.Platform = PlatformEnum.Ebay.ToString();

                        order.AddressId = CreateAddress(ot.ShippingAddress.Name,
                                                        (string.IsNullOrEmpty(ot.ShippingAddress.Street)
                                                             ? ""
                                                             : ot.ShippingAddress.Street) +
                                                        (string.IsNullOrEmpty(ot.ShippingAddress.Street1)
                                                             ? ""
                                                             : ot.ShippingAddress.Street1) +
                                                        (string.IsNullOrEmpty(ot.ShippingAddress.Street2)
                                                             ? ""
                                                             : ot.ShippingAddress.Street2),
                                                        ot.ShippingAddress.CityName, ot.ShippingAddress.StateOrProvince,
                                                        ot.ShippingAddress.CountryName,
                                                        ot.ShippingAddress.Country.ToString(), ot.ShippingAddress.Phone,
                                                        ot.ShippingAddress.Phone, ot.TransactionArray[0].Buyer.Email,
                                                        ot.ShippingAddress.PostalCode, 0);
                        NSession.Save(order);
                        NSession.Flush();
                        foreach (TransactionType item in ot.TransactionArray)
                        {
                            string sku = "";
                            if (item.Variation != null)
                            {
                                sku = item.Variation.SKU;
                            }
                            else
                            {
                                sku = item.Item.SKU;
                            }

                            CreateOrderPruduct(item.Item.ItemID, sku, item.QuantityPurchased, item.Item.Title,
                                               "", 0,
                                               "http://thumbs.ebaystatic.com/pict/" + item.Item.ItemID + "6464_1.jpg",
                                               order.Id,
                                               order.OrderNo);
                        }

                    }
                }
                i++;
            } while (apicall.HasMoreOrders);

            return results;
        }
        #endregion

        #region 创建客户数据
        /// <summary>
        /// 创建客户数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="amount"></param>
        /// <param name="buyOn"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        public static int CreateBuyer(string name, string email, double amount, DateTime buyOn, PlatformEnum platform)
        {
            ISession NSession = SessionBuilder.CreateSession();
            IList<OrderBuyerType> list = NSession.CreateQuery(" from OrderBuyerType where BuyerName=:p and Platform=:p2").SetString("p", name).SetString("p2", platform.ToString()).List<OrderBuyerType>();
            OrderBuyerType buyer = new OrderBuyerType();
            if (list.Count > 0)
            {
                buyer = list[0];
                buyer.BuyCount += 1;
                buyer.BuyAmount += amount;
                buyer.ListBuyOn = buyOn;
            }
            else
            {
                buyer = new OrderBuyerType();
                buyer.BuyerName = name;
                buyer.BuyerEmail = email;
                buyer.FristBuyOn = buyOn;
                buyer.BuyCount = 1;
                buyer.BuyAmount = amount;
                buyer.ListBuyOn = buyOn;
                buyer.Platform = platform.ToString();
            }
            NSession.SaveOrUpdate(buyer);
            NSession.Flush();
            return buyer.Id;
        }
        #endregion

        #region 创建订单发货地址

        /// <summary>
        /// 创建订单发货地址 
        /// </summary>
        /// <param name="addressee"></param>
        /// <param name="street"></param>
        /// <param name="city"></param>
        /// <param name="province"></param>
        /// <param name="country"></param>
        /// <param name="countryCode"></param>
        /// <param name="tel"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="postcode"></param>
        /// <param name="buyerID"></param>
        /// <returns></returns>
        public static int CreateAddress(string addressee, string street, string city, string province, string country, string countryCode, string tel, string phone, string email, string postcode, int buyerID)
        {
            try
            {
                ISession NSession = SessionBuilder.CreateSession();
                OrderAddressType address = new OrderAddressType();
                address.Street = street;
                address.Tel = tel;
                address.City = city;
                address.Province = province;
                address.PostCode = postcode;
                address.Email = email;
                address.Country = country;
                address.CountryCode = countryCode;
                address.Phone = phone;
                address.Addressee = addressee;
                address.BId = buyerID;
                NSession.Save(address);
                NSession.Flush();
                return address.Id;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region 创建订单产品
        /// <summary>
        /// 创建订单产品
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="qty"></param>
        /// <param name="name"></param>
        /// <param name="remark"></param>
        /// <param name="price"></param>
        /// <param name="url"></param>
        /// <param name="oid"></param>
        /// <param name="orderNo"></param>
        public static void CreateOrderPruduct(string sku, int qty, string name, string remark, double price, string url, int oid, string orderNo)
        {
            CreateOrderPruduct(sku, sku, qty, name, remark, price, url, oid, orderNo);
        }

        /// <summary>
        /// 创建订单产品
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="qty"></param>
        /// <param name="name"></param>
        /// <param name="remark"></param>
        /// <param name="price"></param>
        /// <param name="url"></param>
        /// <param name="oid"></param>
        /// <param name="orderNo"></param>
        public static void CreateOrderPruduct(string exSKU, string sku, int qty, string name, string remark, double price, string url, int oid, string orderNo)
        {
            OrderProductType product = new OrderProductType();
            product.ExSKU = exSKU;
            product.SKU = sku;
            product.Qty = qty;
            product.Price = price;
            product.Title = name;
            product.Url = url;
            product.OId = oid;
            product.OrderNo = orderNo;
            product.Remark = remark;
            CreateOrderPruduct(product);

        }

        public static void CreateOrderPruduct(OrderProductType product)
        {
            ISession NSession = SessionBuilder.CreateSession();
            if (product.SKU == null)
                product.SKU = "";
            if (product.SKU.IndexOf("+") != -1)
            {
                foreach (string fo in product.SKU.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    product.SKU = fo;
                    GetItem(product);
                    NSession.Save(product);
                    NSession.Flush();
                }
            }
            else
            {
                GetItem(product);
                NSession.Save(product);
                NSession.Flush();
            }
        }

        public static void GetItem(OrderProductType item)
        {
            ISession NSession = SessionBuilder.CreateSession();
            if (item.SKU.IndexOf(":") != -1)
            {
                System.Text.RegularExpressions.Regex r1 = new System.Text.RegularExpressions.Regex(@":D(?<num>\d+)", System.Text.RegularExpressions.RegexOptions.None);
                System.Text.RegularExpressions.Regex r2 = new System.Text.RegularExpressions.Regex(@":A(?<desc>.+)", System.Text.RegularExpressions.RegexOptions.None);
                System.Text.RegularExpressions.Match mc1 = r1.Match(item.SKU);
                System.Text.RegularExpressions.Match mc2 = r2.Match(item.SKU);
                string[] cels = item.SKU.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                item.SKU = cels[0];
                int fo = Utilities.ToInt(mc1.Groups["num"].ToString());
                if (fo != 0)
                    item.Qty = fo * item.Qty;
                try
                {
                    item.Remark += mc2.Groups["desc"].Value;
                }
                catch (Exception)
                {
                    item.Remark = "";
                }
            }

            IList<ProductType> ps = NSession.CreateQuery("from ProductType where sku='" + item.SKU + "'").List<ProductType>();
            if (ps.Count > 0)
            {
                item.Standard = ps[0].Standard;
            }
        }

        #endregion

        #region 是否存在订单 public static bool IsExist(string OrderExNo)
        public static bool IsExist(string OrderExNo)
        {
            ISession NSession = SessionBuilder.CreateSession();
            object obj = NSession.CreateQuery("select count(Id) from OrderType where OrderExNo=:p").SetString("p", OrderExNo).UniqueResult();
            if (Convert.ToInt32(obj) == 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 获得返回的数据
        public static ResultInfo GetResult(string key, string info, string result, string field1, string field2, string field3, string field4)
        {
            ResultInfo r = new ResultInfo();
            r.Field1 = field1;
            r.Field2 = field2;
            r.Field3 = field3;
            r.Field4 = field4;
            r.Key = key;
            r.Info = info;
            r.Result = result;
            return r;
        }

        public static ResultInfo GetResult(string key, string info, string result)
        {
            return GetResult(key, info, result, "", "", "", "");
        }
        #endregion

        #region 订单验证


        public static bool ValiOrder(OrderType order, List<CountryType> countrys, List<ProductType> products, List<CurrencyType> currencys, List<LogisticsModeType> logistics)
        {
            ISession NSession = SessionBuilder.CreateSession();
            bool resultValue = true;
            order.ErrorInfo = "";

            if (order.Country != null)
            {
                if (
                    countrys.FindIndex(
                        p => p.ECountry == order.Country || p.CountryCode.ToUpper() == order.Country.ToUpper()) == -1)
                {
                    resultValue = false;
                    order.ErrorInfo += "国家不符 ";
                }
            }
            else
            {
                resultValue = false;
                order.ErrorInfo += "国家不符 ";
            }
            if (order.CurrencyCode != null)
            {
                if (currencys.FindIndex(p => p.CurrencyCode.ToUpper() == order.CurrencyCode.ToUpper()) == -1)
                {
                    resultValue = false;
                    order.ErrorInfo += "货币不符 ";
                }

            }
            else
            {
                resultValue = false;
                order.ErrorInfo += "货币不符 ";
            }

            if (logistics.FindIndex(p => p.LogisticsCode == order.LogisticMode) == -1)
            {
                resultValue = false;
                order.ErrorInfo += "货运不符 ";
            }
            order.Products = NSession.CreateQuery("from OrderProductType where OId='" + order.Id + "'").List<OrderProductType>();
            foreach (var item in order.Products)
            {
                if (item.SKU == null)
                {
                    resultValue = false;
                    order.ErrorInfo += "SKU不符";
                    break;
                }
                item.SKU = item.SKU.Trim();
                NSession.SaveOrUpdate(item);
                NSession.Flush();
                if (products.FindIndex(p => p.SKU.Trim().ToUpper() == item.SKU.Trim().ToUpper()) == -1)
                {
                    resultValue = false;
                    order.ErrorInfo += "SKU不符";
                    break;
                }
            }
            NSession.Clear();
            if (resultValue)
            {
                order.Status = OrderStatusEnum.已处理.ToString();
            }
            NSession.SaveOrUpdate(order);
            NSession.Flush();
            return resultValue;


        }
        #endregion

        #region 订单4项属性替换
        public static bool ReplaceBySKU(string ids, string oldValue, string newValue)
        {
            ISession NSession = SessionBuilder.CreateSession();
            string sql = "";
            if (!string.IsNullOrEmpty(ids))
            {
                ids = " and OId in(" + ids + ") ";
            }
            if (!string.IsNullOrEmpty(oldValue))
            {
                oldValue = " and SKU='" + oldValue + "' ";
            }

            sql = "update OrderProductType set SKU='{0}' where 1=1  {1} {2}";
            sql = string.Format(sql, newValue, oldValue, ids);
            IQuery Query = NSession.CreateQuery(sql);
            return Query.ExecuteUpdate() > 0;

        }

        public static bool ReplaceByCountry(string ids, string oldValue, string newValue)
        {
            ISession NSession = SessionBuilder.CreateSession();
            string sql = "";
            if (!string.IsNullOrEmpty(ids))
            {
                ids = "  Id in(" + ids + ")";

            }
            if (!string.IsNullOrEmpty(oldValue))
            {
                oldValue = " and Country='" + oldValue + "' ";
            }

            sql = "update OrderAddressType set Country='{0}',CountryCode='{0}' where 1=1 and Id in(select AddressId from OrderType where  {2}  )  {1}";
            sql = string.Format(sql, newValue, oldValue, ids);
            IQuery Query = NSession.CreateQuery(sql);
            Query.ExecuteUpdate();
            sql = "update OrderType set Country='{0}' where 1=1 and {1} {2}";
            sql = string.Format(sql, newValue, oldValue, ids.Replace("OId", "Id"));
            Query = NSession.CreateQuery(sql);
            Query.ExecuteUpdate();

            return true;
        }

        public static bool ReplaceByCurrencyOrLogistic(string ids, string oldValue, string newValue, int type)
        {
            ISession NSession = SessionBuilder.CreateSession();
            string sql = "";
            if (!string.IsNullOrEmpty(ids))
            {
                ids = "  Id in(" + ids + ")  ";

            }


            if (type == 1)
                sql = "update OrderType set CurrencyCode='{0}' where  {1}";
            else
                sql = "update OrderType set LogisticMode='{0}' where  {1}";

            sql = string.Format(sql, newValue, ids);
            IQuery Query = NSession.CreateQuery(sql);
            return Query.ExecuteUpdate() > 0;

        }
        #endregion

        public static Dictionary<string, string> GetDic(string fileName)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            CsvReader csv = new CsvReader(fileName);
            List<string[]> list = csv.ReadAllRow();
            foreach (var item in list)
            {
                if (item.Length == 2)
                {
                    dic.Add(item[0].Trim(), item[1].Trim());
                }
            }
            return dic;
        }

        public static string DownHtml(string Url, Encoding myEncoding)
        {
            try
            {
                WebClient client = new WebClient();
                StreamReader readerOfStream = new StreamReader(client.OpenRead(Url), myEncoding);
                return readerOfStream.ReadToEnd(); ;
            }
            catch
            {
                return string.Empty;
            }
        }

    }
    #region

    public class Order
    {
        public Order() { }
        public Order(GoodsDataWare goodsDataWare, List<GoodsDataOrder> goodsDataOrderList)
        {
            this.GoodsDataOrderList = goodsDataOrderList;
            this.GoodsDataWare = goodsDataWare;
        }

        private GoodsDataWare goodsDataWare = new GoodsDataWare();
        public GoodsDataWare GoodsDataWare
        {
            get { return goodsDataWare; }
            set { goodsDataWare = value; }
        }
        private List<GoodsDataOrder> goodsDataOrderList = new List<GoodsDataOrder>();

        public List<GoodsDataOrder> GoodsDataOrderList
        {
            get { return goodsDataOrderList; }
            set { goodsDataOrderList = value; }
        }
    }

    public class GoodsDataWare
    {

        /// <summary>
        /// 商品编号
        /// </summary>
        private string _id = string.Empty;

        /// <summary>
        /// 错误信息
        /// </summary>
        private string _errinfo = string.Empty;

        /// <summary>
        /// TotalDiscount
        /// </summary>
        private decimal _totaldiscount = new System.Decimal();

        /// <summary>
        /// TotalCost
        /// </summary>
        private decimal _totalcost = new System.Decimal();

        /// <summary>
        /// 数量
        /// </summary>
        private int _quantity = new System.Int32();

        /// <summary>
        /// 日期
        /// </summary>
        private System.DateTime _updatetime = System.Convert.ToDateTime("1800-1-1");

        /// <summary>
        /// EMS
        /// </summary>
        private string _ems = string.Empty;

        /// <summary>
        /// 是否已发货
        /// </summary>
        private bool _issend = new System.Boolean();

        /// <summary>
        /// 发货日期
        /// </summary>
        private System.DateTime _senddatatime = new System.DateTime();

        /// <summary>
        /// 保护资格
        /// </summary>
        private string _protectioneligibility = string.Empty;

        /// <summary>
        /// 地址状态
        /// </summary>
        private string _addressstatus = string.Empty;

        /// <summary>
        /// 付款人标识
        /// </summary>
        private string _payerid = string.Empty;

        /// <summary>
        /// 地址街
        /// </summary>
        private string _addressstreet = string.Empty;

        /// <summary>
        /// 付款状态
        /// </summary>
        private string _paymentstatus = string.Empty;

        /// <summary>
        /// 字符集
        /// </summary>
        private string _charset = string.Empty;

        /// <summary>
        /// 地址邮编
        /// </summary>
        private string _addresszip = string.Empty;

        /// <summary>
        /// 名字
        /// </summary>
        private string _firstname = string.Empty;

        /// <summary>
        /// 地址国家代码
        /// </summary>
        private string _addresscountrycode = string.Empty;

        /// <summary>
        /// 地址名称
        /// </summary>
        private string _addressname = string.Empty;

        /// <summary>
        /// 自定义
        /// </summary>
        private string _custom = string.Empty;

        /// <summary>
        /// 付款人状态
        /// </summary>
        private string _payerstatus = string.Empty;

        /// <summary>
        /// 商务
        /// </summary>
        private string _business = string.Empty;

        /// <summary>
        /// 地址国家
        /// </summary>
        private string _addresscountry = string.Empty;

        /// <summary>
        /// 地址城市
        /// </summary>
        private string _addresscity = string.Empty;

        /// <summary>
        /// 验证
        /// </summary>
        private string _verifysign = string.Empty;

        /// <summary>
        /// 付款人电邮
        /// </summary>
        private string _payeremail = string.Empty;

        /// <summary>
        /// 事务处理标识
        /// </summary>
        private string _txnid = string.Empty;

        /// <summary>
        /// 付款方式
        /// </summary>
        private string _paymenttype = string.Empty;

        /// <summary>
        /// 付款人的营业名称
        /// </summary>
        private string _payerbusinessname = string.Empty;

        /// <summary>
        /// 姓
        /// </summary>
        private string _lastname = string.Empty;

        /// <summary>
        /// 地址状态
        /// </summary>
        private string _addressstate = string.Empty;

        /// <summary>
        /// 接收电子邮件
        /// </summary>
        private string _receiveremail = string.Empty;

        /// <summary>
        /// 接收器标识
        /// </summary>
        private string _receiverid = string.Empty;

        /// <summary>
        /// 事务处理类型
        /// </summary>
        private string _txntype = string.Empty;

        /// <summary>
        /// 项目名称
        /// </summary>
        private string _itemname = string.Empty;

        /// <summary>
        /// 货币
        /// </summary>
        private string _mccurrency = string.Empty;

        /// <summary>
        /// 项目编号
        /// </summary>
        private string _itemnumber = string.Empty;

        /// <summary>
        /// 居住国家
        /// </summary>
        private string _residencecountry = string.Empty;

        /// <summary>
        /// 交易标题
        /// </summary>
        private string _transactionsubject = string.Empty;

        /// <summary>
        /// NotifyVersion
        /// </summary>
        private string _notifyversion = string.Empty;

        /// <summary>
        /// 备注
        /// </summary>
        private string _memo = string.Empty;

        /// <summary>
        /// 联系电话
        /// </summary>
        private string _contactphone = string.Empty;

        /// <summary>
        /// Options
        /// </summary>
        private string _options = string.Empty;

        /// <summary>
        /// ReasonCode
        /// </summary>
        private string _reasoncode = string.Empty;

        /// <summary>
        /// ParentTxnId
        /// </summary>
        private string _parenttxnid = string.Empty;

        /// <summary>
        /// McGross
        /// </summary>
        private decimal _mcgross = new System.Decimal();

        /// <summary>
        /// 税
        /// </summary>
        private decimal _tax = new System.Decimal();

        /// <summary>
        /// McFee
        /// </summary>
        private decimal _mcfee = new System.Decimal();

        /// <summary>
        /// 数量
        /// </summary>
        private int _mcquantity = new System.Int32();

        /// <summary>
        /// PaymentFee
        /// </summary>
        private decimal _paymentfee = new System.Decimal();

        /// <summary>
        /// HandlingAmount
        /// </summary>
        private decimal _handlingamount = new System.Decimal();

        /// <summary>
        /// PaymentGross
        /// </summary>
        private decimal _paymentgross = new System.Decimal();

        /// <summary>
        /// 航运
        /// </summary>
        private decimal _shipping = new System.Decimal();

        /// <summary>
        /// 03:07:29 Nov 02, 2010 PDT
        /// </summary>
        private System.DateTime _paymentdate = new System.DateTime();

        /// <summary>
        /// 类 GoodsDataWare 的默认构造函数
        /// </summary>
        public GoodsDataWare() { }

        /// <summary>
        /// 类GoodsDataWare 的构造函数
        /// </summary>
        /// <param name="ErrInfo">错误信息</param>
        /// <param name="TotalDiscount">TotalDiscount</param>
        /// <param name="TotalCost">TotalCost</param>
        /// <param name="Quantity">数量</param>
        /// <param name="UpdateTime">日期</param>
        /// <param name="EMS">EMS</param>
        /// <param name="IsSend">是否已发货</param>
        /// <param name="SendDataTime">发货日期</param>
        /// <param name="ProtectionEligibility">保护资格</param>
        /// <param name="AddressStatus">地址状态</param>
        /// <param name="PayerId">付款人标识</param>
        /// <param name="AddressStreet">地址街</param>
        /// <param name="PaymentStatus">付款状态</param>
        /// <param name="Charset">字符集</param>
        /// <param name="AddressZip">地址邮编</param>
        /// <param name="FirstName">名字</param>
        /// <param name="AddressCountryCode">地址国家代码</param>
        /// <param name="AddressName">地址名称</param>
        /// <param name="Custom">自定义</param>
        /// <param name="PayerStatus">付款人状态</param>
        /// <param name="Business">商务</param>
        /// <param name="AddressCountry">地址国家</param>
        /// <param name="AddressCity">地址城市</param>
        /// <param name="VerifySign">验证</param>
        /// <param name="PayerEmail">付款人电邮</param>
        /// <param name="TxnId">事务处理标识</param>
        /// <param name="PaymentType">付款方式</param>
        /// <param name="PayerBusinessName">付款人的营业名称</param>
        /// <param name="LastName">姓</param>
        /// <param name="AddressState">地址状态</param>
        /// <param name="ReceiverEmail">接收电子邮件</param>
        /// <param name="ReceiverId">接收器标识</param>
        /// <param name="TxnType">事务处理类型</param>
        /// <param name="ItemName">项目名称</param>
        /// <param name="McCurrency">货币</param>
        /// <param name="ItemNumber">项目编号</param>
        /// <param name="ResidenceCountry">居住国家</param>
        /// <param name="TransactionSubject">交易标题</param>
        /// <param name="NotifyVersion">NotifyVersion</param>
        /// <param name="Memo">备注</param>
        /// <param name="ContactPhone">联系电话</param>
        /// <param name="Options">Options</param>
        /// <param name="ReasonCode">ReasonCode</param>
        /// <param name="ParentTxnId">ParentTxnId</param>
        /// <param name="McGross">McGross</param>
        /// <param name="Tax">税</param>
        /// <param name="McFee">McFee</param>
        /// <param name="McQuantity">数量</param>
        /// <param name="PaymentFee">PaymentFee</param>
        /// <param name="HandlingAmount">HandlingAmount</param>
        /// <param name="PaymentGross">PaymentGross</param>
        /// <param name="Shipping">航运</param>
        /// <param name="PaymentDate">03:07:29 Nov 02, 2010 PDT</param>
        public GoodsDataWare(
                    string ErrInfo,
                    decimal TotalDiscount,
                    decimal TotalCost,
                    int Quantity,
                    System.DateTime UpdateTime,
                    string EMS,
                    bool IsSend,
                    System.DateTime SendDataTime,
                    string ProtectionEligibility,
                    string AddressStatus,
                    string PayerId,
                    string AddressStreet,
                    string PaymentStatus,
                    string Charset,
                    string AddressZip,
                    string FirstName,
                    string AddressCountryCode,
                    string AddressName,
                    string Custom,
                    string PayerStatus,
                    string Business,
                    string AddressCountry,
                    string AddressCity,
                    string VerifySign,
                    string PayerEmail,
                    string TxnId,
                    string PaymentType,
                    string PayerBusinessName,
                    string LastName,
                    string AddressState,
                    string ReceiverEmail,
                    string ReceiverId,
                    string TxnType,
                    string ItemName,
                    string McCurrency,
                    string ItemNumber,
                    string ResidenceCountry,
                    string TransactionSubject,
                    string NotifyVersion,
                    string Memo,
                    string ContactPhone,
                    string Options,
                    string ReasonCode,
                    string ParentTxnId,
                    decimal McGross,
                    decimal Tax,
                    decimal McFee,
                    int McQuantity,
                    decimal PaymentFee,
                    decimal HandlingAmount,
                    decimal PaymentGross,
                    decimal Shipping,
                    System.DateTime PaymentDate) :
            this()
        {
            this._errinfo = ErrInfo;
            this._totaldiscount = TotalDiscount;
            this._totalcost = TotalCost;
            this._quantity = Quantity;
            this._updatetime = UpdateTime;
            this._ems = EMS;
            this._issend = IsSend;
            this._senddatatime = SendDataTime;
            this._protectioneligibility = ProtectionEligibility;
            this._addressstatus = AddressStatus;
            this._payerid = PayerId;
            this._addressstreet = AddressStreet;
            this._paymentstatus = PaymentStatus;
            this._charset = Charset;
            this._addresszip = AddressZip;
            this._firstname = FirstName;
            this._addresscountrycode = AddressCountryCode;
            this._addressname = AddressName;
            this._custom = Custom;
            this._payerstatus = PayerStatus;
            this._business = Business;
            this._addresscountry = AddressCountry;
            this._addresscity = AddressCity;
            this._verifysign = VerifySign;
            this._payeremail = PayerEmail;
            this._txnid = TxnId;
            this._paymenttype = PaymentType;
            this._payerbusinessname = PayerBusinessName;
            this._lastname = LastName;
            this._addressstate = AddressState;
            this._receiveremail = ReceiverEmail;
            this._receiverid = ReceiverId;
            this._txntype = TxnType;
            this._itemname = ItemName;
            this._mccurrency = McCurrency;
            this._itemnumber = ItemNumber;
            this._residencecountry = ResidenceCountry;
            this._transactionsubject = TransactionSubject;
            this._notifyversion = NotifyVersion;
            this._memo = Memo;
            this._contactphone = ContactPhone;
            this._options = Options;
            this._reasoncode = ReasonCode;
            this._parenttxnid = ParentTxnId;
            this._mcgross = McGross;
            this._tax = Tax;
            this._mcfee = McFee;
            this._mcquantity = McQuantity;
            this._paymentfee = PaymentFee;
            this._handlingamount = HandlingAmount;
            this._paymentgross = PaymentGross;
            this._shipping = Shipping;
            this._paymentdate = PaymentDate;
        }

        /// <summary>
        /// 类GoodsDataWare 的构造函数
        /// </summary>
        /// <param name="Id">商品编号</param>
        /// <param name="ErrInfo">错误信息</param>
        /// <param name="TotalDiscount">TotalDiscount</param>
        /// <param name="TotalCost">TotalCost</param>
        /// <param name="Quantity">数量</param>
        /// <param name="UpdateTime">日期</param>
        /// <param name="EMS">EMS</param>
        /// <param name="IsSend">是否已发货</param>
        /// <param name="SendDataTime">发货日期</param>
        /// <param name="ProtectionEligibility">保护资格</param>
        /// <param name="AddressStatus">地址状态</param>
        /// <param name="PayerId">付款人标识</param>
        /// <param name="AddressStreet">地址街</param>
        /// <param name="PaymentStatus">付款状态</param>
        /// <param name="Charset">字符集</param>
        /// <param name="AddressZip">地址邮编</param>
        /// <param name="FirstName">名字</param>
        /// <param name="AddressCountryCode">地址国家代码</param>
        /// <param name="AddressName">地址名称</param>
        /// <param name="Custom">自定义</param>
        /// <param name="PayerStatus">付款人状态</param>
        /// <param name="Business">商务</param>
        /// <param name="AddressCountry">地址国家</param>
        /// <param name="AddressCity">地址城市</param>
        /// <param name="VerifySign">验证</param>
        /// <param name="PayerEmail">付款人电邮</param>
        /// <param name="TxnId">事务处理标识</param>
        /// <param name="PaymentType">付款方式</param>
        /// <param name="PayerBusinessName">付款人的营业名称</param>
        /// <param name="LastName">姓</param>
        /// <param name="AddressState">地址状态</param>
        /// <param name="ReceiverEmail">接收电子邮件</param>
        /// <param name="ReceiverId">接收器标识</param>
        /// <param name="TxnType">事务处理类型</param>
        /// <param name="ItemName">项目名称</param>
        /// <param name="McCurrency">货币</param>
        /// <param name="ItemNumber">项目编号</param>
        /// <param name="ResidenceCountry">居住国家</param>
        /// <param name="TransactionSubject">交易标题</param>
        /// <param name="NotifyVersion">NotifyVersion</param>
        /// <param name="Memo">备注</param>
        /// <param name="ContactPhone">联系电话</param>
        /// <param name="Options">Options</param>
        /// <param name="ReasonCode">ReasonCode</param>
        /// <param name="ParentTxnId">ParentTxnId</param>
        /// <param name="McGross">McGross</param>
        /// <param name="Tax">税</param>
        /// <param name="McFee">McFee</param>
        /// <param name="McQuantity">数量</param>
        /// <param name="PaymentFee">PaymentFee</param>
        /// <param name="HandlingAmount">HandlingAmount</param>
        /// <param name="PaymentGross">PaymentGross</param>
        /// <param name="Shipping">航运</param>
        /// <param name="PaymentDate">03:07:29 Nov 02, 2010 PDT</param>
        public GoodsDataWare(
                    string Id,
                    string ErrInfo,
                    decimal TotalDiscount,
                    decimal TotalCost,
                    int Quantity,
                    System.DateTime UpdateTime,
                    string EMS,
                    bool IsSend,
                    System.DateTime SendDataTime,
                    string ProtectionEligibility,
                    string AddressStatus,
                    string PayerId,
                    string AddressStreet,
                    string PaymentStatus,
                    string Charset,
                    string AddressZip,
                    string FirstName,
                    string AddressCountryCode,
                    string AddressName,
                    string Custom,
                    string PayerStatus,
                    string Business,
                    string AddressCountry,
                    string AddressCity,
                    string VerifySign,
                    string PayerEmail,
                    string TxnId,
                    string PaymentType,
                    string PayerBusinessName,
                    string LastName,
                    string AddressState,
                    string ReceiverEmail,
                    string ReceiverId,
                    string TxnType,
                    string ItemName,
                    string McCurrency,
                    string ItemNumber,
                    string ResidenceCountry,
                    string TransactionSubject,
                    string NotifyVersion,
                    string Memo,
                    string ContactPhone,
                    string Options,
                    string ReasonCode,
                    string ParentTxnId,
                    decimal McGross,
                    decimal Tax,
                    decimal McFee,
                    int McQuantity,
                    decimal PaymentFee,
                    decimal HandlingAmount,
                    decimal PaymentGross,
                    decimal Shipping,
                    System.DateTime PaymentDate) :
            this()
        {
            this._id = Id;
            this._errinfo = ErrInfo;
            this._totaldiscount = TotalDiscount;
            this._totalcost = TotalCost;
            this._quantity = Quantity;
            this._updatetime = UpdateTime;
            this._ems = EMS;
            this._issend = IsSend;
            this._senddatatime = SendDataTime;
            this._protectioneligibility = ProtectionEligibility;
            this._addressstatus = AddressStatus;
            this._payerid = PayerId;
            this._addressstreet = AddressStreet;
            this._paymentstatus = PaymentStatus;
            this._charset = Charset;
            this._addresszip = AddressZip;
            this._firstname = FirstName;
            this._addresscountrycode = AddressCountryCode;
            this._addressname = AddressName;
            this._custom = Custom;
            this._payerstatus = PayerStatus;
            this._business = Business;
            this._addresscountry = AddressCountry;
            this._addresscity = AddressCity;
            this._verifysign = VerifySign;
            this._payeremail = PayerEmail;
            this._txnid = TxnId;
            this._paymenttype = PaymentType;
            this._payerbusinessname = PayerBusinessName;
            this._lastname = LastName;
            this._addressstate = AddressState;
            this._receiveremail = ReceiverEmail;
            this._receiverid = ReceiverId;
            this._txntype = TxnType;
            this._itemname = ItemName;
            this._mccurrency = McCurrency;
            this._itemnumber = ItemNumber;
            this._residencecountry = ResidenceCountry;
            this._transactionsubject = TransactionSubject;
            this._notifyversion = NotifyVersion;
            this._memo = Memo;
            this._contactphone = ContactPhone;
            this._options = Options;
            this._reasoncode = ReasonCode;
            this._parenttxnid = ParentTxnId;
            this._mcgross = McGross;
            this._tax = Tax;
            this._mcfee = McFee;
            this._mcquantity = McQuantity;
            this._paymentfee = PaymentFee;
            this._handlingamount = HandlingAmount;
            this._paymentgross = PaymentGross;
            this._shipping = Shipping;
            this._paymentdate = PaymentDate;
        }

        /// <summary>
        /// 商品编号
        /// </summary>
        [System.ComponentModel.DataObjectFieldAttribute(true, true)]
        public string Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrInfo
        {
            get
            {
                return this._errinfo;
            }
            set
            {
                this._errinfo = value;
            }
        }

        /// <summary>
        /// TotalDiscount
        /// </summary>
        public decimal TotalDiscount
        {
            get
            {
                return this._totaldiscount;
            }
            set
            {
                this._totaldiscount = value;
            }
        }

        /// <summary>
        /// TotalCost
        /// </summary>
        public decimal TotalCost
        {
            get
            {
                return this._totalcost;
            }
            set
            {
                this._totalcost = value;
            }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity
        {
            get
            {
                return this._quantity;
            }
            set
            {
                this._quantity = value;
            }
        }

        /// <summary>
        /// 日期
        /// </summary>
        public System.DateTime UpdateTime
        {
            get
            {
                return this._updatetime;
            }
            set
            {
                this._updatetime = value;
            }
        }

        /// <summary>
        /// EMS
        /// </summary>
        public string EMS
        {
            get
            {
                return this._ems;
            }
            set
            {
                this._ems = value;
            }
        }

        /// <summary>
        /// 是否已发货
        /// </summary>
        public bool IsSend
        {
            get
            {
                return this._issend;
            }
            set
            {
                this._issend = value;
            }
        }

        /// <summary>
        /// 发货日期
        /// </summary>
        public System.DateTime SendDataTime
        {
            get
            {
                return this._senddatatime;
            }
            set
            {
                this._senddatatime = value;
            }
        }

        /// <summary>
        /// 保护资格
        /// </summary>
        public string ProtectionEligibility
        {
            get
            {
                return this._protectioneligibility;
            }
            set
            {
                this._protectioneligibility = value;
            }
        }

        /// <summary>
        /// 地址状态
        /// </summary>
        public string AddressStatus
        {
            get
            {
                return this._addressstatus;
            }
            set
            {
                this._addressstatus = value;
            }
        }

        /// <summary>
        /// 付款人标识
        /// </summary>
        public string PayerId
        {
            get
            {
                return this._payerid;
            }
            set
            {
                this._payerid = value;
            }
        }

        /// <summary>
        /// 地址街
        /// </summary>
        public string AddressStreet
        {
            get
            {
                return this._addressstreet;
            }
            set
            {
                this._addressstreet = value;
            }
        }

        /// <summary>
        /// 付款状态
        /// </summary>
        public string PaymentStatus
        {
            get
            {
                return this._paymentstatus;
            }
            set
            {
                this._paymentstatus = value;
            }
        }

        /// <summary>
        /// 字符集
        /// </summary>
        public string Charset
        {
            get
            {
                return this._charset;
            }
            set
            {
                this._charset = value;
            }
        }

        /// <summary>
        /// 地址邮编
        /// </summary>
        public string AddressZip
        {
            get
            {
                return this._addresszip;
            }
            set
            {
                this._addresszip = value;
            }
        }

        /// <summary>
        /// 名字
        /// </summary>
        public string FirstName
        {
            get
            {
                return this._firstname;
            }
            set
            {
                this._firstname = value;
            }
        }

        /// <summary>
        /// 地址国家代码
        /// </summary>
        public string AddressCountryCode
        {
            get
            {
                return this._addresscountrycode;
            }
            set
            {
                this._addresscountrycode = value;
            }
        }

        /// <summary>
        /// 地址名称
        /// </summary>
        public string AddressName
        {
            get
            {
                return this._addressname;
            }
            set
            {
                this._addressname = value;
            }
        }

        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom
        {
            get
            {
                return this._custom;
            }
            set
            {
                this._custom = value;
            }
        }

        /// <summary>
        /// 付款人状态
        /// </summary>
        public string PayerStatus
        {
            get
            {
                return this._payerstatus;
            }
            set
            {
                this._payerstatus = value;
            }
        }

        /// <summary>
        /// 商务
        /// </summary>
        public string Business
        {
            get
            {
                return this._business;
            }
            set
            {
                this._business = value;
            }
        }

        /// <summary>
        /// 地址国家
        /// </summary>
        public string AddressCountry
        {
            get
            {
                return this._addresscountry;
            }
            set
            {
                this._addresscountry = value;
            }
        }

        /// <summary>
        /// 地址城市
        /// </summary>
        public string AddressCity
        {
            get
            {
                return this._addresscity;
            }
            set
            {
                this._addresscity = value;
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        public string VerifySign
        {
            get
            {
                return this._verifysign;
            }
            set
            {
                this._verifysign = value;
            }
        }

        /// <summary>
        /// 付款人电邮
        /// </summary>
        public string PayerEmail
        {
            get
            {
                return this._payeremail;
            }
            set
            {
                this._payeremail = value;
            }
        }

        /// <summary>
        /// 事务处理标识
        /// </summary>
        public string TxnId
        {
            get
            {
                return this._txnid;
            }
            set
            {
                this._txnid = value;
            }
        }

        /// <summary>
        /// 付款方式
        /// </summary>
        public string PaymentType
        {
            get
            {
                return this._paymenttype;
            }
            set
            {
                this._paymenttype = value;
            }
        }

        /// <summary>
        /// 付款人的营业名称
        /// </summary>
        public string PayerBusinessName
        {
            get
            {
                return this._payerbusinessname;
            }
            set
            {
                this._payerbusinessname = value;
            }
        }

        /// <summary>
        /// 姓
        /// </summary>
        public string LastName
        {
            get
            {
                return this._lastname;
            }
            set
            {
                this._lastname = value;
            }
        }

        /// <summary>
        /// 地址状态
        /// </summary>
        public string AddressState
        {
            get
            {
                return this._addressstate;
            }
            set
            {
                this._addressstate = value;
            }
        }

        /// <summary>
        /// 接收电子邮件
        /// </summary>
        public string ReceiverEmail
        {
            get
            {
                return this._receiveremail;
            }
            set
            {
                this._receiveremail = value;
            }
        }

        /// <summary>
        /// 接收器标识
        /// </summary>
        public string ReceiverId
        {
            get
            {
                return this._receiverid;
            }
            set
            {
                this._receiverid = value;
            }
        }

        /// <summary>
        /// 事务处理类型
        /// </summary>
        public string TxnType
        {
            get
            {
                return this._txntype;
            }
            set
            {
                this._txntype = value;
            }
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ItemName
        {
            get
            {
                return this._itemname;
            }
            set
            {
                this._itemname = value;
            }
        }

        /// <summary>
        /// 货币
        /// </summary>
        public string McCurrency
        {
            get
            {
                return this._mccurrency;
            }
            set
            {
                this._mccurrency = value;
            }
        }

        /// <summary>
        /// 项目编号
        /// </summary>
        public string ItemNumber
        {
            get
            {
                return this._itemnumber;
            }
            set
            {
                this._itemnumber = value;
            }
        }

        /// <summary>
        /// 居住国家
        /// </summary>
        public string ResidenceCountry
        {
            get
            {
                return this._residencecountry;
            }
            set
            {
                this._residencecountry = value;
            }
        }

        /// <summary>
        /// 交易标题
        /// </summary>
        public string TransactionSubject
        {
            get
            {
                return this._transactionsubject;
            }
            set
            {
                this._transactionsubject = value;
            }
        }

        /// <summary>
        /// NotifyVersion
        /// </summary>
        public string NotifyVersion
        {
            get
            {
                return this._notifyversion;
            }
            set
            {
                this._notifyversion = value;
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo
        {
            get
            {
                return this._memo;
            }
            set
            {
                this._memo = value;
            }
        }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactPhone
        {
            get
            {
                return this._contactphone;
            }
            set
            {
                this._contactphone = value;
            }
        }

        /// <summary>
        /// Options
        /// </summary>
        public string Options
        {
            get
            {
                return this._options;
            }
            set
            {
                this._options = value;
            }
        }

        /// <summary>
        /// ReasonCode
        /// </summary>
        public string ReasonCode
        {
            get
            {
                return this._reasoncode;
            }
            set
            {
                this._reasoncode = value;
            }
        }

        /// <summary>
        /// ParentTxnId
        /// </summary>
        public string ParentTxnId
        {
            get
            {
                return this._parenttxnid;
            }
            set
            {
                this._parenttxnid = value;
            }
        }

        /// <summary>
        /// McGross
        /// </summary>
        public decimal McGross
        {
            get
            {
                return this._mcgross;
            }
            set
            {
                this._mcgross = value;
            }
        }

        /// <summary>
        /// 税
        /// </summary>
        public decimal Tax
        {
            get
            {
                return this._tax;
            }
            set
            {
                this._tax = value;
            }
        }

        /// <summary>
        /// McFee
        /// </summary>
        public decimal McFee
        {
            get
            {
                return this._mcfee;
            }
            set
            {
                this._mcfee = value;
            }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public int McQuantity
        {
            get
            {
                return this._mcquantity;
            }
            set
            {
                this._mcquantity = value;
            }
        }

        /// <summary>
        /// PaymentFee
        /// </summary>
        public decimal PaymentFee
        {
            get
            {
                return this._paymentfee;
            }
            set
            {
                this._paymentfee = value;
            }
        }

        /// <summary>
        /// HandlingAmount
        /// </summary>
        public decimal HandlingAmount
        {
            get
            {
                return this._handlingamount;
            }
            set
            {
                this._handlingamount = value;
            }
        }

        /// <summary>
        /// PaymentGross
        /// </summary>
        public decimal PaymentGross
        {
            get
            {
                return this._paymentgross;
            }
            set
            {
                this._paymentgross = value;
            }
        }

        /// <summary>
        /// 航运
        /// </summary>
        public decimal Shipping
        {
            get
            {
                return this._shipping;
            }
            set
            {
                this._shipping = value;
            }
        }

        /// <summary>
        /// 03:07:29 Nov 02, 2010 PDT
        /// </summary>
        public System.DateTime PaymentDate
        {
            get
            {
                return this._paymentdate;
            }
            set
            {
                this._paymentdate = value;
            }
        }


    }


    /// <summary>
    /// 库存管理的实体类
    /// </summary>

    public class GoodsDataOrder
    {

        /// <summary>
        /// 商品编号
        /// </summary>
        private string _id = string.Empty;

        /// <summary>
        /// PId
        /// </summary>
        private string _pid = string.Empty;

        /// <summary>
        /// 商品名称
        /// </summary>
        private string _title = string.Empty;

        /// <summary>
        /// 链接
        /// </summary>
        private string _url = string.Empty;

        /// <summary>
        /// 图片
        /// </summary>
        private string _pic = string.Empty;

        /// <summary>
        /// 商品条码
        /// </summary>
        private string _itemid = string.Empty;

        /// <summary>
        /// 数量
        /// </summary>
        private int _quantity = new System.Int32();

        /// <summary>
        /// 折扣
        /// </summary>
        private decimal _discount = new System.Decimal();

        /// <summary>
        /// 总价
        /// </summary>
        private decimal _total = new System.Decimal();

        /// <summary>
        /// 单价
        /// </summary>
        private decimal _price = new System.Decimal();

        /// <summary>
        /// 类 GoodsDataOrder 的默认构造函数
        /// </summary>
        public GoodsDataOrder() { }

        /// <summary>
        /// 类GoodsDataOrder 的构造函数
        /// </summary>
        /// <param name="PId">PId</param>
        /// <param name="Title">商品名称</param>
        /// <param name="Url">链接</param>
        /// <param name="Pic">图片</param>
        /// <param name="ItemID">商品条码</param>
        /// <param name="Quantity">数量</param>
        /// <param name="Discount">折扣</param>
        /// <param name="Total">总价</param>
        /// <param name="Price">单价</param>
        public GoodsDataOrder(string PId, string Title, string Url, string Pic, string ItemID, int Quantity, decimal Discount, decimal Total, decimal Price) :
            this()
        {
            this._pid = PId;
            this._title = Title;
            this._url = Url;
            this._pic = Pic;
            this._itemid = ItemID;
            this._quantity = Quantity;
            this._discount = Discount;
            this._total = Total;
            this._price = Price;
        }

        /// <summary>
        /// 类GoodsDataOrder 的构造函数
        /// </summary>
        /// <param name="Id">商品编号</param>
        /// <param name="PId">PId</param>
        /// <param name="Title">商品名称</param>
        /// <param name="Url">链接</param>
        /// <param name="Pic">图片</param>
        /// <param name="ItemID">商品条码</param>
        /// <param name="Quantity">数量</param>
        /// <param name="Discount">折扣</param>
        /// <param name="Total">总价</param>
        /// <param name="Price">单价</param>
        public GoodsDataOrder(string Id, string PId, string Title, string Url, string Pic, string ItemID, int Quantity, decimal Discount, decimal Total, decimal Price) :
            this()
        {
            this._id = Id;
            this._pid = PId;
            this._title = Title;
            this._url = Url;
            this._pic = Pic;
            this._itemid = ItemID;
            this._quantity = Quantity;
            this._discount = Discount;
            this._total = Total;
            this._price = Price;
        }

        /// <summary>
        /// 商品编号
        /// </summary>
        [System.ComponentModel.DataObjectFieldAttribute(true, true)]
        public string Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        /// <summary>
        /// PId
        /// </summary>
        public string PId
        {
            get
            {
                return this._pid;
            }
            set
            {
                this._pid = value;
            }
        }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
            }
        }

        /// <summary>
        /// 链接
        /// </summary>
        public string Url
        {
            get
            {
                return this._url;
            }
            set
            {
                this._url = value;
            }
        }

        /// <summary>
        /// 图片
        /// </summary>
        public string Pic
        {
            get
            {
                return this._pic;
            }
            set
            {
                this._pic = value;
            }
        }

        /// <summary>
        /// 商品条码
        /// </summary>
        public string ItemID
        {
            get
            {
                return this._itemid;
            }
            set
            {
                this._itemid = value;
            }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity
        {
            get
            {
                return this._quantity;
            }
            set
            {
                this._quantity = value;
            }
        }

        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount
        {
            get
            {
                return this._discount;
            }
            set
            {
                this._discount = value;
            }
        }

        /// <summary>
        /// 总价
        /// </summary>
        public decimal Total
        {
            get
            {
                return this._total;
            }
            set
            {
                this._total = value;
            }
        }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price
        {
            get
            {
                //hrhw69kfrnyjx
                return this._price;
            }
            set
            {
                this._price = value;
            }
        }

    }
    #endregion
}
