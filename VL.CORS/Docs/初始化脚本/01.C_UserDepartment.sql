IF OBJECT_ID(N'UserDepartment', N'U') IS  NOT  NULL 
DROP TABLE [UserDepartment];

SET QUOTED_IDENTIFIER ON

-- 新增时间 设置默认值
CREATE TABLE [dbo].[UserDepartment](
	UserId bigint not NULL,
	DepartmentId bigint not NULL,
) ON[PRIMARY]

--设置联合主键
ALTER TABLE [dbo].[UserDepartment] ADD PRIMARY KEY CLUSTERED (UserId,DepartmentId)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

-- 校验
select * from [UserDepartment];