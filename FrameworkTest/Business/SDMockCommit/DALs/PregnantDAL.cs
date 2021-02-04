using Dapper;
using Dapper.Contrib.Extensions;
using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.ValuesSolution;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkTest.Business.SDMockCommit
{
    public class PregnantDAL
    {
        public static string limiter = "DATEADD(day,-1 ,getdate())";
        //public const string limiter = "'2021-01-01'";

        #region SyncOrder

        public static long InsertSyncForFS(DbGroup group, SyncOrder syncForFS)
        {
            return group.Connection.Insert(syncForFS, transaction: group.Transaction);
        }

        public static bool UpdateSyncForFS(DbGroup group, SyncOrder syncForFS)
        {
            return group.Connection.Update(syncForFS, transaction: group.Transaction);
        }

        internal static SyncOrder GetSyncForFS(DbGroup group, TargetType TargetType, string sourceId)
        {
            return group.Connection.Query<SyncOrder>("select * from SyncForFS where TargetType = @TargetType and sourceId = @sourceId", new { TargetType, sourceId }, transaction: group.Transaction).FirstOrDefault();
        }
        #endregion

        #region PhysicalExamination

        internal static List<PhysicalExaminationModel> GetPhysicalExaminationsToCreate(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<PhysicalExaminationModel>($@"
select 
T1.*
,pi_data.personname pi_personname
,pi_data.weight pi_weight
,pi_data.height pi_height
,pi_data.bmi pi_bmi
,vr_data.Id
,vr_data.weight
,vr_data.temperature
,vr_data.heartrate
,vr_data.dbp
,vr_data.sbp
,vr_data.vulva
,vr_data.vagina
,vr_data.cervix
,vr_data.uterus
,vr_data.appendages
from 
(
		SELECT 
		vr.id,vr.idcard
		,max(vr.visitdate) lastestvisitdate
		,min(vr.visitdate) firstvisitdate		
		FROM (
				SELECT top 1
				pi.idcard,vr.id sourceId
				FROM PregnantInfo pi 
				LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
				left join SyncForFS sp on sp.TargetType = 1 and sp.SourceId = pi.Id
				left join SyncForFS se on se.TargetType = 3 and se.SourceId = vr.Id			
				where sp.id is not null and sp.SyncStatus in (2,11) 
				and se.id is null 
				and vr.visitdate = convert(nvarchar,getdate(),23)
		)  as toCreate 
		LEFT JOIN MHC_VisitRecord vr on toCreate.idcard = vr.idcard  and vr.id = toCreate.sourceId
		GROUP BY vr.id,vr.idcard
) as T1
left join PregnantInfo pi_data on pi_data.idcard = T1.idcard
left join MHC_VisitRecord vr_data on vr_data.idcard = T1.idcard and vr_data.visitdate = T1.lastestvisitdate
and vr_data.id = T1.Id
", transaction: dbGroup.Transaction).ToList();
        }

        internal static List<PhysicalExaminationModel> GetPhysicalExaminationsToUpdate(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<PhysicalExaminationModel>($@"
select 
T1.*
,pi_data.personname pi_personname
,pi_data.weight pi_weight
,pi_data.height pi_height
,pi_data.bmi pi_bmi
,vr_data.Id
,vr_data.weight
,vr_data.temperature
,vr_data.heartrate
,vr_data.dbp
,vr_data.sbp
,vr_data.vulva
,vr_data.vagina
,vr_data.cervix
,vr_data.uterus
,vr_data.appendages
from 
(
		SELECT 
		vr.id,vr.idcard
		,max(vr.visitdate) lastestvisitdate
		,min(vr.visitdate) firstvisitdate		
		FROM (
				SELECT top 1
				pi.idcard,vr.id sourceId
				FROM PregnantInfo pi 
				LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
				left join SyncForFS se on se.TargetType = 3 and se.SourceId = vr.Id			
				where se.id is not null and ((se.SyncStatus = 2
				and vr.updatetime > DATEADD( SECOND,10 ,se.SyncTime)
				and vr.visitdate = convert(nvarchar,getdate(),23)				
                ) or s6.SyncStatus = 3)
		)  as toCreate 
		LEFT JOIN MHC_VisitRecord vr on toCreate.idcard = vr.idcard  and vr.id = toCreate.sourceId
		GROUP BY vr.id,vr.idcard
) as T1
left join PregnantInfo pi_data on pi_data.idcard = T1.idcard
left join MHC_VisitRecord vr_data on vr_data.idcard = T1.idcard and vr_data.visitdate = T1.lastestvisitdate
and vr_data.id = T1.Id
", transaction: dbGroup.Transaction).ToList();
        }

        #endregion

        #region ProfessionalExamination
        internal static List<ProfessionalExaminationModel> GetProfessionalExaminationsToCreateByIdCard(DbGroup dbGroup, string idcard)
        {
            return dbGroup.Connection.Query<ProfessionalExaminationModel>($@"
SELECT top 1
vr.id,vr.uterinecontraction,vr.amnioticfluidcharacter
,vr.weight,pi.bmi,pi.idcard,pi.personname,pi.lastmenstrualperiod,pi.dateofprenatal 
,vr.dbp,vr.sbp,vr.uterus,vr.palacemouth,vr.suggestion,vr.generalcomment,vr.followupappointment,vr.brokenwater,vr.multifetal
,vr.chiefcomplaint,vr.presenthistory,vr.heightfundusuterus,vr.abdomencircumference,vr.xianjie,vr.edemastatus
,vr.diagnosisinfo,vr.maindiagnosis,vr.secondarydiagnosis,vr.highriskdic
FROM PregnantInfo pi 
LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
left join SyncForFS s3 on s3.TargetType = 3 and s3.SourceId = vr.Id
left join SyncForFS se on se.TargetType = 4 and se.SourceId = vr.Id			
where s3.id is not null and s3.SyncStatus in (2,11) 
and se.id is null 
and vr.visitdate = convert(nvarchar,getdate(),23)	
and vr.idcard = @idcard
", new { idcard }, transaction: dbGroup.Transaction).ToList();
        }

        internal static List<ProfessionalExaminationModel> GetProfessionalExaminationsToCreate(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<ProfessionalExaminationModel>($@"
SELECT top 1
vr.id,vr.uterinecontraction,vr.amnioticfluidcharacter
,vr.weight,pi.bmi,pi.idcard,pi.personname,pi.lastmenstrualperiod,pi.dateofprenatal,pi.createage,pi.birthday
,vr.dbp,vr.sbp,vr.uterus,vr.palacemouth,vr.suggestion,vr.generalcomment,vr.followupappointment,vr.brokenwater,vr.multifetal
,vr.chiefcomplaint,vr.presenthistory,vr.heightfundusuterus,vr.abdomencircumference,vr.xianjie,vr.edemastatus
,vr.diagnosisinfo,vr.maindiagnosis,vr.secondarydiagnosis,vr.highriskdic
FROM PregnantInfo pi 
LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
left join SyncForFS s3 on s3.TargetType = 3 and s3.SourceId = vr.Id
left join SyncForFS s4 on s4.TargetType = 4 and s4.SourceId = vr.Id			
where s3.id is not null and s3.SyncStatus in (2,11) 
and s4.id is null 
and vr.visitdate = convert(nvarchar,getdate(),23)
", transaction: dbGroup.Transaction).ToList();
        }

        internal static List<ProfessionalExaminationModel> GetProfessionalExaminationsToUpdateByIdCard(DbGroup dbGroup, string idcard)
        {
            return dbGroup.Connection.Query<ProfessionalExaminationModel>($@"
SELECT top 1
vr.id,vr.uterinecontraction,vr.amnioticfluidcharacter
,vr.weight,pi.bmi,pi.idcard,pi.personname,pi.lastmenstrualperiod,pi.dateofprenatal 
,vr.dbp,vr.sbp,vr.uterus,vr.palacemouth,vr.suggestion,vr.generalcomment,vr.followupappointment,vr.brokenwater,vr.multifetal
,vr.chiefcomplaint,vr.presenthistory,vr.heightfundusuterus,vr.abdomencircumference,vr.xianjie,vr.edemastatus
,vr.diagnosisinfo,vr.maindiagnosis,vr.secondarydiagnosis,vr.highriskdic
FROM PregnantInfo pi 
LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
left join SyncForFS s4 on s4.TargetType = 4 and s4.SourceId = vr.Id			
where vr.visitdate = convert(nvarchar,getdate(),23)
and s4.id is not null and s4.SyncStatus in (2,11) 
and vr.updatetime > DATEADD( SECOND,10 ,s4.SyncTime)
and vr.idcard = @idcard
", new { idcard }, transaction: dbGroup.Transaction).ToList();
        }

        internal static List<ProfessionalExaminationModel> GetProfessionalExaminationsToUpdate(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<ProfessionalExaminationModel>($@"
SELECT top 1
vr.id,vr.uterinecontraction,vr.amnioticfluidcharacter
,vr.weight,pi.bmi,pi.idcard,pi.personname,pi.lastmenstrualperiod,pi.dateofprenatal,pi.createage,pi.birthday
,vr.dbp,vr.sbp,vr.uterus,vr.palacemouth,vr.suggestion,vr.generalcomment,vr.followupappointment,vr.brokenwater,vr.multifetal
,vr.chiefcomplaint,vr.presenthistory,vr.heightfundusuterus,vr.abdomencircumference,vr.xianjie,vr.edemastatus
,vr.diagnosisinfo,vr.maindiagnosis,vr.secondarydiagnosis,vr.highriskdic
FROM PregnantInfo pi 
LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
left join SyncForFS s4 on s4.TargetType = 4 and s4.SourceId = vr.Id			
where vr.visitdate = convert(nvarchar,getdate(),23)
and s4.id is not null and ((s4.SyncStatus = 2
and vr.updatetime > DATEADD( SECOND,10 ,s4.SyncTime)
) or s6.SyncStatus = 3)
", transaction: dbGroup.Transaction).ToList();
        }
        //and vr.idcard = '142328199610271518'
        #endregion

        #region PregnantInfo

        internal static List<PregnantInfo> GetPregnantInfoForCreateOrUpdate(DbGroup dbGroup)
        {
            //指定日期之后 非今天 当年会与常规新建和更新重复
            return dbGroup.Connection.Query<PregnantInfo>($@"
select
Top 1
s.id sid,
pi.createtime, pi.updatetime,
pi.*
from PregnantInfo pi
left join SyncForFS s on s.TargetType = 1 and s.SourceId = pi.Id
where s.id is null 
and pi.createtime < convert(nvarchar, getdate(),23) 
and pi.updatetime> {limiter}
", transaction: dbGroup.Transaction).ToList();

            //当天
            //            return dbGroup.Connection.Query<PregnantInfo>($@"
            //select Top 1 
            //s.id sid,
            //pi.createtime, pi.updatetime,
            //pi.*
            //from PregnantInfo pi
            //left join SyncForFS s on s.TargetType = 1 and s.SourceId = pi.Id
            //where s.id is null 
            //and pi.createtime < convert(nvarchar, getdate(),23) 
            //and pi.updatetime> convert(nvarchar, getdate(),23)
            //", transaction: dbGroup.Transaction).ToList();
        }

        internal static IEnumerable<PregnantInfo> GetPregnantInfoForCreate(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<PregnantInfo>($@"
select Top 1
s.id sid,
pi.createtime, pi.updatetime,
pi.*
from PregnantInfo pi
left join SyncForFS s on s.TargetType = 1 and s.SourceId = pi.Id
where s.id is null 
and pi.createtime>= convert(nvarchar, getdate(),23) 
union
(
    select
    Top 1
    s.id sid,
    pi.createtime, pi.updatetime,
    pi.*
    from PregnantInfo pi
    left join SyncForFS s on s.TargetType = 1 and s.SourceId = pi.Id
    where s.id is null 
    and pi.createtime < convert(nvarchar, getdate(),23) 
    and pi.updatetime >= convert(nvarchar, getdate(),23)
)
union
(
	select 
	Top 1
	s4.id sid,
	pi.createtime, pi.updatetime,
	pi.*
	FROM PregnantInfo pi 
	LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
	left join SyncForFS s4 on s4.TargetType = 4 and s4.SourceId = vr.Id			
	left join SyncForFS s1 on s1.TargetType = 1 and s1.SourceId = pi.Id			
	where vr.visitdate >= {limiter} and s4.id is null and s1.id is null
	and pi.createtime < convert(nvarchar, getdate(),23) 
	and pi.updatetime < convert(nvarchar, getdate(),23)
)
", transaction: dbGroup.Transaction).ToList();
        }

        internal static IEnumerable<PregnantInfo> GetPregnantInfoForUpdate(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<PregnantInfo>($@"
select Top 1 s.id sid
,s.SyncTime
,pi.createtime,pi.updatetime
,pi.* from PregnantInfo pi
left join SyncForFS s on s.TargetType = 1 and s.SourceId = pi.Id
where s.id is not null and ((s.SyncStatus = 2
and pi.updatetime > DATEADD( SECOND,10 ,s.SyncTime)
) or s6.SyncStatus = 3)
", transaction: dbGroup.Transaction).ToList();

            //in (2, 11)
        }

        #endregion

        #region Enquiry

        internal static IEnumerable<PregnantInfo> GetPregnantInfosToCreateEnquiries(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<PregnantInfo>($@"
select Top 1 sp.id spid,se.id seid,pi.* 
from PregnantInfo pi
left join SyncForFS sp on sp.TargetType = 1 and sp.SourceId = pi.Id
left join SyncForFS se on se.TargetType = 2 and se.SourceId = pi.Id
where sp.id is not null 
and sp.SyncStatus = 2
and se.id is null
", transaction: dbGroup.Transaction).ToList();
        }

        internal static IEnumerable<PregnantInfo> GetPregnantInfosToUpdateEnquiries(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<PregnantInfo>($@"
select Top 1 se.id seid,se.SyncTime,pi.* 
from PregnantInfo pi
left join SyncForFS se on se.TargetType = 2 and se.SourceId = pi.Id
where se.id is not null 
and ((se.SyncStatus = 2 
and pi.updatetime > DATEADD( SECOND,10 ,se.SyncTime)) or s6.SyncStatus = 3)
", transaction: dbGroup.Transaction).ToList();
        }

        #endregion

        #region HighRisks

        internal static string GetLatestHighRisksByIdCard(DbGroup dbGroup, string idcard)
        {
            return dbGroup.Connection.ExecuteScalar<string>($@"
select top 1 highriskdic,highriskreason from HL_Pregnant.dbo.MHC_VISITRECORD where idcard = @idcard order by id desc
", new { idcard }, transaction: dbGroup.Transaction);
        } 

        #endregion
    }
}
