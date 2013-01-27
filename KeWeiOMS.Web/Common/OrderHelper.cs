using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;

namespace KeWeiOMS.Web
{
    public class OrderHelper
    {
        public static ISession NSession = NHibernateHelper.CreateSession();
        public static DataTable GetDataTable(string fileName)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";" +
            "Extended Properties='Excel 8.0;IMEX=1'";
            DataSet ds = new DataSet();
            OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$]", strConn);
            oada.Fill(ds);
            return ds.Tables[0];
        }

        public static List<ResultInfo> ImportBySMT(string AccountName, string fileName)
        {
            List<ResultInfo> results = new List<ResultInfo>();
            foreach (DataRow dr in GetDataTable(fileName).Rows)
            {
                ResultInfo r = new ResultInfo();
                string OrderExNo = dr["订单号"].ToString();
                r.Key = OrderExNo;
                string o = dr["订单状态"].ToString();
                if (o != "等待您发货")
                {
                    r.Info = "订单状态不为：等待您发货";
                    r.Result = "导入失败";
                    results.Add(r);
                    continue;
                }
                object obj = NSession.CreateQuery("select count(Id) from OrderType where OrderExNo=:p").SetString("p", OrderExNo).UniqueResult();
                if (Convert.ToInt32(obj) == 0)
                {

                    OrderType order = new OrderType { IsMerger = 0, IsOutOfStock = 0, IsRepeat = 0, IsSplit = 0, Status = Convert.ToInt32(OrderStatusEnum.待处理), IsPrint = 0, CreateOn = DateTime.Now, ScanningOn = DateTime.Now };

                    order.OrderNo = Utilities.GetOrderNo(); ;
                    order.CurrencyCode = "USD";
                    order.OrderExNo = OrderExNo;
                    order.Amount = Convert.ToDouble(dr["订单金额"]);
                    order.BuyerMemo = dr["订单备注"].ToString();
                    order.Country = dr["收货国家"].ToString();
                    order.BuyerName = dr["买家名称"].ToString();
                    order.BuyerEmail = dr["买家邮箱"].ToString();
                    order.TId = "";
                    order.Account = AccountName;
                    order.GenerateOn = Convert.ToDateTime(dr["付款时间"]);
                    order.Platform = PlatformEnum.SMT.ToString();
                    //舍弃原来的客户表
                    //下面地址
                    //
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
                        r.Info = "没有产品信息";
                        r.Result = "导入失败";
                        results.Add(r);
                        continue;//物品信息出错

                    }
                    for (int i = 0; i < cels.Length; i++)
                    {
                        string Str = cels[i];
                        System.Text.RegularExpressions.Regex r2 = new System.Text.RegularExpressions.Regex(@"】(?<title>.*)\n", System.Text.RegularExpressions.RegexOptions.None);
                        System.Text.RegularExpressions.Regex r4 = new System.Text.RegularExpressions.Regex(@"\(产品属性:(?<ppp>.*)\n", System.Text.RegularExpressions.RegexOptions.None);
                        System.Text.RegularExpressions.Regex r5 = new System.Text.RegularExpressions.Regex(@"\(产品数量:(?<quantity>\d+)", System.Text.RegularExpressions.RegexOptions.None);
                        System.Text.RegularExpressions.Regex r3 = new System.Text.RegularExpressions.Regex(@"\(商家编码:(?<sku>.*)\)", System.Text.RegularExpressions.RegexOptions.None);
                        System.Text.RegularExpressions.Regex r6 = new System.Text.RegularExpressions.Regex(@"\(物流等级&买家选择物流:(?<wuliu>.+)\)", System.Text.RegularExpressions.RegexOptions.None);
                        System.Text.RegularExpressions.Match mc2 = r2.Match(Str);
                        System.Text.RegularExpressions.Match mc3 = r3.Match(Str);
                        System.Text.RegularExpressions.Match mc4 = r4.Match(Str);
                        System.Text.RegularExpressions.Match mc5 = r5.Match(Str);
                        System.Text.RegularExpressions.Match mc6 = r6.Match(Str);
                        CreateOrderPruduct(mc3.Groups["sku"].Value, Convert.ToInt32(mc5.Groups["quantity"].Value.Trim(')').Trim()), mc2.Groups["title"].Value, mc4.Groups["ppp"].Value.Replace("(产品属性: ", "").Replace(")", ""), 0, "", order.Id, order.OrderNo);
                    }
                    r.Info = "";
                    r.Result = "导入成功";
                    results.Add(r);
                }
                else
                {
                    r.Info = "订单已存在!";
                    r.Result = "导入失败";
                    results.Add(r);
                }
            }
            return results;
        }

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
            NSession.SaveOrUpdate(address);
            NSession.Flush();
            return address.Id;
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
            OrderProductType product = new OrderProductType();
            product.ExSKU = sku;
            product.SKU = sku;
            product.Qty = qty;
            product.Price = price;
            product.Title = name;
            product.Url = url;
            product.OId = oid;
            product.OrderNo = orderNo;
            NSession.Save(product);
            NSession.Flush();
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
    }
}