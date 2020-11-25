IF OBJECT_ID(N'PatientCheckItem', N'U') IS  NOT  NULL 
DROP TABLE PatientCheckItem;

SET QUOTED_IDENTIFIER ON
GO
-- 新增时间 设置默认值
CREATE TABLE [dbo].[PatientCheckItem](
[Id] [bigint] IDENTITY(1,2) NOT NULL,
[OrderId] [bigint] NOT NULL,
--以上为结构型字段
[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
[db_updatetime] [datetime] NULL,
[IsDeleted] tinyint NULL,		--布尔
--以下为内容型字段
[Name] varchar(20) NULL, 		--字符
[Value] varchar(20) NULL, 		--字符
CONSTRAINT [PK_PatientCheckItem] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_PatientCheckItem_db_updatetime]
ON [dbo].[PatientCheckItem]
FOR UPDATE
AS
BEGIN
 update PatientCheckItem set db_updatetime = CURRENT_TIMESTAMP
where id in (select id from inserted)
END
GO

--主要关系索引
CREATE NONCLUSTERED INDEX [idx_PatientCheckItem_Id]
ON [dbo].[PatientCheckItem] (
  [Id] DESC
)

CREATE NONCLUSTERED INDEX [idx_PatientCheckItem_OrderId]
ON [dbo].[PatientCheckItem] (
  [OrderId] DESC
)


