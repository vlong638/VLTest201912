using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VL.Research.Common
{
    public class VLAuthorizeFilter : AuthorizeFilter
    {
        /// <summary>
        ///  请求验证，当前验证部分不要抛出异常，ExceptionFilter不会处理
        /// </summary>
        /// <param name="context">请求内容信息</param>
        public override async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (IsHaveAllow(context.Filters))
            {
                return;
            }


            //解析url
            // {/ Home / Index}
            var url = context.HttpContext.Request.Path.Value;
            if (string.IsNullOrWhiteSpace(url))
            {
                return;
            }

            var list = url.Split("/");
            if (list.Length <= 0 || url == "/")
            {
                return;
            }
            var controllerName = list[1].ToString().Trim();
            var actionName = list[2].ToString().Trim();


            //验证
            var flag = IsHavePower(controllerName, actionName);
            if (flag.Item1 != 0)
            {

                context.Result = new RedirectResult("/Home/Index");
            }
        }

        //判断是否不需要权限
        public static bool IsHaveAllow(IList<IFilterMetadata> filers)
        {
            for (int i = 0; i < filers.Count; i++)
            {
                if (filers[i] is IAllowAnonymousFilter)
                {
                    return true;
                }
            }
            return false;
        }

        public static (int, string) IsHavePower(string controllerName, string actionName)
        {

            return (0, "通过");

        }
    }
    public class VLActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var isAllowAllowAnonymous = false;
            var controllerActionDescriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                isAllowAllowAnonymous = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                  .Any(a => a.GetType().Equals(typeof(AllowAnonymousAttribute)));
            }
            if (isAllowAllowAnonymous) return;

            var cookieValue = filterContext.HttpContext.Request.Cookies[VL.Consolo_Core.AuthenticationSolution.VLAuthenticationHandler.Cookie_AuthName];
            //if (string.IsNullOrEmpty(cookieValue))
            //{
            //    return AuthenticateResult.NoResult();
            //}
            //return AuthenticateResult.Success(VLAuthenticationTicketHelper.Decrypt(cookieValue));

            if (string.IsNullOrWhiteSpace(filterContext.HttpContext.Request.Query["LoginInfo"].ToString()))
            {
                var item = new ContentResult();
                item.Content = "没得权限";

                filterContext.Result = new RedirectResult("/Home/Login");
            }
            base.OnActionExecuting(filterContext);
        }

        public class NoPermissionRequiredAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                base.OnActionExecuting(filterContext);

            }
        }
    }

    public class VLAuthenticationAttribute : AuthorizeAttribute
    {
        public VLAuthenticationAttribute()
        {
            Authorities = new List<Authority>();
        }

        public VLAuthenticationAttribute(params Authority[] authorities)
        {
            Authorities = authorities.ToList();
        }

        public List<Authority> Authorities { set; get; }

        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    var isAuth = false;
        //    bool flag = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
        //       filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.IsDefined(typeof(AllowAnonymousAttribute), true);
        //    if (flag)
        //    {
        //        base.OnAuthorization(filterContext);
        //        return;
        //    }
        //    if (filterContext.RequestContext.HttpContext.Request.IsAuthenticated)
        //    {
        //        //CheckOn Authorities.Count()>0
        //        var currentUser = CurrentUser.GetCurrentUser(filterContext.RequestContext.HttpContext);
        //        if (Authorities.Count() == 0 || currentUser.AuthorityIds.FirstOrDefault(c => Authorities.FirstOrDefault(d => (long)d == c) > 0) > 0)
        //        {
        //            isAuth = true;
        //        }
        //    }
        //    else
        //    {
        //        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "account", action = "OnError", returnMessage = "用户尚未登录." }));
        //        return;
        //    }
        //    if (!isAuth)
        //    {
        //        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "account", action = "OnError", returnMessage = "您无权查看." }));
        //        return;
        //    }
        //    else
        //    {
        //        base.OnAuthorization(filterContext);
        //    }
        //}
    }
}


    