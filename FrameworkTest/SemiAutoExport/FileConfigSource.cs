using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTest0213.SemiAutoExport
{
    /// <summary>
    /// XML配置,表结构
    /// </summary>
    public class FileConfigSource
    {
        public int FunctionCategory { set; get; }
        public int SubFunctionCategory { set; get; }
        public string JoinSQL { set; get; }

        public List<FileConfigField> Fields { set; get; }
    }
}
