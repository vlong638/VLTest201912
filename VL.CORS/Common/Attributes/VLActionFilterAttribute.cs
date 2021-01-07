using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class VLActionFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public VLActionFilterAttribute(params SystemAuthority[] authorities)
        {
            Authorities.AddRange(authorities);
        }

        /// <summary>
        /// 
        /// </summary>
        public List<SystemAuthority> Authorities { set; get; } = new List<SystemAuthority>();
        /// <summary>
        /// 
        /// </summary>
        public APIContext APIContext { set; get; }

        public async Task Invoke(APIContext context)
        {
            APIContext = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //var currentUser = APIContext.GetCurrentUser();
            var endpoint = context.HttpContext.Features.Get<IEndpointFeature>()?.Endpoint;
            if (endpoint != null && endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
            {
                base.OnActionExecuting(context);
            }
            else
            {
                context.Result = new UnauthorizedObjectResult("Unauthorized Access");
                return;
            }

            //var sessionId = CurrentUser.GetSessionId(context.HttpContext);
            //if (sessionId.IsNullOrEmpty())//登录状态失效
            //{
            //    context.Result = new RedirectResult("/Home/Login");
            //    return;
            //}
            //RedisCache redis = (RedisCache)context.HttpContext.RequestServices.GetService(typeof(RedisCache));
            //CurrentUser user = CurrentUser.GetCurrentUser(redis, sessionId);
            //var userAuthorities = user?.Authorities;
            //if (userAuthorities == null || userAuthorities.Count == 0)//权限异常
            //{
            //    context.Result = new RedirectResult("/Home/Login");
            //    return;
            //}
            //if (userAuthorities.FirstOrDefault(c => Authorities.Contains(c)) == Authority.None)//缺少访问权限
            //{
            //    context.Result = new RedirectResult("/Home/NoAccess");
            //    return;
            //}
            //context.Result
        }
    }
}


