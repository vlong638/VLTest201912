using Dapper.Contrib.Extensions;
using System;
using System.ComponentModel;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class ProjectSchedule
    {
        public const string TableName = "ProjectSchedule";

        public long Id { set; get; }
        public long ProjectId { set; get; }
        public long TaskId { set; get; }

        public DateTime? StartedAt { set; get; }
        public ScheduleStatus Status { set; get; }
        public DateTime? LastCompletedAt { set; get; }
        public string ResultFile { set; get; }

        internal bool IsWorking()
        {
            return Status == ScheduleStatus.Ready || Status == ScheduleStatus.Started;
        }
    }

    public enum ScheduleStatus
    {
        None = 0,
        /// <summary>
        /// 等待执行
        /// </summary>
        [Description("等待执行")]
        Ready = 1,
        /// <summary>
        /// 正在执行
        /// </summary>
        [Description("正在执行")]
        Started = 2,
        /// <summary>
        /// 执行成功
        /// </summary>
        [Description("执行成功")]
        Completed = 3,
        /// <summary>
        /// 执行失败
        /// </summary>
        [Description("执行失败")]
        Failed = 4,
    }
}
