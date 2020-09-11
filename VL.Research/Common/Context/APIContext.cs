using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.RedisSolution;
using VL.Research.Common.Configuration;

namespace VL.Research.Common
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
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        public CurrentUser GetCurrentUser() {
            var sessionId = CurrentUser.GetSessionId(HttpContext);
            return CurrentUser.GetCurrentUser(RedisCache,sessionId);
        }

        /// <summary>
        /// 
        /// </summary>
        public DbContext CommonDbContext { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public DbContext FYPTDbContext { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public DbContext SZXTDbContext { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public APIContext(IHttpContextAccessor httpContext, IOptions<DBConfig> loggingConfig, RedisCache redisCache) : base()
        {
            HttpContextAccessor = httpContext;

            CommonDbContext = new DbContext(DBHelper.GetDbConnection(loggingConfig.Value.DefaultConnectionString));
            FYPTDbContext = new DbContext(DBHelper.GetDbConnection(loggingConfig.Value.FYPTConnectionString));
            SZXTDbContext = new DbContext(DBHelper.GetDbConnection(loggingConfig.Value.SZXTConnectionString));

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
        public DbContext GetDBContext(DBSourceType source)
        {
            switch (source)
            {
                case DBSourceType.DefaultConnectionString:
                    return CommonDbContext;
                case DBSourceType.FYPTConnectionString:
                    return FYPTDbContext;
                case DBSourceType.SZXTConnectionString:
                    return SZXTDbContext;
                default:
                    throw new NotImplementedException("尚未支持该类型的dbContext构建");
            }
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public enum DBSourceType
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// 电子病历(Pregnant)
        /// </summary>
        DefaultConnectionString,
        /// <summary>
        /// 妇幼平台
        /// </summary>
        FYPTConnectionString,
        /// <summary>
        /// 生殖系统
        /// </summary>
        SZXTConnectionString,
    }
}
