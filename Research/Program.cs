using System;
using System.Collections.Generic;
using System.Data;
using VL.Consolo_Core.Common.ValuesSolution;

namespace Research
{


    class Program
    {
        static void Main(string[] args)
        {
            #region 思路及伪代码
            //表结构 Child
            //Id
            //PatientId
            //db_createtime
            //db_updatetime
            //Name
            //Birthday
            //Sex
            //IsDeleted

            //表结构 PregnantInfo
            //Id
            //Idcard
            //db_createtime
            //db_updatetime
            //Name
            //Birthday
            //Sex
            //IsDeleted

            //表结构 PatientLabOrder
            //Id
            //PatientId
            //db_createtime
            //db_updatetime
            //IsDeleted
            //IssueDate
            //Name

            //表结构 PatientLabResult
            //Id
            //OrderId
            //db_createtime
            //db_updatetime
            //IsDeleted
            //Name
            //Value

            //主体配置
            //新生儿      Child
            //孕妇        PregnantInfo
            //孕妇检查    PatientLabOrder
            //孕妇检验    PatientLabResult

            //叶子配置1
            //新生儿      Child
            //姓名        Name
            //生日        Birthday
            //性别        Sex

            //叶子配置2
            //孕妇        PregnantInfo
            //身份证      Idcard
            //姓名        Name
            //生日        Birthday
            //性别        Sex

            //路由配置1
            //RouteCP: Child - PregnantInfo 
            //= Child.PregnantInfo = PregnantInfo.Id

            //路由配置2
            //RoutePPCO: PregnantInfo - PatientLabOrder
            //= PatientLabOrder.PregnantInfoId =  PregnantInfo.Id

            //路由配置3
            //RoutePCOPCI: PatientLabOrder - PatientLabResult
            //= PatientLabResult.OrderId =  PatientLabOrder.Id

            //应用案例1:导出婴儿与孕妇的姓名
            //>>>选择主干
            //孕妇
            //>>>选择导出字段
            //姓名                所属孕妇
            //姓名                所属新生儿
            //=> 命中 RouteCP

            //应用案例2:导出孕妇的检验单的检验项的2小时空腹血糖低于xxx的新生儿的数量
            //>>>选择主干
            //婴儿
            //>>>选择导出字段
            //婴儿的数量
            //=> 命中 RouteCP+RoutePPCO+RoutePCOPCI

            //这里有一个问题: 怎么确保用户能够正确理解笛卡尔积 或者这里消除笛卡尔积对用户结果的影响 假如用户操作了复杂条件
            //这里有一个问题: 怎么确保用户能够正确理解笛卡尔积 或者这里消除笛卡尔积对用户结果的影响 假如用户操作了复杂条件
            //这里有一个问题: 怎么确保用户能够正确理解笛卡尔积 或者这里消除笛卡尔积对用户结果的影响 假如用户操作了复杂条件
            //小技巧 count(distinct(Name))
            //但其他的呢 

            //应用案例
            //输出孕妇的多项内容
            //>>>选择主干
            //孕妇
            //>>>选择导出字段
            //孕妇档案编号	        所属孕妇
            //社区孕妇档案编号      所属孕妇
            //分娩记录编号          所属孕妇
            //居住地址              所属孕妇
            //末次月经              所属孕妇
            //建档日期              所属孕妇
            //预产期                所属孕妇
            //分娩日期              所属孕妇
            //姓名                  所属孕妇
            //身份证号              所属孕妇
            //分娩单位              所属孕妇
            //是否纳入保健管理      所属孕妇
            //=> 命中PregnantInfo

            //应用案例
            //输出孕妇的多类检验结果
            //导出      
            //>>>选择主干
            //孕妇
            //>>>选择导出字段
            //12~16周血红蛋白       所属检验
            //12~16周铁蛋白值       所属检验   
            //分组查询 12~16周血红蛋白
            //命中 RouteCP+RoutePPCO+RoutePCOPCI
            //分组查询 12~16周铁蛋白值
            //命中 RouteCP+RoutePPCO+RoutePCOPCI
            //基于主体整合结果

            //应用案例
            //输出90后的婴儿的性别,体重,孕妇的年龄,姓名 12~16周的血红蛋白,12~16周的铁蛋白
            //导出      
            //>>>选择主干
            //孕妇
            //>>>选择导出字段
            //年龄,姓名
            //性别,体重
            //>>>维护输出项
            //12~16周的血红蛋白
            //12~16周的铁蛋白
            //>>>维护主体的条件项
            //孕妇的生日>=1990
            //执行查询
            //整合查询项目 孕妇的年龄,姓名
            //整合查询项目 婴儿的性别,体重
            //分组查询 孕妇的年龄,姓名
            //分组查询 婴儿的性别,体重
            //分组查询 12~16周血红蛋白
            //分组查询 12~16周铁蛋白值
            //基于主体整合结果 
            #endregion

            #region 连表查询案例
            //主体
            var viewEntities = new ViewEntitySet();
            viewEntities.Add(
                new ViewEntity("Child", "新生儿", new List<ViewEntityProperty>()
                   {
                       new ViewEntityProperty("Name","新生儿姓名"),
                       new ViewEntityProperty("Birthday","新生儿生日"),
                   }
                )
            );
            viewEntities.Add(
                new ViewEntity("PregnantInfo", "孕妇", new List<ViewEntityProperty>()
                   {
                       new ViewEntityProperty("Name","孕妇姓名"),
                       new ViewEntityProperty("Birthday","孕妇生日"),
                   }
                )
            );
            //主体范围条件
            var mainConditions = new ConditionSet();
            mainConditions.Conditions.Add(new Field2ValueCondition("Child", "Id", ConditionOperator.IsNotNull));//筛选有新生儿的患者
            mainConditions.Conditions.Add(new Field2ValueCondition("Child", "Name", ConditionOperator.Like, "vl", "%{0}%")); //模糊匹配

            //执行得到查询结果
            DataTable data1 = ReportHelper.GetReport(viewEntities, mainConditions);
            #endregion

            DataTable data2 = Get孕期16_28周的最近一次血红蛋白();
            DataTable data = ReportHelper.UnitData("Idcard", data1, data2);

            DataTable data3 = Get孕期最近一次空腹血糖();
            data = ReportHelper.UnitData("Idcard", data, data3);

            Console.WriteLine("Hello World!");
        }

        private static DataTable Get孕期16_28周的最近一次血红蛋白()
        {
            ///导出单元
            ///16~28周的最近一次血红蛋白
            ///翻译成业务语言:
            ///>>>血红蛋白
            ///根据检查项筛选血红蛋白的检查
            ///>>>16~28周 
            ///查询预产期,检查日期
            ///预产期>=检查日期换算成16周的周一日期
            ///预产期<=检查日期换算成28周的周日日期
            ///>>>最近一次 检查日期最大的一次
            ///根据孕妇分组,取最大的检查日期 形成临时数据集合(孕妇Id,最大的血红蛋白的检查日期)
            ///临时数据集合 关联取到最大的检查日期对应的元组
            ///取出元组的血红蛋白的检查数值

            //pregnant.dateofprenatal,laborder.examtime
            //pregnant.dateofprenatal>laborder.examtime
            //pregnant.dateofprenatal<laborder.examtime

            //`检验时间`小于`孕妇`的`分娩时间` 
            var reportEntity = new ReportEntity("16~28周的最近一次血红蛋白");
            reportEntity.ViewEntitySet.Add(
                new ViewEntity("PregnantInfo", "孕妇", new List<ViewEntityProperty>()
                   {
                       new ViewEntityProperty("Idcard","身份证"),
                       new ViewEntityProperty("DateOfPrenatal","预产期"),
                   }
                )
            );
            reportEntity.ViewEntitySet.Add(
                new ViewEntity("LabOrder", "检查单", new List<ViewEntityProperty>()
                   {
                       new ViewEntityProperty("ExamTime","检查日期"),
                   }
                )
            );
            reportEntity.ViewEntitySet.Add(
                new ViewEntity("LabResult", "检验项", new List<ViewEntityProperty>()
                   {
                       new ViewEntityProperty("ItemId","检验类别"),
                       new ViewEntityProperty("Value","检验结果"),
                   }
                )
            );

            //主体范围条件
            var mainConditions = new ConditionSet();
            mainConditions.Conditions.Add(new Field2ValueCondition("LabResult", "ItemId", ConditionOperator.Equal, "0148"));//筛选有新生儿的患者

            //路由设置
            reportEntity.Routers = new Routers();
            reportEntity.Routers.Add(new Router("PregnantInfo", "Child", RouteType.LeftJoin, new List<RouterOn>() {
                new RouterOn("PregnantInfo","Id", "Child","PregnantInfoId")
            }));

            //分组设置
            var groupSet = new GroupSet();
            groupSet.GroupBys.Add(new GroupBy("PregnantInfo", "Idcard"));
            groupSet.GroupSelects.Add(new GroupSelect("PregnantInfo", "Idcard"));
            groupSet.GroupSelects.Add(new GroupSelect("PregnantInfo", "Value", GroupSelectOperator.Max));

            //执行得到查询结果
            var data2 = ReportHelper.GetReport(reportEntity);
            return data2;
        }

        private static DataTable Get孕期最近一次空腹血糖()
        {
            //孕期最近一次但不超过七天的空腹血糖
            #region 完整sql
            //select temp.*,lr.itemid,lr.itemname,lr.value 
            //from
            //(
            //	select pi.idcard,pi.deliverydate,max(lo.examtime) maxexamtime 
            //	from LabOrder lo
            //	left join LabResult lr on lo.orderid = lr.orderid
            //	left join PregnantInfo pi on lo.idcard = pi.idcard
            //	where lr.itemid = '0148'
            //	and pi.deliverydate > lo.examtime
            //	and pi.deliverydate < dateadd(day,7,lo.examtime)
            //	group by pi.idcard,pi.deliverydate
            //) as temp
            //left join LabOrder lo on temp.idcard = lo.idcard and temp.maxexamtime = lo.examtime
            //left join LabResult lr on lo.orderid = lr.orderid
            //where lr.itemid = '0148' 
            //and temp.deliverydate > lo.examtime
            //and temp.deliverydate < dateadd(day, 7, lo.examtime)
            #endregion
            #region 拆接中间表方案,效率更高
            //构建中间表
            //select pi.idcard,pi.deliverydate,max(lo.examtime) maxexamtime 
            //into temp1227
            //from LabOrder lo
            //left join LabResult lr on lo.orderid = lr.orderid
            //left join PregnantInfo pi on lo.idcard = pi.idcard
            //where lr.itemid = '0148'
            //and pi.deliverydate > lo.examtime
            //and pi.deliverydate < dateadd(day,7,lo.examtime)
            //group by pi.idcard,pi.deliverydate
            //基于中间表关联查询
            //select temp.*,lr.itemid,lr.itemname,lr.value 
            //from temp1227 temp
            //left join LabOrder lo on temp.idcard = lo.idcard and temp.maxexamtime = lo.examtime
            //left join LabResult lr on lo.orderid = lr.orderid
            //where lr.itemid = '0148'
            //删除中间表
            //drop table temp1227 
            #endregion

            //`检验时间`小于`孕妇`的`分娩时间` 
            var reportEntity = new ReportEntity("孕期内最近一次但不超过七天的空腹血糖");
            reportEntity.ViewEntitySet.Add(
                new ViewEntity("PregnantInfo", "孕妇", new List<ViewEntityProperty>()
                    {
                        new ViewEntityProperty("Idcard","身份证"),
                        new ViewEntityProperty("DeliveryDate","分娩日期"),
                    }
                )
            );
            reportEntity.ViewEntitySet.Add(
                new ViewEntity("LabOrder", "检查单", new List<ViewEntityProperty>()
                   {
                       new ViewEntityProperty("ExamTime","检查日期"),
                   }
                )
            );
            reportEntity.ViewEntitySet.Add(
                new ViewEntity("LabResult", "检验项", new List<ViewEntityProperty>()
                   {
                       new ViewEntityProperty("ItemId","检验类别"),
                       new ViewEntityProperty("Value","检验结果"),
                   }
                )
            );

            //主体范围条件
            var mainConditions = new ConditionSet();
            mainConditions.Conditions.Add(new Field2ValueCondition("LabResult", "ItemId", ConditionOperator.Equal, "0148"));
            mainConditions.Conditions.Add(new Field2FieldCondition("PregnantInfo", "deliverydate", ConditionOperator.GreatThan, "LabOrder", "examtime"));
            mainConditions.Conditions.Add(new Field2FieldCondition("PregnantInfo", "deliverydate", ConditionOperator.LessThan, "LabOrder", "examtime",fieldName2CompareFormat: "dateadd(day,7,lo.examtime)"));

            //路由设置
            reportEntity.Routers = new Routers();
            reportEntity.Routers.Add(new Router("PregnantInfo", "LabOrder", RouteType.LeftJoin, new List<RouterOn>() {
                new RouterOn("PregnantInfo","Idcard", "LabOrder","Idcard")
            }));
            reportEntity.Routers.Add(new Router("LabOrder", "LabResult", RouteType.LeftJoin, new List<RouterOn>() {
                new RouterOn("LabOrder","OrderId", "LabResult","OrderId")
            }));

            //分组设置
            var groupSet = new GroupSet();
            groupSet.GroupBys.Add(new GroupBy("PregnantInfo", "Idcard"));
            groupSet.GroupSelects.Add(new GroupSelect("PregnantInfo", "Idcard"));
            groupSet.GroupSelects.Add(new GroupSelect("PregnantInfo", "Value", GroupSelectOperator.Max));

            //执行得到查询结果
            var data2 = ReportHelper.GetReport(reportEntity);
            return data2;
        }

        public enum GroupSelectOperator
        {
            None = 0,
            Max = 1,
            Min = 2,
        }
        /// <summary>
        /// 分组
        /// </summary>
        public class GroupSet
        {
            public List<GroupBy> GroupBys { set; get; }
            public List<GroupSelect> GroupSelects { set; get; }
        }
        /// <summary>
        /// 分组根据字段
        /// </summary>
        public class GroupBy
        {
            public GroupBy(string source, string fieldName)
            {
                Source = source;
                FieldName = fieldName;
            }

            public string Source { set; get; }
            public string FieldName { set; get; }
        }
        /// <summary>
        /// 分组选择字段
        /// </summary>
        public class GroupSelect
        {
            public GroupSelect(string source, string fieldName)
            {
                Source = source;
                FieldName = fieldName;
            }

            public GroupSelect(string source, string fieldName, GroupSelectOperator groupSelectOperator) : this(source, fieldName)
            {
                GroupSelectOperator = groupSelectOperator;
            }

            public string Source { set; get; }
            public string FieldName { set; get; }
            public GroupSelectOperator GroupSelectOperator { get; }
        }
        /// <summary>
        /// 关联连接集合
        /// </summary>
        public class Routers:List<Router>
        {
            
        }
        /// <summary>
        /// 关联连接
        /// </summary>
        public class Router
        {
            public Router(string from, string to, RouteType routeType, List<RouterOn> ons)
            {
                From = from;
                To = to;
                RouteType = routeType;
                Ons = ons;
            }

            public string From { set; get; }
            public string To { set; get; }
            public RouteType RouteType { set; get; }
            public List<RouterOn> Ons { set; get; }
        }

        /// <summary>
        /// 连接内容
        /// </summary>
        public class RouterOn
        {
            public RouterOn(string from, string fromField, string to, string toField)
            {
                From = from;
                FromField = fromField;
                To = to;
                ToField = toField;
            }

            public string From { set; get; }
            public string FromField { set; get; }
            public string To { set; get; }
            public string ToField { set; get; }
        }

        /// <summary>
        /// 连接类型
        /// </summary>
        public enum RouteType
        {
            None = 0,
            LeftJoin = 1,
            RightJoin = 2,
        }
        /// <summary>
        /// 调用的服务接口代理
        /// </summary>
        public class ReportHelper
        {
            internal static DataTable GetReport(ViewEntitySet viewEntities, ConditionSet mainConditions)
            {
                throw new NotImplementedException();
            }

            internal static DataTable GetReport(ReportEntity reportEntity)
            {
                throw new NotImplementedException();
            }

            internal static DataTable UnitData(string unitedBy, DataTable data1, DataTable data2)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 报告单元
        /// </summary>
        public class ReportEntity
        {
            public ReportEntity(string reportName)
            {
                Name = reportName;
            }

            public long Id { set; get; }
            public string Name { set; get; }
            public ViewEntitySet ViewEntitySet { get;  set; }
            public Routers Routers { get; internal set; }
        }
        /// <summary>
        /// 数据集单元集合
        /// </summary>
        public class ViewEntitySet: List<ViewEntity>
        {
        }
        /// <summary>
        /// 数据集单元
        /// </summary>
        public class ViewEntity
        {
            public ViewEntity(string viewEntityName, string displayName, List<ViewEntityProperty> properties)
            {
                ViewEntityName = viewEntityName;
                DisplayName = displayName;
                Properties = properties;
            }

            public string ViewEntityName { get; set; }
            public string DisplayName { get; set; }
            public List<ViewEntityProperty> Properties { get; set; }
        }
        /// <summary>
        /// 数据集单元的字段
        /// </summary>
        public class ViewEntityProperty
        {
            public ViewEntityProperty(string columnName, string displayName)
            {
                ColumnName = columnName;
                DisplayName = displayName;
            }

            public string ColumnName { get; set; }
            public string DisplayName { get; set; }
        }
        /// <summary>
        /// 条件集
        /// </summary>
        public class ConditionSet
        {
            public List<ICondition> Conditions { get; set; } = new List<ICondition>();
            public List<ConditionLinker> ConditionLinkers { get; set; } = new List<ConditionLinker>();
        }
        /// <summary>
        /// 条件连接类型
        /// </summary>
        public enum ConditionLinker
        {
            None = 0,
            And = 1,
            Or = 2,
        }

        public interface ICondition
        {
            
        }

        public class Field2FieldCondition : ICondition
        {
            public Field2FieldCondition(string entityName, string fieldName, ConditionOperator @operator, string entityName2Compare, string fieldName2Compare)
            {
                EntityName = entityName;
                FieldName = fieldName;
                Operator = @operator;
                EntityName2Compare = entityName2Compare;
                FieldName2Compare = fieldName2Compare;
            }

            public Field2FieldCondition(string entityName, string fieldName, ConditionOperator @operator, string entityName2Compare, string fieldName2Compare, string fieldFormat = null, string fieldName2CompareFormat = null)
            {
                EntityName = entityName;
                FieldName = fieldName;
                FieldFormat = fieldFormat;
                Operator = @operator;
                EntityName2Compare = entityName2Compare;
                FieldName2Compare = fieldName2Compare;
                FieldName2CompareFormat = fieldName2CompareFormat;
            }

            public string EntityName { get; set; }
            public string FieldName { get; set; }
            public string FieldFormat { get; set; }
            public ConditionOperator Operator { get; set; }
            public string EntityName2Compare { get; set; }
            public string FieldName2Compare { get; set; }
            public string FieldName2CompareFormat { get; set; }
        }

        /// <summary>
        /// 条件项
        /// </summary>
        public class Field2ValueCondition: ICondition
        {
            public Field2ValueCondition(string entityName, string fieldName, ConditionOperator @operator, string value = "", string valueFormat = "")
            {
                EntityName = entityName;
                FieldName = fieldName;
                Operator = @operator;
                Value = value;
                ValueFormat = valueFormat;
            }

            public string EntityName { get; set; }
            public string FieldName { get; set; }
            public ConditionOperator Operator { get; set; }
            public string Value{ get; set; }
            public string ValueFormat{ get; set; }

            public string GetFormatValue()
            {
                if (ValueFormat.IsNullOrEmpty())
                {
                    throw new NotImplementedException("无效的数据格式化类型");
                }
                return string.Format(ValueFormat, Value);
            }
        }

        /// <summary>
        /// 条件运算类型
        /// </summary>
        public enum ConditionOperator
        {
            None = 0,
            Equal = 1,
            Like = 2,
            IsNotNull = 3,
            IsNull = 4,
            GreatThan = 5,
            LessThan = 6,
        }
    }
}
