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

        internal static List<PregnantDischarge> GetPregnantDischargesToCreate(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<PregnantDischarge>($@"
select top 1 * 
from HELEESB.dbo.V_FWPT_GY_ZHUYUANFM fm
left join HL_Pregnant.dbo.SyncForFS s5 on s5.TargetType = 5 and s5.SourceId = fm.inp_no
where s5.id is null
and fm.inp_no ='0000265533'
", transaction: dbGroup.Transaction).ToList();
        }

        internal static List<PregnantDischarge> GetPregnantDischargesToUpdate(DbGroup dbGroup)
        {
            //VLTODO
            return dbGroup.Connection.Query<PregnantDischarge>($@"
select top 1 * 
from HELEESB.dbo.V_FWPT_GY_ZHUYUANFM fm
left join HL_Pregnant.dbo.SyncForFS s5 on s5.TargetType = 5 and s5.SourceId = fm.inp_no
where s5.id is not null
and fm.inp_no ='0000265533'
", transaction: dbGroup.Transaction).ToList();
        }

        #endregion

        #region ChildDischarge

        internal static List<ChildDischarge> GetChildDischargesToCreate(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<ChildDischarge>($@"
select top 1 * 
from HELEESB.dbo.V_FWPT_GY_ZHUYUANFMYE fm
left join HL_Pregnant.dbo.SyncForFS s6 on s6.TargetType = 6 and s6.SourceId = fm.inp_no
where s6.id is null
and fm.inp_no ='0000265533'
", transaction: dbGroup.Transaction).ToList();
        }

        internal static IEnumerable<ChildDischarge> GetChildDischargesToUpdate(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<ChildDischarge>($@"
select top 1 * 
from HELEESB.dbo.V_FWPT_GY_ZHUYUANFMYE fm
left join HL_Pregnant.dbo.SyncForFS s6 on s6.TargetType = 6 and s6.SourceId = fm.inp_no
where s6.id is not null
and fm.inp_no ='0000265533'
", transaction: dbGroup.Transaction).ToList();
        }

        #endregion

    }
}
