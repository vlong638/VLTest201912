using System.Collections.Generic;

namespace VL.API.Common.Utils.Authentication
{

    /// <summary>
    /// 当前用户
    /// </summary>
    public class CurrentUser
    {
        /// <summary>
        /// 
        /// </summary>
        public long UserId { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 权限集合
        /// </summary>
        public List<int> AuthorityIds { set; get; }


        //public static void SetCurrentUser(CurrentUser currentUser, bool isRemeberMe, HttpResponseBase response)
        //{
        //    var userName = currentUser.UserName;
        //    var userData = currentUser.UserId.ToString() + "_" + string.Join(",", currentUser.AuthorityIds);
        //    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
        //        1,
        //        userName,
        //        DateTime.Now,
        //        DateTime.Now.Add(FormsAuthentication.Timeout),
        //        isRemeberMe,
        //        userData
        //    //UserData有长度限制，后续建议以Session形式存储会话数据
        //    );
        //    HttpCookie cookie = new HttpCookie(
        //        FormsAuthentication.FormsCookieName,
        //        FormsAuthentication.Encrypt(ticket));
        //    response.Cookies.Add(cookie);
        //}

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static CurrentUser GetCurrentUser(Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            //var cookieName = FormsAuthentication.FormsCookieName;
            //var authCookie = httpContext.Request.Cookies[cookieName];
            //var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            //var userName = httpContext.User.Identity.Name;
            //var userData = authTicket.UserData.Split('_');
            //var userId = string.IsNullOrEmpty(userData[0]) ? 0 : Int64.Parse(userData[0]);
            //var authorityIds = userData.Length<2||string.IsNullOrEmpty(userData[1]) ? new long[0] : userData[1].Split(',').Select(c => Int64.Parse(c)).ToArray();
            //return new CurrentUser()
            //{
            //    UserId = userId,
            //    UserName = userName,
            //    AuthorityIds = authorityIds.ToList(),
            //    Authorities = authorityIds.Select(c => (Authority)Enum.Parse(typeof(Authority), c.ToString())).ToList()
            //};

            return new CurrentUser();
        }
    }
}