using FrameworkTest.Common.HttpSolution;
using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace FrameworkTest.Business.SDMockCommit
{
    public interface SourceDataForESB
    {
        string inp_no { get; }
        string SourceId { get; }
        TargetType TargetType { get; }
    }

    public interface SourceDataForPregnant
    {
        string IdCard { get; }
        string SourceId { get; }
        TargetType TargetType { get; }
    }

    public abstract class SyncTask<T1> 
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

        public abstract void DoWork(ServiceContext context, UserInfo userInfo, T1 sourceData, ref StringBuilder sb);

        public virtual void Start_Auto_DoWork(ServiceContext context, UserInfo userInfo, int interval = 1000 * 10)
        {
            while (true)
            {
                var sourceDatas = GetSourceDatas(userInfo);
                foreach (var sourceData in sourceDatas)
                {
                    StringBuilder sb = new StringBuilder();
                    DoLogOnGetSource?.Invoke(sourceData);
                    DoWork(context, userInfo, sourceData, ref sb);
                    DoLogOnWork?.Invoke(sourceData, sb);
                }
                System.Threading.Thread.Sleep(interval);
            }
        }
    }
}
