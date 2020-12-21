IF OBJECT_ID(N'CustomerIndicator', N'U') IS  NOT  NULL 
DROP TABLE [CustomerIndicator];

SET QUOTED_IDENTIFIER ON
GO

-- 新增时间 设置默认值
CREATE TABLE [dbo].[CustomerIndicator](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
	-------------------------------------------
	[TemplateId] bigint NULL,
	[ColumnId] nvarchar(20) NULL,
	[NickName] nvarchar(20) NULL,
	-------------------------------------------
	[CreatorBy] bigint NULL,
	[CreatedAt] datetime NULL,
	[LastModifiedBy] bigint NULL,
	[LastModifiedAt] datetime NULL,

	CONSTRAINT[PK_CustomerIndicator] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]
GO

-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_CustomerIndicator_db_updatetime]
ON[dbo].[CustomerIndicator]
FOR UPDATE
AS
BEGIN
  update [CustomerIndicator] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
GO

-- 校验
select * from [CustomerIndicator];