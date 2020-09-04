namespace VL.Research.Models
{
    /// <summary>
    /// 请求参数实体
    /// </summary>
    public class GetListConfigRequest
    {
        /// <summary>
        /// 列表名称
        /// </summary>
        public string ViewName { set; get; }
        /// <summary>
        /// 自定义配置Id
        /// </summary>
        public long CustomConfigId { set; get; }
        /// <summary>
        /// 默认参数
        /// </summary>
        public string DefaultParams { set; get; }
        /// <summary>
        /// 默认参数
        /// </summary>
        public string HiddenParams { set; get; }
    }
}