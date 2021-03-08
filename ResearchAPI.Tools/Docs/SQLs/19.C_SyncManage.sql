IF OBJECT_ID(N'SyncManage', N'U') IS  NOT  NULL 
DROP TABLE [SyncManage];

SET QUOTED_IDENTIFIER ON

-- 新增时间 设置默认值
CREATE TABLE [dbo].[SyncManage](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[From] [varchar](100) NULL, -- 来源表
	[To] [varchar](100) NULL, -- 目标表
	[OperateType] [tinyint] NULL, -- 操作类型
	[OperateStatus] [tinyint] NULL, -- 操作状态
	[IssueTime] [datetime] NULL, --同步时间
	[LatestDataTime] [datetime] NULL, --同步时间
	[LatestDataField] [varchar](100) NULL, --同步信息记录
	[Message][varchar](MAX) NULL, -- 目标表
	CONSTRAINT[PK_SyncManage] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]

-- 校验
select * from [SyncManage];

