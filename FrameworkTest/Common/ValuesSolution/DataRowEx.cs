using System.Data;

namespace FrameworkTest.Common.ValuesSolution
{
    public static class DataRowEx
    {
        public static string GetRowValue(this DataRow row, string key)
        {
            return row[key].ToString();
        }
    }
}
