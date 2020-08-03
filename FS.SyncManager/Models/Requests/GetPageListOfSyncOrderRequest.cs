using FrameworkTest.Business.SDMockCommit;
using FrameworkTest.Common.PagerSolution;
using FrameworkTest.Common.ValuesSolution;
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

        public string PersonName { set; get; }
        public string SyncStatus { set; get; }
        public TargetType TargetType { set; get; }

        public Dictionary<string, object> GetParams()
        {
            if (args.Count > 0)
                return args;

            if (!string.IsNullOrEmpty(PersonName))
            {
                args.Add(nameof(PersonName), $"%{PersonName}%");
            }
            if (!string.IsNullOrEmpty(SyncStatus))
            {
                args.Add(nameof(SyncStatus), this.SyncStatus.ToEnum<SyncStatus>());
            }
            if (TargetType != TargetType.None)
            {
                args.Add(nameof(TargetType), TargetType);
            }
            return args;
        }
        public string GetWhereCondition(SourceType sourceType)
        {
            wheres.Clear();
            switch (sourceType)
            {
                case SourceType.None:
                    break;
                case SourceType.PregnantInfo:
                case SourceType.MHC_VisitRecord:
                    if (!string.IsNullOrEmpty(PersonName))
                    {
                        wheres.Add($"pi.PersonName Like @PersonName");
                    }
                    break;
                case SourceType.V_FWPT_GY_ZHUYUANFM:
                    if (!string.IsNullOrEmpty(PersonName))
                    {
                        wheres.Add($"br.xingming Like @PersonName");
                    }
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(SyncStatus))
            {
                if (SyncStatus == "99")
                {
                    wheres.Add($"sall.{nameof(SyncStatus)} not in (2,11)");
                }
                else
                {
                    wheres.Add($"sall.{nameof(SyncStatus)} = @SyncStatus");
                }
            }
            if (TargetType != TargetType.None)
            {
                wheres.Add($"sall.{nameof(TargetType)} = @TargetType");
            }
            return wheres.Count == 0 ? "" : " and " + string.Join(" and ", wheres);
        }
        public string ToCountSQL()
        {
            return $@"
select count(*)
from 
(
    select sall.id from SyncForFS sall
    left join PregnantInfo pi on sall.SourceType = 1 and sall.SourceId = pi.id
    where sall.SourceType = 1 {GetWhereCondition(SourceType.PregnantInfo)}
    Union
    select sall.id from SyncForFS sall
    left join MHC_VisitRecord vr on sall.SourceType = 2 and sall.SourceId = vr.id
    left join PregnantInfo pi on vr.Idcard= pi.Idcard
    where sall.SourceType = 2 {GetWhereCondition(SourceType.MHC_VisitRecord)}
    Union
    select sall.id from SyncForFS sall
    left join HELEESB.dbo.V_FWPT_GY_ZHUYUANFM fm on fm.inp_no = sall.SourceId
    left join HELEESB.dbo.V_FWPT_GY_BINGRENXXZY br on br.bingrenid = fm.inp_no
    where sall.SourceType = 3 {GetWhereCondition(SourceType.V_FWPT_GY_ZHUYUANFM)}
) as T
";
        }

        public string ToListSQL()
        {
            return $@"
    select *
    from  
    (
        select pi.PersonName,pi.Idcard,{string.Join(",", FieldNames.Select(c => "sall." + c))} 
        from SyncForFS sall
        left join PregnantInfo pi on sall.SourceType = 1 and sall.SourceId = pi.id
        where sall.SourceType = 1 {GetWhereCondition(SourceType.PregnantInfo)}
        Union
        select pi.PersonName,pi.Idcard,{string.Join(",", FieldNames.Select(c => "sall." + c))} 
        from SyncForFS sall
        left join MHC_VisitRecord vr on sall.SourceType = 2 and sall.SourceId = vr.id
        left join PregnantInfo pi on vr.Idcard= pi.Idcard
        where sall.SourceType = 2 {GetWhereCondition(SourceType.MHC_VisitRecord)}
        Union
        select br.xingming as PersonName,br.shenfenzh as Idcard,{string.Join(",", FieldNames.Select(c => "sall." + c))} 
        from SyncForFS sall
        left join HELEESB.dbo.V_FWPT_GY_ZHUYUANFM fm on fm.inp_no = sall.SourceId
        left join HELEESB.dbo.V_FWPT_GY_BINGRENXXZY br on br.bingrenid = fm.inp_no
        where sall.SourceType = 3 {GetWhereCondition(SourceType.V_FWPT_GY_ZHUYUANFM)}
    ) as T
    {GetOrderCondition()}
    {GetLimitCondition()}
";
        } 

        #endregion
    }
}