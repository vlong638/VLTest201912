using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using VL.Consolo_Core.Common.RedisSolution;

namespace BBee.Common
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
        /// 
        /// </summary>
        public List<Authority> Authorities { set; get; }

        /// <summary>
        /// 缓存当前用户信息
        /// </summary>
        /// <param name="redisCache"></param>
        /// <param name="sessionId"></param>
        /// <param name="currentUser"></param>
        public static void SetCurrentUser(RedisCache redisCache, string sessionId,CurrentUser currentUser)
        {
            redisCache.Set(sessionId, currentUser, DateTime.Now.AddMinutes(5));
        }

        /// <summary>
        /// 从缓存获取当前用户信息
        /// </summary>
        /// <param name="redisCache"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static CurrentUser GetCurrentUser(RedisCache redisCache, string sessionId)
        {
            CurrentUser user;
            redisCache.TryGet(sessionId, out user);
            return user;
        }
        /// <summary>
        /// 设置SessionId到Cookie
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="sessionId"></param>
        public static void SetSessionId(HttpContext httpContext, string sessionId)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, sessionId));
            var claimsIdentity = new ClaimsIdentity(claims, "CustomApiKeyAuth");
            var userPrincipal = new ClaimsPrincipal(claimsIdentity);
            httpContext.SignInAsync(VLAuthenticationHandler.ShemeName, userPrincipal, new AuthenticationProperties());
        }
        /// <summary>
        /// 从Cookie获取SessionId
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string GetSessionId(HttpContext httpContext)
        {
            var sessionId = httpContext.Request.Cookies[VLAuthenticationHandler.Cookie_AuthName];
            return sessionId;
        }
    }
}