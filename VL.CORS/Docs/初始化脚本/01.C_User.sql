IF OBJECT_ID(N'User', N'U') IS  NOT  NULL 
DROP TABLE [User];

SET QUOTED_IDENTIFIER ON

-- 新增时间 设置默认值
CREATE TABLE [dbo].[User](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	-------------------------------------------上关联
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
	-------------------------------------------下管控
	IsDeleted bit default 0,
	-------------------------------------------下内容
	[Name] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
	[Password] nvarchar(32) COLLATE Chinese_PRC_CI_AS  NULL,
	[NickName] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
	[Phone] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
	[Sex] tinyint  NULL,
	[CreatedAt] datetime  NULL
	-------------------------------------------
	CONSTRAINT[PK_User] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]


-- 校验
select * from [User];