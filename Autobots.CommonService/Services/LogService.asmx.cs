using Autobots.Common.ServiceExchange;
using Autobots.CommonServices.Utils;
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
    public class LogService : System.Web.Services.WebService
    {
        [WebMethod]
        public APIResult Info(string message)
        {
            try
            {
                Log4NetLogger.Info(message);
                return new APIResult();
            }
            catch (System.Exception ex)
            {
                return new APIResult(500, ex.Message);
            }
        }

        [WebMethod]
        public APIResult Warn(string message)
        {
            try
            {
                Log4NetLogger.Warn(message);
                return new APIResult();
            }
            catch (System.Exception ex)
            {
                return new APIResult(500, ex.Message);
            }
        }

        [WebMethod]
        public APIResult Error(string message)
        {
            try
            {
                Log4NetLogger.Error(message);
                return new APIResult();
            }
            catch (System.Exception ex)
            {
                return new APIResult(500, ex.Message);
            }
        }
    }
}
