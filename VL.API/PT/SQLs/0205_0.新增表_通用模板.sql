USE [fmpt]
GO

/****** Object:  Table [dbo].[FM_ChildSafeHandover]    Script Date: 2020/2/5 20:26:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ����ʱ�� ����Ĭ��ֵ
CREATE TABLE [dbo].[FM_ChildSafeHandover](
	[Id] [bigint] IDENTITY(1,2) NOT NULL,
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
	[BirthTime] datetime NULL,
	[Sex] tinyint NULL,
	[Weight] int NULL,
	[Remark] nvarchar(50) NULL,
 CONSTRAINT [PK_FM_ChildSafeHandover] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


-- ���� ����
INSERT INTO [dbo].[FM_ChildSafeHandover]([db_updatetime])
VALUES (null);

-- ����ʱ�� ���ô�����
CREATE TRIGGER [dbo].[trigger_FM_ChildSafeHandover_db_updatetime]
ON [dbo].[FM_ChildSafeHandover]
FOR UPDATE
AS
BEGIN
  update FM_ChildSafeHandover set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
GO

-- ���� ����
update FM_ChildSafeHandover set db_createtime = CURRENT_TIMESTAMP where Id = 1;

-- У��
select * from FM_ChildSafeHandover;