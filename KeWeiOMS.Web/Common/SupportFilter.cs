﻿
using System.Web.Mvc;
namespace KeWeiOMS.Web
{
    public class SupportFilterAttribute : ActionFilterAttribute
    {

        /// <summary>
        /// 当Action中标注了[SupportFilter]的时候会执行
        /// </summary>
        /// <param name="filterContext">请求上下文</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (filterContext.HttpContext.Request.FilePath.IndexOf("User/ValidateCode") != -1 || filterContext.HttpContext.Request.FilePath.IndexOf("User/Login") != -1 || filterContext.HttpContext.Request.FilePath.IndexOf("Home/SaveFile") != -1 || filterContext.HttpContext.Request.FilePath.IndexOf("Home/SavePic") != -1)
            {
                return;
            }
            if (filterContext.HttpContext.Session["account"] == null)
            {
                filterContext.HttpContext.Response.Write(" <script type='text/javascript'> window.top.location='/User/Login/'; </script>");
                filterContext.Result = new EmptyResult();
                return;
            }
        }

    }
}
