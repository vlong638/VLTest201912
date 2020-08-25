using System.ComponentModel;

namespace VL.Research.Models.Enums
{
    /// <summary>
    /// 结案 状态
    /// </summary>
    public enum SettleStatus
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        None = -1,
        /// <summary>
        /// 未结案
        /// </summary>
        [Description("未结案")]
        Undone = 0,
        /// <summary>
        /// 已结案
        /// </summary>
        [Description("已结案")]
        Done = 1,
    }
}
