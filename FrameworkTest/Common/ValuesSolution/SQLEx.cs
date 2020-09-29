namespace FrameworkTest.Common.ValuesSolution
{
    public static class SQLEx
    {
        public static string ToMSSQLValue(this string v)
        {
            return "'" + v + "'";
        }
    }
}
