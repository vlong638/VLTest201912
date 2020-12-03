using Research.Common;
using System;

namespace Research
{
    public class BusinessContext
    {
        public static string Root = "PregnantInfo";
        public static BusinessEntities BusinessEntities { set; get; } = ConfigHelper.GetBusinessEntities();
        public static Routers Routers { set; get; } = ConfigHelper.GetRouters();
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
            var customBusinessEntity3 = new CustomBusinessEntity();
            var customBusinessEntity3Router = new Router();

            //维护到处理库
            BusinessContext.BusinessEntities.Add(customBusinessEntity3);
            BusinessContext.Routers.Add(customBusinessEntity3Router);

            //科研任务定义
            var reportTask3 = new ReportTask("母婴基础信息");
            reportTask3.Properties.Add(new BusinessEntityProperty("姓名", "PregnantInfo", "PersonName"));
            reportTask3.Properties.Add(new BusinessEntityProperty("生日", "PregnantInfo", "Birthday"));
            reportTask3.Properties.Add(new BusinessEntityProperty("新生儿姓名", "Child", "Name"));
            reportTask3.Properties.Add(new BusinessEntityProperty("新生儿生日", "Child", "Birthday"));
            reportTask3.Properties.Add(new BusinessEntityProperty("新生儿性别", "Child", "Sex"));
            reportTask3.MainConditions.Add(new Field2ValueWhere("PregnantInfo", "Birthday", WhereOperator.GreatThan, "1991-01-01"));
            var result3 = ResearchGenerator.GetReport(reportTask3);

            Console.WriteLine("Hello World!");
        }
    }
}

