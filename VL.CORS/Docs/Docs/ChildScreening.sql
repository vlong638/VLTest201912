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

 Date: 23/02/2021 11:55:12
*/


-- ----------------------------
-- Table structure for ChildScreening
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[ChildScreening]') AND type IN ('U'))
	DROP TABLE [dbo].[ChildScreening]
GO

CREATE TABLE [dbo].[ChildScreening] (
  [Id] bigint  IDENTITY(1,2) NOT NULL,
  [ChildId] bigint  NULL,
  [IssueTime] datetime2(0)  NULL,
  [Code] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [IodineContact] tinyint  NULL,
  [BloodDrawers] varchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [BloodRedrawStatus] tinyint  NULL,
  [BloodRedrawers] varchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [BirthCertificationDeliveryCode] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [BirthCertificationDeliveryTime] datetime2(0)  NULL
)
GO

ALTER TABLE [dbo].[ChildScreening] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'疾病筛查日期',
'SCHEMA', N'dbo',
'TABLE', N'ChildScreening',
'COLUMN', N'IssueTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'疾病筛查编号',
'SCHEMA', N'dbo',
'TABLE', N'ChildScreening',
'COLUMN', N'Code'
GO

EXEC sp_addextendedproperty
'MS_Description', N'碘接触',
'SCHEMA', N'dbo',
'TABLE', N'ChildScreening',
'COLUMN', N'IodineContact'
GO

EXEC sp_addextendedproperty
'MS_Description', N'采血人',
'SCHEMA', N'dbo',
'TABLE', N'ChildScreening',
'COLUMN', N'BloodDrawers'
GO

EXEC sp_addextendedproperty
'MS_Description', N'重采状态',
'SCHEMA', N'dbo',
'TABLE', N'ChildScreening',
'COLUMN', N'BloodRedrawStatus'
GO

EXEC sp_addextendedproperty
'MS_Description', N'重采人',
'SCHEMA', N'dbo',
'TABLE', N'ChildScreening',
'COLUMN', N'BloodRedrawers'
GO

EXEC sp_addextendedproperty
'MS_Description', N'出生证明签发编号',
'SCHEMA', N'dbo',
'TABLE', N'ChildScreening',
'COLUMN', N'BirthCertificationDeliveryCode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'出生证明签发日期',
'SCHEMA', N'dbo',
'TABLE', N'ChildScreening',
'COLUMN', N'BirthCertificationDeliveryTime'
GO


-- ----------------------------
-- Auto increment value for ChildScreening
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[ChildScreening]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table ChildScreening
-- ----------------------------
ALTER TABLE [dbo].[ChildScreening] ADD CONSTRAINT [PK__ChildScr__3214EC07CFA62F1C] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

