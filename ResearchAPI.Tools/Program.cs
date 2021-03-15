using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Autobots.Infrastracture.Common.ValuesSolution;
using Dapper;
using Dapper.Contrib.Extensions;
using ResearchAPI.CORS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace ResearchAPI.Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandCollection cmds = new CommandCollection();
            cmds.Add(new Command("ls", () =>
            {
                foreach (var cmd in cmds)
                {
                    Console.WriteLine(cmd.Name);
                }
            }));
            cmds.Add(new Command("i1 for 科研表结构初始化", () =>
            {
                var configs = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Configs", "config.json")).FromJson<DBConfig>();
                var files = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "Docs/SQLs"));
                var dBContext = DBHelper.GetDbContext(configs.ConnectionStrings.First(c => c.Key == "ResearchConnectionString").Value);
                foreach (var file in files)
                {
                    var sql = File.ReadAllText(file);
                    Console.WriteLine($"正在执行{file}");
                    dBContext.Execute(sql);
                    Console.WriteLine($"执行成功");
                }
            }));
            cmds.Add(new Command("s1 for 数据初始化", () =>
            {
                var configs = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Configs", "config.json")).FromJson<DBConfig>();
                var sourceDBContext = DBHelper.GetDbContext(configs.ConnectionStrings.First(c => c.Key == "SourceData").Value);
                var targetDBContext = DBHelper.GetDbContext(configs.ConnectionStrings.First(c => c.Key == "TargetData").Value);
                var researchDBContext = DBHelper.GetDbContext(configs.ConnectionStrings.First(c => c.Key == "ResearchConnectionString").Value);
                var businessEntities = ConfigHelper.GetCOSyncEntities(Path.Combine(Environment.CurrentDirectory, "Configs/XMLConfigs/SyncEntities"), "SyncEntities_产科.xml");
                businessEntities.AddRange(ConfigHelper.GetCOSyncEntities(Path.Combine(Environment.CurrentDirectory, "Configs/XMLConfigs/SyncEntities"), "SyncEntities_检验.xml"));
                businessEntities.AddRange(ConfigHelper.GetCOSyncEntities(Path.Combine(Environment.CurrentDirectory, "Configs/XMLConfigs/SyncEntities"), "SyncEntities_盆底.xml"));

                //TODO 希望基于现有表 跳过已存在表的处理

                foreach (var businessEntity in businessEntities)
                {
                    var fromTable = businessEntity.SourceName;
                    var toTable = businessEntity.TargetName;
                    Console.WriteLine($"正在执行初始化{targetDBContext.DbGroup.Connection.Database}.dbo.{toTable}=>{sourceDBContext.DbGroup.Connection.Database}.dbo.{fromTable}");

                    #region 表结构初始化
                    var manage = new SyncManage($"{targetDBContext.DbGroup.Connection.Database}.dbo.{toTable}"
                        , $"{sourceDBContext.DbGroup.Connection.Database}.dbo.{fromTable}"
                        , DateTime.Now
                        , null
                        , ""
                        , $"成功_数据表结构初始化"
                        , OperateType.InitTable
                        , OperateStatus.Success);
                    try
                    {
                        //前置校验
                        foreach (var property in businessEntity.Properties)
                        {
                            if (property.Enum.IsNullOrEmpty())
                            {
                                continue;
                            }
                            //TODO , "盆底" 希望清洗用的枚举是归整的
                            var json = ConfigHelper.GetJsonFileData(Path.Combine(Environment.CurrentDirectory, "Configs\\JsonConfigs", "盆底"), property.Enum);
                            if (json.IsNullOrEmpty())
                            {
                                throw new Exception(property.Enum + ",Json文件不存在");
                            }
                            var dic = json.FromJson<VLKeyValues>();
                            var maxLength = dic.Max(c => c.Key.Length);
                            if (property.MaxLength>0 && property.MaxLength < maxLength)
                            {
                                manage.Message += $"\r\n字段{property.SourceName}长度由于枚举,校准为{maxLength}";
                                property.MaxLength = maxLength;
                            }
                        }

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"CREATE TABLE [dbo].[{toTable}] (");
                        foreach (var property in businessEntity.Properties)
                        {
                            var columnName = property.SourceName;
                            var columnType = property.GetTargetColumnDefinition();
                            sb.AppendLine($"  [{columnName}] {columnType} NULL,");
                        }
                        sb.AppendLine($"[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP");
                        sb.AppendLine($")");
                        foreach (var property in businessEntity.Properties)
                        {
                            var columnName = property.SourceName;
                            var comment = property.DisplayName;
                            sb.AppendLine($"  EXEC sp_addextendedproperty 'MS_Description', N'{comment}','SCHEMA', N'dbo','TABLE', N'{toTable}','COLUMN', N'{columnName}'");
                        }
                        var sql = sb.ToString();
                        targetDBContext.Execute(sql);
                        Console.WriteLine(manage.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"失败");
                        manage.Message = ex.ToString();
                        manage.OperateStatus = OperateStatus.Error;
                    }
                    researchDBContext.Execute(manage.ToInsertSQL(), manage);
                    #endregion

                    #region 表数据同步
                    if (manage.Message.Contains("成功"))
                    {
                        manage = new SyncManage($"{targetDBContext.DbGroup.Connection.Database}.dbo.{toTable}"
                            , $"{sourceDBContext.DbGroup.Connection.Database}.dbo.{fromTable}"
                            , DateTime.Now
                            , null
                            , ""
                            , ""
                            , OperateType.InitData
                            , OperateStatus.Success);
                        try
                        {
                            var sql = $@"
insert into {targetDBContext.DbGroup.Connection.Database}.dbo.{businessEntity.TargetName} ({string.Join(",", businessEntity.Properties.Select(c => c.SourceName))}) 
select {string.Join(",", businessEntity.Properties.Select(c => c.SourceName))} from {sourceDBContext.DbGroup.Connection.Database}.dbo.{businessEntity.SourceName}";
                            targetDBContext.Execute(sql);
                            manage.Message += $"成功_数据表数据初始化";
                            Console.WriteLine(manage.Message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"失败");
                            manage.Message = ex.ToString();
                            manage.OperateStatus = OperateStatus.Error;
                        }
                        researchDBContext.Execute(manage.ToInsertSQL(), manage);
                    }
                    #endregion
                }
            }));
            cmds.Add(new Command("s2 for 数据清洗", () =>
            {
                var configs = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Configs", "config.json")).FromJson<DBConfig>();
                var sourceDBContext = DBHelper.GetDbContext(configs.ConnectionStrings.First(c => c.Key == "SourceData").Value);
                var targetDBContext = DBHelper.GetDbContext(configs.ConnectionStrings.First(c => c.Key == "TargetData").Value);
                var researchDBContext = DBHelper.GetDbContext(configs.ConnectionStrings.First(c => c.Key == "ResearchConnectionString").Value);
                var businessEntities = new COSyncEntities();
                //businessEntities.AddRange(ConfigHelper.GetCOSyncEntities(Path.Combine(Environment.CurrentDirectory, "Configs/XMLConfigs/SyncEntities"), "SyncEntities_产科.xml"));
                //businessEntities.AddRange(ConfigHelper.GetCOSyncEntities(Path.Combine(Environment.CurrentDirectory, "Configs/XMLConfigs/SyncEntities"), "SyncEntities_检验.xml"));
                businessEntities.AddRange(ConfigHelper.GetCOSyncEntities(Path.Combine(Environment.CurrentDirectory, "Configs/XMLConfigs/SyncEntities"), "SyncEntities_盆底.xml"));

                #region 数据清洗
                foreach (var businessEntity in businessEntities)
                {
                    var fromTable = businessEntity.SourceName;
                    var toTable = businessEntity.TargetName;
                    Console.WriteLine($"正在执行数据清洗{targetDBContext.DbGroup.Connection.Database}.dbo.{toTable}");

                    var manage = new SyncManage($"{targetDBContext.DbGroup.Connection.Database}.dbo.{toTable}"
                        , ""
                        , DateTime.Now
                        , null
                        , ""
                        , ""
                        , OperateType.DataTransform
                        , OperateStatus.Success);
                    try
                    {
                        bool hasError = false;
                        //前置校验
                        foreach (var property in businessEntity.Properties)
                        {
                            if (property.Enum.IsNullOrEmpty())
                            {
                                continue;
                            }
                            //TODO , "盆底" 希望清洗用的枚举是归整的
                            var json = ConfigHelper.GetJsonFileData(Path.Combine(Environment.CurrentDirectory, "Configs\\JsonConfigs", "盆底"), property.Enum);
                            if (json.IsNullOrEmpty())
                            {
                                throw new Exception(property.Enum + ",Json文件不存在");
                            }
                        }
                        //数据清洗
                        foreach (var property in businessEntity.Properties)
                        {
                            var propertyName = property.SourceName;
                            var propertyManage = new SyncManage($"{targetDBContext.DbGroup.Connection.Database}.dbo.{toTable}"
                                , $"{propertyName}"
                                , DateTime.Now
                                , null
                                , ""
                                , ""
                                , OperateType.EnumTransform
                                , OperateStatus.Success);
                            if (property.Enum.IsNullOrEmpty() || property.IsEnumText)
                            {
                                continue;
                            }
                            //TODO , "盆底" 希望清洗用的枚举是归整的
                            var json = ConfigHelper.GetJsonFileData(Path.Combine(Environment.CurrentDirectory, "Configs\\JsonConfigs", "盆底"), property.Enum);
                            var dic = json.FromJson<VLKeyValues>();
                            try
                            {
                                var sql = $@"update {targetDBContext.DbGroup.Connection.Database}.dbo.{toTable} set {propertyName} = {dic.ToCaseSQL(propertyName)}";
                                targetDBContext.Execute(sql);

                                propertyManage.Message = $@"成功,字段{propertyName}转换为枚举{property.Enum}";
                            }
                            catch (Exception ex2)
                            {
                                propertyManage.Message = $@"失败,{ex2.ToString()}";
                                propertyManage.OperateStatus = OperateStatus.Error;
                                hasError = true;
                            }
                            Console.WriteLine(propertyManage.Message);
                            researchDBContext.Execute(propertyManage.ToInsertSQL(), propertyManage);
                        }
                        manage.Message = hasError ? "失败" : "成功";
                    }
                    catch (Exception ex)
                    {
                        manage.Message = ex.ToString();
                        manage.OperateStatus = OperateStatus.Error;
                    }
                    Console.WriteLine(manage.Message);
                    researchDBContext.Execute(manage.ToInsertSQL(), manage);
                }
                #endregion
            }));
            cmds.Add(new Command("s3 for 数据统计预处理", () =>
            {
                var configs = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Configs", "config.json")).FromJson<DBConfig>();
                var sourceDBContext = DBHelper.GetDbContext(configs.ConnectionStrings.First(c => c.Key == "SourceData").Value);
                var targetDBContext = DBHelper.GetDbContext(configs.ConnectionStrings.First(c => c.Key == "TargetData").Value);
                var researchDBContext = DBHelper.GetDbContext(configs.ConnectionStrings.First(c => c.Key == "ResearchConnectionString").Value);
                var statisticsEntities = ConfigHelper.GetCOStatisticsEntities(Path.Combine(Environment.CurrentDirectory, "Configs/XMLConfigs/SyncEntities"), "StatisticsEntities_产科.xml");
                var dataStatisticsRepository = new DataStatisticsRepository(researchDBContext);
                foreach (var statisticsEntity in statisticsEntities)
                {
                    var paras = statisticsEntity.Parameters?.ToDictionary(",", "=");
                    var parent = statisticsEntity.ParentFormatter?.Replace(paras) ?? "";
                    var dataStatisticsCategory = statisticsEntity.Id.ToEnum<DataStatisticsCategory>();
                    var dataStatistics = new DataStatistics()
                    {
                        Name = dataStatisticsCategory.GetDescription(),
                        Category = dataStatisticsCategory,
                        IssueTime = DateTime.Now,
                    };
                    if (dataStatisticsRepository.GetByCategory(dataStatistics.Category, parent) != null)
                    {
                        //Console.WriteLine($"处理重复,统计分类{dataStatistics.Category.GetDescription()}");
                        continue;
                    }
                    try
                    {

                        var sql = statisticsEntity.SQLEntity.GetSQL(null);
                        if (sql.IsNullOrEmpty())
                        {
                            throw new NotImplementedException("缺少sql执行脚本,Id:" + statisticsEntity.Id);
                        }
                        var value = targetDBContext.ExecuteScalar(sql, paras);
                        dataStatistics.Value = value.ToString();
                        dataStatistics.Parent = parent;
                        Console.WriteLine($"处理成功,统计分类{dataStatistics.Category.GetDescription()} {parent}");
                    }
                    catch (Exception ex2)
                    {
                        dataStatistics.Message = ex2.ToString();
                        Console.WriteLine($"处理失败,统计分类{dataStatistics.Category.GetDescription()}");
                    }
                    researchDBContext.Execute(dataStatistics.ToInsertSQL(), dataStatistics);
                }
            }));
            cmds.Start();
        }
    }

    public class DataStatisticsRepository : Repository<DataStatistics>
    {
        public DataStatisticsRepository(DbContext context) : base(context)
        {
        }

        internal DataStatistics GetByCategory(DataStatisticsCategory category, string parent)
        {
            return context.DbGroup.Connection.Query<DataStatistics>($"select * from [{nameof(DataStatistics)}] where Category = @Category and Parent = @parent;"
                , new { category, parent }, transaction: _transaction).FirstOrDefault();
        }
    }

    public static class EntityEx
    {
        public static string ToInsertSQL(this object obj)
        {
            StringBuilder sb = new StringBuilder();
            var type = obj.GetType();
            var fields = new List<string>();
            var valueFields = new List<string>();
            foreach (System.Reflection.PropertyInfo info in type.GetProperties())
            {
                var attributes = info.GetCustomAttributes(true);
                if (attributes.Any(c => c is Dapper.Contrib.Extensions.KeyAttribute))
                {
                    continue;
                }
                else
                {
                    fields.Add($"[{info.Name}]");
                    valueFields.Add($"@{info.Name}");
                }
            }
            sb.Append($"insert into {type.Name}({fields.ToArrayString(",")}) values({valueFields.ToArrayString(",")})");
            return sb.ToString();
        }
    }

    public class CommandCollection : List<Command>
    {
        public void Start()
        {
            Console.WriteLine("wait for a command,enter `q` to close");
            string s = "ls";
            do
            {
                var command = this.FirstOrDefault(c => c.Name.StartsWith(s));
                if (command == null)
                {
                    Console.WriteLine("wait for a command,enter `q` to close");
                    continue;
                }
                try
                {
                    command.Execute();
                }
                catch (Exception e
                )
                {
                    var error = e;
                    Console.WriteLine(e.ToString());
                }
                Console.WriteLine("wait for a command,enter `q` to close");
            }
            while ((s = Console.ReadLine().ToLower()) != "q");
        }
    }

    public class Command
    {
        public Command(string name, Action exe)
        {
            Name = name.ToLower();
            Execute = exe;
        }

        public string Name { set; get; }

        public Action Execute { set; get; }
    }

    /// <summary>
    /// 配置样例
    /// </summary>
    public class DBConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public List<VLKeyValue> ConnectionStrings { set; get; }
    }
}
