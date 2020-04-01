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

        internal long CreatePregnantInfo(PregnantInfo pregnant)
        {
            //对于实体创建和更新业务总是进行数据有效性校验
            var validResult = pregnant.Validate();
            if (!validResult.IsValidated)
                return -1;

            pregnant.Id = (int)ServiceContext.Repository_PregnantInfo.Insert(pregnant);
            return pregnant.Id;
        }

        internal bool UpdatePregnantInfo(PregnantInfo pregnant)
        {
            //对于实体创建和更新业务总是进行数据有效性校验
            var validResult = pregnant.Validate();
            if (!validResult.IsValidated)
                return false;

            var result = ServiceContext.Repository_PregnantInfo.Update(pregnant);
            return result;
        }
    }
}
