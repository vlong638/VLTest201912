using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class VLAuthenticationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public VLAuthenticationAttribute()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var endpoint = context.HttpContext.Features.Get<IEndpointFeature>()?.Endpoint;
            if (endpoint != null && (endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>() != null || endpoint.Metadata.GetMetadata<VLAuthorizeAttribute>() != null))
            {
                base.OnActionExecuting(context);
                return;
            }
            var currentUser = CurrentUser.GetCurrentUser(context.HttpContext, StaticAPIContext.RedisCache);
            if (currentUser == null)
            {
                context.Result = new UnauthorizedObjectResult("Unauthorized");
            }
            base.OnActionExecuting(context);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VLAuthorizeAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public VLAuthorizeAttribute(params SystemAuthority[] authorities)
        {
            Authorities.AddRange(authorities.Select(c => (long)c));
        }

        /// <summary>
        /// 
        /// </summary>
        public List<long> Authorities { set; get; } = new List<long>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var endpoint = context.HttpContext.Features.Get<IEndpointFeature>()?.Endpoint;
            if (endpoint != null && endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
            {
                base.OnActionExecuting(context);
                return;
            }
            var currentUser = CurrentUser.GetCurrentUser(context.HttpContext, StaticAPIContext.RedisCache);
            if (currentUser == null)
            {
                context.Result = new UnauthorizedObjectResult("Unauthorized");
            }
            if (!currentUser.UserAuthorityIds.Any(c=>Authorities.Contains(c)))
            {
                context.Result = new UnauthorizedObjectResult("Unauthorized Access To Action");
            }
            base.OnActionExecuting(context);
        }
    }
}


