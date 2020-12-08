using System.Collections.Generic;

namespace ResearchAPI.Common
{
    /// <summary>
    /// 条件集
    /// </summary>
    public class WhereSet
    {
        public List<IWhere> Wheres { get; set; } = new List<IWhere>();
        public List<WhereLinker> WhereLinkers { get; set; } = new List<WhereLinker>();
    }
}
