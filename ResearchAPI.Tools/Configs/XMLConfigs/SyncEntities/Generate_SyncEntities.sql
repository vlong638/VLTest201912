-- SELECT 来源表 as TableName,列名 as ColumnName,字段控件类型 as ControlType,是否文本 as IsEnumText,对应字典 as Enum into StructureV2 FROM [dbo].[Structure]
-- select * from StructureV2


declare @sourceName nvarchar(50)
set @sourceName = 'PregnantInfo'


declare @startIndex int
set @startIndex = 101001000

select '<Properties>' as [Output], 1 as Row,'' as ColumnType
union all
select 
	concat(
		'<Property '
		,'Id="',@startIndex + t.RowNum,'"'
		,' DisplayName="',case when convert(nvarchar(max),t.Remark) ='-' then t.ColumnName else convert(nvarchar(max),t.Remark) end ,'"'
		,' SourceName="',t.ColumnName,'"'
		,' ColumnType="',t.ColumnType,'"'
		,' MaxLength="',t.MaxLength,'"'
		,' Precision="',t.Precision,'"'
		,' Scale="',t.Scale,'"'
		,case when t.Enum != '' then ' Enum="'+t.Enum+'"' else '' end		
		,case when t.IsEnumText != '' then ' IsEnumText="'+t.IsEnumText+'"' else '' end		
		,case when t.ControlType != '' then ' ControlType="'+t.ControlType+'"' else '' end		
		,'/>'
	) as [Output]
	, 2 as Row
	,t.ColumnType
from
(

	SELECT
	row_number() over(order by col.column_id )as RowNum,
	tb.name as TableName,
	col.name AS ColumnName,
	tp.name as ColumnType,
	col.max_length AS MaxLength,
	col.precision AS Precision,
	col.scale AS Scale,
	col.is_nullable  AS IsNullable,
	col.is_identity  AS IsIdentity,
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
	) THEN 1 ELSE 0 END  AS IsPrimaryKey,
	isnull(prop.[value],'-') AS Remark
	,st.ControlType,st.IsEnumText,st.Enum
	FROM sys.tables tb
	left join sys.columns col on col.object_id = tb.object_id
	left join sys.types tp on (col.system_type_id = tp.system_type_id) and  tp.name!= 'sysname'
	left join sys.extended_properties prop on (col.object_id = prop.major_id AND prop.minor_id = col.column_id)
	left join StructureV2 st on tb.name = st.TableName and st.ColumnName = col.name
	WHERE tb.name = @sourceName
) as t
union all
select '</Properties>' as [Output], 3 as Row,'' as ColumnType
order by Row





declare @sourceName nvarchar(50)
set @sourceName = 'PregnantInfo'


declare @startIndex int
set @startIndex = 101001000
select t.IsEnumText
from(
SELECT
	row_number() over(order by col.column_id )as RowNum,
	tb.name as TableName,
	col.name AS ColumnName,
	tp.name as ColumnType,
	col.max_length AS MaxLength,
	col.precision AS Precision,
	col.scale AS Scale,
	col.is_nullable  AS IsNullable,
	col.is_identity  AS IsIdentity,
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
	) THEN 1 ELSE 0 END  AS IsPrimaryKey,
	isnull(prop.[value],'-') AS Remark
	,st.ControlType,st.IsEnumText,st.Enum
	FROM sys.tables tb
	left join sys.columns col on col.object_id = tb.object_id
	left join sys.types tp on (col.system_type_id = tp.system_type_id) and  tp.name!= 'sysname'
	left join sys.extended_properties prop on (col.object_id = prop.major_id AND prop.minor_id = col.column_id)
	left join StructureV2 st on tb.name = st.TableName and st.ColumnName = col.name
	WHERE tb.name = @sourceName
	) as t 
	GROUP BY t.IsEnumText