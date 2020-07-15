using FrameworkTest.Common.PagerSolution;
using FrameworkTest.ConfigurableEntity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FS.SyncManager.Models
{

    public class GetPagedListOfPregnantInfoRequest : VLPageRequest, IQueriablePagedList
    {
        #region OrientInput
        public int page { get; set; }
        public int rows { get; set; }
        public string sort { get; set; }
        public string order { get; set; }
        #endregion

        public GetPagedListOfPregnantInfoRequest()
        { 
        }

        public string PersonName { set; get; }
        public override int PageIndex { get { return page; } }
        public override int PageSize { get { return rows; } }

        public override Dictionary<string, bool> Orders { get { return sort == null ? new Dictionary<string, bool>() : (new Dictionary<string, bool>() { { sort, (order == "asc") } }); } }

        #region IQueriablePagedList

        public List<string> FieldNames { get; set; } = new List<string>() { "*" };

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
select s1.SyncTime,s1.SyncStatus,s1.ErrorMessage
,s2.SyncTime,s2.SyncStatus,s2.ErrorMessage
,TSource.* from
(
    select {string.Join(",", FieldNames)}
    from {PregnantInfo.TableName}
    {GetWhereCondition()}
    {GetOrderCondition()}
    {GetLimitCondition()}
) as TSource
left join SyncForFS s1 on TSource.Id =s1.SourceId and s1.TargetType = 1
left join SyncForFS s2 on TSource.Id =s2.SourceId and s2.TargetType = 2
";
        }

        #endregion
    }
}