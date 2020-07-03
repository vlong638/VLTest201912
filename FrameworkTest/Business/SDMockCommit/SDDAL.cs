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
            return dbGroup.Connection.Query<ProfessionalExaminationModel>($@"
SELECT top 1
vr.id
,pi.idcard
,pi.personname
,pi.lastmenstrualperiod
,pi.dateofprenatal 
,vr.uterus
,vr.palacemouth
,vr.suggestion
,vr.generalcomment
,vr.followupappointment
,vr.brokenwater
,vr.multifetal
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
            return dbGroup.Connection.Query<ProfessionalExaminationModel>($@"
SELECT top 1
vr.id
,pi.idcard,pi.personname,pi.lastmenstrualperiod,pi.dateofprenatal 
,vr.uterus,vr.palacemouth,vr.suggestion,vr.generalcomment,vr.followupappointment,vr.brokenwater,vr.multifetal
FROM PregnantInfo pi 
LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
left join SyncForFS se on se.SourceType = 4 and se.SourceId = vr.Id			
where vr.visitdate = convert(nvarchar,getdate(),23)
and se.id is not null and se.SyncStatus in (2,11) 
and vr.updatetime > DATEADD( SECOND,10 ,se.SyncTime)
", transaction: dbGroup.Transaction).ToList();
        }

    }
}
