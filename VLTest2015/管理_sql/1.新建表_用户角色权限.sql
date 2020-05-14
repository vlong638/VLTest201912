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
drop table [dbo].[Role];
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
drop table [dbo].[RoleAuthority];
CREATE TABLE [dbo].[RoleAuthority](
	[Id] [bigint] IDENTITY(1,2) NOT NULL,
	[RoleId] [bigint] NOT NULL,
	[AuthorityId] [bigint] NOT NULL,
 CONSTRAINT [PK_RoleAuthority] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--�û���ɫ����
drop table [dbo].[UserRole];
CREATE TABLE [dbo].[UserRole](
	[Id] [bigint] IDENTITY(1,2) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[RoleId] [bigint] NOT NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--�û�Ȩ�޽���
drop table [dbo].[UserAuthority];
CREATE TABLE [dbo].[UserAuthority](
	[Id] [bigint] IDENTITY(1,2) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[AuthorityId] [bigint] NOT NULL,
 CONSTRAINT [PK_UserAuthority] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

--���ڶ�User.Name�Ĳ�ѯ����,���ӷǾ۴�Ψһ����
create unique nonclustered
index UQ_NonClu_User_Name
on [User]([Name])
;
--���ڶ�Role.Name�Ĳ�ѯ����,���ӷǾ۴�Ψһ����
create unique nonclustered
index UQ_NonClu_Role_Name
on [Role]([Name])
;
--���ڶ�UserAuthority.UserId�Ĳ�ѯ����,���Ӿ۴ط�Ψһ����
create clustered
index NUQ_Clu_UserAuthority_UserId
on UserAuthority(UserId)
;
--���ڶ�UserRole.UserId�Ĳ�ѯ����,���Ӿ۴ط�Ψһ����
create clustered
index NUQ_Clu_UserRole_UserId
on UserRole(UserId)
;
--���ڶ�RoleAuthority.RoleId�Ĳ�ѯ����,���Ӿ۴ط�Ψһ����
create clustered
index NUQ_Clu_RoleAuthority_RoleId
on RoleAuthority(RoleId)
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










