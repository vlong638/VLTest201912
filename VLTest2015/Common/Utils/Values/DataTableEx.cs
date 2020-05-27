using System.Collections.Generic;
using System.Data;

namespace VLTest2015
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
    }
}