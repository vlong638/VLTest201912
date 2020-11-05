using Autobots.Common.ServiceCommon;
using Autobots.Common.ServiceExchange;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Services;

namespace Autobots.EMRServices.Services.WebServices
{
    /// <summary>
    /// 电子病历基础服务
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class EMRBaseWebService : System.Web.Services.WebService
    {
        /// <summary>
        /// 通用新增
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        [WebMethod]
        public async Task<APIResult<string>> CommonCreate(string content)
        {
            return new APIResult<string>(null);
        }

        /// <summary>
        /// 通用更新
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        [WebMethod]
        public async Task<APIResult<string>> CommonUpdate(string content)
        {
            return new APIResult<string>(null);
        }

        /// <summary>
        /// 通用删除
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        [WebMethod]
        public async Task<APIResult<string>> CommonDelete(string content)
        {
            return new APIResult<string>(null);
        }

        /// <summary>
        /// 通用列表
        /// B超,BScanMeasurement
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        [WebMethod]
        public async Task<APIResult<IPager<List<Dictionary<string, object>>>>> CommonGetList()
        {
            return new APIResult<IPager<List<Dictionary<string, object>>>>(null);
        }
    }
}
