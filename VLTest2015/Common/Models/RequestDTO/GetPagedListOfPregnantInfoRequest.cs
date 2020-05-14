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
        public string Name { set; get; }

        Dictionary<string, object> args = new Dictionary<string, object>();
        List<string> wheres = new List<string>();

        public Dictionary<string, object> GetParams()
        {
            if (args.Count > 0)
                return args;

            if (!string.IsNullOrEmpty(Name))
            {
                args.Add(nameof(Name), $"%{Name}%");
            }
            return args;
        }

        public string GetWhereCondition()
        {
            if (wheres.Count == 0)
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    wheres.Add($"Name Like @Name");
                }
            }
            return wheres.Count == 0 ? "" : "where " + string.Join(" and", wheres);
        }

        public string ToCountSQL()
        {
            return $@"
select count(*)
from {nameof(T_PregnantInfo)}
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
from {nameof(T_PregnantInfo)}
{GetWhereCondition()}
{GetOrderCondition()}
{GetLimitCondition()}
";
        }
    }
}