IF OBJECT_ID(N'ProjectIndicator', N'U') IS  NOT  NULL 
DROP TABLE [ProjectIndicator];

SET QUOTED_IDENTIFIER ON
GO

-- 新增时间 设置默认值
CREATE TABLE [dbo].[ProjectIndicator](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	[ProjectId][bigint] NOT NULL,
	[BusinessEntityId][bigint] NOT NULL,
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
	-------------------------------------------
	[SourceName] nvarchar(20) NULL,
	[ColumnName] nvarchar(20) NULL,
	[DisplayName] nvarchar(20) NULL,
	[ColumnNickName] nvarchar(20) NULL,
	-------------------------------------------

	CONSTRAINT[PK_ProjectIndicator] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]
GO

-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_ProjectIndicator_db_updatetime]
ON[dbo].[ProjectIndicator]
FOR UPDATE
AS
BEGIN
  update [ProjectIndicator] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
GO

-- 校验
select * from [ProjectIndicator];