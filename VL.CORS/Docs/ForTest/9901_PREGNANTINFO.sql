/*
 Navicat Premium Data Transfer

 Source Server         : 181
 Source Server Type    : SQL Server
 Source Server Version : 11006251
 Source Host           : 192.189.101.181:1433
 Source Catalog        : HANGZHOUSFB182
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 11006251
 File Encoding         : 65001

 Date: 30/12/2020 11:06:44
*/


-- ----------------------------
-- Table structure for PREGNANTINFO
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[PREGNANTINFO]') AND type IN ('U'))
	DROP TABLE [dbo].[PREGNANTINFO]
GO

CREATE TABLE [dbo].[PREGNANTINFO] (
  [id] int  IDENTITY(1,1) NOT NULL,
  [personname] varchar(20) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [sexcode] char(1) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [photo] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [cardno] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [idcard] varchar(30) COLLATE Chinese_PRC_CI_AS  NULL,
  [registeredpermanent] varchar(2) COLLATE Chinese_PRC_CI_AS  NULL,
  [bloodtypecode] char(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [homeplace] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [address] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [zipcode] varchar(6) COLLATE Chinese_PRC_CI_AS  NULL,
  [birthday] datetime  NULL,
  [nationalitycode] varchar(3) COLLATE Chinese_PRC_CI_AS  NULL,
  [nationcode] varchar(2) COLLATE Chinese_PRC_CI_AS  NULL,
  [rhbloodcode] char(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [maritalstatuscode] char(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [workcode] varchar(3) COLLATE Chinese_PRC_CI_AS  NULL,
  [insurancecode] varchar(2) COLLATE Chinese_PRC_CI_AS  NULL,
  [educationcode] varchar(2) COLLATE Chinese_PRC_CI_AS  NULL,
  [status] char(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [workplace] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [contact] varchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
  [contactphone] varchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
  [insurancetype] varchar(2) COLLATE Chinese_PRC_CI_AS  NULL,
  [isagrregister] char(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [dateofprenatal] date  NULL,
  [gravidity] varchar(10) COLLATE Chinese_PRC_CI_AS  NULL,
  [phrid] varchar(17) COLLATE Chinese_PRC_CI_AS  NULL,
  [parity] varchar(10) COLLATE Chinese_PRC_CI_AS  NULL,
  [predeliverymode] varchar(2) COLLATE Chinese_PRC_CI_AS  NULL,
  [lastmenstrualperiod] date  NULL,
  [gestationneuropathy] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [operationhistory] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [sbp] int  NULL,
  [dbp] int  NULL,
  [homeaddress] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [pregnantbookid] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [mhcdoctorname] varchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
  [restregioncode] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [ownerarea] varchar(16) COLLATE Chinese_PRC_CI_AS  NULL,
  [realregioncode] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [residencecode] varchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
  [residencepermit] date  NULL,
  [menarcheage] int  NULL,
  [menstrualperiod] int  NULL,
  [cycle] int  NULL,
  [menstrualblood] char(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [dysmenorrhea] char(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [unusualbone] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [trafficflow] int  NULL,
  [naturalabortion] int  NULL,
  [odinopoeia] int  NULL,
  [preterm] int  NULL,
  [dystocia] int  NULL,
  [died] int  NULL,
  [abnormality] int  NULL,
  [newbrondied] int  NULL,
  [qwetimes] int  NULL,
  [ectopicpregnancy] int  NULL,
  [vesicularmole] int  NULL,
  [pregestationdate] date  NULL,
  [pregestationmode] varchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
  [predeliverydate] date  NULL,
  [pasthistory] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [familyhistory] varchar(350) COLLATE Chinese_PRC_CI_AS  NULL,
  [gynecologyops] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [allergichistory] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [poisontouchhis] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [heredityfamilyhistory] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandfamilyhistory] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [gestationalweeks] int  NULL,
  [height] decimal(8,2)  NULL,
  [weight] decimal(8,2)  NULL,
  [pregnantid] varchar(30) COLLATE Chinese_PRC_CI_AS  NULL,
  [manaunitid] varchar(30) COLLATE Chinese_PRC_CI_AS  NULL,
  [createdate] date  NULL,
  [mark] char(1) COLLATE Chinese_PRC_CI_AS DEFAULT ((0)) NULL,
  [uploadtime] date  NULL,
  [updatetime] datetime DEFAULT (getdate()) NULL,
  [reserved0] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [reserved1] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [reserved2] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [reserved3] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [reserved4] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [phonenumber] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [mobilenumber] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [responsedoctorname] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandname] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [inputpregweeks] int  NULL,
  [folicsupplementation] char(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [patientid] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [patientaccount] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [schedule] varchar(5000) COLLATE Chinese_PRC_CI_AS  NULL,
  [homeaddress_text] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [followupdoctorname] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [createtime] datetime DEFAULT (getdate()) NULL,
  [editorcode] varchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
  [editorname] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [pulse] int  NULL,
  [dateofprenatalflag] int DEFAULT ((0)) NULL,
  [lastmodifydate] datetime  NULL,
  [create_localuser] varchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
  [sourceunit] varchar(22) COLLATE Chinese_PRC_CI_AS  NULL,
  [updateflag] char(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [deliverydate] datetime  NULL,
  [builtkcal] datetime  NULL,
  [daqtaskid] varchar(17) COLLATE Chinese_PRC_CI_AS  NULL,
  [deliverytime] datetime2(7)  NULL,
  [deliverystatus] char(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [deliveryid] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [nutrition_visit] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [psy_visit] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [cmed_visit] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [medquery_visit] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [filestatus] int DEFAULT ((0)) NULL,
  [embryowithdrawal] int  NULL,
  [gestationaldays] int  NULL,
  [husbandfamilyhistorytext] varchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [liveplace] varchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [workname] varchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [termbirthtimes] int  NULL,
  [pretermbirthtimes] int  NULL,
  [abortiontimes] int  NULL,
  [existingchildrennum] int  NULL,
  [pregnanthistory] varchar(2000) COLLATE Chinese_PRC_CI_AS  NULL,
  [tpregnancymanner] char(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [bmi] varchar(10) COLLATE Chinese_PRC_CI_AS  NULL,
  [tpregnancymanner_text] varchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [liveplace_text] varchar(1000) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandidcard] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [changein_unit] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [changein_date] date  NULL,
  [changein_ascription] varchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [jiuzhenid] nchar(10) COLLATE Chinese_PRC_CI_AS  NULL,
  [idtype] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [createage] int  NULL,
  [husbandbirthday] date  NULL,
  [husbandage] int  NULL,
  [husbandmobile] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandworkplace] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandnationcode] varchar(2) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandnationalitycode] varchar(3) COLLATE Chinese_PRC_CI_AS  NULL,
  [homeaddressphone] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [liveaddresscode] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [liveaddresstext] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [restregiontext] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [restregionphone] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandhomeaddress] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandhomeaddress_text] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [registrationtype] varchar(2) COLLATE Chinese_PRC_CI_AS  NULL,
  [healthplace] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [sourceunittext] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [nationtext] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [vaginaldeliverynum] int  NULL,
  [caesareansectionnum] int  NULL,
  [presenthistory] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [presenthistorytext] varchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [pasthistorytext] varchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [pasthistoryval] varchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [heredityfamilyhistorytext] varchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [mentalhistory] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [mentalhistorytext] varchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [retarded] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [retardedlevel] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [retardedtext] varchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [malformation] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [malformationtext] varchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [otherfamilyhistory] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [otherfamilyhistorytext] varchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [husheredityfamilyhistory] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [husheredityfamilyhistorytext] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [husmentalhistory] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [husmentalhistorytext] varchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [husretarded] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [husretardedlevel] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [husretardedtext] varchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [husmalformation] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [husmalformationtext] varchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [husotherfamilyhistory] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [husotherfamilyhistorytext] varchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [personnalhistory] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [smoking] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [smokingtext] varchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [drinking] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [drinkingtext] varchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [medicine] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [medicinetext] varchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [allergichistorytext] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [poisontouchhistext] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [radiation] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [radiationtext] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [traumahistory] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [traumahistorytext] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [otherpersonnalhistory] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [otherpersonnalhistorytext] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [gynecologyopstext] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [otheroperationhistory] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [otheroperationhistorytext] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [contraceptionhistory] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [contraception] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [contraceptiontext] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [pregnancyhistory] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [downloadtime] datetime  NULL,
  [dateofprenataltext] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [birthhistorytext] varchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [manageaddresscode] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [manageaddresstext] varchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [isflowpeople] char(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [cyclemin] int  NULL,
  [cyclemax] int  NULL,
  [menstrualperiodmin] int  NULL,
  [menstrualperiodmax] int  NULL,
  [husbandidtype] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandworkcode] varchar(3) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandworkname] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandeducationcode] varchar(2) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandphonenumber] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandliveaddresscode] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandliveaddresstext] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandregistrationtype] varchar(2) COLLATE Chinese_PRC_CI_AS  NULL,
  [preblood] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [afterblood] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [pih] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandisagrregister] char(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [died_fetal] int  NULL,
  [died_stillbirth] int  NULL,
  [overdelivery] int  NULL,
  [restregionname] varchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [homeaddressname] varchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandhomeaddressname] varchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandliveaddressname] varchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [iscreatebook] char(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [dateofprenatalmodifyreason] varchar(1000) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandbloodtypecode] varchar(10) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandrhbloodcode] varchar(10) COLLATE Chinese_PRC_CI_AS  NULL,
  [createbookunit] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [boyname] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [girlname] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [marryyears] int  NULL,
  [husbandpresenthistory] varchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandpersonnalhistory] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandsmoking] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandsmokingtext] varchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbanddrinking] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbanddrinkingtext] varchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [closemarry] varchar(1) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandliveaddresszipcode] varchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
  [husbandemermobile] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [emrmobilenumber] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [restregionmobilenumber] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [homeaddressowner] varchar(1000) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[PREGNANTINFO] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'编号',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'孕妇姓名',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'personname'
GO

EXEC sp_addextendedproperty
'MS_Description', N'性别',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'sexcode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'证件照编号',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'photo'
GO

EXEC sp_addextendedproperty
'MS_Description', N'就诊卡号',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'cardno'
GO

EXEC sp_addextendedproperty
'MS_Description', N'孕妇证件号',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'idcard'
GO

EXEC sp_addextendedproperty
'MS_Description', N'户籍标志',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'registeredpermanent'
GO

EXEC sp_addextendedproperty
'MS_Description', N'血型',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'bloodtypecode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'出生地',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'homeplace'
GO

EXEC sp_addextendedproperty
'MS_Description', N'联系地址',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'address'
GO

EXEC sp_addextendedproperty
'MS_Description', N'邮政编码',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'zipcode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'出生日期',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'birthday'
GO

EXEC sp_addextendedproperty
'MS_Description', N'国籍',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'nationalitycode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'民族',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'nationcode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'RH血型',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'rhbloodcode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'婚姻状况',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'maritalstatuscode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'职业类别',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'workcode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'保险类别',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'insurancecode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'文化程度',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'educationcode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'状态',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'status'
GO

EXEC sp_addextendedproperty
'MS_Description', N'工作单位',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'workplace'
GO

EXEC sp_addextendedproperty
'MS_Description', N'联系人',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'contact'
GO

EXEC sp_addextendedproperty
'MS_Description', N'联系人电话',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'contactphone'
GO

EXEC sp_addextendedproperty
'MS_Description', N'医保类别',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'insurancetype'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否农业户籍',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'isagrregister'
GO

EXEC sp_addextendedproperty
'MS_Description', N'预产期',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'dateofprenatal'
GO

EXEC sp_addextendedproperty
'MS_Description', N'孕次',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'gravidity'
GO

EXEC sp_addextendedproperty
'MS_Description', N'健康档案编号',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'phrid'
GO

EXEC sp_addextendedproperty
'MS_Description', N'产次',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'parity'
GO

EXEC sp_addextendedproperty
'MS_Description', N'前次分娩方式',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'predeliverymode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'末次月经时间',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'lastmenstrualperiod'
GO

EXEC sp_addextendedproperty
'MS_Description', N'妊娠并发症史',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'gestationneuropathy'
GO

EXEC sp_addextendedproperty
'MS_Description', N'手术史',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'operationhistory'
GO

EXEC sp_addextendedproperty
'MS_Description', N'收缩压(mmHg)',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'sbp'
GO

EXEC sp_addextendedproperty
'MS_Description', N'舒张压(mmHg)',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'dbp'
GO

EXEC sp_addextendedproperty
'MS_Description', N'户籍地址',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'homeaddress'
GO

EXEC sp_addextendedproperty
'MS_Description', N'孕册号',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'pregnantbookid'
GO

EXEC sp_addextendedproperty
'MS_Description', N'妇保医生姓名',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'mhcdoctorname'
GO

EXEC sp_addextendedproperty
'MS_Description', N'产休地',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'restregioncode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'孕妇归属地',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'ownerarea'
GO

EXEC sp_addextendedproperty
'MS_Description', N'居(暂)住地址',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'realregioncode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'居(暂)住地址-文本',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'residencecode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'办居住证日期',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'residencepermit'
GO

EXEC sp_addextendedproperty
'MS_Description', N'初潮年龄',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'menarcheage'
GO

EXEC sp_addextendedproperty
'MS_Description', N'经期(天)',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'menstrualperiod'
GO

EXEC sp_addextendedproperty
'MS_Description', N'周期',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'cycle'
GO

EXEC sp_addextendedproperty
'MS_Description', N'月经量',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'menstrualblood'
GO

EXEC sp_addextendedproperty
'MS_Description', N'痛经',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'dysmenorrhea'
GO

EXEC sp_addextendedproperty
'MS_Description', N'(无)异常孕产史',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'unusualbone'
GO

EXEC sp_addextendedproperty
'MS_Description', N'人工流产次数',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'trafficflow'
GO

EXEC sp_addextendedproperty
'MS_Description', N'自然流产次数',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'naturalabortion'
GO

EXEC sp_addextendedproperty
'MS_Description', N'中期引产次数',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'odinopoeia'
GO

EXEC sp_addextendedproperty
'MS_Description', N'早产次数',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'preterm'
GO

EXEC sp_addextendedproperty
'MS_Description', N'难产次数',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'dystocia'
GO

EXEC sp_addextendedproperty
'MS_Description', N'死胎死产次数',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'died'
GO

EXEC sp_addextendedproperty
'MS_Description', N'畸形儿次数',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'abnormality'
GO

EXEC sp_addextendedproperty
'MS_Description', N'死亡儿次数',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'newbrondied'
GO

EXEC sp_addextendedproperty
'MS_Description', N'药物流产次数',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'qwetimes'
GO

EXEC sp_addextendedproperty
'MS_Description', N'宫外孕次数',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'ectopicpregnancy'
GO

EXEC sp_addextendedproperty
'MS_Description', N'葡萄胎次数',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'vesicularmole'
GO

EXEC sp_addextendedproperty
'MS_Description', N'前妊娠终止日期',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'pregestationdate'
GO

EXEC sp_addextendedproperty
'MS_Description', N'前次终止方式',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'pregestationmode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'前次分娩日期',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'predeliverydate'
GO

EXEC sp_addextendedproperty
'MS_Description', N'既往病史',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'pasthistory'
GO

EXEC sp_addextendedproperty
'MS_Description', N'家族史',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'familyhistory'
GO

EXEC sp_addextendedproperty
'MS_Description', N'妇科手术史',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'gynecologyops'
GO

EXEC sp_addextendedproperty
'MS_Description', N'过敏史',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'allergichistory'
GO

EXEC sp_addextendedproperty
'MS_Description', N'毒物接触史',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'poisontouchhis'
GO

EXEC sp_addextendedproperty
'MS_Description', N'遗传家族史',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'heredityfamilyhistory'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫家族史',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandfamilyhistory'
GO

EXEC sp_addextendedproperty
'MS_Description', N'建册孕周',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'gestationalweeks'
GO

EXEC sp_addextendedproperty
'MS_Description', N'身高(cm)',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'height'
GO

EXEC sp_addextendedproperty
'MS_Description', N'基础体重(kg)',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'weight'
GO

EXEC sp_addextendedproperty
'MS_Description', N'孕妇档案号',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'pregnantid'
GO

EXEC sp_addextendedproperty
'MS_Description', N'管辖机构',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'manaunitid'
GO

EXEC sp_addextendedproperty
'MS_Description', N'建册日期',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'createdate'
GO

EXEC sp_addextendedproperty
'MS_Description', N'处理标志',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'mark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'上传时间',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'uploadtime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'处理时间',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'updatetime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'预留0',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'reserved0'
GO

EXEC sp_addextendedproperty
'MS_Description', N'预留1',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'reserved1'
GO

EXEC sp_addextendedproperty
'MS_Description', N'预留2',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'reserved2'
GO

EXEC sp_addextendedproperty
'MS_Description', N'预留3',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'reserved3'
GO

EXEC sp_addextendedproperty
'MS_Description', N'预留4',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'reserved4'
GO

EXEC sp_addextendedproperty
'MS_Description', N'家庭电话',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'phonenumber'
GO

EXEC sp_addextendedproperty
'MS_Description', N'本人电话',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'mobilenumber'
GO

EXEC sp_addextendedproperty
'MS_Description', N'妇保医生姓名',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'responsedoctorname'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫姓名',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandname'
GO

EXEC sp_addextendedproperty
'MS_Description', N'录入孕周',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'inputpregweeks'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否补充叶酸',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'folicsupplementation'
GO

EXEC sp_addextendedproperty
'MS_Description', N'病人id',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'patientid'
GO

EXEC sp_addextendedproperty
'MS_Description', N'医保卡号',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'patientaccount'
GO

EXEC sp_addextendedproperty
'MS_Description', N'计划表',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'schedule'
GO

EXEC sp_addextendedproperty
'MS_Description', N'户籍地址-文本',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'homeaddress_text'
GO

EXEC sp_addextendedproperty
'MS_Description', N'随访医生姓名',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'followupdoctorname'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建时间',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'createtime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'录入护士工号',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'editorcode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'录入护士姓名',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'editorname'
GO

EXEC sp_addextendedproperty
'MS_Description', N'脉搏',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'pulse'
GO

EXEC sp_addextendedproperty
'MS_Description', N'预产期修正标志',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'dateofprenatalflag'
GO

EXEC sp_addextendedproperty
'MS_Description', N'最后修改时间',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'lastmodifydate'
GO

EXEC sp_addextendedproperty
'MS_Description', N'建档医生',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'create_localuser'
GO

EXEC sp_addextendedproperty
'MS_Description', N'来源单位',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'sourceunit'
GO

EXEC sp_addextendedproperty
'MS_Description', N'修改标志',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'updateflag'
GO

EXEC sp_addextendedproperty
'MS_Description', N'分娩日期',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'deliverydate'
GO

EXEC sp_addextendedproperty
'MS_Description', N'建大卡时间',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'builtkcal'
GO

EXEC sp_addextendedproperty
'MS_Description', N'医养护任务id',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'daqtaskid'
GO

EXEC sp_addextendedproperty
'MS_Description', N'医养护传输时间',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'deliverytime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'医养护传输状态',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'deliverystatus'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否去过营养门诊',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'nutrition_visit'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否去过心理门诊',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'psy_visit'
GO

EXEC sp_addextendedproperty
'MS_Description', N'去过中医门诊',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'cmed_visit'
GO

EXEC sp_addextendedproperty
'MS_Description', N'去过用药咨询门诊',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'medquery_visit'
GO

EXEC sp_addextendedproperty
'MS_Description', N'结案标志',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'filestatus'
GO

EXEC sp_addextendedproperty
'MS_Description', N'胚胎停育次数',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'embryowithdrawal'
GO

EXEC sp_addextendedproperty
'MS_Description', N'建册孕天',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'gestationaldays'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫家族史详情',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandfamilyhistorytext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'居住地址',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'liveplace'
GO

EXEC sp_addextendedproperty
'MS_Description', N'职业',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'workname'
GO

EXEC sp_addextendedproperty
'MS_Description', N'足月产次数',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'termbirthtimes'
GO

EXEC sp_addextendedproperty
'MS_Description', N'早产次数',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'pretermbirthtimes'
GO

EXEC sp_addextendedproperty
'MS_Description', N'流产次数',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'abortiontimes'
GO

EXEC sp_addextendedproperty
'MS_Description', N'现存子女数',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'existingchildrennum'
GO

EXEC sp_addextendedproperty
'MS_Description', N'孕产史',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'pregnanthistory'
GO

EXEC sp_addextendedproperty
'MS_Description', N'本次怀孕方式',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'tpregnancymanner'
GO

EXEC sp_addextendedproperty
'MS_Description', N'体质指数',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'bmi'
GO

EXEC sp_addextendedproperty
'MS_Description', N'本次怀孕详情',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'tpregnancymanner_text'
GO

EXEC sp_addextendedproperty
'MS_Description', N'居住地址详情',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'liveplace_text'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫身份证号',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandidcard'
GO

EXEC sp_addextendedproperty
'MS_Description', N'转入机构',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'changein_unit'
GO

EXEC sp_addextendedproperty
'MS_Description', N'转入日期',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'changein_date'
GO

EXEC sp_addextendedproperty
'MS_Description', N'转入来源区属',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'changein_ascription'
GO

EXEC sp_addextendedproperty
'MS_Description', N'门诊号',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'jiuzhenid'
GO

EXEC sp_addextendedproperty
'MS_Description', N'证件类型',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'idtype'
GO

EXEC sp_addextendedproperty
'MS_Description', N'建册年龄',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'createage'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫出生日期',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandbirthday'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫年龄 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandage'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫联系电话 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandmobile'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫工作单位 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandworkplace'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫民族 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandnationcode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫国籍编码 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandnationalitycode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'户籍地址联系电话 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'homeaddressphone'
GO

EXEC sp_addextendedproperty
'MS_Description', N'家庭地址编号 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'liveaddresscode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'家庭地址名称 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'liveaddresstext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'产后休养地址',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'restregiontext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'产后休养地址电话 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'restregionphone'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫户籍地址编号 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandhomeaddress'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫户籍地址 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandhomeaddress_text'
GO

EXEC sp_addextendedproperty
'MS_Description', N'户口类型 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'registrationtype'
GO

EXEC sp_addextendedproperty
'MS_Description', N'生殖健康服务证发放地 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'healthplace'
GO

EXEC sp_addextendedproperty
'MS_Description', N'录入机构名称 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'sourceunittext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'孕妇民族名称 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'nationtext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'阴道分娩 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'vaginaldeliverynum'
GO

EXEC sp_addextendedproperty
'MS_Description', N'剖宫产 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'caesareansectionnum'
GO

EXEC sp_addextendedproperty
'MS_Description', N'现病史 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'presenthistory'
GO

EXEC sp_addextendedproperty
'MS_Description', N'现病史异常详述 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'presenthistorytext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'既往病史详述',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'pasthistorytext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'既往病史详述值，逗号相隔',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'pasthistoryval'
GO

EXEC sp_addextendedproperty
'MS_Description', N'遗传家族史',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'heredityfamilyhistorytext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'精神疾病史 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'mentalhistory'
GO

EXEC sp_addextendedproperty
'MS_Description', N'精神疾病史详述 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'mentalhistorytext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'智障 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'retarded'
GO

EXEC sp_addextendedproperty
'MS_Description', N'智障程度 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'retardedlevel'
GO

EXEC sp_addextendedproperty
'MS_Description', N'智障详述 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'retardedtext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'畸形 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'malformation'
GO

EXEC sp_addextendedproperty
'MS_Description', N'畸形详述 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'malformationtext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'孕妇其他家族病史 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'otherfamilyhistory'
GO

EXEC sp_addextendedproperty
'MS_Description', N'孕妇其他家族史详述 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'otherfamilyhistorytext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫遗传性疾病史 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husheredityfamilyhistory'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫遗传性疾病史详述 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husheredityfamilyhistorytext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'精神疾病史 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husmentalhistory'
GO

EXEC sp_addextendedproperty
'MS_Description', N'精神疾病史详述 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husmentalhistorytext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'智障 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husretarded'
GO

EXEC sp_addextendedproperty
'MS_Description', N'智障程度 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husretardedlevel'
GO

EXEC sp_addextendedproperty
'MS_Description', N'智障详述 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husretardedtext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'畸形 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husmalformation'
GO

EXEC sp_addextendedproperty
'MS_Description', N'畸形详述 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husmalformationtext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫其他家族病史 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husotherfamilyhistory'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫其他家族史详述 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husotherfamilyhistorytext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'个人史 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'personnalhistory'
GO

EXEC sp_addextendedproperty
'MS_Description', N'吸烟 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'smoking'
GO

EXEC sp_addextendedproperty
'MS_Description', N'吸烟详述 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'smokingtext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'饮酒 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'drinking'
GO

EXEC sp_addextendedproperty
'MS_Description', N'饮酒详述 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'drinkingtext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'服用药物 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'medicine'
GO

EXEC sp_addextendedproperty
'MS_Description', N'服用药物详述 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'medicinetext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'药物过敏史详述 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'allergichistorytext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'接触有毒有害物质详述 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'poisontouchhistext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'接触放射线 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'radiation'
GO

EXEC sp_addextendedproperty
'MS_Description', N'接触放射线详述 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'radiationtext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'外伤史 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'traumahistory'
GO

EXEC sp_addextendedproperty
'MS_Description', N'外伤史详述 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'traumahistorytext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'其他个人史 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'otherpersonnalhistory'
GO

EXEC sp_addextendedproperty
'MS_Description', N'其他个人史详述 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'otherpersonnalhistorytext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'妇科手术史',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'gynecologyopstext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'其他手术史 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'otheroperationhistory'
GO

EXEC sp_addextendedproperty
'MS_Description', N'其他手术史详述 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'otheroperationhistorytext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否有避孕史 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'contraceptionhistory'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否避孕 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'contraception'
GO

EXEC sp_addextendedproperty
'MS_Description', N'避孕史详述 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'contraceptiontext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'孕产史 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'pregnancyhistory'
GO

EXEC sp_addextendedproperty
'MS_Description', N'下载时间',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'downloadtime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'预产期详述',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'dateofprenataltext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'生育史',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'birthhistorytext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'管辖地址编码',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'manageaddresscode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'管辖地址-详细地址',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'manageaddresstext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否流动人群',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'isflowpeople'
GO

EXEC sp_addextendedproperty
'MS_Description', N'周期-下限',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'cyclemin'
GO

EXEC sp_addextendedproperty
'MS_Description', N'周期-上限',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'cyclemax'
GO

EXEC sp_addextendedproperty
'MS_Description', N'经期(天)-下限',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'menstrualperiodmin'
GO

EXEC sp_addextendedproperty
'MS_Description', N'经期(天)-上限',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'menstrualperiodmax'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫证件类型',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandidtype'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫职业类别',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandworkcode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫职业名称',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandworkname'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫文化程度',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandeducationcode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫家庭电话',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandphonenumber'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫家庭地址编号 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandliveaddresscode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫家庭地址名称 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandliveaddresstext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫户口类型 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandregistrationtype'
GO

EXEC sp_addextendedproperty
'MS_Description', N'产前出血',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'preblood'
GO

EXEC sp_addextendedproperty
'MS_Description', N'产后出血',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'afterblood'
GO

EXEC sp_addextendedproperty
'MS_Description', N'妊高症',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'pih'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫是否农业户籍',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandisagrregister'
GO

EXEC sp_addextendedproperty
'MS_Description', N'死胎次数',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'died_fetal'
GO

EXEC sp_addextendedproperty
'MS_Description', N'死产次数',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'died_stillbirth'
GO

EXEC sp_addextendedproperty
'MS_Description', N'过期产次数',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'overdelivery'
GO

EXEC sp_addextendedproperty
'MS_Description', N'产休地名称',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'restregionname'
GO

EXEC sp_addextendedproperty
'MS_Description', N'户籍地址名称',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'homeaddressname'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫户籍地址名称',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandhomeaddressname'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫居住地址名称',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandliveaddressname'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否建册',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'iscreatebook'
GO

EXEC sp_addextendedproperty
'MS_Description', N'预产期修改原因',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'dateofprenatalmodifyreason'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫ABO血型',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandbloodtypecode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫Rh血型',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandrhbloodcode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'建册机构',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'createbookunit'
GO

EXEC sp_addextendedproperty
'MS_Description', N'打算给孩子取的名字：男孩名字',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'boyname'
GO

EXEC sp_addextendedproperty
'MS_Description', N'打算给孩子取的名字：女孩名字',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'girlname'
GO

EXEC sp_addextendedproperty
'MS_Description', N'结婚年数',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'marryyears'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫现病史',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandpresenthistory'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫个人史 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandpersonnalhistory'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫吸烟 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandsmoking'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫吸烟详述 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandsmokingtext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫饮酒 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbanddrinking'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫饮酒详述 ',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbanddrinkingtext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否近亲结婚',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'closemarry'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫居住地址邮编',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandliveaddresszipcode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'丈夫紧急联系电话',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'husbandemermobile'
GO

EXEC sp_addextendedproperty
'MS_Description', N'孕妇紧急联系电话',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'emrmobilenumber'
GO

EXEC sp_addextendedproperty
'MS_Description', N'孕妇产后联系电话',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'restregionmobilenumber'
GO

EXEC sp_addextendedproperty
'MS_Description', N'户主',
'SCHEMA', N'dbo',
'TABLE', N'PREGNANTINFO',
'COLUMN', N'homeaddressowner'
GO


-- ----------------------------
-- Auto increment value for PREGNANTINFO
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[PREGNANTINFO]', RESEED, 309212)
GO


-- ----------------------------
-- Indexes structure for table PREGNANTINFO
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_PREGNANTINFO]
ON [dbo].[PREGNANTINFO] (
  [idcard] ASC
)
WITH (
  FILLFACTOR = 90
)
GO

CREATE NONCLUSTERED INDEX [IX_PREGNANTINFO_1]
ON [dbo].[PREGNANTINFO] (
  [pregnantid] ASC
)
WITH (
  FILLFACTOR = 90
)
GO


-- ----------------------------
-- Primary Key structure for table PREGNANTINFO
-- ----------------------------
ALTER TABLE [dbo].[PREGNANTINFO] ADD CONSTRAINT [PK_PREGNANTINFO] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, FILLFACTOR = 90, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

