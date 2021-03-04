/*
 Navicat Premium Data Transfer

 Source Server         : 192.168.50.2 MSSQL
 Source Server Type    : SQL Server
 Source Server Version : 11003000
 Source Host           : 192.168.50.2:1433
 Source Catalog        : HL_Pregnant
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 11003000
 File Encoding         : 65001

 Date: 04/03/2021 16:31:17
*/


-- ----------------------------
-- Table structure for StructureV2
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[StructureV2]') AND type IN ('U'))
	DROP TABLE [dbo].[StructureV2]
GO

CREATE TABLE [dbo].[StructureV2] (
  [TableName] nvarchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [ColumnName] nvarchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [ControlType] nvarchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [IsEnumText] nvarchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [Enum] nvarchar(255) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[StructureV2] SET (LOCK_ESCALATION = TABLE)
GO

