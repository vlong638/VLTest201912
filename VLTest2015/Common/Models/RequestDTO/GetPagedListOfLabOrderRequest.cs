using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VLTest2015.Services;
using VLVLTest2015.Common.Pager;

namespace VLTest2015.Common.Models.RequestDTO
{
    public class GetPagedListOfLabOrderRequest : VLPageRequest, IQueriablePagedList
    {
        public long? PregnantInfoId { set; get; }

        Dictionary<string, object> args = new Dictionary<string, object>();
        List<string> wheres = new List<string>();

        public Dictionary<string, object> GetParams()
        {
            if (args.Count > 0)
                return args;

            if (PregnantInfoId.HasValue && PregnantInfoId != 0)
            {
                args.Add(nameof(PregnantInfoId), $"%{PregnantInfoId}%");
            }
            return args;
        }

        public string GetWhereCondition()
        {
            if (wheres.Count == 0)
            {
                wheres.Add($"PregnantInfoId  = @PregnantInfoId");
            }
            return wheres.Count == 0 ? "" : "where " + string.Join(" and", wheres);
        }

        public string ToCountSQL()
        {
            return $@"
select count(*)
from {nameof(T_LabOrder)}
{GetWhereCondition()}
";
        }

        public string ToListSQL()
        {
            if (Orders.Count == 0)
            {
                Orders.Add("Id", false);
            }
            return $@"
select *
from {nameof(T_LabOrder)}
{GetWhereCondition()}
{GetOrderCondition()}
{GetLimitCondition()}
";
        }
    }
}