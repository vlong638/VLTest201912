using System;
using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    public class EditTaskRequest
    {
        public long TaskId { set; get; }
        public List<EditTaskWhereCondition> Wheres { set; get; }
    }

    public class EditTaskWhereCondition
    {
        public long IndicatorId { set; get; }
        public string Operator { set; get; }
        public string Value { set; get; }
    }
}
