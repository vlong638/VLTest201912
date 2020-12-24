using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    public class EditTaskRequest
    {
        public long TaskId { set; get; }
        public List<TaskWhereModel> Wheres { set; get; }
    }

    public class TaskWhereModel
    {
        public long BusinessEntityId { set; get; }
        public long BusinessEntityPropertyId { set; get; }
        public string PropertyName { set; get; }
        public string Operator { set; get; }
        public string Value { set; get; }
    }
}
