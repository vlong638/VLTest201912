
-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_ProjectTask_db_updatetime]
ON[dbo].[ProjectTask]
FOR UPDATE
AS
BEGIN
  update [ProjectTask] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END