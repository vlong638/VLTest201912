using System.Data;

namespace ConsoleTest0213.SemiAutoExport
{
    /// <summary>
    /// 配置文件 服务
    /// </summary>
    public interface IAllService
    {
        #region Read

        /// <summary>
        /// 获取(检索信息)
        /// </summary>
        /// <param name="configEntityId"></param>
        /// <returns></returns>
        ConfigEntity GetMain(long configEntityId);

        /// <summary>
        /// 获取(输出设置)
        /// </summary>
        /// <param name="configEntityId"></param>
        /// <returns></returns>
        ConfigEntity GetOutputs(long configEntityId);

        /// <summary>
        /// 获取(条件设置_主体)
        /// </summary>
        /// <param name="configEntityId"></param>
        /// <returns></returns>
        ConfigEntity GetConditionMain(long configEntityId);

        /// <summary>
        /// 获取(条件设置_内容)
        /// </summary>
        /// <param name="configEntityId"></param>
        /// <returns></returns>
        ConfigEntity GetConditionDetail(long configEntityId);

        /// <summary>
        /// 获取(完整的配置文件)
        /// </summary>
        /// <param name="configEntityId"></param>
        /// <returns></returns>
        ConfigEntity GetConfigEntity(long configEntityId);

        /// <summary>
        /// 获取页面数据
        /// </summary>
        /// <param name="configEntityId"></param>
        /// <returns></returns>
        DataTable GetData(long configEntityId);

        /// <summary>
        /// 获取Excel
        /// </summary>
        /// <param name="configEntityId"></param>
        /// <returns></returns>
        byte[] GetExcel(long configEntityId);

        #endregion

        #region Write
        /// <summary>
        /// 保存(检索信息)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        long SaveMain(ConfigEntity entity);

        /// <summary>
        /// 修改(检索信息)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool UpdateMain(ConfigEntity entity);

        /// <summary>
        /// 保存(输出设置)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool SaveOutputs(ConfigEntity entity);

        /// <summary>
        /// 修改(输出设置)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool UpdateOutputs(ConfigEntity entity);

        /// <summary>
        /// 保存(条件设置_主体)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool SaveConditionMain(ConfigEntity entity);

        /// <summary>
        /// 修改(条件设置_主体)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool UpdateConditionMain(ConfigEntity entity);

        /// <summary>
        /// 保存(条件设置_内容)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool SaveConditionDetail(ConfigEntity entity);

        /// <summary>
        /// 修改(条件设置_内容)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool UpdateConditionDetail(ConfigEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Delete(long configEntityId);

        #endregion
    }
}
