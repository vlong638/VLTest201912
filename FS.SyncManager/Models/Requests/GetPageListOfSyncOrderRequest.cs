using FrameworkTest.Common.PagerSolution;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FS.SyncManager.Models
{
    public class GetPagedListOfSyncOrderRequest : VLPageRequest, IQueriablePagedList
    {
        #region OrientInput
        public int page { get; set; }
        public int rows { get; set; }
        public string sort { get; set; }
        public string order { get; set; }
        #endregion

        public GetPagedListOfSyncOrderRequest()
        { 
        }

        public string PersonName { set; get; }
        public override int PageIndex { get { return page; } }
        public override int PageSize { get { return rows; } }
        public List<string> FieldNames { get; set; } = new List<string>() { "*" };

        public override Dictionary<string, bool> Orders
        {
            get
            {
                if (_Orders == _Orders || _Orders.Count == 0)
                {
                    _Orders = sort == null ? new Dictionary<string, bool>() : (new Dictionary<string, bool>() { { sort, (order == "asc") } });
                    if (_Orders.Count == 0)
                    {
                        _Orders.Add("Id", false);
                    }
                }
                return _Orders;
            }
        }

        #region IQueriablePagedList

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
                    wheres.Add($"pi.{nameof(PersonName)} Like @PersonName");
                }
            }
            return wheres.Count == 0 ? "" : "and " + string.Join(" and", wheres);
        }
        public string ToCountSQL()
        {
            return $@"
select count(*)
from 
(
    select 1 as f1 from SyncForFS sall
    left join PregnantInfo pi on sall.SourceType = 1 and sall.SourceId = pi.id
    where sall.SourceType = 1 {GetWhereCondition()}
    Union
    select 1 as f1 from SyncForFS sall
    left join MHC_VisitRecord vr on sall.SourceType = 2 and sall.SourceId = vr.id
    left join PregnantInfo pi on vr.Idcard= pi.Idcard
    where sall.SourceType = 2 {GetWhereCondition()}
) as T
";
        }

        public string ToListSQL()
        {
            return $@"
    select *
    from  
    (
        select pi.personname,pi.idcard,{string.Join(",", FieldNames.Select(c => "sall." + c))} 
        from SyncForFS sall
        left join PregnantInfo pi on sall.SourceType = 1 and sall.SourceId = pi.id
        where sall.SourceType = 1 {GetWhereCondition()}
        Union
        select pi.personname,pi.idcard,{string.Join(",", FieldNames.Select(c => "sall." + c))} 
        from SyncForFS sall
        left join MHC_VisitRecord vr on sall.SourceType = 2 and sall.SourceId = vr.id
        left join PregnantInfo pi on vr.Idcard= pi.Idcard
        where sall.SourceType = 2 {GetWhereCondition()}
    ) as T
    {GetOrderCondition()}
    {GetLimitCondition()}
";
        } 

        #endregion
    }
}