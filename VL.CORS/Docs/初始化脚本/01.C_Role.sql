IF OBJECT_ID(N'Role', N'U') IS  NOT  NULL 
DROP TABLE [Role];

SET QUOTED_IDENTIFIER ON

-- 新增时间 设置默认值
CREATE TABLE [dbo].[Role](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
	[Name] nvarchar(20) NULL,
	[Category] smallint NULL,
	CONSTRAINT[PK_Role] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]

-- 校验
select * from [Role];

