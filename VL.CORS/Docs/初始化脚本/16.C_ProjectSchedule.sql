IF OBJECT_ID(N'ProjectSchedule', N'U') IS  NOT  NULL 
DROP TABLE [ProjectSchedule];

SET QUOTED_IDENTIFIER ON

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
	[ResultFile] nvarchar(200) NULL,
	[Message] text COLLATE Chinese_PRC_CI_AS  NULL,
	-------------------------------------------

	CONSTRAINT[PK_ProjectSchedule] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]

-- 校验
select * from [ProjectSchedule];