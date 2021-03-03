IF OBJECT_ID(N'ProjectLog', N'U') IS  NOT  NULL 
DROP TABLE [ProjectLog];

SET QUOTED_IDENTIFIER ON

-- 新增时间 设置默认值
CREATE TABLE [dbo].[ProjectLog](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	[ProjectId][bigint] NOT NULL,
	[OperatorId][bigint] NOT NULL,
	-------------------------------------------上关联
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
	-------------------------------------------下管控
	-------------------------------------------下内容
	[CreatedAt] Datetime NULL,
	[ActionType] tinyint NULL,
	[Text] text NULL,
	-------------------------------------------
	CONSTRAINT[PK_ProjectLog] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]


-- 校验
select * from [ProjectLog];