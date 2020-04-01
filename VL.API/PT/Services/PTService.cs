using System;
using VL.API.Common.Services;
using VL.API.PT.Entities;

namespace VL.API.PT.Services
{
    public class PTService: ServiceBase
    {
        public PregnantInfo GetPregnantInfoById(int id)
        {
            return ServiceContext.Repository_PregnantInfo.GetById(id);
        }

        internal ServiceResult<int> CreatePregnantInfo(PregnantInfo pregnant)
        {
            //对于实体创建和更新业务总是进行数据有效性校验
            var validResult = pregnant.Validate();
            if (!validResult.IsValidated)
                return Error(-1);

            pregnant.Id = (int)ServiceContext.Repository_PregnantInfo.Insert(pregnant);
            return Success(pregnant.Id);
        }

        internal ServiceResult<bool> UpdatePregnantInfo(PregnantInfo pregnant)
        {
            //对于实体创建和更新业务总是进行数据有效性校验
            var validResult = pregnant.Validate();
            if (!validResult.IsValidated)
                return Error(false);

            var result = ServiceContext.Repository_PregnantInfo.Update(pregnant);
            return Success(result);
        }
    }
}
