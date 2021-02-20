using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Autobots.Infrastracture.Common.ValuesSolution
{
    public static class DataTableEx
    {
        /// <summary>     
        /// Datatable 转换为 Json     
        /// </summary>    
        /// <param name="table">Datatable对象</param>     
        /// <returns>Json字符串</returns>     
        public static string ToJson(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();

                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = String.Format(strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append("\"" + strValue + "\"" + ",");
                    }
                    else
                    {
                        jsonString.Append("\"" + strValue + "\"");
                    }
                }
                jsonString.Append("},");
            }
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }
        /// <summary>
        /// Datatable 转换为 字典集合
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 字典 转换为 DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> datas) where T:class
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
