using Autobots.Infrastracture.Common.RedisSolution;
using Autobots.Infrastracture.Common.ValuesSolution;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Common
{
    public class CurrentUser
    {
        public CurrentUser()
        {
        }
        public CurrentUser(User data)
        {
            UserId = data.Id;
            UserName = data.Name;
        }

        public long UserId { set; get; }
        public string UserName { set; get; }
        public List<long> UserAuthorityIds { get; set; }

        public string GetSessionId() {
            return UserId + "_" + (UserId + DateTime.Now.ToString()).ToMD5();
        }

        internal static CurrentUser GetCurrentUser(HttpContext httpContext, RedisCache redisCache)
        {
            StringValues sessionId = StringValues.Empty;
            httpContext.Request.Headers.TryGetValue("VLSession", out sessionId);
            if (sessionId.FirstOrDefault().IsNullOrEmpty())
                return null;
            var currentUser = redisCache.Get<CurrentUser>(sessionId);
            if (currentUser == null)
                throw new NotImplementedException("当前用户不存在");
            return currentUser;
        }

        internal static string SetCurrentUser(RedisCache redisCache,CurrentUser currentUser)
        {
            var sessionId = currentUser.GetSessionId();
            redisCache.Set(sessionId, currentUser, DateTime.Now.AddHours(24));//TODO 这里时效应该是30分钟 根据用户操作来更新
            //var test = redisCache.Get<CurrentUser>(sessionId);
            return sessionId;
        }
    }
}
