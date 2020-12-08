IF OBJECT_ID(N'LabResult', N'U') IS  NOT  NULL 
DROP TABLE [LabResult];


SET QUOTED_IDENTIFIER ON
GO

-- 新增时间 设置默认值
CREATE TABLE [dbo].[LabResult](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	[OrderId] [bigint] NOT NULL,
	[Idcard] varchar(20) NULL, 		--字符
--以上为结构型字段
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
--以下为内容型字段
	[Name] varchar(20) NULL, 		--字符
	[Value] varchar(20) NULL, 		--字符
	CONSTRAINT[PK_LabResult] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]
GO

-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_LabResult_db_updatetime]
ON[dbo].[LabResult]
FOR UPDATE
AS
BEGIN
  update [LabResult] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
GO

-- 校验
select * from [LabResult];