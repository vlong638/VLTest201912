using Dapper;
using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.ValuesSolution;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkTest.Business.SDMockCommit
{
    public class ESBDAL
    {
        #region SyncOrder

        public static long InsertSyncForFS(DbGroup group, SyncOrder syncForFS)
        {
            return group.Connection.ExecuteScalar<long>(@"
INSERT INTO HL_Pregnant.dbo.SyncForFS([SourceType], [SourceId], [SyncTime], [ErrorMessage], [SyncStatus], [TargetType]) 
VALUES (@SourceType, @SourceId, @SyncTime, @ErrorMessage, @SyncStatus, @TargetType);
", syncForFS, transaction: group.Transaction);
        }

        public static bool UpdateSyncForFS(DbGroup group, SyncOrder syncForFS)
        {
            return group.Connection.ExecuteScalar<bool>(@"
update HL_Pregnant.dbo.SyncForFS
set SourceType = @SourceType,SourceId = @SourceId,SyncTime = @SyncTime,ErrorMessage = @ErrorMessage,SyncStatus = @SyncStatus, TargetType = @TargetType
where Id = @Id
", syncForFS, transaction: group.Transaction);
        }

        internal static SyncOrder GetSyncForFS(DbGroup group, TargetType TargetType, string sourceId)
        {
            return group.Connection.Query<SyncOrder>("select * from HL_Pregnant.dbo.SyncForFS where TargetType = @TargetType and sourceId = @sourceId", new { TargetType, sourceId }, transaction: group.Transaction).FirstOrDefault();
        }
        #endregion

        #region PregnantDischarge

        internal static List<PregnantDischarge> GetPregnantDischargesToCreate(DbGroup dbGroup)
        {
            //VLTODO
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
where s5.id is not null and s5.SyncStatus = 2
", transaction: dbGroup.Transaction).ToList();
        }

        #endregion
    }
}
