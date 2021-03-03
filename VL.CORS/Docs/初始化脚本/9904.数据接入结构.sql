
--数据接入结构
-- select * from Structure_PregnantInfo
-- drop table Structure_LabOrder;
-- drop table Structure_LabResult;
-- drop table Structure_MHC_FirstVisitRecord;
-- drop table Structure_MHC_VisitRecord;
-- drop table Structure_PregnantInfo;

select concat('<Property Id="',101005000+RowNum,'" DisplayName="'
	,t.说明
	,'" SourceName="',t.列名,'"'
	,(case
	when t.ColumnType = 1 then ' ColumnType="String"'
	when t.ColumnType = 2 then ' ColumnType="DateTime"'
	when t.ColumnType = 3 then ' ColumnType="Int"'
	when t.ColumnType = 4 then ' ColumnType="Enum" EnumType="'+t.对应字典+'"'
	when t.ColumnType = 5 then ' ColumnType="Decimal"'
	when t.ColumnType = 6 then ' ColumnType="BigInt"'
	else t.数据类型
	end)
	,'/>') as [Output]
	from (
	select 
	row_number()over(order by 列名 )as RowNum
	,(case 
	when 是否文本 = '是' then 1
	when 是否文本 = '否' and 对应字典 is not null then 4
	when 数据类型 = 'varchar' then 1
	when 数据类型 = 'nvarchar' then 1
	when 数据类型 = 'datetime' then 2
	when 数据类型 = 'date' then 2
	when 数据类型 = 'int' then 3
	when 数据类型 = 'bit' then 3
	when 数据类型 = 'nchar' then 1
	when 数据类型 = 'char' then 1
	when 数据类型 = 'decimal' then 5
	when 数据类型 = 'bigint' then 6
	else 0
	end) as ColumnType
	,*
	from Structure
-- 	where 来源表 = 'PregnantInfo'
-- 	where 来源表 = 'MHC_VisitRecord'
	where 来源表 = 'MHC_FirstVisitRecord'
-- 	where 来源表 = 'LabOrder'
-- 	where 来源表 = 'LabResult'
) as t
-- where t.ColumnType = 0

-- select  * from Structure

