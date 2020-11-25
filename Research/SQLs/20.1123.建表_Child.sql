IF OBJECT_ID(N'Child', N'U') IS  NOT  NULL 
DROP TABLE Child;

SET QUOTED_IDENTIFIER ON
GO
-- 新增时间 设置默认值
CREATE TABLE [dbo].[Child](
[Id] [bigint] IDENTITY(1,2) NOT NULL,
PatientId [bigint] NOT NULL,
--以上为结构型字段
[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
[db_updatetime] [datetime] NULL,
[IsDeleted] tinyint NULL,		--布尔
--以下为内容型字段
[Name] varchar(20) NULL, 		--字符
[Birthday] datetime2(7)  NULL,	--日期
[Sex] tinyint NULL,				--枚举
CONSTRAINT [PK_Child] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_Child_db_updatetime]
ON [dbo].[Child]
FOR UPDATE
AS
BEGIN
 update Child set db_updatetime = CURRENT_TIMESTAMP
where id in (select id from inserted)
END
GO

--主要关系索引
CREATE NONCLUSTERED INDEX [idx_Child_Id]
ON [dbo].[Child] (
  [Id] DESC
)

CREATE NONCLUSTERED INDEX [idx_Child_PatientId]
ON [dbo].[Child] (
  PatientId DESC
)


