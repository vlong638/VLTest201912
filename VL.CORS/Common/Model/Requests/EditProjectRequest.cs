using Autobots.Infrastracture.Common.PagerSolution;

namespace ResearchAPI.CORS.Common
{

    /// <summary>
    /// 编辑项目
    /// </summary>
    public class EditProjectRequest : ProjectDTO
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public long ProjectId { set; get; }
    }
}
