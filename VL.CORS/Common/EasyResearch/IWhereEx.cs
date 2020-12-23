using Autobots.Infrastracture.Common.ValuesSolution;
using System;

namespace ResearchAPI.CORS.Common
{
    public static class IWhereEx
    {
        public static string GetValue(this DBField field, string value, string valueFormat)
        {
            var tempValue = value;
            switch (field.ColumnType)
            {
                case "string":
                    tempValue = value;
                    break;
                default:
                    throw new NotImplementedException("未支持的字段类型");
            }
            if (valueFormat.IsNullOrEmpty())
            {
                return tempValue;
            }
            return string.Format(valueFormat, tempValue);
        }

        public static string ToSQL(this RouteType routeType)
        {
            switch (routeType)
            {
                case RouteType.LeftJoin:
                    return "left join";
                case RouteType.InnerJoin:
                    return "inner join";
                default:
                    throw new NotImplementedException("未支持的连接类型");
            }
        }

        public static string ToSQL(this WhereOperator @operator)
        {
            switch (@operator)
            {
                case WhereOperator.Equal:
                    return " = ";
                case WhereOperator.Like:
                    return " like ";
                case WhereOperator.IsNotNull:
                    return " is not null ";
                case WhereOperator.IsNull:
                    return " is null ";
                case WhereOperator.GreatThan:
                    return " > ";
                case WhereOperator.LessThan:
                    return " < ";
                case WhereOperator.GreatOrEqualThan:
                    return " >= ";
                case WhereOperator.LessOrEqualThan:
                    return " <= ";
                default:
                    return "";
            }
        }
    }
}
