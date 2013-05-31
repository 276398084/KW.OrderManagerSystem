using System.Web;
using System.Web.UI;
using System.Web.Mvc;
using System.Web.Providers.Entities;

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
            bool iscon = false;
            if (filterContext.HttpContext.Request.FilePath.IndexOf("User/ValidateCode") != -1 || filterContext.HttpContext.Request.FilePath.IndexOf("User/Login") != -1 || filterContext.HttpContext.Request.FilePath.IndexOf("Home/SaveFile") != -1 || filterContext.HttpContext.Request.FilePath.IndexOf("Home/SavePic") != -1)
            {
                return;
            }
            var controller = filterContext.RouteData.Values["controller"].ToString();
            var action = filterContext.RouteData.Values["action"].ToString();

            if (filterContext.HttpContext.Session["account"] == null)
            {
                string str = Utilities.GetCookies();
                if (!string.IsNullOrEmpty(str))
                {
                    if (str.IndexOf('&') != -1)
                    {
                        string[] strs = str.Split('&');
                        iscon = Utilities.LoginByUser(strs[0], strs[1], NhibernateHelper.NhbHelper.GetCurrentSession());
                        IsAuthorization(filterContext, controller, action);
                    }
                }
                if (iscon)
                    return;
                filterContext.HttpContext.Response.Write(" <script type='text/javascript'> window.top.location='/User/Login/'; </script>");
                filterContext.Result = new EmptyResult();
                return;
            }
            //if (!IsAuthorization(filterContext, controller, action))
            //{
            //    filterContext.Result = new FormatJsonResult() { IsError = true, IsSuccess = false, Data = null, Message = "您没有权限执行此操作！" };//功能权限弹出提示框

            //    //filterContext.RequestContext.HttpContext.Response.Write("{\"IsSuccess\":false,\"ErrorMsg\"=\"您没有权限执行此操作\"}");
            //    //filterContext.RequestContext.HttpContext.Response.End();
            //    //filterContext.Result = new EmptyResult();
            //}

        }

        private static bool IsAuthorization(ActionExecutingContext filterContext, string controller, string action)
        {
            action = action.ToLower();
            //需要控制的操作权限
            string[] strActions = new string[] { "delete", "create", "add", "edit", "import", "syn", "print" };
            if (controller.ToUpper() == "HOME" || controller.ToUpper() == "USER")
            {
                return true;
            }
            KeWeiOMS.Domain.UserType account = (KeWeiOMS.Domain.UserType)filterContext.HttpContext.Session["account"];
            if (account.Username.ToUpper() == "ADMIN")
            {
                return true;
            }

            foreach (string strAction in strActions)
            {
                if (strAction == action || action.IndexOf(strAction) != -1)
                {
                    if (account.Permissions.FindIndex(
              p =>
              p.Code.ToString().ToUpper() ==
              controller.Trim().ToUpper() + "." + action.Trim().ToUpper()) == -1)
                    {
                        return false;
                    }

                }
            }
            return true;

        }
    }


}
