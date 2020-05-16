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
                args.Add(nameof(PregnantInfoId), PregnantInfoId);
            }
            return args;
        }

        public string GetWhereCondition()
        {
            if (wheres.Count == 0)
            {
                //wheres.Add($"l.PregnantInfoId  = @PregnantInfoId");
            }
            return wheres.Count == 0 ? "" : "where " + string.Join(" and", wheres);
        }

        public string ToCountSQL()
        {
            return $@"
select count(*)
from {LabOrder.TableName} l
inner join {PregnantInfo.TableName} p on p.idcard = l.idcard and p.id= @PregnantInfoId
{GetWhereCondition()}
";
        }

        public string ToListSQL()
        {
            if (Orders.Count == 0)
            {
                Orders.Add("l.Id", false);
            }
            return $@"
select l.*
from {LabOrder.TableName} l
inner join {PregnantInfo.TableName} p on p.idcard = l.idcard and p.Id  = @PregnantInfoId
{GetWhereCondition()}
{GetOrderCondition()}
{GetLimitCondition()}
";
        }
    }
}