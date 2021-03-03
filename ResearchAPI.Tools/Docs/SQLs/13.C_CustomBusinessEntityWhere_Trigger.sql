
-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_CustomBusinessEntityWhere_db_updatetime]
ON[dbo].[CustomBusinessEntityWhere]
FOR UPDATE
AS
BEGIN
  update [CustomBusinessEntityWhere] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
