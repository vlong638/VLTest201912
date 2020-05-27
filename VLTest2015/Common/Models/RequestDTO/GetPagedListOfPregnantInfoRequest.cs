using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VLTest2015.Services;
using VLVLTest2015.Common.Pager;

namespace VLTest2015.Common.Models.RequestDTO
{
    public class GetPagedListOfPregnantInfoRequest : VLPageRequest, IQueriablePagedList
    {
        public string PersonName { set; get; }
        public List<string> FieldNames { get; internal set; }

        Dictionary<string, object> args = new Dictionary<string, object>();
        List<string> wheres = new List<string>();

        public Dictionary<string, object> GetParams()
        {
            if (args.Count > 0)
                return args;

            if (!string.IsNullOrEmpty(PersonName))
            {
                args.Add(nameof(PersonName), $"%{PersonName}%");
            }
            return args;
        }

        public string GetWhereCondition()
        {
            if (wheres.Count == 0)
            {
                if (!string.IsNullOrEmpty(PersonName))
                {
                    wheres.Add($"{nameof(PersonName)} Like @PersonName");
                }
            }
            return wheres.Count == 0 ? "" : "where " + string.Join(" and", wheres);
        }

        public string ToCountSQL()
        {
            return $@"
select count(*)
from {PregnantInfo.TableName}
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
select {string.Join(",", FieldNames)}
from {PregnantInfo.TableName}
{GetWhereCondition()}
{GetOrderCondition()}
{GetLimitCondition()}
";
        }
    }
}