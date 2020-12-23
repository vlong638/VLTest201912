IF OBJECT_ID(N'CustomBusinessEntityWhere', N'U') IS  NOT  NULL 
DROP TABLE [CustomBusinessEntityWhere];

SET QUOTED_IDENTIFIER ON
GO

-- 新增时间 设置默认值
CREATE TABLE [dbo].[CustomBusinessEntityWhere](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	[TemplateId] bigint NULL,
	-------------------------------------------上关联
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
	-------------------------------------------下管控
	-------------------------------------------下内容
	[ComponentName] nvarchar(20) NULL,
	[Operator] nvarchar(20) NULL,
	[Value] nvarchar(20) NULL,
	-------------------------------------------

	CONSTRAINT[PK_CustomBusinessEntityWhere] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]
GO

-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_CustomBusinessEntityWhere_db_updatetime]
ON[dbo].[CustomBusinessEntityWhere]
FOR UPDATE
AS
BEGIN
  update [CustomBusinessEntityWhere] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
GO

-- 校验
select * from [CustomBusinessEntityWhere];