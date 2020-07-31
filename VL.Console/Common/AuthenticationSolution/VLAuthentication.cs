//using System.Collections.Generic;
//using System.Linq;

//namespace VL.Consolo_Core.AuthenticationSolution
//{
//    public class VLAuthenticationAttribute : AuthorizeAttribute
//    {
//        public VLAuthenticationAttribute()
//        {
//            Authorities = new List<Authority>();
//        }

//        public VLAuthenticationAttribute(params Authority[] authorities)
//        {
//            Authorities = authorities.ToList();
//        }

//        public List<Authority> Authorities { set; get; }

//        public override void OnAuthorization(AuthorizationContext filterContext)
//        {
//            var isAuth = false;
//            bool flag = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
//               filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.IsDefined(typeof(AllowAnonymousAttribute), true);
//            if (flag)
//            {
//                base.OnAuthorization(filterContext);
//                return;
//            }
//            if (filterContext.RequestContext.HttpContext.Request.IsAuthenticated)
//            {
//                //CheckOn Authorities.Count()>0
//                var currentUser = CurrentUser.GetCurrentUser(filterContext.RequestContext.HttpContext);
//                if (Authorities.Count() == 0 || currentUser.AuthorityIds.FirstOrDefault(c => Authorities.FirstOrDefault(d => (long)d == c) > 0) > 0)
//                {
//                    isAuth = true;
//                }
//            }
//            else
//            {
//                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "account", action = "OnError", returnMessage = "用户尚未登录." }));
//                return;
//            }
//            if (!isAuth)
//            {
//                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "account", action = "OnError", returnMessage = "您无权查看." }));
//                return;
//            }
//            else
//            {
//                base.OnAuthorization(filterContext);
//            }
//        }
//    }
//}


    