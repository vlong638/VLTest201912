ALTER TABLE [dbo].[GY_FUWUJG] ADD HZStatisticsType tinyint  NULL
GO

CREATE NONCLUSTERED INDEX [idx_GY_FUWUJG_HZStatisticsType]
ON [dbo].[GY_FUWUJG] (
  HZStatisticsType ASC
)
GO

EXEC sp_addextendedproperty
'MS_Description', N'湖州统计过滤',
'SCHEMA', N'dbo',
'TABLE', N'GY_FUWUJG',
'COLUMN', N'HZStatisticsType'


--行政过滤
--A1.儿保年报表 ChildHeathArea @夏锦慧 @1014
--A4.五岁以下死亡监测 DeathMonitorUnder5Area @夏锦慧 @1015 (街道过滤DAIMA)
--A5.五岁以下儿童死亡信息 WSYXETSWXXOrganization @张振楠 @1022
--A7.浙江省高危儿管理工作报表 @夏锦慧 GWEArea  @1008
--A8.湖州市流动儿童保健服务情况报表 LDETBJFWJKBBArea @张振楠  @1015
--new A9.早产儿管理情况统计报 @夏锦慧 PrematureBabyManagementArea @1020 专案
--new A11.五岁以下死亡监测(季度) DeathMonitorUnder5QuarterlyArea

update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330502004'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330502005'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330502003'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330502022'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330502010'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330502018'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330502021'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330502013'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330502014'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330502019'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330502016'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330502017'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330502009'

update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330521006'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330521008'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330521009'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330521010'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330521012'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330521011'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330521015'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330521013'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330521016'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '47122068333052112C2201'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330521007'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330521019'

update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330503008'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330503014'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330503004'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330503010'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330503005'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330503002'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330503006'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330503003'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330503013'

update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330523006'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330523010'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330523019'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330523018'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330523016'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330523020'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330523032'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330523027'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330523029'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330523012'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330523033'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330523015'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330523014'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330523022'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330523026'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330523021'

update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330522003'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330522035'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330522036'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330522031'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330522001'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330522037'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330522011'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330522004'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330522005'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330522012'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330522006'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330522007'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330522008'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330522009'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330522010'
update GY_FUWUJG set HZStatisticsType = 1 where FuWuJGBH = '330522002'

--业务过滤
--A2.听力筛查报表 HearingScreeningArea @汪沄杰 @1015
--A3.非户籍儿童与孕产妇健康状况年报表 FHJETYYCFJKZKNBBArea @张振楠  @1014
--new A10.医疗机构早产儿出生情况统计报表 @张振楠 YLJGZCECSQKTJJDBBArea@1019

update GY_FUWUJG set HZStatisticsType = 2 where FuWuJGBH = '330502001'
update GY_FUWUJG set HZStatisticsType = 2 where FuWuJGBH = '330502002'
update GY_FUWUJG set HZStatisticsType = 2 where FuWuJGBH = '330502011'
update GY_FUWUJG set HZStatisticsType = 2 where FuWuJGBH = '330502013'
update GY_FUWUJG set HZStatisticsType = 2 where FuWuJGBH = '330502015'
update GY_FUWUJG set HZStatisticsType = 2 where FuWuJGBH = '330502023'

update GY_FUWUJG set HZStatisticsType = 2 where FuWuJGBH = '330522008'
update GY_FUWUJG set HZStatisticsType = 2 where FuWuJGBH = 'MA29J0PK533052215A5182'
update GY_FUWUJG set HZStatisticsType = 2 where FuWuJGBH = '330522030'
update GY_FUWUJG set HZStatisticsType = 2 where FuWuJGBH = '330522029'
update GY_FUWUJG set HZStatisticsType = 2 where FuWuJGBH = '330522028'

