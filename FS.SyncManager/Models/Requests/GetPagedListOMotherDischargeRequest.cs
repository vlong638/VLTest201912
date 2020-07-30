using FrameworkTest.Common.PagerSolution;
using FrameworkTest.Common.ValuesSolution;
using System.Collections.Generic;

namespace FS.SyncManager.Models
{
    public class GetPagedListOfMotherDischargeRequest : VLPageRequest, IQueriablePagedList
    {
        #region OrientInput
        public int page { get; set; }
        public int rows { get; set; }
        public string sort { get; set; }
        public string order { get; set; }
        #endregion

        public GetPagedListOfMotherDischargeRequest()
        {
        }

        public string xingming { set; get; }
        public string inp_no { set; get; }
        public string chuyuanrqfixed { set; get; }

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

            if (!string.IsNullOrEmpty(xingming))
            {
                args.Add(nameof(xingming), $"%{xingming}%");
            }
            if (!string.IsNullOrEmpty(inp_no))
            {
                args.Add(nameof(inp_no), inp_no);
            }
            var chuyuanrqfixedDate = chuyuanrqfixed.ToDateTime();
            if (chuyuanrqfixedDate.HasValue)
            {
                args.Add("chuyuanrqfixedStart", chuyuanrqfixedDate.Value.ToString("yyyy-MM-dd"));
                args.Add("chuyuanrqfixedEnd", chuyuanrqfixedDate.Value.AddDays(1).ToString("yyyy-MM-dd"));
            }

            return args;
        }
        public string GetWhereCondition()
        {
            if (wheres.Count == 0)
            {
                if (!string.IsNullOrEmpty(xingming))
                {
                    wheres.Add($"{nameof(xingming)} Like @xingming");
                }
                if (!string.IsNullOrEmpty(inp_no))
                {
                    wheres.Add($"{nameof(inp_no)} = @inp_no");
                }
                var chuyuanrqfixedDate = chuyuanrqfixed.ToDateTime();
                if (chuyuanrqfixedDate.HasValue)
                {
                    wheres.Add($"chuyuanrqfixed>=@chuyuanrqfixedStart and chuyuanrqfixed<@chuyuanrqfixedEnd");
                }
            }
            return wheres.Count == 0 ? "" : "where " + string.Join(" and ", wheres);
        }
        public string ToCountSQL()
        {
            return $@"
select count(*)
from (
    select fm.inp_no as id
    ,br.shouji,br.xingming,br.chuyuanrqfixed
    ,pi.idcard,pi.createage,pi.restregioncode,pi.restregiontext
    ,fm.inp_no,fm.FMRQDate,fm.FMFSData,fm.ZCJGData ,fm.TWData ,fm.XYData ,fm.RFQKData ,fm.gdgddata ,fm.hyskdata ,fm.ELUData ,fm.CLJZDData 
    from HELEESB.dbo.V_FWPT_GY_ZHUYUANFM fm
    left join HL_Pregnant.dbo.SyncForFS s5 on s5.TargetType = 5 and s5.SourceId = fm.inp_no
    left join HELEESB.dbo.V_FWPT_GY_BINGRENXXZY br on br.bingrenid = fm.inp_no
    left join HL_Pregnant.dbo.PregnantInfo pi on pi.idcard = br.shenfenzh
) as TSource
{GetWhereCondition()}
";
        }
        public string ToListSQL()
        {
            if (Orders.Count == 0)
            {
                Orders.Add("br.chuyuanrqfixed", false);
            }
            return $@"
select 
1
,s5.Id as SyncIdTos5,s5.SyncTime as LastSyncTimeTos5,s5.SyncStatus as SyncStatusTos5,s5.ErrorMessage as SyncMessageTos5
,{string.Join(",", FieldNames)}
from (
    select fm.inp_no as id
    ,br.shouji,br.xingming,br.chuyuanrqfixed
    ,pi.idcard,pi.createage,pi.restregioncode,pi.restregiontext
    ,fm.inp_no,fm.FMRQDate,fm.FMFSData,fm.ZCJGData ,fm.TWData ,fm.XYData ,fm.RFQKData ,fm.gdgddata ,fm.hyskdata ,fm.ELUData ,fm.CLJZDData 
    from HELEESB.dbo.V_FWPT_GY_ZHUYUANFM fm
    left join HL_Pregnant.dbo.SyncForFS s5 on s5.TargetType = 5 and s5.SourceId = fm.inp_no
    left join HELEESB.dbo.V_FWPT_GY_BINGRENXXZY br on br.bingrenid = fm.inp_no
    left join HL_Pregnant.dbo.PregnantInfo pi on pi.idcard = br.shenfenzh
) as TSource
{GetWhereCondition()}
{GetOrderCondition()}
{GetLimitCondition()}
left join SyncForFS s5 on TSource.Id =s5.SourceId and s5.TargetType = 1
";
        }

        #endregion
    }
}