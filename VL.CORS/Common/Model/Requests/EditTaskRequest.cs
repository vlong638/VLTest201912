using System;
using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    public class EditTaskRequest
    {
        public long TaskId { set; get; }
        public List<EditTaskWhereModel> Wheres { set; get; }
    }

    public class EditTaskWhereModel
    {
        //public long BusinessEntityId { set; get; }
        //public long BusinessEntityPropertyId { set; get; }
        //public string EntityName { set; get; }
        //public string PropertyName { set; get; }

        public long IndicatorId { set; get; }
        public string Operator { set; get; }
        public string Value { set; get; }
    }

    public class GetTaskModel
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public long ProjectId { set; get; }
        /// <summary>
        /// 队列Id
        /// </summary>
        public long TaskId { set; get; }
        /// <summary>
        /// 队列名称
        /// </summary>
        public string TaskName { set; get; }
        /// <summary>
        /// 队列条件
        /// </summary>
        public List<GetTaskWhereModel> Wheres { set; get; }

        /// <summary>
        /// 可导出文件
        /// </summary>
        public string ResultFile { set; get; }
        /// <summary>
        /// 上一次执行完成时间
        /// </summary>
        public DateTime? LastCompletedAt { set; get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetTaskWhereModel: ProjectTaskWhere
    {
        /// <summary>
        /// 操作类型的显示名称
        /// </summary>
        public string OperatorName { set; get; }
        /// <summary>
        /// 属性的显示名称
        /// </summary>
        public string DisplayName { set; get; }
    }
}
