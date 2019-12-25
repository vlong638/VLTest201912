namespace VLTest2015.DAL
{
    /// <summary>
    /// 基础的仓储服务
    /// </summary>
    public interface IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        long Insert(TEntity entity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update(TEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Delete(TEntity entity);

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetById(long id);

        /// <summary>
        /// 插入多条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(TEntity[] entitys);

        /// <summary>
        /// 根据Id删除实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteById(long id);
    }
}