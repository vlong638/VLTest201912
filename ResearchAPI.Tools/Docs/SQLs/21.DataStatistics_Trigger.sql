
-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_DataStatistics_db_updatetime]
ON[dbo].[DataStatistics]
FOR UPDATE
AS
BEGIN
  update [DataStatistics] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
