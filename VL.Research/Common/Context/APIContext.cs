using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using VL.Consolo_Core.Common.DBSolution;
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
        public APIContext(IHttpContextAccessor httpContext, IOptions<DBConfig> loggingConfig) : base()
        {
            HttpContextAccessor = httpContext;
            CommonDbContext = new DbContext(DBHelper.GetDbConnection(loggingConfig.Value.ConnectionString));
            FYPTDbContext = new DbContext(DBHelper.GetDbConnection(loggingConfig.Value.FYPTConnectionString));
            SZXTDbContext = new DbContext(DBHelper.GetDbConnection(loggingConfig.Value.SZXTConnectionString));
        }

        #region Auth
        //public void SetCurrentUser(CurrentUser currentUser, bool isRemeberMe = false)
        //{
        //    CurrentUser.SetCurrentUser(currentUser, isRemeberMe, HttpContext);
        //}


        //public CurrentUser GetCurrentUser()
        //{
        //    var httpContext = HttpContext;
        //    return CurrentUser.GetCurrentUser(httpContext);
        //}
        #endregion

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
                case DBSourceType.Pregnant:
                    return CommonDbContext;
                case DBSourceType.FYPT:
                    return FYPTDbContext;
                case DBSourceType.SZXT:
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
        /// 电子病历(Pregnant)
        /// </summary>
        Pregnant,
        /// <summary>
        /// 妇幼平台
        /// </summary>
        FYPT,
        /// <summary>
        /// 生殖系统
        /// </summary>
        SZXT,
    }
}
