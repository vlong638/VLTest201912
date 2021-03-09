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
                            var json = ConfigHelper.GetJsonFileData(Path.Combine(Environment.CurrentDirectory, "Configs\\JsonConfigs", businessEntities.BusinessType), property.Enum);
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
                var businessEntities = ConfigHelper.GetCOSyncEntities(Path.Combine(Environment.CurrentDirectory, "Configs/XMLConfigs/SyncEntities"), "SyncEntities_产科.xml");

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
                            var json = ConfigHelper.GetJsonFileData(Path.Combine(Environment.CurrentDirectory, "Configs\\JsonConfigs", businessEntities.BusinessType), property.Enum);
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
                            var json = ConfigHelper.GetJsonFileData(Path.Combine(Environment.CurrentDirectory, "Configs\\JsonConfigs", businessEntities.BusinessType), property.Enum);
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
                    var dataStatisticsCategory = statisticsEntity.Id.ToEnum<DataStatisticsCategory>();
                    var dataStatistics = new DataStatistics()
                    {
                        Name = dataStatisticsCategory.GetDescription(),
                        Category = dataStatisticsCategory,
                        IssueTime = DateTime.Now,
                    };
                    if (dataStatisticsRepository.GetByCategory(dataStatistics.Category) != null)
                    {
                        //Console.WriteLine($"处理重复,统计分类{dataStatistics.Category.GetDescription()}");
                        continue;
                    }
                    try
                    {

                        var paras = statisticsEntity.Parameters?.ToDictionary(",", "=");
                        var sql = statisticsEntity.SQLEntity.GetSQL(null);
                        if (sql.IsNullOrEmpty())
                        {
                            throw new NotImplementedException("缺少sql执行脚本,Id:" + statisticsEntity.Id);
                        }
                        var value = targetDBContext.ExecuteScalar(sql, paras);
                        dataStatistics.Value = value.ToString();
                        dataStatistics.Parent = statisticsEntity.ParentFormatter?.Replace(paras);
                        Console.WriteLine($"处理成功,统计分类{dataStatistics.Category.GetDescription()}");
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

        internal DataStatistics GetByCategory(DataStatisticsCategory category)
        {
            return context.DbGroup.Connection.Query<DataStatistics>($"select * from [{nameof(DataStatistics)}] where Category = @Category;"
                , new { category }, transaction: _transaction).FirstOrDefault();
        }
    }

    [Table("DataStatistics")]
    public class DataStatistics
    { 
        [Key]
        public long Id { set; get; }
        public string Name { set; get; }
        public DataStatisticsCategory Category { set; get; }
        public string Value { set; get; }
        public DateTime IssueTime { set; get; }
        public string Parent { set; get; }
        public string Message { set; get; }
    }

    public enum DataStatisticsCategory
    {
        #region PregnantInfo, 101001

        /// <summary>
        /// 产妇总数
        /// </summary>
        [Description("产妇总数")]
        PT_PatientCount = 101001001,
        /// <summary> 
        ///  档案总数
        /// </summary>
        [Description("档案总数")]
        PT_PregnantRecordCount = 101001002,

        /// <summary> 
        ///  建册年龄分布 lt20  @createtime,birthday 
        /// </summary>
        [Description("建册年龄分布 lt20  @createtime,birthday")]
        PT_CreateBookAge_lt20_Count = 101001003,
        /// <summary> 
        ///  建册年龄分布 20-29  @createtime,birthday 
        /// </summary>
        [Description("建册年龄分布 20-29  @createtime,birthday")]
        PT_CreateBookAge_20_29_Count = 101001004,
        /// <summary> 
        ///  建册年龄分布 30-34  @createtime,birthday 
        /// </summary>
        [Description("建册年龄分布 30-34  @createtime,birthday")]
        PT_CreateBookAge_30_34_Count = 101001005,
        /// <summary> 
        ///  建册年龄分布 35-39  @createtime,birthday 
        /// </summary>
        [Description("建册年龄分布 35-39  @createtime,birthday")]
        PT_CreateBookAge_35_39_Count = 101001006,
        /// <summary> 
        ///  建册年龄分布 40-44  @createtime,birthday 
        /// </summary>
        [Description("建册年龄分布 40-44  @createtime,birthday")]
        PT_CreateBookAge_40_44_Count = 101001007,
        /// <summary> 
        ///  建册年龄分布 gt44  @createtime,birthday 
        /// </summary>
        [Description("建册年龄分布 gt44  @createtime,birthday")]
        PT_CreateBookAge_gt_44_Count = 101001008,
        #endregion

        #region MHC_VisitRecord,101003

        /// <summary> 
        ///  病历总数 
        /// </summary>
        [Description("病历总数")]
        PT_MHC_VisitRecordCount = 101003001,

        #endregion

        #region MHC_VisitRecord_Monthly,101103

        /// <summary> 
        ///  病历总数 月周期 
        /// </summary>
        [Description("病历总数 月周期")]
        PT_MHC_VisitRecordCount_Monthly = 101103001,

        #endregion

        #region MHC_HighRiskReason,101007

        /// <summary> 
        ///  五色高危分布_绿色 
        /// </summary>
        [Description("五色高危分布_绿色")]
        PT_RiskLevel_Green_Count = 101007001,
        /// <summary> 
        ///  五色高危分布_黄色 
        /// </summary>
        [Description("五色高危分布_黄色")]
        PT_RiskLevel_Yellow_Count = 101007002,
        /// <summary> 
        ///  五色高危分布_橙色 
        /// </summary>
        [Description("五色高危分布_橙色")]
        PT_RiskLevel_Orange_Count = 101007003,
        /// <summary> 
        ///  五色高危分布_红色 
        /// </summary>
        [Description("五色高危分布_红色")]
        PT_RiskLevel_Red_Count = 101007004,
        /// <summary> 
        ///  五色高危分布_紫色 
        /// </summary>
        [Description("五色高危分布_紫色")]
        PT_RiskLevel_Violet_Count = 101007005,

        #endregion

        #region CDH_NeonateRecord 101008

        /// <summary> 
        ///  新生儿总数 
        /// </summary>
        [Description("新生儿总数")]
        PT_ChildCount = 101008001,
        /// <summary> 
        ///  新生儿档案总数 
        /// </summary>
        [Description("新生儿档案总数")]
        PT_ChildRecordCount = 101008002,
        /// <summary> 
        ///  新生儿男孩总数 
        /// </summary>
        [Description("新生儿男孩总数")]
        PT_BoyCount = 101008004,
        /// <summary> 
        ///  新生儿女孩总数 
        /// </summary>
        [Description("新生儿女孩总数")]
        PT_GirlCount = 101008005,

        #endregion

        #region CDH_NeonateRecord 101108

        /// <summary> 
        ///  新生儿档案总数月周期 
        /// </summary>
        [Description("新生儿档案总数月周期")]
        PT_ChildRecordCount_Monthly = 101108002,

        #endregion

        #region CDH_DeliveryRecord 101009

        /// <summary> 
        ///  已分娩产妇总数 
        /// </summary>
        [Description("已分娩产妇总数")]
        PT_MotherCount = 101009001,
        /// <summary> 
        ///  待分娩产妇总数 
        /// </summary>
        [Description("待分娩产妇总数")]
        PT_PregnantCount = 101009002,
        /// <summary> 
        ///  流产产妇总数 
        /// </summary>
        [Description("流产产妇总数")]
        PT_AbortionCount = 101009003,

        /// <summary> 
        ///  分娩结局`存活`母亲总数 @fenmianjjmc 
        ///  CDH_DeliveryRecord: 553773
        ///  PregnantInfo: 1174824
        ///  342148 = 13s
        /// </summary>
        [Description("分娩结局`存活`母亲总数 @fenmianjjmc")]
        PT_PregnancyOutcomeAliveCount = 101009004,
        /// <summary> 
        ///  分娩结局`死胎`母亲总数 @fenmianjjmc 
        /// </summary>
        [Description("分娩结局`死胎`母亲总数 @fenmianjjmc")]
        PT_PregnancyOutcomeStillBirthCount = 101009005,
        /// <summary> 
        ///  分娩结局`死产`母亲总数 @fenmianjjmc 
        /// </summary>
        [Description("分娩结局`死产`母亲总数 @fenmianjjmc")]
        PT_PregnancyOutcomeDeadBirthCount = 101009006,

        /// <summary> 
        ///  母亲结局`产时死亡`母亲总数 @chanfujjmc 
        /// </summary>
        [Description("母亲结局`产时死亡`母亲总数 @chanfujjmc")]
        PT_MotherOutcomeDeadBirthCount = 101009007,
        /// <summary> 
        ///  母亲结局`存活`母亲总数 @chanfujjmc 
        /// </summary>
        [Description("母亲结局`存活`母亲总数 @chanfujjmc")]
        PT_MotherOutcomeAliveCount = 101009008,

        //<=27周+6天
        //>=28周  and <=36周+6天
        //>=37周  and <41周+6天
        //>=42

        /// <summary> 
        ///  分娩孕周 小于28周 
        /// </summary>
        [Description("分娩孕周 小于28周")]
        PT_Boy_DeliveryWeekCount_lt28 = 101009011,
        /// <summary> 
        ///  分娩孕周 28周-37周 
        /// </summary>
        [Description("分娩孕周 28周-37周")]
        PT_Boy_DeliveryWeekCount_28_37 = 101009012,
        /// <summary> 
        ///  分娩孕周 37周-41周 
        /// </summary>
        [Description("分娩孕周 37周-41周")]
        PT_Boy_DeliveryWeekCount_37_41 = 101009013,
        /// <summary> 
        ///  分娩孕周 大于41周 
        /// </summary>
        [Description("分娩孕周 大于41周")]
        PT_Boy_DeliveryWeekCount_gt41 = 101009014,

        /// <summary> 
        ///  分娩孕周 小于28周 
        /// </summary>
        [Description("分娩孕周 小于28周")]
        PT_Girl_DeliveryWeekCount_lt28 = 101009015,
        /// <summary> 
        ///  分娩孕周 28周-37周 
        /// </summary>
        [Description("分娩孕周 28周-37周")]
        PT_Girl_DeliveryWeekCount_28_37 = 101009016,
        /// <summary> 
        ///  分娩孕周 37周-41周 
        /// </summary>
        [Description("分娩孕周 37周-41周")]
        PT_Girl_DeliveryWeekCount_37_41 = 101009017,
        /// <summary> 
        ///  分娩孕周 大于41周 
        /// </summary>
        [Description("分娩孕周 大于41周")]
        PT_Girl_DeliveryWeekCount_gt41 = 101009018,


        /// <summary> 
        ///  分娩方式`产钳助产` @fenmianfsmc 
        /// </summary>
        [Description("分娩方式`产钳助产` @fenmianfsmc")]
        PT_DeliveryMode_ForcepsCount = 101009020,
        /// <summary> 
        ///  分娩方式`毁胎术` @fenmianfsmc 
        /// </summary>
        [Description("分娩方式`毁胎术` @fenmianfsmc")]
        PT_DeliveryMode_DestroyCount = 101009021,
        /// <summary> 
        ///  分娩方式`剖宫产` @fenmianfsmc 
        /// </summary>
        [Description("分娩方式`剖宫产` @fenmianfsmc")]
        PT_DeliveryMode_CaesareanCount = 101009022,
        /// <summary> 
        ///  分娩方式`其他` @fenmianfsmc 
        /// </summary>
        [Description("分娩方式`其他` @fenmianfsmc")]
        PT_DeliveryMode_OtherCount = 101009023,
        /// <summary> 
        ///  分娩方式`其它` @fenmianfsmc 
        /// </summary>
        [Description("分娩方式`其它` @fenmianfsmc")]
        PT_DeliveryMode_Other2Count = 101009024,
        /// <summary> 
        ///  分娩方式`胎头吸引` @fenmianfsmc 
        /// </summary>
        [Description("分娩方式`胎头吸引` @fenmianfsmc")]
        PT_DeliveryMode_ExtractionCount = 101009025,
        /// <summary> 
        ///  分娩方式`臀位助产` @fenmianfsmc 
        /// </summary>
        [Description("分娩方式`臀位助产` @fenmianfsmc")]
        PT_DeliveryMode_BreechAssistCount = 101009026,
        /// <summary> 
        ///  分娩方式`阴道自然分娩` @fenmianfsmc 
        /// </summary>
        [Description("分娩方式`阴道自然分娩` @fenmianfsmc")]
        PT_DeliveryMode_VaginalCount = 101009027,
        /// <summary> 
        ///  分娩方式`治疗性引产` @fenmianfsmc 
        /// </summary>
        [Description("分娩方式`治疗性引产` @fenmianfsmc")]
        PT_DeliveryMode_InducedCount = 101009028,

        #endregion

        #region LabOrder 199001

        /// <summary> 
        ///  检验记录数 
        /// </summary>
        [Description("检验记录数")]
        Common_LabOrderCount = 199001001,

        #endregion

        #region LabResult 199002

        /// <summary> 
        ///  检验记录项目数 
        /// </summary>
        [Description("检验记录项目数")]
        Common_LabResultCount = 199002001,

        #endregion
    }

    [Table("SyncManage")]
    public class SyncManage
    {
        public SyncManage(string from, string to, DateTime issueTime, DateTime? latestDataTime, string latestDataField, string message, OperateType operateType, OperateStatus operateStatus)
        {
            From = from;
            To = to;
            IssueTime = issueTime;
            LatestDataTime = latestDataTime;
            LatestDataField = latestDataField;
            Message = message;
            OperateType = operateType;
            OperateStatus = operateStatus;
        }

        [Key]
        public long Id { set; get; }
        public string From { set; get; }
        public string To { set; get; }
        public OperateType OperateType { set; get; }
        public OperateStatus OperateStatus { set; get; }
        public DateTime IssueTime { set; get; }
        public DateTime? LatestDataTime { set; get; }
        public string LatestDataField { set; get; }
        public string Message { set; get; }
    }

    public enum OperateStatus
    {
        None = 0,
        Success = 1,
        Error = 2,
    }

    public enum OperateType
    {
        None = 0,
        /// <summary>
        /// 表结构初始化
        /// </summary>
        InitTable = 1,
        /// <summary>
        /// 数据初始化
        /// </summary>
        InitData = 2,
        /// <summary>
        /// 数据转换
        /// </summary>
        DataTransform = 3,
        /// <summary>
        /// 枚举转换
        /// </summary>
        EnumTransform = 4,
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
