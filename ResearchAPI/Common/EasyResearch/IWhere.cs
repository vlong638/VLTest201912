using System.Collections.Generic;

namespace ResearchAPI.Common
{
    /// <summary>
    /// 筛选条件接口
    /// </summary>
    public interface IWhere
    {
        string ToSQL(Dictionary<string, string> tableAlias);
        KeyValuePair<string, object>? GetParameter();
    }
}
