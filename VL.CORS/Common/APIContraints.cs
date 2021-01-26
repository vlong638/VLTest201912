namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 静态量,常量
    /// </summary>
    public class APIContraints
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        public static DBConfig DBConfig { set; get; }

        public static string ResearchDbContext { set; get; } = "ResearchConnectionString";
        public static EasyResearchConfig EasyResearchConfig { get; internal set; }
    }
}
