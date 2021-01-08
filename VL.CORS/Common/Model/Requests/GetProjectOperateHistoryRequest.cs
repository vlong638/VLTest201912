using System;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class GetProjectOperateHistoryRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public long ProjectId { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? OperateTimeStart { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? OperateTimeEnd { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public long? OperatorId { set; get; }
    }
}
