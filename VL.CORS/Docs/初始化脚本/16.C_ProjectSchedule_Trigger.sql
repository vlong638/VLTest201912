

-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_ProjectSchedule_db_updatetime]
ON[dbo].[ProjectSchedule]
FOR UPDATE
AS
BEGIN
  update [ProjectSchedule] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END