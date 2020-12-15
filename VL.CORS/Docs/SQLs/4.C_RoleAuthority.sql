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

-- 校验
select * from [RoleAuthority];