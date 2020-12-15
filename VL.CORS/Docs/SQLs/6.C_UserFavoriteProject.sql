IF OBJECT_ID(N'UserFavoriteProject', N'U') IS  NOT  NULL 
DROP TABLE [UserFavoriteProject];

SET QUOTED_IDENTIFIER ON
GO

-- 新增时间 设置默认值
CREATE TABLE [dbo].[UserFavoriteProject](
	UserId bigint not NULL,
	ProjectId bigint not NULL,
) ON[PRIMARY]
GO
GO

-- 校验
select * from [UserFavoriteProject];