IF OBJECT_ID(N'BusinessEntity', N'U') IS  NOT  NULL 
DROP TABLE [BusinessEntity];

SET QUOTED_IDENTIFIER ON
GO

-- 新增时间 设置默认值
CREATE TABLE [dbo].[BusinessEntity](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	-------------------------------------------上关联
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
	-------------------------------------------下管控
	-------------------------------------------下内容
	[DisplayName] nvarchar(20) NULL,
	-------------------------------------------

	CONSTRAINT[PK_BusinessEntity] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]
GO

-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_BusinessEntity_db_updatetime]
ON[dbo].[BusinessEntity]
FOR UPDATE
AS
BEGIN
  update [BusinessEntity] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
GO

-- 校验
select * from [BusinessEntity];