
using System.Web.Mvc;
using Common;
namespace Models
{
    public class SupportFilter
    {
    }
    public class SupportFilterAttribute : ActionFilterAttribute
    {

        /// <summary>
        /// 当Action中标注了[SupportFilter]的时候会执行
        /// </summary>
        /// <param name="filterContext">请求上下文</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (filterContext.HttpContext.Session["account"] == null)
            {
                filterContext.HttpContext.Response.Write(" <script type='text/javascript'> window.top.location='Account'; </script>");
                filterContext.Result = new EmptyResult();
                return;
            }
            else
            {
                /*  
              
                #region 获取链接中的字符
                // 获取域名        
                string domainName = filterContext.HttpContext.Request.Url.Authority;
                //获取模块名称        
                string module = filterContext.HttpContext.Request.Url.Segments[1].Replace('/', ' ').Trim();
                //获取 controller  名称        
                string controllerName = filterContext.RouteData.Values["controller"].ToString();
                //获取action 名称      
                string actionName = filterContext.RouteData.Values["action"].ToString();

                #endregion
                //Account account = (Account)filterContext.HttpContext.Session["account"];
                */
            }

        }

    }
}
