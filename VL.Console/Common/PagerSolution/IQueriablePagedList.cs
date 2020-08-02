using System.Collections.Generic;

namespace VL.Consolo_Core.Common.PagerSolution
{
    public interface IQueriablePagedList
    {
        string GetWhereCondition();
        Dictionary<string, object> GetParams();
        string ToListSQL();
        string ToCountSQL();
    }
}
