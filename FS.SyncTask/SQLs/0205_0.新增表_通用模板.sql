
/****** Object:  Table [dbo].SyncForFS    Script Date: 2020/2/5 20:26:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ����ʱ�� ����Ĭ��ֵ
CREATE TABLE [dbo].SyncForFS(
	[Id] [bigint] IDENTITY(1,2) NOT NULL,
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
    [SourceType] smallint  NULL,
    [SourceId] varchar(32) COLLATE Chinese_PRC_CI_AS  NULL,
    [SyncTime] datetime  NULL,
    [ErrorMessage] varchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
    [HasError] smallint  NULL,
 CONSTRAINT [PK_fm_ChildSafeHandover] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


-- ���� ����
INSERT INTO [dbo].SyncForFS([db_updatetime])
VALUES (null);

-- ����ʱ�� ���ô�����
CREATE TRIGGER [dbo].[trigger_SyncForFS_db_updatetime]
ON [dbo].SyncForFS
FOR UPDATE
AS
BEGIN
  update SyncForFS set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
GO

-- ���� ����
update SyncForFS set db_createtime = CURRENT_TIMESTAMP where Id = 1;

-- У��
select * from SyncForFS;


-- ����
CREATE NONCLUSTERED INDEX [SyncForFS_SourceType_SourceId]
ON [dbo].[SyncForFS] (
  [SourceType] ASC,
  [SourceId] ASC
)
