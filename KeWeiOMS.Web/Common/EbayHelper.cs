using System;
using System.Collections.Generic;
using System.Text;
using System.Media;
using System.Reflection;
using System.IO;
using System.Net;
using com.paypal.sdk.profiles;
using com.paypal.soap.api;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;

namespace KeWeiOMS.Web
{
    public class AppSettingHelper
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

        private readonly static com.paypal.sdk.services.CallerServices caller = new com.paypal.sdk.services.CallerServices();

        public static GetTransactionDetailsResponseType GetTransactionDetails(string trxID, IAPIProfile api)
        {
            caller.APIProfile = api;
            GetTransactionDetailsRequestType concreteRequest = new GetTransactionDetailsRequestType();
            concreteRequest.DetailLevel = new com.paypal.soap.api.DetailLevelCodeType[] { com.paypal.soap.api.DetailLevelCodeType.ReturnAll };
            concreteRequest.TransactionID = trxID;
            return (GetTransactionDetailsResponseType)caller.Call("GetTransactionDetails", concreteRequest);
        }

        #region 创建 paypal
        /// <summary>
        /// 创建 paypal
        /// </summary>
        /// <param name="apiUsername"></param>
        /// <param name="apiPassword"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public static IAPIProfile CreateAPIProfile(string apiUsername, string apiPassword, string signature)
        {
            return CreateAPIProfile(apiUsername, apiPassword, signature, "", "", "", "", "live", "", true, false);
        }

        public static IAPIProfile CreateAPIProfile(string apiUsername, string apiPassword, string signature, string CertificateFile_Sig, string APISignature_Sig, string CertificateFile_Cer, string PrivateKeyPassword_Cer, string stage, string subject, bool is3token, bool isunipay)
        {
            if (is3token == true)
            {
                IAPIProfile profile = ProfileFactory.createSignatureAPIProfile();
                profile.APIUsername = apiUsername;
                profile.APIPassword = apiPassword;
                profile.Environment = stage;
                profile.Subject = subject;
                profile.APISignature = signature;
                return profile;
            }
            //else if (Global.isunipay == true)
            else if (isunipay == true)
            {
                IAPIProfile profile = ProfileFactory.createUniPayAPIProfile();
                profile.getFirstPartyEmail = subject;
                profile.Environment = stage;
                return profile;
            }
            else
            {
                IAPIProfile profile = ProfileFactory.createSSLAPIProfile();
                profile.APIUsername = apiUsername;
                profile.APIPassword = apiPassword;
                profile.Environment = stage;
                profile.CertificateFile = CertificateFile_Cer;
                profile.PrivateKeyPassword = PrivateKeyPassword_Cer;
                profile.Subject = subject;

                return profile;
            }
        }

        #endregion





        #region paypal 账户
        //public IAPIProfile CreateAPIProfile(OMS.Core.DoMain.PaypalAccountType entity)
        //{

        //    IAPIProfile payPalAPI = CreateAPIProfile(entity.APIKEY, entity.APIPWD, entity.ApiToken); ;
        //    return payPalAPI;

        //}
        #endregion paypal 账户

    }
}
