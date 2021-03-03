IF OBJECT_ID(N'CustomBusinessEntityProperty', N'U') IS  NOT  NULL 
DROP TABLE [CustomBusinessEntityProperty];

SET QUOTED_IDENTIFIER ON

-- 新增时间 设置默认值
CREATE TABLE [dbo].[CustomBusinessEntityProperty](
	[Id][bigint] IDENTITY(300000000, 2) NOT NULL,
	[BusinessEntityId][bigint] NOT NULL,
	TemplateId[bigint] NOT NULL,
	TemplatePropertyId[bigint] NOT NULL,
	-------------------------------------------上关联
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
	-------------------------------------------
	[EntityName] nvarchar(50) NULL,
	[Name] nvarchar(50) NULL,
	[DisplayName] nvarchar(50) NULL,
	[ColumnType] TinyInt NULL,
	[EnumType] nvarchar(50) NULL,
	-------------------------------------------
	[CreatorBy] bigint NULL,
	[CreatedAt] datetime NULL,
	-------------------------------------------
	CONSTRAINT[PK_CustomBusinessEntityProperty] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]


-- 校验
select * from [CustomBusinessEntityProperty];