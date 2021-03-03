﻿
-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_CustomBusinessEntity_db_updatetime]
ON[dbo].[CustomBusinessEntity]
FOR UPDATE
AS
BEGIN
  update [CustomBusinessEntity] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
