IF OBJECT_ID(N'BusinessType', N'U') IS  NOT  NULL 
DROP TABLE [BusinessType];

SET QUOTED_IDENTIFIER ON

-- 新增时间 设置默认值
CREATE TABLE [dbo].[BusinessType](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	-------------------------------------------上关联
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
	-------------------------------------------下管控
	-------------------------------------------下内容
	[Name] nvarchar(20) NULL,
	-------------------------------------------

	CONSTRAINT[PK_BusinessType] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]


-- 校验
select * from [BusinessType];