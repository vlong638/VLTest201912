using Autobots.Common.ServiceBase;
using Autobots.Common.ServiceExchange;
using Autobots.CommonServices.Utils;
using System.Collections.Generic;
using System.Web.Services;

namespace Autobots.CommonServices.Services
{
    /// <summary>
    /// 日志服务
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class LogService : WebService
    {
        #region CommonLog
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">记录信息</param>
        /// <param name="location">定位信息</param>
        /// <param name="operator">操作人</param>
        /// <returns></returns>
        [WebMethod]
        public APIResult Info(string message, string locator, string @operator)
        {
            return WebServiceHelper.DelegateWebService(() =>
            {
                Log4NetLogger.Info(message);
                return new APIResult();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">记录信息</param>
        /// <param name="location">定位信息</param>
        /// <param name="operator">操作人</param>
        /// <returns></returns>
        [WebMethod]
        public APIResult Warn(string message, string locator, string @operator)
        {
            return WebServiceHelper.DelegateWebService(() =>
            {
                Log4NetLogger.Warn(message);
                return new APIResult();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">记录信息</param>
        /// <param name="location">定位信息</param>
        /// <param name="operator">操作人</param>
        /// <returns></returns>
        [WebMethod]
        public APIResult Error(string message, string locator, string @operator)
        {
            return WebServiceHelper.DelegateWebService(() =>
            {
                Log4NetLogger.Error(message);
                return new APIResult();
            });
        } 

        #endregion
    }
}
