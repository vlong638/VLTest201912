IF OBJECT_ID(N'RoleAuthority', N'U') IS  NOT  NULL 
DROP TABLE [RoleAuthority];

SET QUOTED_IDENTIFIER ON
GO

-- 新增时间 设置默认值
CREATE TABLE [dbo].[RoleAuthority](
	RoleId bigint not NULL,
	AuthorityId bigint not NULL,
) ON[PRIMARY]
GO
GO

--设置联合主键
ALTER TABLE [dbo].[RoleAuthority] ADD PRIMARY KEY CLUSTERED (RoleId, AuthorityId)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

-- 校验
select * from [RoleAuthority];