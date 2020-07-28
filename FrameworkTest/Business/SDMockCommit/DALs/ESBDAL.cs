using Dapper;
using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkTest.Business.SDMockCommit
{
    public class ESBDAL
    {
        #region SyncOrder

        public static long InsertSyncForFS(DbGroup dbGroup, SyncOrder syncForFS)
        {
            return dbGroup.Connection.ExecuteScalar<long>(@"
INSERT INTO HL_Pregnant.dbo.SyncForFS([SourceType], [SourceId], [SyncTime], [ErrorMessage], [SyncStatus], [TargetType]) 
VALUES (@SourceType, @SourceId, @SyncTime, @ErrorMessage, @SyncStatus, @TargetType);
", syncForFS, transaction: dbGroup.Transaction);
        }

        public static bool UpdateSyncForFS(DbGroup dbGroup, SyncOrder syncForFS)
        {
            return dbGroup.Connection.ExecuteScalar<bool>(@"
update HL_Pregnant.dbo.SyncForFS
set SourceType = @SourceType,SourceId = @SourceId,SyncTime = @SyncTime,ErrorMessage = @ErrorMessage,SyncStatus = @SyncStatus, TargetType = @TargetType
where Id = @Id
", syncForFS, transaction: dbGroup.Transaction);
        }

        internal static SyncOrder GetSyncForFS(DbGroup dbGroup, TargetType TargetType, string sourceId)
        {
            return dbGroup.Connection.Query<SyncOrder>("select * from HL_Pregnant.dbo.SyncForFS where TargetType = @TargetType and sourceId = @sourceId", new { TargetType, sourceId }, transaction: dbGroup.Transaction).FirstOrDefault();
        }

        #endregion

        #region PregnantDischarge

        internal static int UpdatePregnantDischarge(DbGroup dbGroup)
        {
            return dbGroup.Connection.ExecuteScalar<int>(@"
update V_FWPT_GY_BINGRENXXZY 
set chuyuanrqfixed = convert(datetime,concat(substring(chuyuanrq,1,4),'-',substring(chuyuanrq,5,2),'-',substring(chuyuanrq,7,2),' ',substring(chuyuanrq,9,2),':',substring(chuyuanrq,11,2),':',substring(chuyuanrq,13,2)))
where chuyuanrq is not null and chuyuanrqfixed is null
", transaction: dbGroup.Transaction);
        }

        internal static List<PregnantDischargeModel> GetPregnantDischargesToCreate(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<PregnantDischargeModel>($@"
select top 1 ''
,br.shouji,br.xingming,br.chuyuanrqfixed
,pi.idcard,pi.createage,pi.restregioncode,pi.restregiontext
,fm.inp_no,fm.FMRQDate,fm.FMFSData,fm.ZCJGData ,fm.TWData ,fm.XYData ,fm.RFQKData ,fm.gdgddata ,fm.hyskdata ,fm.ELUData ,fm.CLJZDData 
from HELEESB.dbo.V_FWPT_GY_ZHUYUANFM fm
left join HL_Pregnant.dbo.SyncForFS s5 on s5.TargetType = 5 and s5.SourceId = fm.inp_no
left join HELEESB.dbo.V_FWPT_GY_BINGRENXXZY br on br.bingrenid = fm.inp_no
left join HL_Pregnant.dbo.PregnantInfo pi on pi.idcard = br.shenfenzh
where s5.id is null
and br.chuyuanrqfixed >= convert(nvarchar, getdate(),23) 
-- and fm.inp_no ='0000312639'
", transaction: dbGroup.Transaction).ToList();
        }

        internal static List<PregnantDischargeModel> GetPregnantDischargesToUpdate(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<PregnantDischargeModel>($@"
select top 1 ''
,br.shouji,br.xingming,br.chuyuanrqfixed
,pi.idcard,pi.createage,pi.restregioncode,pi.restregiontext
,fm.inp_no,fm.FMRQDate,fm.FMFSData,fm.ZCJGData ,fm.TWData ,fm.XYData ,fm.RFQKData ,fm.gdgddata ,fm.hyskdata ,fm.ELUData ,fm.CLJZDData 
from HELEESB.dbo.V_FWPT_GY_ZHUYUANFM fm
left join HL_Pregnant.dbo.SyncForFS s5 on s5.TargetType = 5 and s5.SourceId = fm.inp_no
left join HELEESB.dbo.V_FWPT_GY_BINGRENXXZY br on br.bingrenid = fm.inp_no
left join HL_Pregnant.dbo.PregnantInfo pi on pi.idcard = br.shenfenzh
where s5.id is not null and s5.SyncStatus = 2
and br.chuyuanrqfixed >= convert(nvarchar, getdate(),23) 
and fm.downloadtime > s5.SyncTime
-- and fm.inp_no ='0000312639'
", transaction: dbGroup.Transaction).ToList();
        }

        #endregion

        #region ChildDischarge

        internal static List<ChildDischargeModel> GetChildDischargesToCreate(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<ChildDischargeModel>($@"
select top 1 * 
from HELEESB.dbo.V_FWPT_GY_ZHUYUANFMYE fm
left join HL_Pregnant.dbo.SyncForFS s6 on s6.TargetType = 6 and s6.SourceId = fm.inp_no
left join HELEESB.dbo.V_FWPT_GY_BINGRENXXZY br on br.bingrenid = fm.inp_no
where s6.id is null
and fm.inp_no ='0000265533'
", transaction: dbGroup.Transaction).ToList();
        }

        internal static IEnumerable<ChildDischargeModel> GetChildDischargesToUpdate(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<ChildDischargeModel>($@"
select top 1 * 
from HELEESB.dbo.V_FWPT_GY_ZHUYUANFMYE fm
left join HL_Pregnant.dbo.SyncForFS s6 on s6.TargetType = 6 and s6.SourceId = fm.inp_no
where s6.id is not null
and fm.inp_no ='0000265533'
", transaction: dbGroup.Transaction).ToList();
        }

        #endregion

        #region Diagnosis

        internal static IEnumerable<Diagnosis> GetDiagnosisByPatientIdAndVisitId(DbGroup dbGroup, string patientId, string visitId)
        {
            return dbGroup.Connection.Query<Diagnosis>($@"
select * from V_FWPT_GY_ZHUYUANZD where patient_Id = @patientId and inp_no = @visitId
", new { patientId, visitId }, transaction: dbGroup.Transaction).ToList();
        }

        #endregion

        #region Inspection
        internal static List<Inspection> GetInspectionsByPatientId(DbGroup dbGroup, string patientId)
        {
            return dbGroup.Connection.Query<Inspection>($@"
select jy.chinesename,jy.testresult,jy.measuretime
from V_FWPT_JY_JIANYANSQ sq
left join v_fwpt_jy_jianyanjg jy on sq.laborder = jy.doctadviseno
where sq.bingrenid = @patientId
", new { patientId }, transaction: dbGroup.Transaction).ToList();
        }

        #endregion

        #region Advice
        internal static List<Advice> GetAdvicesByPatientId(DbGroup dbGroup, string patientId)
        {
            return dbGroup.Connection.Query<Advice>($@"
select yizhumc from V_FWPT_MZ_YIJI where bingrenid = @patientId
", new { patientId }, transaction: dbGroup.Transaction).ToList();
        } 
        #endregion
    }
}
