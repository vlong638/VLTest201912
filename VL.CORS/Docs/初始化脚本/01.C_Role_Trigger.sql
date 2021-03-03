
-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_Role_db_updatetime]
ON[dbo].[Role]
FOR UPDATE
AS
BEGIN
  update [Role] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END