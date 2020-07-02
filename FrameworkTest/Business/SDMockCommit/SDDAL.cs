using Dapper;
using Dapper.Contrib.Extensions;
using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.FileSolution;
using FrameworkTest.Common.HttpSolution;
using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FrameworkTest.Business.SDMockCommit
{
    public class SDDAL
    {
        public static long InsertSyncForFS(DbGroup group, SyncOrder syncForFS)
        {
            return group.Connection.Insert(syncForFS, transaction: group.Transaction);
        }

        public static bool UpdateSyncForFS(DbGroup group, SyncOrder syncForFS)
        {
            return group.Connection.Update(syncForFS, transaction: group.Transaction);
        }

        internal static SyncOrder GetSyncForFS(DbGroup group, SourceType sourceType, string sourceId)
        {
            return group.Connection.Query<SyncOrder>("select * from SyncForFS where sourceType = @sourceType and sourceId = @sourceId", new { sourceType, sourceId }, transaction: group.Transaction).FirstOrDefault();
        }

        internal static List<PhysicalExaminationModel> GetPhysicalExaminationsToCreate(DbGroup group)
        {
            throw new NotImplementedException();
//            return group.Connection.Query<PhysicalExaminationModel>($@"
//SELECT top 10
//pi.idcard
//,vr.visitdate 
//,sp.id
//,se.id
//FROM PregnantInfo pi 
//LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
//left join SyncForFS sp on sp.SourceType = 3 and sp.SourceId = vr.Id
//left join SyncForFS se on se.SourceType = 4 and se.SourceId = vr.Id			
//where sp.id is not null and sp.SyncStatus in (2,11) 
//and se.id is null 
//and vr.visitdate = convert(nvarchar,getdate(),23)
//", transaction: group.Transaction).ToList();
        }

        internal static List<ProfessionalExaminationModel> GetProfessionalExaminationsToCreate(DbGroup dbGroup)
        {
            //142328199610271518	李丽	2019-12-18	2020-10-08	0	12	NULL	NULL	2020-07-30	2	[{"index":"0","heartrate":"66","position":"01","presentposition":"1","fetalmove":"1"},{"index":"2","heartrate":"88","position":"02","presentposition":"2","fetalmove":"2"}]	2020-07-02	1393	NULL
            return dbGroup.Connection.Query<ProfessionalExaminationModel>($@"
SELECT top 1
vr.id
,pi.idcard
,pi.personname
,pi.lastmenstrualperiod --末次月经
,pi.dateofprenatal  --预产期
,vr.uterus--子宫 1=异常 0=正常
,vr.palacemouth --宫口 详见枚举
,vr.suggestion --处理意见
,vr.generalcomment --其他评估
,vr.followupappointment --下次随访
--预约目的
,vr.brokenwater --破水
,vr.multifetal --多胎
--胎数
--胎方位
--胎先露
-- ,vr.visitdate 
-- ,sp.id
-- ,se.id
FROM PregnantInfo pi 
LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
left join SyncForFS sp on sp.SourceType = 3 and sp.SourceId = vr.Id
left join SyncForFS se on se.SourceType = 4 and se.SourceId = vr.Id			
where sp.id is not null and sp.SyncStatus in (2,11) 
and se.id is null 
and vr.visitdate = convert(nvarchar,getdate(),23)
", transaction: dbGroup.Transaction).ToList();
        }

        internal static List<ProfessionalExaminationModel> GetProfessionalExaminationsToUpdate(DbGroup dbGroup)
        {
            throw new NotImplementedException("1854");
            return dbGroup.Connection.Query<ProfessionalExaminationModel>($@"
SELECT top 1
vr.id
,pi.idcard
,pi.personname
,pi.lastmenstrualperiod --末次月经
,pi.dateofprenatal  --预产期
,vr.uterus--子宫 1=异常 0=正常
,vr.palacemouth --宫口 详见枚举
,vr.suggestion --处理意见
,vr.generalcomment --其他评估
,vr.followupappointment --下次随访
--预约目的
,vr.brokenwater --破水
,vr.multifetal --多胎
--胎数
--胎方位
--胎先露
-- ,vr.visitdate 
-- ,sp.id
-- ,se.id
FROM PregnantInfo pi 
LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
left join SyncForFS sp on sp.SourceType = 3 and sp.SourceId = vr.Id
left join SyncForFS se on se.SourceType = 4 and se.SourceId = vr.Id			
where sp.id is not null and sp.SyncStatus in (2,11) 
and se.id is null 
and vr.visitdate = convert(nvarchar,getdate(),23)
", transaction: dbGroup.Transaction).ToList();
        }

    }
}
