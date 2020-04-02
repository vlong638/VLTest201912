using System.Collections.Generic;

namespace VL.API.Common.Models
{
    /// <summary>
    /// 复杂分页入参样例
    /// </summary>
    public class GetComplexPagedListSampleRequest : VLPageRequest, IQueriablePagedList
    {
        public int? AgeTop { set; get; }
        public int? AgeBottom { set; get; }

        Dictionary<string, object> args = new Dictionary<string, object>();
        List<string> wheres = new List<string>();

        public Dictionary<string, object> GetParams()
        {
            if (AgeTop.HasValue)
            {
                args.Add("@AgeTop", AgeTop);
            }
            if (AgeBottom.HasValue)
            {
                args.Add("@AgeBottom", AgeBottom);
            }
            return args;
        }

        public string GetWhereCondition()
        {
            if (AgeTop.HasValue)
            {
                wheres.Add($"AgeTop<@AgeTop");
            }
            if (AgeBottom.HasValue)
            {
                wheres.Add($"AgeBottom>@AgeBottom");
            }
            return wheres.Count == 0 ? "" : "where " + string.Join(",", wheres);
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
