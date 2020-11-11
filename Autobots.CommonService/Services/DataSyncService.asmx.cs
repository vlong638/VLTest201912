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
        #region CommonTasks

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task">执行什么</param>
        /// <param name="schedule">执行计划</param>
        /// <returns></returns>
        [WebMethod]
        public APIResult<SyncResult> StartSync(string taskType, string schedule)
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

        /// <summary>
        /// 获取执行任务(已经执行的,正在执行的)
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public SyncResult GetTasks()
        {
            return new SyncResult() { SyncStatus = SyncStatus.Waiting };
        }

        /// <summary>
        /// 执行报告
        /// </summary>
        /// <param name="syncId">执行项目Id</param>
        /// <returns></returns>
        [WebMethod]
        public SyncResult GetSyncStatus(string taskId)
        {
            return new SyncResult() { SyncStatus = SyncStatus.Waiting };
        } 

        #endregion
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
