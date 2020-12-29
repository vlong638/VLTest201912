IF OBJECT_ID(N'ProjectLog', N'U') IS  NOT  NULL 
DROP TABLE [ProjectLog];

SET QUOTED_IDENTIFIER ON
GO

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
GO

-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_ProjectLog_db_updatetime]
ON[dbo].[ProjectLog]
FOR UPDATE
AS
BEGIN
  update [ProjectLog] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
GO

-- 校验
select * from [ProjectLog];