IF OBJECT_ID(N'CustomBusinessEntityProperty', N'U') IS  NOT  NULL 
DROP TABLE [CustomBusinessEntityProperty];

SET QUOTED_IDENTIFIER ON
GO

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
GO

-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_CustomBusinessEntityProperty_db_updatetime]
ON[dbo].[CustomBusinessEntityProperty]
FOR UPDATE
AS
BEGIN
  update [CustomBusinessEntityProperty] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
GO

-- 校验
select * from [CustomBusinessEntityProperty];