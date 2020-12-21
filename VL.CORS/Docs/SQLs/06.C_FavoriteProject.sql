IF OBJECT_ID(N'FavoriteProject', N'U') IS  NOT  NULL 
DROP TABLE [FavoriteProject];

SET QUOTED_IDENTIFIER ON
GO

-- 新增时间 设置默认值
CREATE TABLE [dbo].[FavoriteProject](
	UserId bigint not NULL,
	ProjectId bigint not NULL,
) ON[PRIMARY]
GO
GO

--设置联合主键
ALTER TABLE [dbo].[FavoriteProject] ADD PRIMARY KEY CLUSTERED ([UserId], [ProjectId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

-- 校验
select * from [FavoriteProject];

