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
        public long BusinessEntityId { set; get; }
        public long BusinessEntityPropertyId { set; get; }
        public string EntityName { set; get; }
        public string PropertyName { set; get; }
        public string Operator { set; get; }
        public string Value { set; get; }
    }

    public class GetTaskModel
    {
        public long ProjectId { set; get; }
        public long TaskId { set; get; }
        public string TaskName { set; get; }
        public List<GetTaskWhereModel> Wheres { set; get; }
    }

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
