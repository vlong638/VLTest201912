using System.Collections.Generic;

namespace VLVLTest2015.Common.Pager
{
    public interface IQueriablePagedList
    {
        string GetWhereCondition();
        Dictionary<string, object> GetParams();
        string ToListSQL();
        string ToCountSQL();
    }
}
