using System;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class GetProjectOperateHistoryModel
    {
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateAt { set; get; }
        /// <summary>
        /// 操作概要描述
        /// </summary>
        public string OperatorSummary { set; get; }
    }
}
