using System.Collections.Generic;

namespace Autobots.Common.ServiceBase
{
    /// <summary>
    /// 健康检测
    /// </summary>
    public interface IHealthCheck
    {
        /// <summary>
        /// 存活检测
        /// </summary>
        /// <returns></returns>
        bool IsAlive();

        /// <summary>
        /// 获取 依赖项检测报告
        /// </summary>
        /// <returns></returns>
        List<ReferenceCheckReport> GetReferenceCheckReports();

        /// <summary>
        /// 获取 负载检测报告
        /// </summary>
        /// <returns></returns>
        LoadingCheckReport GetLoadingCheckReport();
    }

    /// <summary>
    /// 依赖项检测报告
    /// </summary>
    public class ReferenceCheckReport
    { 
        public string Item { set; get; }
        public bool IsOK{ set; get; }
        public string Message { set; get; }
    }

    /// <summary>
    /// 负载检测报告
    /// </summary>
    public class LoadingCheckReport
    {
        public decimal CPU { set; get; }

        public decimal TotalMemory { set; get; }
        public decimal MemoryUsed { set; get; }

        public decimal TotalStorage { set; get; }
        public decimal StorageUsed { set; get; }

        public int OnlineUsers { set; get; }
    }
}
