using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using VL.Consolo_Core.Common.ConfigSolution;
using VL.Consolo_Core.Common.ControllerSolution;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.RepositorySolution;
using VL.Consolo_Core.Common.ServiceSolution;

namespace Research.Common
{
    public class SQLBuilder
    {
        internal static string Build(List<BusinessEntityProperty> properties, List<Router> routers, List<IWhere> mainConditions)
        {
            throw new NotImplementedException();
        }
    }

    public class SharedService
    {
        public ServiceResult<DataTable> GetReport(string sql,Dictionary<string, object> parameters)
        {
            var vlConfig = VLConfigHelper.GetVLConfig("configs", "config.xml");
            var connectionString = vlConfig.GetByKey("ConnectionStrings", "HL_Pregnant");
            var context =  new DbContext(DBHelper.GetDbConnection(connectionString));
            var repository = new SharedRepository(context);
            var result =  repository.ExecuteReader(sql, parameters);
            return new ServiceResult<DataTable>(result);
        }
    }

    public class SharedRepository : Repository<object>
    {
        public SharedRepository(DbContext context) : base(context)
        {
        }

        public DataTable ExecuteReader(string sql, Dictionary<string, object> parameters)
        {
            DataTable table = new DataTable("MyTable");
            var reader = context.DbGroup.Connection.ExecuteReader(sql, parameters, transaction: _transaction);
            table.Load(reader);
            return table;
        }
    }

    /// <summary>
    /// 调用的服务接口代理
    /// </summary>
    public class ResearchGenerator
    {
        internal static APIResult<DataTable> GetReport(ReportTask reportEntity)
        {
            var parameters = reportEntity.GetParameters();
            var sql = reportEntity.GetSQL(parameters);
            var serviceResult = new SharedService().GetReport(sql, parameters);
            if (serviceResult.IsSuccess)
            {
                return new APIResult<DataTable>(serviceResult.Data);
            }
            else
            {
                return new APIResult<DataTable>(null, serviceResult.Message);
            }
        }
    }
}