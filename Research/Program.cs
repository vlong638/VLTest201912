using Research.Common;
using System;
using System.Collections.Generic;

namespace Research
{
    public class BusinessContext
    {
        public static string Root = "PregnantInfo";
        public static BusinessEntities BusinessEntities { set; get; } = ConfigHelper.GetBusinessEntities("XMLConfigs\\VLTest项目", "BusinessEntities.xml");
        public static Routers Routers { set; get; } = ConfigHelper.GetRouters("XMLConfigs\\VLTest项目", "Routers.xml");
        public static List<BusinessEntityTemplate> Templates { set; get; } = new List<BusinessEntityTemplate>();
    }

    class Program
    {
        static void Main(string[] args)
        {
            //单表导出
            if (false)
            {
                var reportTask = new ReportTask("母亲基础信息");
                reportTask.Properties.Add(new BusinessEntityProperty("姓名", "PregnantInfo", "PersonName"));
                reportTask.Properties.Add(new BusinessEntityProperty("生日", "PregnantInfo", "Birthday"));
                reportTask.MainConditions.Add(new Field2ValueWhere("PregnantInfo", "Birthday", WhereOperator.GreatThan, "1991-01-01"));
                var result = ResearchGenerator.GetReport(reportTask); 
            }

            //相邻多表关联
            if (false)
            {
                var reportTask = new ReportTask("母亲检查信息");
                reportTask.Properties.Add(new BusinessEntityProperty("姓名", "PregnantInfo", "PersonName"));
                reportTask.Properties.Add(new BusinessEntityProperty("生日", "PregnantInfo", "Birthday"));
                reportTask.Properties.Add(new BusinessEntityProperty("检查名称", "LabOrder", "ExamName"));
                reportTask.Properties.Add(new BusinessEntityProperty("检查日期", "LabOrder", "ExamTime"));
                reportTask.MainConditions.Add(new Field2ValueWhere("LabOrder", "ExamTime", WhereOperator.GreatThan, "2020-01-01"));
                var result = ResearchGenerator.GetReport(reportTask); 
            }

            //跨表多表关联
            if (false)
            {
                var reportTask = new ReportTask("母亲检验信息");
                reportTask.Properties.Add(new BusinessEntityProperty("姓名", "PregnantInfo", "PersonName"));
                reportTask.Properties.Add(new BusinessEntityProperty("生日", "PregnantInfo", "Birthday"));
                reportTask.Properties.Add(new BusinessEntityProperty("检验类型", "LabResult", "ItemId"));
                reportTask.Properties.Add(new BusinessEntityProperty("检验名称", "LabResult", "ItemName"));
                reportTask.Properties.Add(new BusinessEntityProperty("检验结果", "LabResult", "Value"));
                reportTask.MainConditions.Add(new Field2ValueWhere("LabResult", "ItemId", WhereOperator.Equal, "9126"));
                var result = ResearchGenerator.GetReport(reportTask);
            }

            //定义自定义指标
            if (true)
            {
                //1.模板定义
                //2.用户配置模板
                //3.用户定义任务
                //4.任务执行

                //1.模板定义
                //XML
                var routeTemplate = ConfigHelper.GetBusinessEntityTemplate("XMLConfigs\\VLTest项目", "Template_检验.xml");
                BusinessContext.Templates.Add(routeTemplate);

                //2.用户配置模板
                var customBusinessEntity = ConfigHelper.GetCustomBusinessEntity("XMLConfigs\\VLTest项目", "CustomBusinessEntity_检验.xml");

                //var reportName = "孕期检验结果-空腹血糖";
                //var customBusinessEntity = new BusinessEntity()
                //{ 
                //    DisplayName = reportName,         //页面上的命名
                //    Template = "检验结果分类周期模板", //用什么模板
                //    Properties =new List<BusinessEntityProperty>()
                //    {
                //        new BusinessEntityProperty("空腹血糖-值",reportName,"Value"),
                //        new BusinessEntityProperty("空腹血糖-检查名称",reportName,"ExamName"),
                //        new BusinessEntityProperty("空腹血糖-检查日期",reportName,"ExamTime"),
                //        new BusinessEntityProperty("空腹血糖-检验名称",reportName,"ItemName"),
                //        new BusinessEntityProperty("空腹血糖-检验结果",reportName,"Value"),
                //    },
                //    SQLConfig = routeTemplate.SQLConfig,
                //};

                var customBusinessEntityRouter = new Router();
                customBusinessEntityRouter.From = "PregnantInfo";
                customBusinessEntityRouter.FromAlias = "pi";
                customBusinessEntityRouter.To = customBusinessEntity.ReportName;
                customBusinessEntityRouter.RouteType = RouteType.LeftJoin;
                customBusinessEntityRouter.IsFromTemplate = true;
                customBusinessEntityRouter.Ons.Add(new RouterOn("Idcard", "Idcard"));

                ////追加到路由和指标库
                //BusinessContext.CustomBusinessEntity = customBusinessEntity;
                //BusinessContext.CustomRouters.Add(customBusinessEntityRouter);

                //3.用户定义任务
                var reportTask = new ReportTask("母亲检验信息");
                reportTask.Properties.Add(new BusinessEntityProperty("姓名", "PregnantInfo", "PersonName"));
                reportTask.Properties.Add(new BusinessEntityProperty("生日", "PregnantInfo", "Birthday"));
                reportTask.Properties.Add(new BusinessEntityProperty("空腹血糖-检查名称", customBusinessEntity.ReportName, "ExamName"));
                reportTask.Properties.Add(new BusinessEntityProperty("空腹血糖-检查日期", customBusinessEntity.ReportName, "ExamTime"));
                reportTask.Properties.Add(new BusinessEntityProperty("空腹血糖-检验名称", customBusinessEntity.ReportName, "ItemName"));
                reportTask.Properties.Add(new BusinessEntityProperty("空腹血糖-检验结果", customBusinessEntity.ReportName, "Value"));
                reportTask.MainConditions.Add(new Field2ValueWhere(customBusinessEntity.ReportName, "检验类别", WhereOperator.Equal, "0148"));
                reportTask.MainConditions.Add(new Field2ValueWhere(customBusinessEntity.ReportName, "Value", WhereOperator.GreatOrEqualThan, "10"));
                reportTask.CustomBusinessEntities.Add(customBusinessEntity);
                reportTask.CustomRouters.Add(customBusinessEntityRouter);

                //4.任务执行
                 var result = ResearchGenerator.GetReport(reportTask);
            }

            Console.WriteLine("Hello World!");
        }
    }
}

