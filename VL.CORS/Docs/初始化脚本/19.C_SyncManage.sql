
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[SyncManage]') AND type IN ('U'))
	DROP TABLE [dbo].[SyncManage]


CREATE TABLE [dbo].[SyncManage] (
  [Id] bigint  IDENTITY(1,2) NOT NULL,
  [From] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [To] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [OperateType] tinyint  NULL,
  [Message] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [LatestDataTime] datetime2(7)  NULL,
  [LatestDataField] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [IsssueTime] datetime2(7)  NULL,
)


ALTER TABLE [dbo].[SyncManage] SET (LOCK_ESCALATION = TABLE)



-- ----------------------------
-- Records of SyncManage
-- ----------------------------
SET IDENTITY_INSERT [dbo].[SyncManage] ON


SET IDENTITY_INSERT [dbo].[SyncManage] OFF



-- ----------------------------
-- Auto increment value for SyncManage
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[SyncManage]', RESEED, 1)



-- ----------------------------
-- Primary Key structure for table SyncManage
-- ----------------------------
ALTER TABLE [dbo].[SyncManage] ADD CONSTRAINT [PK__SyncMana__3214EC07B674C275] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]


