﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.RedisSolution;
using BBee.Common.Configuration;

namespace BBee.Common
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
            if (connectionString==null)
            {
                    throw new NotImplementedException("尚未支持该类型的dbContext构建");
            }
            return new DbContext(DBHelper.GetDbConnection(connectionString.Value));
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
        /// <summary>
        /// 生殖系统
        /// </summary>
        HZConnectionString,
    }
}
