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
using VL.Consolo_Core.Common.RedisSolution;
using VL.Consolo_Core.Common.ValuesSolution;

namespace VL.Research.Common
{
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sessionId = context.HttpContext.Request.Cookies[VLAuthenticationHandler.Cookie_AuthName];
            if (sessionId.IsNullOrEmpty())//登录状态失效
            {
                context.Result = new RedirectResult("/Home/Login");
                return;
            }
            RedisCache redis = (RedisCache)context.HttpContext.RequestServices.GetService(typeof(RedisCache));
            List<Authority> userAuthorities;
            redis.TryGet(sessionId, out userAuthorities);
            if (userAuthorities == null || userAuthorities.Count == 0)//权限异常
            {
                context.Result = new RedirectResult("/Home/Login");
                return;
            }
            if (userAuthorities.FirstOrDefault(c => Authorities.Contains(c)) == Authority.None)//缺少访问权限
            {
                context.Result = new RedirectResult("/Home/NoAccess");
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}


