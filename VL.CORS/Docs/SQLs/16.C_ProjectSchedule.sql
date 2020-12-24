IF OBJECT_ID(N'ProjectSchedule', N'U') IS  NOT  NULL 
DROP TABLE [ProjectSchedule];

SET QUOTED_IDENTIFIER ON
GO

-- 新增时间 设置默认值
CREATE TABLE [dbo].[ProjectSchedule](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	[ProjectId][bigint] NOT NULL,
	[TaskId][bigint] NOT NULL,
	-------------------------------------------
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
	-------------------------------------------
	[StartedAt] Datetime NULL,
	[Status] nvarchar(20) NULL,
	[LastCompletedAt] Datetime NULL,
	[ResultFile] nvarchar(20) NULL,
	-------------------------------------------

	CONSTRAINT[PK_ProjectSchedule] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]
GO

-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_ProjectSchedule_db_updatetime]
ON[dbo].[ProjectSchedule]
FOR UPDATE
AS
BEGIN
  update [ProjectSchedule] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
GO

-- 校验
select * from [ProjectSchedule];