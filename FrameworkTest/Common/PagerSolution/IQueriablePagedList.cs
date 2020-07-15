using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Common.PagerSolution
{
    public interface IQueriablePagedList
    {
        string GetWhereCondition();
        Dictionary<string, object> GetParams();
        string ToListSQL();
        string ToCountSQL();
    }
}
