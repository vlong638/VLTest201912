using Dapper.Contrib.Extensions;
using System;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class ProjectTask
    {
        public const string TableName = "ProjectTask";

        public long Id { set; get; }
        public long ProjectId { set; get; }
        /// <summary>
        /// 队列名称
        /// </summary>
        public string Name { set; get; }
    }
}
