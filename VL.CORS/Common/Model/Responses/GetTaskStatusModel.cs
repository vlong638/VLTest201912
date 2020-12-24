using Dapper.Contrib.Extensions;
using System;

namespace ResearchAPI.CORS.Common
{
    public class GetTaskStatusModel
    {
        /// <summary>
        /// 执行状态
        /// </summary>
        public ScheduleStatus ScheduleStatus { set; get; }
        /// <summary>
        /// 执行状态_文本
        /// </summary>
        public string ScheduleStatusName { set; get; }
        /// <summary>
        /// 执行比例
        /// </summary>
        public int ProcessingRate { set; get; }
    }
}
