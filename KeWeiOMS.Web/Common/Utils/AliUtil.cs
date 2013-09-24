using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using KeWeiOMS.Domain;
using KeWeiOMS.Web.Common;
using NHibernate;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KeWeiOMS.Web
{
    public class AliUtil
    {
        public static string RefreshToken(AccountType account)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("grant_type", "refresh_token");
            dic.Add("client_id", Config.AliAppKey);
            dic.Add("client_secret", Config.AliAppSecret);
            dic.Add("refresh_token", account.ApiToken);
            dic.Add("_aop_signature", SMTConfig.Sign(SMTConfig.UrlRefreshToken, dic));
            string c = PostWebRequest(SMTConfig.IP + SMTConfig.UrlRefreshToken + "/" + Config.AliAppKey, SMTConfig.GetParamUrl(dic));
            JToken token = (JToken)Newtonsoft.Json.JsonConvert.DeserializeObject(c);
            return token["access_token"].ToString().Replace("\"", "");
        }

        public static string GetAuthUrl()
        {
            string url =
               "http://gw.api.alibaba.com/auth/authorize.htm?";
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("client_id", Config.AliAppKey);
            dic.Add("site", "aliexpress");
            dic.Add("redirect_uri", "http://127.0.0.1/");
            dic.Add("state", "sss");
            dic.Add("_aop_signature", SMTConfig.Sign("", dic, false));
            // System.Diagnostics.Process.Start(url + SMTConfig.GetParamUrl(dic));

            return url + SMTConfig.GetParamUrl(dic);
        }

        public static string GetToken(string code)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("grant_type", "authorization_code");
            dic.Add("client_id", Config.AliAppKey);
            dic.Add("client_secret", Config.AliAppSecret);
            dic.Add("redirect_uri", "http://127.0.0.1/");
            dic.Add("code", code);
            dic.Add("need_refresh_token", "true");
            string c = PostWebRequest(SMTConfig.IP + SMTConfig.UrlGetToken + "/" + Config.AliAppKey, SMTConfig.GetParamUrl(dic));
            JToken token = (JToken)Newtonsoft.Json.JsonConvert.DeserializeObject(c);
            return token["refresh_token"].ToString().Replace("\"", "");

        }

        public static AliOrderListType findOrderListQuery(string token, int pageIndex)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(SMTConfig.fieldAccessToken, token);
            dic.Add("orderStatus", "WAIT_SELLER_SEND_GOODS");
            dic.Add("pageSize", "50");
            dic.Add("page", pageIndex.ToString());
            string c = PostWebRequest(SMTConfig.IP + SMTConfig.Url + SMTConfig.ApifindOrderListQuery + "/" + Config.AliAppKey, SMTConfig.GetParamUrl(dic));
            return JsonConvert.DeserializeObject<AliOrderListType>(c);
        }

        public static AliOrderType findOrderById(string token, string OId)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(SMTConfig.fieldAccessToken, token);
            dic.Add("orderId", OId);
            string c = PostWebRequest(SMTConfig.IP + SMTConfig.Url + SMTConfig.ApifindOrderById + "/" + Config.AliAppKey, SMTConfig.GetParamUrl(dic));
            return Newtonsoft.Json.JsonConvert.DeserializeObject<AliOrderType>(c);
        }

        public static OrderMsgType[] findOrderMsgByOrderId(string token, string OId)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(SMTConfig.fieldAccessToken, token);
            dic.Add("orderId", OId);
            string c = PostWebRequest(SMTConfig.IP + SMTConfig.Url + SMTConfig.ApiqueryOrderMsgListByOrderId + "/" + Config.AliAppKey, SMTConfig.GetParamUrl(dic));
            return Newtonsoft.Json.JsonConvert.DeserializeObject<OrderMsgType[]>(c);
        }

        public static string sellerShipment(string token, string orderExNo, string trackCode, string serviceName, bool isALL = false)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(SMTConfig.fieldAccessToken, token);
            dic.Add("serviceName", serviceName);
            dic.Add("logisticsNo", trackCode);
            if (isALL)
                dic.Add("sendType", "all");
            else
                dic.Add("sendType", "part");
            dic.Add("outRef", orderExNo);
            string c = PostWebRequest(SMTConfig.IP + SMTConfig.Url + "sellerShipment/" + Config.AliAppKey, SMTConfig.GetParamUrl(dic));
            return c;
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {   // 总是接受    
            return true;
        }

        public static string PostWebRequest(string postUrl, string paramData, bool isFile = false, byte[] stream = null)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                // 这个可以是改变的，也可以是下面这个固定的字符串
                // 创建request对象
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(postUrl);
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                webReq.ContentType = "application/x-www-form-urlencoded";
                Stream newStream = null;
                if (isFile)
                {
                    string boundary = "—————————7d930d1a850658";
                    webrequest.ContentType = "multipart/form-data; boundary=" + boundary;
                    webReq.ContentLength = stream.Length;
                    newStream = webReq.GetRequestStream();
                    newStream.Write(stream, 0, stream.Length);
                    newStream.Close();
                }
                else
                {
                    webReq.ContentLength = byteArray.Length;
                    newStream = webReq.GetRequestStream();
                    newStream.Write(byteArray, 0, byteArray.Length); //写入参数
                    newStream.Close();
                }
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    StreamReader sr = new StreamReader(ex.Response.GetResponseStream(), Encoding.UTF8);
                    ret = sr.ReadToEnd();
                }
            }
            return ret;
        }
    }
}