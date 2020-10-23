-- 补充排序字段
ALTER TABLE [dbo].[GY_FUWUJG] ADD [DiZhiBMOrder] varchar(255) NULL ; 
-- 数据订正
update GY_FUWUJG set DiZhiBMOrder = case when dizhibm ='000000' then '999999' else dizhibm end ; 


-- 校验
select jg.dizhimc 地区名称
,jg.dizhibm
from (
		select dizhibm,dizhimc,DiZhiBMOrder
		from GY_FUWUJG
		group by dizhibm,dizhimc,DiZhiBMOrder
		order by DiZhiBMOrder asc
		offset 0 rows fetch next 10 rows only
) jg
order by DiZhiBMOrder

