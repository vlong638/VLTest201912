using System;
using System.Collections.Generic;
using System.Text;

namespace VL.Consoling.SemiAutoExport
{
    /// <summary>
    /// 字段,0 000 00中的第5-6位
    /// </summary>
    public enum SubFunctionField
    {
        女方信息_助记符 = 101101,
        女方信息_姓名 = 101102,
        女方信息_年龄 = 101103,
        女方信息_体重 = 101104,
        女方信息_职业 = 101105,
        女方信息_单位 = 101106,
        女方信息_性别 = 101107,
        女方性病史_名称 = 101308,
        女方性病史_编码 = 101309,
    }
}
