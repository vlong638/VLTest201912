IF OBJECT_ID(N'ProjectTaskWhere', N'U') IS  NOT  NULL 
DROP TABLE [ProjectTaskWhere];

SET QUOTED_IDENTIFIER ON

-- 新增时间 设置默认值
CREATE TABLE [dbo].[ProjectTaskWhere](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	[ProjectId][bigint] NOT NULL,
	[ParentId][bigint] NULL,
	[TaskId][bigint] NOT NULL,
	[IndicatorId][bigint] NOT NULL,
	BusinessEntityId [bigint] NOT NULL,
	BusinessEntityPropertyId [bigint] NOT NULL,
	-------------------------------------------
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
	-------------------------------------------
	[EntityName] nvarchar(50) NULL,
	[PropertyName] nvarchar(50) NULL,
	[Operator] nvarchar(20) NULL,
	[Value] nvarchar(20) NULL,
	[WhereCategory][tinyint] NOT NULL,
	-------------------------------------------

	CONSTRAINT[PK_ProjectTaskWhere] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]

-- 校验
select * from [ProjectTaskWhere];

