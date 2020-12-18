IF OBJECT_ID(N'BusinessEntityProperty', N'U') IS  NOT  NULL 
DROP TABLE [BusinessEntityProperty];

SET QUOTED_IDENTIFIER ON
GO

-- 新增时间 设置默认值
CREATE TABLE [dbo].[BusinessEntityProperty](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	[BusinessEntityId][bigint] NOT NULL,
	-------------------------------------------上关联
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
	-------------------------------------------下管控
	[TableName] nvarchar(20) NULL,
	[ColumnName] nvarchar(20) NULL,
	-------------------------------------------下内容
	[DisplayName] nvarchar(20) NULL,
	-------------------------------------------

	CONSTRAINT[PK_BusinessEntityProperty] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]
GO

-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_BusinessEntityProperty_db_updatetime]
ON[dbo].[BusinessEntityProperty]
FOR UPDATE
AS
BEGIN
  update [BusinessEntityProperty] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
GO

-- 校验
select * from [BusinessEntityProperty];