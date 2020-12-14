
//select t.idcard
//	, pi.pregnantid as 社区孕妇档案编号
//	, pi.homeaddress_text 居住地址
//	, pi.lastmenstrualperiod 末次月经
//	, pi.createdate 建档日期
//	, pi.dateofprenatal 预产期
//	, convert(nvarchar, t.分娩日期,23) 分娩日期
//	,pi.personname as 姓名
//	,pi.idcard as 身份证号
//	,'杭州市妇产科医院' as 分娩单位
//	,'???' 是否纳入保健管理
//	,pi.createage as 年龄
//	,pi.bmi as [体重指数（BMI）]
//	,pi.isagrregister as 居住地
//	,pi.educationcode as 孕妇文化程度
//	,pi.gravidity as [孕次（包括本次妊娠）]
//	,pi.parity as [产次（不包括本次）]
//	,pi.caesareansectionnum as 是否有剖宫产史
//	,fvr.highrisklevel 建册时的高危评估等级
//	,'???' 孕前疾病
//	, t.胎数
//	,'???' 孕妇学校
//	,'???' 孕期补充铁剂
//	, t.分娩孕周
//	, t.新生儿出生体重	
//	, t.所生新生儿出生评分1	
//	, t.所生新生儿出生评分5
//	, t.所生新生儿出生评分10
//	, t.所生新生儿出生缺陷
//	, t.分娩方式	
//	, t.妊娠结局	
//	, t.胎膜早破
//	, t.[产后出血（ml）]
//	,t.有输血
//	,t.妊高症
//	,case when exists(select 1 from MHC_HighRiskReason where PregnantInfoId = pi.id and HighRiskReasonId in ('a20602','b20602')) then '有' else '无' end as 妊娠期糖尿病
//	,t.胎盘早剥
//	,t.前置胎盘
//	,'???'[ICP（肝内胆汁淤积症）]
//	,t.是否进行抢救
//	,tyzq.value as [孕早期血红蛋白值（g / L)]
//	,'???' as [铁蛋白值]
//	,t12xh.value as [12~16周血红蛋白]
//	,t12t.value as [12~16周铁蛋白值]
//	,t16xh.value as [16~28周血红蛋白]
//	,t16t.value as [16~28周铁蛋白值]
//	,t28xh.value as [28~37周血红蛋白]
//	,t28t.value as [28~37周铁蛋白值]
//	,t37xh.value as [37周以后血红蛋白]
//	,t37t.value as [37周以后铁蛋白值]
//	,t42xh.value as [产褥期血红蛋白]
//	,p42.PuerperalInfection as [是否有产褥感染]
//	,'???' as [是否有地中海贫血]
//	,'???' as [孕产妇死亡]
//	,case when dateadd(day, 7, t.分娩日期) > t.[7天内新生儿死亡]  then '是' else '否' end as [7天内新生儿死亡]
//	,case when dateadd(day, 30, t.分娩日期) > t.[7天内新生儿死亡] then '是' else '否' end as [1月内新生儿死亡]
//	,case when gi.id is not null then '是' else '否' end as [是否有关联的儿童档案]
//	, pe1.weight as [儿童体重1个月(kg)]
//	, pe1.height as [儿童身高1(cm)]
//	, pe1.feedingBehavior as [喂养方式1]
//	, pe3.weight as [体重(kg)]
//	, pe3.height as [身高3(cm)]
//	, pe3.feedingBehavior as [喂养方式3]
//	, pe6.weight as [体重6(kg)]
//	, pe6.height as [身高6(cm)]
//	, pe6.feedingBehavior as [喂养方式6]
//	,'???' as [儿童血红蛋白(g / l)]
//	,'???' as [儿童血红蛋白时间]
//	,'???' as [f81]
//	, tOGTT0.value as [空腹血糖]
//	, tOGTT1.value as [1小时血糖]
//	, tOGTT2.value as [2小时血糖]
//	, t0308.value as [D二聚体-分娩前]
//	, t0308max.maxvalue as [D二聚体-最高值]
//	, vr.dbp as [分娩前舒张压]
//	, vr.sbp as [分娩前收缩压]
//	, vr.weight as [分娩前体重]
//	, pi.height as [身高]
//	, vr12.checkweek as [孕周1]
//	, vr12.weight as [体重1]
//	, vr16.checkweek as [孕周2]
//	, vr16.weight as [体重2]
//	, vr24.checkweek as [孕周3]
//	, vr24.weight as [体重3]
//	, vr32.checkweek as [孕周4]
//	, vr32.weight as [体重4]
//	, vr36.checkweek as [孕周5]
//	, vr36.weight as [体重5]
//	, vr40.checkweek as [孕周6]
//	, vr40.weight as [体重6]
//from temp1210_distinct t
//left join PregnantInfo pi on pi.idcard = t.idcard and pi.lastmenstrualperiod = t.maxpilastmodifydate
//left join MHC_FIRSTVISITRECORD fvr on fvr.pregnantinfoid = pi.id and fvr.lastmodifydate = t.maxfvrlastmodifydate
//left join MHC_Postnatal42dayRecord p42 on p42.pregnantinfoid = pi.id and p42.lastmodifydate  = t.maxp42lastmodifydate
//left join CC_GeneralInfo gi on gi.motherIdcard = t.idcard and gi.id = t.giid

//	left join CC_PhysicalExam pe1 on pe1.generalinfoid = gi.id and pe1.checkupDate<dateadd(day,50, t.fenmianrqsj) and pe1.checkupDate > dateadd(day,10, t.fenmianrqsj)
//	left join CC_PhysicalExam pe3 on pe3.generalinfoid = gi.id and pe3.checkupDate < dateadd(day,60, t.fenmianrqsj) and pe3.checkupDate > dateadd(day,120, t.fenmianrqsj)
//	left join CC_PhysicalExam pe6 on pe6.generalinfoid = gi.id and pe3.checkupDate < dateadd(day,150, t.fenmianrqsj) and pe3.checkupDate > dateadd(day,210, t.fenmianrqsj)
//	left join(
//		select tt.*, vr.sbp, vr.dbp, vr.weight
//		from (
//			select pi.idcard, max(vr.visitdate) maxvisitdate
//			from temp1210_distinct t
//			left join PregnantInfo pi on pi.idcard = t.idcard
//			left join MHC_VISITRECORD vr on vr.pregnantInfoId = pi.id
//			where vr.visitdate<t.fenmianrqsj
//			and dateadd(day,14, vr.visitdate) > t.fenmianrqsj
//			group by pi.idcard 
//		)as tt
//		left join MHC_VISITRECORD vr on vr.idcard = tt.idcard and vr.visitdate = tt.maxvisitdate
//	) as vr on vr.idcard = t.idcard
//	left join (
//		select t.idcard, vr.weight, vr.checkweek
//		from (
//			select pi.idcard, t.dateofprenatal, min(abs(28*7 - DATEDIFF(day, vr.visitdate, t.dateofprenatal))) mindiff
//			 from  temp1210_distinct t
//			left join PregnantInfo pi on pi.idcard = t.idcard
//			left join MHC_VISITRECORD vr on vr.pregnantInfoId = pi.id
//			where dateadd(day,40*7,vr.visitdate) > t.dateofprenatal
//			and dateadd(day,28*7, vr.visitdate) < t.dateofprenatal
//			group by pi.idcard , t.dateofprenatal
//		)as t
//		left join MHC_VISITRECORD vr on vr.idcard = t.idcard and abs(28*7 - DATEDIFF(day, vr.visitdate, t.dateofprenatal )) = t.mindiff
//	) as vr12 on vr12.idcard = t.idcard
//	left join (
//		select t.idcard, vr.weight, vr.checkweek
//		from (
//			select pi.idcard, t.dateofprenatal, min(abs(24*7 - DATEDIFF(day, vr.visitdate, t.dateofprenatal))) mindiff
//			 from  temp1210_distinct t
//			left join PregnantInfo pi on pi.idcard = t.idcard
//			left join MHC_VISITRECORD vr on vr.pregnantInfoId = pi.id
//			where dateadd(day,27*7,vr.visitdate) > t.dateofprenatal
//			and dateadd(day,19*7, vr.visitdate) < t.dateofprenatal
//			group by pi.idcard , t.dateofprenatal
//		)as t
//		left join MHC_VISITRECORD vr on vr.idcard = t.idcard and abs(24*7 - DATEDIFF(day, vr.visitdate, t.dateofprenatal )) = t.mindiff
//	) as vr16 on vr16.idcard = t.idcard
//	left join (
//		select t.idcard, vr.weight, vr.checkweek
//		from (
//			select pi.idcard, t.dateofprenatal, min(abs(16*7 - DATEDIFF(day, vr.visitdate, t.dateofprenatal))) mindiff
//			 from  temp1210_distinct t
//			left join PregnantInfo pi on pi.idcard = t.idcard
//			left join MHC_VISITRECORD vr on vr.pregnantInfoId = pi.id
//			where dateadd(day,18*7,vr.visitdate) > t.dateofprenatal
//			and dateadd(day,11*7, vr.visitdate) < t.dateofprenatal
//			group by pi.idcard , t.dateofprenatal
//		)as t
//		left join MHC_VISITRECORD vr on vr.idcard = t.idcard and abs(16*7 - DATEDIFF(day, vr.visitdate, t.dateofprenatal )) = t.mindiff
//	) as vr24 on vr24.idcard = t.idcard
//	left join (
//		select t.idcard, vr.weight, vr.checkweek
//		from (
//			select pi.idcard, t.dateofprenatal, min(abs(8*7 - DATEDIFF(day, vr.visitdate, t.dateofprenatal))) mindiff
//			 from  temp1210_distinct t
//			left join PregnantInfo pi on pi.idcard = t.idcard
//			left join MHC_VISITRECORD vr on vr.pregnantInfoId = pi.id
//			where dateadd(day,10*7,vr.visitdate) > t.dateofprenatal
//			and dateadd(day,6*7, vr.visitdate) < t.dateofprenatal
//			group by pi.idcard , t.dateofprenatal
//		)as t
//		left join MHC_VISITRECORD vr on vr.idcard = t.idcard and abs(8*7 - DATEDIFF(day, vr.visitdate, t.dateofprenatal )) = t.mindiff
//	) as vr32 on vr32.idcard = t.idcard
//	left join (
//		select t.idcard, vr.weight, vr.checkweek
//		from (
//			select pi.idcard, t.dateofprenatal, min(abs(4*7 - DATEDIFF(day, vr.visitdate, t.dateofprenatal))) mindiff
//			 from  temp1210_distinct t
//			left join PregnantInfo pi on pi.idcard = t.idcard
//			left join MHC_VISITRECORD vr on vr.pregnantInfoId = pi.id
//			where dateadd(day,5*7,vr.visitdate) > t.dateofprenatal
//			and dateadd(day,3*7, vr.visitdate) < t.dateofprenatal
//			group by pi.idcard , t.dateofprenatal
//		)as t
//		left join MHC_VISITRECORD vr on vr.idcard = t.idcard and abs(4*7 - DATEDIFF(day, vr.visitdate, t.dateofprenatal )) = t.mindiff
//	) as vr36 on vr36.idcard = t.idcard
//	left join (
//		select t.idcard, vr.weight, vr.checkweek
//		from (
//			select pi.idcard, t.dateofprenatal, min(abs(0*7 - DATEDIFF(day, vr.visitdate, t.dateofprenatal))) mindiff
//			 from  temp1210_distinct t
//			left join PregnantInfo pi on pi.idcard = t.idcard
//			left join MHC_VISITRECORD vr on vr.pregnantInfoId = pi.id
//			where dateadd(day,2*7,vr.visitdate) > t.dateofprenatal
//			and dateadd(day,-10*7, vr.visitdate) < t.dateofprenatal
//			group by pi.idcard , t.dateofprenatal
//		)as t
//		left join MHC_VISITRECORD vr on vr.idcard = t.idcard and abs(0*7 - DATEDIFF(day, vr.visitdate, t.dateofprenatal )) = t.mindiff
//	) as vr40 on vr40.idcard = t.idcard
//	left join (
//		select temp.*, lr.itemid, lr.itemname, lr.value
//		from
//		(
//				select t.idcard, t.dateofprenatal, max(lo.examtime) maxexamtime
//				from temp1210_distinct t
//			left join LabOrder lo on lo.idcard = t.idcard
//				left join LabResult lr on lo.orderid = lr.orderid
//				where lr.itemid = '0193'
//				and t.dateofprenatal > dateadd(day, 84, lo.examtime)
//				and t.dateofprenatal<dateadd(day, 280, lo.examtime)
//				group by t.idcard, t.dateofprenatal
//		) as temp
//		left join LabOrder lo on temp.idcard = lo.idcard and temp.maxexamtime = lo.examtime
//		left join LabResult lr on lo.orderid = lr.orderid
//		where lr.itemid = '0193' 
//		and temp.dateofprenatal > dateadd(day, 12*7, lo.examtime)
//		and temp.dateofprenatal<dateadd(day, 40*7, lo.examtime)
//	) as tyzq on t.idcard = tyzq.idcard
//	left join (
//		select temp.*, lr.itemid, lr.itemname, lr.value
//		from
//		(
//			select t.idcard, t.dateofprenatal, max(lo.examtime) maxexamtime
//			from temp1210_distinct t
//			left join LabOrder lo on lo.idcard = t.idcard
//			left join LabResult lr on lo.orderid = lr.orderid
//			where lr.itemid = '0193'
//			and t.dateofprenatal > dateadd(day, 24*7, lo.examtime)
//			and t.dateofprenatal<dateadd(day, 28*7, lo.examtime)
//			group by t.idcard, t.dateofprenatal
//		) as temp
//		left join LabOrder lo on temp.idcard = lo.idcard and temp.maxexamtime = lo.examtime
//		left join LabResult lr on lo.orderid = lr.orderid
//		where lr.itemid = '0193' 
//		and temp.dateofprenatal > dateadd(day, 24*7, lo.examtime)
//		and temp.dateofprenatal<dateadd(day, 28*7, lo.examtime)
//	) as t12xh on t12xh.idcard = t.idcard
//	left join
//	(
//		select temp.*, lr.itemid, lr.itemname, lr.value
//		from
//		(
//			select t.idcard, t.dateofprenatal, max(lo.examtime) maxexamtime
//			from temp1210_distinct t
//			left join LabOrder lo on lo.idcard = t.idcard
//			left join LabResult lr on lo.orderid = lr.orderid
//			where lr.itemid = '0258'
//			and t.dateofprenatal > dateadd(day, 24*7, lo.examtime)
//			and t.dateofprenatal<dateadd(day, 28*7, lo.examtime)
//			group by t.idcard, t.dateofprenatal
//		) as temp
//		left join LabOrder lo on temp.idcard = lo.idcard and temp.maxexamtime = lo.examtime
//		left join LabResult lr on lo.orderid = lr.orderid
//		where lr.itemid = '0258' 
//		and temp.dateofprenatal > dateadd(day, 24*7, lo.examtime)
//		and temp.dateofprenatal<dateadd(day, 28*7, lo.examtime)
//	) as t12t on t12t.idcard = t.idcard
//	left join
//	(
//		select temp.*, lr.itemid, lr.itemname, lr.value
//		from
//		(
//			select t.idcard, t.dateofprenatal, max(lo.examtime) maxexamtime
//			from temp1210_distinct t
//			left join LabOrder lo on lo.idcard = t.idcard
//			left join LabResult lr on lo.orderid = lr.orderid
//			where lr.itemid = '0193'
//			and t.dateofprenatal > dateadd(day, 12*7, lo.examtime)
//			and t.dateofprenatal<dateadd(day, 24*7, lo.examtime)
//			group by t.idcard, t.dateofprenatal
//		) as temp
//		left join LabOrder lo on temp.idcard = lo.idcard and temp.maxexamtime = lo.examtime
//		left join LabResult lr on lo.orderid = lr.orderid
//		where lr.itemid = '0193' 
//		and temp.dateofprenatal > dateadd(day, 12*7, lo.examtime)
//		and temp.dateofprenatal<dateadd(day, 24*7, lo.examtime)
//	) as t16xh on t16xh.idcard = t.idcard
//	left join
//	(
//		select temp.*, lr.itemid, lr.itemname, lr.value
//		from
//		(
//			select t.idcard, t.dateofprenatal, max(lo.examtime) maxexamtime
//			from temp1210_distinct t
//			left join LabOrder lo on lo.idcard = t.idcard
//			left join LabResult lr on lo.orderid = lr.orderid
//			where lr.itemid = '0258'
//			and t.dateofprenatal > dateadd(day, 12*7, lo.examtime)
//			and t.dateofprenatal<dateadd(day, 24*7, lo.examtime)
//			group by t.idcard, t.dateofprenatal
//		) as temp
//		left join LabOrder lo on temp.idcard = lo.idcard and temp.maxexamtime = lo.examtime
//		left join LabResult lr on lo.orderid = lr.orderid
//		where lr.itemid = '0258' 
//		and temp.dateofprenatal > dateadd(day, 12*7, lo.examtime)
//		and temp.dateofprenatal<dateadd(day, 24*7, lo.examtime)
//	) as t16t on t16t.idcard = t.idcard
//	left join
//	(
//		select temp.*, lr.itemid, lr.itemname, lr.value
//		from
//		(
//			select t.idcard, t.dateofprenatal, max(lo.examtime) maxexamtime
//			from temp1210_distinct t
//			left join LabOrder lo on lo.idcard = t.idcard
//			left join LabResult lr on lo.orderid = lr.orderid
//			where lr.itemid = '0193'
//			and t.dateofprenatal > dateadd(day, 3*7, lo.examtime)
//			and t.dateofprenatal<dateadd(day, 12*7, lo.examtime)
//			group by t.idcard, t.dateofprenatal
//		) as temp
//		left join LabOrder lo on temp.idcard = lo.idcard and temp.maxexamtime = lo.examtime
//		left join LabResult lr on lo.orderid = lr.orderid
//		where lr.itemid = '0193' 
//		and temp.dateofprenatal > dateadd(day, 3*7, lo.examtime)
//		and temp.dateofprenatal<dateadd(day, 12*7, lo.examtime)
//	) as t28xh on t28xh.idcard = t.idcard
//	left join
//	(
//		select temp.*, lr.itemid, lr.itemname, lr.value
//		from
//		(
//			select t.idcard, t.dateofprenatal, max(lo.examtime) maxexamtime
//			from temp1210_distinct t
//			left join LabOrder lo on lo.idcard = t.idcard
//			left join LabResult lr on lo.orderid = lr.orderid
//			where lr.itemid = '0258'
//			and t.dateofprenatal > dateadd(day, 3*7, lo.examtime)
//			and t.dateofprenatal<dateadd(day, 12*7, lo.examtime)
//			group by t.idcard, t.dateofprenatal
//		) as temp
//		left join LabOrder lo on temp.idcard = lo.idcard and temp.maxexamtime = lo.examtime
//		left join LabResult lr on lo.orderid = lr.orderid
//		where lr.itemid = '0258' 
//		and temp.dateofprenatal > dateadd(day, 3*7, lo.examtime)
//		and temp.dateofprenatal<dateadd(day, 12*7, lo.examtime)
//	) as t28t on t28t.idcard = t.idcard
//	left join
//	(
//		select temp.*, lr.itemid, lr.itemname, lr.value
//		from
//		(
//			select t.idcard, t.dateofprenatal, t.fenmianrqsj, max(lo.examtime) maxexamtime
//			from temp1210_distinct t
//			left join LabOrder lo on lo.idcard = t.idcard
//			left join LabResult lr on lo.orderid = lr.orderid
//			where lr.itemid = '0193'
//			and t.fenmianrqsj > dateadd(day, 0, lo.examtime)
//			and t.dateofprenatal<dateadd(day, 3*7, lo.examtime)
//			group by t.idcard, t.dateofprenatal, t.fenmianrqsj
//		) as temp
//		left join LabOrder lo on temp.idcard = lo.idcard and temp.maxexamtime = lo.examtime
//		left join LabResult lr on lo.orderid = lr.orderid
//		where lr.itemid = '0193' 
//		and temp.fenmianrqsj > dateadd(day, 0, lo.examtime)
//		and temp.dateofprenatal<dateadd(day, 3*7, lo.examtime)
//	) as t37xh on t37xh.idcard = t.idcard
//	left join
//	(
//		select temp.*, lr.itemid, lr.itemname, lr.value
//		from
//		(
//			select t.idcard, t.dateofprenatal, t.fenmianrqsj, max(lo.examtime) maxexamtime
//			from temp1210_distinct t
//			left join LabOrder lo on lo.idcard = t.idcard
//			left join LabResult lr on lo.orderid = lr.orderid
//			where lr.itemid = '0258'
//			and t.fenmianrqsj > dateadd(day, 0, lo.examtime)
//			and t.dateofprenatal<dateadd(day, 3*7, lo.examtime)
//			group by t.idcard, t.dateofprenatal, t.fenmianrqsj
//		) as temp
//		left join LabOrder lo on temp.idcard = lo.idcard and temp.maxexamtime = lo.examtime
//		left join LabResult lr on lo.orderid = lr.orderid
//		where lr.itemid = '0258' 
//		and temp.fenmianrqsj > dateadd(day, 0, lo.examtime)
//		and temp.dateofprenatal<dateadd(day, 3*7, lo.examtime)
//	) as t37t on t37t.idcard = t.idcard
//	left join
//	(
//		select temp.*, lr.itemid, lr.itemname, lr.value
//		from
//		(
//			select t.idcard, t.dateofprenatal, t.fenmianrqsj, max(lo.examtime) maxexamtime
//			from temp1210_distinct t
//			left join LabOrder lo on lo.idcard = t.idcard
//			left join LabResult lr on lo.orderid = lr.orderid
//			where lr.itemid = '0193'
//			and t.fenmianrqsj > dateadd(day, 0, lo.examtime)
//			and t.fenmianrqsj<dateadd(day, 6*7, lo.examtime)
//			group by t.idcard, t.dateofprenatal, t.fenmianrqsj
//		) as temp
//		left join LabOrder lo on temp.idcard = lo.idcard and temp.maxexamtime = lo.examtime
//		left join LabResult lr on lo.orderid = lr.orderid
//		where lr.itemid = '0193' 
//		and temp.fenmianrqsj > dateadd(day, 0, lo.examtime)
//		and temp.fenmianrqsj<dateadd(day, 6*7, lo.examtime)
//	) as t42xh on t42xh.idcard = t.idcard
//	left join
//	(
//		select temp.*, lr.itemid, lr.itemname, lr.value
//		from
//		(
//			select t.idcard, t.dateofprenatal, t.fenmianrqsj, max(lo.examtime) maxexamtime
//			from temp1210_distinct t
//			left join LabOrder lo on lo.idcard = t.idcard
//			left join LabResult lr on lo.orderid = lr.orderid
//			where lr.itemid = '9231'
//			and t.fenmianrqsj > dateadd(day, 0, lo.examtime)
//			and t.fenmianrqsj<dateadd(day, 2*7, lo.examtime)
//			group by t.idcard, t.dateofprenatal, t.fenmianrqsj
//		) as temp
//		left join LabOrder lo on temp.idcard = lo.idcard and temp.maxexamtime = lo.examtime
//		left join LabResult lr on lo.orderid = lr.orderid
//		where lr.itemid = '9231' 
//		and temp.fenmianrqsj > dateadd(day, 0, lo.examtime)
//		and temp.fenmianrqsj<dateadd(day, 2*7, lo.examtime)
//	) as tOGTT0 on tOGTT0.idcard = t.idcard
//	left join
//	(
//		select temp.*, lr.itemid, lr.itemname, lr.value
//		from
//		(
//			select t.idcard, t.dateofprenatal, t.fenmianrqsj, max(lo.examtime) maxexamtime
//			from temp1210_distinct t
//			left join LabOrder lo on lo.idcard = t.idcard
//			left join LabResult lr on lo.orderid = lr.orderid
//			where lr.itemid = '0296'
//			and t.fenmianrqsj > dateadd(day, 0, lo.examtime)
//			and t.fenmianrqsj<dateadd(day, 2*7, lo.examtime)
//			group by t.idcard, t.dateofprenatal, t.fenmianrqsj
//		) as temp
//		left join LabOrder lo on temp.idcard = lo.idcard and temp.maxexamtime = lo.examtime
//		left join LabResult lr on lo.orderid = lr.orderid
//		where lr.itemid = '0296' 
//		and temp.fenmianrqsj > dateadd(day, 0, lo.examtime)
//		and temp.fenmianrqsj<dateadd(day, 2*7, lo.examtime)
//	) as tOGTT1 on tOGTT1.idcard = t.idcard
//	left join
//	(
//		select temp.*, lr.itemid, lr.itemname, lr.value
//		from
//		(
//			select t.idcard, t.dateofprenatal, t.fenmianrqsj, max(lo.examtime) maxexamtime
//			from temp1210_distinct t
//			left join LabOrder lo on lo.idcard = t.idcard
//			left join LabResult lr on lo.orderid = lr.orderid
//			where lr.itemid = '0298'
//			and t.fenmianrqsj > dateadd(day, 0, lo.examtime)
//			and t.fenmianrqsj<dateadd(day, 2*7, lo.examtime)
//			group by t.idcard, t.dateofprenatal, t.fenmianrqsj
//		) as temp
//		left join LabOrder lo on temp.idcard = lo.idcard and temp.maxexamtime = lo.examtime
//		left join LabResult lr on lo.orderid = lr.orderid
//		where lr.itemid = '0298' 
//		and temp.fenmianrqsj > dateadd(day, 0, lo.examtime)
//		and temp.fenmianrqsj<dateadd(day, 2*7, lo.examtime)
//	) as tOGTT2 on tOGTT2.idcard = t.idcard
//	left join
//	(
//		select temp.*, lr.itemid, lr.itemname, lr.value
//		from
//		(
//			select t.idcard, t.dateofprenatal, t.fenmianrqsj, max(lo.examtime) maxexamtime
//			from temp1210_distinct t
//			left join LabOrder lo on lo.idcard = t.idcard
//			left join LabResult lr on lo.orderid = lr.orderid
//			where lr.itemid = '0308'
//			and t.fenmianrqsj > dateadd(day, 0, lo.examtime)
//			and t.fenmianrqsj<dateadd(day, 2*7, lo.examtime)
//			group by t.idcard, t.dateofprenatal, t.fenmianrqsj
//		) as temp
//		left join LabOrder lo on temp.idcard = lo.idcard and temp.maxexamtime = lo.examtime
//		left join LabResult lr on lo.orderid = lr.orderid
//		where lr.itemid = '0308' 
//		and temp.fenmianrqsj > dateadd(day, 0, lo.examtime)
//		and temp.fenmianrqsj<dateadd(day, 2*7, lo.examtime)
//	) as t0308 on t0308.idcard = t.idcard
//	left join
//	(
//		select t.idcard, t.dateofprenatal, t.fenmianrqsj, max(lr.value) maxvalue
//		from temp1210_distinct t
//		left join LabOrder lo on lo.idcard = t.idcard
//		left join LabResult lr on lo.orderid = lr.orderid
//		where lr.itemid = '0308'
//		and t.fenmianrqsj > dateadd(day, 0, lo.examtime)
//		and t.fenmianrqsj<dateadd(day, 2*7, lo.examtime)
//		group by t.idcard, t.dateofprenatal, t.fenmianrqsj
//	) as t0308max on t0308max.idcard = t.idcard 