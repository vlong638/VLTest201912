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
            return CurrentUser.GetCurrentUser(HttpContext, RedisCache);
        }

        internal string SetCurrentUser(CurrentUser currentUser)
        {
            return CurrentUser.SetCurrentUser(RedisCache, currentUser);
        }

        #endregion
    }
}
