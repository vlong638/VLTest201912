﻿
-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_BusinessEntity_db_updatetime]
ON[dbo].[BusinessEntity]
FOR UPDATE
AS
BEGIN
  update [BusinessEntity] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END