using Research.Common;
using System;

namespace Research
{
    public class BusinessContext
    {
        public static BusinessEntities BusinessEntities { set; get; } = ConfigHelper.GetBusinessEntities();
        public static Routers Routers { set; get; } = ConfigHelper.GetRouters();
    }

    class Program
    {
        static void Main(string[] args)
        {
            //单表导出
            var reportTask1 = new ReportTask("母亲基础信息");
            reportTask1.Properties.Add(new BusinessEntityProperty("姓名", "PregnantInfo", "Name"));
            reportTask1.Properties.Add(new BusinessEntityProperty("生日", "PregnantInfo", "Birthday"));
            reportTask1.MainConditions.Add(new Field2ValueWhere("PregnantInfo", "Birthday", WhereOperator.GreatThan, "1991-01-01"));
            var result1 = ResearchGenerator.GetReport(reportTask1);

            //基础多表关联
            var reportTask2 = new ReportTask("母婴基础信息");
            reportTask2.Properties.Add(new BusinessEntityProperty("姓名", "PregnantInfo", "Name"));
            reportTask2.Properties.Add(new BusinessEntityProperty("生日", "PregnantInfo", "Birthday"));
            reportTask2.Properties.Add(new BusinessEntityProperty("新生儿姓名", "Child", "Name"));
            reportTask2.Properties.Add(new BusinessEntityProperty("新生儿生日", "Child", "Birthday"));
            reportTask2.Properties.Add(new BusinessEntityProperty("新生儿性别", "Child", "Sex"));
            reportTask2.MainConditions.Add(new Field2ValueWhere("PregnantInfo", "Birthday", WhereOperator.GreatThan, "1991-01-01"));
            var result2 = ResearchGenerator.GetReport(reportTask1);

            //定义自定义指标
            var customBusinessEntity3 = new CustomBusinessEntity();
            var customBusinessEntity3Router = new Router();

            //维护到处理库
            BusinessContext.BusinessEntities.Add(customBusinessEntity3);
            BusinessContext.Routers.Add(customBusinessEntity3Router);

            //科研任务定义
            var reportTask3 = new ReportTask("母婴基础信息");
            reportTask3.Properties.Add(new BusinessEntityProperty("姓名", "PregnantInfo", "Name"));
            reportTask3.Properties.Add(new BusinessEntityProperty("生日", "PregnantInfo", "Birthday"));
            reportTask3.Properties.Add(new BusinessEntityProperty("新生儿姓名", "Child", "Name"));
            reportTask3.Properties.Add(new BusinessEntityProperty("新生儿生日", "Child", "Birthday"));
            reportTask3.Properties.Add(new BusinessEntityProperty("新生儿性别", "Child", "Sex"));
            reportTask3.MainConditions.Add(new Field2ValueWhere("PregnantInfo", "Birthday", WhereOperator.GreatThan, "1991-01-01"));
            var result3 = ResearchGenerator.GetReport(reportTask1);





            Console.WriteLine("Hello World!");
        }
    }
}
