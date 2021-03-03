﻿
-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_ProjectLog_db_updatetime]
ON[dbo].[ProjectLog]
FOR UPDATE
AS
BEGIN
  update [ProjectLog] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END