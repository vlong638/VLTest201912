﻿IF OBJECT_ID(N'ProjectDepartment', N'U') IS  NOT  NULL 
DROP TABLE [ProjectDepartment];

SET QUOTED_IDENTIFIER ON
GO

-- 新增时间 设置默认值
CREATE TABLE [dbo].[ProjectDepartment](	
	[ProjectId] bigint not NULL,
	[DepartmentId] bigint not NULL
)
GO

-- 校验
select * from [ProjectDepartment];