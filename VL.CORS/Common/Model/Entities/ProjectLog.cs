using Dapper.Contrib.Extensions;
using System;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class ProjectLog
    {
        public const string TableName = "ProjectLog";

        public long Id { set; get; }
        public long ProjectId { set; get; }
        public long OperatorId { set; get; }

        public DateTime CreatedAt { set; get; }
        public ActionType ActionType { set; get; }
    }

    public enum ActionType
    {
        None = 0,


    }
}
