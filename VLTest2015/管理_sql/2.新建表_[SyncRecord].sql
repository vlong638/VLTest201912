/*
 Navicat SQL Server Data Transfer

 Source Server         : local
 Source Server Type    : SQL Server
 Source Server Version : 11002100
 Source Host           : LAPTOP-NQBU1OIS\SQLEXPRESS:1433
 Source Catalog        : VLTest
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 11002100
 File Encoding         : 65001

 Date: 03/06/2020 13:51:40
*/


-- ----------------------------
-- Table structure for SyncRecord
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[SyncRecord]') AND type IN ('U'))
	DROP TABLE [dbo].[SyncRecord]
GO

CREATE TABLE [dbo].[SyncRecord] (
  [Id] bigint  IDENTITY(1,2) NOT NULL,
  [db_createtime] datetime DEFAULT (getdate()) NULL,
  [db_updatetime] datetime  NULL,
  [SourceName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [TableName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [FieldName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Value] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [IncreaseValue] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [IncreaseType] nvarchar(10) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[SyncRecord] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Triggers structure for table SyncRecord
-- ----------------------------
CREATE TRIGGER [dbo].[trigger_SyncRecord_db_updatetime]
ON [dbo].[SyncRecord]
WITH EXECUTE AS CALLER
FOR UPDATE
AS
BEGIN
  update SyncRecord set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
GO


-- ----------------------------
-- Primary Key structure for table SyncRecord
-- ----------------------------
ALTER TABLE [dbo].[SyncRecord] ADD CONSTRAINT [PK_SyncRecord] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

