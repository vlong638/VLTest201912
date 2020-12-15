IF OBJECT_ID(N'User', N'U') IS  NOT  NULL 
DROP TABLE [User];

SET QUOTED_IDENTIFIER ON
GO

-- 新增时间 设置默认值
CREATE TABLE [dbo].[User](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
	IsDeleted char(1) default 0,
	-------------------------------------------
	[Name] nvarchar(20) NULL,
	[Password] nvarchar(32) NULL,
	[NickName] nvarchar(20) NULL,
	-------------------------------------------
	CONSTRAINT[PK_User] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]
GO

-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_User_db_updatetime]
ON[dbo].[User]
FOR UPDATE
AS
BEGIN
  update [User] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
GO

-- 校验
select * from [User];