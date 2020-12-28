using Autobots.Infrastracture.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace ResearchAPI.CORS.Common
{
    public class IfConditionV2
    {
        public static string ElementName = "If";
        public string Operator { set; get; }
        public string ComponentName { set; get; }
        public string Value { set; get; }
        public string Text { set; get; }

        public IfConditionV2(XElement element)
        {
            Operator = element.Attribute(nameof(Operator)).Value;
            ComponentName = element.Attribute(nameof(ComponentName))?.Value;
            Value = element.Attribute(nameof(Value))?.Value;
            Text = WebUtility.HtmlDecode(element.Value);
        }

        internal string GetSQL(List<SQLConfigV2Where> wheres)
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
    /// 查询sql配置
    /// </summary>
    public class SQLConfigV2
    {
        #region 预设配置
        /// <summary>
        /// 
        /// </summary>
        public static string ElementName = "View";

        /// <summary>
        /// 页面名称
        /// </summary>
        public string ViewName { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public SQLConfigV2Source Source { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public SQLConfigV2()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public SQLConfigV2(XElement element)
        {
            ViewName = element.Attribute(nameof(ViewName))?.Value;
            Source = element.Descendants(SQLConfigV2Source.ElementName).Select(c => new SQLConfigV2Source(c)).First();
        }
        #endregion

        #region 动态更新
        internal void UpdateOrderBy(string field, string order)
        {
            var tempField = field.IsNullOrEmpty() ? Source.DefaultComponentName : field;
            var tempOrder = field.IsNullOrEmpty() ? Source.DefaultOrder : order;
            foreach (var OrderBy in Source.OrderBys)
            {
                OrderBy.IsOn = false;

                if (tempField == OrderBy.ComponentName)
                {
                    OrderBy.IsOn = true;
                    OrderBy.IsAsc = tempOrder == "asc";
                }
            }
        }
        internal void UpdateWheres(List<VLKeyValue> wheres)
        {
            if (wheres == null)
                return;

            foreach (var where in wheres)
            {
                var whereConfig = Source.Wheres.FirstOrDefault(c => c.ComponentName.ToLower() == where.Key.ToLower());
                if (where != null && !where.Value.IsNullOrEmpty() && whereConfig != null)
                {
                    whereConfig.IsOn = true;
                    whereConfig.Value = where.Value;
                }
            }
        }

        #region 分页
        public int PageIndex { set; get; }
        public int PageSize { set; get; }
        public int Skip
        {
            get
            {
                var skip = (PageIndex - 1) * PageSize;
                return skip > 0 ? skip : 0;
            }
        }
        public int Limit
        {
            get { return PageSize > 0 ? PageSize : 10; }
        }

        #endregion
        #endregion

        #region 结果使用

        private static string GetFormattedValue(SQLConfigV2Where where)
        {
            if (where.Value.IsNullOrEmpty())
            {
                return where.Value;
            }
            return where.Value.ToString();
        }

        #endregion
    }

    /// <summary>
    /// 数据源
    /// </summary>
    public class SQLConfigV2Source
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
        public List<SQLConfigV2Property> Properties { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public List<ExportSourceTransformV2> Transforms { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public List<ExportSourceMapping> Mappings { set; get; }
        /// <summary>
        /// 页面条件项
        /// </summary>
        public List<SQLConfigV2Where> Wheres { set; get; }
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
        public SQLConfigV2SQLs SQLs { set; get; }
        /// <summary>
        /// 预设CountSQL
        /// </summary>
        public string CountSQL { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public SQLConfigV2Source(XElement element)
        {
            SourceName = element.Attribute(nameof(SourceName))?.Value ?? "";
            DBSourceType = element.Attribute(nameof(DBSourceType))?.Value ?? "";
            Properties = element.Descendants(SQLConfigV2Property.ElementName).Select(c => new SQLConfigV2Property(c)).ToList();
            Wheres = element.Descendants(SQLConfigV2Where.ElementName).Select(c => new SQLConfigV2Where(c)).ToList();
            OrderBys = element.Descendants(SQLConfigOrderBy.ElementName).Select(c => new SQLConfigOrderBy(c)).ToList();
            SQLs = element.Descendants(nameof(SQLs))?.Select(c => new SQLConfigV2SQLs(c)).FirstOrDefault();
            if (SQLs == null)
                SQLs = new SQLConfigV2SQLs() { Texts = new List<string>() { element.Descendants("SQL")?.FirstOrDefault()?.ToString().TrimStart("<SQL>").TrimEnd("</SQL>") } };
            //SQL = WebUtility.HtmlDecode(SQL);
            CountSQL = element.Descendants(nameof(CountSQL))?.FirstOrDefault()?.ToString().TrimStart("<CountSQL>").TrimEnd("</CountSQL>");
            //CountSQL = WebUtility.HtmlDecode(CountSQL);
            var OrderBysRoot = element.Descendants(SQLConfigOrderBy.RootElementName).FirstOrDefault();
            DefaultComponentName = OrderBysRoot?.Attribute(nameof(DefaultComponentName))?.Value ?? "";
            DefaultOrder = OrderBysRoot?.Attribute(nameof(DefaultOrder))?.Value ?? "";
            Transforms = element.Descendants(ExportSourceTransformV2.ElementName).Select(c => new ExportSourceTransformV2(c)).ToList();
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

        public string GetListSQL(string sql, int skip = 0, int limit = 0)
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
            if (orderByIsOn != null)
            {
                var orderBy = orderByIsOn.Alias;
                var order = orderByIsOn.IsAsc ? "asc" : "desc";
                sql = sql.Replace("@OrderBy", $"order by {orderBy} {order}");
            }
            //Pager
            if (limit == 0)
            {
                sql = sql.Replace("@Pager", $"");
            }
            else
            {
                sql = sql.Replace("@Pager", $"offset {skip} rows fetch next {limit} rows only");
            }
            return sql;
        }
        private void UpdateIf(ref string sql, List<SQLConfigV2Where> wheres)
        {
            var ifItems = sql.GetMatches("<If", "</If>");
            foreach (var ifItem in ifItems)
            {
                var xItem = new XDocument(new XElement("root", XElement.Parse(ifItem)));
                var ifEntity = xItem.Descendants(IfCondition.ElementName).Select(c => new IfConditionV2(c)).First();
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
                    switch (where.DataType)
                    {
                        case "ArrayInt":
                            var values = where.Value.Split(",").Select(c =>
                            {
                                var r = 0L;
                                long.TryParse(c, out r);
                                return r;
                            });
                            args.Add(where.ComponentName, values);
                            break;
                        default:
                            args.Add(where.ComponentName, where.Formatter.IsNullOrEmpty() ? where.Value : where.Formatter.Replace("@" + where.ComponentName, where.Value.ToString()));
                            break;
                    }
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
                                        if (joinValue != "")
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
                            temp = dataSources[mapping.SourceName].AsEnumerable().Where(c => c.Field<string>(mapping.RelatedBy.ColumnName) == relatedString);
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

    public enum MappingFunctionType
    {
        None = 0,
        First,
        SumInt,
    }

    public class ExportSourceMapping
    {
        public static string ElementName = "Mapping";

        /// <summary>
        /// 来源列名
        /// </summary>
        public string SourceName { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string FieldName { set; get; }
        /// <summary>
        /// 目标列名
        /// </summary>
        public string TargetColumnName { set; get; }
        /// <summary>
        /// 转换类型
        /// </summary>
        public MappingFunctionType FunctionType { set; get; }
        /// <summary>
        /// 关联项目
        /// </summary>
        public ExportSourceMappingRelatedBy RelatedBy { set; get; }
        /// <summary>
        /// 条件转换
        /// </summary>
        public List<ExportSourceMappingWhere> Wheres { set; get; }
        /// <summary>
        /// 排序项目
        /// </summary>
        public ExportSourceMappingOrderBy OrderBy { set; get; }

        public ExportSourceMapping(XElement element)
        {
            SourceName = element.Attribute(nameof(SourceName))?.Value;
            FieldName = element.Attribute(nameof(FieldName))?.Value;
            TargetColumnName = element.Attribute(nameof(TargetColumnName))?.Value;
            FunctionType = element.Attribute(nameof(FunctionType))?.Value.ToEnum<MappingFunctionType>() ?? MappingFunctionType.None;
            RelatedBy = element.Descendants(ExportSourceMappingRelatedBy.ElementName).Select(c => new ExportSourceMappingRelatedBy(c)).FirstOrDefault();
            Wheres = element.Descendants(ExportSourceMappingWhere.ElementName).Select(c => new ExportSourceMappingWhere(c)).ToList();
            OrderBy = element.Descendants(ExportSourceMappingOrderBy.ElementName).Select(c => new ExportSourceMappingOrderBy(c)).FirstOrDefault();
        }
    }

    public class ExportSourceMappingOrderBy
    {
        public static string ElementName = "MappingOrderBy";

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { set; get; }
        /// <summary>
        /// 是否正序
        /// </summary>
        public bool IsAsc { set; get; }

        public ExportSourceMappingOrderBy(XElement element)
        {
            ColumnName = element.Attribute(nameof(ColumnName))?.Value;
            IsAsc = element.Attribute(nameof(IsAsc))?.Value.ToBool() ?? true;
        }
    }

    public class ExportSourceMappingWhere
    {
        public static string ElementName = "MappingWhere";

        /// <summary>
        /// 来源列名
        /// </summary>
        public string ColumnName { set; get; }
        /// <summary>
        /// 来源列名
        /// </summary>
        public DataType DataType { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public OperatorType Operator { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Value { set; get; }

        public ExportSourceMappingWhere(XElement element)
        {
            ColumnName = element.Attribute(nameof(ColumnName))?.Value;
            DataType = element.Attribute(nameof(DataType))?.Value.ToEnum<DataType>() ?? DataType.None;
            Operator = element.Attribute(nameof(Operator))?.Value.ToEnum<OperatorType>() ?? OperatorType.None;
            Value = element.Attribute(nameof(Value))?.Value;
        }
    }

    public class ExportSourceMappingRelatedBy
    {
        public static string ElementName = "MappingRelatedBy";

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { set; get; }
        /// <summary>
        /// 列名
        /// </summary>
        public string RelatedColumnName { set; get; }
        /// <summary>
        /// 来源列名
        /// </summary>
        public DataType DataType { set; get; }

        public ExportSourceMappingRelatedBy(XElement element)
        {
            ColumnName = element.Attribute(nameof(ColumnName))?.Value;
            RelatedColumnName = element.Attribute(nameof(RelatedColumnName))?.Value;
            DataType = element.Attribute(nameof(DataType))?.Value.ToEnum<DataType>() ?? DataType.None;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SQLConfigV2Where
    {
        /// <summary>
        /// 
        /// </summary>
        public const string ElementName = "Where";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public SQLConfigV2Where(XElement element)
        {
            ComponentName = element.Attribute(nameof(ComponentName))?.Value;
            Formatter = element.Attribute(nameof(Formatter))?.Value;
            DataType = element.Attribute(nameof(DataType))?.Value;
            IsOn = element.Attribute(nameof(IsOn))?.Value.ToBool() ?? false;
            Required = element.Attribute(nameof(Required))?.Value.ToBool() ?? false;
            SQL = element.Value;
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsOn { set; get; }
        /// <summary>
        /// 是否必须
        /// </summary>
        public bool Required { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { set; get; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ComponentName { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public string SQL { set; get; }
        /// <summary>
        /// 格式化
        /// </summary>
        public string Formatter { get; set; }
        /// <summary>
        /// 字段处理类型
        /// </summary>
        public string DataType { get; set; }
    }

    public class SQLConfigV2SQLs
    {
        public List<string> Texts { set; get; }
        public string UnitedBy { set; get; }

        public SQLConfigV2SQLs()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public SQLConfigV2SQLs(XElement element)
        {
            Texts = element.Descendants("SQL").Select(c => c.ToString().TrimStart("<SQL>").TrimEnd("</SQL>")).ToList();
            UnitedBy = element.Attribute(nameof(UnitedBy))?.Value ?? "";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SQLConfigOrderBy
    {
        /// 
        /// </summary>
        public const string RootElementName = "OrderBys";
        /// <summary>
        /// 
        /// </summary>
        public const string ElementName = "OrderBy";

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsOn { set; get; }
        /// <summary>
        /// 是否正序
        /// </summary>
        public bool IsAsc { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string ComponentName { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Alias { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public SQLConfigOrderBy(XElement element)
        {
            ComponentName = element.Attribute(nameof(ComponentName))?.Value;
            Alias = element.Attribute(nameof(Alias))?.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XElement ToXElement()
        {
            var property = new XElement(ElementName);
            property.SetAttributeValue(nameof(ComponentName), ComponentName);
            property.SetAttributeValue(nameof(Alias), Alias);
            return property;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SQLConfigV2Property
    {
        /// <summary>
        /// 
        /// </summary>
        public const string ElementName = "Property";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public SQLConfigV2Property(XElement element)
        {
            IsOn = element.Attribute(nameof(IsOn))?.Value.ToBool() ?? true;
            ColumnName = element.Attribute(nameof(ColumnName))?.Value;
            Alias = element.Attribute(nameof(Alias))?.Value;
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsOn { set; get; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string ColumnName { set; get; }
        /// <summary>
        /// 查询用名称
        /// </summary>
        public string Alias { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XElement ToXElement()
        {
            var property = new XElement(ElementName);
            property.SetAttributeValue(nameof(ColumnName), ColumnName);
            property.SetAttributeValue(nameof(Alias), Alias);
            return property;
        }
    }

    public enum SourceType
    {
        None = 0,
        Field,
        JsonList,
    }
    public enum TransfromFunctionType
    {
        None = 0,
        Case,
        SumInt,
        SumCase,
        Join,
        JoinCase,
    }
    public class ExportSourceTransformV2
    {
        public static string ElementName = "Transform";

        /// <summary>
        /// 来源列名
        /// </summary>
        public string ColumnName { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string SubFieldName { set; get; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public SourceType SourceType { set; get; }
        /// <summary>
        /// 转换类型
        /// </summary>
        public TransfromFunctionType FunctionType { set; get; }
        /// <summary>
        /// Case; TransfromFunctionType.Join 分隔符
        /// </summary>
        public string Splitter { set; get; }
        /// <summary>
        /// 目标列名
        /// </summary>
        public string TargetColumnName { set; get; }
        public List<ExportSourceTransformCase> Cases { set; get; }

        public ExportSourceTransformV2(XElement element)
        {
            ColumnName = element.Attribute(nameof(ColumnName))?.Value;
            SubFieldName = element.Attribute(nameof(SubFieldName))?.Value;
            SourceType = element.Attribute(nameof(SourceType))?.Value.ToEnum<SourceType>() ?? SourceType.None;
            FunctionType = element.Attribute(nameof(FunctionType))?.Value.ToEnum<TransfromFunctionType>() ?? TransfromFunctionType.None;
            Splitter = element.Attribute(nameof(Splitter))?.Value;
            TargetColumnName = element.Attribute(nameof(TargetColumnName))?.Value;
            Cases = element.Descendants(ExportSourceTransformCase.ElementName).Select(c => new ExportSourceTransformCase(c)).ToList();
        }
    }

    public enum DataType
    {
        None = 0,
        Default = 1,
        String = 2,
        Int = 3,
        DateTime = 4,
    }

    public enum OperatorType
    {
        None = 0,
        eq,// =        
        neq,
        gt,// >
        lt,// <
    }

    public class ExportSourceTransformCase
    {
        public static string ElementName = "Case";

        /// <summary>
        /// 来源列名
        /// </summary>
        public string ColumnName { set; get; }
        /// <summary>
        /// 来源列名
        /// </summary>
        public DataType DataType { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string When { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Then { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public OperatorType Operator { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Value { set; get; }

        public ExportSourceTransformCase(XElement element)
        {
            ColumnName = element.Attribute(nameof(ColumnName))?.Value;
            DataType = element.Attribute(nameof(DataType))?.Value.ToEnum<DataType>() ?? DataType.None;
            When = element.Attribute(nameof(When))?.Value;
            Then = element.Attribute(nameof(Then))?.Value;
            Operator = element.Attribute(nameof(Operator))?.Value.ToEnum<OperatorType>() ?? OperatorType.None;
            Value = element.Attribute(nameof(Value))?.Value;
        }
    }
}
