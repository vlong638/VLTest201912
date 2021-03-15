IF OBJECT_ID(N'ProjectIndicator', N'U') IS  NOT  NULL 
DROP TABLE [ProjectIndicator];

SET QUOTED_IDENTIFIER ON

-- 新增时间 设置默认值
CREATE TABLE [dbo].[ProjectIndicator](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	[ProjectId][bigint] NOT NULL,
	[BusinessEntityId][bigint] NOT NULL,
	[BusinessEntityPropertyId][bigint] NOT NULL,
	[TemplateId][bigint] NOT NULL,
	[TemplatePropertyId][bigint] NOT NULL,
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
	-------------------------------------------
	EntitySourceName nvarchar(50) NULL,
	PropertySourceName nvarchar(50) NULL,
	PropertyDisplayName nvarchar(50) NULL,
	-------------------------------------------

	CONSTRAINT[PK_ProjectIndicator] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]

-- 校验
select * from [ProjectIndicator];