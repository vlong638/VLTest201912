IF OBJECT_ID(N'DataStatistics', N'U') IS  NOT  NULL 
DROP TABLE [DataStatistics];

SET QUOTED_IDENTIFIER ON

-- 新增时间 设置默认值
CREATE TABLE [dbo].[DataStatistics](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
	-------------------------------------------
	[Name] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
	[IssueTime] datetime  NULL,
	[Category] tinyint  NULL,
	[Value] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
	[Parent] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
	[Message] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,

	CONSTRAINT[PK_DataStatistics] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]


-- 校验
select * from [DataStatistics];
