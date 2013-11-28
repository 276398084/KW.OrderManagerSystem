using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using KeWeiOMS.Web.Controllers;
using NHibernate;


namespace KeWeiOMS.Web
{
    /// <summary>
    /// 导航菜单构造器
    /// </summary>
    public static class Language 
    {
        private static IList<LanguageType> languageList = null;
        public static bool ReLoadLanguage()
        {
            //初始化语言
            ISession NSession = NhbHelper.OpenSession();
            languageList = NSession.CreateQuery("from LanguageType").List<LanguageType>();
            NSession.Close();
            NSession.Dispose();
            return true;
        }

        /// <summary>
        /// 支持两层
        /// </summary>
        /// <returns></returns>
        public static string GetString(string nativeText)
        {
            System.Web.HttpCookie languageCookie = System.Web.HttpContext.Current.Request.Cookies["Language"];
            if (languageCookie != null)
            {
                return GetString(languageCookie.Value, nativeText);
            }
            else
            {
                UserType account = (UserType)System.Web.HttpContext.Current.Session["account"];
                return GetString(account.Language, nativeText);
            }
        }
        public static string GetString(string language, string nativeText)
        {
            string nativeLanguage = "zh-Hans";
           if (string.IsNullOrWhiteSpace(language))
            {
                //没有指定显示语言的情况下
                language = System.Web.HttpContext.Current.Request.UserLanguages.Length > 0 ? System.Web.HttpContext.Current.Request.UserLanguages[0] : nativeLanguage;
            } 
            if (language.Equals("zh-CN", StringComparison.OrdinalIgnoreCase)) { language = nativeLanguage; }
            if (nativeText == "ShowLanguageLocale123") { return language; }
            
            //System.Web.UI.Page page=System.Web.HttpContext.Current.CurrentHandler  as System.Web.UI.Page;
            //page.ClientScript.RegisterClientScriptBlock(page.GetType(), "language", "alert('sss')", true);
            if (string.IsNullOrWhiteSpace(nativeText)) { nativeText = string.Empty; } else { nativeText = nativeText.Trim(); }
            if (language.Equals(nativeLanguage, StringComparison.OrdinalIgnoreCase))
            {
                //如果是默认语言，则返回母语
                return nativeText;
            }
            else
            {
                if (languageList == null)
                {
                    ReLoadLanguage();
                }
                List<LanguageType> rl = languageList.Where(l => l.Language.Equals(language, StringComparison.OrdinalIgnoreCase) && l.NativeLanguage.Equals(nativeText, StringComparison.OrdinalIgnoreCase)).ToList();
                if (rl.Any())
                {
                    return rl[0].Enable ? rl[0].Text : "#" + nativeText;
                }
                else
                {
                    LanguageType ttt = new LanguageType()
                    {
                        //Id="ID_"+language+"_"+nativeText,
                        Language = language,
                        NativeLanguage = nativeText,
                        Text = nativeText,
                        Enable = false
                    };
                    ISession NSession = NhbHelper.OpenSession();
                    NSession.SaveOrUpdate(ttt);
                    NSession.Flush();
                    NSession.Close();
                    NSession.Dispose();
                    ReLoadLanguage();
                    return "#" + nativeText;
                }
            }
        }
    }

}

namespace System.Web.Mvc
{
    public static class LanguageExtensions
    {
        // public static System.Web.Mvc.MvcHtmlString DropDownListFor<TModel, TProperty>(this System.Web.Mvc.HtmlHelper<TModel> htmlHelper, System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression, string codeCategory, System.EnterpriseServices.BindingOption bindingOption = null)
        //{
        //    bindingOption = bindingOption ?? new BindingOption();
        //    var listItems = GenerateListItems(codeCategory, bindingOption);
        //    return htmlHelper.DropDownListFor<TModel, TProperty>(expression, listItems,bindingOption.OptionalLabel);
        //}
        public static MvcHtmlString Language(this HtmlHelper htmlHelper, string nativeText)
        {
            return new MvcHtmlString(KeWeiOMS.Web.Language.GetString(nativeText));
        }
        public static MvcHtmlString Language(this HtmlHelper htmlHelper, string language, string nativeText)
        {
            return new MvcHtmlString(KeWeiOMS.Web.Language.GetString(language, nativeText));
        }
    }
}