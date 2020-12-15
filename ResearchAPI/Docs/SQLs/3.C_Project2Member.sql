IF OBJECT_ID(N'Project2Member', N'U') IS  NOT  NULL 
DROP TABLE [Project2Member];

SET QUOTED_IDENTIFIER ON
GO

-- 新增时间 设置默认值
CREATE TABLE [dbo].[Project2Member](	
	[ProjectId] bigint not NULL,
	[UserId] bigint not NULL,
	[RoleId] bigint not null,
)
GO

-- 校验
select * from [Project2Member];