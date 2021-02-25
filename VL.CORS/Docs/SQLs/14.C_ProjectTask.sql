IF OBJECT_ID(N'ProjectTask', N'U') IS  NOT  NULL 
DROP TABLE [ProjectTask];

SET QUOTED_IDENTIFIER ON
GO

-- 新增时间 设置默认值
CREATE TABLE [dbo].[ProjectTask](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	[ProjectId][bigint] NOT NULL,
	IsDeleted char(1) default 0,
	-------------------------------------------
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
	-------------------------------------------
	[Name] nvarchar(50) NULL,
	-------------------------------------------

	CONSTRAINT[PK_ProjectTask] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]
GO

-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_ProjectTask_db_updatetime]
ON[dbo].[ProjectTask]
FOR UPDATE
AS
BEGIN
  update [ProjectTask] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
GO

-- 校验
select * from [ProjectTask];