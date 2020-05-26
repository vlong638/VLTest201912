using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.ConfigurableEntity
{
}
//select 
//'<Column colname="'+def.[列名]+'" typname="'+def.[数据类型]+'" maxlength="'+def.[占用字节数]
//+'" precision="'+def.[数字长度]+'" scale="'+def.[小数位数]
//+'" isnullable="'+def.[是否允许空]+'" isidentity="'+def.[是否自增]
//+'" iskey="'+def.[是否是主键]+'" propvalue="'+def.[说明]+'"></Column>'
//as definition
//,def.*
//from
//(
//     SELECT
//     tb.name as 表名,
//     col.column_id,
//   col.name AS 列名,
//   typ.name as 数据类型,
//   cast(col.max_length as varchar(200)) AS 占用字节数,
//     cast(col.[precision] as varchar(200)) AS 数字长度,
//   cast(col.scale as varchar(200)) AS 小数位数,
//   cast(col.is_nullable as varchar(200)) AS 是否允许空,
//   cast(col.is_identity as varchar(200))  AS 是否自增,
//   cast(case when exists
//      (SELECT 1
//        FROM
//          sys.indexes idx
//            join sys.index_columns idxCol
//            on(idx.object_id = idxCol.object_id)
//         WHERE
//            idx.object_id = col.object_id
//             AND idxCol.index_column_id = col.column_id
//             AND idx.is_primary_key = 1
//       ) THEN 1 ELSE 0 END as varchar(200)) AS 是否是主键,
//   cast(isnull(prop.[value], '-') as varchar(200)) AS 说明
//FROM sys.columns col
//left join sys.types typ on (col.system_type_id = typ.system_type_id)
//left join sys.extended_properties prop on(col.object_id = prop.major_id AND prop.minor_id = col.column_id)
//left join sys.tables tb on col.object_id = tb.object_id
//WHERE tb.name in ('CC_PhysicalExam_1_8'
//		 ,'dev_0430_新增字段_CC_PhysicalExam_12_30','dev_0430_新增字段_CC_PhysicalExam_36_72')
//) as def
//order by def.column_id