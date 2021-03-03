
-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_BusinessEntityProperty_db_updatetime]
ON[dbo].[BusinessEntityProperty]
FOR UPDATE
AS
BEGIN
  update [BusinessEntityProperty] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END