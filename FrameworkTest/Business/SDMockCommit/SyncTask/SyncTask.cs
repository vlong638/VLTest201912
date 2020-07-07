using FrameworkTest.Common.HttpSolution;
using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace FrameworkTest.Business.SDMockCommit
{
    public interface SourceData
    {
        string IdCard { get; }
        string SourceId { get; }
        SourceType SourceType { get; }
    }

    public abstract class SyncTask<T1> where T1 : SourceData
    {
        protected SyncTask(ServiceContext context)
        {
            Context = context;
        }

        public ServiceContext Context { set; get; }

        public Action<T1> DoLogOnGetSource { set; get; }
        public Action<T1, StringBuilder> DoLogOnWork { set; get; }
        /// <summary>
        /// 获取待新建的来源数据
        /// </summary>
        /// <returns></returns>
        public abstract List<T1> GetSourceDatas(UserInfo userInfo);

        public abstract void DoWork(ServiceContext context, UserInfo userInfo, T1 sourceData);

        public virtual void Start_Auto_DoWork(ServiceContext context, UserInfo userInfo, int interval = 1000 * 10)
        {
            while (true)
            {
                var sourceDatas = GetSourceDatas(userInfo);
                foreach (var sourceData in sourceDatas)
                {
                    DoLogOnGetSource?.Invoke(sourceData);
                    DoWork(context, userInfo, sourceData);
                }
                System.Threading.Thread.Sleep(interval);
            }
        }
    }
}
