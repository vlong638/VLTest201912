﻿IF OBJECT_ID(N'ProjectTask', N'U') IS  NOT  NULL 
DROP TABLE [ProjectTask];

SET QUOTED_IDENTIFIER ON

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


-- 校验
select * from [ProjectTask];