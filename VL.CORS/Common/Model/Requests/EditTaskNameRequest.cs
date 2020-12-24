using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    public class EditTaskNameRequest
    {
        public long TaskId { set; get; }
        public string TaskName { set; get; }
    }
}
