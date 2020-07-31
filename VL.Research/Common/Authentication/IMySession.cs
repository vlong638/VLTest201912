namespace VLTest2015.Common
{
    /// <summary>
    /// 会话层
    /// 可在CurrentUser介于B/S两端交互过程构建缓存存储的一层
    /// </summary>
    public interface IMySession
    {
        #region （后端存储）

        /// <summary>
        /// 设置项
        /// 清空项时，将value设为null即可
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set<T>(string key, T value);

        /// <summary>
        /// 获取项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        #endregion

        #region （前端交互）

        /// <summary>
        /// 获取会话id
        /// </summary>
        /// <returns></returns>
        string GetSessionId();

        /// <summary>
        /// 设置会话id
        /// </summary>
        /// <param name="sessinId"></param>
        void SetSessionId(string sessinId);

        /// <summary>
        /// 用户退出
        /// </summary>
        /// <param name="sessionId"></param>
        void Clear();
        #endregion
    }
}