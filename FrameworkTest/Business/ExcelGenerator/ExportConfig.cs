using Dapper;
using FrameworkTest.Common.DALSolution;
using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.ExcelSolution;
using FrameworkTest.Common.ValuesSolution;
using NPOI.SS.Formula;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace FrameworkTest.Business.ExcelGenerator
{
    /// <summary>
    /// 查询sql配置
    /// </summary>
    public class ExportConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public static string NodeElementName = "Export";

        /// <summary>
        /// 导出标识名
        /// </summary>
        public string ExportName { set; get; }
        /// <summary>
        /// 输出文件名
        /// </summary>
        public string FileName { set; get; }
        /// <summary>
        /// 输出数据源
        /// </summary>
        public List<ExportSheet> Sheets { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public ExportConfig()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ExportConfig(XElement element)
        {
            ExportName = element.Attribute(nameof(ExportName)).Value;
            FileName = element.Attribute(nameof(FileName)).Value;
            Sheets = element.Descendants(ExportSheet.NodeElementName).Select(c => new ExportSheet(c)).ToList();
        }
    }
    /// <summary>
    /// 查询sql配置
    /// </summary>
    public class ExportSheet
    {
        /// <summary>
        /// 
        /// </summary>
        public static string NodeElementName = "Sheet";

        /// <summary>
        /// Sheet名称
        /// </summary>
        public string SheetName { set; get; }
        /// <summary>
        /// 输出数据源
        /// </summary>
        public List<ExportSource> Sources { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public ExportSheet()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ExportSheet(XElement element)
        {
            SheetName = element.Attribute(nameof(SheetName)).Value;
            Sources = element.Descendants(ExportSource.NodeElementName).Select(c => new ExportSource(c)).ToList();
        }

        internal void Render(ISheet sheet)
        {
            List<string> contentsToDeal = new List<string>();
            var rowsCount = sheet.LastRowNum;
            for (int i = 0; i < rowsCount; i++)
            {
                var row = sheet.GetRow(i);
                var columnsCount = row.LastCellNum;
                for (int j = 0; j < columnsCount; j++)
                {
                    var text = sheet.VLGetCellValueAsString(i, j) ?? "";
                    if (text.StartsWith("@"))
                    {
                        contentsToDeal.Add(text);
                        PlaceHolder holder = new PlaceHolder(text);
                        if (!DataSources.ContainsKey(holder.Source))
                            continue;
                        var talbe = (DataSources[holder.Source]);
                        if (talbe == null || !talbe.Columns.Contains(holder.Field))
                            continue;
                        if (holder.LoopAuto)
                        {
                            if (j == 0)
                            {
                                for (int n = 0; n < talbe.Rows.Count; n++)
                                {
                                    sheet.CreateRow(i + 1);
                                }
                            }
                            for (int n = 0; n < talbe.Rows.Count; n++)
                            {
                                var value = talbe.Rows[n][holder.Field]?.ToString();
                                sheet.VLSetCellValue(i + n, j, value);
                            }
                        }
                        else if (holder.Loop > 1)
                        {
                            for (int n = 0; n < holder.Loop; n++)
                            {
                                if (n + 1 > talbe.Rows.Count)
                                    break;

                                var value = talbe.Rows[n][holder.Field]?.ToString();
                                sheet.VLSetCellValue(i + n, j, value);
                            }
                        }
                        else
                        {
                            var value = talbe.Rows[0][holder.Field]?.ToString();
                            sheet.VLSetCellValue(i, j, value);
                        }
                    }
                }
            }
        }

        internal void UpdateWheres(List<KeyValue> wheres)
        {
            if (wheres == null)
                return;

            foreach (var sourceConfig in Sources)
            {
                foreach (var where in wheres)
                {
                    var whereConfig = sourceConfig.Wheres.FirstOrDefault(c => c.ComponentName.ToLower() == where.Key.ToLower());
                    if (!where.Value.IsNullOrEmpty())
                    {
                        whereConfig.IsOn = true;
                        whereConfig.Value = where.Value;
                    }
                }
            }
        }

        Dictionary<string, DataTable> DataSources { set; get; }
        internal void PrepareSourceData()
        {
            DataSources = new Dictionary<string, DataTable>();
            foreach (var sourceConfig in Sources)
            {
                var dbContext = new DbContext(DBHelper.GetSqlDbConnection("Data Source=192.168.50.102;Initial Catalog=HL_Pregnant;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=HZFYUSER;Password=HZFYPWD"));
                var sampleRespository = new SampleRespository(dbContext);
                var result = dbContext.DelegateTransaction((g) =>
                {
                    return sampleRespository.GetCommonSelect(sourceConfig);
                });
                DataSources[sourceConfig.SourceName] = result.Data;
            }
        }
    }
    /// <summary>
    /// 数据源
    /// </summary>
    public class ExportSource
    {
        /// <summary>
        /// 
        /// </summary>
        public const string NodeElementName = "Source";

        /// <summary>
        /// 数据源名称
        /// </summary>
        public string SourceName { set; get; }
        /// <summary>
        /// 页面字段
        /// </summary>
        public List<ExportSourceProperty> Properties { set; get; }
        /// <summary>
        /// 页面条件项
        /// </summary>
        public List<ExportSourceWhere> Wheres { set; get; }
        /// <summary>
        /// 页面排序项
        /// </summary>
        public List<ExportSourceOrderBy> OrderBys { set; get; }
        public string SQL { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ExportSource(XElement element)
        {
            SourceName = element.Attribute(nameof(SourceName))?.Value;
            SQL = element.Descendants("SQL").First().Value;
            Properties = element.Descendants(ExportSourceProperty.NodeElementName).Select(c => new ExportSourceProperty(c)).ToList();
            Wheres = element.Descendants(ExportSourceWhere.NodeElementName).Select(c => new ExportSourceWhere(c)).ToList();
            OrderBys = element.Descendants(ExportSourceOrderBy.NodeElementName).Select(c => new ExportSourceOrderBy(c)).ToList();
        }

        internal string GetListSQL()
        {
            var sql = SQL;
            var propertiesIsOn = Properties.Select(c => c.Alias);
            var fields = propertiesIsOn.Count() == 0 ? "*" : string.Join(",", propertiesIsOn);
            sql = sql.Replace("@Properties", fields);
            var wheresIsOn = Wheres.Where(c => c.IsOn).Select(c => c.SQL);
            var wheres = wheresIsOn.Count() == 0 ? "" : $"where {string.Join(" and ", wheresIsOn)}";
            sql = sql.Replace("@Wheres", wheres);
            var orderByIsOn = OrderBys.FirstOrDefault(c => c.IsOn) ?? OrderBys.First();
            var orderBy = orderByIsOn.Alias;
            var order = orderByIsOn.IsAsc ? "asc" : "desc";
            sql = sql.Replace("@OrderBy", $"order by {orderBy} {order}");
            return sql;
        }

        internal Dictionary<string, object> GetParams()
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
    }
    /// <summary>
    /// 
    /// </summary>
    public class ExportSourceProperty
    {
        /// <summary>
        /// 
        /// </summary>
        public const string NodeElementName = "Property";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ExportSourceProperty(XElement element)
        {
            ColumnName = element.Attribute(nameof(ColumnName))?.Value;
            Alias = element.Attribute(nameof(Alias))?.Value;
        }

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
            var property = new XElement(NodeElementName);
            property.SetAttributeValue(nameof(ColumnName), ColumnName);
            property.SetAttributeValue(nameof(Alias), Alias);
            return property;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ExportSourceWhere
    {
        /// <summary>
        /// 
        /// </summary>
        public const string NodeElementName = "Where";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ExportSourceWhere(XElement element)
        {
            ComponentName = element.Attribute(nameof(ComponentName))?.Value;
            Formatter = element.Attribute(nameof(Formatter))?.Value;
            IsOn = element.Attribute(nameof(IsOn))?.Value.ToBool() ?? false;
            SQL = element.Value;
        }

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

        #region compute
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsOn { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public object Value { set; get; } 
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XElement ToXElement()
        {
            var property = new XElement(NodeElementName);
            property.SetAttributeValue(nameof(ComponentName), ComponentName);
            property.SetAttributeValue(nameof(Formatter), Formatter);
            property.SetValue(SQL);
            return property;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ExportSourceOrderBy
    {
        /// <summary>
        /// 
        /// </summary>
        public const string NodeElementName = "OrderBy";

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
        public ExportSourceOrderBy(XElement element)
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
            var property = new XElement(NodeElementName);
            property.SetAttributeValue(nameof(ComponentName), ComponentName);
            property.SetAttributeValue(nameof(Alias), Alias);
            return property;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SampleRespository
    {
        DbContext Context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public SampleRespository(DbContext context)
        {
            Context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetCommonSelect(ExportSource config)
        {
            var sql = config.GetListSQL();
            var pars = config.GetParams();
            DataTable table = new DataTable("MyTable");
            var reader = Context.DbGroup.Connection.ExecuteReader(sql, pars, transaction: Context.DbGroup.Transaction);
            table.Load(reader);
            return table;
        }
    }
}
