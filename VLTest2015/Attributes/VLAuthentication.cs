using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace VLTest2015.Attributes
{
    public class VLAuthenticationAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var isAuth = false;
            if (!filterContext.RequestContext.HttpContext.Request.IsAuthenticated)
            {
                isAuth = false;
            }
            else
            {
                isAuth = true;

                //if (filterContext.RequestContext.HttpContext.User.Identity != null)
                //{
                //    var roleApi = new RoleApi();
                //    var actionDescriptor = filterContext.ActionDescriptor;
                //    var controllerDescriptor = actionDescriptor.ControllerDescriptor;
                //    var controller = controllerDescriptor.ControllerName;
                //    var action = actionDescriptor.ActionName;
                //    var ticket = (filterContext.RequestContext.HttpContext.User.Identity as FormsIdentity).Ticket;
                //    var role = roleApi.GetById(ticket.Version);
                //    if (role != null)
                //    {
                //        isAuth = role.Permissions.Any(x => x.Permission.Controller.ToLower() == controller.ToLower() && x.Permission.Action.ToLower() == action.ToLower());
                //    }
                //}
            }
            if (!isAuth)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "account", action = "login", returnUrl = filterContext.HttpContext.Request.Url, returnMessage = "您无权查看." }));
                return;
            }
            else
            {
                base.OnAuthorization(filterContext);
            }
        }
    }
}


    