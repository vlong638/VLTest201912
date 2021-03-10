namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class GetLatestProjectModel
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public long ProjectId { set; get; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { set; get; }
        /// <summary>
        /// 项目更新日期
        /// </summary>
        public string LastModifiedAt { set; get; }
    }
}
