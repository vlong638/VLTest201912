use VLTest;

--�û�����
drop table [dbo].[User];
CREATE TABLE [dbo].[User](
	[Id] [bigint] IDENTITY(1,2) NOT NULL,
	[Password] [varchar](32) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	--[db_CreatedAt] [datetime] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--��ɫ����
CREATE TABLE [dbo].[Role](
	[Id] [bigint] IDENTITY(1,2) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--��ɫȨ�޽���
CREATE TABLE [dbo].[RoleAuthority](
	[RoleId] [bigint] NOT NULL,
	[AuthorityId] [bigint] NOT NULL,
 CONSTRAINT [PK_RoleAuthority] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--�û���ɫ����
CREATE TABLE [dbo].[UserRole](
	[UserId] [bigint] NOT NULL,
	[RoleId] [bigint] NOT NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--�û�Ȩ�޽���
CREATE TABLE [dbo].[UserAuthority](
	[UserId] [bigint] NOT NULL,
	[AuthorityId] [bigint] NOT NULL,
 CONSTRAINT [PK_UserAuthority] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--���ڶ�User.Name�Ĳ�ѯ����,���ӷǾ۴�Ψһ����
create unique nonclustered
index UQ_NonClu_User_Name
on [User]([Name])
;
--���ڶ�Role.Name�Ĳ�ѯ����,���ӷǾ۴�Ψһ����
create unique nonclustered        --��ʾ����Ψһ�ۼ�����
index UQ_NonClu_Role_Name      --��������
on [Role]([Name])        --���ݱ����ƣ�����������������
;

use VLTest;
select * from [User];
select * from [Role];
select * from [RoleAuthority];
select * from [UserRole];
select * from [UserAuthority];

--�û��б�,sqlserver��ҳ
select * from [User]
order by Id offset 4 rows fetch next 5 rows only;

--�û��б�,����UserIds�����û���ɫ
select ur.UserId,r.Name as RoleName from [Role] r
left join [UserRole] ur on ur.RoleId = r.Id
where ur.UserId in (1,3);

--�û��б�����
select Id,Name from [User]
where  Name like '%1%' 
order by Id offset 0 rows fetch next 3 rows only 










