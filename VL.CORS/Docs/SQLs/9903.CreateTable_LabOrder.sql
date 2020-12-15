IF OBJECT_ID(N'LabOrder', N'U') IS  NOT  NULL 
DROP TABLE LabOrder;


SET QUOTED_IDENTIFIER ON
GO

-- 新增时间 设置默认值
CREATE TABLE [dbo].[LabOrder](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	[PatientId] [bigint] NOT NULL,
--以上为结构型字段
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
--以下为内容型字段
	[IssueDate] datetime2(7)  NULL,	--日期
	[Name] varchar(20) NULL, 		--字符
	CONSTRAINT[PK_LabOrder] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]
GO

-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_LabOrder_db_updatetime]
ON[dbo].[LabOrder]
FOR UPDATE
AS
BEGIN
  update [LabOrder] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
GO

-- 校验
select * from [LabOrder];