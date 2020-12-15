IF OBJECT_ID(N'ProjectMember', N'U') IS  NOT  NULL 
DROP TABLE [ProjectMember];

SET QUOTED_IDENTIFIER ON
GO

-- 新增时间 设置默认值
CREATE TABLE [dbo].[ProjectMember](	
	[ProjectId] bigint not NULL,
	[UserId] bigint not NULL,
	[RoleId] bigint not null,
)
GO

-- 校验
select * from [ProjectMember];