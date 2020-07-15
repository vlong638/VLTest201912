using FrameworkTest.ConfigurableEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Common.PagerSolution
{
    public interface IQueriablePagedList
    {
        List<string> FieldNames { set; get; }
        string GetWhereCondition();
        Dictionary<string, object> GetParams();
        string ToListSQL();
        string ToCountSQL();
    }

    public static class IQueriablePagedListEx
    {
        public static void UpdateFieldNames(this IQueriablePagedList request, EntityAppConfig viewConfig)
        {
            viewConfig.Properties.RemoveAll(c => !c.IsNeedOnDatabase);
            if (viewConfig.Properties.Count > 0)
                request.FieldNames = viewConfig.Properties.Select(c => c.ColumnName).ToList();
        }
    }
}
