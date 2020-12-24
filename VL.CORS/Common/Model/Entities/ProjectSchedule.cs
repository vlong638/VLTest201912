using Dapper.Contrib.Extensions;
using System;

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
        None,
        Ready,
        Started,
        Completed,
        Failed,
    }
}
