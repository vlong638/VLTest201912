-- 注意: text暂不支持 需改为varchar(max) 大规格内容不支持科研系统输出

declare @startIndex int
set @startIndex = 103007000

declare @sourceName nvarchar(50)
set @sourceName = 'fm_postnatal'

select '<Properties>' as [Output], 1 as Row,1 as ColumnType
union all
select 
	concat(
		'<Property Id="'
		,@startIndex + t.RowNum
		,'" DisplayName="'
		,convert(nvarchar(max),t.说明) 
		,'" SourceName="'
		,t.列名
		,'"'
		,(
			case
			when t.ColumnType = 1 then ' ColumnType="String"'
			when t.ColumnType = 2 then ' ColumnType="DateTime"'
			when t.ColumnType = 3 then ' ColumnType="Int"'
			when t.ColumnType = 5 then ' ColumnType="Decimal"'
			when t.ColumnType = 6 then ' ColumnType="BigInt"'
			else convert(nvarchar(max),t.ColumnType) 
			end)
		,'/>'
	) as [Output]
	, 2 as Row
	,t.ColumnType
from
(

	SELECT
	row_number() over(order by col.column_id )as RowNum,
	tb.name as 表名,
	col.name AS 列名,
	tp.name as DataType,
	(case 
		when tp.name = 'varchar' then 1
		when tp.name = 'nvarchar' then 1
		when tp.name = 'datetime' then 2
		when tp.name = 'date' then 2
		when tp.name = 'int' then 3
		when tp.name = 'tinyint' then 3
		when tp.name = 'bit' then 3
		when tp.name = 'nchar' then 1
		when tp.name = 'char' then 1
		when tp.name = 'decimal' then 5
		when tp.name = 'bigint' then 6
		else 0
	end) as ColumnType,
	col.max_length AS 占用字节数,
	col.precision AS 数字长度,
	col.scale AS 小数位数,
	col.is_nullable  AS 是否允许空,
	col.is_identity  AS 是否自增,
	case when exists 
	( 
		SELECT 1 
		FROM 
		sys.indexes idx 
		join sys.index_columns idxCol 
		on (idx.object_id = idxCol.object_id)
		WHERE
		idx.object_id = col.object_id
		AND idxCol.index_column_id = col.column_id
		AND idx.is_primary_key = 1
	) THEN 1 ELSE 0 END  AS 是否是主键,
	isnull(prop.[value],'-') AS 说明
	FROM sys.tables tb
	left join sys.columns col on col.object_id = tb.object_id
	left join sys.types tp on (col.system_type_id = tp.system_type_id) and  tp.name!= 'sysname'
	left join sys.extended_properties prop on (col.object_id = prop.major_id AND prop.minor_id = col.column_id)
	WHERE tb.name = @sourceName
) as t
union all
select '</Properties>' as [Output], 3 as Row,1 as ColumnType
order by Row

