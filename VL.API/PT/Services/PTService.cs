using System;
using VL.API.Common.Services;
using VL.API.PT.Entities;

namespace VL.API.PT.Services
{
    public class PTService: ServiceBase
    {
        /// <summary>
        /// 单数据库查询样例
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PregnantInfo GetPregnantInfoById(int id)
        {
            return ServiceContext.Repository_PregnantInfo.GetById(id);
        }

        /// <summary>
        /// 单数据库新增样例
        /// </summary>
        /// <param name="pregnant"></param>
        /// <returns></returns>
        internal ServiceResult<int> CreatePregnantInfo(PregnantInfo pregnant)
        {
            //对于实体创建和更新业务总是进行数据有效性校验
            var validResult = pregnant.Validate();
            if (!validResult.IsValidated)
                return Error(-1);

            var result = DelegateTransaction(ServiceContext.DbGroup_Pregnant, () =>
             {
                 pregnant.Id = (int)ServiceContext.Repository_PregnantInfo.Insert(pregnant);
                 return pregnant.Id;
             });
            return result;
        }

        /// <summary>
        /// 单数据库更新样例
        /// </summary>
        /// <param name="pregnant"></param>
        /// <returns></returns>
        internal ServiceResult<bool> UpdatePregnantInfo(PregnantInfo pregnant)
        {
            //对于实体创建和更新业务总是进行数据有效性校验
            var validResult = pregnant.Validate();
            if (!validResult.IsValidated)
                return Error(false);

            var result = DelegateTransaction(ServiceContext.DbGroup_Pregnant, () =>
            {
                var result = ServiceContext.Repository_PregnantInfo.Update(pregnant);
                return result;
            });
            return result;
        }

        /// <summary>
        /// 多数据库事务样例
        /// </summary>
        /// <param name="pregnant"></param>
        /// <returns></returns>
        internal ServiceResult<bool> MultipleSourceSample(PregnantInfo pregnant)
        {
            //对于实体创建和更新业务总是进行数据有效性校验
            var validResult = pregnant.Validate();
            if (!validResult.IsValidated)
                return Error(false);

            var result = DelegateTransaction(ServiceContext.DbGroup_Pregnant, ServiceContext.DbGroup_Sample01, () =>
            {
                var result = ServiceContext.Repository_PregnantInfo.Update(pregnant);
                return result;
            });
            return result;
        }
    }
}
