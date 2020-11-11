using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Xml.XPath;
using VL.Consolo_Core.Common.ValuesSolution;

namespace VL.Consolo_Core.Common.ExcelExportSolution
{
    class IfCondition
    {
        public static string ElementName = "If";
        public string Operator { set; get; }
        public string ComponentName { set; get; }
        public string Value { set; get; }
        public string Text { set; get; }

        public IfCondition(XElement element)
        {
            Operator = element.Attribute(nameof(Operator)).Value;
            ComponentName = element.Attribute(nameof(ComponentName))?.Value;
            Value = element.Attribute(nameof(Value))?.Value;
            Text = WebUtility.HtmlDecode(element.Value);
        }

        internal string GetSQL(List<SQLConfigWhere> wheres)
        {
            switch (Operator)
            {
                case "NotEmpty":
                    var where = wheres.FirstOrDefault(c => c.ComponentName == ComponentName);
                    if (where != null && !where.Value.IsNullOrEmpty())
                    {
                        return Text;
                    }
                    break;
                case "eq":
                    where = wheres.FirstOrDefault(c => c.ComponentName == ComponentName);
                    if (where != null && where.Value == Value)
                    {
                        return Text;
                    }
                    break;
                default:
                    break;
            }
            return "";
        }
    }

    /// <summary>
    /// 数据源
    /// </summary>
    public class SQLConfigSource
    {
        /// <summary>
        /// 
        /// </summary>
        public const string ElementName = "Source";

        /// <summary>
        /// 数据源名称
        /// </summary>
        public string SourceName { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string DBSourceType { set; get; }

        /// <summary>
        /// 页面字段
        /// </summary>
        public List<SQLConfigProperty> Properties { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public List<ExportSourceTransform> Transforms { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public List<ExportSourceMapping> Mappings { set; get; }
        /// <summary>
        /// 页面条件项
        /// </summary>
        public List<SQLConfigWhere> Wheres { set; get; }
        /// <summary>
        /// 页面排序项
        /// </summary>
        public List<SQLConfigOrderBy> OrderBys { set; get; }
        /// <summary>
        /// 默认排序项
        /// </summary>
        public string DefaultComponentName { get; private set; }
        /// <summary>
        /// 默认排序项
        /// </summary>
        public string DefaultOrder { get; private set; }
        /// <summary>
        /// 预设SQL
        /// </summary>
        public SQLConfigSQLs SQLs { set; get; }
        /// <summary>
        /// 预设CountSQL
        /// </summary>
        public string CountSQL { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public SQLConfigSource(XElement element)
        {
            SourceName = element.Attribute(nameof(SourceName))?.Value ?? "";
            DBSourceType = element.Attribute(nameof(DBSourceType))?.Value ?? "";
            Properties = element.Descendants(SQLConfigProperty.ElementName).Select(c => new SQLConfigProperty(c)).ToList();
            Wheres = element.Descendants(SQLConfigWhere.ElementName).Select(c => new SQLConfigWhere(c)).ToList();
            OrderBys = element.Descendants(SQLConfigOrderBy.ElementName).Select(c => new SQLConfigOrderBy(c)).ToList();
            SQLs = element.Descendants(nameof(SQLs))?.Select(c => new SQLConfigSQLs(c)).FirstOrDefault();
            if (SQLs==null)
                SQLs = new SQLConfigSQLs() { Texts = new List<string>() { element.Descendants("SQL")?.FirstOrDefault()?.ToString().TrimStart("<SQL>").TrimEnd("</SQL>") } };
            //SQL = WebUtility.HtmlDecode(SQL);
            CountSQL = element.Descendants(nameof(CountSQL))?.FirstOrDefault()?.ToString().TrimStart("<CountSQL>").TrimEnd("</CountSQL>");
            //CountSQL = WebUtility.HtmlDecode(CountSQL);
            var OrderBysRoot = element.Descendants(SQLConfigOrderBy.RootElementName).FirstOrDefault();
            DefaultComponentName = OrderBysRoot?.Attribute(nameof(DefaultComponentName))?.Value ?? "";
            DefaultOrder = OrderBysRoot?.Attribute(nameof(DefaultOrder))?.Value ?? "";
            Transforms = element.Descendants(ExportSourceTransform.ElementName).Select(c => new ExportSourceTransform(c)).ToList();
            Mappings = element.Descendants(ExportSourceMapping.ElementName).Select(c => new ExportSourceMapping(c)).ToList();


            //SourceName = element.Attribute(nameof(SourceName))?.Value;
            //DBSourceType = element.Attribute(nameof(DBSourceType))?.Value;
            //SQL = element.Descendants("SQL").First().Value;
            //Properties = element.Descendants(SQLConfigProperty.ElementName).Select(c => new SQLConfigProperty(c)).ToList();
            //Transforms = element.Descendants(ExportSourceTransform.ElementName).Select(c => new ExportSourceTransform(c)).ToList();
            //Mappings = element.Descendants(ExportSourceMapping.ElementName).Select(c => new ExportSourceMapping(c)).ToList();
            //Wheres = element.Descendants(SQLConfigWhere.ElementName).Select(c => new SQLConfigWhere(c)).ToList();
            //OrderBys = element.Descendants(SQLConfigOrderBy.ElementName).Select(c => new SQLConfigOrderBy(c)).ToList();
        }

        public string GetCountSQL()
        {
            if (!CountSQL.IsNullOrEmpty())
            {
                var sql = CountSQL;
                UpdateIf(ref sql, Wheres);
                var wheresIsOn = Wheres.Where(c => c.IsOn).Select(c => c.SQL);
                var wheres = wheresIsOn.Count() == 0 ? "" : $"where {string.Join(" and ", wheresIsOn)}";
                sql = sql.Replace("@Wheres", wheres);
                return sql;
            }
            else
            {
                var sql = SQLs.Texts.FirstOrDefault();
                UpdateIf(ref sql, Wheres);
                sql = WebUtility.HtmlDecode(sql);
                sql = sql.Replace("@Properties", "count(*)");
                var wheresIsOn = Wheres.Where(c => c.IsOn).Select(c => c.SQL);
                var wheres = wheresIsOn.Count() == 0 ? "" : $"where {string.Join(" and ", wheresIsOn)}";
                sql = sql.Replace("@Wheres", wheres);
                sql = sql.Replace("@OrderBy", $"");
                sql = sql.Replace("@Pager", $"");
                return sql;
            }
        }

        public string GetListSQL(string sql,int skip = 0,int limit = 0)
        {
            //Properties
            var propertiesIsOn = Properties.Select(c => c.Alias);
            var fields = propertiesIsOn.Count() == 0 ? "*" : string.Join(",", propertiesIsOn);
            sql = sql.Replace("@Properties", fields);
            //Where
            UpdateIf(ref sql, Wheres);
            sql = WebUtility.HtmlDecode(sql);
            var wheresIsOn = Wheres.Where(c => c.IsOn).Select(c => c.SQL);
            var wheres = wheresIsOn.Count() == 0 ? "" : $"where {string.Join(" and ", wheresIsOn)}";
            sql = sql.Replace("@Wheres", wheres);
            //OrderBy
            var orderByIsOn = OrderBys.Count() == 0 ? null : OrderBys.FirstOrDefault(c => c.IsOn);
            if (orderByIsOn!=null)
            {
                var orderBy = orderByIsOn.Alias;
                var order = orderByIsOn.IsAsc ? "asc" : "desc";
                sql = sql.Replace("@OrderBy", $"order by {orderBy} {order}");
            }
            //Pager
            if (limit==0)
            {
                sql = sql.Replace("@Pager", $"");
            }
            else
            {
                sql = sql.Replace("@Pager", $"offset {skip} rows fetch next {limit} rows only");
            }
            return sql;
        }
        private void UpdateIf(ref string sql, List<SQLConfigWhere> wheres)
        {
            var ifItems = sql.GetMatches("<If", "</If>");
            foreach (var ifItem in ifItems)
            {
                var xItem = new XDocument(new XElement("root", XElement.Parse(ifItem)));
                var ifEntity = xItem.Descendants(IfCondition.ElementName).Select(c => new IfCondition(c)).First();
                sql = sql.Replace(ifItem, ifEntity.GetSQL(wheres));
            }
        }

        public Dictionary<string, object> GetParams()
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            foreach (var where in Wheres)
            {
                if (where.IsOn)
                {
                    args.Add(where.ComponentName, where.Formatter.IsNullOrEmpty() ? where.Value : where.Formatter.Replace("@" + where.ComponentName, where.Value.ToString()));
                }
            }
            return args;
        }

        public void DoTransforms(ref DataTable datatable)
        {
            foreach (var transform in Transforms)
            {
                switch (transform.SourceType)
                {
                    case SourceType.Field:
                        #region Field
                        switch (transform.FunctionType)
                        {
                            case TransfromFunctionType.Case:
                                #region Case
                                datatable.Columns.Add(new DataColumn(transform.TargetColumnName));
                                for (int i = 0; i < datatable.Rows.Count; i++)
                                {
                                    var value = GetValue(datatable.Rows[i], transform.Cases);
                                    datatable.Rows[i][transform.TargetColumnName] = value;
                                }
                                #endregion
                                break;
                            default:
                                throw new NotImplementedException("SourceType.Field未支持该`FunctionType` ");
                        }
                        #endregion
                        break;
                    case SourceType.JsonList:
                        #region JsonList
                        switch (transform.FunctionType)
                        {
                            case TransfromFunctionType.None:
                                break;
                            case TransfromFunctionType.Case:
                                break;
                            case TransfromFunctionType.SumInt:
                                #region SumInt
                                datatable.Columns.Add(new DataColumn(transform.TargetColumnName));
                                for (int i = 0; i < datatable.Rows.Count; i++)
                                {
                                    var jsonTable = datatable.Rows[i][transform.ColumnName].ToString()?.FromJson<DataTable>();
                                    if (jsonTable == null)
                                        continue;
                                    var sumValue = jsonTable.AsEnumerable().Sum(c =>
                                    {
                                        var text = c.Field<string>(transform.SubFieldName);
                                        var value = text?.ToInt();
                                        return value.HasValue ? value.Value : 0;
                                    });
                                    datatable.Rows[i][transform.TargetColumnName] = sumValue;
                                }
                                #endregion
                                break;
                            case TransfromFunctionType.SumCase:
                                #region SumCase
                                datatable.Columns.Add(new DataColumn(transform.TargetColumnName));
                                for (int i = 0; i < datatable.Rows.Count; i++)
                                {
                                    var jsonTable = datatable.Rows[i][transform.ColumnName].ToString()?.FromJson<DataTable>();
                                    if (jsonTable == null)
                                        continue;

                                    var sumValue = 0;
                                    foreach (var row in jsonTable.AsEnumerable())
                                    {
                                        sumValue += GetValue(row, transform.Cases).ToInt() ?? 0;
                                    }
                                    datatable.Rows[i][transform.TargetColumnName] = sumValue;
                                }
                                #endregion
                                break;
                            case TransfromFunctionType.Join:
                                #region Join
                                datatable.Columns.Add(new DataColumn(transform.TargetColumnName));
                                for (int i = 0; i < datatable.Rows.Count; i++)
                                {
                                    var jsonTable = datatable.Rows[i][transform.ColumnName].ToString()?.FromJson<DataTable>();
                                    if (jsonTable == null)
                                        continue;

                                    var joinValue = "";
                                    foreach (var row in jsonTable.AsEnumerable())
                                    {
                                        if (joinValue!="")
                                        {
                                            joinValue += transform.Splitter;
                                        }
                                        var text = row.Field<string>(transform.SubFieldName);
                                        joinValue += text;
                                    }
                                    datatable.Rows[i][transform.TargetColumnName] = joinValue;
                                } 
                                #endregion
                                break;
                            case TransfromFunctionType.JoinCase:
                                #region JoinCase
                                datatable.Columns.Add(new DataColumn(transform.TargetColumnName));
                                for (int i = 0; i < datatable.Rows.Count; i++)
                                {
                                    var jsonTable = datatable.Rows[i][transform.ColumnName].ToString()?.FromJson<DataTable>();
                                    if (jsonTable == null)
                                        continue;

                                    var joinValue = "";
                                    foreach (var row in jsonTable.AsEnumerable())
                                    {
                                        if (joinValue != "")
                                        {
                                            joinValue += transform.Splitter;
                                        }
                                        joinValue += GetValue(row, transform.Cases);
                                    }
                                    datatable.Rows[i][transform.TargetColumnName] = joinValue;
                                }
                                #endregion
                                break;
                            default:
                                throw new NotImplementedException("未支持该`FunctionType` ");
                        }
                        #endregion
                        break;
                    default:
                        throw new NotImplementedException("SumInt下未支持该`SourceType` ");
                }
            }
        }

        public void DoMappings(ref DataTable datatable, Dictionary<string, DataTable> dataSources)
        {
            foreach (var mapping in Mappings)
            {
                datatable.Columns.Add(new DataColumn(mapping.TargetColumnName));
                var sourceData = dataSources[mapping.SourceName];
                for (int i = 0; i < datatable.Rows.Count; i++)
                {
                    var row = datatable.Rows[i];
                    EnumerableRowCollection<DataRow> temp = null;
                    //关联
                    switch (mapping.RelatedBy.DataType)
                    {
                        case DataType.String:
                            var relatedString = row[mapping.RelatedBy.RelatedColumnName].ToString();
                            temp= dataSources[mapping.SourceName].AsEnumerable().Where(c => c.Field<string>(mapping.RelatedBy.ColumnName) == relatedString);
                            break;
                        case DataType.Int:
                            var relatedInt = row[mapping.RelatedBy.RelatedColumnName].ToInt();
                            temp = dataSources[mapping.SourceName].AsEnumerable().Where(c => c.Field<int>(mapping.RelatedBy.ColumnName) == relatedInt);
                            break;
                        case DataType.DateTime:
                            var relatedDateTime = row[mapping.RelatedBy.RelatedColumnName].ToDateTime();
                            temp = dataSources[mapping.SourceName].AsEnumerable().Where(c => c.Field<DateTime>(mapping.RelatedBy.ColumnName) == relatedDateTime);
                            break;
                        default:
                            break;
                    }
                    //筛选
                    foreach (var where in mapping.Wheres)
                    {
                        switch (where.DataType)
                        {
                            case DataType.String:
                                var compareString = where.Value;
                                switch (where.Operator)
                                {
                                    case OperatorType.eq:
                                        temp = temp.Where(c => c.Field<string>(where.ColumnName) == compareString);
                                        break;
                                    case OperatorType.neq:
                                        temp = temp.Where(c => c.Field<string>(where.ColumnName) != compareString);
                                        break;
                                    default:
                                        throw new NotImplementedException("未支持该`OperatorType` 201");
                                }
                                break;
                            case DataType.Int:
                                var compareInt = where.Value.ToInt();
                                switch (where.Operator)
                                {
                                    case OperatorType.eq:
                                        temp = temp.Where(c => c.Field<int>(where.ColumnName) == compareInt);
                                        break;
                                    case OperatorType.neq:
                                        temp = temp.Where(c => c.Field<int>(where.ColumnName) != compareInt);
                                        break;
                                    case OperatorType.gt:
                                        temp = temp.Where(c => c.Field<int>(where.ColumnName) > compareInt);
                                        break;
                                    case OperatorType.lt:
                                        temp = temp.Where(c => c.Field<int>(where.ColumnName) < compareInt);
                                        break;
                                    default:
                                        throw new NotImplementedException("未支持该`OperatorType` 220");
                                }
                                break;
                            case DataType.DateTime:
                                var compareDateTime = where.Value.ToDateTime();
                                switch (where.Operator)
                                {
                                    case OperatorType.eq:
                                        temp = temp.Where(c => c.Field<DateTime?>(where.ColumnName) == compareDateTime);
                                        break;
                                    case OperatorType.neq:
                                        temp = temp.Where(c => c.Field<DateTime?>(where.ColumnName) != compareDateTime);
                                        break;
                                    case OperatorType.gt:
                                        temp = temp.Where(c => c.Field<DateTime?>(where.ColumnName) > compareDateTime);
                                        break;
                                    case OperatorType.lt:
                                        temp = temp.Where(c => c.Field<DateTime?>(where.ColumnName) < compareDateTime);
                                        break;
                                    default:
                                        throw new NotImplementedException("未支持该`OperatorType` 241");
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    //排序
                    if (mapping.OrderBy.IsAsc)
                    {
                        temp = temp.OrderBy(c => c[mapping.OrderBy.ColumnName]);
                    }
                    else
                    {
                        temp = temp.OrderByDescending(c => c[mapping.OrderBy.ColumnName]);
                    }
                    //求值
                    if (temp == null || temp.Count() == 0)
                        continue;
                    switch (mapping.FunctionType)
                    {
                        case MappingFunctionType.None:
                            break;
                        case MappingFunctionType.First:
                            row[mapping.TargetColumnName] = temp.First().Field<string>(mapping.FieldName);
                            break;
                        case MappingFunctionType.SumInt:
                            row[mapping.TargetColumnName] = temp.Sum(c => c.Field<string>(mapping.FieldName)?.ToInt() ?? 0);
                            break;
                        default:
                            throw new NotImplementedException("未支持该`FunctionType` 269");
                    }
                }
            }
        }

        private string GetValue(DataRow row, List<ExportSourceTransformCase> cases)
        {
            foreach (var @case in cases)
            {
                string text = "";
                if (@case.DataType != DataType.Default)
                    text = row[@case.ColumnName].ToString();
                switch (@case.DataType)
                {
                    case DataType.Default:
                        return @case.Then;
                    case DataType.String:
                        if (text == @case.Value)
                            return @case.Then;
                        break;
                    case DataType.Int:
                        var sourceValueInt = text.ToInt();
                        if (!sourceValueInt.HasValue)
                            continue;
                        var compareValueInt = @case.Value.ToInt();
                        if (!compareValueInt.HasValue)
                            throw new NotImplementedException("无效的值配置");
                        switch (@case.Operator)
                        {
                            case OperatorType.eq:
                                if (sourceValueInt == compareValueInt)
                                    return @case.Then;
                                break;
                            case OperatorType.neq:
                                if (sourceValueInt != compareValueInt)
                                    return @case.Then;
                                break;
                            case OperatorType.gt:
                                if (sourceValueInt > compareValueInt)
                                    return @case.Then;
                                break;
                            case OperatorType.lt:
                                if (sourceValueInt < compareValueInt)
                                    return @case.Then;
                                break;
                            default:
                                throw new NotImplementedException("未支持该`Operator` locator:471");
                        }
                        break;
                    case DataType.DateTime:
                        var sourceValueTime = text.ToDateTime();
                        if (!sourceValueTime.HasValue)
                            continue;
                        var compareValueTime = @case.Value.ToDateTime();
                        if (!compareValueTime.HasValue)
                            throw new NotImplementedException("无效的值配置");
                        switch (@case.Operator)
                        {
                            case OperatorType.eq:
                                if (sourceValueTime.Value == compareValueTime.Value)
                                    return @case.Then;
                                break;
                            case OperatorType.neq:
                                if (sourceValueTime.Value != compareValueTime.Value)
                                    return @case.Then;
                                break;
                            case OperatorType.gt:
                                if (sourceValueTime.Value > compareValueTime.Value)
                                    return @case.Then;
                                break;
                            case OperatorType.lt:
                                if (sourceValueTime.Value < compareValueTime.Value)
                                    return @case.Then;
                                break;
                            default:
                                throw new NotImplementedException("未支持该`Operator` locator:500");
                        }
                        break;
                    default:
                        throw new NotImplementedException("未支持该`CaseType` locator:466");
                }
            }
            return "";
        }

        public string CheckWheres(List<VLKeyValue> keyValues)
        {
            foreach (var where in Wheres)
            {
                if (where.Required)
                {
                    var matched = keyValues.FirstOrDefault(c => c.Key == where.ComponentName);
                    if (matched == null || matched.Value.IsNullOrEmpty())
                    {
                        return "缺少必要的参数";
                    }
                }
            }
            return "";
        }
    }

    public class SQLConfigSQLs
    {
        public List<string> Texts { set; get; }
        public string UnitedBy { set; get; }

        public SQLConfigSQLs()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public SQLConfigSQLs(XElement element)
        {
            Texts = element.Descendants("SQL").Select(c => c.ToString().TrimStart("<SQL>").TrimEnd("</SQL>")).ToList();
            UnitedBy = element.Attribute(nameof(UnitedBy))?.Value ?? "";
        }
    }
}
