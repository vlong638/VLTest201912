
-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_Project_db_updatetime]
ON[dbo].[Project]
FOR UPDATE
AS
BEGIN
  update [Project] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
