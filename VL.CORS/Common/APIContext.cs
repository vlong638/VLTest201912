using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RedisSolution;
using Autobots.Infrastracture.Common.ValuesSolution;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Text;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class APIContext
    {
        /// <summary>
        /// 
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public HttpContext HttpContext { set { HttpContextAccessor.HttpContext = value; } get { return HttpContextAccessor.HttpContext; } }

        /// <summary>
        /// 
        /// </summary>
        public RedisCache RedisCache { set; get; }

        ///// <summary>
        ///// 获取当前用户信息
        ///// </summary>
        ///// <returns></returns>
        //public CurrentUser GetCurrentUser()
        //{
        //    var sessionId = CurrentUser.GetSessionId(HttpContext);
        //    return CurrentUser.GetCurrentUser(RedisCache, sessionId);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //public APIContext(IHttpContextAccessor httpContext, RedisCache redisCache) : base()
        //{
        //    HttpContextAccessor = httpContext;
        //    RedisCache = redisCache;
        //}

        /// <summary>
        /// 
        /// </summary>
        public APIContext(IHttpContextAccessor httpContext, RedisCache redisCache) : base()
        {
            HttpContextAccessor = httpContext;
            RedisCache = redisCache;
        }

        #region Common

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetWebPath()
        {
            var request = HttpContext.Request;
            return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .Append(request.PathBase)
                .ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public DbContext GetDBContext(string source)
        {
            var connectionString = APIContraints.DBConfig.ConnectionStrings.FirstOrDefault(c => c.Key == source);
            if (connectionString == null)
            {
                throw new NotImplementedException("尚未支持该类型的dbContext构建");
            }
            return new DbContext(DBHelper.GetDbConnection(connectionString.Value));
        }

        internal CurrentUser GetCurrentUser()
        {
            StringValues sessionId = StringValues.Empty;
            HttpContext.Request.Headers.TryGetValue("VLSession", out sessionId);
            if (sessionId.FirstOrDefault().IsNullOrEmpty())
                return null;
            var currentUser = RedisCache.Get<CurrentUser>(sessionId);
            if (currentUser == null)
                throw new NotImplementedException("当前用户不存在");
            return currentUser;
        }

        internal string SetCurrentUser(CurrentUser currentUser)
        {
            var sessionId = currentUser.GetSessionId();
            RedisCache.Set(sessionId, currentUser, DateTime.Now.AddHours(24));//TODO 这里时效应该是30分钟 根据用户操作来更新
            //var currentUser = RedisCache.Get<CurrentUser>(sessionId);
            return sessionId;
        }

        #endregion
    }
}
