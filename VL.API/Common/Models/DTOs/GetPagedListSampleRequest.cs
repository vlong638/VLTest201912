using System.Collections.Generic;

namespace VL.API.Common.Models
{
    public class GetPagedListSampleRequest : VLPageRequest, IQueriablePagedList
    {
        public Dictionary<string, object> GetParams()
        {
            return new Dictionary<string, object>();
        }

        public string GetWhereCondition()
        {
            return "";
        }

        public string ToCountSQL()
        {
            return $@"
select count(*)
from PregnantInfo
{GetWhereCondition()}
";
        }

        public string ToListSQL()
        {
            return $@"
select *
from PregnantInfo
{GetWhereCondition()}
{GetOrderCondition()}
{GetLimitCondition()}
";
        }
    }
}
