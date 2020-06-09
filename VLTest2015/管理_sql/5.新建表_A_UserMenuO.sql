/*
 Navicat SQL Server Data Transfer

 Source Server         : local
 Source Server Type    : SQL Server
 Source Server Version : 11002100
 Source Host           : LAPTOP-NQBU1OIS\SQLEXPRESS:1433
 Source Catalog        : VLTest
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 11002100
 File Encoding         : 65001

 Date: 08/06/2020 17:12:42
*/


-- ----------------------------
-- Table structure for A_UserMenu
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[A_UserMenu]') AND type IN ('U'))
	DROP TABLE [dbo].[A_UserMenu]
GO

CREATE TABLE [dbo].[A_UserMenu] (
  [Id] bigint  IDENTITY(1,2) NOT NULL,
  [UserId] bigint  NULL,
  [MenuName] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [URL] varchar(255) COLLATE Chinese_PRC_CI_AS  NULL,
  [EntityAppConfig] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[A_UserMenu] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Primary Key structure for table A_UserMenu
-- ----------------------------
ALTER TABLE [dbo].[A_UserMenu] ADD CONSTRAINT [PK_User_copy1] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

