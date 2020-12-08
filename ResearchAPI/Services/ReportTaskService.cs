using Autobots.Infrastracture.Common.ServiceSolution;
using ResearchAPI.Common;
using ResearchAPI.Controllers;
using System;

namespace ResearchAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ReportTaskService
    {
        internal ServiceResult<bool> ExecuteReportTask(long taskId)
        {
            var reportTask = ReportTaskDomain.GetReportTask(taskId);
            var reportProject = ReportTaskDomain.GetReportProject(reportTask.ProjectId);




            throw new NotImplementedException();
        }

        internal ServiceResult<long> CreateCustomBusinessEntity(CreateCustomBusinessEntityRequest request)
        {
            var id= ReportTaskDomain.CreateCustomBusinessEntity(request);
            return new ServiceResult<long>(id);
        }

        internal ServiceResult<bool> EditCustomBusinessEntity(EditCustomBusinessEntityRequest request)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ReportTaskDomain
    {
        internal static long CreateCustomBusinessEntity(CreateCustomBusinessEntityRequest request)
        {
            throw new NotImplementedException();
        }

        internal static ReportTask GetReportTask(long taskId)
        {
            var reportTask = new ReportTask("母亲检验信息");
            reportTask.Properties.Add(new BusinessEntityProperty("姓名", "PregnantInfo", "PersonName"));
            reportTask.Properties.Add(new BusinessEntityProperty("生日", "PregnantInfo", "Birthday"));
            //reportTask.Properties.Add(new BusinessEntityProperty("空腹血糖-检查名称", customBusinessEntity.ReportName, "ExamName"));
            //reportTask.Properties.Add(new BusinessEntityProperty("空腹血糖-检查日期", customBusinessEntity.ReportName, "ExamTime"));
            //reportTask.Properties.Add(new BusinessEntityProperty("空腹血糖-检验名称", customBusinessEntity.ReportName, "ItemName"));
            //reportTask.Properties.Add(new BusinessEntityProperty("空腹血糖-检验结果", customBusinessEntity.ReportName, "Value"));
            //reportTask.MainConditions.Add(new Field2ValueWhere(customBusinessEntity.ReportName, "检验类别", WhereOperator.Equal, "0148"));
            //reportTask.MainConditions.Add(new Field2ValueWhere(customBusinessEntity.ReportName, "Value", WhereOperator.GreatOrEqualThan, "10"));
            //reportTask.CustomBusinessEntities.Add(customBusinessEntity);
            //reportTask.CustomRouters.Add(customBusinessEntityRouter);
            return reportTask;
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      