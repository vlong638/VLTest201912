using System.Collections.Generic;
using VL.Consolo_Core.Common.PagerSolution;

namespace VL.Research.Models
{
    public class GetPagedListOfVisitRecordRequest : VLPagerRequest, IQueriablePagedList
    {
        public long? PregnantInfoId { set; get; }

        Dictionary<string, object> args = new Dictionary<string, object>();
        List<string> wheres = new List<string>();

        public Dictionary<string, object> GetParams()
        {
            if (args.Count > 0)
                return args;

            if (PregnantInfoId.HasValue&& PregnantInfoId!=0)
            {
                args.Add(nameof(PregnantInfoId), PregnantInfoId);
            }
            return args;
        }

        public string GetWhereCondition()
        {
            if (wheres.Count == 0)
            {
                //wheres.Add($"v.PregnantInfoId = @PregnantInfoId");
            }
            return wheres.Count == 0 ? "" : "where " + string.Join(" and", wheres);
        }

        public string ToCountSQL()
        {
            return $@"
select count(*)
from {VisitRecord.TableName} v
inner join {PregnantInfo.TableName} p on p.idcard = v.idcard and p.Id = @PregnantInfoId
{GetWhereCondition()}
";
        }

        public string ToListSQL()
        {
            if (Orders.Count == 0)
            {
                Orders.Add("v.Id", false);
            }
            return $@"
select v.*
from {VisitRecord.TableName} v
inner join {PregnantInfo.TableName} p on p.idcard = v.idcard and p.Id = @PregnantInfoId
{GetWhereCondition()}
{GetOrderCondition()}
{GetLimitCondition()}
";
        }
    }
}