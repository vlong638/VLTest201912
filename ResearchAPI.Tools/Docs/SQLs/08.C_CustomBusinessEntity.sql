IF OBJECT_ID(N'CustomBusinessEntity', N'U') IS  NOT  NULL 
DROP TABLE [CustomBusinessEntity];

SET QUOTED_IDENTIFIER ON

-- 新增时间 设置默认值
CREATE TABLE [dbo].[CustomBusinessEntity](
	[Id][bigint] IDENTITY(300000, 2) NOT NULL,
	[TemplateId] bigint NULL,
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
	-------------------------------------------
	[Name] nvarchar(20) NULL,
	[DisplayName] nvarchar(20) NULL,
	-------------------------------------------
	[CreatorBy] bigint NULL,
	[CreatedAt] datetime NULL,
	CONSTRAINT[PK_CustomBusinessEntity] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]

-- 校验
select * from [CustomBusinessEntity];