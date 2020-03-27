using System.Web.Mvc;
using VLTest2015.Attributes;
using VLTest2015.Authentication;

namespace VLTest2015.Controllers
{
    [VLAuthentication]
    public class BaseController : Controller
    {
        #region Common
        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        protected void AddErrors(params string[] errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        #endregion

        #region Auth
        public void SetCurrentUser(CurrentUser currentUser, bool isRemeberMe = false)
        {
            CurrentUser.SetCurrentUser(currentUser, isRemeberMe, Response);
        }


        public CurrentUser GetCurrentUser()
        {
            var httpContext = HttpContext;
            return CurrentUser.GetCurrentUser(httpContext);
        } 
        #endregion
    }
}