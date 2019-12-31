using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VLTest2015.Attributes;
using VLTest2015.Services;

namespace VLTest2015.Controllers
{
    [VLAuthentication]
    public class BaseController : Controller
    {
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

        public void SetCurrentUser(CurrentUser currentUser, bool isRemeberMe = false)
        {
            CurrentUser.SetCurrentUser(currentUser, isRemeberMe, Response);
        }


        public CurrentUser GetCurrentUser()
        {
            var httpContext = HttpContext;
            return CurrentUser.GetCurrentUser(httpContext);
        }
    }

    /// <summary>
    /// 当前用户
    /// </summary>
    public class CurrentUser
    {
        public long UserId { set; get; }
        public string UserName { set; get; }
        public List<Authority> Authorities { set; get; }
        public List<long> AuthorityIds { set; get; }


        public static void SetCurrentUser(CurrentUser currentUser, bool isRemeberMe, HttpResponseBase response)
        {
            var userName = currentUser.UserName;
            var idAndAuth = currentUser.UserId.ToString() + "_" + string.Join(",", currentUser.AuthorityIds);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,
                userName,
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                isRemeberMe,
                idAndAuth
            //UserData有长度限制，后续建议以Session形式存储会话数据
            );
            HttpCookie cookie = new HttpCookie(
                FormsAuthentication.FormsCookieName,
                FormsAuthentication.Encrypt(ticket));
            response.Cookies.Add(cookie);
        }

        public static CurrentUser GetCurrentUser(HttpContextBase httpContext)
        {
            var cookieName = FormsAuthentication.FormsCookieName;
            var authCookie = httpContext.Request.Cookies[cookieName];
            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            var userName = httpContext.User.Identity.Name;
            var datas = authTicket.UserData.Split('_');
            var userId = 0L;
            Int64.TryParse(datas[0], out userId);
            var authorityIds = string.IsNullOrEmpty(datas[1])?new long[0]:datas[1].Split(',').Select(c => Int64.Parse(c)).ToArray();
            return new CurrentUser()
            {
                UserId = userId,
                UserName = userName,
                AuthorityIds = authorityIds.ToList(),
                Authorities = authorityIds.Select(c=>(Authority)Enum.Parse(typeof(Authority),c.ToString())).ToList()
            };
        }
    }

    /// <summary>
    /// 会话层
    /// 可在CurrentUser介于B/S两端交互过程构建缓存存储的一层
    /// </summary>
    public interface IMySession
    {
        #region （后端存储）
        /// <summary>
        /// 设置项
        /// 清空项时，将value设为null即可
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set<T>(string key, T value);

        /// <summary>
        /// 获取项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);
        #endregion

        #region （前端交互）
        /// <summary>
        /// 获取会话id
        /// </summary>
        /// <returns></returns>
        string GetSessionId();

        /// <summary>
        /// 设置会话id
        /// </summary>
        /// <param name="sessinId"></param>
        void SetSessionId(string sessinId);

        /// <summary>
        /// 用户退出
        /// </summary>
        /// <param name="sessionId"></param>
        void Clear();
        #endregion
    }
}