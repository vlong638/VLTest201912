﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace VL.Consoling.SemiAutoExport
{
    /// <summary>
    /// 项目分类,0 000 00中的第2-4位
    /// </summary>
    public enum SubFunctionCategory
    {
        [Description("女方信息Description")]
        女方信息 = 1011,
        女方既往史 = 1012,
        女方性病史 = 1013,
    }
}
