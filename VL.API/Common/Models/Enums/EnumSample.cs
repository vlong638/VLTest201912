using System.ComponentModel;

namespace VL.API.PT.Enums
{
    /// <summary>
    /// 剖宫产位置
    /// </summary>
    public enum CaesareanPosition
    {
        [Description("")]
        None = 0,
        [Description("低颈")]
        低颈 = 1,
        [Description("古典")]
        古典 = 2,
        [Description("腹膜外")]
        腹膜外 = 3,
        [Description("其他")]
        其他 = 4,
    }
}
