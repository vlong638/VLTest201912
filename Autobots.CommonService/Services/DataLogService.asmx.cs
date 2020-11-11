using Autobots.Common.ServiceBase;
using Autobots.Common.ServiceCommon;
using System.Collections.Generic;
using System.Web.Services;

namespace Autobots.CommonServices.Services
{
    /// <summary>
    /// DataLogService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class DataLogService : System.Web.Services.WebService, IHealthCheck
    {
        #region IHealthCheck
        public bool IsAlive()
        {
            throw new System.NotImplementedException();
        }
        public List<ReferenceCheckReport> GetReferenceCheckReports()
        {
            throw new System.NotImplementedException();
        }
        public LoadingCheckReport GetLoadingCheckReport()
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region CommonDataLogs

        /// <summary>
        /// 保存数据日志
        /// </summary>
        /// <param name="sourceId">来源Id</param>
        /// <param name="operateBy">操作人Id</param>
        /// <param name="operateType">操作类型:(新增,删除,更改,编辑新增(子项),编辑删除(子项))</param>
        /// <param name="oldContent">老数据</param>
        /// <param name="newContent">新数据</param>
        /// <returns></returns>
        [WebMethod]
        public bool SaveDataLog(string sourceId, string operateBy, string operateType, string oldContent, string newContent)
        {
            return true;
        }

        /// <summary>
        /// 对比数据日志
        /// </summary>
        /// <param name="oldContent"></param>
        /// <param name="newContent"></param>
        /// <returns></returns>
        [WebMethod]
        public List<DataLogChange> CompareDataLog(string oldContent, string newContent)
        {
            return new List<DataLogChange>() {
                new DataLogChange("姓名","王武","王五"),
                new DataLogChange("年龄","25","15"),
            };
        } 

        #endregion
    }
}
