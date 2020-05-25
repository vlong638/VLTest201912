using Dapper;
using System.Collections.Generic;
using System.Linq;
using VL.API.Common.Models;
using VL.API.Common.Models.Entities;
using VL.API.Common.Repositories;

namespace VL.API.PT.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class PregnantInfoRepository : DapperRepository<PregnantInfo>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbGroup"></param>
        public PregnantInfoRepository(DbGroup dbGroup) : base(dbGroup)
        {
        }

        internal List<PregnantInfo> GetPregnantInfoPagedList(GetPagedListSampleRequest request)
        {
            return dbGroup.Connection.Query<PregnantInfo>(request.ToListSQL(), request.GetParams()).ToList();
        }

        internal int GetPregnantInfoPagedListCount(GetPagedListSampleRequest request)
        {
            return dbGroup.Connection.ExecuteScalar<int>(request.ToCountSQL(), request.GetParams());
        }

        internal List<PregnantInfo> GetComplexPregnantInfoPagedList(GetComplexPagedListSampleRequest request)
        {
            return dbGroup.Connection.Query<PregnantInfo>(request.ToListSQL(), request.GetParams()).ToList();
        }

        internal int GetComplexPregnantInfoPagedListCount(GetComplexPagedListSampleRequest request)
        {
            return dbGroup.Connection.ExecuteScalar<int>(request.ToCountSQL(), request.GetParams());
        }
    }
}
