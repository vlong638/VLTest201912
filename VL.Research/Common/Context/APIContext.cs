using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
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
        public APIContext(IHttpContextAccessor httpContext, IOptions<DBConfig> loggingConfig) : base()
        {
            HttpContextAccessor = httpContext;
            CommonDbContext = new DbContext(DBHelper.GetDbConnection(loggingConfig.Value.ConnectionString));
            FYPTDbContext = new DbContext(DBHelper.GetDbConnection(loggingConfig.Value.FYPTConnectionString));
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
    }
}
