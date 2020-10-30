using System.ComponentModel;

namespace BBee.Models.Enums
{
    /// <summary>
    /// 分娩状态
    /// </summary>
    public enum DeliveryStatus
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        None = -1,
        /// <summary>
        /// 未分娩
        /// </summary>
        [Description("未分娩")]
        Undone = 0,
        /// <summary>
        /// 已分娩
        /// </summary>
        [Description("已分娩")]
        Done = 1,
    }
}
