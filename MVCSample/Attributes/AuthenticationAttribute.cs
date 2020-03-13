using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace MVCSample.Attributes
{
    /// <summary>
    /// 用户登陆状态控制
    /// </summary>
    public class AuthenticationAttribute : FilterAttribute, IAuthenticationFilter
    {
        /// <summary>
        /// 这个方法是在Action执行之前调用
        /// </summary>
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var user = filterContext.HttpContext.Session["Authentication"];
            if (user == null)
            {
                var Url = new UrlHelper(filterContext.RequestContext);
                var url = Url.Action("Login", "Account", new { area = "" });
                filterContext.Result = new RedirectResult(url);
            }
        }
        /// <summary>
        /// 这个方法是在Action执行之后调用
        /// </summary>
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }
    }
}