

-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_ProjectTaskWhere_db_updatetime]
ON[dbo].[ProjectTaskWhere]
FOR UPDATE
AS
BEGIN
  update [ProjectTaskWhere] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END