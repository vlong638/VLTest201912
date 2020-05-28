using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FrameworkTest.Common.ValuesSolution
{
    public static class DataTableEx
    {
        public static List<Dictionary<string, object>> ToList(this DataTable dt)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> line = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    line.Add(dc.ColumnName, dr[dc].ToString());
                }
                list.Add(line);
            }
            return list;
        }
        public static DataTable ToDataTable<T>(this List<T> datas) where T : class
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            var dt = new DataTable();
            foreach (var p in properties)
            {
                //获取类型
                Type colType = p.PropertyType;
                //当类型为Nullable<>时
                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    colType = colType.GetGenericArguments()[0];
                }
                dt.Columns.Add(p.Name, colType);
            }
            for (int i = 0; i < datas.Count(); i++)
            {
                var row = dt.NewRow();
                foreach (var p in properties)
                {
                    object obj = p.GetValue(datas[i], null);
                    row[p.Name] = obj ?? DBNull.Value;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
}
