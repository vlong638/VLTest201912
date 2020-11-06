using Autobots.Common.ServiceExchange;
using System;
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
        public APIResult<SyncResult> StartSync(string syncType)
        {
            var syncId = Guid.NewGuid();
            return new APIResult<SyncResult>()
            {
                Data = new SyncResult()
                {
                    SyncId = syncId.ToString(),
                    SyncStatus = SyncStatus.Waiting,
                }
            };
        }

        [WebMethod]
        public SyncResult GetSyncStatus(string syncId)
        {
            return new SyncResult() { SyncStatus = SyncStatus.Waiting };
        }
    }

    /// <summary>
    /// 同步任务状态
    /// </summary>
    public enum SyncStatus
    {
        None,
        Waiting,
        Processing,
        Completed,
        Error,
    }

    /// <summary>
    /// 同步任务结果
    /// </summary>
    public class SyncResult
    {
        /// <summary>
        /// 同步任务Id
        /// </summary>
        public string SyncId { set; get; }

        /// <summary>
        /// 同步任务
        /// </summary>
        public SyncStatus SyncStatus { set; get; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { set; get; }
    }
}
