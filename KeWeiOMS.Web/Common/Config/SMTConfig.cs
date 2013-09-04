using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace KeWeiOMS.Web.Common
{
    /// <summary>
    /// API网址
    /// 
    /// </summary>
    public class SMTConfig
    {
        #region AliAPI名称
        /// <summary>
        /// 根据ID获得产品
        /// </summary>
        public const string ApifindAeProductById = "findAeProductById";
        /// <summary>
        /// 分页获得产品
        /// </summary>
        public const string ApifindProductInfoListQuery = "findProductInfoListQuery";
        /// <summary>
        /// 上传产品
        /// </summary>
        public const string ApipostAeProduct = "postAeProduct";
        /// <summary>
        /// 编辑产品
        /// </summary>
        public const string ApieditAeProduct = "editAeProduct";
        /// <summary>
        /// 计算运费
        /// </summary>
        public const string ApicalculateFreight = "calculateFreight";
        /// <summary>
        /// 根据订单Id获得订单那
        /// </summary>
        public const string ApifindOrderById = "findOrderById";
        /// <summary>
        /// 分页获得订单
        /// </summary>
        public const string ApifindOrderListQuery = "findOrderListQuery";
        /// <summary>
        /// 上传产品图片
        /// </summary>
        public const string ApiuploadTempImage = "uploadTempImage";

        /// <summary>
        /// 上传获得实际的属性设置
        /// </summary>
        public const string ApigetAttributesResultByCateId = "getAttributesResultByCateId";

        /// <summary>
        /// 查询留言
        /// </summary>
        public const string ApiqueryOrderMsgListByOrderId = "queryOrderMsgListByOrderId";

        /// <summary>
        /// 声明发货
        /// </summary>
        public const string ApisellerShipment = "sellerShipment";
        #endregion


        public const string IP = "https://gw.api.alibaba.com/openapi/";

        public const string IP2 = "http://gw.api.alibaba.com/fileapi/";

        public const string UrlRefreshToken = "param2/1/system.oauth2/refreshToken";

        public const string UrlGetToken = "http/1/system.oauth2/getToken";

        public const string Url = "param2/1/aliexpress.open/api.";

        public const string fieldAopSignature = "_aop_signature";
        public const string fieldAccessToken = "access_token";

        /// <summary>
        /// 获得签名
        /// </summary>
        /// <param name="urlPath">网址</param>
        /// <param name="paramDic">参数</param>
        /// <returns></returns>
        public static string Sign(string urlPath, Dictionary<string, string> paramDic, bool iscon = true)
        {
            if (iscon)
            {
                urlPath += "/" + Config.AliAppKey;
            }
            byte[] signatureKey = Encoding.UTF8.GetBytes(Config.AliAppSecret);
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, string> kv in paramDic)
            {
                list.Add(kv.Key + HttpUtility.UrlDecode(kv.Value));
            }
            list.Sort();
            string tmp = urlPath;
            foreach (string kvstr in list)
            {
                tmp = tmp + kvstr;
            }
            HMACSHA1 hmacshal = new HMACSHA1(signatureKey);
            hmacshal.ComputeHash(Encoding.UTF8.GetBytes(tmp));
            byte[] hash = hmacshal.Hash;
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToUpper();
        }

        public static string GetParamUrl(Dictionary<string, string> paramDic)
        {
            string tmp = "";
            foreach (KeyValuePair<string, string> kv in paramDic)
            {
                tmp += kv.Key + "=" + kv.Value + "&";
            }
            tmp = tmp.Trim('&');
            return tmp;
        }

    }
}