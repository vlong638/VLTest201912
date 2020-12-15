IF OBJECT_ID(N'PregnantInfo', N'U') IS  NOT  NULL 
DROP TABLE PregnantInfo;

SET QUOTED_IDENTIFIER ON
GO

-- 新增时间 设置默认值
CREATE TABLE [dbo].[PregnantInfo](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	[Idcard] varchar(20) NULL, 		--字符
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
	[Name] varchar(20) NULL, 		--字符
	[deliverydate] datetime2(7)  NULL,	--日期
	[Sex] tinyint NULL,				--枚举
	CONSTRAINT[PK_PregnantInfo] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]
GO

-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_PregnantInfo_db_updatetime]
ON[dbo].[PregnantInfo]
FOR UPDATE
AS
BEGIN
  update [PregnantInfo] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
GO

-- 校验
select * from [PregnantInfo];