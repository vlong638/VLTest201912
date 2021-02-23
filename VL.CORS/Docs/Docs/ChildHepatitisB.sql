/*
 Navicat Premium Data Transfer

 Source Server         : 192.168.50.2 MSSQL
 Source Server Type    : SQL Server
 Source Server Version : 11003000
 Source Host           : 192.168.50.2:1433
 Source Catalog        : fmpt
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 11003000
 File Encoding         : 65001

 Date: 22/02/2021 15:10:28
*/


-- ----------------------------
-- Table structure for ChildHepatitisB
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[ChildHepatitisB]') AND type IN ('U'))
	DROP TABLE [dbo].[ChildHepatitisB]
GO

CREATE TABLE [dbo].[ChildHepatitisB] (
  [Id] bigint  IDENTITY(1,2) NOT NULL,
  [ChildId] bigint  NULL,
  [InoculatedStatus] tinyint  NULL,
  [Dosage] tinyint  NULL,
  [BatchCode] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [InoculateTime] datetime2(0)  NULL,
  [Code] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Amount] tinyint  NULL,
  [ChargeStatus] tinyint  NULL,
  [Manufacturer] tinyint  NULL,
  [InoculationSite] tinyint  NULL,
  [InoculationMethod] tinyint  NULL,
  [InoculateOperators] varchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [UninoculatedReason] varchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [UninoculatedReason2] varchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [HBIGInoculatedStatus] tinyint  NULL,
  [HBIGDosage] tinyint  NULL,
  [HBIGChargeStatus] tinyint  NULL
)
GO

ALTER TABLE [dbo].[ChildHepatitisB] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'接种情况',
'SCHEMA', N'dbo',
'TABLE', N'ChildHepatitisB',
'COLUMN', N'InoculatedStatus'
GO

EXEC sp_addextendedproperty
'MS_Description', N'剂量',
'SCHEMA', N'dbo',
'TABLE', N'ChildHepatitisB',
'COLUMN', N'Dosage'
GO

EXEC sp_addextendedproperty
'MS_Description', N'疫苗批号',
'SCHEMA', N'dbo',
'TABLE', N'ChildHepatitisB',
'COLUMN', N'BatchCode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'接种时间',
'SCHEMA', N'dbo',
'TABLE', N'ChildHepatitisB',
'COLUMN', N'InoculateTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'疫苗编码',
'SCHEMA', N'dbo',
'TABLE', N'ChildHepatitisB',
'COLUMN', N'Code'
GO

EXEC sp_addextendedproperty
'MS_Description', N'接种剂次',
'SCHEMA', N'dbo',
'TABLE', N'ChildHepatitisB',
'COLUMN', N'Amount'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否免费',
'SCHEMA', N'dbo',
'TABLE', N'ChildHepatitisB',
'COLUMN', N'ChargeStatus'
GO

EXEC sp_addextendedproperty
'MS_Description', N'生产厂家',
'SCHEMA', N'dbo',
'TABLE', N'ChildHepatitisB',
'COLUMN', N'Manufacturer'
GO

EXEC sp_addextendedproperty
'MS_Description', N'接种部位',
'SCHEMA', N'dbo',
'TABLE', N'ChildHepatitisB',
'COLUMN', N'InoculationSite'
GO

EXEC sp_addextendedproperty
'MS_Description', N'接种途径',
'SCHEMA', N'dbo',
'TABLE', N'ChildHepatitisB',
'COLUMN', N'InoculationMethod'
GO

EXEC sp_addextendedproperty
'MS_Description', N'接种者',
'SCHEMA', N'dbo',
'TABLE', N'ChildHepatitisB',
'COLUMN', N'InoculateOperators'
GO

EXEC sp_addextendedproperty
'MS_Description', N'未接种原因',
'SCHEMA', N'dbo',
'TABLE', N'ChildHepatitisB',
'COLUMN', N'UninoculatedReason'
GO

EXEC sp_addextendedproperty
'MS_Description', N'未接种其他原因',
'SCHEMA', N'dbo',
'TABLE', N'ChildHepatitisB',
'COLUMN', N'UninoculatedReason2'
GO

EXEC sp_addextendedproperty
'MS_Description', N'乙肝免疫球蛋白 接种情况',
'SCHEMA', N'dbo',
'TABLE', N'ChildHepatitisB',
'COLUMN', N'HBIGInoculatedStatus'
GO

EXEC sp_addextendedproperty
'MS_Description', N'乙肝免疫球蛋白 剂量',
'SCHEMA', N'dbo',
'TABLE', N'ChildHepatitisB',
'COLUMN', N'HBIGDosage'
GO

EXEC sp_addextendedproperty
'MS_Description', N'乙肝免疫球蛋白 是否免费',
'SCHEMA', N'dbo',
'TABLE', N'ChildHepatitisB',
'COLUMN', N'HBIGChargeStatus'
GO


-- ----------------------------
-- Auto increment value for ChildHepatitisB
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[ChildHepatitisB]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table ChildHepatitisB
-- ----------------------------
ALTER TABLE [dbo].[ChildHepatitisB] ADD CONSTRAINT [PK__ChildHep__3214EC072201D597] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

