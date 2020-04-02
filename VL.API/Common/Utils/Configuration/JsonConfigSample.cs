namespace VL.API.Common.Utils.Configuration
{
    /// <summary>
    /// 配置样例
    /// </summary>
    public class JsonConfigSample
    {
        public LogLevelConfig LogLevel { set; get; }

    }
    /// <summary>
    /// 配置样例
    /// </summary>
    public class LogLevelConfig
    {
        public string Default { set; get; }
        public string Microsoft { set; get; }
    }
}
