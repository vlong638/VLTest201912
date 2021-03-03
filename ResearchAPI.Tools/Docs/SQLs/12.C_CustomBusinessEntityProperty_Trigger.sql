
-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_CustomBusinessEntityProperty_db_updatetime]
ON[dbo].[CustomBusinessEntityProperty]
FOR UPDATE
AS
BEGIN
  update [CustomBusinessEntityProperty] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END