﻿
-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_User_db_updatetime]
ON[dbo].[User]
FOR UPDATE
AS
BEGIN
  update [User] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END