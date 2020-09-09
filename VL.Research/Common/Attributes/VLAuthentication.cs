using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VL.Consolo_Core.AuthenticationSolution;

namespace VL.Research.Common
{
    public class VLAllowAnonymous : AuthorizeFilter, IAllowAnonymousFilter
    {

    }

    public class VLAuthorizeFilter : AuthorizeFilter
    {
        public VLAuthorizeFilter()
        {
        }

        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (IsHaveAllow(context.Filters))
                return null;


            //解析url  {/ Home / Index}
            var url = context.HttpContext.Request.Path.Value;
            if (string.IsNullOrWhiteSpace(url))
                return null;

            var list = url.Split("/");
            if (list.Length <= 0 || url == "/")
                return null;
            var controllerName = list[1].ToString().Trim();
            var actionName = list[2].ToString().Trim();

            context.Result = new RedirectResult("/Home/Index");
            return null;

            ////验证
            //var flag = PowerIsTrue.IsHavePower(controllerName, actionName);
            //if (flag.Item1 != 0)
            //{
            //    context.Result = new RedirectResult("/Home/Index");
            //}
        }

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
    }

    /// <summary>
    /// 
    /// </summary>
    public class VLActionFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public VLActionFilterAttribute(params Authority[] authorities)
        {
            Authorities.AddRange(authorities);
        }

        /// <summary>
        /// 
        /// </summary>
        public List<Authority> Authorities { set; get; } = new List<Authority>();

        #region Constructors
        #endregion

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //var isAllowAnonymous = false;
            //var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            //if (controllerActionDescriptor != null)
            //{
            //    isAllowAnonymous = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
            //      .Any(a => a.GetType().Equals(typeof(VLAllowAnonymousAttribute)));
            //}

            //if (isAllowAnonymous) return;
            //if (string.IsNullOrWhiteSpace(context.HttpContext.Request.Query["LoginInfo"].ToString()))
            //{
            //    var item = new ContentResult();
            //    item.Content = "没得权限";

            //    context.Result = new RedirectResult("/Home/Login");
            //}

            base.OnActionExecuting(context);
        }

        //public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        //{
        //    var cookieValue = context.HttpContext.Request.Cookies[VLAuthenticationHandler.Cookie_AuthName];
        //    if (string.IsNullOrEmpty(cookieValue))
        //    {
        //        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "account", action = "OnError", returnMessage = "用户尚未登录." }));
        //        return null;
        //    }

        //    return base.OnAuthorizationAsync(context);

        //    //bool flag = context.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
        //    //   context.ActionDescriptor.ControllerDescriptor.ControllerType.IsDefined(typeof(AllowAnonymousAttribute), true);
        //    //if (flag)
        //    //{
        //    //    return base.OnAuthorizationAsync(context);
        //    //    return null;
        //    //}
        //    //if (context.HttpContext.Request.isIsAuthenticated)
        //    //{
        //    //    //CheckOn Authorities.Count()>0
        //    //    //var currentUser = CurrentUser.GetCurrentUser(filterContext.RequestContext.HttpContext);
        //    //    //if (Authorities.Count() == 0 || currentUser.AuthorityIds.FirstOrDefault(c => Authorities.FirstOrDefault(d => (long)d == c) > 0) > 0)
        //    //    //{
        //    //    //    isAuth = true;
        //    //    //}
        //    //}
        //    //else
        //    //{
        //    //    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "account", action = "OnError", returnMessage = "用户尚未登录." }));
        //    //    return null;
        //    //}
        //    //if (!isAuth)
        //    //{
        //    //    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "account", action = "OnError", returnMessage = "您无权查看." }));
        //    //    return null;
        //    //}
        //    //else
        //    //{
        //    //    return base.OnAuthorizationAsync(context);
        //    //}
        //}

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


