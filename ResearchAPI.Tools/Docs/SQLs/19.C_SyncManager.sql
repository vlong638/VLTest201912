IF OBJECT_ID(N'SyncManager', N'U') IS  NOT  NULL 
DROP TABLE [SyncManager];

SET QUOTED_IDENTIFIER ON

-- 新增时间 设置默认值
CREATE TABLE [dbo].[SyncManager](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[SourceTable] [varchar](32) NULL, -- 来源表
	[TargetTable] [varchar](32) NULL, -- 目标表
	[SyncTime] [datetime] NULL, --同步时间
	[ErrorMessage] [varchar](max) NULL, --同步信息记录
	[SyncStatus] [smallint] NULL,--同步状态
	[TargetType] [smallint] NULL,
	[OperateType] [smallint] NULL
	CONSTRAINT[PK_SyncManager] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]

-- 校验
select * from [SyncManager];

