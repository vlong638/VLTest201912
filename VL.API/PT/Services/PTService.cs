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
            ///VLCore:关键原则
            ///如果有数据规范性的校验,比如一个创建业务内需要符合的逻辑,应由Factory进行统一
            ///如 PregnantInfo.Create(){ DoSets();PregnantInfo.Validate();}

            pregnant.Id = (int)ServiceContext.Repository_PregnantInfo.Insert(pregnant);
            return pregnant.Id;
        }

        internal bool UpdatePregnantInfo(PregnantInfo pregnant)
        {
            //VLCore:关键原则
            ///如果有数据规范性的校验,比如一个变更业务内需要符合的逻辑,应由Factory进行统一
            ///如 PregnantInfo.Update(){ DoSets();PregnantInfo.Validate();}

            var result = ServiceContext.Repository_PregnantInfo.Update(pregnant);
            return result;
        }
    }
}
