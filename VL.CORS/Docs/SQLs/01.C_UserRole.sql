IF OBJECT_ID(N'UserRole', N'U') IS  NOT  NULL 
DROP TABLE [UserRole];

SET QUOTED_IDENTIFIER ON
GO

-- 新增时间 设置默认值
CREATE TABLE [dbo].[UserRole](
	UserId bigint not NULL,
	RoleId bigint not NULL,
) ON[PRIMARY]
GO
GO

--设置联合主键
ALTER TABLE [dbo].[UserRole] ADD PRIMARY KEY CLUSTERED (UserId,RoleId)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

-- 校验
select * from [UserRole];