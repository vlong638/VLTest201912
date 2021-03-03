
-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_ProjectIndicator_db_updatetime]
ON[dbo].[ProjectIndicator]
FOR UPDATE
AS
BEGIN
  update [ProjectIndicator] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
