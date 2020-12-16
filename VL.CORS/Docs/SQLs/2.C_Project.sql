IF OBJECT_ID(N'Project', N'U') IS  NOT  NULL 
DROP TABLE [Project];

SET QUOTED_IDENTIFIER ON
GO

-- 新增时间 设置默认值
CREATE TABLE [dbo].[Project](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
	IsDeleted char(1) default 0,
	-------------------------------------------
	[Name] nvarchar(20) NULL,
	[DepartmentId] bigint NULL,
	[ViewAuthorizeType] tinyint NULL,
	[ProjectDescription] nvarchar(20) NULL,
	-------------------------------------------
	[CreatorId] bigint NULL,
	[CreatedAt] datetime NULL,
	[LastModifiedAt] datetime NULL,

	CONSTRAINT[PK_Project] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]
GO

-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_Project_db_updatetime]
ON[dbo].[Project]
FOR UPDATE
AS
BEGIN
  update [Project] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
GO

-- 校验
select * from [Project];