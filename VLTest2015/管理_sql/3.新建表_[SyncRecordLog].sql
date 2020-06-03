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

 Date: 03/06/2020 13:51:49
*/


-- ----------------------------
-- Table structure for SyncRecordLog
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[SyncRecordLog]') AND type IN ('U'))
	DROP TABLE [dbo].[SyncRecordLog]
GO

CREATE TABLE [dbo].[SyncRecordLog] (
  [Id] bigint  IDENTITY(1,2) NOT NULL,
  [db_createtime] datetime DEFAULT (getdate()) NULL,
  [db_updatetime] datetime  NULL,
  [SyncRecordId] bigint  NULL,
  [FromValue] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [ToValue] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Count] int  NULL
)
GO

ALTER TABLE [dbo].[SyncRecordLog] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Triggers structure for table SyncRecordLog
-- ----------------------------
CREATE TRIGGER [dbo].[trigger_SyncRecordLog_db_updatetime]
ON [dbo].[SyncRecordLog]
WITH EXECUTE AS CALLER
FOR UPDATE
AS
BEGIN
  update SyncRecordLog set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
GO


-- ----------------------------
-- Primary Key structure for table SyncRecordLog
-- ----------------------------
ALTER TABLE [dbo].[SyncRecordLog] ADD CONSTRAINT [PK_SyncRecordLog] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

