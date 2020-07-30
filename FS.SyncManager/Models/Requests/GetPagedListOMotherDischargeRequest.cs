using FrameworkTest.Common.PagerSolution;
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
            return wheres.Count == 0 ? "" : "where " + string.Join(" and ", wheres);
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
                Orders.Add("br.chuyuanrqfixed", false);
            }
            return $@"
select {string.Join(",", FieldNames)}
from (
    select top 1 ''
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
";
        }

        #endregion
    }
}