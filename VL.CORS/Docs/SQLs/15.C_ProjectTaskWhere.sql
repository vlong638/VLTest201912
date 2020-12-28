﻿IF OBJECT_ID(N'ProjectTaskWhere', N'U') IS  NOT  NULL 
DROP TABLE [ProjectTaskWhere];

SET QUOTED_IDENTIFIER ON
GO

-- 新增时间 设置默认值
CREATE TABLE [dbo].[ProjectTaskWhere](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	[ProjectId][bigint] NOT NULL,
	[TaskId][bigint] NOT NULL,
	BusinessEntityId [bigint] NOT NULL,
	BusinessEntityPropertyId [bigint] NOT NULL,
	-------------------------------------------
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
	-------------------------------------------
	[EntityName] nvarchar(20) NULL,
	[PropertyName] nvarchar(20) NULL,
	[Operator] nvarchar(20) NULL,
	[Value] nvarchar(20) NULL,
	-------------------------------------------

	CONSTRAINT[PK_ProjectTaskWhere] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]
GO

-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_ProjectTaskWhere_db_updatetime]
ON[dbo].[ProjectTaskWhere]
FOR UPDATE
AS
BEGIN
  update [ProjectTaskWhere] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
GO

-- 校验
select * from [ProjectTaskWhere];