using System.Web.Services;

namespace Autobots.CommonServices.Services
{
    /// <summary>
    /// DataSyncService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class DataSyncService : System.Web.Services.WebService
    {
        [WebMethod]
        public bool SyncData(string syncType)
        {
            return true;
        }
    }
}
