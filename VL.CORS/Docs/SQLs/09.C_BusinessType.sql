IF OBJECT_ID(N'BusinessType', N'U') IS  NOT  NULL 
DROP TABLE [BusinessType];

SET QUOTED_IDENTIFIER ON
GO

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
GO

-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_BusinessType_db_updatetime]
ON[dbo].[BusinessType]
FOR UPDATE
AS
BEGIN
  update [BusinessType] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
GO

-- 校验
select * from [BusinessType];