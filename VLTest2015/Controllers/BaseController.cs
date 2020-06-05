using NPOI.OpenXmlFormats.Dml;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VLTest2015.Attributes;
using VLTest2015.Authentication;
using VLTest2015.Common.Controllers;

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

        #region APIResult,便捷方法
        public JsonResult Success<T>(T data)
        {
            return Json(new APIResult<T>(data));
        }
        public JsonResult Error<T>(T data, IList<string> messages)
        {
            return Json(new APIResult<T>(data, messages.ToArray()));
        }
        public JsonResult Error<T>(T data, params string[] messages)
        {
            return Json(new APIResult<T>(data, messages));
        }
        public JsonResult Error<T>(T data, int code, params string[] messages)
        {
            return Json(new APIResult<T>(data, code, messages));
        }
        #endregion

        #region FileDownload

        public FileResult Download(string path, string fileName)
        {
            return File(path, "text/plain", fileName);
        }

        #endregion
    }
}